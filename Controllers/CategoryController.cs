using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        
        public JsonResult Get()
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var categories = mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").AsQueryable();
            return new JsonResult(categories);
        }
        
        [HttpGet]
        [Route("{id}")]

        public JsonResult Get(int id)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var filter = Builders<Category>.Filter.Eq("CategoryID", id);
            var category = mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").Find(filter).FirstOrDefault();
            return new JsonResult(category);
        }
        
        [HttpPost]

        public JsonResult Post(Category category)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            int lastID = mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").AsQueryable().Count();
            category.CategoryID = lastID + 1;
            mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").InsertOne(category);

            return new JsonResult(category);
        }
        [HttpPut]
        public JsonResult Put(Category category)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var filter = Builders<Category>.Filter.Eq("CategoryID", category.CategoryID);
            var title = Builders<Category>.Update.Set("Title", category.Title);
            var description = Builders<Category>.Update.Set("Description", category.Description);

            mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").UpdateOne(filter,title);
            mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").UpdateOne(filter, description);
            return new JsonResult(category);
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient mongo = new MongoClient(_configuration.GetConnectionString("MongoDBCon"));
            var category = mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").Find(x => x.CategoryID == id).FirstOrDefault();
            if (category == null)
                return new JsonResult(NotFound());
            else
            {
                mongo.GetDatabase("Hurriyet").GetCollection<Category>("Category").DeleteOne(x => x.CategoryID == id);
                mongo.GetDatabase("Hurriyet").GetCollection<Post>("Post").DeleteMany(x => x.CategoryID == id);
                return new JsonResult(category.CategoryID + " numbered record is deleted");
            }
            
        }
      }
}
