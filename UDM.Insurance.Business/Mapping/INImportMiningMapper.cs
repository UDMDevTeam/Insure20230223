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
    /// <summary>
    /// Contains methods to fill, save and delete inimportmining objects.
    /// </summary>
    public partial class INImportMiningMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportmining object from the database.
        /// </summary>
        /// <param name="inimportmining">The id of the inimportmining object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportmining object.</param>
        /// <returns>True if the inimportmining object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportMining inimportmining)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportmining.ConnectionName, INImportMiningQueries.Delete(inimportmining, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportMining object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportmining object from the database.
        /// </summary>
        /// <param name="inimportmining">The id of the inimportmining object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportmining history.</param>
        /// <returns>True if the inimportmining history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportMining inimportmining)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportmining.ConnectionName, INImportMiningQueries.DeleteHistory(inimportmining, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportMining history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportmining object from the database.
        /// </summary>
        /// <param name="inimportmining">The id of the inimportmining object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportmining object.</param>
        /// <returns>True if the inimportmining object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportMining inimportmining)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportmining.ConnectionName, INImportMiningQueries.UnDelete(inimportmining, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportMining object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportmining object from the data reader.
        /// </summary>
        /// <param name="inimportmining">The inimportmining object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportMining inimportmining, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportmining.IsLoaded = true;
                    inimportmining.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    inimportmining.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    inimportmining.Rank = reader["Rank"] != DBNull.Value ? (short)reader["Rank"] : (short?)null;
                    inimportmining.AllocationDate = reader["AllocationDate"] != DBNull.Value ? (DateTime)reader["AllocationDate"] : (DateTime?)null;
                    inimportmining.StampDate = (DateTime)reader["StampDate"];
                    inimportmining.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportMining does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportMining object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportmining object from the database.
        /// </summary>
        /// <param name="inimportmining">The inimportmining to fill.</param>
        internal static void Fill(INImportMining inimportmining)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportmining.ConnectionName, INImportMiningQueries.Fill(inimportmining, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportmining, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportMining object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportmining object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportMining inimportmining)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportmining.ConnectionName, INImportMiningQueries.FillData(inimportmining, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportMining object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportmining object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportmining">The inimportmining to fill from history.</param>
        internal static void FillHistory(INImportMining inimportmining, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportmining.ConnectionName, INImportMiningQueries.FillHistory(inimportmining, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportmining, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportMining object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportmining objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportMiningCollection List(bool showDeleted, string connectionName)
        {
            INImportMiningCollection collection = new INImportMiningCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportMiningQueries.ListDeleted() : INImportMiningQueries.List(), null);
                while (reader.Read())
                {
                    INImportMining inimportmining = new INImportMining((long)reader["ID"]);
                    inimportmining.ConnectionName = connectionName;
                    collection.Add(inimportmining);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportMining objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportmining objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportMiningQueries.ListDeleted() : INImportMiningQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportMining objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportmining object from the database.
        /// </summary>
        /// <param name="inimportmining">The inimportmining to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportMining inimportmining)
        {
            INImportMiningCollection collection = new INImportMiningCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportmining.ConnectionName, INImportMiningQueries.ListHistory(inimportmining, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportMining in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportmining object to the database.
        /// </summary>
        /// <param name="inimportmining">The INImportMining object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportmining was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportMining inimportmining)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportmining.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportmining.ConnectionName, INImportMiningQueries.Save(inimportmining, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportmining.ConnectionName, INImportMiningQueries.Save(inimportmining, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportmining.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportmining.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportMining object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportmining objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="allocationdate">The allocationdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportMiningCollection Search(long? fkimportid, long? fkuserid, short? rank, DateTime? allocationdate, string connectionName)
        {
            INImportMiningCollection collection = new INImportMiningCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportMiningQueries.Search(fkimportid, fkuserid, rank, allocationdate), null);
                while (reader.Read())
                {
                    INImportMining inimportmining = new INImportMining((long)reader["ID"]);
                    inimportmining.ConnectionName = connectionName;
                    collection.Add(inimportmining);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportMining objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportmining objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="allocationdate">The allocationdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkimportid, long? fkuserid, short? rank, DateTime? allocationdate, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportMiningQueries.Search(fkimportid, fkuserid, rank, allocationdate), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportMining objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportmining objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="allocationdate">The allocationdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportMining SearchOne(long? fkimportid, long? fkuserid, short? rank, DateTime? allocationdate, string connectionName)
        {
            INImportMining inimportmining = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportMiningQueries.Search(fkimportid, fkuserid, rank, allocationdate), null);
                if (reader.Read())
                {
                    inimportmining = new INImportMining((long)reader["ID"]);
                    inimportmining.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportMining objects in the database", ex);
            }
            return inimportmining;
        }
        #endregion
    }
}
