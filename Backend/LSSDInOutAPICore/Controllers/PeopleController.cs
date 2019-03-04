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
    [Route("api/people")]
    [ApiController]
    [EnableCors(Settings.CorsPolicyName)]
    [Produces("application/json")]
    public class PeopleController : ControllerBase
    {
        private readonly PersonRepository _personRepository;
        private readonly Settings _settings;

        public PeopleController(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            _personRepository = new PersonRepository(_settings.dbConnectionString);
        }

        // GET: api/Person
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _personRepository.GetAll();
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return _personRepository.Get(id);   
        }

    }
}
