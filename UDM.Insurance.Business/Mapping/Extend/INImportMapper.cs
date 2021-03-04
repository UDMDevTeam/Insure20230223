using System;
using System.Data;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains custom methods to fill, save and delete INImport objects.
    /// </summary>
    public partial class INImportMapper
    {
        #region Search

        public static INImportCollection Search(long? fkuserid, bool? isCopied, long? fkbatchid, string connectionName)
        {
            INImportCollection collection = new INImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportQueries.Search(fkuserid, isCopied, fkbatchid), null);
                while (reader.Read())
                {
                    INImport inImport = new INImport((long)reader["ID"]);
                    inImport.ConnectionName = connectionName;
                    collection.Add(inImport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImport objects in the database", ex);
            }
            return collection;
        }

        public static INImportCollection Search(long? fkuserid, long? fkbatchid, string connectionName)
        {
            INImportCollection collection = new INImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportQueries.Search(fkuserid, fkbatchid), null);
                while (reader.Read())
                {
                    INImport inImport = new INImport((long)reader["ID"]);
                    inImport.ConnectionName = connectionName;
                    collection.Add(inImport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImport objects in the database", ex);
            }
            return collection;
        }

        public static INImportCollection Search(long? fkuserid, long? fkbatchid, bool? isprinted, string connectionName)
        {
            INImportCollection collection = new INImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportQueries.Search(fkuserid, fkbatchid, isprinted), null);
                while (reader.Read())
                {
                    INImport inImport = new INImport((long)reader["ID"]);
                    inImport.ConnectionName = connectionName;
                    collection.Add(inImport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImport objects in the database", ex);
            }
            return collection;
        }

        #endregion
    }
}
