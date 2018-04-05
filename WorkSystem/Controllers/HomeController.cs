using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WorkSystem.Models;
using WorkSystem.ViewModels;

namespace WorkSystem.Controllers
{
    public class HomeController : Controller
    {
        string Baseurl = "http://localhost/WorkSystemAPI/";
        public async Task<ActionResult> Index()
        {
            List<Employee> employees = new List<Employee>();
            Timekeep timekeep = new Timekeep();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource 
                HttpResponseMessage Res1 = await client.GetAsync("api/employees/getdayandweek");

                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var timekeepResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api
                    timekeep = JsonConvert.DeserializeObject<Timekeep>(timekeepResponse);
                }
            }
            var model = new EmployeesVewModel();
            model.Employees = employees;
            model.Timekeep = timekeep;

            return View(model);
        }

        public async Task<ActionResult> GetNextEmployees()
        {
            List<Employee> employees = new List<Employee>();
            Timekeep timekeep = new Timekeep();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource 
                HttpResponseMessage Res = await client.GetAsync("api/employees/getnextemployees");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    employees = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                }

                //Sending request to find web api REST service resource
                HttpResponseMessage Res1 = await client.GetAsync("api/employees/getdayandweek");

                //Checking the response is successful or not which is sent using HttpClient
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var timekeepResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api
                    timekeep = JsonConvert.DeserializeObject<Timekeep>(timekeepResponse);
                }
            }
            var model = new EmployeesVewModel();
            model.Employees = employees;
            model.Timekeep = timekeep;

            return View("Index", model);
        }

        public async Task<ActionResult> WorkLog()
        {
            List<WorkLog> workLog = new List<WorkLog>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource 
                HttpResponseMessage response = await client.GetAsync("api/employees/getworklog");

                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = response.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api
                    workLog = JsonConvert.DeserializeObject<List<WorkLog>>(EmpResponse);
                }
            }
                return View(workLog);
        }

        public async Task<ActionResult> Employees()
        {
            List<Employee> employees = new List<Employee>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource 
                HttpResponseMessage response = await client.GetAsync("api/employees/getemployees");

                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = response.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api
                    employees = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                }
            }

            return View(employees);
        }

        public async Task<ActionResult> Rules()
        {
            var workRule = new WorkRule();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource 
                HttpResponseMessage response = await client.GetAsync("api/employees/getrules");

                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var rulesResponse = response.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api
                    workRule = JsonConvert.DeserializeObject<WorkRule>(rulesResponse);
                }
            }
            return View(workRule);
        }

        public async Task<ActionResult> ChangeRules(WorkRule workRule)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost/WorkSystemAPI/api/employees/changerules"));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Timeout = 4000; //ms
            var itemToSend = JsonConvert.SerializeObject(workRule);
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                streamWriter.Write(itemToSend);
                streamWriter.Flush();
                streamWriter.Dispose();
            }

            // Send the request to the server and wait for the response:  
            using (var response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:  
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var message = reader.ReadToEnd();
                }
            }
            return View("Rules", workRule);
        }
    }
}