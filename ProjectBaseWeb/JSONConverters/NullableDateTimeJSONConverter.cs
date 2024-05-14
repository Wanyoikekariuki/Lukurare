using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBaseWeb.JSONConverters
{
    public class NullableDateTimeJSONConverter : JsonConverter<DateTime?>
    {
        //https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Converters/IsoDateTimeConverter.cs
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
        private DateTimeZoneManger DateTimeZoneService { get; }

        public NullableDateTimeJSONConverter() { }

        public NullableDateTimeJSONConverter(IServiceCollection serviceCollection)
            : this()
        {
            var provider = serviceCollection.BuildServiceProvider();
            DateTimeZoneService = ActivatorUtilities.CreateInstance<DateTimeZoneManger>(provider);
        }

        public override void WriteJson(
            JsonWriter writer,
            DateTime? value1,
            JsonSerializer serializer
        )
        {
            var hoursDiffrenceFromUTC = DateTimeZoneService.GetHoursFromTimeZone();
            if (value1.HasValue)
            {
                var value = value1.Value;
                var newValue = value.AddHours(hoursDiffrenceFromUTC);
                var isoDateFormat = $"{newValue:yyyy-MM-ddTHH:mm:ss.FFFFFFFK}";
                writer.WriteValue(isoDateFormat);
            }
            else
                writer.WriteValue("null");
        }

        public override DateTime? ReadJson(
            JsonReader reader,
            Type objectType,
            DateTime? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            string rawDateValue = (string)reader.Value;
            if (string.IsNullOrEmpty(rawDateValue) || rawDateValue != "null")
            {
                //2021-11-08T16:35:00.000Z
                var stringNew = rawDateValue.Substring(0, 19);
                DateTime parsedDate = DateTime.Parse(rawDateValue);
                if (
                    !DateTime.TryParseExact(
                        stringNew,
                        "yyyy-MM-ddTHH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out parsedDate
                    )
                )
                    parsedDate = DateTime.Parse(rawDateValue);

                if (rawDateValue.Contains("Z") == false)
                {
                    var hoursDiffrenceFromUTC = DateTimeZoneService.GetHoursFromTimeZone() * -1;
                    parsedDate = parsedDate.AddHours(hoursDiffrenceFromUTC);
                }
                return parsedDate;
            }
            DateTime? dateTimeNull = null;
            return dateTimeNull;
        }
    }
}
