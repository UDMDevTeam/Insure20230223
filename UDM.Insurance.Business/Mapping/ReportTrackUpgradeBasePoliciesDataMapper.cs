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
    /// Contains methods to fill, save and delete reporttrackupgradebasepoliciesdata objects.
    /// </summary>
    public partial class ReportTrackUpgradeBasePoliciesDataMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) reporttrackupgradebasepoliciesdata object from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The id of the reporttrackupgradebasepoliciesdata object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the reporttrackupgradebasepoliciesdata object.</param>
        /// <returns>True if the reporttrackupgradebasepoliciesdata object was deleted successfully, else false.</returns>
        internal static bool Delete(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.Delete(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a ReportTrackUpgradeBasePoliciesData object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) reporttrackupgradebasepoliciesdata object from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The id of the reporttrackupgradebasepoliciesdata object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the reporttrackupgradebasepoliciesdata history.</param>
        /// <returns>True if the reporttrackupgradebasepoliciesdata history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.DeleteHistory(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete ReportTrackUpgradeBasePoliciesData history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) reporttrackupgradebasepoliciesdata object from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The id of the reporttrackupgradebasepoliciesdata object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the reporttrackupgradebasepoliciesdata object.</param>
        /// <returns>True if the reporttrackupgradebasepoliciesdata object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.UnDelete(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a ReportTrackUpgradeBasePoliciesData object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the reporttrackupgradebasepoliciesdata object from the data reader.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    reporttrackupgradebasepoliciesdata.IsLoaded = true;
                    reporttrackupgradebasepoliciesdata.Year = reader["Year"] != DBNull.Value ? (long)reader["Year"] : (long?)null;
                    reporttrackupgradebasepoliciesdata.Month = reader["Month"] != DBNull.Value ? (string)reader["Month"] : (string)null;
                    reporttrackupgradebasepoliciesdata.LeadsReceived = reader["LeadsReceived"] != DBNull.Value ? (long)reader["LeadsReceived"] : (long?)null;
                    reporttrackupgradebasepoliciesdata.SheMaccLeads = reader["SheMaccLeads"] != DBNull.Value ? (long)reader["SheMaccLeads"] : (long?)null;
                    reporttrackupgradebasepoliciesdata.TargetPercentage = reader["TargetPercentage"] != DBNull.Value ? (decimal)reader["TargetPercentage"] : (decimal?)null;
                    reporttrackupgradebasepoliciesdata.StampDate = (DateTime)reader["StampDate"];
                    reporttrackupgradebasepoliciesdata.HasChanged = false;
                }
                else
                {
                    throw new MapperException("ReportTrackUpgradeBasePoliciesData does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a ReportTrackUpgradeBasePoliciesData object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an reporttrackupgradebasepoliciesdata object from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata to fill.</param>
        internal static void Fill(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.Fill(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(reporttrackupgradebasepoliciesdata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a ReportTrackUpgradeBasePoliciesData object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a reporttrackupgradebasepoliciesdata object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.FillData(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a ReportTrackUpgradeBasePoliciesData object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an reporttrackupgradebasepoliciesdata object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata to fill from history.</param>
        internal static void FillHistory(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.FillHistory(reporttrackupgradebasepoliciesdata, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(reporttrackupgradebasepoliciesdata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a ReportTrackUpgradeBasePoliciesData object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the reporttrackupgradebasepoliciesdata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static ReportTrackUpgradeBasePoliciesDataCollection List(bool showDeleted, string connectionName)
        {
            ReportTrackUpgradeBasePoliciesDataCollection collection = new ReportTrackUpgradeBasePoliciesDataCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? ReportTrackUpgradeBasePoliciesDataQueries.ListDeleted() : ReportTrackUpgradeBasePoliciesDataQueries.List(), null);
                while (reader.Read())
                {
                    ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata = new ReportTrackUpgradeBasePoliciesData((long)reader["ID"]);
                    reporttrackupgradebasepoliciesdata.ConnectionName = connectionName;
                    collection.Add(reporttrackupgradebasepoliciesdata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list ReportTrackUpgradeBasePoliciesData objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the reporttrackupgradebasepoliciesdata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? ReportTrackUpgradeBasePoliciesDataQueries.ListDeleted() : ReportTrackUpgradeBasePoliciesDataQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list ReportTrackUpgradeBasePoliciesData objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) reporttrackupgradebasepoliciesdata object from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata)
        {
            ReportTrackUpgradeBasePoliciesDataCollection collection = new ReportTrackUpgradeBasePoliciesDataCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.ListHistory(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) ReportTrackUpgradeBasePoliciesData in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an reporttrackupgradebasepoliciesdata object to the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The ReportTrackUpgradeBasePoliciesData object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the reporttrackupgradebasepoliciesdata was saved successfull, otherwise, false.</returns>
        internal static bool Save(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (reporttrackupgradebasepoliciesdata.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.Save(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(reporttrackupgradebasepoliciesdata.ConnectionName, ReportTrackUpgradeBasePoliciesDataQueries.Save(reporttrackupgradebasepoliciesdata, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            reporttrackupgradebasepoliciesdata.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= reporttrackupgradebasepoliciesdata.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a ReportTrackUpgradeBasePoliciesData object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for reporttrackupgradebasepoliciesdata objects in the database.
        /// </summary>
        /// <param name="year">The year search criteria.</param>
        /// <param name="month">The month search criteria.</param>
        /// <param name="leadsreceived">The leadsreceived search criteria.</param>
        /// <param name="shemaccleads">The shemaccleads search criteria.</param>
        /// <param name="targetpercentage">The targetpercentage search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static ReportTrackUpgradeBasePoliciesDataCollection Search(long? year, string month, long? leadsreceived, long? shemaccleads, decimal? targetpercentage, string connectionName)
        {
            ReportTrackUpgradeBasePoliciesDataCollection collection = new ReportTrackUpgradeBasePoliciesDataCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ReportTrackUpgradeBasePoliciesDataQueries.Search(year, month, leadsreceived, shemaccleads, targetpercentage), null);
                while (reader.Read())
                {
                    ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata = new ReportTrackUpgradeBasePoliciesData((long)reader["ID"]);
                    reporttrackupgradebasepoliciesdata.ConnectionName = connectionName;
                    collection.Add(reporttrackupgradebasepoliciesdata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for ReportTrackUpgradeBasePoliciesData objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for reporttrackupgradebasepoliciesdata objects in the database.
        /// </summary>
        /// <param name="year">The year search criteria.</param>
        /// <param name="month">The month search criteria.</param>
        /// <param name="leadsreceived">The leadsreceived search criteria.</param>
        /// <param name="shemaccleads">The shemaccleads search criteria.</param>
        /// <param name="targetpercentage">The targetpercentage search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? year, string month, long? leadsreceived, long? shemaccleads, decimal? targetpercentage, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, ReportTrackUpgradeBasePoliciesDataQueries.Search(year, month, leadsreceived, shemaccleads, targetpercentage), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for ReportTrackUpgradeBasePoliciesData objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 reporttrackupgradebasepoliciesdata objects in the database.
        /// </summary>
        /// <param name="year">The year search criteria.</param>
        /// <param name="month">The month search criteria.</param>
        /// <param name="leadsreceived">The leadsreceived search criteria.</param>
        /// <param name="shemaccleads">The shemaccleads search criteria.</param>
        /// <param name="targetpercentage">The targetpercentage search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static ReportTrackUpgradeBasePoliciesData SearchOne(long? year, string month, long? leadsreceived, long? shemaccleads, decimal? targetpercentage, string connectionName)
        {
            ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, ReportTrackUpgradeBasePoliciesDataQueries.Search(year, month, leadsreceived, shemaccleads, targetpercentage), null);
                if (reader.Read())
                {
                    reporttrackupgradebasepoliciesdata = new ReportTrackUpgradeBasePoliciesData((long)reader["ID"]);
                    reporttrackupgradebasepoliciesdata.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for ReportTrackUpgradeBasePoliciesData objects in the database", ex);
            }
            return reporttrackupgradebasepoliciesdata;
        }
        #endregion
    }
}
