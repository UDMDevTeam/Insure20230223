using System;
using System.Data;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using UDM.Insurance.Business.Queries;

namespace UDM.Insurance.Business.Mapping
{
    public partial class INUserScheduleMapper
    {
        #region Search

        public static INUserScheduleCollection Search(long? fksystemid, long? fkuserid, long? fkinimportid, string connectionName)
        {
            INUserScheduleCollection collection = new INUserScheduleCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INUserScheduleQueries.Search(fksystemid, fkuserid, fkinimportid), null);
                while (reader.Read())
                {
                    INUserSchedule inuserschedule = new INUserSchedule((long)reader["ID"]);
                    inuserschedule.ConnectionName = connectionName;
                    collection.Add(inuserschedule);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INUserSchedule objects in the database", ex);
            }
            return collection;
        }

        #endregion
    }
}
