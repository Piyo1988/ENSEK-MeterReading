using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ENSEK_MeterReading.Models.DAL;
using ENSEK_MeterReading.Models.BL;

namespace ENSEK_MeterReading.Controllers
{
    public class MeterController : ApiController
    {
        IMeterReadingService BLservice = null;

        //public MeterController()
        //{ }
        public MeterController(IMeterReadingService _service)
        {
            BLservice = _service;
        }
       
        // GET api/<controller>   
        [HttpGet]
        public IHttpActionResult GetAllTestAccounts()
        {
            List<TestAccounts> accountlist = new List<TestAccounts>();
            try
            {
                accountlist = BLservice.GetAllTestAccounts();
                if (accountlist != null && accountlist.Count > 0)               
                    return Ok(accountlist);                
                else
                    return NotFound();           
            }
            catch(Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));
            }
        }

        public IHttpActionResult GetTestAccount(int accountId)
        {
            TestAccounts account = new TestAccounts();
            try
            {
                account = BLservice.GetTestAccount(accountId);
                if (account != null)
                    return Ok(account);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));
            }
        }

        // POST api/<controller>
        public IHttpActionResult PostTestAccount([FromBody] TestAccounts ta)
        {
            int result;
            try
            {
                if(ta!=null)
                {
                   result= BLservice.PostTestAccount(ta);
                    if(result>0)                    
                        return Ok(result);
                    else
                        return BadRequest("Record with Account ID - " + ta.AccountId + "not inserted into the database");
                }
                else
                    return BadRequest("No Record available to be inserted into the database");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));
            }
        }

        public IHttpActionResult PutTestAccount([FromBody] TestAccounts ta)
        {
            int result;
            try
            {
                if (ta != null)
                {
                    result = BLservice.PutTestAccout(ta);
                    if (result > 0)
                        return Ok(result);
                    else
                        return BadRequest("Record with Account ID - " + ta.AccountId + "not updated into the database");
                }
                else
                    return BadRequest("No Record available to be updated into the database");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Unable to fetch Accounts information from the database. Error Message- {0}", ex.Message));
            }
        }

        
        public IHttpActionResult DeleteTestAccount(int accountId)
        {
            int result;
            try
            {
                result = BLservice.DeleteTestAccount(accountId);
                if (result !=-1)
                    return Ok(result);
                else
                    return BadRequest("Record with Account ID - " + accountId + "not deleted in the database");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));
               
            }
        }
        public void Post([FromBody] string value)
        {
            
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        public async Task<string> PostBulkMeterReading()
        {

            string fileToBeRead = string.Empty;
            List<MeterReading> md = null;           
            
            int successfullReadingCnt = 0;         

            if (Request.Content.IsMimeMultipartContent("form-data"))
            {
                var serverUploadPath = HttpContext.Current.Server.MapPath(@"~/FileUploads");

                //deleting old files if any
                Array.ForEach(Directory.GetFiles(serverUploadPath), File.Delete);

                var provider = new MultipartFileStreamProvider(serverUploadPath);
                
                try
                {
                    //Saving the meter reading file to the server
                    await Request.Content.ReadAsMultipartAsync(provider);

                    foreach (var file in provider.FileData)
                    {
                        var filename = file.Headers.ContentDisposition.FileName.Trim('"');
                        var localfilename = file.LocalFileName;
                        var newFileNameWithfullfilepath = Path.Combine(serverUploadPath, filename);
                        File.Move(localfilename, newFileNameWithfullfilepath);

                        fileToBeRead = newFileNameWithfullfilepath;
                    }

                    //if file is present, read each line and validate it and increment the counter for correct/wrong records
                    if (!String.IsNullOrEmpty(fileToBeRead))
                    {
                        var filecontents = File.ReadAllLines(fileToBeRead);

                        if (filecontents.Length > 0)
                        {
                            int accountID;
                            DateTime meterReadingTime;
                            int metervalue;
                            md = new List<MeterReading>();

                            //Fetch all the valid Accounts from the Sql Database to validate accross the meter reading data
                            List<int> validAccounts = BLservice.GetValidAccounts();

                            foreach (var eachline in filecontents)
                            {
                                string[] eachcolumn = eachline.Split(',');

                                //Validate data                               
                                if (int.TryParse(eachcolumn[0], out accountID) && validAccounts.IndexOf(accountID)!=-1 && DateTime.TryParse(eachcolumn[1], out meterReadingTime) && int.TryParse(Regex.IsMatch(eachcolumn[2], @"^[0-9]+$") ?eachcolumn[2].ToString().PadLeft(5,'0'):null, out metervalue))
                                {                                      
                                    if (md != null)
                                    {
                                        var meterreadingrecord = md.FirstOrDefault(row => (row.AccountId == accountID) && (row.MeterReadingDateTime == meterReadingTime) && (row.MeterReadValue == metervalue));
                                        if (meterreadingrecord == null)
                                            md.Add(new MeterReading() { AccountId = accountID, MeterReadingDateTime = meterReadingTime, MeterReadValue = metervalue }); 
                                    }
                                }                               
                            }                           

                            //remove the file after its use and use the list object instead for further processing
                            File.Delete(fileToBeRead);

                            //Push only the correct records to Sql Database through a call to DAL method 
                            if(md.Count>0)
                            {
                              successfullReadingCnt = BLservice.PostMeterReadingForAccounts(md);
                                if (successfullReadingCnt > 0)
                                {
                                    return string.Format("The number of successful readings - {0} and failed readings - {1}", successfullReadingCnt, filecontents.Length - successfullReadingCnt);
                                }
                                else
                                {
                                    return string.Format("No new successful readings recorded- "+Environment.NewLine+"POSSIBLE REASONS-1. Same records exists in database +"+Environment.NewLine+"2. Invalid records ");
                                }
                            }
                        }
                        else
                        {
                            return string.Format("The file {0} is empty and cannot be processed", fileToBeRead);
                        }
                    }
                    else
                    {
                        return string.Format("The file {0} is empty and cannot be processed",fileToBeRead);
                    }
                    
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
               
            }
            else
            {
                return "No form-data is included in the request to process";
            }
            return null;
        }
    }
}