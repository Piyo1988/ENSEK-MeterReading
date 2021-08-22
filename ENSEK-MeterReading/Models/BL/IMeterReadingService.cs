using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENSEK_MeterReading.Models.DAL;

namespace ENSEK_MeterReading.Models.BL
{
    public interface IMeterReadingService
    {
        /// <summary>
        /// Meter Reading section
        /// </summary>
        /// <returns></returns>
        List<int> GetValidAccounts();     
        int PostMeterReadingForAccounts(List<MeterReading> md);

        /// <summary>
        /// Test account CRUD Section
        /// </summary>
        /// <returns></returns>

        List<TestAccounts> GetAllTestAccounts();
        TestAccounts GetTestAccount(int AccountId);
        int PutTestAccout(TestAccounts ta);
        int DeleteTestAccount(int accountId);
        int PostTestAccount(TestAccounts ta);

    }
}
