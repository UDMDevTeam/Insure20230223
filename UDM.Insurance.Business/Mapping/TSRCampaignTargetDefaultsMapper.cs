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
    /// Contains methods to fill, save and delete tsrcampaigntargetdefaults objects.
    /// </summary>
    public partial class TSRCampaignTargetDefaultsMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) tsrcampaigntargetdefaults object from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The id of the tsrcampaigntargetdefaults object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the tsrcampaigntargetdefaults object.</param>
        /// <returns>True if the tsrcampaigntargetdefaults object was deleted successfully, else false.</returns>
        internal static bool Delete(TSRCampaignTargetDefaults tsrcampaigntargetdefaults)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.Delete(tsrcampaigntargetdefaults, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a TSRCampaignTargetDefaults object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) tsrcampaigntargetdefaults object from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The id of the tsrcampaigntargetdefaults object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the tsrcampaigntargetdefaults history.</param>
        /// <returns>True if the tsrcampaigntargetdefaults history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(TSRCampaignTargetDefaults tsrcampaigntargetdefaults)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.DeleteHistory(tsrcampaigntargetdefaults, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete TSRCampaignTargetDefaults history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) tsrcampaigntargetdefaults object from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The id of the tsrcampaigntargetdefaults object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the tsrcampaigntargetdefaults object.</param>
        /// <returns>True if the tsrcampaigntargetdefaults object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(TSRCampaignTargetDefaults tsrcampaigntargetdefaults)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.UnDelete(tsrcampaigntargetdefaults, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a TSRCampaignTargetDefaults object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the tsrcampaigntargetdefaults object from the data reader.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    tsrcampaigntargetdefaults.IsLoaded = true;
                    tsrcampaigntargetdefaults.FKINCampaignID = reader["FKINCampaignID"] != DBNull.Value ? (long)reader["FKINCampaignID"] : (long?)null;
                    tsrcampaigntargetdefaults.SalesPerHourTarget = reader["SalesPerHourTarget"] != DBNull.Value ? (Double)reader["SalesPerHourTarget"] : (Double?)null;
                    tsrcampaigntargetdefaults.BasePremiumTarget = reader["BasePremiumTarget"] != DBNull.Value ? (decimal)reader["BasePremiumTarget"] : (decimal?)null;
                    tsrcampaigntargetdefaults.PartnerPremiumTarget = reader["PartnerPremiumTarget"] != DBNull.Value ? (decimal)reader["PartnerPremiumTarget"] : (decimal?)null;
                    tsrcampaigntargetdefaults.ChildPremiumTarget = reader["ChildPremiumTarget"] != DBNull.Value ? (decimal)reader["ChildPremiumTarget"] : (decimal?)null;
                    tsrcampaigntargetdefaults.PartnerTarget = reader["PartnerTarget"] != DBNull.Value ? (Double)reader["PartnerTarget"] : (Double?)null;
                    tsrcampaigntargetdefaults.ChildTarget = reader["ChildTarget"] != DBNull.Value ? (Double)reader["ChildTarget"] : (Double?)null;
                    tsrcampaigntargetdefaults.DateApplicableFrom = reader["DateApplicableFrom"] != DBNull.Value ? (DateTime)reader["DateApplicableFrom"] : (DateTime?)null;
                    tsrcampaigntargetdefaults.BaseUnitTarget = reader["BaseUnitTarget"] != DBNull.Value ? (Double)reader["BaseUnitTarget"] : (Double?)null;
                    tsrcampaigntargetdefaults.AccDisSelectedItem = reader["AccDisSelectedItem"] != DBNull.Value ? (string)reader["AccDisSelectedItem"] : (string)null;
                    tsrcampaigntargetdefaults.FKINCampaignClusterID = reader["FKINCampaignClusterID"] != DBNull.Value ? (long)reader["FKINCampaignClusterID"] : (long?)null;
                    tsrcampaigntargetdefaults.HasChanged = false;
                }
                else
                {
                    throw new MapperException("TSRCampaignTargetDefaults does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a TSRCampaignTargetDefaults object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an tsrcampaigntargetdefaults object from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults to fill.</param>
        internal static void Fill(TSRCampaignTargetDefaults tsrcampaigntargetdefaults)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.Fill(tsrcampaigntargetdefaults, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(tsrcampaigntargetdefaults, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a TSRCampaignTargetDefaults object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a tsrcampaigntargetdefaults object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(TSRCampaignTargetDefaults tsrcampaigntargetdefaults)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.FillData(tsrcampaigntargetdefaults, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a TSRCampaignTargetDefaults object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an tsrcampaigntargetdefaults object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults to fill from history.</param>
        internal static void FillHistory(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.FillHistory(tsrcampaigntargetdefaults, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(tsrcampaigntargetdefaults, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a TSRCampaignTargetDefaults object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the tsrcampaigntargetdefaults objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static TSRCampaignTargetDefaultsCollection List(bool showDeleted, string connectionName)
        {
            TSRCampaignTargetDefaultsCollection collection = new TSRCampaignTargetDefaultsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? TSRCampaignTargetDefaultsQueries.ListDeleted() : TSRCampaignTargetDefaultsQueries.List(), null);
                while (reader.Read())
                {
                    TSRCampaignTargetDefaults tsrcampaigntargetdefaults = new TSRCampaignTargetDefaults((long)reader["ID"]);
                    tsrcampaigntargetdefaults.ConnectionName = connectionName;
                    collection.Add(tsrcampaigntargetdefaults);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list TSRCampaignTargetDefaults objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the tsrcampaigntargetdefaults objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? TSRCampaignTargetDefaultsQueries.ListDeleted() : TSRCampaignTargetDefaultsQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list TSRCampaignTargetDefaults objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) tsrcampaigntargetdefaults object from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(TSRCampaignTargetDefaults tsrcampaigntargetdefaults)
        {
            TSRCampaignTargetDefaultsCollection collection = new TSRCampaignTargetDefaultsCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.ListHistory(tsrcampaigntargetdefaults, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) TSRCampaignTargetDefaults in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an tsrcampaigntargetdefaults object to the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The TSRCampaignTargetDefaults object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the tsrcampaigntargetdefaults was saved successfull, otherwise, false.</returns>
        internal static bool Save(TSRCampaignTargetDefaults tsrcampaigntargetdefaults)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (tsrcampaigntargetdefaults.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.Save(tsrcampaigntargetdefaults, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(tsrcampaigntargetdefaults.ConnectionName, TSRCampaignTargetDefaultsQueries.Save(tsrcampaigntargetdefaults, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            tsrcampaigntargetdefaults.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= tsrcampaigntargetdefaults.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a TSRCampaignTargetDefaults object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for tsrcampaigntargetdefaults objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="salesperhourtarget">The salesperhourtarget search criteria.</param>
        /// <param name="basepremiumtarget">The basepremiumtarget search criteria.</param>
        /// <param name="partnerpremiumtarget">The partnerpremiumtarget search criteria.</param>
        /// <param name="childpremiumtarget">The childpremiumtarget search criteria.</param>
        /// <param name="partnertarget">The partnertarget search criteria.</param>
        /// <param name="childtarget">The childtarget search criteria.</param>
        /// <param name="dateapplicablefrom">The dateapplicablefrom search criteria.</param>
        /// <param name="baseunittarget">The baseunittarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="fkincampaignclusterid">The fkincampaignclusterid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static TSRCampaignTargetDefaultsCollection Search(long? fkincampaignid, Double? salesperhourtarget, decimal? basepremiumtarget, decimal? partnerpremiumtarget, decimal? childpremiumtarget, Double? partnertarget, Double? childtarget, DateTime? dateapplicablefrom, Double? baseunittarget, string accdisselecteditem, long? fkincampaignclusterid, string connectionName)
        {
            TSRCampaignTargetDefaultsCollection collection = new TSRCampaignTargetDefaultsCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, TSRCampaignTargetDefaultsQueries.Search(fkincampaignid, salesperhourtarget, basepremiumtarget, partnerpremiumtarget, childpremiumtarget, partnertarget, childtarget, dateapplicablefrom, baseunittarget, accdisselecteditem, fkincampaignclusterid), null);
                while (reader.Read())
                {
                    TSRCampaignTargetDefaults tsrcampaigntargetdefaults = new TSRCampaignTargetDefaults((long)reader["ID"]);
                    tsrcampaigntargetdefaults.ConnectionName = connectionName;
                    collection.Add(tsrcampaigntargetdefaults);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for TSRCampaignTargetDefaults objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for tsrcampaigntargetdefaults objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="salesperhourtarget">The salesperhourtarget search criteria.</param>
        /// <param name="basepremiumtarget">The basepremiumtarget search criteria.</param>
        /// <param name="partnerpremiumtarget">The partnerpremiumtarget search criteria.</param>
        /// <param name="childpremiumtarget">The childpremiumtarget search criteria.</param>
        /// <param name="partnertarget">The partnertarget search criteria.</param>
        /// <param name="childtarget">The childtarget search criteria.</param>
        /// <param name="dateapplicablefrom">The dateapplicablefrom search criteria.</param>
        /// <param name="baseunittarget">The baseunittarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="fkincampaignclusterid">The fkincampaignclusterid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkincampaignid, Double? salesperhourtarget, decimal? basepremiumtarget, decimal? partnerpremiumtarget, decimal? childpremiumtarget, Double? partnertarget, Double? childtarget, DateTime? dateapplicablefrom, Double? baseunittarget, string accdisselecteditem, long? fkincampaignclusterid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, TSRCampaignTargetDefaultsQueries.Search(fkincampaignid, salesperhourtarget, basepremiumtarget, partnerpremiumtarget, childpremiumtarget, partnertarget, childtarget, dateapplicablefrom, baseunittarget, accdisselecteditem, fkincampaignclusterid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for TSRCampaignTargetDefaults objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 tsrcampaigntargetdefaults objects in the database.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="salesperhourtarget">The salesperhourtarget search criteria.</param>
        /// <param name="basepremiumtarget">The basepremiumtarget search criteria.</param>
        /// <param name="partnerpremiumtarget">The partnerpremiumtarget search criteria.</param>
        /// <param name="childpremiumtarget">The childpremiumtarget search criteria.</param>
        /// <param name="partnertarget">The partnertarget search criteria.</param>
        /// <param name="childtarget">The childtarget search criteria.</param>
        /// <param name="dateapplicablefrom">The dateapplicablefrom search criteria.</param>
        /// <param name="baseunittarget">The baseunittarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="fkincampaignclusterid">The fkincampaignclusterid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static TSRCampaignTargetDefaults SearchOne(long? fkincampaignid, Double? salesperhourtarget, decimal? basepremiumtarget, decimal? partnerpremiumtarget, decimal? childpremiumtarget, Double? partnertarget, Double? childtarget, DateTime? dateapplicablefrom, Double? baseunittarget, string accdisselecteditem, long? fkincampaignclusterid, string connectionName)
        {
            TSRCampaignTargetDefaults tsrcampaigntargetdefaults = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, TSRCampaignTargetDefaultsQueries.Search(fkincampaignid, salesperhourtarget, basepremiumtarget, partnerpremiumtarget, childpremiumtarget, partnertarget, childtarget, dateapplicablefrom, baseunittarget, accdisselecteditem, fkincampaignclusterid), null);
                if (reader.Read())
                {
                    tsrcampaigntargetdefaults = new TSRCampaignTargetDefaults((long)reader["ID"]);
                    tsrcampaigntargetdefaults.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for TSRCampaignTargetDefaults objects in the database", ex);
            }
            return tsrcampaigntargetdefaults;
        }
        #endregion
    }
}
