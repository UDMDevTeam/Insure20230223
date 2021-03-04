using System;
using System.Data;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains custom methods to fill, save and delete inlead objects.
    /// </summary>
    public partial class INLeadMapper
    {
        #region Search

        public static INLead SearchOne(string idno, string connectionName)
        {
            INLead inlead = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadQueries.Search(idno), null);
                if (reader.Read())
                {
                    inlead = new INLead((long)reader["ID"]);
                    inlead.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLead objects in the database", ex);
            }
            return inlead;
        }

        public static INLead SearchOne(string firstname, string surname, DateTime? dateofbirth, string connectionName)
        {
            INLead inlead = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadQueries.Search(firstname, surname, dateofbirth), null);
                if (reader.Read())
                {
                    inlead = new INLead((long)reader["ID"]);
                    inlead.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLead objects in the database", ex);
            }
            return inlead;
        }

        public static INLead SearchOne(string firstname, string surname, string strTelCell, string connectionName)
        {
            INLead inlead = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadQueries.Search(firstname, surname, strTelCell), null);
                if (reader.Read())
                {
                    inlead = new INLead((long)reader["ID"]);
                    inlead.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLead objects in the database", ex);
            }
            return inlead;
        }

        #endregion
    }
}
