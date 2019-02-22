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
    public class PeopleController : ApiController
    {
        private readonly PersonRepository _personRepository;

        public PeopleController()
        {
            _personRepository = new PersonRepository(Settings.dbConnectionString);
        }

        // GET: api/Person
        public IEnumerable<Person> Get()
        {
            return _personRepository.GetAll();
        }

        // GET: api/Person/5
        public Person Get(int id)
        {
            return _personRepository.Get(id);            
        }

        /*
        // POST: api/Person
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Person/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
        }*/
    }
}
