using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;

using UDM.Insurance.Business;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business.Mapping
{
    public partial class INGiftRedeemMapper
    {

        #region Search
        
        public static INGiftRedeem SearchOne(long? fkinimportid, string connectionName)
        {
            INGiftRedeem ingiftredeem = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INGiftRedeemQueries.Search(fkinimportid, null, null, null, null, null, null, null), null);
                if (reader.Read())
                {
                    ingiftredeem = new INGiftRedeem((long)reader["ID"]);
                    ingiftredeem.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INGiftRedeem objects in the database", ex);
            }

            return ingiftredeem;
        }
        
        #endregion

    }
}
