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
    public class GroupMembersController : ApiController
    {
        private readonly PersonRepository _personRepository;

        public GroupMembersController()
        {
            _personRepository = new PersonRepository(Settings.dbConnectionString);
        }

        // GET api/<controller>
        public IEnumerable<Person> Get()
        {
            return _personRepository.GetAll();
        }

        // GET api/<controller>/5
        public IEnumerable<Person> Get(int id)
        {
            return _personRepository.GetGroupMembers(id);
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
        }*/
    }
}