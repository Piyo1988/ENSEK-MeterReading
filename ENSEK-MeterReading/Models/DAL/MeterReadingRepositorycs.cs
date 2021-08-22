using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ENSEK_MeterReading.Models.DAL
{
    public class MeterReadingRepositorycs:IMeterReadingRepository
    {
        public int DeleteTestAccount(int accountId)
        {
            int recordsaffected = 0;
            try
            {
                using (var dbcontext = new ENSEKMeterReadingEntities())
                {
                    var ta = dbcontext.Test_Accounts.Where(s => s.AccountId == accountId).FirstOrDefault();
                    dbcontext.Entry(ta).State = System.Data.EntityState.Deleted;
                    dbcontext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex; //error logging not implemented yet
            }
            return recordsaffected;
        }
        public int PutTestAccout(TestAccounts ta)
        {
            int recordsaffected = 0;
            try
            {
                if (ta != null)
                {
                    using (var dbcontext = new ENSEKMeterReadingEntities())
                    {
                        var rowinDB = dbcontext.Test_Accounts.Where(s => (s.AccountId == ta.AccountId) && (s.FirstName == ta.FirstName && (s.LastName == ta.LastName))).FirstOrDefault() ;
                        if (rowinDB != null)
                        {
                            //rowinDB.AccountId = ta.AccountId;
                            rowinDB.FirstName = ta.FirstName;
                            rowinDB.LastName = ta.LastName;
                          
                            dbcontext.Test_Accounts.Add(rowinDB);
                            recordsaffected = dbcontext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; //error logging not implemented yet
            }
            return recordsaffected;
        }
        public int PostTestAccount(TestAccounts ta)
        {
            int recordsaffected = 0;
            try
            {
                if(ta!=null)
                {
                    using (var dbcontext = new ENSEKMeterReadingEntities())
                    {
                        var testaccountDBinstance = new Test_Accounts() { AccountId = ta.AccountId, FirstName = ta.FirstName, LastName = ta.LastName };
                        dbcontext.Test_Accounts.Add(testaccountDBinstance);
                       recordsaffected= dbcontext.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex; //error logging not implemented yet
            }
            return recordsaffected;
        }
        public List<TestAccounts> GetAllTestAccounts()
        {
            List<TestAccounts> ta = new List<TestAccounts>();

            try
            {
                using (var dbcontext = new ENSEKMeterReadingEntities())
                {
                    var accountlist = dbcontext.Test_Accounts.Select(s => s).ToList();
                    if (accountlist != null && accountlist.Count > 0)
                    {
                        foreach (var item in accountlist)
                        {
                            ta.Add(new TestAccounts { AccountId = item.AccountId, FirstName = item.FirstName, LastName = item.LastName });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex; //error logging not implemented yet
            }
            return ta;
        }

        public TestAccounts GetTestAccount(int AccountId)
        {
            TestAccounts ta = null;
            try
            {
                using (var dbcontext = new ENSEKMeterReadingEntities())
                {
                    
                    var account = dbcontext.Test_Accounts.Where(s => s.AccountId == AccountId).Select(s => s).FirstOrDefault<Test_Accounts>();
                    if (account != null)
                        ta = new TestAccounts() { AccountId = account.AccountId, FirstName = account.FirstName, LastName = account.LastName };

                }
            }
            catch(Exception ex)
            {
                throw ex; //error logging not implemented yet
            }
            return ta;
        }

        public List<int> GetValidAccounts()
        {
            using(var dbcontext=new ENSEKMeterReadingEntities())
            {
                List<int> ValidAccounts = new List<int>();
                try
                {
                    ValidAccounts= dbcontext.Test_Accounts.Select(r => r.AccountId).Distinct().ToList();
                }
                catch(Exception ex)
                {
                    throw ex; //error logging not implemented yet
                }
                return ValidAccounts;
            }
        }
        public int PostMeterReadingForAccounts(List<MeterReading> md)
        {            
            int result = 0;

            try
            {
                if (md != null && md.Count > 0)
                {
                    using (var dbcontext = new ENSEKMeterReadingEntities())
                    {

                        foreach (var row in md)
                        {
                            var duplicaterecord = dbcontext.Meter_Reading.FirstOrDefault(val => (val.AccountId == row.AccountId) && (val.MeterReadingDateTime == row.MeterReadingDateTime) && (val.MeterReadValue == row.MeterReadValue));

                            if (duplicaterecord == null)
                            {
                                var meter_readingEntity = new Meter_Reading { AccountId = row.AccountId, MeterReadingDateTime = row.MeterReadingDateTime, MeterReadValue = row.MeterReadValue };
                                dbcontext.Meter_Reading.Add(meter_readingEntity);
                            }

                        }
                        result = dbcontext.SaveChanges();
                    }
                }
                else
                {
                    return result;
                }
            }
            catch(Exception ex)
            {
                throw ex; //error logging not implemented yet
            }
            return result;

        }

       
    }
}