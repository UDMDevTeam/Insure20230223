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
    /// Contains methods to fill, save and delete callmonitoringunallocation objects.
    /// </summary>
    public partial class CallMonitoringUnallocationMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) callmonitoringunallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The id of the callmonitoringunallocation object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the callmonitoringunallocation object.</param>
        /// <returns>True if the callmonitoringunallocation object was deleted successfully, else false.</returns>
        internal static bool Delete(CallMonitoringUnallocation callmonitoringunallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.Delete(callmonitoringunallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a CallMonitoringUnallocation object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) callmonitoringunallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The id of the callmonitoringunallocation object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the callmonitoringunallocation history.</param>
        /// <returns>True if the callmonitoringunallocation history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(CallMonitoringUnallocation callmonitoringunallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.DeleteHistory(callmonitoringunallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete CallMonitoringUnallocation history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) callmonitoringunallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The id of the callmonitoringunallocation object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the callmonitoringunallocation object.</param>
        /// <returns>True if the callmonitoringunallocation object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(CallMonitoringUnallocation callmonitoringunallocation)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.UnDelete(callmonitoringunallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a CallMonitoringUnallocation object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the callmonitoringunallocation object from the data reader.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(CallMonitoringUnallocation callmonitoringunallocation, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    callmonitoringunallocation.IsLoaded = true;
                    callmonitoringunallocation.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    callmonitoringunallocation.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    callmonitoringunallocation.IsSavedCarriedForward = reader["IsSavedCarriedForward"] != DBNull.Value ? (bool)reader["IsSavedCarriedForward"] : (bool?)null;
                    callmonitoringunallocation.ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? (DateTime)reader["ExpiryDate"] : (DateTime?)null;
                    callmonitoringunallocation.AllocatedByUserID = reader["AllocatedByUserID"] != DBNull.Value ? (long)reader["AllocatedByUserID"] : (long?)null;
                    callmonitoringunallocation.CallMonitoringAllocationDate = reader["CallMonitoringAllocationDate"] != DBNull.Value ? (DateTime)reader["CallMonitoringAllocationDate"] : (DateTime?)null;
                    callmonitoringunallocation.StampDate = (DateTime)reader["StampDate"];
                    callmonitoringunallocation.HasChanged = false;
                }
                else
                {
                    throw new MapperException("CallMonitoringUnallocation does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CallMonitoringUnallocation object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an callmonitoringunallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation to fill.</param>
        internal static void Fill(CallMonitoringUnallocation callmonitoringunallocation)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.Fill(callmonitoringunallocation, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(callmonitoringunallocation, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CallMonitoringUnallocation object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a callmonitoringunallocation object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(CallMonitoringUnallocation callmonitoringunallocation)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.FillData(callmonitoringunallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a CallMonitoringUnallocation object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an callmonitoringunallocation object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation to fill from history.</param>
        internal static void FillHistory(CallMonitoringUnallocation callmonitoringunallocation, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.FillHistory(callmonitoringunallocation, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(callmonitoringunallocation, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a CallMonitoringUnallocation object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the callmonitoringunallocation objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static CallMonitoringUnallocationCollection List(bool showDeleted, string connectionName)
        {
            CallMonitoringUnallocationCollection collection = new CallMonitoringUnallocationCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? CallMonitoringUnallocationQueries.ListDeleted() : CallMonitoringUnallocationQueries.List(), null);
                while (reader.Read())
                {
                    CallMonitoringUnallocation callmonitoringunallocation = new CallMonitoringUnallocation((long)reader["ID"]);
                    callmonitoringunallocation.ConnectionName = connectionName;
                    collection.Add(callmonitoringunallocation);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list CallMonitoringUnallocation objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the callmonitoringunallocation objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? CallMonitoringUnallocationQueries.ListDeleted() : CallMonitoringUnallocationQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list CallMonitoringUnallocation objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) callmonitoringunallocation object from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(CallMonitoringUnallocation callmonitoringunallocation)
        {
            CallMonitoringUnallocationCollection collection = new CallMonitoringUnallocationCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.ListHistory(callmonitoringunallocation, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) CallMonitoringUnallocation in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an callmonitoringunallocation object to the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The CallMonitoringUnallocation object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the callmonitoringunallocation was saved successfull, otherwise, false.</returns>
        internal static bool Save(CallMonitoringUnallocation callmonitoringunallocation)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (callmonitoringunallocation.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.Save(callmonitoringunallocation, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(callmonitoringunallocation.ConnectionName, CallMonitoringUnallocationQueries.Save(callmonitoringunallocation, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            callmonitoringunallocation.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= callmonitoringunallocation.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a CallMonitoringUnallocation object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for callmonitoringunallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <param name="allocatedbyuserid">The allocatedbyuserid search criteria.</param>
        /// <param name="callmonitoringallocationdate">The callmonitoringallocationdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static CallMonitoringUnallocationCollection Search(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate, long? allocatedbyuserid, DateTime? callmonitoringallocationdate, string connectionName)
        {
            CallMonitoringUnallocationCollection collection = new CallMonitoringUnallocationCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, CallMonitoringUnallocationQueries.Search(fkinimportid, fkuserid, issavedcarriedforward, expirydate, allocatedbyuserid, callmonitoringallocationdate), null);
                while (reader.Read())
                {
                    CallMonitoringUnallocation callmonitoringunallocation = new CallMonitoringUnallocation((long)reader["ID"]);
                    callmonitoringunallocation.ConnectionName = connectionName;
                    collection.Add(callmonitoringunallocation);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CallMonitoringUnallocation objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for callmonitoringunallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <param name="allocatedbyuserid">The allocatedbyuserid search criteria.</param>
        /// <param name="callmonitoringallocationdate">The callmonitoringallocationdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate, long? allocatedbyuserid, DateTime? callmonitoringallocationdate, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, CallMonitoringUnallocationQueries.Search(fkinimportid, fkuserid, issavedcarriedforward, expirydate, allocatedbyuserid, callmonitoringallocationdate), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CallMonitoringUnallocation objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 callmonitoringunallocation objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <param name="allocatedbyuserid">The allocatedbyuserid search criteria.</param>
        /// <param name="callmonitoringallocationdate">The callmonitoringallocationdate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static CallMonitoringUnallocation SearchOne(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate, long? allocatedbyuserid, DateTime? callmonitoringallocationdate, string connectionName)
        {
            CallMonitoringUnallocation callmonitoringunallocation = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, CallMonitoringUnallocationQueries.Search(fkinimportid, fkuserid, issavedcarriedforward, expirydate, allocatedbyuserid, callmonitoringallocationdate), null);
                if (reader.Read())
                {
                    callmonitoringunallocation = new CallMonitoringUnallocation((long)reader["ID"]);
                    callmonitoringunallocation.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for CallMonitoringUnallocation objects in the database", ex);
            }
            return callmonitoringunallocation;
        }
        #endregion
    }
}
