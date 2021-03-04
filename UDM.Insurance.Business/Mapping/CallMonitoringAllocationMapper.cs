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
    /// Contains methods to fill, save and delete callmonitoringallocation objects.
    /// </summary>
    public partial class CallMonitoringAllocationMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) callmonitoringallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The id of the callmonitoringallocation object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the callmonitoringallocation object.</param>
        /// <returns>True if the callmonitoringallocation object was deleted successfully, else false.</returns>
        internal static bool Delete(CallMonitoringAllocation callmonitoringallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.Delete(callmonitoringallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a CallMonitoringAllocation object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) callmonitoringallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The id of the callmonitoringallocation object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the callmonitoringallocation history.</param>
        /// <returns>True if the callmonitoringallocation history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(CallMonitoringAllocation callmonitoringallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.DeleteHistory(callmonitoringallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete CallMonitoringAllocation history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) callmonitoringallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The id of the callmonitoringallocation object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the callmonitoringallocation object.</param>
        /// <returns>True if the callmonitoringallocation object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(CallMonitoringAllocation callmonitoringallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.UnDelete(callmonitoringallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a CallMonitoringAllocation object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the callmonitoringallocation object from the data reader.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(CallMonitoringAllocation callmonitoringallocation, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    callmonitoringallocation.IsLoaded = true;
                    callmonitoringallocation.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    callmonitoringallocation.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    callmonitoringallocation.IsSavedCarriedForward = reader["IsSavedCarriedForward"] != DBNull.Value ? (bool)reader["IsSavedCarriedForward"] : (bool?)null;
                    callmonitoringallocation.ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? (DateTime)reader["ExpiryDate"] : (DateTime?)null;
                    callmonitoringallocation.StampDate = (DateTime)reader["StampDate"];
                    callmonitoringallocation.HasChanged = false;
                }
                else
                {
                    throw new MapperException("CallMonitoringAllocation does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CallMonitoringAllocation object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an callmonitoringallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation to fill.</param>
        internal static void Fill(CallMonitoringAllocation callmonitoringallocation)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.Fill(callmonitoringallocation, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(callmonitoringallocation, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CallMonitoringAllocation object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a callmonitoringallocation object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(CallMonitoringAllocation callmonitoringallocation)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.FillData(callmonitoringallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a CallMonitoringAllocation object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an callmonitoringallocation object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="callmonitoringallocation">The callmonitoringallocation to fill from history.</param>
        internal static void FillHistory(CallMonitoringAllocation callmonitoringallocation, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.FillHistory(callmonitoringallocation, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(callmonitoringallocation, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CallMonitoringAllocation object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the callmonitoringallocation objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static CallMonitoringAllocationCollection List(bool showDeleted, string connectionName)
        {
            CallMonitoringAllocationCollection collection = new CallMonitoringAllocationCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? CallMonitoringAllocationQueries.ListDeleted() : CallMonitoringAllocationQueries.List(), null);
                while (reader.Read())
                {
                    CallMonitoringAllocation callmonitoringallocation = new CallMonitoringAllocation((long)reader["ID"]);
                    callmonitoringallocation.ConnectionName = connectionName;
                    collection.Add(callmonitoringallocation);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list CallMonitoringAllocation objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the callmonitoringallocation objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? CallMonitoringAllocationQueries.ListDeleted() : CallMonitoringAllocationQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list CallMonitoringAllocation objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) callmonitoringallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(CallMonitoringAllocation callmonitoringallocation)
        {
            CallMonitoringAllocationCollection collection = new CallMonitoringAllocationCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.ListHistory(callmonitoringallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) CallMonitoringAllocation in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an callmonitoringallocation object to the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The CallMonitoringAllocation object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the callmonitoringallocation was saved successfull, otherwise, false.</returns>
        internal static bool Save(CallMonitoringAllocation callmonitoringallocation)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (callmonitoringallocation.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.Save(callmonitoringallocation, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(callmonitoringallocation.ConnectionName, CallMonitoringAllocationQueries.Save(callmonitoringallocation, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            callmonitoringallocation.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= callmonitoringallocation.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a CallMonitoringAllocation object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for callmonitoringallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static CallMonitoringAllocationCollection Search(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate, string connectionName)
        {
            CallMonitoringAllocationCollection collection = new CallMonitoringAllocationCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, CallMonitoringAllocationQueries.Search(fkinimportid, fkuserid, issavedcarriedforward, expirydate), null);
                while (reader.Read())
                {
                    CallMonitoringAllocation callmonitoringallocation = new CallMonitoringAllocation((long)reader["ID"]);
                    callmonitoringallocation.ConnectionName = connectionName;
                    collection.Add(callmonitoringallocation);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CallMonitoringAllocation objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for callmonitoringallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, CallMonitoringAllocationQueries.Search(fkinimportid, fkuserid, issavedcarriedforward, expirydate), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CallMonitoringAllocation objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 callmonitoringallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static CallMonitoringAllocation SearchOne(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate, string connectionName)
        {
            CallMonitoringAllocation callmonitoringallocation = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, CallMonitoringAllocationQueries.Search(fkinimportid, fkuserid, issavedcarriedforward, expirydate), null);
                if (reader.Read())
                {
                    callmonitoringallocation = new CallMonitoringAllocation((long)reader["ID"]);
                    callmonitoringallocation.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CallMonitoringAllocation objects in the database", ex);
            }
            return callmonitoringallocation;
        }
        #endregion
    }
}
