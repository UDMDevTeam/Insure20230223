using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDM.Insurance.Interface.Data
{
    public class ActivityDetail
    {
        public string ReferenceNumber { get; set; }
        public DateTime ActivityDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string FinalDescription { get; set; }
    }
}
