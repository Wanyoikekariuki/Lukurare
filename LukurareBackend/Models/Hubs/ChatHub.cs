using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class ChatHub : Hub
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static int currentlyLoggedInUserId;

    //Key: UserId, Value: connectionId(From SignalR)
    private static Dictionary<int, string> userConnectionMap = new Dictionary<int, string>();

    public ChatHub(IHttpContextAccessor _httpContextAccessor) => this._httpContextAccessor = _httpContextAccessor;

        // Add a user to the connected users collection when they connect
        public override async Task OnConnectedAsync()
        {

        var isAuth = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        //Check if the User is authenticated
        if (isAuth)
        {
            //Users are returned as a ClaimsIdentity list, FirstorDefault gets the first one; In this case the only one
            var user = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault();

            if (user != null)
            {
                //Get the claims associated with that user.
                var UserClaimCollection = user.Claims;

                //Id claim is initially returned as a string. Conversion done after initialization
                string id = "";

                foreach (var idnum in UserClaimCollection)
                {
                    id = idnum.Value;
                }

                //Id's data type is parsed to int
                currentlyLoggedInUserId = int.Parse(id);

                string connectionId = Context.ConnectionId;

                // Associate the user's connection ID with their username
                if (!userConnectionMap.ContainsKey(currentlyLoggedInUserId))
                {
                    userConnectionMap.Add(currentlyLoggedInUserId, connectionId);
                }
                else
                {
                    // Handle the case where a user might have multiple connections 
                    userConnectionMap[currentlyLoggedInUserId] = connectionId;
                }

            }

            await base.OnConnectedAsync();

        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var username = userConnectionMap.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

        //We check if username is not equal to the default value of an int, which is 0.
        //If the key is found, it will be a valid integer, and we proceed to remove it from the dictionary.
        if (username != default(int))
        {
            userConnectionMap.Remove(username);
            currentlyLoggedInUserId = 0;
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task CheckOnlineStatus(int receipientsId)
    {

        #region getUserIdFromHttpContextAccessor_Initialize_currentlyLoggedInUserId
        //Users are returned as a ClaimsIdentity list, FirstorDefault gets the first one; In this case the only one
        var user = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault();

        if (user != null)
        {
            //Get the claims associated with that user.
            var UserClaimCollection = user.Claims;

            //Id claim is initially returned as a string. Conversion done after initialization
            string id = "";

            foreach (var idnum in UserClaimCollection)
            {
                id = idnum.Value;
            }

            //Id's data type is parsed to int
            currentlyLoggedInUserId = int.Parse(id);

        }
        #endregion
        if (userConnectionMap.TryGetValue(receipientsId, out string connectionId))
        {
            await Clients.Client(userConnectionMap[currentlyLoggedInUserId]).SendAsync("UserOnline", receipientsId);
        }
        else
        {
            await Clients.Client(userConnectionMap[currentlyLoggedInUserId]).SendAsync("UserOffline", "User is not online. Consider sending them an Email instead", receipientsId);
        }
    }


    public async Task SendMessageToUser(int receipientsId, string message)
    {

        await CheckOnlineStatus(receipientsId);


        #region getUserIdFromHttpContextAccessor_Initialize_currentlyLoggedInUserId
        //Users are returned as a ClaimsIdentity list, FirstorDefault gets the first one; In this case the only one
        var user = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault();

        if (user != null)
        {
            //Get the claims associated with that user.
            var UserClaimCollection = user.Claims;

            //Id claim is initially returned as a string. Conversion done after initialization
            string id = "";

            foreach (var idnum in UserClaimCollection)
            {
                id = idnum.Value;
            }

            //Id's data type is parsed to int
            currentlyLoggedInUserId = int.Parse(id);

        }
        #endregion

        // Get the sender's connection ID from the currently authenticated user
        string senderConnectionId = userConnectionMap[currentlyLoggedInUserId]; 
            
        // Check if the sender and the recipient's ID is the same (trying to send a message to self)
        if (currentlyLoggedInUserId.ToString() == receipientsId.ToString())
        {
            await Clients.Client(senderConnectionId).SendAsync("ReceiveErrorMessage", "One cannot send message to self");
            return;
        }
        //Get the valid receipients according to the currentlyLoggedInUser and validate.

        //Check if the sender and the receiver's ID is the same. i.e trying to send message to self
        //if (currentlyLoggedInUserId == receipientsId)
        //    {
        //        return;
        //    }

        //Is a valid recipient, Not sending message to self so find the receipient ConnectionId and send message to receipient.
        if (userConnectionMap.TryGetValue(receipientsId, out string connectionId))
        {
            //await Clients.Client(connectionId).SendAsync("checkOnlineStatus", currentlyLoggedInUserId);
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", currentlyLoggedInUserId, message);
        }
        //else
        //{
        //    // Handle the case where the user is not connected.

        //   // await Clients.Client(userConnectionMap[currentlyLoggedInUserId]).SendAsync("UserOffline", message);
        //}
    }

    //Broadcast message to All Clients.
    public Task SendMessageBroadcast(string user, string message)
    {
        return Clients.All.SendAsync("BroadcastMessage", user, message);
    }

}