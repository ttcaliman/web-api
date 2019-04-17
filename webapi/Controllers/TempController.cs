using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using webapi.Models;
using webapi.Core;
using System.Net.Http;
using System.Net;

namespace webapi.Controllers
{
    public class TempController : ApiController
    {
        List<MCity> mCities = new List<MCity>();
        List<MTemperature> mTemperatures = new List<MTemperature>();
        MCity mCity = new MCity();
        MTemperature mTemperature = new MTemperature();
        City tCity = new City();

        // GET
        [Route("temperatures")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (DBEntities database = new DBEntities())
            {
                foreach (var vCity in database.City.ToList())
                {

                    mCity.Id = vCity.Id;
                    mCity.city = vCity.city;
                    var vTemperature = (database.Temperature.Where(t => t.City_id == vCity.Id).ToList()).LastOrDefault();
                    if (vTemperature != null)
                    {
                        mTemperature.Id = vTemperature.Id;
                        mTemperature.date = vTemperature.date.ToString();
                        mTemperature.temperature = vTemperature.temperature;
                        mTemperature.City_id = vTemperature.City_id;
                        mTemperatures.Add(mTemperature);
                        mCity.temperatures = mTemperatures;
                    }
                    mCities.Add(mCity);
                    mCity = new MCity();
                    mTemperatures = new List<MTemperature>();
                }

            }
            return Request.CreateResponse(HttpStatusCode.OK, mCities);

        }

        // GET NAME
        [Route("cities/{city_name}/temperatures")]
        [HttpGet]
        public HttpResponseMessage Get(string city_name)
        {
            using (DBEntities database = new DBEntities())
            {
                var vCity = database.City.FirstOrDefault(c => c.city == city_name);
                if (vCity == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound); ;
                }
                mCity.Id = vCity.Id;
                mCity.city = vCity.city;
                foreach (var vTemperature in database.Temperature.Where(t => t.City_id == vCity.Id))
                {
                    mTemperature.Id = vTemperature.Id;
                    mTemperature.date = vTemperature.date.ToString();
                    mTemperature.temperature = vTemperature.temperature;
                    mTemperature.City_id = vTemperature.City_id;
                    mTemperatures.Add(mTemperature);
                    mTemperature = new MTemperature();
                }
                mCity.temperatures = mTemperatures;
            }
            return Request.CreateResponse(HttpStatusCode.OK, mCity);

        }

        // POST
        [Route("cities/{city_name}")]
        [HttpPost]
        public HttpResponseMessage Post(string city_name)
        {
            var response = APIs.ApiGet(city_name);
            using (DBEntities database = new DBEntities())
            {
                var cityExist = database.City.FirstOrDefault(c => c.city == response.city_name);
                if (cityExist != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "city already exists");

                }
                tCity.city = response.city_name;

                database.City.Add(tCity);
                database.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK);

        }

        // POST CEP
        [Route("cities/by_cep/{cep}")]
        [HttpPost]
        public HttpResponseMessage PostCep(string cep)
        {
            string city_name = APIs.ApiGetCep(cep);
            if (city_name == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "city_name is null");
            }
            return Post(city_name);
        }

        // DELETE
        [Route("cities/{city_name}")]
        [HttpDelete]
        public HttpResponseMessage Delete(string city_name)
        {
            using (DBEntities database = new DBEntities())
            {
                tCity = database.City.FirstOrDefault(c => c.city == city_name);
                if (tCity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "city not registred");

                }

                database.City.Remove(tCity);
                database.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK);

        }

        // DELETE TEMP
        [Route("cities/{city_name}/temperatures")]
        [HttpDelete]
        public HttpResponseMessage DeleteTemps(string city_name)
        {
            using (DBEntities database = new DBEntities())
            {
                var vCity = database.City.FirstOrDefault(c => c.city == city_name);
                if (vCity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "city not found");
                }
                var vTemperatures = database.Temperature.Where(t => t.City.city == vCity.city).ToList();

                database.Temperature.RemoveRange(vTemperatures);
                //try
                //{
                database.SaveChanges();
                //}
                //catch (Exception)
                //{

                //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "city not found");
                //}
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
