using LSSDInOutLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSSDInOutAPI.Repositories
{
    public class PersonWithStatusRepository
    {
        private PersonRepository _personRepository;
        private StatusRepository _statusRepository;
        
        public PersonWithStatusRepository(string dbConnectionString)
        {
            _personRepository = new PersonRepository(dbConnectionString);
            _statusRepository = new StatusRepository(dbConnectionString);
        }

        public PersonWithStatus Get(int id)
        {
            Person p = _personRepository.Get(id);
            if (p != null)
            {
                return p.AddStatus(_statusRepository.GetActiveForPerson(id));
            }

            return null;
        }

        public List<PersonWithStatus> GetAll()
        {
            List<PersonWithStatus> returnMe = new List<PersonWithStatus>();
            foreach(Person p in _personRepository.GetAll())
            {
                returnMe.Add(p.AddStatus(_statusRepository.GetActiveForPerson(p.ID)));
            }


            return returnMe;
        }
    }
}