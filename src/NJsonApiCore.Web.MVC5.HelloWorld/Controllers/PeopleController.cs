using NJsonApi.Web.MVC5.HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NJsonApi.Web.MVC5.HelloWorld.Controllers
{
    [Route("people")]
    public class PeopleController : ApiController
    {
        [HttpGet]
        public IEnumerable<Comment> Get()
        {
            return StaticPersistentStore.Comments;
        }

        [Route("{id}")]
        [HttpGet()]
        public Person Get(int id)
        {
            return StaticPersistentStore.People.Single(w => w.Id == id);
        }
    }
}