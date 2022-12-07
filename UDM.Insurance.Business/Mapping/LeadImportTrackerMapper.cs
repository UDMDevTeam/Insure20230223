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
    public partial class LeadImportTrackerMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the calldata object.</param>
        /// <returns>True if the calldata object was deleted successfully, else false.</returns>
        internal static bool Delete(LeadImportTracker calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, LeadImportTrackerQueries.Delete(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a LeadImportTracker object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the calldata history.</param>
        /// <returns>True if the calldata history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(LeadImportTracker calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, LeadImportTrackerQueries.DeleteHistory(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete LeadImportTracker history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The id of the calldata object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the calldata object.</param>
        /// <returns>True if the calldata object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(LeadImportTracker calldata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(calldata.ConnectionName, LeadImportTrackerQueries.UnDelete(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a LeadImportTracker object from the database", ex);
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
        private static void Fill(LeadImportTracker calldata, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    calldata.IsLoaded = true;
                    calldata.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    calldata.FKINBatchID = reader["FKINBatchID"] != DBNull.Value ? (long)reader["FKINBatchID"] : (long?)null;
                    calldata.ConservedStatus = reader["ConservedStatus"] != DBNull.Value ? (string)reader["ConservedStatus"] : (string)null;
                    calldata.StampDate = (DateTime)reader["StampDate"];
                    calldata.HasChanged = false;
                }
                else
                {
                    throw new MapperException("LeadImportTracker does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a LeadImportTracker object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an calldata object from the database.
        /// </summary>
        /// <param name="calldata">The calldata to fill.</param>
        internal static void Fill(LeadImportTracker calldata)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, LeadImportTrackerQueries.Fill(calldata, ref parameters), parameters);
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
                throw new MapperException("Failed to populate a CallData object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a calldata object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(LeadImportTracker calldata)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(calldata.ConnectionName, LeadImportTrackerQueries.FillData(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a LeadImportTracker object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an calldata object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="calldata">The calldata to fill from history.</param>
        internal static void FillHistory(LeadImportTracker calldata, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, LeadImportTrackerQueries.FillHistory(calldata, stampUserID, stampDate, ref parameters), parameters);
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
                throw new MapperException("Failed to populate a LeadImportTracker object from the database", ex);
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
        public static LeadImportTrackerCollection List(bool showDeleted, string connectionName)
        {
            LeadImportTrackerCollection collection = new LeadImportTrackerCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? LeadImportTrackerQueries.ListDeleted() : LeadImportTrackerQueries.List(), null);
                while (reader.Read())
                {
                    LeadImportTracker calldata = new LeadImportTracker((long)reader["ID"]);
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
                throw new MapperException("Failed to list LeadImportTracker objects in the database", ex);
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
                return Database.ExecuteDataSet(connectionName, showDeleted ? LeadImportTrackerQueries.ListDeleted() : LeadImportTrackerQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list LeadImportTracker objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) calldata object from the database.
        /// </summary>
        /// <param name="calldata">The calldata to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(LeadImportTracker calldata)
        {
            LeadImportTrackerCollection collection = new LeadImportTrackerCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(calldata.ConnectionName, LeadImportTrackerQueries.ListHistory(calldata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) LeadImportTracker in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an calldata object to the database.
        /// </summary>
        /// <param name="calldata">The CallData object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the calldata was saved successfull, otherwise, false.</returns>
        internal static bool Save(LeadImportTracker calldata)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (calldata.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(calldata.ConnectionName, LeadImportTrackerQueries.Save(calldata, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(calldata.ConnectionName, LeadImportTrackerQueries.Save(calldata, ref parameters), parameters);
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
                throw new MapperException("Failed to save a LeadImportTracker object to the database", ex);
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
        public static LeadImportTrackerCollection Search(long? fkimportid, long? fkinbatchid, string connectionName)
        {
            LeadImportTrackerCollection collection = new LeadImportTrackerCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, LeadImportTrackerQueries.Search(fkimportid, fkinbatchid), null);
                while (reader.Read())
                {
                    LeadImportTracker calldata = new LeadImportTracker((long)reader["ID"]);
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
                throw new MapperException("Failed to search for LeadImportTracker objects in the database", ex);
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
        public static DataSet SearchData(long? fkimportid, long? fkinbatchid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, LeadImportTrackerQueries.Search(fkimportid, fkinbatchid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for LeadImportTracker objects in the database", ex);
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
        public static LeadImportTracker SearchOne(long? fkimportid, long? fkibatchid, string connectionName)
        {
            LeadImportTracker calldata = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, LeadImportTrackerQueries.Search(fkimportid, fkibatchid), null);
                if (reader.Read())
                {
                    calldata = new LeadImportTracker((long)reader["ID"]);
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
                throw new MapperException("Failed to search for LeadImportTracker objects in the database", ex);
            }
            return calldata;
        }
        #endregion
    }
}
