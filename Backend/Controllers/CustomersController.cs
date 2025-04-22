using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyM_CRM.Data;
using CompanyM_CRM.Models;
using CompanyM_CRM.Models.DTOs;

namespace CompanyM_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public CustomersController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            
            var customerDtos = customers.Select(c => new CustomerDto
            {
                CustomerID = c.CustomerID,
                CustomerName = c.CustomerName,
                Email = c.Email,
                Phone = c.Phone,
                DateAdded = c.DateAdded
            }).ToList();
            
            return customerDtos;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = new CustomerDto
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                Phone = customer.Phone,
                DateAdded = customer.DateAdded
            };

            return customerDto;
        }

        // GET: api/Customers/Search/{name}
        [HttpGet("Search/{name}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomers(string name)
        {
            var customers = await _context.Customers
                .Where(c => c.CustomerName.Contains(name))
                .ToListAsync();
                
            var customerDtos = customers.Select(c => new CustomerDto
            {
                CustomerID = c.CustomerID,
                CustomerName = c.CustomerName,
                Email = c.Email,
                Phone = c.Phone,
                DateAdded = c.DateAdded
            }).ToList();
            
            return customerDtos;
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var customerDto = new CustomerDto
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                Phone = customer.Phone,
                DateAdded = customer.DateAdded
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerID }, customerDto);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }

        // GET: api/Customers/test
        [HttpGet("test")]
        public ActionResult<string> TestConnection()
        {
            try {
                bool canConnect = _context.Database.CanConnect();
                var connectionString = _context.Database.GetConnectionString();
                
                return Ok(new { 
                    DatabaseConnection = canConnect ? "Success" : "Failed",
                    ConnectionString = connectionString
                });
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error connecting to database: {ex.Message}");
            }
        }
    }
}