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
    public class StatusController : ControllerBase
    {
        private StatusRepository _statusRepository;

        public StatusController(IConfiguration settings)
        {
            string _connstring = settings.GetConnectionString(StaticConfig.ConnectionStringName);
            _statusRepository = new StatusRepository(_connstring);
        }

        // GET: api/Status
        [HttpGet]
        public IEnumerable<Status> Get()
        {
            return _statusRepository.GetAll();
        }

        // GET: api/Status/5
        [HttpGet("{id}")]
        public Status Get(int id)
        {
            return _statusRepository.Get(id);
        }

        // POST: api/Status
        [HttpPost]
        public IActionResult Post([FromBody] Status value)
        {
            if (value == null)
            {
                return NotFound();
            }
            _statusRepository.AddStatus(value);
            return StatusCode(204);
        }

        // PUT: api/Status/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Status value)
        {
            // Update the status
            if (_statusRepository.Get(id) != null)
            {
                _statusRepository.DeleteStatus(value);
                _statusRepository.AddStatus(value);
                return Ok(value);
            }
            else
            {
                return NotFound("Status with id " + id + " not found");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _statusRepository.DeleteStatus(id);
        }
    }
}