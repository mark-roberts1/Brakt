using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Brakt.Client
{
    public class ApiConfiguration
    {
        internal static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public string BaseUrl { get; set; }
    }
}
