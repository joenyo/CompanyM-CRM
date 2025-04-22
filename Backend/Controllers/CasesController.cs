using System;
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
    public class CasesController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public CasesController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/Cases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseDto>>> GetCases()
        {
            var cases = await _context.Cases
                .Include(c => c.Customer)
                .Include(c => c.Channel)
                .ToListAsync();

            var caseDtos = cases.Select(c => new CaseDto
            {
                CaseID = c.CaseID,
                CustomerID = c.CustomerID,
                CustomerName = c.Customer?.CustomerName ?? "Unknown",
                ChannelID = c.ChannelID,
                ChannelName = c.Channel?.ChannelName ?? "Unknown",
                Subject = c.Subject ?? "",
                Description = c.Description,
                Status = c.Status ?? "",
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate
            }).ToList();

            return caseDtos;
        }

        // GET: api/Cases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CaseDto>> GetCase(int id)
        {
            var @case = await _context.Cases
                .Include(c => c.Customer)
                .Include(c => c.Channel)
                .FirstOrDefaultAsync(c => c.CaseID == id);

            if (@case == null)
            {
                return NotFound();
            }

            var caseDto = new CaseDto
            {
                CaseID = @case.CaseID,
                CustomerID = @case.CustomerID,
                CustomerName = @case.Customer?.CustomerName ?? "Unknown",
                ChannelID = @case.ChannelID,
                ChannelName = @case.Channel?.ChannelName ?? "Unknown",
                Subject = @case.Subject ?? "",
                Description = @case.Description,
                Status = @case.Status ?? "",
                CreatedDate = @case.CreatedDate,
                LastUpdatedDate = @case.LastUpdatedDate
            };

            return caseDto;
        }

        // GET: api/Cases/ByCustomer/5
        [HttpGet("ByCustomer/{customerId}")]
        public async Task<ActionResult<IEnumerable<CaseDto>>> GetCasesByCustomer(int customerId)
        {
            var cases = await _context.Cases
                .Where(c => c.CustomerID == customerId)
                .Include(c => c.Customer)
                .Include(c => c.Channel)
                .ToListAsync();

            var caseDtos = cases.Select(c => new CaseDto
            {
                CaseID = c.CaseID,
                CustomerID = c.CustomerID,
                CustomerName = c.Customer?.CustomerName ?? "Unknown",
                ChannelID = c.ChannelID,
                ChannelName = c.Channel?.ChannelName ?? "Unknown",
                Subject = c.Subject ?? "",
                Description = c.Description,
                Status = c.Status ?? "",
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate
            }).ToList();

            return caseDtos;
        }

        // GET: api/Cases/ByChannel/5
        [HttpGet("ByChannel/{channelId}")]
        public async Task<ActionResult<IEnumerable<CaseDto>>> GetCasesByChannel(int channelId)
        {
            var cases = await _context.Cases
                .Where(c => c.ChannelID == channelId)
                .Include(c => c.Customer)
                .Include(c => c.Channel)
                .ToListAsync();

            var caseDtos = cases.Select(c => new CaseDto
            {
                CaseID = c.CaseID,
                CustomerID = c.CustomerID,
                CustomerName = c.Customer?.CustomerName ?? "Unknown",
                ChannelID = c.ChannelID,
                ChannelName = c.Channel?.ChannelName ?? "Unknown",
                Subject = c.Subject ?? "",
                Description = c.Description,
                Status = c.Status ?? "",
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate
            }).ToList();

            return caseDtos;
        }

        // GET: api/Cases/Search?customerName=John&channelId=4
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<CaseDto>>> SearchCases(string? customerName, int? channelId)
        {
            var query = _context.Cases
                .Include(c => c.Customer)
                .Include(c => c.Channel)
                .AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(c => c.Customer != null && c.Customer.CustomerName.Contains(customerName));
            }

            if (channelId.HasValue)
            {
                query = query.Where(c => c.ChannelID == channelId.Value);
            }

            var cases = await query.ToListAsync();

            var caseDtos = cases.Select(c => new CaseDto
            {
                CaseID = c.CaseID,
                CustomerID = c.CustomerID,
                CustomerName = c.Customer?.CustomerName ?? "Unknown",
                ChannelID = c.ChannelID,
                ChannelName = c.Channel?.ChannelName ?? "Unknown",
                Subject = c.Subject ?? "",
                Description = c.Description,
                Status = c.Status ?? "",
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate
            }).ToList();

            return caseDtos;
        }

        // POST: api/Cases
        [HttpPost]
        public async Task<ActionResult<CaseDto>> CreateCase(Case @case)
        {
            @case.CreatedDate = DateTime.Now;
            @case.LastUpdatedDate = DateTime.Now;
            
            _context.Cases.Add(@case);
            await _context.SaveChangesAsync();

            // Load the related entities
            await _context.Entry(@case).Reference(c => c.Customer).LoadAsync();
            await _context.Entry(@case).Reference(c => c.Channel).LoadAsync();

            var caseDto = new CaseDto
            {
                CaseID = @case.CaseID,
                CustomerID = @case.CustomerID,
                CustomerName = @case.Customer?.CustomerName ?? "Unknown",
                ChannelID = @case.ChannelID,
                ChannelName = @case.Channel?.ChannelName ?? "Unknown",
                Subject = @case.Subject ?? "",
                Description = @case.Description,
                Status = @case.Status ?? "",
                CreatedDate = @case.CreatedDate,
                LastUpdatedDate = @case.LastUpdatedDate
            };

            return CreatedAtAction(nameof(GetCase), new { id = @case.CaseID }, caseDto);
        }

        // PUT: api/Cases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCase(int id, Case @case)
        {
            if (id != @case.CaseID)
            {
                return BadRequest();
            }

            @case.LastUpdatedDate = DateTime.Now;
            _context.Entry(@case).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseExists(id))
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

        // DELETE: api/Cases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var @case = await _context.Cases.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }

            _context.Cases.Remove(@case);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.CaseID == id);
        }
    }
}