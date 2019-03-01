using LSSDInOutAPI.Repositories;
using LSSDInOutLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LSSDInOutAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupsController : ApiController
    {
        private readonly GroupRepository _groupRepository;

        public GroupsController()
        {
            _groupRepository = new GroupRepository(Settings.dbConnectionString);

        }

        // GET api/<controller>
        public IEnumerable<Group> Get()
        {
            return _groupRepository.GetAll();
        }

        // GET api/<controller>/5
        public Group Get(int id)
        {
            return _groupRepository.Get(id);
        }

        /*
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        */
    }
}