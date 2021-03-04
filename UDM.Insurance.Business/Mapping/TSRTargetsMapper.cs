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
    /// Contains methods to fill, save and delete tsrtargets objects.
    /// </summary>
    public partial class TSRTargetsMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) tsrtargets object from the database.
        /// </summary>
        /// <param name="tsrtargets">The id of the tsrtargets object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the tsrtargets object.</param>
        /// <returns>True if the tsrtargets object was deleted successfully, else false.</returns>
        internal static bool Delete(TSRTargets tsrtargets)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(tsrtargets.ConnectionName, TSRTargetsQueries.Delete(tsrtargets, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a TSRTargets object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) tsrtargets object from the database.
        /// </summary>
        /// <param name="tsrtargets">The id of the tsrtargets object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the tsrtargets history.</param>
        /// <returns>True if the tsrtargets history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(TSRTargets tsrtargets)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(tsrtargets.ConnectionName, TSRTargetsQueries.DeleteHistory(tsrtargets, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete TSRTargets history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) tsrtargets object from the database.
        /// </summary>
        /// <param name="tsrtargets">The id of the tsrtargets object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the tsrtargets object.</param>
        /// <returns>True if the tsrtargets object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(TSRTargets tsrtargets)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(tsrtargets.ConnectionName, TSRTargetsQueries.UnDelete(tsrtargets, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a TSRTargets object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the tsrtargets object from the data reader.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(TSRTargets tsrtargets, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    tsrtargets.IsLoaded = true;
                    tsrtargets.FKINWeekID = reader["FKINWeekID"] != DBNull.Value ? (long)reader["FKINWeekID"] : (long?)null;
                    tsrtargets.FKINCampaignID = reader["FKINCampaignID"] != DBNull.Value ? (long)reader["FKINCampaignID"] : (long?)null;
                    tsrtargets.FKAgentID = reader["FKAgentID"] != DBNull.Value ? (long)reader["FKAgentID"] : (long?)null;
                    tsrtargets.DateFrom = reader["DateFrom"] != DBNull.Value ? (DateTime)reader["DateFrom"] : (DateTime?)null;
                    tsrtargets.DateTo = reader["DateTo"] != DBNull.Value ? (DateTime)reader["DateTo"] : (DateTime?)null;
                    tsrtargets.Hours = reader["Hours"] != DBNull.Value ? (Double)reader["Hours"] : (Double?)null;
                    tsrtargets.BaseTarget = reader["BaseTarget"] != DBNull.Value ? (Double)reader["BaseTarget"] : (Double?)null;
                    tsrtargets.PremiumTarget = reader["PremiumTarget"] != DBNull.Value ? (Double)reader["PremiumTarget"] : (Double?)null;
                    tsrtargets.AccDisSelectedItem = reader["AccDisSelectedItem"] != DBNull.Value ? (string)reader["AccDisSelectedItem"] : (string)null;
                    tsrtargets.UnitTarget = reader["UnitTarget"] != DBNull.Value ? (Double)reader["UnitTarget"] : (Double?)null;
                    tsrtargets.HasChanged = false;
                }
                else
                {
                    throw new MapperException("TSRTargets does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a TSRTargets object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an tsrtargets object from the database.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets to fill.</param>
        internal static void Fill(TSRTargets tsrtargets)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(tsrtargets.ConnectionName, TSRTargetsQueries.Fill(tsrtargets, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(tsrtargets, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a TSRTargets object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a tsrtargets object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(TSRTargets tsrtargets)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(tsrtargets.ConnectionName, TSRTargetsQueries.FillData(tsrtargets, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a TSRTargets object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an tsrtargets object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="tsrtargets">The tsrtargets to fill from history.</param>
        internal static void FillHistory(TSRTargets tsrtargets, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(tsrtargets.ConnectionName, TSRTargetsQueries.FillHistory(tsrtargets, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(tsrtargets, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a TSRTargets object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the tsrtargets objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static TSRTargetsCollection List(bool showDeleted, string connectionName)
        {
            TSRTargetsCollection collection = new TSRTargetsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? TSRTargetsQueries.ListDeleted() : TSRTargetsQueries.List(), null);
                while (reader.Read())
                {
                    TSRTargets tsrtargets = new TSRTargets((long)reader["ID"]);
                    tsrtargets.ConnectionName = connectionName;
                    collection.Add(tsrtargets);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list TSRTargets objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the tsrtargets objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? TSRTargetsQueries.ListDeleted() : TSRTargetsQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list TSRTargets objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) tsrtargets object from the database.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(TSRTargets tsrtargets)
        {
            TSRTargetsCollection collection = new TSRTargetsCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(tsrtargets.ConnectionName, TSRTargetsQueries.ListHistory(tsrtargets, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) TSRTargets in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an tsrtargets object to the database.
        /// </summary>
        /// <param name="tsrtargets">The TSRTargets object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the tsrtargets was saved successfull, otherwise, false.</returns>
        internal static bool Save(TSRTargets tsrtargets)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (tsrtargets.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(tsrtargets.ConnectionName, TSRTargetsQueries.Save(tsrtargets, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(tsrtargets.ConnectionName, TSRTargetsQueries.Save(tsrtargets, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            tsrtargets.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= tsrtargets.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a TSRTargets object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for tsrtargets objects in the database.
        /// </summary>
        /// <param name="fkinweekid">The fkinweekid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="fkagentid">The fkagentid search criteria.</param>
        /// <param name="datefrom">The datefrom search criteria.</param>
        /// <param name="dateto">The dateto search criteria.</param>
        /// <param name="hours">The hours search criteria.</param>
        /// <param name="basetarget">The basetarget search criteria.</param>
        /// <param name="premiumtarget">The premiumtarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="unittarget">The unittarget search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static TSRTargetsCollection Search(long? fkinweekid, long? fkincampaignid, long? fkagentid, DateTime? datefrom, DateTime? dateto, Double? hours, Double? basetarget, Double? premiumtarget, string accdisselecteditem, Double? unittarget, string connectionName)
        {
            TSRTargetsCollection collection = new TSRTargetsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, TSRTargetsQueries.Search(fkinweekid, fkincampaignid, fkagentid, datefrom, dateto, hours, basetarget, premiumtarget, accdisselecteditem, unittarget), null);
                while (reader.Read())
                {
                    TSRTargets tsrtargets = new TSRTargets((long)reader["ID"]);
                    tsrtargets.ConnectionName = connectionName;
                    collection.Add(tsrtargets);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for TSRTargets objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for tsrtargets objects in the database.
        /// </summary>
        /// <param name="fkinweekid">The fkinweekid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="fkagentid">The fkagentid search criteria.</param>
        /// <param name="datefrom">The datefrom search criteria.</param>
        /// <param name="dateto">The dateto search criteria.</param>
        /// <param name="hours">The hours search criteria.</param>
        /// <param name="basetarget">The basetarget search criteria.</param>
        /// <param name="premiumtarget">The premiumtarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="unittarget">The unittarget search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinweekid, long? fkincampaignid, long? fkagentid, DateTime? datefrom, DateTime? dateto, Double? hours, Double? basetarget, Double? premiumtarget, string accdisselecteditem, Double? unittarget, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, TSRTargetsQueries.Search(fkinweekid, fkincampaignid, fkagentid, datefrom, dateto, hours, basetarget, premiumtarget, accdisselecteditem, unittarget), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for TSRTargets objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 tsrtargets objects in the database.
        /// </summary>
        /// <param name="fkinweekid">The fkinweekid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="fkagentid">The fkagentid search criteria.</param>
        /// <param name="datefrom">The datefrom search criteria.</param>
        /// <param name="dateto">The dateto search criteria.</param>
        /// <param name="hours">The hours search criteria.</param>
        /// <param name="basetarget">The basetarget search criteria.</param>
        /// <param name="premiumtarget">The premiumtarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="unittarget">The unittarget search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static TSRTargets SearchOne(long? fkinweekid, long? fkincampaignid, long? fkagentid, DateTime? datefrom, DateTime? dateto, Double? hours, Double? basetarget, Double? premiumtarget, string accdisselecteditem, Double? unittarget, string connectionName)
        {
            TSRTargets tsrtargets = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, TSRTargetsQueries.Search(fkinweekid, fkincampaignid, fkagentid, datefrom, dateto, hours, basetarget, premiumtarget, accdisselecteditem, unittarget), null);
                if (reader.Read())
                {
                    tsrtargets = new TSRTargets((long)reader["ID"]);
                    tsrtargets.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for TSRTargets objects in the database", ex);
            }
            return tsrtargets;
        }
        #endregion
    }
}
