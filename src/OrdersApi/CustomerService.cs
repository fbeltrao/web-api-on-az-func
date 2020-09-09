namespace OrdersApi
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/customers/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = response.Content;
            var jsonResponse = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CustomerDto>(jsonResponse);
        }
    }
}