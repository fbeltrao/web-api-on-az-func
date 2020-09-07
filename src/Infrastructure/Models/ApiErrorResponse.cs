using Newtonsoft.Json;
using System.Collections.Generic;

namespace Infrastructure
{

    public class ApiErrorResponse : IApiResponse
    {
        public string ErrorCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Errors { get; set; }

    }
}