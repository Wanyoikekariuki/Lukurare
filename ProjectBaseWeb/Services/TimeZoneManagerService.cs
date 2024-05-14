using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBaseWeb.Services
{
    public static class TimeZoneManagerService
    {
        public static string DajJSTimeZoneInfo = "X-DayJS-TimeZone";

        static object lockGetSetTimeZoneInfo = new object();

        public static string _timeZoneInformation;
        public static string TimeZoneInformation
        {
            get
            {
                lock (lockGetSetTimeZoneInfo)
                {
                    return _timeZoneInformation;
                }
            }
            private set
            {
                lock (lockGetSetTimeZoneInfo)
                {
                    _timeZoneInformation = value;
                }
            }
        }

        public async static Task Process(HttpContext context, Func<Task> next)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (context.Request.Headers.ContainsKey(DajJSTimeZoneInfo))
                    {
                        var timeZoneOffset = context.Request.Headers[DajJSTimeZoneInfo];
                        TimeZoneInformation = timeZoneOffset;
                    }
                }
                catch (Exception) { }
            });
        }
    }
}
