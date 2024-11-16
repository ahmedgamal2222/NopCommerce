using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Customers.DynamicsIntegration.Services;

namespace Nop.Plugin.Customers.DynamicsIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly DynamicsIntegrationService _dynamicsService;

        public CustomerApiController()
        {
            _dynamicsService = new DynamicsIntegrationService();
        }

        [HttpPost("register")]
        public IActionResult RegisterCustomer([FromBody] CustomerDto customer)
        {
            if (_dynamicsService.ContactExists(customer.Email))
            {
                return BadRequest("A contact with this email already exists in CRM.");
            }

            _dynamicsService.CreateContact(customer.FirstName, customer.LastName, customer.Email);
            return Ok("Contact created successfully in CRM.");
        }

        [HttpGet("getByEmail")]
        public IActionResult GetCustomerByEmail(string email)
        {
            var contact = _dynamicsService.GetContactByEmail(email);
            if (contact == null)
            {
                return NotFound("No contact found with this email.");
            }
            return Ok(contact);
        }

        [HttpPut("update")]
        public IActionResult UpdateCustomer([FromBody] CustomerDto customer)
        {
            if (!_dynamicsService.ContactExists(customer.Email))
            {
                return NotFound("No contact found with this email.");
            }

            _dynamicsService.UpdateContact(customer.FirstName, customer.LastName, customer.Email);
            return Ok("Contact updated successfully in CRM.");
        }

        [HttpDelete("delete")]
        public IActionResult DeleteCustomer(string email)
        {
            if (!_dynamicsService.ContactExists(email))
            {
                return NotFound("No contact found with this email.");
            }

            _dynamicsService.DeleteContact(email);
            return Ok("Contact deleted successfully from CRM.");
        }

        [HttpGet("getAll")]
        public IActionResult GetAllCustomers()
        {
            var customers = _dynamicsService.GetAllContacts();
            return Ok(customers);
        }
    }

    public class CustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
