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
    public class ChannelTypesController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public ChannelTypesController(CrmDbContext context)
        {
            _context = context;
        }

        // GET: api/ChannelTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChannelTypeDto>>> GetChannelTypes()
        {
            var channelTypes = await _context.ChannelTypes.ToListAsync();
            
            var channelTypeDtos = channelTypes.Select(c => new ChannelTypeDto
            {
                ChannelID = c.ChannelID,
                ChannelName = c.ChannelName
            }).ToList();
            
            return channelTypeDtos;
        }

        // GET: api/ChannelTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelTypeDto>> GetChannelType(int id)
        {
            var channelType = await _context.ChannelTypes.FindAsync(id);

            if (channelType == null)
            {
                return NotFound();
            }

            var channelTypeDto = new ChannelTypeDto
            {
                ChannelID = channelType.ChannelID,
                ChannelName = channelType.ChannelName
            };

            return channelTypeDto;
        }
    }
}