using System;
using System.Collections.Generic;
using System.Linq;

namespace NJsonApi.Web.MVCCore.HelloWorld.Models
{
    /// <summary>
    /// Primitive backing store for persistence.
    /// </summary>
    public static class StaticPersistentStore
    {
        private static int currentId { get; set; }

        public static List<Article> Articles { get; set; }

        public static List<Person> People { get; set; }

        public static List<Comment> Comments { get; set; }

        static StaticPersistentStore()
        {
            currentId = 1;

            Articles = new List<Article>();
            People = new List<Person>();
            Comments = new List<Comment>();

            var article1 = new Article("JSON API paints my bikeshed!");
            var article2 = new Article("JSON API makes the tea!");

            var person1 = new Person("Dan", "Gebhardt", "dgeb");
            var person2 = new Person("Rob", "Lang", "brainwipe");

            article1.Author = person1;
            article2.Author = person2;
            Articles.Add(article2);
            Articles.Add(article1);
            People.Add(person1);
            People.Add(person2);

            for (var i = 0; i < 5000; i++)
            {

                var comment1 = new Comment($"First! + {i}");
                var comment2 = new Comment($"I like XML Better + {i}");

                article1.Comments.Add(comment1);
                article1.Comments.Add(comment2);

                comment1.Author = person1;
                comment2.Author = person2;

                Comments.Add(comment1);
                Comments.Add(comment2);
            }
        }

        public static int GetNextId()
        {
            return currentId++;
        }
    }
}