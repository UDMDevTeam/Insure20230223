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
    /// Contains methods to fill, save and delete inimportschedules objects.
    /// </summary>
    public partial class INImportSchedulesMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportschedules object from the database.
        /// </summary>
        /// <param name="inimportschedules">The id of the inimportschedules object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportschedules object.</param>
        /// <returns>True if the inimportschedules object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportSchedules inimportschedules)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportschedules.ConnectionName, INImportSchedulesQueries.Delete(inimportschedules, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportSchedules object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportschedules object from the database.
        /// </summary>
        /// <param name="inimportschedules">The id of the inimportschedules object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportschedules history.</param>
        /// <returns>True if the inimportschedules history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportSchedules inimportschedules)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportschedules.ConnectionName, INImportSchedulesQueries.DeleteHistory(inimportschedules, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportSchedules history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportschedules object from the database.
        /// </summary>
        /// <param name="inimportschedules">The id of the inimportschedules object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportschedules object.</param>
        /// <returns>True if the inimportschedules object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportSchedules inimportschedules)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportschedules.ConnectionName, INImportSchedulesQueries.UnDelete(inimportschedules, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportSchedules object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportschedules object from the data reader.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportSchedules inimportschedules, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportschedules.IsLoaded = true;
                    inimportschedules.FKINCampaignID = reader["FKINCampaignID"] != DBNull.Value ? (long)reader["FKINCampaignID"] : (long?)null;
                    inimportschedules.BatchName = reader["BatchName"] != DBNull.Value ? (string)reader["BatchName"] : (string)null;
                    inimportschedules.UDMCode = reader["UDMCode"] != DBNull.Value ? (string)reader["UDMCode"] : (string)null;
                    inimportschedules.ImportFile = reader["ImportFile"] != DBNull.Value ? (byte[])reader["ImportFile"] : (byte[])null;
                    inimportschedules.ScheduleDate = reader["ScheduleDate"] != DBNull.Value ? (DateTime)reader["ScheduleDate"] : (DateTime?)null;
                    inimportschedules.ScheduleTime = reader["ScheduleTime"] != DBNull.Value ? (TimeSpan)reader["ScheduleTime"] : (TimeSpan?)null;
                    inimportschedules.HasRun = reader["HasRun"] != DBNull.Value ? (bool)reader["HasRun"] : (bool?)null;
                    inimportschedules.NumberOfLeads = reader["NumberOfLeads"] != DBNull.Value ? (int)reader["NumberOfLeads"] : (int?)null;
                    inimportschedules.ImportAtempts = reader["ImportAtempts"] != DBNull.Value ? (int)reader["ImportAtempts"] : (int?)null;
                    inimportschedules.DateReceived = reader["DateReceived"] != DBNull.Value ? (DateTime)reader["DateReceived"] : (DateTime?)null;
                    inimportschedules.StampDate = (DateTime)reader["StampDate"];
                    inimportschedules.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportSchedules does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportSchedules object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportschedules object from the database.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules to fill.</param>
        internal static void Fill(INImportSchedules inimportschedules)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportschedules.ConnectionName, INImportSchedulesQueries.Fill(inimportschedules, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportschedules, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportSchedules object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportschedules object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportSchedules inimportschedules)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportschedules.ConnectionName, INImportSchedulesQueries.FillData(inimportschedules, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportSchedules object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportschedules object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportschedules">The inimportschedules to fill from history.</param>
        internal static void FillHistory(INImportSchedules inimportschedules, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportschedules.ConnectionName, INImportSchedulesQueries.FillHistory(inimportschedules, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportschedules, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportSchedules object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportschedules objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportSchedulesCollection List(bool showDeleted, string connectionName)
        {
            INImportSchedulesCollection collection = new INImportSchedulesCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportSchedulesQueries.ListDeleted() : INImportSchedulesQueries.List(), null);
                while (reader.Read())
                {
                    INImportSchedules inimportschedules = new INImportSchedules((long)reader["ID"]);
                    inimportschedules.ConnectionName = connectionName;
                    collection.Add(inimportschedules);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportSchedules objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportschedules objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportSchedulesQueries.ListDeleted() : INImportSchedulesQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportSchedules objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportschedules object from the database.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportSchedules inimportschedules)
        {
            INImportSchedulesCollection collection = new INImportSchedulesCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportschedules.ConnectionName, INImportSchedulesQueries.ListHistory(inimportschedules, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportSchedules in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportschedules object to the database.
        /// </summary>
        /// <param name="inimportschedules">The INImportSchedules object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportschedules was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportSchedules inimportschedules)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportschedules.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportschedules.ConnectionName, INImportSchedulesQueries.Save(inimportschedules, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportschedules.ConnectionName, INImportSchedulesQueries.Save(inimportschedules, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportschedules.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportschedules.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportSchedules object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportschedules objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="batchname">The batchname search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="importfile">The importfile search criteria.</param>
        /// <param name="scheduledate">The scheduledate search criteria.</param>
        /// <param name="scheduletime">The scheduletime search criteria.</param>
        /// <param name="hasrun">The hasrun search criteria.</param>
        /// <param name="numberofleads">The numberofleads search criteria.</param>
        /// <param name="importatempts">The importatempts search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportSchedulesCollection Search(long? fkincampaignid, string batchname, string udmcode, byte[] importfile, DateTime? scheduledate, TimeSpan? scheduletime, bool? hasrun, int? numberofleads, int? importatempts, DateTime? datereceived, string connectionName)
        {
            INImportSchedulesCollection collection = new INImportSchedulesCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportSchedulesQueries.Search(fkincampaignid, batchname, udmcode, importfile, scheduledate, scheduletime, hasrun, numberofleads, importatempts, datereceived), null);
                while (reader.Read())
                {
                    INImportSchedules inimportschedules = new INImportSchedules((long)reader["ID"]);
                    inimportschedules.ConnectionName = connectionName;
                    collection.Add(inimportschedules);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportSchedules objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportschedules objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="batchname">The batchname search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="importfile">The importfile search criteria.</param>
        /// <param name="scheduledate">The scheduledate search criteria.</param>
        /// <param name="scheduletime">The scheduletime search criteria.</param>
        /// <param name="hasrun">The hasrun search criteria.</param>
        /// <param name="numberofleads">The numberofleads search criteria.</param>
        /// <param name="importatempts">The importatempts search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkincampaignid, string batchname, string udmcode, byte[] importfile, DateTime? scheduledate, TimeSpan? scheduletime, bool? hasrun, int? numberofleads, int? importatempts, DateTime? datereceived, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportSchedulesQueries.Search(fkincampaignid, batchname, udmcode, importfile, scheduledate, scheduletime, hasrun, numberofleads, importatempts, datereceived), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportSchedules objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportschedules objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="batchname">The batchname search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="importfile">The importfile search criteria.</param>
        /// <param name="scheduledate">The scheduledate search criteria.</param>
        /// <param name="scheduletime">The scheduletime search criteria.</param>
        /// <param name="hasrun">The hasrun search criteria.</param>
        /// <param name="numberofleads">The numberofleads search criteria.</param>
        /// <param name="importatempts">The importatempts search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportSchedules SearchOne(long? fkincampaignid, string batchname, string udmcode, byte[] importfile, DateTime? scheduledate, TimeSpan? scheduletime, bool? hasrun, int? numberofleads, int? importatempts, DateTime? datereceived, string connectionName)
        {
            INImportSchedules inimportschedules = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportSchedulesQueries.Search(fkincampaignid, batchname, udmcode, importfile, scheduledate, scheduletime, hasrun, numberofleads, importatempts, datereceived), null);
                if (reader.Read())
                {
                    inimportschedules = new INImportSchedules((long)reader["ID"]);
                    inimportschedules.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportSchedules objects in the database", ex);
            }
            return inimportschedules;
        }
        #endregion
    }
}
