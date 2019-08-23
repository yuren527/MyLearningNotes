using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SimpleRestServer.Models;
using System.Collections;

namespace SimpleRestServer.Controllers
{
    public class PersonController : ApiController
    {
        // GET: api/Person
        public ArrayList Get()
        {
            PersonPersistence pp = new PersonPersistence();
            return pp.GetPersons();
        }

        // GET: api/Person/5
        public Person Get(int id)
        {
            PersonPersistence pp = new PersonPersistence();           
            return pp.GetPerson(id);
        }

        // POST: api/Person
        public HttpResponseMessage Post([FromBody]Person person)
        {
            PersonPersistence pp = new PersonPersistence();
            long id = pp.SavePerson(person);
            person.ID = id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, "person/" + id.ToString());
            return response;
        }

        // PUT: api/Person/5
        public HttpResponseMessage Put(int id, [FromBody]Person person)
        {
            PersonPersistence pp = new PersonPersistence();
            if(pp.UpdatePerson(id, person))
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
                response.Headers.Location = new Uri(Request.RequestUri, "person/" + id.ToString());
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // DELETE: api/Person/5
        public HttpResponseMessage Delete(int id)
        {
            PersonPersistence pp = new PersonPersistence();
            if (pp.DeletePerson(id))
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
