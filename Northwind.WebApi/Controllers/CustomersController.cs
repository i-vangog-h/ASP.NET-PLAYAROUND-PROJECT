using Microsoft.AspNetCore.Mvc;  // To use [Route], [ApiController], ControllerBase and so on.
using Northwind.EntityModels; //To use Customer
using Northwind.WebApi.Repositories; //To use ICustomerRepository

namespace Northwind.WebApi.Controllers;

// Base address: api/customers
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private ICustomerRepository _repo;

    public CustomersController(ICustomerRepository repo)
    {
        _repo = repo;
    }

    // GET: api/customers
    // GET: api/customers/?country=[country]
    // this will always return a list of customers (but it might be empty)
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
    public async Task<IEnumerable<Customer>> GetCustomers(string? country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return await _repo.RetrieveAllAsync();
        }
        else
        {
            return (await _repo.RetrieveAllAsync())
            .Where(customer => customer.Country == country);
        }
    }

    // GET: api/customers/[id]
    [HttpGet("{id}", Name = nameof(GetCustomer))] //Named route
    [ProducesResponseType(200, Type = typeof(Customer))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomer(string id)
    {
        Customer? c = await _repo.RetrieveAsync(id);

        if (c is null)
        {
            return NotFound(); // 404 Resource not found.
        }
        
        return Ok(c); // 200 OK with customer in body
    }

    // POST: api/customers
    // BODY: Customer (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Customer))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        if (customer is null)
        {
            return BadRequest();
        }

        Customer? addedCustomer = await _repo.CreateAsync(customer);

        if(addedCustomer is null)
        {
            return BadRequest("Repository failed to create customer");
        }

        return CreatedAtRoute(
            routeName: nameof(GetCustomer),
            routeValues: new {id = addedCustomer.CustomerId.ToLower()},
            value: addedCustomer
        );
    }

    //PUT: api/customers/[id]
    //BODY: Customer (JSON, XML)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(string id, [FromBody] Customer c)
    {

        id = id.ToUpper();
        c.CustomerId = c.CustomerId.ToUpper();
        if (c == null || c.CustomerId != id)
        {
            return BadRequest(); // 400 Bad request.
        }

        Customer? existing = await _repo.RetrieveAsync(id);

        if (existing is null)
        {
            return NotFound(); //404 Resource not found
        }
        
        Customer? updatedCustomer = await _repo.UpdateAsync(c);

        if(updatedCustomer is null)
        {
            return BadRequest("Repository failed to update customer");
        }

        return NoContent();
    }

    //DELETE: api/customers/[id]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id)
    {
        // Take control of problem details.
        if (id == "bad")
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://localhost:5151/customers/failed-to-delete",
                Title = $"Customer ID {id} found but failed to delete.",
                Detail = "More details like Company Name, Country and so on.",
                Instance = HttpContext.Request.Path
            };
            return BadRequest(problemDetails); // 400 Bad Request
        }

        Customer? existing = await _repo.RetrieveAsync(id);

        if (existing is null)
        {
            return NotFound();
        }

        bool? deleted = await _repo.DeleteAsync(id);

        if (deleted.HasValue && deleted.Value)
        {
            return NoContent(); // 204 No content
        }

        return BadRequest($"Customer {id} was found but failed to delete."); // 400 Bad request
    }
}
