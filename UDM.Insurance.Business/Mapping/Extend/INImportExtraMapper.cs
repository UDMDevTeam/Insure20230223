using System;
using System.Data;
using Embriant.Framework.Data;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Exceptions;

namespace UDM.Insurance.Business.Mapping
{
    public partial class INImportExtraMapper
    {
        #region Search

        public static INImportExtra SearchOne(long? fkinimportid, string connectionName)
        {
            INImportExtra inimportextra = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportExtraQueries.Search(fkinimportid), null);
                if (reader.Read())
                {
                    inimportextra = new INImportExtra((long)reader["ID"]);
                    inimportextra.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportExtra objects in the database", ex);
            }
            return inimportextra;
        }

        #endregion
    }
}
