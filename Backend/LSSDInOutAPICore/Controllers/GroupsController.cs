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
    [Route("api/groups")]
    [ApiController]
    [EnableCors(Settings.CorsPolicyName)]
    [Produces("application/json")]
    public class GroupsController : ControllerBase
    {
        private readonly GroupRepository _groupRepository;
        private readonly Settings _settings;

        public GroupsController(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            _groupRepository = new GroupRepository(_settings.dbConnectionString);
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

    }
}