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
    }
}