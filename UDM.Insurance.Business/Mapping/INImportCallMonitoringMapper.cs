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
    /// Contains methods to fill, save and delete inimportcallmonitoring objects.
    /// </summary>
    public partial class INImportCallMonitoringMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimportcallmonitoring object from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The id of the inimportcallmonitoring object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimportcallmonitoring object.</param>
        /// <returns>True if the inimportcallmonitoring object was deleted successfully, else false.</returns>
        internal static bool Delete(INImportCallMonitoring inimportcallmonitoring)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.Delete(inimportcallmonitoring, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImportCallMonitoring object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimportcallmonitoring object from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The id of the inimportcallmonitoring object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimportcallmonitoring history.</param>
        /// <returns>True if the inimportcallmonitoring history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImportCallMonitoring inimportcallmonitoring)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.DeleteHistory(inimportcallmonitoring, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImportCallMonitoring history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimportcallmonitoring object from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The id of the inimportcallmonitoring object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimportcallmonitoring object.</param>
        /// <returns>True if the inimportcallmonitoring object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImportCallMonitoring inimportcallmonitoring)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.UnDelete(inimportcallmonitoring, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImportCallMonitoring object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimportcallmonitoring object from the data reader.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImportCallMonitoring inimportcallmonitoring, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimportcallmonitoring.IsLoaded = true;
                    inimportcallmonitoring.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (long)reader["FKINImportID"] : (long?)null;
                    inimportcallmonitoring.IsBankingDetailsCapturedCorrectly = reader["IsBankingDetailsCapturedCorrectly"] != DBNull.Value ? (bool)reader["IsBankingDetailsCapturedCorrectly"] : (bool?)null;
                    inimportcallmonitoring.WasAccountVerified = reader["WasAccountVerified"] != DBNull.Value ? (bool)reader["WasAccountVerified"] : (bool?)null;
                    inimportcallmonitoring.WasDebitDateConfirmed = reader["WasDebitDateConfirmed"] != DBNull.Value ? (bool)reader["WasDebitDateConfirmed"] : (bool?)null;
                    inimportcallmonitoring.IsAccountInClientsName = reader["IsAccountInClientsName"] != DBNull.Value ? (bool)reader["IsAccountInClientsName"] : (bool?)null;
                    inimportcallmonitoring.DoesClientHaveSigningPower = reader["DoesClientHaveSigningPower"] != DBNull.Value ? (bool)reader["DoesClientHaveSigningPower"] : (bool?)null;
                    inimportcallmonitoring.WasCorrectClosureQuestionAsked = reader["WasCorrectClosureQuestionAsked"] != DBNull.Value ? (bool)reader["WasCorrectClosureQuestionAsked"] : (bool?)null;
                    inimportcallmonitoring.WasResponseClearAndPositive = reader["WasResponseClearAndPositive"] != DBNull.Value ? (bool)reader["WasResponseClearAndPositive"] : (bool?)null;
                    inimportcallmonitoring.WasUDMAndPLMentionedAsFSPs = reader["WasUDMAndPLMentionedAsFSPs"] != DBNull.Value ? (bool)reader["WasUDMAndPLMentionedAsFSPs"] : (bool?)null;
                    inimportcallmonitoring.WasDebitAmountMentionedCorrectly = reader["WasDebitAmountMentionedCorrectly"] != DBNull.Value ? (bool)reader["WasDebitAmountMentionedCorrectly"] : (bool?)null;
                    inimportcallmonitoring.WasFirstDebitDateExplainedCorrectly = reader["WasFirstDebitDateExplainedCorrectly"] != DBNull.Value ? (bool)reader["WasFirstDebitDateExplainedCorrectly"] : (bool?)null;
                    inimportcallmonitoring.WasCorrectCoverCommencementDateMentioned = reader["WasCorrectCoverCommencementDateMentioned"] != DBNull.Value ? (bool)reader["WasCorrectCoverCommencementDateMentioned"] : (bool?)null;
                    inimportcallmonitoring.WasNonPaymentProcedureExplained = reader["WasNonPaymentProcedureExplained"] != DBNull.Value ? (bool)reader["WasNonPaymentProcedureExplained"] : (bool?)null;
                    inimportcallmonitoring.WasAnnualIncreaseExplained = reader["WasAnnualIncreaseExplained"] != DBNull.Value ? (bool)reader["WasAnnualIncreaseExplained"] : (bool?)null;
                    inimportcallmonitoring.WasCorrectQuestionAskedBumpUpClosure = reader["WasCorrectQuestionAskedBumpUpClosure"] != DBNull.Value ? (bool)reader["WasCorrectQuestionAskedBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.WasResponseClearAndPositiveBumpUpClosure = reader["WasResponseClearAndPositiveBumpUpClosure"] != DBNull.Value ? (bool)reader["WasResponseClearAndPositiveBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.WasUDMAndPLMentionedAsFSPsBumpUpClosure = reader["WasUDMAndPLMentionedAsFSPsBumpUpClosure"] != DBNull.Value ? (bool)reader["WasUDMAndPLMentionedAsFSPsBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.WasDebitAmountMentionedCorrectlyBumpUpClosure = reader["WasDebitAmountMentionedCorrectlyBumpUpClosure"] != DBNull.Value ? (bool)reader["WasDebitAmountMentionedCorrectlyBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.WasFirstDebitDateExplainedCorrectlyBumpUpClosure = reader["WasFirstDebitDateExplainedCorrectlyBumpUpClosure"] != DBNull.Value ? (bool)reader["WasFirstDebitDateExplainedCorrectlyBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.WasCorrectCoverCommencementDateMentionedBumpUpClosure = reader["WasCorrectCoverCommencementDateMentionedBumpUpClosure"] != DBNull.Value ? (bool)reader["WasCorrectCoverCommencementDateMentionedBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.WasNonPaymentProcedureExplainedBumpUpClosure = reader["WasNonPaymentProcedureExplainedBumpUpClosure"] != DBNull.Value ? (bool)reader["WasNonPaymentProcedureExplainedBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.WasAnnualIncreaseExplainedBumpUpClosure = reader["WasAnnualIncreaseExplainedBumpUpClosure"] != DBNull.Value ? (bool)reader["WasAnnualIncreaseExplainedBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.FKINCallMonitoringOutcomeID = reader["FKINCallMonitoringOutcomeID"] != DBNull.Value ? (long)reader["FKINCallMonitoringOutcomeID"] : (long?)null;
                    inimportcallmonitoring.Comments = reader["Comments"] != DBNull.Value ? (string)reader["Comments"] : (string)null;
                    inimportcallmonitoring.FKCallMonitoringUserID = reader["FKCallMonitoringUserID"] != DBNull.Value ? (long)reader["FKCallMonitoringUserID"] : (long?)null;
                    inimportcallmonitoring.WasCallEvaluatedBySecondaryUser = reader["WasCallEvaluatedBySecondaryUser"] != DBNull.Value ? (bool)reader["WasCallEvaluatedBySecondaryUser"] : (bool?)null;
                    inimportcallmonitoring.FKSecondaryCallMonitoringUserID = reader["FKSecondaryCallMonitoringUserID"] != DBNull.Value ? (long)reader["FKSecondaryCallMonitoringUserID"] : (long?)null;
                    inimportcallmonitoring.FKINCallAssessmentOutcomeID = reader["FKINCallAssessmentOutcomeID"] != DBNull.Value ? (long)reader["FKINCallAssessmentOutcomeID"] : (long?)null;
                    inimportcallmonitoring.ExclusionsExplained = reader["ExclusionsExplained"] != DBNull.Value ? (bool)reader["ExclusionsExplained"] : (bool?)null;
                    inimportcallmonitoring.ExclusionsExplainedBumpUpClosure = reader["ExclusionsExplainedBumpUpClosure"] != DBNull.Value ? (bool)reader["ExclusionsExplainedBumpUpClosure"] : (bool?)null;
                    inimportcallmonitoring.IsRecoveredSale = reader["IsRecoveredSale"] != DBNull.Value ? (bool)reader["IsRecoveredSale"] : (bool?)null;
                    inimportcallmonitoring.WasMoneyBackVsVoucherBenefitsExplainedCorrectly = reader["WasMoneyBackVsVoucherBenefitsExplainedCorrectly"] != DBNull.Value ? (bool)reader["WasMoneyBackVsVoucherBenefitsExplainedCorrectly"] : (bool?)null;
                    inimportcallmonitoring.IsCallMonitored = reader["IsCallMonitored"] != DBNull.Value ? (bool)reader["IsCallMonitored"] : (bool?)null;
                    inimportcallmonitoring.WasClearYesGivenInSalesQuestion = reader["WasClearYesGivenInSalesQuestion"] != DBNull.Value ? (bool)reader["WasClearYesGivenInSalesQuestion"] : (bool?)null;
                    inimportcallmonitoring.WasPermissionQuestionAsked = reader["WasPermissionQuestionAsked"] != DBNull.Value ? (bool)reader["WasPermissionQuestionAsked"] : (bool?)null;
                    inimportcallmonitoring.WasNextOfKinQuestionAsked = reader["WasNextOfKinQuestionAsked"] != DBNull.Value ? (bool)reader["WasNextOfKinQuestionAsked"] : (bool?)null;
                    inimportcallmonitoring.FKTertiaryCallMonitoringUserID = reader["FKTertiaryCallMonitoringUserID"] != DBNull.Value ? (long)reader["FKTertiaryCallMonitoringUserID"] : (long?)null;
                    inimportcallmonitoring.IsTSRBUSavedCarriedForward = reader["IsTSRBUSavedCarriedForward"] != DBNull.Value ? (bool)reader["IsTSRBUSavedCarriedForward"] : (bool?)null;
                    inimportcallmonitoring.TSRBUSavedCarriedForwardDate = reader["TSRBUSavedCarriedForwardDate"] != DBNull.Value ? (DateTime)reader["TSRBUSavedCarriedForwardDate"] : (DateTime?)null;
                    inimportcallmonitoring.TSRBUSavedCarriedForwardAssignedByUserID = reader["TSRBUSavedCarriedForwardAssignedByUserID"] != DBNull.Value ? (long)reader["TSRBUSavedCarriedForwardAssignedByUserID"] : (long?)null;
                    inimportcallmonitoring.CallMonitoredDate = reader["CallMonitoredDate"] != DBNull.Value ? (DateTime)reader["CallMonitoredDate"] : (DateTime?)null;
                    inimportcallmonitoring.StampDate = (DateTime)reader["StampDate"];
                    inimportcallmonitoring.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImportCallMonitoring does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportCallMonitoring object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportcallmonitoring object from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring to fill.</param>
        internal static void Fill(INImportCallMonitoring inimportcallmonitoring)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.Fill(inimportcallmonitoring, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportcallmonitoring, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportCallMonitoring object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimportcallmonitoring object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImportCallMonitoring inimportcallmonitoring)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.FillData(inimportcallmonitoring, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImportCallMonitoring object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimportcallmonitoring object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring to fill from history.</param>
        internal static void FillHistory(INImportCallMonitoring inimportcallmonitoring, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.FillHistory(inimportcallmonitoring, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimportcallmonitoring, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImportCallMonitoring object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportcallmonitoring objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportCallMonitoringCollection List(bool showDeleted, string connectionName)
        {
            INImportCallMonitoringCollection collection = new INImportCallMonitoringCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportCallMonitoringQueries.ListDeleted() : INImportCallMonitoringQueries.List(), null);
                while (reader.Read())
                {
                    INImportCallMonitoring inimportcallmonitoring = new INImportCallMonitoring((long)reader["ID"]);
                    inimportcallmonitoring.ConnectionName = connectionName;
                    collection.Add(inimportcallmonitoring);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportCallMonitoring objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimportcallmonitoring objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportCallMonitoringQueries.ListDeleted() : INImportCallMonitoringQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImportCallMonitoring objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimportcallmonitoring object from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImportCallMonitoring inimportcallmonitoring)
        {
            INImportCallMonitoringCollection collection = new INImportCallMonitoringCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.ListHistory(inimportcallmonitoring, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImportCallMonitoring in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimportcallmonitoring object to the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The INImportCallMonitoring object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimportcallmonitoring was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImportCallMonitoring inimportcallmonitoring)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimportcallmonitoring.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.Save(inimportcallmonitoring, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimportcallmonitoring.ConnectionName, INImportCallMonitoringQueries.Save(inimportcallmonitoring, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimportcallmonitoring.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimportcallmonitoring.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImportCallMonitoring object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportcallmonitoring objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="isbankingdetailscapturedcorrectly">The isbankingdetailscapturedcorrectly search criteria.</param>
        /// <param name="wasaccountverified">The wasaccountverified search criteria.</param>
        /// <param name="wasdebitdateconfirmed">The wasdebitdateconfirmed search criteria.</param>
        /// <param name="isaccountinclientsname">The isaccountinclientsname search criteria.</param>
        /// <param name="doesclienthavesigningpower">The doesclienthavesigningpower search criteria.</param>
        /// <param name="wascorrectclosurequestionasked">The wascorrectclosurequestionasked search criteria.</param>
        /// <param name="wasresponseclearandpositive">The wasresponseclearandpositive search criteria.</param>
        /// <param name="wasudmandplmentionedasfsps">The wasudmandplmentionedasfsps search criteria.</param>
        /// <param name="wasdebitamountmentionedcorrectly">The wasdebitamountmentionedcorrectly search criteria.</param>
        /// <param name="wasfirstdebitdateexplainedcorrectly">The wasfirstdebitdateexplainedcorrectly search criteria.</param>
        /// <param name="wascorrectcovercommencementdatementioned">The wascorrectcovercommencementdatementioned search criteria.</param>
        /// <param name="wasnonpaymentprocedureexplained">The wasnonpaymentprocedureexplained search criteria.</param>
        /// <param name="wasannualincreaseexplained">The wasannualincreaseexplained search criteria.</param>
        /// <param name="wascorrectquestionaskedbumpupclosure">The wascorrectquestionaskedbumpupclosure search criteria.</param>
        /// <param name="wasresponseclearandpositivebumpupclosure">The wasresponseclearandpositivebumpupclosure search criteria.</param>
        /// <param name="wasudmandplmentionedasfspsbumpupclosure">The wasudmandplmentionedasfspsbumpupclosure search criteria.</param>
        /// <param name="wasdebitamountmentionedcorrectlybumpupclosure">The wasdebitamountmentionedcorrectlybumpupclosure search criteria.</param>
        /// <param name="wasfirstdebitdateexplainedcorrectlybumpupclosure">The wasfirstdebitdateexplainedcorrectlybumpupclosure search criteria.</param>
        /// <param name="wascorrectcovercommencementdatementionedbumpupclosure">The wascorrectcovercommencementdatementionedbumpupclosure search criteria.</param>
        /// <param name="wasnonpaymentprocedureexplainedbumpupclosure">The wasnonpaymentprocedureexplainedbumpupclosure search criteria.</param>
        /// <param name="wasannualincreaseexplainedbumpupclosure">The wasannualincreaseexplainedbumpupclosure search criteria.</param>
        /// <param name="fkincallmonitoringoutcomeid">The fkincallmonitoringoutcomeid search criteria.</param>
        /// <param name="comments">The comments search criteria.</param>
        /// <param name="fkcallmonitoringuserid">The fkcallmonitoringuserid search criteria.</param>
        /// <param name="wascallevaluatedbysecondaryuser">The wascallevaluatedbysecondaryuser search criteria.</param>
        /// <param name="fksecondarycallmonitoringuserid">The fksecondarycallmonitoringuserid search criteria.</param>
        /// <param name="fkincallassessmentoutcomeid">The fkincallassessmentoutcomeid search criteria.</param>
        /// <param name="exclusionsexplained">The exclusionsexplained search criteria.</param>
        /// <param name="exclusionsexplainedbumpupclosure">The exclusionsexplainedbumpupclosure search criteria.</param>
        /// <param name="isrecoveredsale">The isrecoveredsale search criteria.</param>
        /// <param name="wasmoneybackvsvoucherbenefitsexplainedcorrectly">The wasmoneybackvsvoucherbenefitsexplainedcorrectly search criteria.</param>
        /// <param name="iscallmonitored">The iscallmonitored search criteria.</param>
        /// <param name="wasclearyesgiveninsalesquestion">The wasclearyesgiveninsalesquestion search criteria.</param>
        /// <param name="waspermissionquestionasked">The waspermissionquestionasked search criteria.</param>
        /// <param name="wasnextofkinquestionasked">The wasnextofkinquestionasked search criteria.</param>
        /// <param name="fktertiarycallmonitoringuserid">The fktertiarycallmonitoringuserid search criteria.</param>
        /// <param name="istsrbusavedcarriedforward">The istsrbusavedcarriedforward search criteria.</param>
        /// <param name="tsrbusavedcarriedforwarddate">The tsrbusavedcarriedforwarddate search criteria.</param>
        /// <param name="tsrbusavedcarriedforwardassignedbyuserid">The tsrbusavedcarriedforwardassignedbyuserid search criteria.</param>
        /// <param name="callmonitoreddate">The callmonitoreddate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportCallMonitoringCollection Search(long? fkinimportid, bool? isbankingdetailscapturedcorrectly, bool? wasaccountverified, bool? wasdebitdateconfirmed, bool? isaccountinclientsname, bool? doesclienthavesigningpower, bool? wascorrectclosurequestionasked, bool? wasresponseclearandpositive, bool? wasudmandplmentionedasfsps, bool? wasdebitamountmentionedcorrectly, bool? wasfirstdebitdateexplainedcorrectly, bool? wascorrectcovercommencementdatementioned, bool? wasnonpaymentprocedureexplained, bool? wasannualincreaseexplained, bool? wascorrectquestionaskedbumpupclosure, bool? wasresponseclearandpositivebumpupclosure, bool? wasudmandplmentionedasfspsbumpupclosure, bool? wasdebitamountmentionedcorrectlybumpupclosure, bool? wasfirstdebitdateexplainedcorrectlybumpupclosure, bool? wascorrectcovercommencementdatementionedbumpupclosure, bool? wasnonpaymentprocedureexplainedbumpupclosure, bool? wasannualincreaseexplainedbumpupclosure, long? fkincallmonitoringoutcomeid, string comments, long? fkcallmonitoringuserid, bool? wascallevaluatedbysecondaryuser, long? fksecondarycallmonitoringuserid, long? fkincallassessmentoutcomeid, bool? exclusionsexplained, bool? exclusionsexplainedbumpupclosure, bool? isrecoveredsale, bool? wasmoneybackvsvoucherbenefitsexplainedcorrectly, bool? iscallmonitored, bool? wasclearyesgiveninsalesquestion, bool? waspermissionquestionasked, bool? wasnextofkinquestionasked, long? fktertiarycallmonitoringuserid, bool? istsrbusavedcarriedforward, DateTime? tsrbusavedcarriedforwarddate, long? tsrbusavedcarriedforwardassignedbyuserid, DateTime? callmonitoreddate, string connectionName)
        {
            INImportCallMonitoringCollection collection = new INImportCallMonitoringCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportCallMonitoringQueries.Search(fkinimportid, isbankingdetailscapturedcorrectly, wasaccountverified, wasdebitdateconfirmed, isaccountinclientsname, doesclienthavesigningpower, wascorrectclosurequestionasked, wasresponseclearandpositive, wasudmandplmentionedasfsps, wasdebitamountmentionedcorrectly, wasfirstdebitdateexplainedcorrectly, wascorrectcovercommencementdatementioned, wasnonpaymentprocedureexplained, wasannualincreaseexplained, wascorrectquestionaskedbumpupclosure, wasresponseclearandpositivebumpupclosure, wasudmandplmentionedasfspsbumpupclosure, wasdebitamountmentionedcorrectlybumpupclosure, wasfirstdebitdateexplainedcorrectlybumpupclosure, wascorrectcovercommencementdatementionedbumpupclosure, wasnonpaymentprocedureexplainedbumpupclosure, wasannualincreaseexplainedbumpupclosure, fkincallmonitoringoutcomeid, comments, fkcallmonitoringuserid, wascallevaluatedbysecondaryuser, fksecondarycallmonitoringuserid, fkincallassessmentoutcomeid, exclusionsexplained, exclusionsexplainedbumpupclosure, isrecoveredsale, wasmoneybackvsvoucherbenefitsexplainedcorrectly, iscallmonitored, wasclearyesgiveninsalesquestion, waspermissionquestionasked, wasnextofkinquestionasked, fktertiarycallmonitoringuserid, istsrbusavedcarriedforward, tsrbusavedcarriedforwarddate, tsrbusavedcarriedforwardassignedbyuserid, callmonitoreddate), null);
                while (reader.Read())
                {
                    INImportCallMonitoring inimportcallmonitoring = new INImportCallMonitoring((long)reader["ID"]);
                    inimportcallmonitoring.ConnectionName = connectionName;
                    collection.Add(inimportcallmonitoring);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportCallMonitoring objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimportcallmonitoring objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="isbankingdetailscapturedcorrectly">The isbankingdetailscapturedcorrectly search criteria.</param>
        /// <param name="wasaccountverified">The wasaccountverified search criteria.</param>
        /// <param name="wasdebitdateconfirmed">The wasdebitdateconfirmed search criteria.</param>
        /// <param name="isaccountinclientsname">The isaccountinclientsname search criteria.</param>
        /// <param name="doesclienthavesigningpower">The doesclienthavesigningpower search criteria.</param>
        /// <param name="wascorrectclosurequestionasked">The wascorrectclosurequestionasked search criteria.</param>
        /// <param name="wasresponseclearandpositive">The wasresponseclearandpositive search criteria.</param>
        /// <param name="wasudmandplmentionedasfsps">The wasudmandplmentionedasfsps search criteria.</param>
        /// <param name="wasdebitamountmentionedcorrectly">The wasdebitamountmentionedcorrectly search criteria.</param>
        /// <param name="wasfirstdebitdateexplainedcorrectly">The wasfirstdebitdateexplainedcorrectly search criteria.</param>
        /// <param name="wascorrectcovercommencementdatementioned">The wascorrectcovercommencementdatementioned search criteria.</param>
        /// <param name="wasnonpaymentprocedureexplained">The wasnonpaymentprocedureexplained search criteria.</param>
        /// <param name="wasannualincreaseexplained">The wasannualincreaseexplained search criteria.</param>
        /// <param name="wascorrectquestionaskedbumpupclosure">The wascorrectquestionaskedbumpupclosure search criteria.</param>
        /// <param name="wasresponseclearandpositivebumpupclosure">The wasresponseclearandpositivebumpupclosure search criteria.</param>
        /// <param name="wasudmandplmentionedasfspsbumpupclosure">The wasudmandplmentionedasfspsbumpupclosure search criteria.</param>
        /// <param name="wasdebitamountmentionedcorrectlybumpupclosure">The wasdebitamountmentionedcorrectlybumpupclosure search criteria.</param>
        /// <param name="wasfirstdebitdateexplainedcorrectlybumpupclosure">The wasfirstdebitdateexplainedcorrectlybumpupclosure search criteria.</param>
        /// <param name="wascorrectcovercommencementdatementionedbumpupclosure">The wascorrectcovercommencementdatementionedbumpupclosure search criteria.</param>
        /// <param name="wasnonpaymentprocedureexplainedbumpupclosure">The wasnonpaymentprocedureexplainedbumpupclosure search criteria.</param>
        /// <param name="wasannualincreaseexplainedbumpupclosure">The wasannualincreaseexplainedbumpupclosure search criteria.</param>
        /// <param name="fkincallmonitoringoutcomeid">The fkincallmonitoringoutcomeid search criteria.</param>
        /// <param name="comments">The comments search criteria.</param>
        /// <param name="fkcallmonitoringuserid">The fkcallmonitoringuserid search criteria.</param>
        /// <param name="wascallevaluatedbysecondaryuser">The wascallevaluatedbysecondaryuser search criteria.</param>
        /// <param name="fksecondarycallmonitoringuserid">The fksecondarycallmonitoringuserid search criteria.</param>
        /// <param name="fkincallassessmentoutcomeid">The fkincallassessmentoutcomeid search criteria.</param>
        /// <param name="exclusionsexplained">The exclusionsexplained search criteria.</param>
        /// <param name="exclusionsexplainedbumpupclosure">The exclusionsexplainedbumpupclosure search criteria.</param>
        /// <param name="isrecoveredsale">The isrecoveredsale search criteria.</param>
        /// <param name="wasmoneybackvsvoucherbenefitsexplainedcorrectly">The wasmoneybackvsvoucherbenefitsexplainedcorrectly search criteria.</param>
        /// <param name="iscallmonitored">The iscallmonitored search criteria.</param>
        /// <param name="wasclearyesgiveninsalesquestion">The wasclearyesgiveninsalesquestion search criteria.</param>
        /// <param name="waspermissionquestionasked">The waspermissionquestionasked search criteria.</param>
        /// <param name="wasnextofkinquestionasked">The wasnextofkinquestionasked search criteria.</param>
        /// <param name="fktertiarycallmonitoringuserid">The fktertiarycallmonitoringuserid search criteria.</param>
        /// <param name="istsrbusavedcarriedforward">The istsrbusavedcarriedforward search criteria.</param>
        /// <param name="tsrbusavedcarriedforwarddate">The tsrbusavedcarriedforwarddate search criteria.</param>
        /// <param name="tsrbusavedcarriedforwardassignedbyuserid">The tsrbusavedcarriedforwardassignedbyuserid search criteria.</param>
        /// <param name="callmonitoreddate">The callmonitoreddate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinimportid, bool? isbankingdetailscapturedcorrectly, bool? wasaccountverified, bool? wasdebitdateconfirmed, bool? isaccountinclientsname, bool? doesclienthavesigningpower, bool? wascorrectclosurequestionasked, bool? wasresponseclearandpositive, bool? wasudmandplmentionedasfsps, bool? wasdebitamountmentionedcorrectly, bool? wasfirstdebitdateexplainedcorrectly, bool? wascorrectcovercommencementdatementioned, bool? wasnonpaymentprocedureexplained, bool? wasannualincreaseexplained, bool? wascorrectquestionaskedbumpupclosure, bool? wasresponseclearandpositivebumpupclosure, bool? wasudmandplmentionedasfspsbumpupclosure, bool? wasdebitamountmentionedcorrectlybumpupclosure, bool? wasfirstdebitdateexplainedcorrectlybumpupclosure, bool? wascorrectcovercommencementdatementionedbumpupclosure, bool? wasnonpaymentprocedureexplainedbumpupclosure, bool? wasannualincreaseexplainedbumpupclosure, long? fkincallmonitoringoutcomeid, string comments, long? fkcallmonitoringuserid, bool? wascallevaluatedbysecondaryuser, long? fksecondarycallmonitoringuserid, long? fkincallassessmentoutcomeid, bool? exclusionsexplained, bool? exclusionsexplainedbumpupclosure, bool? isrecoveredsale, bool? wasmoneybackvsvoucherbenefitsexplainedcorrectly, bool? iscallmonitored, bool? wasclearyesgiveninsalesquestion, bool? waspermissionquestionasked, bool? wasnextofkinquestionasked, long? fktertiarycallmonitoringuserid, bool? istsrbusavedcarriedforward, DateTime? tsrbusavedcarriedforwarddate, long? tsrbusavedcarriedforwardassignedbyuserid, DateTime? callmonitoreddate, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportCallMonitoringQueries.Search(fkinimportid, isbankingdetailscapturedcorrectly, wasaccountverified, wasdebitdateconfirmed, isaccountinclientsname, doesclienthavesigningpower, wascorrectclosurequestionasked, wasresponseclearandpositive, wasudmandplmentionedasfsps, wasdebitamountmentionedcorrectly, wasfirstdebitdateexplainedcorrectly, wascorrectcovercommencementdatementioned, wasnonpaymentprocedureexplained, wasannualincreaseexplained, wascorrectquestionaskedbumpupclosure, wasresponseclearandpositivebumpupclosure, wasudmandplmentionedasfspsbumpupclosure, wasdebitamountmentionedcorrectlybumpupclosure, wasfirstdebitdateexplainedcorrectlybumpupclosure, wascorrectcovercommencementdatementionedbumpupclosure, wasnonpaymentprocedureexplainedbumpupclosure, wasannualincreaseexplainedbumpupclosure, fkincallmonitoringoutcomeid, comments, fkcallmonitoringuserid, wascallevaluatedbysecondaryuser, fksecondarycallmonitoringuserid, fkincallassessmentoutcomeid, exclusionsexplained, exclusionsexplainedbumpupclosure, isrecoveredsale, wasmoneybackvsvoucherbenefitsexplainedcorrectly, iscallmonitored, wasclearyesgiveninsalesquestion, waspermissionquestionasked, wasnextofkinquestionasked, fktertiarycallmonitoringuserid, istsrbusavedcarriedforward, tsrbusavedcarriedforwarddate, tsrbusavedcarriedforwardassignedbyuserid, callmonitoreddate), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportCallMonitoring objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimportcallmonitoring objects in the database.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="isbankingdetailscapturedcorrectly">The isbankingdetailscapturedcorrectly search criteria.</param>
        /// <param name="wasaccountverified">The wasaccountverified search criteria.</param>
        /// <param name="wasdebitdateconfirmed">The wasdebitdateconfirmed search criteria.</param>
        /// <param name="isaccountinclientsname">The isaccountinclientsname search criteria.</param>
        /// <param name="doesclienthavesigningpower">The doesclienthavesigningpower search criteria.</param>
        /// <param name="wascorrectclosurequestionasked">The wascorrectclosurequestionasked search criteria.</param>
        /// <param name="wasresponseclearandpositive">The wasresponseclearandpositive search criteria.</param>
        /// <param name="wasudmandplmentionedasfsps">The wasudmandplmentionedasfsps search criteria.</param>
        /// <param name="wasdebitamountmentionedcorrectly">The wasdebitamountmentionedcorrectly search criteria.</param>
        /// <param name="wasfirstdebitdateexplainedcorrectly">The wasfirstdebitdateexplainedcorrectly search criteria.</param>
        /// <param name="wascorrectcovercommencementdatementioned">The wascorrectcovercommencementdatementioned search criteria.</param>
        /// <param name="wasnonpaymentprocedureexplained">The wasnonpaymentprocedureexplained search criteria.</param>
        /// <param name="wasannualincreaseexplained">The wasannualincreaseexplained search criteria.</param>
        /// <param name="wascorrectquestionaskedbumpupclosure">The wascorrectquestionaskedbumpupclosure search criteria.</param>
        /// <param name="wasresponseclearandpositivebumpupclosure">The wasresponseclearandpositivebumpupclosure search criteria.</param>
        /// <param name="wasudmandplmentionedasfspsbumpupclosure">The wasudmandplmentionedasfspsbumpupclosure search criteria.</param>
        /// <param name="wasdebitamountmentionedcorrectlybumpupclosure">The wasdebitamountmentionedcorrectlybumpupclosure search criteria.</param>
        /// <param name="wasfirstdebitdateexplainedcorrectlybumpupclosure">The wasfirstdebitdateexplainedcorrectlybumpupclosure search criteria.</param>
        /// <param name="wascorrectcovercommencementdatementionedbumpupclosure">The wascorrectcovercommencementdatementionedbumpupclosure search criteria.</param>
        /// <param name="wasnonpaymentprocedureexplainedbumpupclosure">The wasnonpaymentprocedureexplainedbumpupclosure search criteria.</param>
        /// <param name="wasannualincreaseexplainedbumpupclosure">The wasannualincreaseexplainedbumpupclosure search criteria.</param>
        /// <param name="fkincallmonitoringoutcomeid">The fkincallmonitoringoutcomeid search criteria.</param>
        /// <param name="comments">The comments search criteria.</param>
        /// <param name="fkcallmonitoringuserid">The fkcallmonitoringuserid search criteria.</param>
        /// <param name="wascallevaluatedbysecondaryuser">The wascallevaluatedbysecondaryuser search criteria.</param>
        /// <param name="fksecondarycallmonitoringuserid">The fksecondarycallmonitoringuserid search criteria.</param>
        /// <param name="fkincallassessmentoutcomeid">The fkincallassessmentoutcomeid search criteria.</param>
        /// <param name="exclusionsexplained">The exclusionsexplained search criteria.</param>
        /// <param name="exclusionsexplainedbumpupclosure">The exclusionsexplainedbumpupclosure search criteria.</param>
        /// <param name="isrecoveredsale">The isrecoveredsale search criteria.</param>
        /// <param name="wasmoneybackvsvoucherbenefitsexplainedcorrectly">The wasmoneybackvsvoucherbenefitsexplainedcorrectly search criteria.</param>
        /// <param name="iscallmonitored">The iscallmonitored search criteria.</param>
        /// <param name="wasclearyesgiveninsalesquestion">The wasclearyesgiveninsalesquestion search criteria.</param>
        /// <param name="waspermissionquestionasked">The waspermissionquestionasked search criteria.</param>
        /// <param name="wasnextofkinquestionasked">The wasnextofkinquestionasked search criteria.</param>
        /// <param name="fktertiarycallmonitoringuserid">The fktertiarycallmonitoringuserid search criteria.</param>
        /// <param name="istsrbusavedcarriedforward">The istsrbusavedcarriedforward search criteria.</param>
        /// <param name="tsrbusavedcarriedforwarddate">The tsrbusavedcarriedforwarddate search criteria.</param>
        /// <param name="tsrbusavedcarriedforwardassignedbyuserid">The tsrbusavedcarriedforwardassignedbyuserid search criteria.</param>
        /// <param name="callmonitoreddate">The callmonitoreddate search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportCallMonitoring SearchOne(long? fkinimportid, bool? isbankingdetailscapturedcorrectly, bool? wasaccountverified, bool? wasdebitdateconfirmed, bool? isaccountinclientsname, bool? doesclienthavesigningpower, bool? wascorrectclosurequestionasked, bool? wasresponseclearandpositive, bool? wasudmandplmentionedasfsps, bool? wasdebitamountmentionedcorrectly, bool? wasfirstdebitdateexplainedcorrectly, bool? wascorrectcovercommencementdatementioned, bool? wasnonpaymentprocedureexplained, bool? wasannualincreaseexplained, bool? wascorrectquestionaskedbumpupclosure, bool? wasresponseclearandpositivebumpupclosure, bool? wasudmandplmentionedasfspsbumpupclosure, bool? wasdebitamountmentionedcorrectlybumpupclosure, bool? wasfirstdebitdateexplainedcorrectlybumpupclosure, bool? wascorrectcovercommencementdatementionedbumpupclosure, bool? wasnonpaymentprocedureexplainedbumpupclosure, bool? wasannualincreaseexplainedbumpupclosure, long? fkincallmonitoringoutcomeid, string comments, long? fkcallmonitoringuserid, bool? wascallevaluatedbysecondaryuser, long? fksecondarycallmonitoringuserid, long? fkincallassessmentoutcomeid, bool? exclusionsexplained, bool? exclusionsexplainedbumpupclosure, bool? isrecoveredsale, bool? wasmoneybackvsvoucherbenefitsexplainedcorrectly, bool? iscallmonitored, bool? wasclearyesgiveninsalesquestion, bool? waspermissionquestionasked, bool? wasnextofkinquestionasked, long? fktertiarycallmonitoringuserid, bool? istsrbusavedcarriedforward, DateTime? tsrbusavedcarriedforwarddate, long? tsrbusavedcarriedforwardassignedbyuserid, DateTime? callmonitoreddate, string connectionName)
        {
            INImportCallMonitoring inimportcallmonitoring = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportCallMonitoringQueries.Search(fkinimportid, isbankingdetailscapturedcorrectly, wasaccountverified, wasdebitdateconfirmed, isaccountinclientsname, doesclienthavesigningpower, wascorrectclosurequestionasked, wasresponseclearandpositive, wasudmandplmentionedasfsps, wasdebitamountmentionedcorrectly, wasfirstdebitdateexplainedcorrectly, wascorrectcovercommencementdatementioned, wasnonpaymentprocedureexplained, wasannualincreaseexplained, wascorrectquestionaskedbumpupclosure, wasresponseclearandpositivebumpupclosure, wasudmandplmentionedasfspsbumpupclosure, wasdebitamountmentionedcorrectlybumpupclosure, wasfirstdebitdateexplainedcorrectlybumpupclosure, wascorrectcovercommencementdatementionedbumpupclosure, wasnonpaymentprocedureexplainedbumpupclosure, wasannualincreaseexplainedbumpupclosure, fkincallmonitoringoutcomeid, comments, fkcallmonitoringuserid, wascallevaluatedbysecondaryuser, fksecondarycallmonitoringuserid, fkincallassessmentoutcomeid, exclusionsexplained, exclusionsexplainedbumpupclosure, isrecoveredsale, wasmoneybackvsvoucherbenefitsexplainedcorrectly, iscallmonitored, wasclearyesgiveninsalesquestion, waspermissionquestionasked, wasnextofkinquestionasked, fktertiarycallmonitoringuserid, istsrbusavedcarriedforward, tsrbusavedcarriedforwarddate, tsrbusavedcarriedforwardassignedbyuserid, callmonitoreddate), null);
                if (reader.Read())
                {
                    inimportcallmonitoring = new INImportCallMonitoring((long)reader["ID"]);
                    inimportcallmonitoring.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImportCallMonitoring objects in the database", ex);
            }
            return inimportcallmonitoring;
        }
        #endregion
    }
}
