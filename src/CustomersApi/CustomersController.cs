using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CustomersApi
{
    public class CustomersController : FunctionControllerBase
    {
        public CustomersController(ILogger<CustomersController> logger) : base(logger)
        {
        }

        [FunctionName(nameof(GetCustomer))]
        public IActionResult GetCustomer(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "customers/{id}")] HttpRequest req,
            int id)
        {
            IActionResult DoGetCustomer()
            {
                if (id <= 0)
                {
                    return ValidationFailed("Id must be positive");
                }

                // Simulate does not exist
                if (id == 100)
                {
                    return NotFound();
                }

                return OK(new CustomerDto
                {
                    Id = id,
                    Name = "Maria",
                    Age = 30
                });
            }

            return Run(DoGetCustomer);
        }


        [FunctionName(nameof(DeleteCustomer))]
        public IActionResult DeleteCustomer(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "customers/{id}")] HttpRequest req,
            int id)
        {
            IActionResult DoDeleteCustomer()
            {
                if (id <= 0)
                {
                    return ValidationFailed("Id must be positive");
                }

                Logger.LogInformation("Deleting customer {customer}", id);

                if (id == 10)
                {
                    throw new ApplicationException("Database failure deleting customer");
                }

                return new OkResult();
            }

            return Run(DoDeleteCustomer);
        }
        

        [FunctionName(nameof(CreateCustomer))]
        public IActionResult CreateCustomer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "customers")] CreateCustomerDto createCustomerDto,
            HttpRequest req // just to show that HttpRequest is still available even if we bind to Http post
            )
        {
            IActionResult DoCreateCustomer()
            {
                var validationResult = Validate(createCustomerDto);
                if (!validationResult.IsValid)
                { 
                    return ValidationFailed(validationResult);
                }

                Logger.LogInformation("Creating new customer");

                // Simulate conflict
                if (createCustomerDto.Name == "John")
                {
                    return Failed(HttpStatusCode.Conflict, "Customer already exists", ErrorCodes.AlreadyExists);
                }

                var createdCustomer = new CustomerDto
                {
                    Id = 1,
                    Name = createCustomerDto.Name,
                    Age = createCustomerDto.Age,
                };

                Logger.LogInformation("Customer {customer} created", createdCustomer.Id);

                return Created($"api/customers/{createdCustomer.Id}", createdCustomer);
            }

            return Run(DoCreateCustomer);
        }

        

        [FunctionName(nameof(UpdateCustomer))]
        public IActionResult UpdateCustomer(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "customers/{id}")] CreateCustomerDto createCustomerDto,
            int id
            )
        {
            // This will not return the common error response
            // Needs to be wrapped in a try..catch or Run(() => {})
            throw new Exception("Update customer failed");
        }
    }
}
