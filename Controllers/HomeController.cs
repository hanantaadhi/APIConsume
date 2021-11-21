using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using APIConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System;
using Microsoft.AspNetCore.Http;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace APIConsume.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<User> userList = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44324/api/user"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                }
            }
            return View(userList);
        }

        public ViewResult GetQuery() => View();

        [HttpPost]
        public async Task<IActionResult> GetUser(int m_dukcapil_data_id)
        {
            User users = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44324/api/user/" + m_dukcapil_data_id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        users = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View(users);
        }

        public ViewResult AddQuery() => View();

        [HttpPost]
        public async Task<IActionResult> AddQuery(User reservation)
        {
            User receivedReservation = new User();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44324/api/user", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedReservation = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(receivedReservation);
        }

        /*[HttpPost]
        public async Task<IActionResult> AddQuery(Reservation reservation)
        {
            Reservation receivedReservation = new Reservation();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Key", "Secret@123");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44324/api/user", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ViewBag.Result = apiResponse;
                        return View();
                    }
                }
            }
            return View(receivedReservation);
        }*/

        public async Task<IActionResult> UpdateQuery(int m_dukcapil_data_id)
        {
            User users = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44324/api/user/" + m_dukcapil_data_id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuery(User users)
        {
            User receivedUsers = new User();
            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(users.m_dukcapil_data_id.ToString()), "Id");
                content.Add(new StringContent(users.NIK), "NIK");
                content.Add(new StringContent(users.name), "name");
                content.Add(new StringContent(users.maiden_name), "maiden_name");
                content.Add(new StringContent(users.birth_date.ToString()), "birth_date");
                content.Add(new StringContent(users.gender), "gender");
                content.Add(new StringContent(users.religion_id.ToString()), "religion_id");
                content.Add(new StringContent(users.marital_status_id.ToString()), "marital_status_id");

                using (var response = await httpClient.PutAsync("https://localhost:44324/api/user", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedUsers = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(receivedUsers);
        }

        public async Task<IActionResult> UpdateQueryPatch(int m_dukcapil_data_id)
        {
            User users = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44324/api/Reservation/" + m_dukcapil_data_id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQueryPatch(int m_dukcapil_data_id, User users)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://localhost:44324/api/user/" + m_dukcapil_data_id),
                    Method = new HttpMethod("Patch"),
                    Content = new StringContent("[{ \"op\":\"replace\", \"path\":\"NIK\", \"value\":\"" + users.NIK + "\"},{ \"op\":\"replace\", \"path\":\"name\", \"value\":\"" + users.name + "\"}]", Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuery(int m_dukcapil_data_id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44324/api/user/" + m_dukcapil_data_id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public ViewResult AddFile() => View();

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            string apiResponse = "";
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                using (var fileStream = file.OpenReadStream())
                {
                    form.Add(new StreamContent(fileStream), "file", file.FileName);
                    using (var response = await httpClient.PostAsync("https://localhost:44324/api/user/UploadFile", form))
                    {
                        response.EnsureSuccessStatusCode();
                        apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return View((object)apiResponse);
        }

        public ViewResult AddQueryByXml() => View();

        [HttpPost]
        public async Task<IActionResult> AddQueryByXml(User reservation)
        {
            User receivedReservation = new User();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(ConvertObjectToXMLString(reservation), Encoding.UTF8, "application/xml");

                using (var response = await httpClient.PostAsync("https://localhost:44324/api/user/PostXml", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedReservation = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(receivedReservation);
        }

        string ConvertObjectToXMLString(object classObject)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }
    }
}