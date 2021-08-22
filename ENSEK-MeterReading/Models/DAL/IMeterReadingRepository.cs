using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK_MeterReading.Models.DAL
{
   public  interface IMeterReadingRepository
    {
        /// <summary>
        /// Meter Reading section
        /// </summary>
        /// <returns></returns>
        List<int> GetValidAccounts();        
        int PostMeterReadingForAccounts(List<MeterReading> md);



        /// <summary>
        /// testa accounts CRUD section
        /// </summary>

        List<TestAccounts> GetAllTestAccounts();
        int PutTestAccout(TestAccounts ta);
        int DeleteTestAccount(int accountId);
        int PostTestAccount(TestAccounts ta);
        TestAccounts GetTestAccount(int AccountId);
    }
}
