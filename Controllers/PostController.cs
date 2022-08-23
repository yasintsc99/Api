using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var posts = mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").AsQueryable();
            return new JsonResult(posts);
        }

        [HttpGet]
        [Route("{id}")]

        public JsonResult Get(int id)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var filter = Builders<Post>.Filter.Eq("PostID", id);
            var post = mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").Find(filter).FirstOrDefault();
            return new JsonResult(post);
        }

        [HttpPost]

        public JsonResult Post(Post post)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            int lastID = mongo.GetDatabase("Hurriyet").GetCollection<Category>("Post").AsQueryable().Count();
            post.PostID = lastID + 1;
            mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").InsertOne(post);

            return new JsonResult(post);
        }
        [HttpPut]
        public JsonResult Put(Post post)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var filter = Builders<Post>.Filter.Eq("Post", post.PostID);
            var title = Builders<Post>.Update.Set("Title", post.Title);
            var description = Builders<Post>.Update.Set("Description", post.Description);
            var content = Builders<Post>.Update.Set("Content", post.Content);
            var categoryID = Builders<Post>.Update.Set("CategoryID", post.CategoryID);
            mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").UpdateOne(filter, title);
            mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").UpdateOne(filter, description);
            mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").UpdateOne(filter, content);
            mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").UpdateOne(filter, categoryID);
            return new JsonResult(post);
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var post = mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").Find(x => x.PostID == id).FirstOrDefault();
            if (post == null)
                return new JsonResult(NotFound());
            else
            {
                mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").DeleteOne(x => x.PostID == id);
                return new JsonResult(post.PostID + " numbered record is deleted");
            }

        }

    }
}
