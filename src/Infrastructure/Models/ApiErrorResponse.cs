namespace Infrastructure
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Contract for api errors.
    /// </summary>
    public class ApiErrorResponse : IApiResponse
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the error messages.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Errors { get; set; }
    }
}