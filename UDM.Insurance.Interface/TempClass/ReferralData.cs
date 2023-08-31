using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDM.Insurance.Interface.TempClass
{
    public class ReferralData
    {
        public string FKINImportID { get; set; }
        public string ReferralNumber { get; set; }
        public string Name { get; set; }
        public string CellNumber { get; set; }
        public long? Relationship { get; set; }
        public long? Gender { get; set; }

    }

}
