using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using webapi.Models;
using webapi.Core;

namespace webapi.Controllers
{
    public class TempController : ApiController
    {

        // GET:Tested
        [Route("temperatures")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<MCity> mCities = new List<MCity>();
            List<MTemperature> mTemperatures = new List<MTemperature>();
            MCity mCity = new MCity();
            MTemperature mTemperature;
            using (DBEntities database = new DBEntities())
            {
                foreach (var vCity in database.City.ToList())
                {
                    mTemperature = new MTemperature();//ex

                    mCity.Id = vCity.Id;
                    mCity.city = vCity.city;
                    var vTemperature = (database.Temperature.Where(t => t.City_id == vCity.Id)).ToList().Last();

                    mTemperature.Id = vTemperature.Id;
                    mTemperature.date = "" + vTemperature.date;//gambiarra
                    mTemperature.temperature = vTemperature.temperature;
                    mTemperature.City_id = vTemperature.City_id;
                    mTemperatures.Add(mTemperature);
                    mCity.temperatures = mTemperatures;
                    mCities.Add(mCity);
                    mCity = new MCity();
                    mTemperatures = new List<MTemperature>();
                }

            }

            return Ok(mCities);
        }

        // GET:Tested
        [Route("cities/{city_name}/temperatures")]
        [HttpGet]
        public IHttpActionResult Get(string city_name)
        {
            MCity mCity = new MCity();
            MTemperature mTemperature = new MTemperature();
            List<MTemperature> mTemperatures = new List<MTemperature>();
            using (DBEntities database = new DBEntities())
            {
                var vCity = database.City.FirstOrDefault(c => c.city == city_name);
                if (vCity == null)
                {
                    return NotFound();
                }
                mCity.Id = vCity.Id;
                mCity.city = vCity.city;
                foreach (var vTemperature in database.Temperature.Where(t => t.City_id == vCity.Id))
                {
                    mTemperature.Id = vTemperature.Id;
                    mTemperature.date = "" + vTemperature.date;//gambiarra
                    mTemperature.temperature = vTemperature.temperature;
                    mTemperature.City_id = vTemperature.City_id;
                    mTemperatures.Add(mTemperature);
                    mTemperature = new MTemperature();
                }
                mCity.temperatures = mTemperatures;
            }

            return Ok(mCity);
        }

        // POST:Tested
        [Route("cities/{city_name}")]
        [HttpPost]
        public IHttpActionResult Post(string city_name)
        {
            if (city_name == null)
            {
                return BadRequest();
            }
            City tCity = new City();
            var response = APIs.ApiGet(city_name);
            using (DBEntities database = new DBEntities())
            {
                var cityExist = database.City.FirstOrDefault(c => c.city == response.city_name);
                if (cityExist != null)
                {
                    return BadRequest();
                }
                tCity.city = response.city_name;

                database.City.Add(tCity);
                database.SaveChanges();
            }
            return Ok(tCity.city);
        }

        // POST: Tested
        [Route("cities/by_cep/{cep}")]
        [HttpPost]
        public IHttpActionResult PostCep(string cep)
        {
            string city_name = APIs.ApiGetCep(cep);
            return Post(city_name);
        }

        // DELETE: Tested
        [Route("cities/{city_name}")]
        [HttpDelete]
        public IHttpActionResult Delete(string city_name)
        {
            City tCity = new City();
            using (DBEntities database = new DBEntities())
            {
                tCity = database.City.FirstOrDefault(c => c.city == city_name);
                if (tCity == null)
                {
                    return BadRequest();
                }
                database.City.Remove(tCity);
                database.SaveChanges();
            }
            return Ok();
        }

        // DELETE:Tested
        [Route("cities/{city_name}/temperatures")]
        [HttpDelete]
        public IHttpActionResult DeleteTemps(string city_name)
        {
            City tCity = new City();
            using (DBEntities database = new DBEntities())
            {
                var vCity = database.City.FirstOrDefault(c => c.city == city_name);
                if (vCity == null)
                {
                    return BadRequest();
                }
                var vTemperatures = database.Temperature.Where(t => t.City.city == vCity.city).ToList();
                database.Temperature.RemoveRange(vTemperatures);
                try
                {
                    database.SaveChanges();
                }
                catch (Exception)
                {

                    return NotFound();
                }
            }
            return Ok();
        }
    }
}
