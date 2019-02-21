using LSSDInOutAPI.Repositories;
using LSSDInOutLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LSSDInOutAPI.Controllers
{
    public class StatusController : ApiController
    {
        private StatusRepository _statusRepository;

        public StatusController()
        {
            this._statusRepository = new StatusRepository(Settings.dbConnectionString);
        }
        
        // GET: api/Status
        public IEnumerable<Status> Get()
        {
            return _statusRepository.GetAll();
        }

        
        // GET: api/Status/5
        public List<Status> Get(int id)
        {
            return _statusRepository.GetAllForPerson(id);
        }

        // POST: api/Status
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Status/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Status/5
        public void Delete(int id)
        {
        }
    }
}
