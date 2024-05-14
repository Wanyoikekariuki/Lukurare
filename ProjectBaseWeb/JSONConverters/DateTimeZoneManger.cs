using Microsoft.AspNetCore.Http;
using ProjectBaseWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBaseWeb.JSONConverters
{
    public class DateTimeZoneManger : IDateTimeZoneManger
    {
        private readonly IHttpContextAccessor context;

        public DateTimeZoneManger(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor;
        }

        public double GetHoursFromTimeZone()
        {
            bool isAdd = false;
            var timeZoneInfo = ZoneInfo();
            if (timeZoneInfo.Contains("+"))
                isAdd = true;
            timeZoneInfo = timeZoneInfo.Replace("+", "");
            var timeZoneSplit = timeZoneInfo.Split(":");
            int hours = 0,
                minutes = 0;
            int.TryParse(timeZoneSplit[0], out hours);
            int.TryParse(timeZoneSplit[1], out minutes);
            var minuteHours = minutes / 60.0; //get the minutes in the hours
            var hoursAndMinutes = hours + minuteHours;
            return isAdd ? hoursAndMinutes : -hoursAndMinutes;
        }

        string ZoneInfo()
        {
            var timeZoneOffset = string.Empty;
            try
            {
                if (
                    context.HttpContext.Request.Headers.ContainsKey(
                        TimeZoneManagerService.DajJSTimeZoneInfo
                    )
                )
                {
                    timeZoneOffset = context.HttpContext.Request.Headers[
                        TimeZoneManagerService.DajJSTimeZoneInfo
                    ];
                    return timeZoneOffset;
                }
            }
            catch (Exception) { }
            return timeZoneOffset;
        }
    }
}
