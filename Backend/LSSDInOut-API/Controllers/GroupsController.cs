using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LSSD.InOut.Lib.Model;
using LSSD.InOut.Data.Repositories;
using LSSD.InOut.API;

namespace LSSD.InOut.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly GroupRepository _groupRepository;

        public GroupsController(IConfiguration settings)
        {
            string _connstring = settings.GetConnectionString(StaticConfig.ConnectionStringName);
            _groupRepository = new GroupRepository(_connstring);
        }

        // GET: api/Groups 
        [HttpGet]
        public IEnumerable<Group> Get()
        {
            return _groupRepository.GetAll();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public Group Get(int id)
        {
            return _groupRepository.Get(id);
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Group value)
        {
            if (_groupRepository.Get(id) != null)
            {
                _groupRepository.Update(value);
                return Ok(value);
            }
            else
            {
                return NotFound("Groups with id " + id + " not found");
            }
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody] Group value)
        {
            if (_groupRepository.Get(id) != null)            {
                _groupRepository.Delete(value);
                return Ok(value);
            }
            else
            {
                return NotFound("Groups with id " + id + " not found");
            }
        }

        // POST: api/Groups/5
        [HttpPost]
        public IActionResult Post([FromBody] Group value)
        {
            try {
                _groupRepository.Add(value);
                return Ok(value);
            }
            catch
            {
                return BadRequest("An error occurred.");
            }
        }
    }
}