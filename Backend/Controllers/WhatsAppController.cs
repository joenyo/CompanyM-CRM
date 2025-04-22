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
    public class WhatsAppController : ControllerBase
    {
        private readonly CrmDbContext _context;
        private readonly int _whatsAppChannelId = 4; // WhatsApp channel ID is 4 based on our seed data

        public WhatsAppController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/WhatsApp
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseDto>>> GetWhatsAppCases()
        {
            var cases = await _context.Cases
                .Where(c => c.ChannelID == _whatsAppChannelId)
                .Include(c => c.Customer)
                .Include(c => c.Channel)
                .ToListAsync();

            var caseDtos = cases.Select(c => new CaseDto
            {
                CaseID = c.CaseID,
                CustomerID = c.CustomerID,
                CustomerName = c.Customer?.CustomerName ?? "Unknown",
                ChannelID = c.ChannelID,
                ChannelName = c.Channel?.ChannelName ?? "WhatsApp",
                Subject = c.Subject ?? "",
                Description = c.Description,
                Status = c.Status ?? "",
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate
            }).ToList();

            return caseDtos;
        }

        // GET: api/WhatsApp/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CaseDto>> GetWhatsAppCase(int id)
        {
            var @case = await _context.Cases
                .Where(c => c.ChannelID == _whatsAppChannelId && c.CaseID == id)
                .Include(c => c.Customer)
                .Include(c => c.Channel)
                .FirstOrDefaultAsync();

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
                ChannelName = @case.Channel?.ChannelName ?? "WhatsApp",
                Subject = @case.Subject ?? "",
                Description = @case.Description,
                Status = @case.Status ?? "",
                CreatedDate = @case.CreatedDate,
                LastUpdatedDate = @case.LastUpdatedDate
            };

            return caseDto;
        }

        // POST: api/WhatsApp
        [HttpPost]
        public async Task<ActionResult<CaseDto>> CreateWhatsAppCase(Case @case)
        {
            @case.ChannelID = _whatsAppChannelId;
            @case.CreatedDate = DateTime.Now;
            @case.LastUpdatedDate = DateTime.Now;
            
            _context.Cases.Add(@case);
            await _context.SaveChangesAsync();

            // Load the customer
            await _context.Entry(@case).Reference(c => c.Customer).LoadAsync();

            var caseDto = new CaseDto
            {
                CaseID = @case.CaseID,
                CustomerID = @case.CustomerID,
                CustomerName = @case.Customer?.CustomerName ?? "Unknown",
                ChannelID = @case.ChannelID,
                ChannelName = "WhatsApp",
                Subject = @case.Subject ?? "",
                Description = @case.Description,
                Status = @case.Status ?? "",
                CreatedDate = @case.CreatedDate,
                LastUpdatedDate = @case.LastUpdatedDate
            };

            return CreatedAtAction(nameof(GetWhatsAppCase), new { id = @case.CaseID }, caseDto);
        }

        // PUT: api/WhatsApp/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWhatsAppCase(int id, Case @case)
        {
            if (id != @case.CaseID)
            {
                return BadRequest();
            }

            // Ensure it's a WhatsApp case
            var existingCase = await _context.Cases.FindAsync(id);
            if (existingCase == null || existingCase.ChannelID != _whatsAppChannelId)
            {
                return NotFound("Case not found or not a WhatsApp case");
            }

            @case.ChannelID = _whatsAppChannelId; // Ensure channel remains WhatsApp
            @case.LastUpdatedDate = DateTime.Now;
            
            _context.Entry(existingCase).State = EntityState.Detached;
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

        // DELETE: api/WhatsApp/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWhatsAppCase(int id)
        {
            var @case = await _context.Cases
                .Where(c => c.ChannelID == _whatsAppChannelId && c.CaseID == id)
                .FirstOrDefaultAsync();
                
            if (@case == null)
            {
                return NotFound("Case not found or not a WhatsApp case");
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