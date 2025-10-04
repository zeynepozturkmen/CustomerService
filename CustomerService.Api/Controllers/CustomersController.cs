using CustomerService.Application.Commands;
using CustomerService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
        {
            var customerId = await _mediator.Send(command);
            return Ok(customerId);
        }

        // GET: api/customers/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await _mediator.Send(query);
            return Ok(customer);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return NotFound("Customer not found");

            return Ok("Customer updated successfully");
        }
    }
}
