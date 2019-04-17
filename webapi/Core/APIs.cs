using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using webapi.Models;

namespace webapi.Core
{
    public class APIs
    {
        //THREAD 
        public static void RefreshTemp()
        {
            DBEntities database;
            Temperature tTemperature;
            bool repeat = true;
            while (repeat)
            {
                database = new DBEntities();
                try
                {

                    foreach (var vCity in database.City.ToList())
                    {
                        try
                        {
                            var response = ApiGet(vCity.city);
                            tTemperature = new Temperature
                            {
                                date = DateTime.Parse(response.date + " " + response.time + ":00"),
                                temperature = response.temp,
                                City = vCity,
                                City_id = vCity.Id
                            };
                            int count = vCity.Temperature.Count;
                            while (count >= 30)
                            {
                                var vTemperature = database.Temperature.First();
                                database.Temperature.Remove(vTemperature);
                                database.SaveChanges();
                                count--;
                            }
                            database.Temperature.Add(tTemperature);

                            database.SaveChanges();
                        }
                        catch (Exception)//try again them finally continue
                        { continue; }

                    }
                    Thread.Sleep(10000);
                }
                catch (Exception)
                { continue; }
            }
        }

        //CORE 1 bug: retorna "Sao Paulo" como default
        public static ResponseTemp ApiGet(string city_name)
        {
            string uri = "https://api.hgbrasil.com/weather/?format=json&city_name=" + city_name + "&array_limit=2&fields=only_results,temp,city_name,time,date&key=04f6182c";
            ResponseTemp response = new ResponseTemp();
            //try
            //{
                string responseJSON = new HttpClient().GetStringAsync(uri).Result;
                response = JsonConvert.DeserializeObject<ResponseTemp>(responseJSON);

            //}
            //catch (Exception)
            //{
            //    //do nothing
            //}

            return response;

        }

        //CORE Tested
        public static string ApiGetCep(string cep)
        {
            string uri = "https://viacep.com.br/ws/" + cep + "/json/";
            String localidade = null;
            try
            {
                string responseJSON = new HttpClient().GetStringAsync(uri).Result;
                dynamic obj = JsonConvert.DeserializeObject(responseJSON);
                localidade = JsonConvert.DeserializeObject<String>(JsonConvert.SerializeObject(obj["localidade"]));
            }
            catch (Exception)
            {
                //do nothing
            }
            return localidade;

        }

    }
}