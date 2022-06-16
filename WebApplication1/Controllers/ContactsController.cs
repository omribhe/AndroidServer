using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Models;
using WebApplication1.Hubs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private readonly MyHub _hub;

        public ContactsController(MyHub hub)
        {
            _hub = hub;
        }

        public class ContactTemp
        {
            public string id { get; set; }
            public string name { get; set; }

            public string server { get; set; }

            public string last { get; set; }

            public string lastdate { get; set; }

            public ContactTemp(Contact c)
            {
                id = c.id;  
                name = c.name;
                server = c.server;
                last = c.last;
                lastdate = c.lastdate;
            }
        }


        // GET: api/<ContactsController>
        [HttpGet]
        public IEnumerable<ContactTemp> Get([FromQuery] string username)
        {
            bool isUserEx = Database.Exist(username);
            if (!isUserEx)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
            }
            User u = Database.Get(username);
            List<ContactTemp> contactTemps = new List<ContactTemp>();
            foreach (Contact temp in u.Contacts)
            {
                contactTemps.Add(new ContactTemp(temp));
            }
            return contactTemps;
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public ContactTemp Get([FromQuery] string username, string id)
        {
            bool isUserEx = Database.Exist(username);
            if (!isUserEx)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
            }
            User u = Database.Get(username);
            Contact c = u.Contacts.Find(x => x.id == id);
            if (null == c)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
            }
            ContactTemp contactTemp = new ContactTemp(c);
            return contactTemp;
        }


        public class UserContact
        {
            public string contact { get; set; }

            public string nickName { get; set; }

            public string server { get; set; }
        }

        // POST api/<ContactsController>
        [HttpPost]
        public void Post([FromQuery] string username,[FromBody] UserContact userContact)
        {
            bool isUserEx = Database.Exist(username);
            if (!isUserEx)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                User u = Database.Get(username);
                if (!u.Contacts.Exists(x => x.id == userContact.contact)) { 
                Database.addContactFromString(username, userContact.contact, userContact.nickName, userContact.server);
                base.Response.StatusCode = (int)HttpStatusCode.Created;
                }
                else
                {
                    base.Response.StatusCode = (int)HttpStatusCode.NotFound;

                }
            }
            _hub.SendMessage("", "", "");
        }


        public class UserPut
        {
            public string nickName { get; set; }

            public string server { get; set; }
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public void Put([FromQuery] string username, string id, [FromBody] UserPut userPut)
        {
            bool isUserEx = Database.Exist(username);
            if (!isUserEx)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                User u = Database.Get(username);
                if (u.Contacts.Exists(x => x.id == id))
                {
                    Database.putContactFromString(username, id, userPut.nickName, userPut.server);
                    base.Response.StatusCode = (int)HttpStatusCode.NoContent;
                }
                else
                {
                    base.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public void Delete([FromQuery] string username, string id)
        {
            bool isUserEx = Database.Exist(username);
            if (!isUserEx)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                User u = Database.Get(username);
                if (u.Contacts.Exists(x => x.id == id))
                {
                    Database.delContact(username, id);
                    base.Response.StatusCode = (int)HttpStatusCode.NoContent;
                }
                else
                {
                    base.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
        }
    }
}
