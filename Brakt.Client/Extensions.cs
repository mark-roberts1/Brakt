using RestSharp;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt.Client
{
    internal static class Extensions
    {
        internal static IRestResponse ThrowIfError(this IRestResponse response)
        {
            if ((int)response.StatusCode >= 400)
            {
                var ex = JsonSerializer.Deserialize<ApiError>(response.Content, ApiConfiguration.SerializerOptions);

                throw new Exception(ex.Message);
            }

            return response;
        }
    }
}
