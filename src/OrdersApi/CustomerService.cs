namespace OrdersApi
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    internal class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(new Uri($"/api/customers/{id}"), cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = response.Content;
            var jsonResponse = await content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<CustomerDto>(jsonResponse);
        }
    }
}