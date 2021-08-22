using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ENSEK_MeterReading.Models.DAL;

namespace ENSEK_MeterReading.Models.BL
{
    public class MeterReadingService:IMeterReadingService
    {
        IMeterReadingRepository repo = null;
        public MeterReadingService(IMeterReadingRepository repo)
        {
            this.repo = repo;
        }

        public int PostMeterReadingForAccounts(List<MeterReading> md)
        {
            return repo.PostMeterReadingForAccounts(md);
        }

        public List<int> GetValidAccounts()
        {
            return repo.GetValidAccounts();
        }        

        public List<TestAccounts> GetAllTestAccounts()
        {
            return repo.GetAllTestAccounts();
        }

        public TestAccounts GetTestAccount(int AccountId)
        {
            return repo.GetTestAccount(AccountId);
        }

        public int PutTestAccout(TestAccounts ta)
        {
            return repo.PutTestAccout(ta);
        }

        public int DeleteTestAccount(int accountId)
        {
            return repo.DeleteTestAccount(accountId);
        }

        public int PostTestAccount(TestAccounts ta)
        {
            throw new NotImplementedException();
        }
    }
}