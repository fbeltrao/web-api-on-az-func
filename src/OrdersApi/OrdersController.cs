namespace OrdersApi
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Infrastructure;
    using System.Threading;

    public class OrdersController : FunctionControllerBase
    {
        private readonly ICustomerService _customerService;

        public OrdersController(ILogger<OrdersController> logger, ICustomerService customerService) : base(logger)
        {
            _customerService = customerService;
        }

        [FunctionName(nameof(CreateOrder))]
        public async Task<IActionResult> CreateOrder(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] CreateOrderDto request,
            CancellationToken cancellationToken)
        {
            async Task<IActionResult> DoCreateOrder()
            {
                var validationResult = Validate(request);
                if (!validationResult.IsValid)
                {
                    return ValidationFailed(validationResult);
                }

                var customer = await _customerService.GetCustomerAsync(request.CustomerId, cancellationToken);

                var createdOrder = new OrderDto
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = customer.Id,
                    CustomerName = customer.Name,
                };

                return Created($"api/orders/{createdOrder.Id}", createdOrder);
            }

            return await RunAsync(DoCreateOrder);
        }
    }
}
