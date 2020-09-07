using System.Threading;
using System.Threading.Tasks;

namespace OrdersApi
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomerAsync(int id, CancellationToken cancellationToken);
    }
}