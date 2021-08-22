using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ENSEK_EnergySupplierClient.Controllers
{
    public class MeterReadingController : Controller
    {
        string baseurl = "https://localhost:44329/";

        public ActionResult PostBulkMeterReadingFileToAPI()
        {
            return View();
        }
        // GET: MeterReading
        [HttpPost]
        public ActionResult PostBulkMeterReadingFileToAPI(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        byte[] bytes = new byte[file.InputStream.Length - 1];
                        file.InputStream.Read(bytes, 0, bytes.Length);
                        var filecontent = new ByteArrayContent(bytes);
                        filecontent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                        content.Add(filecontent);
                        var result = client.PostAsync(baseurl + "api/Meter/PostBulkMeterReading", content).Result;
                        if (result.IsSuccessStatusCode)
                        {
                           ViewBag.Response= result.Content.ReadAsStringAsync().Result;
                            return View();
                        }
                    }
                }
            }
            return View();
        }
    }
}