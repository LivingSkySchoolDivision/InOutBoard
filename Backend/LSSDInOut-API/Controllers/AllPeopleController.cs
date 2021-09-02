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
    public class AllPeopleController : ControllerBase
    {
        private readonly PersonRepository _personRepository;

        public AllPeopleController(IConfiguration settings)
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
    }
}
