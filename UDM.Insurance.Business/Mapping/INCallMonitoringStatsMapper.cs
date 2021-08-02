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
    /// Contains methods to fill, save and delete closure objects.
    /// </summary>
    public partial class INCallMonitoringStatsMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The id of the closure object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the closure object.</param>
        /// <returns>True if the closure object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportCallMonitoringStats iNImportCallMonitoringStats)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.Delete(iNImportCallMonitoringStats, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a Document object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The id of the closure object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the closure history.</param>
        /// <returns>True if the closure history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportCallMonitoringStats iNImportCallMonitoringStats)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.DeleteHistory(iNImportCallMonitoringStats, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete Document history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The id of the closure object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the closure object.</param>
        /// <returns>True if the closure object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportCallMonitoringStats iNImportCallMonitoringStats)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.UnDelete(iNImportCallMonitoringStats, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a Document object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the closure object from the data reader.
        /// </summary>
        /// <param name="document">The closure object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportCallMonitoringStats iNImportCallMonitoringStats, IDataReader reader)
        {
            try
            {

                if (reader["ID"] != DBNull.Value)
                {

                    iNImportCallMonitoringStats.IsLoaded = true;
                    iNImportCallMonitoringStats.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    iNImportCallMonitoringStats.FKINlkpCampaignGroupType = reader["FKlkpINCampaignGroupType"].ToString();
                    iNImportCallMonitoringStats.StartTimeOverAssessment = reader["StartTimeOverAssessment"] != DBNull.Value ? (DateTime)reader["StartTimeOverAssessment"] : (DateTime?)null;
                    iNImportCallMonitoringStats.EndTimeOverAssessment = reader["EndTimeOverAssessment"] != DBNull.Value ? (DateTime)reader["EndTimeOverAssessment"] : (DateTime?)null;
                    iNImportCallMonitoringStats.StartTimeOverAssessorOutcome = reader["StartTimeOverAssessorOutcome"] != DBNull.Value ? (DateTime)reader["StartTimeOverAssessorOutcome"] : (DateTime?)null;
                    iNImportCallMonitoringStats.EndTimeOverAssessorOutcome = reader["EndTimeOverAssessorOutcome"] != DBNull.Value ? (DateTime)reader["EndTimeOverAssessorOutcome"] : (DateTime?)null;
                    iNImportCallMonitoringStats.StartTimeCFOverAssessment = reader["StartTimeCFOverAssessment"] != DBNull.Value ? (DateTime)reader["StartTimeCFOverAssessment"] : (DateTime?)null;
                    iNImportCallMonitoringStats.EndTimeCFOverAssessment = reader["EndTimeCFOverAssessment"] != DBNull.Value ? (DateTime)reader["EndTimeCFOverAssessment"] : (DateTime?)null;
                    iNImportCallMonitoringStats.StampDate = (DateTime)reader["StampDate"];
                    iNImportCallMonitoringStats.HasChanged = false;
                  
                    
                }
                else
                {
                    throw new MapperException("Document does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Document object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an closure object from the database.
        /// </summary>
        /// <param name="document">The closure to fill.</param>
        internal static void Fill(INImportCallMonitoringStats iNImportCallMonitoringStats)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.Fill(iNImportCallMonitoringStats, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(iNImportCallMonitoringStats, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Document object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a closure object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportCallMonitoringStats iNImportCallMonitoringStats)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.FillData(iNImportCallMonitoringStats, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a Document object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an closure object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="iNImportCallMonitoringStats">The closure to fill from history.</param>
        internal static void FillHistory(INImportCallMonitoringStats iNImportCallMonitoringStats, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.FillHistory(iNImportCallMonitoringStats, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(iNImportCallMonitoringStats, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Document object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the closure objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INCallMonitoringStatsCollection List(bool showDeleted, string connectionName)
        {
            INCallMonitoringStatsCollection collection = new INCallMonitoringStatsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INCallMonitoringStatsQueries.ListDeleted() : INCallMonitoringStatsQueries.List(), null);
                while (reader.Read())
                {
                    INImportCallMonitoringStats iNImportCallMonitoringStats = new INImportCallMonitoringStats((long)reader["ID"]);
                    iNImportCallMonitoringStats.ConnectionName = connectionName;
                    collection.Add(iNImportCallMonitoringStats);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Document objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the closure objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INCallMonitoringStatsQueries.ListDeleted() : INCallMonitoringStatsQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Document objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) closure object from the database.
        /// </summary>
        /// <param name="document">The closure to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportCallMonitoringStats iNImportCallMonitoringStats)
        {
            INCallMonitoringStatsCollection collection = new INCallMonitoringStatsCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.ListHistory(iNImportCallMonitoringStats, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) Document in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an closure object to the database.
        /// </summary>
        /// <param name="document">The Closure object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the closure was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportCallMonitoringStats iNImportCallMonitoringStats)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                //string strSQL = "Select FKCampaignID FROM INMySuccessCampaignDetails"; 

                if (iNImportCallMonitoringStats.ID != 0)
                {
                    iNImportCallMonitoringStats.IsLoaded = false;
                }

                if (iNImportCallMonitoringStats.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.Save(iNImportCallMonitoringStats, ref parameters), parameters);
                    result = affectedRows > 0;
                }

                else
                {
                    IDataReader reader = Database.ExecuteReader(iNImportCallMonitoringStats.ConnectionName, INCallMonitoringStatsQueries.Save(iNImportCallMonitoringStats, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            iNImportCallMonitoringStats.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= iNImportCallMonitoringStats.ID != 0;
                }
            }

            catch (Exception ex)
            {
                throw new MapperException("Failed to save a data object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for closure objects in the database.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INCallMonitoringStatsCollection Search(long? fkinimportid, string connectionName)
        {
            INCallMonitoringStatsCollection collection = new INCallMonitoringStatsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCallMonitoringStatsQueries.Search(fkinimportid), null);
                while (reader.Read())
                {
                    INImportCallMonitoringStats iNImportCallMonitoringStats = new INImportCallMonitoringStats((long)reader["ID"]);
                    iNImportCallMonitoringStats.ConnectionName = connectionName;
                    collection.Add(iNImportCallMonitoringStats);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for data objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for closure objects in the database.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="documentid">The fklanguageid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INCallMonitoringStatsQueries.Search(fkinimportid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for data objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 closure objects in the database.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportCallMonitoringStats SearchOne(long? fkinimportid, string connectionName)
        {
            INImportCallMonitoringStats iNImportCallMonitoringStats = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INCallMonitoringStatsQueries.Search(fkinimportid), null);
                if (reader.Read())
                {
                    iNImportCallMonitoringStats = new INImportCallMonitoringStats((long)reader["ID"]);
                    iNImportCallMonitoringStats.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for data objects in the database", ex);
            }
            return iNImportCallMonitoringStats;
        }
        #endregion
    }
}
