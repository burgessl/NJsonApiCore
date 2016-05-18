using NJsonApi.Infrastructure;
using NJsonApi.Web.MVC5.HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NJsonApi.Web.MVC5.HelloWorld.Controllers
{
    [Route("articles")]
    public class ArticlesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Article> Get()
        {
            return StaticPersistentStore.Articles;
        }

        [Route(("{id}"))]
        [HttpGet]
        public Article Get(int id)
        {
            return StaticPersistentStore.Articles.Single(w => w.Id == id);
        }

        [HttpPost]
        public Article Post([FromBody]Delta<Article> article)
        {
            var newArticle = article.ToObject();
            newArticle.Id = StaticPersistentStore.GetNextId();
            StaticPersistentStore.Articles.Add(newArticle);
            return newArticle;
        }

        [Route("{id}")]
        [HttpPatch()]
        public Article Patch([FromBody]Delta<Article> update, int id)
        {
            var article = StaticPersistentStore.Articles.Single(w => w.Id == id);
            update.ApplySimpleProperties(article);
            return article;
        }

        [Route("{id}")]
        [HttpDelete()]
        public void Delete(int id)
        {
            StaticPersistentStore.Articles.RemoveAll(x => x.Id == id);
        }
    }
}