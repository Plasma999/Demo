using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIDemo.Controllers
{
    public class person
    {
        public string name { get; set; }
        public string surname { get; set; }
    }

    public class ProductController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value3", "value4" };
        }

        // GET api/<controller>/5
        public int GetProductById(int id)
        {
            return id;
        }

        public string GetProductByCategory(string name)
        {
            return name;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {

        }

        // PUT api/<controller>/5
        public void Put(int id, person p)
        {
            string s = p.name + "," + p.surname;
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}