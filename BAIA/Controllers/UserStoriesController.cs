﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAIA.Data;
using BAIA.Models;
using Microsoft.AspNetCore.Cors;

namespace BAIA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]

    public class UserStoriesController : ControllerBase
    {
        private readonly BAIA_DB_Context _context;

        public UserStoriesController(BAIA_DB_Context context)
        {
            _context = context;
        }

        // GET: api/UserStories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserStory>>> GetUserStories()
        {
            return await _context.UserStories.ToListAsync();
        }

        // GET: api/UserStories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserStory>> GetUserStory(int id)
        {
            var userStory = await _context.UserStories.FindAsync(id);

            if (userStory == null)
            {
                return NotFound();
            }

            return userStory;
        }

        // PUT: api/UserStories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserStory(int id, UserStory userStory)
        {
            if (id != userStory.UserStoryID)
            {
                return BadRequest();
            }

            _context.Entry(userStory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStoryExists(id))
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

        // POST: api/UserStories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableCors]
        public async Task<ActionResult<UserStory>> PostUserStory(UserStory userStory)
        {
            _context.UserStories.Add(userStory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserStory", new { id = userStory.UserStoryID }, userStory);
        }

        // DELETE: api/UserStories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserStory(int id)
        {
            var userStory = await _context.UserStories.FindAsync(id);
            if (userStory == null)
            {
                return NotFound();
            }

            _context.UserStories.Remove(userStory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserStoryExists(int id)
        {
            return _context.UserStories.Any(e => e.UserStoryID == id);
        }
    }
}