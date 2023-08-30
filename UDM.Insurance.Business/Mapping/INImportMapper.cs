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
    /// Contains methods to fill, save and delete inimport objects.
    /// </summary>
    public partial class INImportMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inimport object from the database.
        /// </summary>
        /// <param name="inimport">The id of the inimport object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inimport object.</param>
        /// <returns>True if the inimport object was deleted successfully, else false.</returns>
        internal static bool Delete(INImport inimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimport.ConnectionName, INImportQueries.Delete(inimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inimport object from the database.
        /// </summary>
        /// <param name="inimport">The id of the inimport object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inimport history.</param>
        /// <returns>True if the inimport history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INImport inimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimport.ConnectionName, INImportQueries.DeleteHistory(inimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INImport history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inimport object from the database.
        /// </summary>
        /// <param name="inimport">The id of the inimport object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inimport object.</param>
        /// <returns>True if the inimport object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INImport inimport)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inimport.ConnectionName, INImportQueries.UnDelete(inimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INImport object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inimport object from the data reader.
        /// </summary>
        /// <param name="inimport">The inimport object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INImport inimport, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inimport.IsLoaded = true;
                    inimport.FKUserID = reader["FKUserID"] != DBNull.Value ? (long)reader["FKUserID"] : (long?)null;
                    inimport.FKINCampaignID = reader["FKINCampaignID"] != DBNull.Value ? (long)reader["FKINCampaignID"] : (long?)null;
                    inimport.FKINBatchID = reader["FKINBatchID"] != DBNull.Value ? (long)reader["FKINBatchID"] : (long?)null;
                    inimport.FKINLeadStatusID = reader["FKINLeadStatusID"] != DBNull.Value ? (long)reader["FKINLeadStatusID"] : (long?)null;
                    inimport.FKINDeclineReasonID = reader["FKINDeclineReasonID"] != DBNull.Value ? (long)reader["FKINDeclineReasonID"] : (long?)null;
                    inimport.FKINPolicyID = reader["FKINPolicyID"] != DBNull.Value ? (long)reader["FKINPolicyID"] : (long?)null;
                    inimport.FKINLeadID = reader["FKINLeadID"] != DBNull.Value ? (long)reader["FKINLeadID"] : (long?)null;
                    inimport.RefNo = reader["RefNo"] != DBNull.Value ? (string)reader["RefNo"] : (string)null;
                    inimport.ReferrorPolicyID = reader["ReferrorPolicyID"] != DBNull.Value ? (string)reader["ReferrorPolicyID"] : (string)null;
                    inimport.FKINReferrorTitleID = reader["FKINReferrorTitleID"] != DBNull.Value ? (long)reader["FKINReferrorTitleID"] : (long?)null;
                    inimport.Referror = reader["Referror"] != DBNull.Value ? (string)reader["Referror"] : (string)null;
                    inimport.FKINReferrorRelationshipID = reader["FKINReferrorRelationshipID"] != DBNull.Value ? (long)reader["FKINReferrorRelationshipID"] : (long?)null;
                    inimport.ReferrorContact = reader["ReferrorContact"] != DBNull.Value ? (string)reader["ReferrorContact"] : (string)null;
                    inimport.Gift = reader["Gift"] != DBNull.Value ? (string)reader["Gift"] : (string)null;
                    inimport.PlatinumContactDate = reader["PlatinumContactDate"] != DBNull.Value ? (DateTime)reader["PlatinumContactDate"] : (DateTime?)null;
                    inimport.PlatinumContactTime = reader["PlatinumContactTime"] != DBNull.Value ? (TimeSpan)reader["PlatinumContactTime"] : (TimeSpan?)null;
                    inimport.CancerOption = reader["CancerOption"] != DBNull.Value ? (string)reader["CancerOption"] : (string)null;
                    inimport.PlatinumAge = reader["PlatinumAge"] != DBNull.Value ? (short)reader["PlatinumAge"] : (short?)null;
                    inimport.AllocationDate = reader["AllocationDate"] != DBNull.Value ? (DateTime)reader["AllocationDate"] : (DateTime?)null;
                    inimport.IsPrinted = reader["IsPrinted"] != DBNull.Value ? (Byte)reader["IsPrinted"] : (Byte?)null;
                    inimport.DateOfSale = reader["DateOfSale"] != DBNull.Value ? (DateTime)reader["DateOfSale"] : (DateTime?)null;
                    inimport.BankCallRef = reader["BankCallRef"] != DBNull.Value ? (string)reader["BankCallRef"] : (string)null;
                    inimport.FKBankCallRefUserID = reader["FKBankCallRefUserID"] != DBNull.Value ? (long)reader["FKBankCallRefUserID"] : (long?)null;
                    inimport.BankStationNo = reader["BankStationNo"] != DBNull.Value ? (string)reader["BankStationNo"] : (string)null;
                    inimport.BankDate = reader["BankDate"] != DBNull.Value ? (DateTime)reader["BankDate"] : (DateTime?)null;
                    inimport.BankTime = reader["BankTime"] != DBNull.Value ? (TimeSpan)reader["BankTime"] : (TimeSpan?)null;
                    inimport.FKBankTelNumberTypeID = reader["FKBankTelNumberTypeID"] != DBNull.Value ? (long)reader["FKBankTelNumberTypeID"] : (long?)null;
                    inimport.SaleCallRef = reader["SaleCallRef"] != DBNull.Value ? (string)reader["SaleCallRef"] : (string)null;
                    inimport.FKSaleCallRefUserID = reader["FKSaleCallRefUserID"] != DBNull.Value ? (long)reader["FKSaleCallRefUserID"] : (long?)null;
                    inimport.SaleStationNo = reader["SaleStationNo"] != DBNull.Value ? (string)reader["SaleStationNo"] : (string)null;
                    inimport.SaleDate = reader["SaleDate"] != DBNull.Value ? (DateTime)reader["SaleDate"] : (DateTime?)null;
                    inimport.SaleTime = reader["SaleTime"] != DBNull.Value ? (TimeSpan)reader["SaleTime"] : (TimeSpan?)null;
                    inimport.FKSaleTelNumberTypeID = reader["FKSaleTelNumberTypeID"] != DBNull.Value ? (long)reader["FKSaleTelNumberTypeID"] : (long?)null;
                    inimport.ConfCallRef = reader["ConfCallRef"] != DBNull.Value ? (string)reader["ConfCallRef"] : (string)null;
                    inimport.FKConfCallRefUserID = reader["FKConfCallRefUserID"] != DBNull.Value ? (long)reader["FKConfCallRefUserID"] : (long?)null;
                    inimport.ConfStationNo = reader["ConfStationNo"] != DBNull.Value ? (string)reader["ConfStationNo"] : (string)null;
                    inimport.ConfDate = reader["ConfDate"] != DBNull.Value ? (DateTime)reader["ConfDate"] : (DateTime?)null;
                    inimport.ConfTime = reader["ConfTime"] != DBNull.Value ? (TimeSpan)reader["ConfTime"] : (TimeSpan?)null;
                    inimport.FKConfTelNumberTypeID = reader["FKConfTelNumberTypeID"] != DBNull.Value ? (long)reader["FKConfTelNumberTypeID"] : (long?)null;
                    inimport.IsConfirmed = reader["IsConfirmed"] != DBNull.Value ? (bool)reader["IsConfirmed"] : (bool?)null;
                    inimport.Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : (string)null;
                    inimport.ImportDate = reader["ImportDate"] != DBNull.Value ? (DateTime)reader["ImportDate"] : (DateTime?)null;
                    inimport.FKClosureID = reader["FKClosureID"] != DBNull.Value ? (long)reader["FKClosureID"] : (long?)null;
                    inimport.Feedback = reader["Feedback"] != DBNull.Value ? (string)reader["Feedback"] : (string)null;
                    inimport.FeedbackDate = reader["FeedbackDate"] != DBNull.Value ? (DateTime)reader["FeedbackDate"] : (DateTime?)null;
                    inimport.FutureContactDate = reader["FutureContactDate"] != DBNull.Value ? (DateTime)reader["FutureContactDate"] : (DateTime?)null;
                    inimport.FKINImportedPolicyDataID = reader["FKINImportedPolicyDataID"] != DBNull.Value ? (long)reader["FKINImportedPolicyDataID"] : (long?)null;
                    inimport.Testing1 = reader["Testing1"] != DBNull.Value ? (string)reader["Testing1"] : (string)null;
                    inimport.FKINLeadFeedbackID = reader["FKINLeadFeedbackID"] != DBNull.Value ? (long)reader["FKINLeadFeedbackID"] : (long?)null;
                    inimport.FKINCancellationReasonID = reader["FKINCancellationReasonID"] != DBNull.Value ? (long)reader["FKINCancellationReasonID"] : (long?)null;
                    inimport.IsCopied = reader["IsCopied"] != DBNull.Value ? (bool)reader["IsCopied"] : (bool?)null;
                    inimport.FKINConfirmationFeedbackID = reader["FKINConfirmationFeedbackID"] != DBNull.Value ? (long)reader["FKINConfirmationFeedbackID"] : (long?)null;
                    inimport.FKINParentBatchID = reader["FKINParentBatchID"] != DBNull.Value ? (long)reader["FKINParentBatchID"] : (long?)null;
                    inimport.BonusLead = reader["BonusLead"] != DBNull.Value ? (bool)reader["BonusLead"] : (bool?)null;
                    inimport.FKBatchCallRefUserID = reader["FKBatchCallRefUserID"] != DBNull.Value ? (long)reader["FKBatchCallRefUserID"] : (long?)null;
                    inimport.IsMining = reader["IsMining"] != DBNull.Value ? (bool)reader["IsMining"] : (bool?)null;
                    inimport.ConfWorkDate = reader["ConfWorkDate"] != DBNull.Value ? (DateTime)reader["ConfWorkDate"] : (DateTime?)null;
                    inimport.IsChecked = reader["IsChecked"] != DBNull.Value ? (bool)reader["IsChecked"] : (bool?)null;
                    inimport.CheckedDate = reader["CheckedDate"] != DBNull.Value ? (DateTime)reader["CheckedDate"] : (DateTime?)null;
                    inimport.DateBatched = reader["DateBatched"] != DBNull.Value ? (DateTime)reader["DateBatched"] : (DateTime?)null;
                    inimport.IsCopyDuplicate = reader["IsCopyDuplicate"] != DBNull.Value ? (bool)reader["IsCopyDuplicate"] : (bool?)null;
                    inimport.FKINDiaryReasonID = reader["FKINDiaryReasonID"] != DBNull.Value ? (long)reader["FKINDiaryReasonID"] : (long?)null;
                    inimport.FKINCarriedForwardReasonID = reader["FKINCarriedForwardReasonID"] != DBNull.Value ? (long)reader["FKINCarriedForwardReasonID"] : (long?)null;
                    inimport.FKINCallMonitoringCarriedForwardReasonID = reader["FKINCallMonitoringCarriedForwardReasonID"] != DBNull.Value ? (long)reader["FKINCallMonitoringCarriedForwardReasonID"] : (long?)null;
                    inimport.PermissionQuestionAsked = reader["PermissionQuestionAsked"] != DBNull.Value ? (bool)reader["PermissionQuestionAsked"] : (bool?)null;
                    inimport.FKINCallMonitoringCancellationReasonID = reader["FKINCallMonitoringCancellationReasonID"] != DBNull.Value ? (long)reader["FKINCallMonitoringCancellationReasonID"] : (long?)null;
                    inimport.IsFutureAllocation = reader["IsFutureAllocation"] != DBNull.Value ? (bool)reader["IsFutureAllocation"] : (bool?)null;
                    inimport.MoneyBackDate = reader["MoneyBackDate"] != DBNull.Value ? (DateTime)reader["MoneyBackDate"] : (DateTime?)null;
                    inimport.ConversionMBDate = reader["ConversionMBDate"] != DBNull.Value ? (DateTime)reader["ConversionMBDate"] : (DateTime?)null;
                    inimport.ObtainedReferrals = reader["ObtainedReferrals"] != DBNull.Value ? (bool)reader["ObtainedReferrals"] : (bool?)null;
                    inimport.StampDate = (DateTime)reader["StampDate"];
                    inimport.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INImport does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimport object from the database.
        /// </summary>
        /// <param name="inimport">The inimport to fill.</param>
        internal static void Fill(INImport inimport)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimport.ConnectionName, INImportQueries.Fill(inimport, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImport object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inimport object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INImport inimport)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimport.ConnectionName, INImportQueries.FillData(inimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INImport object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inimport object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inimport">The inimport to fill from history.</param>
        internal static void FillHistory(INImport inimport, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inimport.ConnectionName, INImportQueries.FillHistory(inimport, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inimport, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INImport object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INImportCollection List(bool showDeleted, string connectionName)
        {
            INImportCollection collection = new INImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INImportQueries.ListDeleted() : INImportQueries.List(), null);
                while (reader.Read())
                {
                    INImport inimport = new INImport((long)reader["ID"]);
                    inimport.ConnectionName = connectionName;
                    collection.Add(inimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inimport objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INImportQueries.ListDeleted() : INImportQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inimport object from the database.
        /// </summary>
        /// <param name="inimport">The inimport to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INImport inimport)
        {
            INImportCollection collection = new INImportCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inimport.ConnectionName, INImportQueries.ListHistory(inimport, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INImport in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inimport object to the database.
        /// </summary>
        /// <param name="inimport">The INImport object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inimport was saved successfull, otherwise, false.</returns>
        internal static bool Save(INImport inimport)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inimport.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inimport.ConnectionName, INImportQueries.Save(inimport, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inimport.ConnectionName, INImportQueries.Save(inimport, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inimport.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inimport.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INImport object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimport objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="fkinleadstatusid">The fkinleadstatusid search criteria.</param>
        /// <param name="fkindeclinereasonid">The fkindeclinereasonid search criteria.</param>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinleadid">The fkinleadid search criteria.</param>
        /// <param name="refno">The refno search criteria.</param>
        /// <param name="referrorpolicyid">The referrorpolicyid search criteria.</param>
        /// <param name="fkinreferrortitleid">The fkinreferrortitleid search criteria.</param>
        /// <param name="referror">The referror search criteria.</param>
        /// <param name="fkinreferrorrelationshipid">The fkinreferrorrelationshipid search criteria.</param>
        /// <param name="referrorcontact">The referrorcontact search criteria.</param>
        /// <param name="gift">The gift search criteria.</param>
        /// <param name="platinumcontactdate">The platinumcontactdate search criteria.</param>
        /// <param name="platinumcontacttime">The platinumcontacttime search criteria.</param>
        /// <param name="canceroption">The canceroption search criteria.</param>
        /// <param name="platinumage">The platinumage search criteria.</param>
        /// <param name="allocationdate">The allocationdate search criteria.</param>
        /// <param name="isprinted">The isprinted search criteria.</param>
        /// <param name="dateofsale">The dateofsale search criteria.</param>
        /// <param name="bankcallref">The bankcallref search criteria.</param>
        /// <param name="fkbankcallrefuserid">The fkbankcallrefuserid search criteria.</param>
        /// <param name="bankstationno">The bankstationno search criteria.</param>
        /// <param name="bankdate">The bankdate search criteria.</param>
        /// <param name="banktime">The banktime search criteria.</param>
        /// <param name="fkbanktelnumbertypeid">The fkbanktelnumbertypeid search criteria.</param>
        /// <param name="salecallref">The salecallref search criteria.</param>
        /// <param name="fksalecallrefuserid">The fksalecallrefuserid search criteria.</param>
        /// <param name="salestationno">The salestationno search criteria.</param>
        /// <param name="saledate">The saledate search criteria.</param>
        /// <param name="saletime">The saletime search criteria.</param>
        /// <param name="fksaletelnumbertypeid">The fksaletelnumbertypeid search criteria.</param>
        /// <param name="confcallref">The confcallref search criteria.</param>
        /// <param name="fkconfcallrefuserid">The fkconfcallrefuserid search criteria.</param>
        /// <param name="confstationno">The confstationno search criteria.</param>
        /// <param name="confdate">The confdate search criteria.</param>
        /// <param name="conftime">The conftime search criteria.</param>
        /// <param name="fkconftelnumbertypeid">The fkconftelnumbertypeid search criteria.</param>
        /// <param name="isconfirmed">The isconfirmed search criteria.</param>
        /// <param name="notes">The notes search criteria.</param>
        /// <param name="importdate">The importdate search criteria.</param>
        /// <param name="fkclosureid">The fkclosureid search criteria.</param>
        /// <param name="feedback">The feedback search criteria.</param>
        /// <param name="feedbackdate">The feedbackdate search criteria.</param>
        /// <param name="futurecontactdate">The futurecontactdate search criteria.</param>
        /// <param name="fkinimportedpolicydataid">The fkinimportedpolicydataid search criteria.</param>
        /// <param name="testing1">The testing1 search criteria.</param>
        /// <param name="fkinleadfeedbackid">The fkinleadfeedbackid search criteria.</param>
        /// <param name="fkincancellationreasonid">The fkincancellationreasonid search criteria.</param>
        /// <param name="iscopied">The iscopied search criteria.</param>
        /// <param name="fkinconfirmationfeedbackid">The fkinconfirmationfeedbackid search criteria.</param>
        /// <param name="fkinparentbatchid">The fkinparentbatchid search criteria.</param>
        /// <param name="bonuslead">The bonuslead search criteria.</param>
        /// <param name="fkbatchcallrefuserid">The fkbatchcallrefuserid search criteria.</param>
        /// <param name="ismining">The ismining search criteria.</param>
        /// <param name="confworkdate">The confworkdate search criteria.</param>
        /// <param name="ischecked">The ischecked search criteria.</param>
        /// <param name="checkeddate">The checkeddate search criteria.</param>
        /// <param name="datebatched">The datebatched search criteria.</param>
        /// <param name="iscopyduplicate">The iscopyduplicate search criteria.</param>
        /// <param name="fkindiaryreasonid">The fkindiaryreasonid search criteria.</param>
        /// <param name="fkincarriedforwardreasonid">The fkincarriedforwardreasonid search criteria.</param>
        /// <param name="fkincallmonitoringcarriedforwardreasonid">The fkincallmonitoringcarriedforwardreasonid search criteria.</param>
        /// <param name="permissionquestionasked">The permissionquestionasked search criteria.</param>
        /// <param name="fkincallmonitoringcancellationreasonid">The fkincallmonitoringcancellationreasonid search criteria.</param>
        /// <param name="isfutureallocation">The isfutureallocation search criteria.</param>
        /// <param name="moneybackdate"></param>
        /// <param name="obtainedreferrals"></param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImportCollection Search(long? fkuserid, long? fkincampaignid, long? fkinbatchid, long? fkinleadstatusid, long? fkindeclinereasonid, long? fkinpolicyid, long? fkinleadid, string refno, string referrorpolicyid, long? fkinreferrortitleid, string referror, long? fkinreferrorrelationshipid, string referrorcontact, string gift, DateTime? platinumcontactdate, TimeSpan? platinumcontacttime, string canceroption, short? platinumage, DateTime? allocationdate, Byte? isprinted, DateTime? dateofsale, string bankcallref, long? fkbankcallrefuserid, string bankstationno, DateTime? bankdate, TimeSpan? banktime, long? fkbanktelnumbertypeid, string salecallref, long? fksalecallrefuserid, string salestationno, DateTime? saledate, TimeSpan? saletime, long? fksaletelnumbertypeid, string confcallref, long? fkconfcallrefuserid, string confstationno, DateTime? confdate, TimeSpan? conftime, long? fkconftelnumbertypeid, bool? isconfirmed, string notes, DateTime? importdate, long? fkclosureid, string feedback, DateTime? feedbackdate, DateTime? futurecontactdate, long? fkinimportedpolicydataid, string testing1, long? fkinleadfeedbackid, long? fkincancellationreasonid, bool? iscopied, long? fkinconfirmationfeedbackid, long? fkinparentbatchid, bool? bonuslead, long? fkbatchcallrefuserid, bool? ismining, DateTime? confworkdate, bool? ischecked, DateTime? checkeddate, DateTime? datebatched, bool? iscopyduplicate, long? fkindiaryreasonid, long? fkincarriedforwardreasonid, long? fkincallmonitoringcarriedforwardreasonid, bool? permissionquestionasked, long? fkincallmonitoringcancellationreasonid, bool? isfutureallocation, DateTime? moneybackdate, DateTime? conversionmbdate, bool? obtainedreferrals, string connectionName)
        {
            INImportCollection collection = new INImportCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportQueries.Search(fkuserid, fkincampaignid, fkinbatchid, fkinleadstatusid, fkindeclinereasonid, fkinpolicyid, fkinleadid, refno, referrorpolicyid, fkinreferrortitleid, referror, fkinreferrorrelationshipid, referrorcontact, gift, platinumcontactdate, platinumcontacttime, canceroption, platinumage, allocationdate, isprinted, dateofsale, bankcallref, fkbankcallrefuserid, bankstationno, bankdate, banktime, fkbanktelnumbertypeid, salecallref, fksalecallrefuserid, salestationno, saledate, saletime, fksaletelnumbertypeid, confcallref, fkconfcallrefuserid, confstationno, confdate, conftime, fkconftelnumbertypeid, isconfirmed, notes, importdate, fkclosureid, feedback, feedbackdate, futurecontactdate, fkinimportedpolicydataid, testing1, fkinleadfeedbackid, fkincancellationreasonid, iscopied, fkinconfirmationfeedbackid, fkinparentbatchid, bonuslead, fkbatchcallrefuserid, ismining, confworkdate, ischecked, checkeddate, datebatched, iscopyduplicate, fkindiaryreasonid, fkincarriedforwardreasonid, fkincallmonitoringcarriedforwardreasonid, permissionquestionasked, fkincallmonitoringcancellationreasonid, isfutureallocation, moneybackdate, conversionmbdate, obtainedreferrals), null);
                while (reader.Read())
                {
                    INImport inimport = new INImport((long)reader["ID"]);
                    inimport.ConnectionName = connectionName;
                    collection.Add(inimport);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImport objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inimport objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="fkinleadstatusid">The fkinleadstatusid search criteria.</param>
        /// <param name="fkindeclinereasonid">The fkindeclinereasonid search criteria.</param>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinleadid">The fkinleadid search criteria.</param>
        /// <param name="refno">The refno search criteria.</param>
        /// <param name="referrorpolicyid">The referrorpolicyid search criteria.</param>
        /// <param name="fkinreferrortitleid">The fkinreferrortitleid search criteria.</param>
        /// <param name="referror">The referror search criteria.</param>
        /// <param name="fkinreferrorrelationshipid">The fkinreferrorrelationshipid search criteria.</param>
        /// <param name="referrorcontact">The referrorcontact search criteria.</param>
        /// <param name="gift">The gift search criteria.</param>
        /// <param name="platinumcontactdate">The platinumcontactdate search criteria.</param>
        /// <param name="platinumcontacttime">The platinumcontacttime search criteria.</param>
        /// <param name="canceroption">The canceroption search criteria.</param>
        /// <param name="platinumage">The platinumage search criteria.</param>
        /// <param name="allocationdate">The allocationdate search criteria.</param>
        /// <param name="isprinted">The isprinted search criteria.</param>
        /// <param name="dateofsale">The dateofsale search criteria.</param>
        /// <param name="bankcallref">The bankcallref search criteria.</param>
        /// <param name="fkbankcallrefuserid">The fkbankcallrefuserid search criteria.</param>
        /// <param name="bankstationno">The bankstationno search criteria.</param>
        /// <param name="bankdate">The bankdate search criteria.</param>
        /// <param name="banktime">The banktime search criteria.</param>
        /// <param name="fkbanktelnumbertypeid">The fkbanktelnumbertypeid search criteria.</param>
        /// <param name="salecallref">The salecallref search criteria.</param>
        /// <param name="fksalecallrefuserid">The fksalecallrefuserid search criteria.</param>
        /// <param name="salestationno">The salestationno search criteria.</param>
        /// <param name="saledate">The saledate search criteria.</param>
        /// <param name="saletime">The saletime search criteria.</param>
        /// <param name="fksaletelnumbertypeid">The fksaletelnumbertypeid search criteria.</param>
        /// <param name="confcallref">The confcallref search criteria.</param>
        /// <param name="fkconfcallrefuserid">The fkconfcallrefuserid search criteria.</param>
        /// <param name="confstationno">The confstationno search criteria.</param>
        /// <param name="confdate">The confdate search criteria.</param>
        /// <param name="conftime">The conftime search criteria.</param>
        /// <param name="fkconftelnumbertypeid">The fkconftelnumbertypeid search criteria.</param>
        /// <param name="isconfirmed">The isconfirmed search criteria.</param>
        /// <param name="notes">The notes search criteria.</param>
        /// <param name="importdate">The importdate search criteria.</param>
        /// <param name="fkclosureid">The fkclosureid search criteria.</param>
        /// <param name="feedback">The feedback search criteria.</param>
        /// <param name="feedbackdate">The feedbackdate search criteria.</param>
        /// <param name="futurecontactdate">The futurecontactdate search criteria.</param>
        /// <param name="fkinimportedpolicydataid">The fkinimportedpolicydataid search criteria.</param>
        /// <param name="testing1">The testing1 search criteria.</param>
        /// <param name="fkinleadfeedbackid">The fkinleadfeedbackid search criteria.</param>
        /// <param name="fkincancellationreasonid">The fkincancellationreasonid search criteria.</param>
        /// <param name="iscopied">The iscopied search criteria.</param>
        /// <param name="fkinconfirmationfeedbackid">The fkinconfirmationfeedbackid search criteria.</param>
        /// <param name="fkinparentbatchid">The fkinparentbatchid search criteria.</param>
        /// <param name="bonuslead">The bonuslead search criteria.</param>
        /// <param name="fkbatchcallrefuserid">The fkbatchcallrefuserid search criteria.</param>
        /// <param name="ismining">The ismining search criteria.</param>
        /// <param name="confworkdate">The confworkdate search criteria.</param>
        /// <param name="ischecked">The ischecked search criteria.</param>
        /// <param name="checkeddate">The checkeddate search criteria.</param>
        /// <param name="datebatched">The datebatched search criteria.</param>
        /// <param name="iscopyduplicate">The iscopyduplicate search criteria.</param>
        /// <param name="fkindiaryreasonid">The fkindiaryreasonid search criteria.</param>
        /// <param name="fkincarriedforwardreasonid">The fkincarriedforwardreasonid search criteria.</param>
        /// <param name="fkincallmonitoringcarriedforwardreasonid">The fkincallmonitoringcarriedforwardreasonid search criteria.</param>
        /// <param name="permissionquestionasked">The permissionquestionasked search criteria.</param>
        /// <param name="fkincallmonitoringcancellationreasonid">The fkincallmonitoringcancellationreasonid search criteria.</param>
        /// <param name="isfutureallocation">The isfutureallocation search criteria.</param>
        /// <param name="moneybackdate"></param>
        /// <param name="obtainedreferrals"></param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkuserid, long? fkincampaignid, long? fkinbatchid, long? fkinleadstatusid, long? fkindeclinereasonid, long? fkinpolicyid, long? fkinleadid, string refno, string referrorpolicyid, long? fkinreferrortitleid, string referror, long? fkinreferrorrelationshipid, string referrorcontact, string gift, DateTime? platinumcontactdate, TimeSpan? platinumcontacttime, string canceroption, short? platinumage, DateTime? allocationdate, Byte? isprinted, DateTime? dateofsale, string bankcallref, long? fkbankcallrefuserid, string bankstationno, DateTime? bankdate, TimeSpan? banktime, long? fkbanktelnumbertypeid, string salecallref, long? fksalecallrefuserid, string salestationno, DateTime? saledate, TimeSpan? saletime, long? fksaletelnumbertypeid, string confcallref, long? fkconfcallrefuserid, string confstationno, DateTime? confdate, TimeSpan? conftime, long? fkconftelnumbertypeid, bool? isconfirmed, string notes, DateTime? importdate, long? fkclosureid, string feedback, DateTime? feedbackdate, DateTime? futurecontactdate, long? fkinimportedpolicydataid, string testing1, long? fkinleadfeedbackid, long? fkincancellationreasonid, bool? iscopied, long? fkinconfirmationfeedbackid, long? fkinparentbatchid, bool? bonuslead, long? fkbatchcallrefuserid, bool? ismining, DateTime? confworkdate, bool? ischecked, DateTime? checkeddate, DateTime? datebatched, bool? iscopyduplicate, long? fkindiaryreasonid, long? fkincarriedforwardreasonid, long? fkincallmonitoringcarriedforwardreasonid, bool? permissionquestionasked, long? fkincallmonitoringcancellationreasonid, bool? isfutureallocation, DateTime? moneybackdate, DateTime? conversionmbdate, bool? obtainedrefferals, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INImportQueries.Search(fkuserid, fkincampaignid, fkinbatchid, fkinleadstatusid, fkindeclinereasonid, fkinpolicyid, fkinleadid, refno, referrorpolicyid, fkinreferrortitleid, referror, fkinreferrorrelationshipid, referrorcontact, gift, platinumcontactdate, platinumcontacttime, canceroption, platinumage, allocationdate, isprinted, dateofsale, bankcallref, fkbankcallrefuserid, bankstationno, bankdate, banktime, fkbanktelnumbertypeid, salecallref, fksalecallrefuserid, salestationno, saledate, saletime, fksaletelnumbertypeid, confcallref, fkconfcallrefuserid, confstationno, confdate, conftime, fkconftelnumbertypeid, isconfirmed, notes, importdate, fkclosureid, feedback, feedbackdate, futurecontactdate, fkinimportedpolicydataid, testing1, fkinleadfeedbackid, fkincancellationreasonid, iscopied, fkinconfirmationfeedbackid, fkinparentbatchid, bonuslead, fkbatchcallrefuserid, ismining, confworkdate, ischecked, checkeddate, datebatched, iscopyduplicate, fkindiaryreasonid, fkincarriedforwardreasonid, fkincallmonitoringcarriedforwardreasonid, permissionquestionasked, fkincallmonitoringcancellationreasonid, isfutureallocation, moneybackdate, conversionmbdate, obtainedrefferals), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImport objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inimport objects in the database.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="fkinleadstatusid">The fkinleadstatusid search criteria.</param>
        /// <param name="fkindeclinereasonid">The fkindeclinereasonid search criteria.</param>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinleadid">The fkinleadid search criteria.</param>
        /// <param name="refno">The refno search criteria.</param>
        /// <param name="referrorpolicyid">The referrorpolicyid search criteria.</param>
        /// <param name="fkinreferrortitleid">The fkinreferrortitleid search criteria.</param>
        /// <param name="referror">The referror search criteria.</param>
        /// <param name="fkinreferrorrelationshipid">The fkinreferrorrelationshipid search criteria.</param>
        /// <param name="referrorcontact">The referrorcontact search criteria.</param>
        /// <param name="gift">The gift search criteria.</param>
        /// <param name="platinumcontactdate">The platinumcontactdate search criteria.</param>
        /// <param name="platinumcontacttime">The platinumcontacttime search criteria.</param>
        /// <param name="canceroption">The canceroption search criteria.</param>
        /// <param name="platinumage">The platinumage search criteria.</param>
        /// <param name="allocationdate">The allocationdate search criteria.</param>
        /// <param name="isprinted">The isprinted search criteria.</param>
        /// <param name="dateofsale">The dateofsale search criteria.</param>
        /// <param name="bankcallref">The bankcallref search criteria.</param>
        /// <param name="fkbankcallrefuserid">The fkbankcallrefuserid search criteria.</param>
        /// <param name="bankstationno">The bankstationno search criteria.</param>
        /// <param name="bankdate">The bankdate search criteria.</param>
        /// <param name="banktime">The banktime search criteria.</param>
        /// <param name="fkbanktelnumbertypeid">The fkbanktelnumbertypeid search criteria.</param>
        /// <param name="salecallref">The salecallref search criteria.</param>
        /// <param name="fksalecallrefuserid">The fksalecallrefuserid search criteria.</param>
        /// <param name="salestationno">The salestationno search criteria.</param>
        /// <param name="saledate">The saledate search criteria.</param>
        /// <param name="saletime">The saletime search criteria.</param>
        /// <param name="fksaletelnumbertypeid">The fksaletelnumbertypeid search criteria.</param>
        /// <param name="confcallref">The confcallref search criteria.</param>
        /// <param name="fkconfcallrefuserid">The fkconfcallrefuserid search criteria.</param>
        /// <param name="confstationno">The confstationno search criteria.</param>
        /// <param name="confdate">The confdate search criteria.</param>
        /// <param name="conftime">The conftime search criteria.</param>
        /// <param name="fkconftelnumbertypeid">The fkconftelnumbertypeid search criteria.</param>
        /// <param name="isconfirmed">The isconfirmed search criteria.</param>
        /// <param name="notes">The notes search criteria.</param>
        /// <param name="importdate">The importdate search criteria.</param>
        /// <param name="fkclosureid">The fkclosureid search criteria.</param>
        /// <param name="feedback">The feedback search criteria.</param>
        /// <param name="feedbackdate">The feedbackdate search criteria.</param>
        /// <param name="futurecontactdate">The futurecontactdate search criteria.</param>
        /// <param name="fkinimportedpolicydataid">The fkinimportedpolicydataid search criteria.</param>
        /// <param name="testing1">The testing1 search criteria.</param>
        /// <param name="fkinleadfeedbackid">The fkinleadfeedbackid search criteria.</param>
        /// <param name="fkincancellationreasonid">The fkincancellationreasonid search criteria.</param>
        /// <param name="iscopied">The iscopied search criteria.</param>
        /// <param name="fkinconfirmationfeedbackid">The fkinconfirmationfeedbackid search criteria.</param>
        /// <param name="fkinparentbatchid">The fkinparentbatchid search criteria.</param>
        /// <param name="bonuslead">The bonuslead search criteria.</param>
        /// <param name="fkbatchcallrefuserid">The fkbatchcallrefuserid search criteria.</param>
        /// <param name="ismining">The ismining search criteria.</param>
        /// <param name="confworkdate">The confworkdate search criteria.</param>
        /// <param name="ischecked">The ischecked search criteria.</param>
        /// <param name="checkeddate">The checkeddate search criteria.</param>
        /// <param name="datebatched">The datebatched search criteria.</param>
        /// <param name="iscopyduplicate">The iscopyduplicate search criteria.</param>
        /// <param name="fkindiaryreasonid">The fkindiaryreasonid search criteria.</param>
        /// <param name="fkincarriedforwardreasonid">The fkincarriedforwardreasonid search criteria.</param>
        /// <param name="fkincallmonitoringcarriedforwardreasonid">The fkincallmonitoringcarriedforwardreasonid search criteria.</param>
        /// <param name="permissionquestionasked">The permissionquestionasked search criteria.</param>
        /// <param name="fkincallmonitoringcancellationreasonid">The fkincallmonitoringcancellationreasonid search criteria.</param>
        /// <param name="isfutureallocation">The isfutureallocation search criteria.</param>
        /// <param name="moneybackdate"></param>
        /// <param name="obtainedreferrals"></param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INImport SearchOne(long? fkuserid, long? fkincampaignid, long? fkinbatchid, long? fkinleadstatusid, long? fkindeclinereasonid, long? fkinpolicyid, long? fkinleadid, string refno, string referrorpolicyid, long? fkinreferrortitleid, string referror, long? fkinreferrorrelationshipid, string referrorcontact, string gift, DateTime? platinumcontactdate, TimeSpan? platinumcontacttime, string canceroption, short? platinumage, DateTime? allocationdate, Byte? isprinted, DateTime? dateofsale, string bankcallref, long? fkbankcallrefuserid, string bankstationno, DateTime? bankdate, TimeSpan? banktime, long? fkbanktelnumbertypeid, string salecallref, long? fksalecallrefuserid, string salestationno, DateTime? saledate, TimeSpan? saletime, long? fksaletelnumbertypeid, string confcallref, long? fkconfcallrefuserid, string confstationno, DateTime? confdate, TimeSpan? conftime, long? fkconftelnumbertypeid, bool? isconfirmed, string notes, DateTime? importdate, long? fkclosureid, string feedback, DateTime? feedbackdate, DateTime? futurecontactdate, long? fkinimportedpolicydataid, string testing1, long? fkinleadfeedbackid, long? fkincancellationreasonid, bool? iscopied, long? fkinconfirmationfeedbackid, long? fkinparentbatchid, bool? bonuslead, long? fkbatchcallrefuserid, bool? ismining, DateTime? confworkdate, bool? ischecked, DateTime? checkeddate, DateTime? datebatched, bool? iscopyduplicate, long? fkindiaryreasonid, long? fkincarriedforwardreasonid, long? fkincallmonitoringcarriedforwardreasonid, bool? permissionquestionasked, long? fkincallmonitoringcancellationreasonid, bool? isfutureallocation, DateTime? moneybackdate, DateTime? conversionmbdate, bool? obtainedreferrals, string connectionName)
        {
            INImport inimport = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INImportQueries.Search(fkuserid, fkincampaignid, fkinbatchid, fkinleadstatusid, fkindeclinereasonid, fkinpolicyid, fkinleadid, refno, referrorpolicyid, fkinreferrortitleid, referror, fkinreferrorrelationshipid, referrorcontact, gift, platinumcontactdate, platinumcontacttime, canceroption, platinumage, allocationdate, isprinted, dateofsale, bankcallref, fkbankcallrefuserid, bankstationno, bankdate, banktime, fkbanktelnumbertypeid, salecallref, fksalecallrefuserid, salestationno, saledate, saletime, fksaletelnumbertypeid, confcallref, fkconfcallrefuserid, confstationno, confdate, conftime, fkconftelnumbertypeid, isconfirmed, notes, importdate, fkclosureid, feedback, feedbackdate, futurecontactdate, fkinimportedpolicydataid, testing1, fkinleadfeedbackid, fkincancellationreasonid, iscopied, fkinconfirmationfeedbackid, fkinparentbatchid, bonuslead, fkbatchcallrefuserid, ismining, confworkdate, ischecked, checkeddate, datebatched, iscopyduplicate, fkindiaryreasonid, fkincarriedforwardreasonid, fkincallmonitoringcarriedforwardreasonid, permissionquestionasked, fkincallmonitoringcancellationreasonid, isfutureallocation, moneybackdate, conversionmbdate, obtainedreferrals), null);
                if (reader.Read())
                {
                    inimport = new INImport((long)reader["ID"]);
                    inimport.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INImport objects in the database", ex);
            }
            return inimport;
        }
        #endregion
    }
}
