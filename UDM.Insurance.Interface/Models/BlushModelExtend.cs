

namespace UDM.Insurance.Interface.Models
{
    public partial class BlushEntities
    {
        public BlushEntities(string connectionString) //: base(connectionString)
        #if (DEBUG)
            : base("metadata=res://*/Models.BlushModel.csdl|res://*/Models.BlushModel.ssdl|res://*/Models.BlushModel.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source=udmsql;Initial Catalog=BlushDebug;Integrated Security=False;user id=sa;password=admin;MultipleActiveResultSets=True;Application Name=EntityFramework\"")
        #elif (TESTBUILD)
            : base("metadata=res://*/Models.BlushModel.csdl|res://*/Models.BlushModel.ssdl|res://*/Models.BlushModel.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source=udmsql;Initial Catalog=BlushTest;Integrated Security=False;user id=sa;password=admin;MultipleActiveResultSets=True;Application Name=EntityFramework\"")
        #else
            : base("metadata=res://*/Models.BlushModel.csdl|res://*/Models.BlushModel.ssdl|res://*/Models.BlushModel.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source=udmsql;Initial Catalog=Blush;Integrated Security=False;user id=sa;password=admin;MultipleActiveResultSets=True;Application Name=EntityFramework\"")
        #endif
        {


        }
    }
}
