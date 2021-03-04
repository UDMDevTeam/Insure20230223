using System;
using System.Data;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains custom methods to fill, save and delete inpolicy objects.
    /// </summary>
    public partial class INPolicyMapper
    {
        #region Search

        public static INPolicy SearchOne(string policyid, string connectionName)
        {
            INPolicy inpolicy = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyQueries.Search(policyid), null);
                if (reader.Read())
                {
                    inpolicy = new INPolicy((long)reader["ID"]);
                    inpolicy.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicy objects in the database", ex);
            }
            return inpolicy;
        }

        #endregion
    }
}
