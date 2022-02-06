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
    /// Contains methods to fill, save and delete calldata objects.
    /// </summary>
    public partial class SalesToCallMonitoringMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) calldata object from the database.
        /// </summary>
        /// <param name="SalesToCallMonitoring">The id of the calldata object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the calldata object.</param>
        /// <returns>True if the calldata object was deleted successfully, else false.</returns>
        internal static bool Delete(SalesToCallMonitoring calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, SalesToCallMonitoringQueries.Delete(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a SalesToCallMonitoring object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the calldata history.</param>
        /// <returns>True if the calldata history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(SalesToCallMonitoring calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, SalesToCallMonitoringQueries.DeleteHistory(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete SalesToCallMonitoring history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the calldata object.</param>
        /// <returns>True if the calldata object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(SalesToCallMonitoring calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, SalesToCallMonitoringQueries.UnDelete(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a SalesToCallMonitoring object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the calldata object from the data reader.
        /// </summary>
        /// <param name="calldata">The calldata object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(SalesToCallMonitoring calldata, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    calldata.IsLoaded = true;
                    calldata.FKImportID = reader["FKImportID"] != DBNull.Value ? (long)reader["FKImportID"] : (long?)null;
                    calldata.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    calldata.IsDisplayed = reader["IsDisplayed"] != DBNull.Value ? (string)reader["IsDisplayed"] : (string)null;
                    calldata.StampDate = (DateTime)reader["StampDate"];
                    calldata.HasChanged = false;
                }
                else
                {
                    throw new MapperException("SalesToCallMonitoring does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SalesToCallMonitoring object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an calldata object from the database.
        /// </summary>
        /// <param name="calldata">The calldata to fill.</param>
        internal static void Fill(SalesToCallMonitoring calldata)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, SalesToCallMonitoringQueries.Fill(calldata, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(calldata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SalesToCallMonitoring object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a calldata object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(SalesToCallMonitoring calldata)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(calldata.ConnectionName, SalesToCallMonitoringQueries.FillData(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a SalesToCallMonitoring object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an calldata object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="calldata">The calldata to fill from history.</param>
        internal static void FillHistory(SalesToCallMonitoring calldata, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, SalesToCallMonitoringQueries.FillHistory(calldata, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(calldata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a SalesToCallMonitoring object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the calldata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static SalesToCallMonitoringCollection List(bool showDeleted, string connectionName)
        {
            SalesToCallMonitoringCollection collection = new SalesToCallMonitoringCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? SalesToCallMonitoringQueries.ListDeleted() : SalesToCallMonitoringQueries.List(), null);
                while (reader.Read())
                {
                    SalesToCallMonitoring calldata = new SalesToCallMonitoring((long)reader["ID"]);
                    calldata.ConnectionName = connectionName;
                    collection.Add(calldata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list SalesToCallMonitoring objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the calldata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? SalesToCallMonitoringQueries.ListDeleted() : SalesToCallMonitoringQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list SalesToCallMonitoring objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The calldata to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(SalesToCallMonitoring calldata)
        {
            SalesToCallMonitoringCollection collection = new SalesToCallMonitoringCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(calldata.ConnectionName, SalesToCallMonitoringQueries.ListHistory(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) SalesToCallMonitoring in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an calldata object to the database.
        /// </summary>
        /// <param name="calldata">The SalesToCallMonitoring object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the calldata was saved successfull, otherwise, false.</returns>
        internal static bool Save(SalesToCallMonitoring calldata)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (calldata.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(calldata.ConnectionName, SalesToCallMonitoringQueries.Save(calldata, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, SalesToCallMonitoringQueries.Save(calldata, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            calldata.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= calldata.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a SalesToCallMonitoring object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for calldata objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static SalesToCallMonitoringCollection Search(long? fkimportid, long? fkuserid, string isdisplayed, string connectionName)
        {
            SalesToCallMonitoringCollection collection = new SalesToCallMonitoringCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, SalesToCallMonitoringQueries.Search(fkimportid, fkuserid, isdisplayed), null);
                while (reader.Read())
                {
                    SalesToCallMonitoring calldata = new SalesToCallMonitoring((long)reader["ID"]);
                    calldata.ConnectionName = connectionName;
                    collection.Add(calldata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SalesToCallMonitoring objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for calldata objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkimportid, long? fkuserid, string isdisplayed, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, SalesToCallMonitoringQueries.Search(fkimportid, fkuserid, isdisplayed), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SalesToCallMonitoring objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 calldata objects in the database.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static SalesToCallMonitoring SearchOne(long? fkimportid, long? fkuserid, string isdisplayed,  string connectionName)
        {
            SalesToCallMonitoring calldata = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, SalesToCallMonitoringQueries.Search(fkimportid, fkuserid, isdisplayed), null);
                if (reader.Read())
                {
                    calldata = new SalesToCallMonitoring((long)reader["ID"]);
                    calldata.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for SalesToCallMonitoring objects in the database", ex);
            }
            return calldata;
        }
        #endregion
    }
}
