using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using LSSDInOutAPI_MSSQL.Repositories;
using LSSDInOutLib.Model;
using Microsoft.Extensions.Options;

namespace LSSDInOutAPICore.Controllers
{  
    [Route("api/status")]
    [ApiController]
    [EnableCors(Settings.CorsPolicyName)]
    [Produces("application/json")]
    public class StatusController : ControllerBase
    {
        private StatusRepository _statusRepository;
        private readonly Settings _settings;

        public StatusController(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            this._statusRepository = new StatusRepository(_settings.dbConnectionString);
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
            } else
            {
                return  NotFound("Status with id " + id + " not found");
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