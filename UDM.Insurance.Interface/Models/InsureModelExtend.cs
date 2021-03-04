

namespace UDM.Insurance.Interface.Models
{
    public partial class InsureEntities
    {
        public InsureEntities(string connectionString) //: base(connectionString)
        #if (DEBUG)
            : base("metadata=res://*/Models.InsureModel.csdl|res://*/Models.InsureModel.ssdl|res://*/Models.InsureModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=udmsql;initial catalog=InsureDebug;Integrated Security=False;user id=sa;password=admin;MultipleActiveResultSets=True;Application Name=EntityFramework\"")
        #elif (TESTBUILD)
            : base("metadata=res://*/Models.InsureModel.csdl|res://*/Models.InsureModel.ssdl|res://*/Models.InsureModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=udmsql;initial catalog=InsureTest;Integrated Security=False;user id=sa;password=admin;MultipleActiveResultSets=True;Application Name=EntityFramework\"")
        #elif (TRAININGBUILD)
            : base("metadata=res://*/Models.InsureModel.csdl|res://*/Models.InsureModel.ssdl|res://*/Models.InsureModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=udmsql;initial catalog=InsureTraining;Integrated Security=False;user id=sa;password=admin;MultipleActiveResultSets=True;Application Name=EntityFramework\"")
        #else
            : base("metadata=res://*/Models.InsureModel.csdl|res://*/Models.InsureModel.ssdl|res://*/Models.InsureModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=udmsql;initial catalog=Insure;Integrated Security=False;user id=sa;password=admin;MultipleActiveResultSets=True;Application Name=EntityFramework\"")
        #endif
        {


        }
    }
}
