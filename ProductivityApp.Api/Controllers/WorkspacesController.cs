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
    public class WorkspacesController : ControllerBase
    {
        private readonly ProductivityContext _context;

        public WorkspacesController(ProductivityContext context)
        {
            _context = context;
        }

        // GET: api/Workspaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workspace>>> GetWorkspaces()
        {
            var workspaces = await _context.Workspaces
               .Include(w => w.CreatedByUser)
               .Include(w => w.Projects)
               .ToListAsync();

            return workspaces;
        }

        // GET: api/Workspaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Workspace>> GetWorkspace(int id)
        {
            var workspace = await _context.Workspaces.FindAsync(id);

            if (workspace == null)
            {
                return NotFound();
            }

            return workspace;
        }

        // PUT: api/Workspaces/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkspace(int id, Workspace workspace)
        {
            if (id != workspace.WorkspaceId)
            {
                return BadRequest();
            }

            _context.Entry(workspace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkspaceExists(id))
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

        // POST: api/Workspaces
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Workspace>> PostWorkspace(Workspace workspace)
        {
            _context.Workspaces.Add(workspace);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkspace", new { id = workspace.WorkspaceId }, workspace);
        }

        // DELETE: api/Workspaces/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Workspace>> DeleteWorkspace(int id)
        {
            var workspace = await _context.Workspaces.FindAsync(id);
            if (workspace == null)
            {
                return NotFound();
            }

            _context.Workspaces.Remove(workspace);
            await _context.SaveChangesAsync();

            return workspace;
        }

        private bool WorkspaceExists(int id)
        {
            return _context.Workspaces.Any(e => e.WorkspaceId == id);
        }
    }
}
