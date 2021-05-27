using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductivityApp.DataAccess;
using ProductivityApp.Model;

namespace ProductivityApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionTagsController : ControllerBase
    {
        private readonly ProductivityContext _context;

        public SessionTagsController(ProductivityContext context)
        {
            _context = context;
        }

        // GET: api/SessionTags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionTag>>> GetSessionTags()
        {
            var sessionTags = await _context.SessionTags
                .Include(p => p.Session)
                .Include(p => p.Tag)
                .ToListAsync();

            return sessionTags;
        }

        // GET: api/SessionTags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionTag>> GetSessionTag(int id)
        {
            var sessionTag = await _context.SessionTags.FindAsync(id);

            if (sessionTag == null)
            {
                return NotFound();
            }

            return sessionTag;
        }

        // PUT: api/SessionTags/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSessionTag(int id, SessionTag sessionTag)
        {
            if (id != sessionTag.SessionId)
            {
                return BadRequest();
            }

            _context.Entry(sessionTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionTagExists(id))
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

        // POST: api/SessionTags
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SessionTag>> PostSessionTag(SessionTag sessionTag)
        {
            _context.SessionTags.Add(sessionTag);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SessionTagExists(sessionTag.SessionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSessionTag", new { id = sessionTag.SessionId }, sessionTag);
        }

        // DELETE: api/SessionTags/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SessionTag>> DeleteSessionTag(int id)
        {
            var sessionTag = await _context.SessionTags.FindAsync(id);
            if (sessionTag == null)
            {
                return NotFound();
            }

            _context.SessionTags.Remove(sessionTag);
            await _context.SaveChangesAsync();

            return sessionTag;
        }

        private bool SessionTagExists(int id)
        {
            return _context.SessionTags.Any(e => e.SessionId == id);
        }
    }
}
