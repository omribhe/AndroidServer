using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Hubs;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/transfer")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly MyHub _hub;

        public TransferController(MyHub hub)
        {
            _hub = hub;
        }

        public class TransferBody
        {
            public string from { get; set; }
            public string to { get; set; }
                
            public string content { get; set; }
        }


        // POST api/<TransferController>
        [HttpPost]
        public void Post([FromBody] TransferBody postTransfer)  
        {
            bool isUserEx = Database.Exist(postTransfer.to);
            if (!isUserEx)
            {
                base.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                User u = Database.Get(postTransfer.to);
                if (u.Contacts.Exists(x => x.id == postTransfer.from))
                {
                    Database.addTransfer(postTransfer.to, postTransfer.from, postTransfer.content, false);
                    base.Response.StatusCode = (int)HttpStatusCode.Created;
                }
                else
                {
                    base.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                _hub.SendMessage(postTransfer.to, postTransfer.from, postTransfer.content);
            }
        }
    }
}
