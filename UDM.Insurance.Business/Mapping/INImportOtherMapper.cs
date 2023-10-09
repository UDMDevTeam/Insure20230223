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
    /// Contains methods to fill, save and delete inimportother objects.
    /// </summary>
    public partial class INImportOtherMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportother object from the database.
        /// </summary>
        /// <param name="inimportother">The id of the inimportother object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportother object.</param>
        /// <returns>True if the inimportother object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportOther inimportother)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportother.ConnectionName, INImportOtherQueries.Delete(inimportother, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportOther object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportother object from the database.
        /// </summary>
        /// <param name="inimportother">The id of the inimportother object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportother history.</param>
        /// <returns>True if the inimportother history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportOther inimportother)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportother.ConnectionName, INImportOtherQueries.DeleteHistory(inimportother, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportOther history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportother object from the database.
        /// </summary>
        /// <param name="inimportother">The id of the inimportother object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportother object.</param>
        /// <returns>True if the inimportother object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportOther inimportother)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportother.ConnectionName, INImportOtherQueries.UnDelete(inimportother, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportOther object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportother object from the data reader.
        /// </summary>
        /// <param name="inimportother">The inimportother object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportOther inimportother, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportother.IsLoaded = true;
                    inimportother.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inimportother.FKINBatchID = reader["FKINBatchID"] != DBNull.Value ? (long)reader["FKINBatchID"] : (long?)null;
                    inimportother.RefNo = reader["RefNo"] != DBNull.Value ? (string)reader["RefNo"] : (string)null;
                    inimportother.AccountType = reader["AccountType"] != DBNull.Value ? (string)reader["AccountType"] : (string)null;
                    inimportother.StartDate = reader["StartDate"] != DBNull.Value ? (DateTime)reader["StartDate"] : (DateTime?)null;
                    inimportother.EndDate = reader["EndDate"] != DBNull.Value ? (DateTime)reader["EndDate"] : (DateTime?)null;
                    inimportother.ReferralFrom = reader["ReferralFrom"] != DBNull.Value ? (string)reader["ReferralFrom"] : (string)null;
                    inimportother.AddressFrom = reader["AddressFrom"] != DBNull.Value ? (string)reader["AddressFrom"] : (string)null;
                    inimportother.TimesRemarketed = reader["TimesRemarketed"] != DBNull.Value ? (short)reader["TimesRemarketed"] : (short?)null;
                    inimportother.LastDateRemarketed = reader["LastDateRemarketed"] != DBNull.Value ? (DateTime)reader["LastDateRemarketed"] : (DateTime?)null;
                    inimportother.CollectedDate = reader["CollectedDate"] != DBNull.Value ? (DateTime)reader["CollectedDate"] : (DateTime?)null;
                    inimportother.CommencementDate = reader["CommencementDate"] != DBNull.Value ? (DateTime)reader["CommencementDate"] : (DateTime?)null;
                    inimportother.DurationInForce = reader["DurationInForce"] != DBNull.Value ? (int)reader["DurationInForce"] : (int?)null;
                    inimportother.DurationSinceOOF = reader["DurationSinceOOF"] != DBNull.Value ? (int)reader["DurationSinceOOF"] : (int?)null;
                    inimportother.NumColls = reader["NumColls"] != DBNull.Value ? (int)reader["NumColls"] : (int?)null;
                    inimportother.OOFDate = reader["OOFDate"] != DBNull.Value ? (DateTime)reader["OOFDate"] : (DateTime?)null;
                    inimportother.OOFType = reader["OOFType"] != DBNull.Value ? (string)reader["OOFType"] : (string)null;
                    inimportother.UpgradeCount = reader["UpgradeCount"] != DBNull.Value ? (int)reader["UpgradeCount"] : (int?)null;
                    inimportother.Premium = reader["Premium"] != DBNull.Value ? (decimal)reader["Premium"] : (decimal?)null;
                    inimportother.Bank = reader["Bank"] != DBNull.Value ? (string)reader["Bank"] : (string)null;
                    inimportother.Last4Digits = reader["Last4Digits"] != DBNull.Value ? (string)reader["Last4Digits"] : (string)null;
                    inimportother.ExtendedSalesDate = reader["ExtendedSalesDate"] != DBNull.Value ? (DateTime)reader["ExtendedSalesDate"] : (DateTime?)null;
                    inimportother.OriginalCampaign = reader["OriginalCampaign"] != DBNull.Value ? (string)reader["OriginalCampaign"] : (string)null;
                    inimportother.StampDate = (DateTime)reader["StampDate"];
                    inimportother.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportOther does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportOther object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportother object from the database.
        /// </summary>
        /// <param name="inimportother">The inimportother to fill.</param>
        internal static void Fill(INImportOther inimportother)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportother.ConnectionName, INImportOtherQueries.Fill(inimportother, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportother, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportOther object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportother object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportOther inimportother)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportother.ConnectionName, INImportOtherQueries.FillData(inimportother, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportOther object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportother object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportother">The inimportother to fill from history.</param>
        internal static void FillHistory(INImportOther inimportother, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportother.ConnectionName, INImportOtherQueries.FillHistory(inimportother, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportother, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportOther object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportother objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportOtherCollection List(bool showDeleted, string connectionName)
        {
            INImportOtherCollection collection = new INImportOtherCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportOtherQueries.ListDeleted() : INImportOtherQueries.List(), null);
                while (reader.Read())
                {
                    INImportOther inimportother = new INImportOther((long)reader["ID"]);
                    inimportother.ConnectionName = connectionName;
                    collection.Add(inimportother);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportOther objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportother objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportOtherQueries.ListDeleted() : INImportOtherQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportOther objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportother object from the database.
        /// </summary>
        /// <param name="inimportother">The inimportother to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportOther inimportother)
        {
            INImportOtherCollection collection = new INImportOtherCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportother.ConnectionName, INImportOtherQueries.ListHistory(inimportother, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportOther in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportother object to the database.
        /// </summary>
        /// <param name="inimportother">The INImportOther object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportother was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportOther inimportother)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportother.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportother.ConnectionName, INImportOtherQueries.Save(inimportother, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportother.ConnectionName, INImportOtherQueries.Save(inimportother, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportother.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportother.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportOther object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportother objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="refno">The refno search criteria.</param>
        /// <param name="accounttype">The accounttype search criteria.</param>
        /// <param name="startdate">The startdate search criteria.</param>
        /// <param name="enddate">The enddate search criteria.</param>
        /// <param name="referralfrom">The referralfrom search criteria.</param>
        /// <param name="addressfrom">The addressfrom search criteria.</param>
        /// <param name="timesremarketed">The timesremarketed search criteria.</param>
        /// <param name="lastdateremarketed">The lastdateremarketed search criteria.</param>
        /// <param name="collecteddate">The collecteddate search criteria.</param>
        /// <param name="commencementdate">The commencementdate search criteria.</param>
        /// <param name="durationinforce">The durationinforce search criteria.</param>
        /// <param name="durationsinceoof">The durationsinceoof search criteria.</param>
        /// <param name="numcolls">The numcolls search criteria.</param>
        /// <param name="oofdate">The oofdate search criteria.</param>
        /// <param name="ooftype">The ooftype search criteria.</param>
        /// <param name="upgradecount">The upgradecount search criteria.</param>
        /// <param name="premium">The premium search criteria.</param>
        /// <param name="bank">The bank search criteria.</param>
        /// <param name="last4digits">The last4digits search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportOtherCollection Search(long? fkinimportid, long? fkinbatchid, string refno, string accounttype, DateTime? startdate, DateTime? enddate, string referralfrom, string addressfrom, short? timesremarketed, DateTime? lastdateremarketed, DateTime? collecteddate, DateTime? commencementdate, int? durationinforce, int? durationsinceoof, int? numcolls, DateTime? oofdate, string ooftype, int? upgradecount, decimal? premium, string bank, string last4digits,DateTime? extendedsalesdate, string originalcampaign, string connectionName)
        {
            INImportOtherCollection collection = new INImportOtherCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportOtherQueries.Search(fkinimportid, fkinbatchid, refno, accounttype, startdate, enddate, referralfrom, addressfrom, timesremarketed, lastdateremarketed, collecteddate, commencementdate, durationinforce, durationsinceoof, numcolls, oofdate, ooftype, upgradecount, premium, bank, last4digits, extendedsalesdate, originalcampaign), null);
                while (reader.Read())
                {
                    INImportOther inimportother = new INImportOther((long)reader["ID"]);
                    inimportother.ConnectionName = connectionName;
                    collection.Add(inimportother);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportOther objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportother objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="refno">The refno search criteria.</param>
        /// <param name="accounttype">The accounttype search criteria.</param>
        /// <param name="startdate">The startdate search criteria.</param>
        /// <param name="enddate">The enddate search criteria.</param>
        /// <param name="referralfrom">The referralfrom search criteria.</param>
        /// <param name="addressfrom">The addressfrom search criteria.</param>
        /// <param name="timesremarketed">The timesremarketed search criteria.</param>
        /// <param name="lastdateremarketed">The lastdateremarketed search criteria.</param>
        /// <param name="collecteddate">The collecteddate search criteria.</param>
        /// <param name="commencementdate">The commencementdate search criteria.</param>
        /// <param name="durationinforce">The durationinforce search criteria.</param>
        /// <param name="durationsinceoof">The durationsinceoof search criteria.</param>
        /// <param name="numcolls">The numcolls search criteria.</param>
        /// <param name="oofdate">The oofdate search criteria.</param>
        /// <param name="ooftype">The ooftype search criteria.</param>
        /// <param name="upgradecount">The upgradecount search criteria.</param>
        /// <param name="premium">The premium search criteria.</param>
        /// <param name="bank">The bank search criteria.</param>
        /// <param name="last4digits">The last4digits search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, long? fkinbatchid, string refno, string accounttype, DateTime? startdate, DateTime? enddate, string referralfrom, string addressfrom, short? timesremarketed, DateTime? lastdateremarketed, DateTime? collecteddate, DateTime? commencementdate, int? durationinforce, int? durationsinceoof, int? numcolls, DateTime? oofdate, string ooftype, int? upgradecount, decimal? premium, string bank, string last4digits, DateTime? extendedsalesdate, string originalcampaign, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportOtherQueries.Search(fkinimportid, fkinbatchid, refno, accounttype, startdate, enddate, referralfrom, addressfrom, timesremarketed, lastdateremarketed, collecteddate, commencementdate, durationinforce, durationsinceoof, numcolls, oofdate, ooftype, upgradecount, premium, bank, last4digits, extendedsalesdate, originalcampaign), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportOther objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportother objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="refno">The refno search criteria.</param>
        /// <param name="accounttype">The accounttype search criteria.</param>
        /// <param name="startdate">The startdate search criteria.</param>
        /// <param name="enddate">The enddate search criteria.</param>
        /// <param name="referralfrom">The referralfrom search criteria.</param>
        /// <param name="addressfrom">The addressfrom search criteria.</param>
        /// <param name="timesremarketed">The timesremarketed search criteria.</param>
        /// <param name="lastdateremarketed">The lastdateremarketed search criteria.</param>
        /// <param name="collecteddate">The collecteddate search criteria.</param>
        /// <param name="commencementdate">The commencementdate search criteria.</param>
        /// <param name="durationinforce">The durationinforce search criteria.</param>
        /// <param name="durationsinceoof">The durationsinceoof search criteria.</param>
        /// <param name="numcolls">The numcolls search criteria.</param>
        /// <param name="oofdate">The oofdate search criteria.</param>
        /// <param name="ooftype">The ooftype search criteria.</param>
        /// <param name="upgradecount">The upgradecount search criteria.</param>
        /// <param name="premium">The premium search criteria.</param>
        /// <param name="bank">The bank search criteria.</param>
        /// <param name="last4digits">The last4digits search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportOther SearchOne(long? fkinimportid, long? fkinbatchid, string refno, string accounttype, DateTime? startdate, DateTime? enddate, string referralfrom, string addressfrom, short? timesremarketed, DateTime? lastdateremarketed, DateTime? collecteddate, DateTime? commencementdate, int? durationinforce, int? durationsinceoof, int? numcolls, DateTime? oofdate, string ooftype, int? upgradecount, decimal? premium, string bank, string last4digits, DateTime? extendedsalesdate, string originalcampaign, string connectionName)
        {
            INImportOther inimportother = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportOtherQueries.Search(fkinimportid, fkinbatchid, refno, accounttype, startdate, enddate, referralfrom, addressfrom, timesremarketed, lastdateremarketed, collecteddate, commencementdate, durationinforce, durationsinceoof, numcolls, oofdate, ooftype, upgradecount, premium, bank, last4digits, extendedsalesdate, originalcampaign), null);
                if (reader.Read())
                {
                    inimportother = new INImportOther((long)reader["ID"]);
                    inimportother.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportOther objects in the database", ex);
            }
            return inimportother;
        }
        #endregion
    }
}
