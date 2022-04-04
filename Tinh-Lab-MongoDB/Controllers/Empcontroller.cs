using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using Tinh_Lab_MongoDB.Models;
using MongoDB.Driver;
using System.Configuration;


namespace Tinh_Lab_MongoDB.Controllers
{
    public class Empcontroller : ApiController
    {



        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
        //Tinh Change

        [Route("InsertEmployee")]
        [HttpPost]
        public object Addemployee(Employee objVM)
        {
            try
            {   ///Insert Emoloyeee  
                #region InsertDetails  
                if (objVM.Id == null)
                {
                    string constr = ConfigurationManager.AppSettings["connectionString"];
                    var Client = new MongoClient(constr);
                    var DB = Client.GetDatabase("Employee");
                    var collection = DB.GetCollection<Employee>("EmployeeDetails");
                    collection.InsertOne(objVM);
                    return new Status
                    { Result = "Success", Message = "Employee Details Insert Successfully" };
                }
                #endregion
                ///Update Emoloyeee  
                #region updateDetails  
                else
                {
                    string constr = ConfigurationManager.AppSettings["connectionString"];
                    var Client = new MongoClient(constr);
                    var Db = Client.GetDatabase("Employee");
                    var collection = Db.GetCollection<Employee>("EmployeeDetails");

                    var update = collection.FindOneAndUpdateAsync(Builders<Employee>.Filter.Eq("Id", objVM.Id), Builders<Employee>.Update.Set("Name", objVM.Name).Set("Department", objVM.Department).Set("Address", objVM.Address).Set("City", objVM.City).Set("Country", objVM.Country));

                    return new Status
                    { Result = "Success", Message = "Employee Details Update Successfully" };
                }
                #endregion
            }

            catch (Exception ex)
            {
                return new Status
                { Result = "Error", Message = ex.Message.ToString() };
            }

        }

        #region DeleteEmployee  
        [Route("Delete")]
        [HttpGet]
        public object Delete(string id)
        {
            try
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<Employee>("EmployeeDetails");
                var DeleteRecored = collection.DeleteOneAsync(
                               Builders<Employee>.Filter.Eq("Id", id));
                return new Status
                { Result = "Success", Message = "Employee Details Delete  Successfully" };

            }
            catch (Exception ex)
            {
                return new Status
                { Result = "Error", Message = ex.Message.ToString() };
            }

        }
        #endregion


        #region Getemployeedetails  
        [Route("GetAllEmployee")]
        [HttpGet]
        public object GetAllEmployee()
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<Employee>("EmployeeDetails").Find(new BsonDocument()).ToList();
            return Json(collection);

        }
        #endregion

        #region EmpdetaisById  
        [Route("GetEmployeeById")]
        [HttpGet]
        public object GetEmployeeById(string id)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase("Employee");
            var collection = DB.GetCollection<Employee>("EmployeeDetails");
            var plant = collection.Find(Builders<Employee>.Filter.Where(s => s.Id == id)).FirstOrDefault();
            return Json(plant);

        }
        #endregion

    }


}