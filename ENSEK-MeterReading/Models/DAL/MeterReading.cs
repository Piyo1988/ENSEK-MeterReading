using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENSEK_MeterReading.Models.DAL
{
    public class MeterReading
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Nullable<System.DateTime> MeterReadingDateTime { get; set; }
        public Nullable<int> MeterReadValue { get; set; }
        
    }
}