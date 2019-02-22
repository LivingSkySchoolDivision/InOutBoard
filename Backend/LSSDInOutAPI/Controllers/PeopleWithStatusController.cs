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
    public class PeopleWithStatusController : ApiController
    {
        private readonly PersonWithStatusRepository _personRepository;

        public PeopleWithStatusController()
        {
            _personRepository = new PersonWithStatusRepository(Settings.dbConnectionString);
        }

        // GET: api/PersonWithStatus
        public IEnumerable<PersonWithStatus> Get()
        {
            return _personRepository.GetAll();
        }

        // GET: api/PersonWithStatus/5
        public PersonWithStatus Get(int id)
        {
            return _personRepository.Get(id);
        }
    }
}
