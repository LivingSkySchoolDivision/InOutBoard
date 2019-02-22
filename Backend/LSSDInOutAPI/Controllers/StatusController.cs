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
        public Status Get(int id)
        {
            return _statusRepository.Get(id);
        }

        // POST: api/Status
        public void Post([FromBody]Status value)
        {            
            if (value == null)
            {
                throw new Exception("value is null");
            }
            _statusRepository.AddStatus(value);
        }

        // PUT: api/Status/5
        public HttpResponseMessage Put(int id, [FromBody]Status value)
        {
            // Update the status
            if (_statusRepository.Get(id) != null)
            {
                _statusRepository.DeleteStatus(value);
                _statusRepository.AddStatus(value);                
                return Request.CreateResponse(HttpStatusCode.OK, value);
            } else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Status with id " + id + " not found");
            }            
        }

        // DELETE: api/Status/5
        public void Delete(int id)
        {
            _statusRepository.DeleteStatus(id);
        }
    }
}
