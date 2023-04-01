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
    public class PeopleController : ControllerBase
    {
        private readonly PersonRepository _personRepository;

        public PeopleController(IConfiguration settings)
        {
            string _connstring = settings.GetConnectionString(StaticConfig.ConnectionStringName);
            _personRepository = new PersonRepository(_connstring);
        }

        // GET: api/Person
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _personRepository.GetEnabled();
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return _personRepository.Get(id);
        }

        // PUT: api/Person/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Person value)
        {
            if (_personRepository.Get(id) != null)
            {
                _personRepository.Update(value);
                return Ok(value);
            }
            else
            {
                return NotFound("Person with id " + id + " not found");
            }
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody] Person value)
        {
            if (_personRepository.Get(id) != null)            {
                _personRepository.Delete(value);
                return Ok(value);
            }
            else
            {
                return NotFound("Person with id " + id + " not found");
            }
        }

    }
}