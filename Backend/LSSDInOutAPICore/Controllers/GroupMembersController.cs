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
    [Route("api/groupmembers")]
    [ApiController]
    [EnableCors(Settings.CorsPolicyName)]
    [Produces("application/json")]
    public class GroupMembersController : ControllerBase
    {
        private readonly PersonRepository _personRepository;
        private readonly Settings _settings;

        public GroupMembersController(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            _personRepository = new PersonRepository(_settings.dbConnectionString);
        }

        // GET: api/groupmembers
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _personRepository.GetAll();
        }

        // GET: api/groupmembers/5
        [HttpGet("{id}")]
        public IEnumerable<Person> Get(int id)
        {
            return _personRepository.GetGroupMembers(id);
        }


    }
}