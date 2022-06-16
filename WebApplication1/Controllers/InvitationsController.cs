using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Models;
using WebApplication1.Hubs;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/invitations")]
    [ApiController]
    public class InvitationsController : ControllerBase

    {
        private readonly MyHub _hub;

        public InvitationsController(MyHub hub)
        {
            _hub = hub;
        }
        public class InvitationsJson
        {
            public string from { get; set; }

            public string to { get; set; }

            public string server { get; set; }

        }

        // POST api/<InvitationsController>
        [HttpPost]
        public void Post([FromBody] InvitationsJson invitationsJson)
        {
            bool isExist = Database.Exist(invitationsJson.to);
            if (!isExist)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                User u = Database.Get(invitationsJson.to);
                if (u.Contacts.Exists(x => x.id == invitationsJson.from) == false)
                {
                    Database.addInvitation(invitationsJson.to, invitationsJson.from, invitationsJson.server);
                }
                base.Response.StatusCode = (int)HttpStatusCode.Created;
            }
            _hub.SendContact(invitationsJson.from, invitationsJson.to, invitationsJson.server);
            
        }
    }
}
