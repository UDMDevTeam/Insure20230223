using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimport objects.
    /// </summary>
    internal abstract partial class INImportQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimport from the database.
        /// </summary>
        /// <param name="inimport">The inimport object to delete.</param>
        /// <returns>A query that can be used to delete the inimport from the database.</returns>
        internal static string Delete(INImport inimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimport != null)
            {
                query = "INSERT INTO [zHstINImport] ([ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID] FROM [INImport] WHERE [INImport].[ID] = @ID; ";
                query += "DELETE FROM [INImport] WHERE [INImport].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimport from the database.
        /// </summary>
        /// <param name="inimport">The inimport object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimport from the database.</returns>
        internal static string DeleteHistory(INImport inimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimport != null)
            {
                query = "DELETE FROM [zHstINImport] WHERE [zHstINImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimport from the database.
        /// </summary>
        /// <param name="inimport">The inimport object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimport from the database.</returns>
        internal static string UnDelete(INImport inimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimport != null)
            {
                query = "INSERT INTO [INImport] ([ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID] FROM [zHstINImport] WHERE [zHstINImport].[ID] = @ID AND [zHstINImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImport] WHERE [zHstINImport].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImport] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImport] WHERE [zHstINImport].[ID] = @ID AND [zHstINImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImport] WHERE [zHstINImport].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImport] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimport.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimport object.
        /// </summary>
        /// <param name="inimport">The inimport object to fill.</param>
        /// <returns>A query that can be used to fill the inimport object.</returns>
        internal static string Fill(INImport inimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimport != null)
            {
                query = "SELECT [ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID] FROM [INImport] WHERE [INImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimport data.
        /// </summary>
        /// <param name="inimport">The inimport to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimport data.</returns>
        internal static string FillData(INImport inimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimport != null)
            {
            query.Append("SELECT [INImport].[ID], [INImport].[FKUserID], [INImport].[FKINCampaignID], [INImport].[FKINBatchID], [INImport].[FKINLeadStatusID], [INImport].[FKINDeclineReasonID], [INImport].[FKINPolicyID], [INImport].[FKINLeadID], [INImport].[RefNo], [INImport].[ReferrorPolicyID], [INImport].[FKINReferrorTitleID], [INImport].[Referror], [INImport].[FKINReferrorRelationshipID], [INImport].[ReferrorContact], [INImport].[Gift], [INImport].[PlatinumContactDate], [INImport].[PlatinumContactTime], [INImport].[CancerOption], [INImport].[PlatinumAge], [INImport].[AllocationDate], [INImport].[IsPrinted], [INImport].[DateOfSale], [INImport].[BankCallRef], [INImport].[FKBankCallRefUserID], [INImport].[BankStationNo], [INImport].[BankDate], [INImport].[BankTime], [INImport].[FKBankTelNumberTypeID], [INImport].[SaleCallRef], [INImport].[FKSaleCallRefUserID], [INImport].[SaleStationNo], [INImport].[SaleDate], [INImport].[SaleTime], [INImport].[FKSaleTelNumberTypeID], [INImport].[ConfCallRef], [INImport].[FKConfCallRefUserID], [INImport].[ConfStationNo], [INImport].[ConfDate], [INImport].[ConfTime], [INImport].[FKConfTelNumberTypeID], [INImport].[IsConfirmed], [INImport].[Notes], [INImport].[ImportDate], [INImport].[FKClosureID], [INImport].[Feedback], [INImport].[FeedbackDate], [INImport].[FutureContactDate], [INImport].[FKINImportedPolicyDataID], [INImport].[Testing1], [INImport].[FKINLeadFeedbackID], [INImport].[FKINCancellationReasonID], [INImport].[IsCopied], [INImport].[FKINConfirmationFeedbackID], [INImport].[FKINParentBatchID], [INImport].[BonusLead], [INImport].[FKBatchCallRefUserID], [INImport].[IsMining], [INImport].[ConfWorkDate], [INImport].[IsChecked], [INImport].[CheckedDate], [INImport].[DateBatched], [INImport].[IsCopyDuplicate], [INImport].[FKINDiaryReasonID], [INImport].[FKINCarriedForwardReasonID], [INImport].[FKINCallMonitoringCarriedForwardReasonID], [INImport].[PermissionQuestionAsked], [INImport].[FKINCallMonitoringCancellationReasonID], [INImport].[IsFutureAllocation], [INImport].[MoneyBackDate], [INImport].[ConversionMBDate], [INImport].[ObtainedReferrals],  [INImport].[StampDate], [INImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImport] ");
                query.Append(" WHERE [INImport].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimport.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimport object from history.
        /// </summary>
        /// <param name="inimport">The inimport object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimport object from history.</returns>
        internal static string FillHistory(INImport inimport, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimport != null)
            {
                query = "SELECT [ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID] FROM [zHstINImport] WHERE [zHstINImport].[ID] = @ID AND [zHstINImport].[StampUserID] = @StampUserID AND [zHstINImport].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimport.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimports in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImport].[ID], [INImport].[FKUserID], [INImport].[FKINCampaignID], [INImport].[FKINBatchID], [INImport].[FKINLeadStatusID], [INImport].[FKINDeclineReasonID], [INImport].[FKINPolicyID], [INImport].[FKINLeadID], [INImport].[RefNo], [INImport].[ReferrorPolicyID], [INImport].[FKINReferrorTitleID], [INImport].[Referror], [INImport].[FKINReferrorRelationshipID], [INImport].[ReferrorContact], [INImport].[Gift], [INImport].[PlatinumContactDate], [INImport].[PlatinumContactTime], [INImport].[CancerOption], [INImport].[PlatinumAge], [INImport].[AllocationDate], [INImport].[IsPrinted], [INImport].[DateOfSale], [INImport].[BankCallRef], [INImport].[FKBankCallRefUserID], [INImport].[BankStationNo], [INImport].[BankDate], [INImport].[BankTime], [INImport].[FKBankTelNumberTypeID], [INImport].[SaleCallRef], [INImport].[FKSaleCallRefUserID], [INImport].[SaleStationNo], [INImport].[SaleDate], [INImport].[SaleTime], [INImport].[FKSaleTelNumberTypeID], [INImport].[ConfCallRef], [INImport].[FKConfCallRefUserID], [INImport].[ConfStationNo], [INImport].[ConfDate], [INImport].[ConfTime], [INImport].[FKConfTelNumberTypeID], [INImport].[IsConfirmed], [INImport].[Notes], [INImport].[ImportDate], [INImport].[FKClosureID], [INImport].[Feedback], [INImport].[FeedbackDate], [INImport].[FutureContactDate], [INImport].[FKINImportedPolicyDataID], [INImport].[Testing1], [INImport].[FKINLeadFeedbackID], [INImport].[FKINCancellationReasonID], [INImport].[IsCopied], [INImport].[FKINConfirmationFeedbackID], [INImport].[FKINParentBatchID], [INImport].[BonusLead], [INImport].[FKBatchCallRefUserID], [INImport].[IsMining], [INImport].[ConfWorkDate], [INImport].[IsChecked], [INImport].[CheckedDate], [INImport].[DateBatched], [INImport].[IsCopyDuplicate], [INImport].[FKINDiaryReasonID], [INImport].[FKINCarriedForwardReasonID], [INImport].[FKINCallMonitoringCarriedForwardReasonID], [INImport].[PermissionQuestionAsked], [INImport].[FKINCallMonitoringCancellationReasonID], [INImport].[IsFutureAllocation], [INImport].[MoneyBackDate], [INImport].[ConversionMBDate], [INImport].[ObtainedReferrals], [INImport].[StampDate], [INImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImport] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimports in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImport].[ID], [zHstINImport].[FKUserID], [zHstINImport].[FKINCampaignID], [zHstINImport].[FKINBatchID], [zHstINImport].[FKINLeadStatusID], [zHstINImport].[FKINDeclineReasonID], [zHstINImport].[FKINPolicyID], [zHstINImport].[FKINLeadID], [zHstINImport].[RefNo], [zHstINImport].[ReferrorPolicyID], [zHstINImport].[FKINReferrorTitleID], [zHstINImport].[Referror], [zHstINImport].[FKINReferrorRelationshipID], [zHstINImport].[ReferrorContact], [zHstINImport].[Gift], [zHstINImport].[PlatinumContactDate], [zHstINImport].[PlatinumContactTime], [zHstINImport].[CancerOption], [zHstINImport].[PlatinumAge], [zHstINImport].[AllocationDate], [zHstINImport].[IsPrinted], [zHstINImport].[DateOfSale], [zHstINImport].[BankCallRef], [zHstINImport].[FKBankCallRefUserID], [zHstINImport].[BankStationNo], [zHstINImport].[BankDate], [zHstINImport].[BankTime], [zHstINImport].[FKBankTelNumberTypeID], [zHstINImport].[SaleCallRef], [zHstINImport].[FKSaleCallRefUserID], [zHstINImport].[SaleStationNo], [zHstINImport].[SaleDate], [zHstINImport].[SaleTime], [zHstINImport].[FKSaleTelNumberTypeID], [zHstINImport].[ConfCallRef], [zHstINImport].[FKConfCallRefUserID], [zHstINImport].[ConfStationNo], [zHstINImport].[ConfDate], [zHstINImport].[ConfTime], [zHstINImport].[FKConfTelNumberTypeID], [zHstINImport].[IsConfirmed], [zHstINImport].[Notes], [zHstINImport].[ImportDate], [zHstINImport].[FKClosureID], [zHstINImport].[Feedback], [zHstINImport].[FeedbackDate], [zHstINImport].[FutureContactDate], [zHstINImport].[FKINImportedPolicyDataID], [zHstINImport].[Testing1], [zHstINImport].[FKINLeadFeedbackID], [zHstINImport].[FKINCancellationReasonID], [zHstINImport].[IsCopied], [zHstINImport].[FKINConfirmationFeedbackID], [zHstINImport].[FKINParentBatchID], [zHstINImport].[BonusLead], [zHstINImport].[FKBatchCallRefUserID], [zHstINImport].[IsMining], [zHstINImport].[ConfWorkDate], [zHstINImport].[IsChecked], [zHstINImport].[CheckedDate], [zHstINImport].[DateBatched], [zHstINImport].[IsCopyDuplicate], [zHstINImport].[FKINDiaryReasonID], [zHstINImport].[FKINCarriedForwardReasonID], [zHstINImport].[FKINCallMonitoringCarriedForwardReasonID], [zHstINImport].[PermissionQuestionAsked], [zHstINImport].[FKINCallMonitoringCancellationReasonID], [zHstINImport].[IsFutureAllocation], [zHstINImport].[MoneyBackDate], [zHstINImport].[ConversionMBDate], [zHstINImport].[ObtainedReferrals], [zHstINImport].[StampDate], [zHstINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImport] ");
            query.Append("INNER JOIN (SELECT [zHstINImport].[ID], MAX([zHstINImport].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImport] ");
            query.Append("WHERE [zHstINImport].[ID] NOT IN (SELECT [INImport].[ID] FROM [INImport]) ");
            query.Append("GROUP BY [zHstINImport].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImport].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImport].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimport in the database.
        /// </summary>
        /// <param name="inimport">The inimport object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimport in the database.</returns>
        public static string ListHistory(INImport inimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimport != null)
            {
            query.Append("SELECT [zHstINImport].[ID], [zHstINImport].[FKUserID], [zHstINImport].[FKINCampaignID], [zHstINImport].[FKINBatchID], [zHstINImport].[FKINLeadStatusID], [zHstINImport].[FKINDeclineReasonID], [zHstINImport].[FKINPolicyID], [zHstINImport].[FKINLeadID], [zHstINImport].[RefNo], [zHstINImport].[ReferrorPolicyID], [zHstINImport].[FKINReferrorTitleID], [zHstINImport].[Referror], [zHstINImport].[FKINReferrorRelationshipID], [zHstINImport].[ReferrorContact], [zHstINImport].[Gift], [zHstINImport].[PlatinumContactDate], [zHstINImport].[PlatinumContactTime], [zHstINImport].[CancerOption], [zHstINImport].[PlatinumAge], [zHstINImport].[AllocationDate], [zHstINImport].[IsPrinted], [zHstINImport].[DateOfSale], [zHstINImport].[BankCallRef], [zHstINImport].[FKBankCallRefUserID], [zHstINImport].[BankStationNo], [zHstINImport].[BankDate], [zHstINImport].[BankTime], [zHstINImport].[FKBankTelNumberTypeID], [zHstINImport].[SaleCallRef], [zHstINImport].[FKSaleCallRefUserID], [zHstINImport].[SaleStationNo], [zHstINImport].[SaleDate], [zHstINImport].[SaleTime], [zHstINImport].[FKSaleTelNumberTypeID], [zHstINImport].[ConfCallRef], [zHstINImport].[FKConfCallRefUserID], [zHstINImport].[ConfStationNo], [zHstINImport].[ConfDate], [zHstINImport].[ConfTime], [zHstINImport].[FKConfTelNumberTypeID], [zHstINImport].[IsConfirmed], [zHstINImport].[Notes], [zHstINImport].[ImportDate], [zHstINImport].[FKClosureID], [zHstINImport].[Feedback], [zHstINImport].[FeedbackDate], [zHstINImport].[FutureContactDate], [zHstINImport].[FKINImportedPolicyDataID], [zHstINImport].[Testing1], [zHstINImport].[FKINLeadFeedbackID], [zHstINImport].[FKINCancellationReasonID], [zHstINImport].[IsCopied], [zHstINImport].[FKINConfirmationFeedbackID], [zHstINImport].[FKINParentBatchID], [zHstINImport].[BonusLead], [zHstINImport].[FKBatchCallRefUserID], [zHstINImport].[IsMining], [zHstINImport].[ConfWorkDate], [zHstINImport].[IsChecked], [zHstINImport].[CheckedDate], [zHstINImport].[DateBatched], [zHstINImport].[IsCopyDuplicate], [zHstINImport].[FKINDiaryReasonID], [zHstINImport].[FKINCarriedForwardReasonID], [zHstINImport].[FKINCallMonitoringCarriedForwardReasonID], [zHstINImport].[PermissionQuestionAsked], [zHstINImport].[FKINCallMonitoringCancellationReasonID], [zHstINImport].[IsFutureAllocation], [zHstINImport].[MoneyBackDate], [zHstINImport].[ConversionMBDate], [zHstINImport].[ObtainedReferrals] [zHstINImport].[StampDate], [zHstINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImport] ");
                query.Append(" WHERE [zHstINImport].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImport].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimport.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimport to the database.
        /// </summary>
        /// <param name="inimport">The inimport to save.</param>
        /// <returns>A query that can be used to save the inimport to the database.</returns>
        internal static string Save(INImport inimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimport != null)
            {
                if (inimport.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImport] ([ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID] FROM [INImport] WHERE [INImport].[ID] = @ID; ");
                    query.Append("UPDATE [INImport]");
                    parameters = new object[71];
                    query.Append(" SET [FKUserID] = @FKUserID");
                    parameters[0] = Database.GetParameter("@FKUserID", inimport.FKUserID.HasValue ? (object)inimport.FKUserID.Value : DBNull.Value);
                    query.Append(", [FKINCampaignID] = @FKINCampaignID");
                    parameters[1] = Database.GetParameter("@FKINCampaignID", inimport.FKINCampaignID.HasValue ? (object)inimport.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [FKINBatchID] = @FKINBatchID");
                    parameters[2] = Database.GetParameter("@FKINBatchID", inimport.FKINBatchID.HasValue ? (object)inimport.FKINBatchID.Value : DBNull.Value);
                    query.Append(", [FKINLeadStatusID] = @FKINLeadStatusID");
                    parameters[3] = Database.GetParameter("@FKINLeadStatusID", inimport.FKINLeadStatusID.HasValue ? (object)inimport.FKINLeadStatusID.Value : DBNull.Value);
                    query.Append(", [FKINDeclineReasonID] = @FKINDeclineReasonID");
                    parameters[4] = Database.GetParameter("@FKINDeclineReasonID", inimport.FKINDeclineReasonID.HasValue ? (object)inimport.FKINDeclineReasonID.Value : DBNull.Value);
                    query.Append(", [FKINPolicyID] = @FKINPolicyID");
                    parameters[5] = Database.GetParameter("@FKINPolicyID", inimport.FKINPolicyID.HasValue ? (object)inimport.FKINPolicyID.Value : DBNull.Value);
                    query.Append(", [FKINLeadID] = @FKINLeadID");
                    parameters[6] = Database.GetParameter("@FKINLeadID", inimport.FKINLeadID.HasValue ? (object)inimport.FKINLeadID.Value : DBNull.Value);
                    query.Append(", [RefNo] = @RefNo");
                    parameters[7] = Database.GetParameter("@RefNo", string.IsNullOrEmpty(inimport.RefNo) ? DBNull.Value : (object)inimport.RefNo);
                    query.Append(", [ReferrorPolicyID] = @ReferrorPolicyID");
                    parameters[8] = Database.GetParameter("@ReferrorPolicyID", string.IsNullOrEmpty(inimport.ReferrorPolicyID) ? DBNull.Value : (object)inimport.ReferrorPolicyID);
                    query.Append(", [FKINReferrorTitleID] = @FKINReferrorTitleID");
                    parameters[9] = Database.GetParameter("@FKINReferrorTitleID", inimport.FKINReferrorTitleID.HasValue ? (object)inimport.FKINReferrorTitleID.Value : DBNull.Value);
                    query.Append(", [Referror] = @Referror");
                    parameters[10] = Database.GetParameter("@Referror", string.IsNullOrEmpty(inimport.Referror) ? DBNull.Value : (object)inimport.Referror);
                    query.Append(", [FKINReferrorRelationshipID] = @FKINReferrorRelationshipID");
                    parameters[11] = Database.GetParameter("@FKINReferrorRelationshipID", inimport.FKINReferrorRelationshipID.HasValue ? (object)inimport.FKINReferrorRelationshipID.Value : DBNull.Value);
                    query.Append(", [ReferrorContact] = @ReferrorContact");
                    parameters[12] = Database.GetParameter("@ReferrorContact", string.IsNullOrEmpty(inimport.ReferrorContact) ? DBNull.Value : (object)inimport.ReferrorContact);
                    query.Append(", [Gift] = @Gift");
                    parameters[13] = Database.GetParameter("@Gift", string.IsNullOrEmpty(inimport.Gift) ? DBNull.Value : (object)inimport.Gift);
                    query.Append(", [PlatinumContactDate] = @PlatinumContactDate");
                    parameters[14] = Database.GetParameter("@PlatinumContactDate", inimport.PlatinumContactDate.HasValue ? (object)inimport.PlatinumContactDate.Value : DBNull.Value);
                    query.Append(", [PlatinumContactTime] = @PlatinumContactTime");
                    parameters[15] = Database.GetParameter("@PlatinumContactTime", inimport.PlatinumContactTime.HasValue ? (object)inimport.PlatinumContactTime.Value : DBNull.Value);
                    query.Append(", [CancerOption] = @CancerOption");
                    parameters[16] = Database.GetParameter("@CancerOption", string.IsNullOrEmpty(inimport.CancerOption) ? DBNull.Value : (object)inimport.CancerOption);
                    query.Append(", [PlatinumAge] = @PlatinumAge");
                    parameters[17] = Database.GetParameter("@PlatinumAge", inimport.PlatinumAge.HasValue ? (object)inimport.PlatinumAge.Value : DBNull.Value);
                    query.Append(", [AllocationDate] = @AllocationDate");
                    parameters[18] = Database.GetParameter("@AllocationDate", inimport.AllocationDate.HasValue ? (object)inimport.AllocationDate.Value : DBNull.Value);
                    query.Append(", [IsPrinted] = @IsPrinted");
                    parameters[19] = Database.GetParameter("@IsPrinted", inimport.IsPrinted.HasValue ? (object)inimport.IsPrinted.Value : DBNull.Value);
                    query.Append(", [DateOfSale] = @DateOfSale");
                    parameters[20] = Database.GetParameter("@DateOfSale", inimport.DateOfSale.HasValue ? (object)inimport.DateOfSale.Value : DBNull.Value);
                    query.Append(", [BankCallRef] = @BankCallRef");
                    parameters[21] = Database.GetParameter("@BankCallRef", string.IsNullOrEmpty(inimport.BankCallRef) ? DBNull.Value : (object)inimport.BankCallRef);
                    query.Append(", [FKBankCallRefUserID] = @FKBankCallRefUserID");
                    parameters[22] = Database.GetParameter("@FKBankCallRefUserID", inimport.FKBankCallRefUserID.HasValue ? (object)inimport.FKBankCallRefUserID.Value : DBNull.Value);
                    query.Append(", [BankStationNo] = @BankStationNo");
                    parameters[23] = Database.GetParameter("@BankStationNo", string.IsNullOrEmpty(inimport.BankStationNo) ? DBNull.Value : (object)inimport.BankStationNo);
                    query.Append(", [BankDate] = @BankDate");
                    parameters[24] = Database.GetParameter("@BankDate", inimport.BankDate.HasValue ? (object)inimport.BankDate.Value : DBNull.Value);
                    query.Append(", [BankTime] = @BankTime");
                    parameters[25] = Database.GetParameter("@BankTime", inimport.BankTime.HasValue ? (object)inimport.BankTime.Value : DBNull.Value);
                    query.Append(", [FKBankTelNumberTypeID] = @FKBankTelNumberTypeID");
                    parameters[26] = Database.GetParameter("@FKBankTelNumberTypeID", inimport.FKBankTelNumberTypeID.HasValue ? (object)inimport.FKBankTelNumberTypeID.Value : DBNull.Value);
                    query.Append(", [SaleCallRef] = @SaleCallRef");
                    parameters[27] = Database.GetParameter("@SaleCallRef", string.IsNullOrEmpty(inimport.SaleCallRef) ? DBNull.Value : (object)inimport.SaleCallRef);
                    query.Append(", [FKSaleCallRefUserID] = @FKSaleCallRefUserID");
                    parameters[28] = Database.GetParameter("@FKSaleCallRefUserID", inimport.FKSaleCallRefUserID.HasValue ? (object)inimport.FKSaleCallRefUserID.Value : DBNull.Value);
                    query.Append(", [SaleStationNo] = @SaleStationNo");
                    parameters[29] = Database.GetParameter("@SaleStationNo", string.IsNullOrEmpty(inimport.SaleStationNo) ? DBNull.Value : (object)inimport.SaleStationNo);
                    query.Append(", [SaleDate] = @SaleDate");
                    parameters[30] = Database.GetParameter("@SaleDate", inimport.SaleDate.HasValue ? (object)inimport.SaleDate.Value : DBNull.Value);
                    query.Append(", [SaleTime] = @SaleTime");
                    parameters[31] = Database.GetParameter("@SaleTime", inimport.SaleTime.HasValue ? (object)inimport.SaleTime.Value : DBNull.Value);
                    query.Append(", [FKSaleTelNumberTypeID] = @FKSaleTelNumberTypeID");
                    parameters[32] = Database.GetParameter("@FKSaleTelNumberTypeID", inimport.FKSaleTelNumberTypeID.HasValue ? (object)inimport.FKSaleTelNumberTypeID.Value : DBNull.Value);
                    query.Append(", [ConfCallRef] = @ConfCallRef");
                    parameters[33] = Database.GetParameter("@ConfCallRef", string.IsNullOrEmpty(inimport.ConfCallRef) ? DBNull.Value : (object)inimport.ConfCallRef);
                    query.Append(", [FKConfCallRefUserID] = @FKConfCallRefUserID");
                    parameters[34] = Database.GetParameter("@FKConfCallRefUserID", inimport.FKConfCallRefUserID.HasValue ? (object)inimport.FKConfCallRefUserID.Value : DBNull.Value);
                    query.Append(", [ConfStationNo] = @ConfStationNo");
                    parameters[35] = Database.GetParameter("@ConfStationNo", string.IsNullOrEmpty(inimport.ConfStationNo) ? DBNull.Value : (object)inimport.ConfStationNo);
                    query.Append(", [ConfDate] = @ConfDate");
                    parameters[36] = Database.GetParameter("@ConfDate", inimport.ConfDate.HasValue ? (object)inimport.ConfDate.Value : DBNull.Value);
                    query.Append(", [ConfTime] = @ConfTime");
                    parameters[37] = Database.GetParameter("@ConfTime", inimport.ConfTime.HasValue ? (object)inimport.ConfTime.Value : DBNull.Value);
                    query.Append(", [FKConfTelNumberTypeID] = @FKConfTelNumberTypeID");
                    parameters[38] = Database.GetParameter("@FKConfTelNumberTypeID", inimport.FKConfTelNumberTypeID.HasValue ? (object)inimport.FKConfTelNumberTypeID.Value : DBNull.Value);
                    query.Append(", [IsConfirmed] = @IsConfirmed");
                    parameters[39] = Database.GetParameter("@IsConfirmed", inimport.IsConfirmed.HasValue ? (object)inimport.IsConfirmed.Value : DBNull.Value);
                    query.Append(", [Notes] = @Notes");
                    parameters[40] = Database.GetParameter("@Notes", string.IsNullOrEmpty(inimport.Notes) ? DBNull.Value : (object)inimport.Notes);
                    query.Append(", [ImportDate] = @ImportDate");
                    parameters[41] = Database.GetParameter("@ImportDate", inimport.ImportDate.HasValue ? (object)inimport.ImportDate.Value : DBNull.Value);
                    query.Append(", [FKClosureID] = @FKClosureID");
                    parameters[42] = Database.GetParameter("@FKClosureID", inimport.FKClosureID.HasValue ? (object)inimport.FKClosureID.Value : DBNull.Value);
                    query.Append(", [Feedback] = @Feedback");
                    parameters[43] = Database.GetParameter("@Feedback", string.IsNullOrEmpty(inimport.Feedback) ? DBNull.Value : (object)inimport.Feedback);
                    query.Append(", [FeedbackDate] = @FeedbackDate");
                    parameters[44] = Database.GetParameter("@FeedbackDate", inimport.FeedbackDate.HasValue ? (object)inimport.FeedbackDate.Value : DBNull.Value);
                    query.Append(", [FutureContactDate] = @FutureContactDate");
                    parameters[45] = Database.GetParameter("@FutureContactDate", inimport.FutureContactDate.HasValue ? (object)inimport.FutureContactDate.Value : DBNull.Value);
                    query.Append(", [FKINImportedPolicyDataID] = @FKINImportedPolicyDataID");
                    parameters[46] = Database.GetParameter("@FKINImportedPolicyDataID", inimport.FKINImportedPolicyDataID.HasValue ? (object)inimport.FKINImportedPolicyDataID.Value : DBNull.Value);
                    query.Append(", [Testing1] = @Testing1");
                    parameters[47] = Database.GetParameter("@Testing1", string.IsNullOrEmpty(inimport.Testing1) ? DBNull.Value : (object)inimport.Testing1);
                    query.Append(", [FKINLeadFeedbackID] = @FKINLeadFeedbackID");
                    parameters[48] = Database.GetParameter("@FKINLeadFeedbackID", inimport.FKINLeadFeedbackID.HasValue ? (object)inimport.FKINLeadFeedbackID.Value : DBNull.Value);
                    query.Append(", [FKINCancellationReasonID] = @FKINCancellationReasonID");
                    parameters[49] = Database.GetParameter("@FKINCancellationReasonID", inimport.FKINCancellationReasonID.HasValue ? (object)inimport.FKINCancellationReasonID.Value : DBNull.Value);
                    query.Append(", [IsCopied] = @IsCopied");
                    parameters[50] = Database.GetParameter("@IsCopied", inimport.IsCopied.HasValue ? (object)inimport.IsCopied.Value : DBNull.Value);
                    query.Append(", [FKINConfirmationFeedbackID] = @FKINConfirmationFeedbackID");
                    parameters[51] = Database.GetParameter("@FKINConfirmationFeedbackID", inimport.FKINConfirmationFeedbackID.HasValue ? (object)inimport.FKINConfirmationFeedbackID.Value : DBNull.Value);
                    query.Append(", [FKINParentBatchID] = @FKINParentBatchID");
                    parameters[52] = Database.GetParameter("@FKINParentBatchID", inimport.FKINParentBatchID.HasValue ? (object)inimport.FKINParentBatchID.Value : DBNull.Value);
                    query.Append(", [BonusLead] = @BonusLead");
                    parameters[53] = Database.GetParameter("@BonusLead", inimport.BonusLead.HasValue ? (object)inimport.BonusLead.Value : DBNull.Value);
                    query.Append(", [FKBatchCallRefUserID] = @FKBatchCallRefUserID");
                    parameters[54] = Database.GetParameter("@FKBatchCallRefUserID", inimport.FKBatchCallRefUserID.HasValue ? (object)inimport.FKBatchCallRefUserID.Value : DBNull.Value);
                    query.Append(", [IsMining] = @IsMining");
                    parameters[55] = Database.GetParameter("@IsMining", inimport.IsMining.HasValue ? (object)inimport.IsMining.Value : DBNull.Value);
                    query.Append(", [ConfWorkDate] = @ConfWorkDate");
                    parameters[56] = Database.GetParameter("@ConfWorkDate", inimport.ConfWorkDate.HasValue ? (object)inimport.ConfWorkDate.Value : DBNull.Value);
                    query.Append(", [IsChecked] = @IsChecked");
                    parameters[57] = Database.GetParameter("@IsChecked", inimport.IsChecked.HasValue ? (object)inimport.IsChecked.Value : DBNull.Value);
                    query.Append(", [CheckedDate] = @CheckedDate");
                    parameters[58] = Database.GetParameter("@CheckedDate", inimport.CheckedDate.HasValue ? (object)inimport.CheckedDate.Value : DBNull.Value);
                    query.Append(", [DateBatched] = @DateBatched");
                    parameters[59] = Database.GetParameter("@DateBatched", inimport.DateBatched.HasValue ? (object)inimport.DateBatched.Value : DBNull.Value);
                    query.Append(", [IsCopyDuplicate] = @IsCopyDuplicate");
                    parameters[60] = Database.GetParameter("@IsCopyDuplicate", inimport.IsCopyDuplicate.HasValue ? (object)inimport.IsCopyDuplicate.Value : DBNull.Value);
                    query.Append(", [FKINDiaryReasonID] = @FKINDiaryReasonID");
                    parameters[61] = Database.GetParameter("@FKINDiaryReasonID", inimport.FKINDiaryReasonID.HasValue ? (object)inimport.FKINDiaryReasonID.Value : DBNull.Value);
                    query.Append(", [FKINCarriedForwardReasonID] = @FKINCarriedForwardReasonID");
                    parameters[62] = Database.GetParameter("@FKINCarriedForwardReasonID", inimport.FKINCarriedForwardReasonID.HasValue ? (object)inimport.FKINCarriedForwardReasonID.Value : DBNull.Value);
                    query.Append(", [FKINCallMonitoringCarriedForwardReasonID] = @FKINCallMonitoringCarriedForwardReasonID");
                    parameters[63] = Database.GetParameter("@FKINCallMonitoringCarriedForwardReasonID", inimport.FKINCallMonitoringCarriedForwardReasonID.HasValue ? (object)inimport.FKINCallMonitoringCarriedForwardReasonID.Value : DBNull.Value);
                    query.Append(", [PermissionQuestionAsked] = @PermissionQuestionAsked");
                    parameters[64] = Database.GetParameter("@PermissionQuestionAsked", inimport.PermissionQuestionAsked.HasValue ? (object)inimport.PermissionQuestionAsked.Value : DBNull.Value);
                    query.Append(", [FKINCallMonitoringCancellationReasonID] = @FKINCallMonitoringCancellationReasonID");
                    parameters[65] = Database.GetParameter("@FKINCallMonitoringCancellationReasonID", inimport.FKINCallMonitoringCancellationReasonID.HasValue ? (object)inimport.FKINCallMonitoringCancellationReasonID.Value : DBNull.Value);
                    query.Append(", [IsFutureAllocation] = @IsFutureAllocation");
                    parameters[66] = Database.GetParameter("@IsFutureAllocation", inimport.IsFutureAllocation.HasValue ? (object)inimport.IsFutureAllocation.Value : DBNull.Value);
                    query.Append(", [MoneyBackDate] = @MoneyBackDate");
                    parameters[67] = Database.GetParameter("@MoneyBackDate", inimport.MoneyBackDate.HasValue ? (object)inimport.MoneyBackDate.Value : DBNull.Value);
                    query.Append(", [ConversionMBDate] = @ConversionMBDate");
                    parameters[68] = Database.GetParameter("@ConversionMBDate", inimport.ConversionMBDate.HasValue ? (object)inimport.ConversionMBDate.Value : DBNull.Value);
                    query.Append(", [ObtainedReferrals] = @ObtainedReferrals");
                    parameters[69] = Database.GetParameter("@ObtainedReferrals", inimport.ObtainedReferrals.HasValue ? (object)inimport.ObtainedReferrals.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImport].[ID] = @ID"); 
                    parameters[70] = Database.GetParameter("@ID", inimport.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImport] ([FKUserID], [FKINCampaignID], [FKINBatchID], [FKINLeadStatusID], [FKINDeclineReasonID], [FKINPolicyID], [FKINLeadID], [RefNo], [ReferrorPolicyID], [FKINReferrorTitleID], [Referror], [FKINReferrorRelationshipID], [ReferrorContact], [Gift], [PlatinumContactDate], [PlatinumContactTime], [CancerOption], [PlatinumAge], [AllocationDate], [IsPrinted], [DateOfSale], [BankCallRef], [FKBankCallRefUserID], [BankStationNo], [BankDate], [BankTime], [FKBankTelNumberTypeID], [SaleCallRef], [FKSaleCallRefUserID], [SaleStationNo], [SaleDate], [SaleTime], [FKSaleTelNumberTypeID], [ConfCallRef], [FKConfCallRefUserID], [ConfStationNo], [ConfDate], [ConfTime], [FKConfTelNumberTypeID], [IsConfirmed], [Notes], [ImportDate], [FKClosureID], [Feedback], [FeedbackDate], [FutureContactDate], [FKINImportedPolicyDataID], [Testing1], [FKINLeadFeedbackID], [FKINCancellationReasonID], [IsCopied], [FKINConfirmationFeedbackID], [FKINParentBatchID], [BonusLead], [FKBatchCallRefUserID], [IsMining], [ConfWorkDate], [IsChecked], [CheckedDate], [DateBatched], [IsCopyDuplicate], [FKINDiaryReasonID], [FKINCarriedForwardReasonID], [FKINCallMonitoringCarriedForwardReasonID], [PermissionQuestionAsked], [FKINCallMonitoringCancellationReasonID], [IsFutureAllocation], [MoneyBackDate], [ConversionMBDate], [ObtainedReferrals], [StampDate], [StampUserID]) VALUES(@FKUserID, @FKINCampaignID, @FKINBatchID, @FKINLeadStatusID, @FKINDeclineReasonID, @FKINPolicyID, @FKINLeadID, @RefNo, @ReferrorPolicyID, @FKINReferrorTitleID, @Referror, @FKINReferrorRelationshipID, @ReferrorContact, @Gift, @PlatinumContactDate, @PlatinumContactTime, @CancerOption, @PlatinumAge, @AllocationDate, @IsPrinted, @DateOfSale, @BankCallRef, @FKBankCallRefUserID, @BankStationNo, @BankDate, @BankTime, @FKBankTelNumberTypeID, @SaleCallRef, @FKSaleCallRefUserID, @SaleStationNo, @SaleDate, @SaleTime, @FKSaleTelNumberTypeID, @ConfCallRef, @FKConfCallRefUserID, @ConfStationNo, @ConfDate, @ConfTime, @FKConfTelNumberTypeID, @IsConfirmed, @Notes, @ImportDate, @FKClosureID, @Feedback, @FeedbackDate, @FutureContactDate, @FKINImportedPolicyDataID, @Testing1, @FKINLeadFeedbackID, @FKINCancellationReasonID, @IsCopied, @FKINConfirmationFeedbackID, @FKINParentBatchID, @BonusLead, @FKBatchCallRefUserID, @IsMining, @ConfWorkDate, @IsChecked, @CheckedDate, @DateBatched, @IsCopyDuplicate, @FKINDiaryReasonID, @FKINCarriedForwardReasonID, @FKINCallMonitoringCarriedForwardReasonID, @PermissionQuestionAsked, @FKINCallMonitoringCancellationReasonID, @IsFutureAllocation, @MoneyBackDate, @ConversionMBDate, @ObtainedReferrals," + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[70];
                    parameters[0] = Database.GetParameter("@FKUserID", inimport.FKUserID.HasValue ? (object)inimport.FKUserID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINCampaignID", inimport.FKINCampaignID.HasValue ? (object)inimport.FKINCampaignID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKINBatchID", inimport.FKINBatchID.HasValue ? (object)inimport.FKINBatchID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKINLeadStatusID", inimport.FKINLeadStatusID.HasValue ? (object)inimport.FKINLeadStatusID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@FKINDeclineReasonID", inimport.FKINDeclineReasonID.HasValue ? (object)inimport.FKINDeclineReasonID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@FKINPolicyID", inimport.FKINPolicyID.HasValue ? (object)inimport.FKINPolicyID.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@FKINLeadID", inimport.FKINLeadID.HasValue ? (object)inimport.FKINLeadID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@RefNo", string.IsNullOrEmpty(inimport.RefNo) ? DBNull.Value : (object)inimport.RefNo);
                    parameters[8] = Database.GetParameter("@ReferrorPolicyID", string.IsNullOrEmpty(inimport.ReferrorPolicyID) ? DBNull.Value : (object)inimport.ReferrorPolicyID);
                    parameters[9] = Database.GetParameter("@FKINReferrorTitleID", inimport.FKINReferrorTitleID.HasValue ? (object)inimport.FKINReferrorTitleID.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@Referror", string.IsNullOrEmpty(inimport.Referror) ? DBNull.Value : (object)inimport.Referror);
                    parameters[11] = Database.GetParameter("@FKINReferrorRelationshipID", inimport.FKINReferrorRelationshipID.HasValue ? (object)inimport.FKINReferrorRelationshipID.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@ReferrorContact", string.IsNullOrEmpty(inimport.ReferrorContact) ? DBNull.Value : (object)inimport.ReferrorContact);
                    parameters[13] = Database.GetParameter("@Gift", string.IsNullOrEmpty(inimport.Gift) ? DBNull.Value : (object)inimport.Gift);
                    parameters[14] = Database.GetParameter("@PlatinumContactDate", inimport.PlatinumContactDate.HasValue ? (object)inimport.PlatinumContactDate.Value : DBNull.Value);
                    parameters[15] = Database.GetParameter("@PlatinumContactTime", inimport.PlatinumContactTime.HasValue ? (object)inimport.PlatinumContactTime.Value : DBNull.Value);
                    parameters[16] = Database.GetParameter("@CancerOption", string.IsNullOrEmpty(inimport.CancerOption) ? DBNull.Value : (object)inimport.CancerOption);
                    parameters[17] = Database.GetParameter("@PlatinumAge", inimport.PlatinumAge.HasValue ? (object)inimport.PlatinumAge.Value : DBNull.Value);
                    parameters[18] = Database.GetParameter("@AllocationDate", inimport.AllocationDate.HasValue ? (object)inimport.AllocationDate.Value : DBNull.Value);
                    parameters[19] = Database.GetParameter("@IsPrinted", inimport.IsPrinted.HasValue ? (object)inimport.IsPrinted.Value : DBNull.Value);
                    parameters[20] = Database.GetParameter("@DateOfSale", inimport.DateOfSale.HasValue ? (object)inimport.DateOfSale.Value : DBNull.Value);
                    parameters[21] = Database.GetParameter("@BankCallRef", string.IsNullOrEmpty(inimport.BankCallRef) ? DBNull.Value : (object)inimport.BankCallRef);
                    parameters[22] = Database.GetParameter("@FKBankCallRefUserID", inimport.FKBankCallRefUserID.HasValue ? (object)inimport.FKBankCallRefUserID.Value : DBNull.Value);
                    parameters[23] = Database.GetParameter("@BankStationNo", string.IsNullOrEmpty(inimport.BankStationNo) ? DBNull.Value : (object)inimport.BankStationNo);
                    parameters[24] = Database.GetParameter("@BankDate", inimport.BankDate.HasValue ? (object)inimport.BankDate.Value : DBNull.Value);
                    parameters[25] = Database.GetParameter("@BankTime", inimport.BankTime.HasValue ? (object)inimport.BankTime.Value : DBNull.Value);
                    parameters[26] = Database.GetParameter("@FKBankTelNumberTypeID", inimport.FKBankTelNumberTypeID.HasValue ? (object)inimport.FKBankTelNumberTypeID.Value : DBNull.Value);
                    parameters[27] = Database.GetParameter("@SaleCallRef", string.IsNullOrEmpty(inimport.SaleCallRef) ? DBNull.Value : (object)inimport.SaleCallRef);
                    parameters[28] = Database.GetParameter("@FKSaleCallRefUserID", inimport.FKSaleCallRefUserID.HasValue ? (object)inimport.FKSaleCallRefUserID.Value : DBNull.Value);
                    parameters[29] = Database.GetParameter("@SaleStationNo", string.IsNullOrEmpty(inimport.SaleStationNo) ? DBNull.Value : (object)inimport.SaleStationNo);
                    parameters[30] = Database.GetParameter("@SaleDate", inimport.SaleDate.HasValue ? (object)inimport.SaleDate.Value : DBNull.Value);
                    parameters[31] = Database.GetParameter("@SaleTime", inimport.SaleTime.HasValue ? (object)inimport.SaleTime.Value : DBNull.Value);
                    parameters[32] = Database.GetParameter("@FKSaleTelNumberTypeID", inimport.FKSaleTelNumberTypeID.HasValue ? (object)inimport.FKSaleTelNumberTypeID.Value : DBNull.Value);
                    parameters[33] = Database.GetParameter("@ConfCallRef", string.IsNullOrEmpty(inimport.ConfCallRef) ? DBNull.Value : (object)inimport.ConfCallRef);
                    parameters[34] = Database.GetParameter("@FKConfCallRefUserID", inimport.FKConfCallRefUserID.HasValue ? (object)inimport.FKConfCallRefUserID.Value : DBNull.Value);
                    parameters[35] = Database.GetParameter("@ConfStationNo", string.IsNullOrEmpty(inimport.ConfStationNo) ? DBNull.Value : (object)inimport.ConfStationNo);
                    parameters[36] = Database.GetParameter("@ConfDate", inimport.ConfDate.HasValue ? (object)inimport.ConfDate.Value : DBNull.Value);
                    parameters[37] = Database.GetParameter("@ConfTime", inimport.ConfTime.HasValue ? (object)inimport.ConfTime.Value : DBNull.Value);
                    parameters[38] = Database.GetParameter("@FKConfTelNumberTypeID", inimport.FKConfTelNumberTypeID.HasValue ? (object)inimport.FKConfTelNumberTypeID.Value : DBNull.Value);
                    parameters[39] = Database.GetParameter("@IsConfirmed", inimport.IsConfirmed.HasValue ? (object)inimport.IsConfirmed.Value : DBNull.Value);
                    parameters[40] = Database.GetParameter("@Notes", string.IsNullOrEmpty(inimport.Notes) ? DBNull.Value : (object)inimport.Notes);
                    parameters[41] = Database.GetParameter("@ImportDate", inimport.ImportDate.HasValue ? (object)inimport.ImportDate.Value : DBNull.Value);
                    parameters[42] = Database.GetParameter("@FKClosureID", inimport.FKClosureID.HasValue ? (object)inimport.FKClosureID.Value : DBNull.Value);
                    parameters[43] = Database.GetParameter("@Feedback", string.IsNullOrEmpty(inimport.Feedback) ? DBNull.Value : (object)inimport.Feedback);
                    parameters[44] = Database.GetParameter("@FeedbackDate", inimport.FeedbackDate.HasValue ? (object)inimport.FeedbackDate.Value : DBNull.Value);
                    parameters[45] = Database.GetParameter("@FutureContactDate", inimport.FutureContactDate.HasValue ? (object)inimport.FutureContactDate.Value : DBNull.Value);
                    parameters[46] = Database.GetParameter("@FKINImportedPolicyDataID", inimport.FKINImportedPolicyDataID.HasValue ? (object)inimport.FKINImportedPolicyDataID.Value : DBNull.Value);
                    parameters[47] = Database.GetParameter("@Testing1", string.IsNullOrEmpty(inimport.Testing1) ? DBNull.Value : (object)inimport.Testing1);
                    parameters[48] = Database.GetParameter("@FKINLeadFeedbackID", inimport.FKINLeadFeedbackID.HasValue ? (object)inimport.FKINLeadFeedbackID.Value : DBNull.Value);
                    parameters[49] = Database.GetParameter("@FKINCancellationReasonID", inimport.FKINCancellationReasonID.HasValue ? (object)inimport.FKINCancellationReasonID.Value : DBNull.Value);
                    parameters[50] = Database.GetParameter("@IsCopied", inimport.IsCopied.HasValue ? (object)inimport.IsCopied.Value : DBNull.Value);
                    parameters[51] = Database.GetParameter("@FKINConfirmationFeedbackID", inimport.FKINConfirmationFeedbackID.HasValue ? (object)inimport.FKINConfirmationFeedbackID.Value : DBNull.Value);
                    parameters[52] = Database.GetParameter("@FKINParentBatchID", inimport.FKINParentBatchID.HasValue ? (object)inimport.FKINParentBatchID.Value : DBNull.Value);
                    parameters[53] = Database.GetParameter("@BonusLead", inimport.BonusLead.HasValue ? (object)inimport.BonusLead.Value : DBNull.Value);
                    parameters[54] = Database.GetParameter("@FKBatchCallRefUserID", inimport.FKBatchCallRefUserID.HasValue ? (object)inimport.FKBatchCallRefUserID.Value : DBNull.Value);
                    parameters[55] = Database.GetParameter("@IsMining", inimport.IsMining.HasValue ? (object)inimport.IsMining.Value : DBNull.Value);
                    parameters[56] = Database.GetParameter("@ConfWorkDate", inimport.ConfWorkDate.HasValue ? (object)inimport.ConfWorkDate.Value : DBNull.Value);
                    parameters[57] = Database.GetParameter("@IsChecked", inimport.IsChecked.HasValue ? (object)inimport.IsChecked.Value : DBNull.Value);
                    parameters[58] = Database.GetParameter("@CheckedDate", inimport.CheckedDate.HasValue ? (object)inimport.CheckedDate.Value : DBNull.Value);
                    parameters[59] = Database.GetParameter("@DateBatched", inimport.DateBatched.HasValue ? (object)inimport.DateBatched.Value : DBNull.Value);
                    parameters[60] = Database.GetParameter("@IsCopyDuplicate", inimport.IsCopyDuplicate.HasValue ? (object)inimport.IsCopyDuplicate.Value : DBNull.Value);
                    parameters[61] = Database.GetParameter("@FKINDiaryReasonID", inimport.FKINDiaryReasonID.HasValue ? (object)inimport.FKINDiaryReasonID.Value : DBNull.Value);
                    parameters[62] = Database.GetParameter("@FKINCarriedForwardReasonID", inimport.FKINCarriedForwardReasonID.HasValue ? (object)inimport.FKINCarriedForwardReasonID.Value : DBNull.Value);
                    parameters[63] = Database.GetParameter("@FKINCallMonitoringCarriedForwardReasonID", inimport.FKINCallMonitoringCarriedForwardReasonID.HasValue ? (object)inimport.FKINCallMonitoringCarriedForwardReasonID.Value : DBNull.Value);
                    parameters[64] = Database.GetParameter("@PermissionQuestionAsked", inimport.PermissionQuestionAsked.HasValue ? (object)inimport.PermissionQuestionAsked.Value : DBNull.Value);
                    parameters[65] = Database.GetParameter("@FKINCallMonitoringCancellationReasonID", inimport.FKINCallMonitoringCancellationReasonID.HasValue ? (object)inimport.FKINCallMonitoringCancellationReasonID.Value : DBNull.Value);
                    parameters[66] = Database.GetParameter("@IsFutureAllocation", inimport.IsFutureAllocation.HasValue ? (object)inimport.IsFutureAllocation.Value : DBNull.Value);
                    parameters[67] = Database.GetParameter("@MoneyBackDate", inimport.MoneyBackDate.HasValue ? (object)inimport.MoneyBackDate.Value : DBNull.Value);
                    parameters[68] = Database.GetParameter("@ConversionMBDate", inimport.ConversionMBDate.HasValue ? (object)inimport.ConversionMBDate.Value : DBNull.Value);
                    parameters[69] = Database.GetParameter("@ObtainedReferrals", inimport.ObtainedReferrals.HasValue ? (object)inimport.ObtainedReferrals.Value : DBNull.Value);

                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimports that match the search criteria.
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
        /// <param name="obtainedreferrals"></param>
        /// <param name="moneybackdate"></param>
        /// <returns>A query that can be used to search for inimports based on the search criteria.</returns>
        internal static string Search(long? fkuserid, long? fkincampaignid, long? fkinbatchid, long? fkinleadstatusid, long? fkindeclinereasonid, long? fkinpolicyid, long? fkinleadid, string refno, string referrorpolicyid, long? fkinreferrortitleid, string referror, long? fkinreferrorrelationshipid, string referrorcontact, string gift, DateTime? platinumcontactdate, TimeSpan? platinumcontacttime, string canceroption, short? platinumage, DateTime? allocationdate, Byte? isprinted, DateTime? dateofsale, string bankcallref, long? fkbankcallrefuserid, string bankstationno, DateTime? bankdate, TimeSpan? banktime, long? fkbanktelnumbertypeid, string salecallref, long? fksalecallrefuserid, string salestationno, DateTime? saledate, TimeSpan? saletime, long? fksaletelnumbertypeid, string confcallref, long? fkconfcallrefuserid, string confstationno, DateTime? confdate, TimeSpan? conftime, long? fkconftelnumbertypeid, bool? isconfirmed, string notes, DateTime? importdate, long? fkclosureid, string feedback, DateTime? feedbackdate, DateTime? futurecontactdate, long? fkinimportedpolicydataid, string testing1, long? fkinleadfeedbackid, long? fkincancellationreasonid, bool? iscopied, long? fkinconfirmationfeedbackid, long? fkinparentbatchid, bool? bonuslead, long? fkbatchcallrefuserid, bool? ismining, DateTime? confworkdate, bool? ischecked, DateTime? checkeddate, DateTime? datebatched, bool? iscopyduplicate, long? fkindiaryreasonid, long? fkincarriedforwardreasonid, long? fkincallmonitoringcarriedforwardreasonid, bool? permissionquestionasked, long? fkincallmonitoringcancellationreasonid, bool? isfutureallocation, DateTime? moneybackdate, DateTime? conversionmbdate, bool? obtainedreferrals)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKUserID] = " + fkuserid + "");
            }
            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINCampaignID] = " + fkincampaignid + "");
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINBatchID] = " + fkinbatchid + "");
            }
            if (fkinleadstatusid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINLeadStatusID] = " + fkinleadstatusid + "");
            }
            if (fkindeclinereasonid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINDeclineReasonID] = " + fkindeclinereasonid + "");
            }
            if (fkinpolicyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINPolicyID] = " + fkinpolicyid + "");
            }
            if (fkinleadid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINLeadID] = " + fkinleadid + "");
            }
            if (refno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[RefNo] LIKE '" + refno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (referrorpolicyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ReferrorPolicyID] LIKE '" + referrorpolicyid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkinreferrortitleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINReferrorTitleID] = " + fkinreferrortitleid + "");
            }
            if (referror != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[Referror] LIKE '" + referror.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkinreferrorrelationshipid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINReferrorRelationshipID] = " + fkinreferrorrelationshipid + "");
            }
            if (referrorcontact != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ReferrorContact] LIKE '" + referrorcontact.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (gift != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[Gift] LIKE '" + gift.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (platinumcontactdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[PlatinumContactDate] = '" + platinumcontactdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (platinumcontacttime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[PlatinumContactTime] = " + platinumcontacttime + "");
            }
            if (canceroption != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[CancerOption] LIKE '" + canceroption.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (platinumage != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[PlatinumAge] = " + platinumage + "");
            }
            if (allocationdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[AllocationDate] = '" + allocationdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (isprinted != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[IsPrinted] = " + isprinted + "");
            }
            if (dateofsale != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[DateOfSale] = '" + dateofsale.Value.ToString(Database.DateFormat) + "'");
            }
            if (bankcallref != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[BankCallRef] LIKE '" + bankcallref.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkbankcallrefuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKBankCallRefUserID] = " + fkbankcallrefuserid + "");
            }
            if (bankstationno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[BankStationNo] LIKE '" + bankstationno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (bankdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[BankDate] = '" + bankdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (banktime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[BankTime] = " + banktime + "");
            }
            if (fkbanktelnumbertypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKBankTelNumberTypeID] = " + fkbanktelnumbertypeid + "");
            }
            if (salecallref != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[SaleCallRef] LIKE '" + salecallref.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fksalecallrefuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKSaleCallRefUserID] = " + fksalecallrefuserid + "");
            }
            if (salestationno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[SaleStationNo] LIKE '" + salestationno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (saledate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[SaleDate] = '" + saledate.Value.ToString(Database.DateFormat) + "'");
            }
            if (saletime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[SaleTime] = " + saletime + "");
            }
            if (fksaletelnumbertypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKSaleTelNumberTypeID] = " + fksaletelnumbertypeid + "");
            }
            if (confcallref != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ConfCallRef] LIKE '" + confcallref.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkconfcallrefuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKConfCallRefUserID] = " + fkconfcallrefuserid + "");
            }
            if (confstationno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ConfStationNo] LIKE '" + confstationno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (confdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ConfDate] = '" + confdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (conftime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ConfTime] = " + conftime + "");
            }
            if (fkconftelnumbertypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKConfTelNumberTypeID] = " + fkconftelnumbertypeid + "");
            }
            if (isconfirmed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[IsConfirmed] = " + ((bool)isconfirmed ? "1" : "0"));
            }
            if (notes != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[Notes] LIKE '" + notes.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (importdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ImportDate] = '" + importdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (fkclosureid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKClosureID] = " + fkclosureid + "");
            }
            if (feedback != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[Feedback] LIKE '" + feedback.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (feedbackdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FeedbackDate] = '" + feedbackdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (futurecontactdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FutureContactDate] = '" + futurecontactdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (fkinimportedpolicydataid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINImportedPolicyDataID] = " + fkinimportedpolicydataid + "");
            }
            if (testing1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[Testing1] LIKE '" + testing1.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkinleadfeedbackid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINLeadFeedbackID] = " + fkinleadfeedbackid + "");
            }
            if (fkincancellationreasonid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINCancellationReasonID] = " + fkincancellationreasonid + "");
            }
            if (iscopied != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[IsCopied] = " + ((bool)iscopied ? "1" : "0"));
            }
            if (fkinconfirmationfeedbackid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINConfirmationFeedbackID] = " + fkinconfirmationfeedbackid + "");
            }
            if (fkinparentbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINParentBatchID] = " + fkinparentbatchid + "");
            }
            if (bonuslead != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[BonusLead] = " + ((bool)bonuslead ? "1" : "0"));
            }
            if (fkbatchcallrefuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKBatchCallRefUserID] = " + fkbatchcallrefuserid + "");
            }
            if (ismining != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[IsMining] = " + ((bool)ismining ? "1" : "0"));
            }
            if (confworkdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ConfWorkDate] = '" + confworkdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (ischecked != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[IsChecked] = " + ((bool)ischecked ? "1" : "0"));
            }
            if (checkeddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[CheckedDate] = '" + checkeddate.Value.ToString(Database.DateFormat) + "'");
            }
            if (datebatched != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[DateBatched] = '" + datebatched.Value.ToString(Database.DateFormat) + "'");
            }
            if (iscopyduplicate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[IsCopyDuplicate] = " + ((bool)iscopyduplicate ? "1" : "0"));
            }
            if (fkindiaryreasonid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINDiaryReasonID] = " + fkindiaryreasonid + "");
            }
            if (fkincarriedforwardreasonid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINCarriedForwardReasonID] = " + fkincarriedforwardreasonid + "");
            }
            if (fkincallmonitoringcarriedforwardreasonid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINCallMonitoringCarriedForwardReasonID] = " + fkincallmonitoringcarriedforwardreasonid + "");
            }
            if (permissionquestionasked != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[PermissionQuestionAsked] = " + ((bool)permissionquestionasked ? "1" : "0"));
            }
            if (fkincallmonitoringcancellationreasonid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINCallMonitoringCancellationReasonID] = " + fkincallmonitoringcancellationreasonid + "");
            }
            if (isfutureallocation != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[IsFutureAllocation] = " + ((bool)isfutureallocation ? "1" : "0"));
            }
            if(moneybackdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[MoneyBackDate] = '" + moneybackdate.Value.ToString(Database.DateFormat) + "'");
            }
            if(conversionmbdate != null)
            {
                
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ConversionMBDate] = '" + conversionmbdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (obtainedreferrals != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[ObtainedReferrals] = " + ((bool)obtainedreferrals ? "1" : "0"));
            }

            query.Append("SELECT [INImport].[ID], [INImport].[FKUserID], [INImport].[FKINCampaignID], [INImport].[FKINBatchID], [INImport].[FKINLeadStatusID], [INImport].[FKINDeclineReasonID], [INImport].[FKINPolicyID], [INImport].[FKINLeadID], [INImport].[RefNo], [INImport].[ReferrorPolicyID], [INImport].[FKINReferrorTitleID], [INImport].[Referror], [INImport].[FKINReferrorRelationshipID], [INImport].[ReferrorContact], [INImport].[Gift], [INImport].[PlatinumContactDate], [INImport].[PlatinumContactTime], [INImport].[CancerOption], [INImport].[PlatinumAge], [INImport].[AllocationDate], [INImport].[IsPrinted], [INImport].[DateOfSale], [INImport].[BankCallRef], [INImport].[FKBankCallRefUserID], [INImport].[BankStationNo], [INImport].[BankDate], [INImport].[BankTime], [INImport].[FKBankTelNumberTypeID], [INImport].[SaleCallRef], [INImport].[FKSaleCallRefUserID], [INImport].[SaleStationNo], [INImport].[SaleDate], [INImport].[SaleTime], [INImport].[FKSaleTelNumberTypeID], [INImport].[ConfCallRef], [INImport].[FKConfCallRefUserID], [INImport].[ConfStationNo], [INImport].[ConfDate], [INImport].[ConfTime], [INImport].[FKConfTelNumberTypeID], [INImport].[IsConfirmed], [INImport].[Notes], [INImport].[ImportDate], [INImport].[FKClosureID], [INImport].[Feedback], [INImport].[FeedbackDate], [INImport].[FutureContactDate], [INImport].[FKINImportedPolicyDataID], [INImport].[Testing1], [INImport].[FKINLeadFeedbackID], [INImport].[FKINCancellationReasonID], [INImport].[IsCopied], [INImport].[FKINConfirmationFeedbackID], [INImport].[FKINParentBatchID], [INImport].[BonusLead], [INImport].[FKBatchCallRefUserID], [INImport].[IsMining], [INImport].[ConfWorkDate], [INImport].[IsChecked], [INImport].[CheckedDate], [INImport].[DateBatched], [INImport].[IsCopyDuplicate], [INImport].[FKINDiaryReasonID], [INImport].[FKINCarriedForwardReasonID], [INImport].[FKINCallMonitoringCarriedForwardReasonID], [INImport].[PermissionQuestionAsked], [INImport].[FKINCallMonitoringCancellationReasonID], [INImport].[IsFutureAllocation], [INImport].[MoneyBackDate], [INImport].[ConversionMBDate], [INImport].[ObtainedReferrals], [INImport].[StampDate], [INImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImport] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
