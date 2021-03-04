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
    /// Contains methods to fill, save and delete inimportedpolicydata objects.
    /// </summary>
    public partial class INImportedPolicyDataMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportedpolicydata object from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The id of the inimportedpolicydata object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportedpolicydata object.</param>
        /// <returns>True if the inimportedpolicydata object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportedPolicyData inimportedpolicydata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.Delete(inimportedpolicydata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportedPolicyData object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportedpolicydata object from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The id of the inimportedpolicydata object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportedpolicydata history.</param>
        /// <returns>True if the inimportedpolicydata history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportedPolicyData inimportedpolicydata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.DeleteHistory(inimportedpolicydata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportedPolicyData history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportedpolicydata object from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The id of the inimportedpolicydata object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportedpolicydata object.</param>
        /// <returns>True if the inimportedpolicydata object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportedPolicyData inimportedpolicydata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.UnDelete(inimportedpolicydata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportedPolicyData object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportedpolicydata object from the data reader.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportedPolicyData inimportedpolicydata, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportedpolicydata.IsLoaded = true;
                    inimportedpolicydata.CommenceDate = reader["CommenceDate"] != DBNull.Value ? (DateTime)reader["CommenceDate"] : (DateTime?)null;
                    inimportedpolicydata.AppSignDate = reader["AppSignDate"] != DBNull.Value ? (DateTime)reader["AppSignDate"] : (DateTime?)null;
                    inimportedpolicydata.ContractPremium = reader["ContractPremium"] != DBNull.Value ? (decimal)reader["ContractPremium"] : (decimal?)null;
                    inimportedpolicydata.ContractTerm = reader["ContractTerm"] != DBNull.Value ? (int)reader["ContractTerm"] : (int?)null;
                    inimportedpolicydata.LapseDate = reader["LapseDate"] != DBNull.Value ? (DateTime)reader["LapseDate"] : (DateTime?)null;
                    inimportedpolicydata.LA1CancerCover = reader["LA1CancerCover"] != DBNull.Value ? (decimal)reader["LA1CancerCover"] : (decimal?)null;
                    inimportedpolicydata.LA1CancerPremium = reader["LA1CancerPremium"] != DBNull.Value ? (decimal)reader["LA1CancerPremium"] : (decimal?)null;
                    inimportedpolicydata.LA1AccidentalDeathCover = reader["LA1AccidentalDeathCover"] != DBNull.Value ? (decimal)reader["LA1AccidentalDeathCover"] : (decimal?)null;
                    inimportedpolicydata.LA1AccidentalDeathPremium = reader["LA1AccidentalDeathPremium"] != DBNull.Value ? (decimal)reader["LA1AccidentalDeathPremium"] : (decimal?)null;
                    inimportedpolicydata.LA1DisabilityCover = reader["LA1DisabilityCover"] != DBNull.Value ? (decimal)reader["LA1DisabilityCover"] : (decimal?)null;
                    inimportedpolicydata.LA1DisabilityPremium = reader["LA1DisabilityPremium"] != DBNull.Value ? (decimal)reader["LA1DisabilityPremium"] : (decimal?)null;
                    inimportedpolicydata.LA1FuneralCover = reader["LA1FuneralCover"] != DBNull.Value ? (decimal)reader["LA1FuneralCover"] : (decimal?)null;
                    inimportedpolicydata.LA1FuneralPremium = reader["LA1FuneralPremium"] != DBNull.Value ? (decimal)reader["LA1FuneralPremium"] : (decimal?)null;
                    inimportedpolicydata.LA2CancerCover = reader["LA2CancerCover"] != DBNull.Value ? (decimal)reader["LA2CancerCover"] : (decimal?)null;
                    inimportedpolicydata.LA2CancerPremium = reader["LA2CancerPremium"] != DBNull.Value ? (decimal)reader["LA2CancerPremium"] : (decimal?)null;
                    inimportedpolicydata.LA2AccidentalDeathCover = reader["LA2AccidentalDeathCover"] != DBNull.Value ? (decimal)reader["LA2AccidentalDeathCover"] : (decimal?)null;
                    inimportedpolicydata.LA2AccidentalDeathPremium = reader["LA2AccidentalDeathPremium"] != DBNull.Value ? (decimal)reader["LA2AccidentalDeathPremium"] : (decimal?)null;
                    inimportedpolicydata.LA2DisabilityCover = reader["LA2DisabilityCover"] != DBNull.Value ? (decimal)reader["LA2DisabilityCover"] : (decimal?)null;
                    inimportedpolicydata.LA2DisabilityPremium = reader["LA2DisabilityPremium"] != DBNull.Value ? (decimal)reader["LA2DisabilityPremium"] : (decimal?)null;
                    inimportedpolicydata.LA2FuneralCover = reader["LA2FuneralCover"] != DBNull.Value ? (decimal)reader["LA2FuneralCover"] : (decimal?)null;
                    inimportedpolicydata.LA2FuneralPremium = reader["LA2FuneralPremium"] != DBNull.Value ? (decimal)reader["LA2FuneralPremium"] : (decimal?)null;
                    inimportedpolicydata.KidsCancerCover = reader["KidsCancerCover"] != DBNull.Value ? (decimal)reader["KidsCancerCover"] : (decimal?)null;
                    inimportedpolicydata.KidsCancerPremium = reader["KidsCancerPremium"] != DBNull.Value ? (decimal)reader["KidsCancerPremium"] : (decimal?)null;
                    inimportedpolicydata.KidsDisabilityCover = reader["KidsDisabilityCover"] != DBNull.Value ? (decimal)reader["KidsDisabilityCover"] : (decimal?)null;
                    inimportedpolicydata.KidsDisabilityPremium = reader["KidsDisabilityPremium"] != DBNull.Value ? (decimal)reader["KidsDisabilityPremium"] : (decimal?)null;
                    inimportedpolicydata.PolicyFee = reader["PolicyFee"] != DBNull.Value ? (decimal)reader["PolicyFee"] : (decimal?)null;
                    inimportedpolicydata.MoneyBackPremium = reader["MoneyBackPremium"] != DBNull.Value ? (decimal)reader["MoneyBackPremium"] : (decimal?)null;
                    inimportedpolicydata.MoneyBackTerm = reader["MoneyBackTerm"] != DBNull.Value ? (int)reader["MoneyBackTerm"] : (int?)null;
                    inimportedpolicydata.StampDate = (DateTime)reader["StampDate"];
                    inimportedpolicydata.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportedPolicyData does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportedPolicyData object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportedpolicydata object from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata to fill.</param>
        internal static void Fill(INImportedPolicyData inimportedpolicydata)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.Fill(inimportedpolicydata, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportedpolicydata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportedPolicyData object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportedpolicydata object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportedPolicyData inimportedpolicydata)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.FillData(inimportedpolicydata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportedPolicyData object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportedpolicydata object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportedpolicydata">The inimportedpolicydata to fill from history.</param>
        internal static void FillHistory(INImportedPolicyData inimportedpolicydata, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.FillHistory(inimportedpolicydata, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportedpolicydata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportedPolicyData object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportedpolicydata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportedPolicyDataCollection List(bool showDeleted, string connectionName)
        {
            INImportedPolicyDataCollection collection = new INImportedPolicyDataCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportedPolicyDataQueries.ListDeleted() : INImportedPolicyDataQueries.List(), null);
                while (reader.Read())
                {
                    INImportedPolicyData inimportedpolicydata = new INImportedPolicyData((long)reader["ID"]);
                    inimportedpolicydata.ConnectionName = connectionName;
                    collection.Add(inimportedpolicydata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportedPolicyData objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportedpolicydata objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportedPolicyDataQueries.ListDeleted() : INImportedPolicyDataQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportedPolicyData objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportedpolicydata object from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportedPolicyData inimportedpolicydata)
        {
            INImportedPolicyDataCollection collection = new INImportedPolicyDataCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.ListHistory(inimportedpolicydata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportedPolicyData in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportedpolicydata object to the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The INImportedPolicyData object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportedpolicydata was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportedPolicyData inimportedpolicydata)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportedpolicydata.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.Save(inimportedpolicydata, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportedpolicydata.ConnectionName, INImportedPolicyDataQueries.Save(inimportedpolicydata, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportedpolicydata.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportedpolicydata.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportedPolicyData object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportedpolicydata objects in the database.
        /// </summary>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="appsigndate">The appsigndate search criteria.</param>
        /// <param name="contractpremium">The contractpremium search criteria.</param>
        /// <param name="contractterm">The contractterm search criteria.</param>
        /// <param name="lapsedate">The lapsedate search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1accidentaldeathcover">The la1accidentaldeathcover search criteria.</param>
        /// <param name="la1accidentaldeathpremium">The la1accidentaldeathpremium search criteria.</param>
        /// <param name="la1disabilitycover">The la1disabilitycover search criteria.</param>
        /// <param name="la1disabilitypremium">The la1disabilitypremium search criteria.</param>
        /// <param name="la1funeralcover">The la1funeralcover search criteria.</param>
        /// <param name="la1funeralpremium">The la1funeralpremium search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2accidentaldeathcover">The la2accidentaldeathcover search criteria.</param>
        /// <param name="la2accidentaldeathpremium">The la2accidentaldeathpremium search criteria.</param>
        /// <param name="la2disabilitycover">The la2disabilitycover search criteria.</param>
        /// <param name="la2disabilitypremium">The la2disabilitypremium search criteria.</param>
        /// <param name="la2funeralcover">The la2funeralcover search criteria.</param>
        /// <param name="la2funeralpremium">The la2funeralpremium search criteria.</param>
        /// <param name="kidscancercover">The kidscancercover search criteria.</param>
        /// <param name="kidscancerpremium">The kidscancerpremium search criteria.</param>
        /// <param name="kidsdisabilitycover">The kidsdisabilitycover search criteria.</param>
        /// <param name="kidsdisabilitypremium">The kidsdisabilitypremium search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="moneybackpremium">The moneybackpremium search criteria.</param>
        /// <param name="moneybackterm">The moneybackterm search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportedPolicyDataCollection Search(DateTime? commencedate, DateTime? appsigndate, decimal? contractpremium, int? contractterm, DateTime? lapsedate, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1disabilitycover, decimal? la1disabilitypremium, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2disabilitycover, decimal? la2disabilitypremium, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? kidscancercover, decimal? kidscancerpremium, decimal? kidsdisabilitycover, decimal? kidsdisabilitypremium, decimal? policyfee, decimal? moneybackpremium, int? moneybackterm, string connectionName)
        {
            INImportedPolicyDataCollection collection = new INImportedPolicyDataCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportedPolicyDataQueries.Search(commencedate, appsigndate, contractpremium, contractterm, lapsedate, la1cancercover, la1cancerpremium, la1accidentaldeathcover, la1accidentaldeathpremium, la1disabilitycover, la1disabilitypremium, la1funeralcover, la1funeralpremium, la2cancercover, la2cancerpremium, la2accidentaldeathcover, la2accidentaldeathpremium, la2disabilitycover, la2disabilitypremium, la2funeralcover, la2funeralpremium, kidscancercover, kidscancerpremium, kidsdisabilitycover, kidsdisabilitypremium, policyfee, moneybackpremium, moneybackterm), null);
                while (reader.Read())
                {
                    INImportedPolicyData inimportedpolicydata = new INImportedPolicyData((long)reader["ID"]);
                    inimportedpolicydata.ConnectionName = connectionName;
                    collection.Add(inimportedpolicydata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportedPolicyData objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportedpolicydata objects in the database.
        /// </summary>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="appsigndate">The appsigndate search criteria.</param>
        /// <param name="contractpremium">The contractpremium search criteria.</param>
        /// <param name="contractterm">The contractterm search criteria.</param>
        /// <param name="lapsedate">The lapsedate search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1accidentaldeathcover">The la1accidentaldeathcover search criteria.</param>
        /// <param name="la1accidentaldeathpremium">The la1accidentaldeathpremium search criteria.</param>
        /// <param name="la1disabilitycover">The la1disabilitycover search criteria.</param>
        /// <param name="la1disabilitypremium">The la1disabilitypremium search criteria.</param>
        /// <param name="la1funeralcover">The la1funeralcover search criteria.</param>
        /// <param name="la1funeralpremium">The la1funeralpremium search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2accidentaldeathcover">The la2accidentaldeathcover search criteria.</param>
        /// <param name="la2accidentaldeathpremium">The la2accidentaldeathpremium search criteria.</param>
        /// <param name="la2disabilitycover">The la2disabilitycover search criteria.</param>
        /// <param name="la2disabilitypremium">The la2disabilitypremium search criteria.</param>
        /// <param name="la2funeralcover">The la2funeralcover search criteria.</param>
        /// <param name="la2funeralpremium">The la2funeralpremium search criteria.</param>
        /// <param name="kidscancercover">The kidscancercover search criteria.</param>
        /// <param name="kidscancerpremium">The kidscancerpremium search criteria.</param>
        /// <param name="kidsdisabilitycover">The kidsdisabilitycover search criteria.</param>
        /// <param name="kidsdisabilitypremium">The kidsdisabilitypremium search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="moneybackpremium">The moneybackpremium search criteria.</param>
        /// <param name="moneybackterm">The moneybackterm search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(DateTime? commencedate, DateTime? appsigndate, decimal? contractpremium, int? contractterm, DateTime? lapsedate, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1disabilitycover, decimal? la1disabilitypremium, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2disabilitycover, decimal? la2disabilitypremium, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? kidscancercover, decimal? kidscancerpremium, decimal? kidsdisabilitycover, decimal? kidsdisabilitypremium, decimal? policyfee, decimal? moneybackpremium, int? moneybackterm, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportedPolicyDataQueries.Search(commencedate, appsigndate, contractpremium, contractterm, lapsedate, la1cancercover, la1cancerpremium, la1accidentaldeathcover, la1accidentaldeathpremium, la1disabilitycover, la1disabilitypremium, la1funeralcover, la1funeralpremium, la2cancercover, la2cancerpremium, la2accidentaldeathcover, la2accidentaldeathpremium, la2disabilitycover, la2disabilitypremium, la2funeralcover, la2funeralpremium, kidscancercover, kidscancerpremium, kidsdisabilitycover, kidsdisabilitypremium, policyfee, moneybackpremium, moneybackterm), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportedPolicyData objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportedpolicydata objects in the database.
        /// </summary>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="appsigndate">The appsigndate search criteria.</param>
        /// <param name="contractpremium">The contractpremium search criteria.</param>
        /// <param name="contractterm">The contractterm search criteria.</param>
        /// <param name="lapsedate">The lapsedate search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1accidentaldeathcover">The la1accidentaldeathcover search criteria.</param>
        /// <param name="la1accidentaldeathpremium">The la1accidentaldeathpremium search criteria.</param>
        /// <param name="la1disabilitycover">The la1disabilitycover search criteria.</param>
        /// <param name="la1disabilitypremium">The la1disabilitypremium search criteria.</param>
        /// <param name="la1funeralcover">The la1funeralcover search criteria.</param>
        /// <param name="la1funeralpremium">The la1funeralpremium search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2accidentaldeathcover">The la2accidentaldeathcover search criteria.</param>
        /// <param name="la2accidentaldeathpremium">The la2accidentaldeathpremium search criteria.</param>
        /// <param name="la2disabilitycover">The la2disabilitycover search criteria.</param>
        /// <param name="la2disabilitypremium">The la2disabilitypremium search criteria.</param>
        /// <param name="la2funeralcover">The la2funeralcover search criteria.</param>
        /// <param name="la2funeralpremium">The la2funeralpremium search criteria.</param>
        /// <param name="kidscancercover">The kidscancercover search criteria.</param>
        /// <param name="kidscancerpremium">The kidscancerpremium search criteria.</param>
        /// <param name="kidsdisabilitycover">The kidsdisabilitycover search criteria.</param>
        /// <param name="kidsdisabilitypremium">The kidsdisabilitypremium search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="moneybackpremium">The moneybackpremium search criteria.</param>
        /// <param name="moneybackterm">The moneybackterm search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportedPolicyData SearchOne(DateTime? commencedate, DateTime? appsigndate, decimal? contractpremium, int? contractterm, DateTime? lapsedate, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1disabilitycover, decimal? la1disabilitypremium, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2disabilitycover, decimal? la2disabilitypremium, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? kidscancercover, decimal? kidscancerpremium, decimal? kidsdisabilitycover, decimal? kidsdisabilitypremium, decimal? policyfee, decimal? moneybackpremium, int? moneybackterm, string connectionName)
        {
            INImportedPolicyData inimportedpolicydata = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportedPolicyDataQueries.Search(commencedate, appsigndate, contractpremium, contractterm, lapsedate, la1cancercover, la1cancerpremium, la1accidentaldeathcover, la1accidentaldeathpremium, la1disabilitycover, la1disabilitypremium, la1funeralcover, la1funeralpremium, la2cancercover, la2cancerpremium, la2accidentaldeathcover, la2accidentaldeathpremium, la2disabilitycover, la2disabilitypremium, la2funeralcover, la2funeralpremium, kidscancercover, kidscancerpremium, kidsdisabilitycover, kidsdisabilitypremium, policyfee, moneybackpremium, moneybackterm), null);
                if (reader.Read())
                {
                    inimportedpolicydata = new INImportedPolicyData((long)reader["ID"]);
                    inimportedpolicydata.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportedPolicyData objects in the database", ex);
            }
            return inimportedpolicydata;
        }
        #endregion
    }
}
