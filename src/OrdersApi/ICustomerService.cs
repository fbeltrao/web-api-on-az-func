namespace OrdersApi
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationToken);
    }
}