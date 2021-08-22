using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ENSEK_EnergySupplierClient.Models;

namespace ENSEK_EnergySupplierClient.Controllers
{
    public class AccountsController : Controller
    {
        string baseurl = "https://localhost:44329/";
        // GET: Accounts

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTestAccounts()
        {
            using (var client = new HttpClient())
            {                
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(baseurl+"api/Meter/GetAllTestAccounts");
                if (response.IsSuccessStatusCode)
                {
                    var ta = response.Content.ReadAsAsync<List<TestAccount>>().Result;
                    return View(ta);
                }
                return View();
            }
        }

            [HttpGet]
            // GET: Accounts/Details/5
            public async Task<ActionResult> GetTestAccount(int accountId)
            {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(baseurl + "api/Meter/GetTestAccount?accountId=" + accountId);
                if (response.IsSuccessStatusCode)
                {
                    var ta = response.Content.ReadAsAsync<TestAccount>().Result;
                    return View(ta);
                }
            }
            return View();
        }

            // GET: Accounts/Create
            public ActionResult AddNewTestAccount()
            {
                return View();
            }

            // POST: Accounts/Create
            [HttpPost]
            public ActionResult Create(FormCollection collection)
            {
                try
                {
                    // TODO: Add insert logic here

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
        
        // GET: Accounts/Edit/5
        public async Task<ActionResult> EditTestAccount(int accountId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(baseurl + "api/Meter/GetTestAccount?accountId=" + accountId);
                if (response.IsSuccessStatusCode)
                {
                    var ta = response.Content.ReadAsAsync<TestAccount>().Result;
                    return View(ta);
                }
            }
            return View();
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        public async Task<ActionResult> EditTestAccount(TestAccount ta)
        {
            try
            {
                if (ta != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await client.PutAsJsonAsync(baseurl + "api/Meter/PutTestAccount", ta);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("GetAllTestAccounts");
                        }
                        return View(ta);
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounts/Delete/5
        
        public async Task<ActionResult> DeleteTestAccount(int accountId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(baseurl + "api/Meter/GetTestAccount?accountId=" + accountId);
                if (response.IsSuccessStatusCode)
                {
                    var ta = response.Content.ReadAsAsync<TestAccount>().Result;
                    return View(ta);
                }
            }
            return View();
        }


        // POST: Accounts/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteTestAccountPOST(int accountId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.DeleteAsync(baseurl + "api/Meter/DeleteTestAccount?accountId="+accountId);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetAllTestAccounts");
                    }
                }// TODO: Add delete logic here
                return View();
                
            }
            catch
            {
                return View();
            }
        }
    }
}
