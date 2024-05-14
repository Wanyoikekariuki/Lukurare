using RestSharp;
using System.Net;

namespace CommunicationLibrary.Security.Credentials
{
    public interface IAuthenticable
    {
        Parameter Authenticate();
        Parameter AuthorizationHeader { get; }
        bool Authenticated { get; }
        NetworkCredential Credentials { get; }
    }
}
