using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using System;
using System.Text;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportcallmonitoring objects.
    /// </summary>
    internal abstract partial class INImportCallMonitoringQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportcallmonitoring from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring object to delete.</param>
        /// <returns>A query that can be used to delete the inimportcallmonitoring from the database.</returns>
        internal static string Delete(INImportCallMonitoring inimportcallmonitoring, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportcallmonitoring != null)
            {
                query = "INSERT INTO [zHstINImportCallMonitoring] ([ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasDebiCheckProcessExplainedCorrectly], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasDebiCheckProcessExplainedCorrectly], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [StampDate], [StampUserID] FROM [INImportCallMonitoring] WHERE [INImportCallMonitoring].[ID] = @ID; ";
                query += "DELETE FROM [INImportCallMonitoring] WHERE [INImportCallMonitoring].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportcallmonitoring from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportcallmonitoring from the database.</returns>
        internal static string DeleteHistory(INImportCallMonitoring inimportcallmonitoring, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportcallmonitoring != null)
            {
                query = "DELETE FROM [zHstINImportCallMonitoring] WHERE [zHstINImportCallMonitoring].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportcallmonitoring from the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportcallmonitoring from the database.</returns>
        internal static string UnDelete(INImportCallMonitoring inimportcallmonitoring, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportcallmonitoring != null)
            {
                query = "INSERT INTO [INImportCallMonitoring] ([ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasDebiCheckProcessExplainedCorrectly], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasDebiCheckProcessExplainedCorrectly], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [StampDate], [StampUserID] FROM [zHstINImportCallMonitoring] WHERE [zHstINImportCallMonitoring].[ID] = @ID AND [zHstINImportCallMonitoring].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportCallMonitoring] WHERE [zHstINImportCallMonitoring].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportCallMonitoring] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportCallMonitoring] WHERE [zHstINImportCallMonitoring].[ID] = @ID AND [zHstINImportCallMonitoring].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportCallMonitoring] WHERE [zHstINImportCallMonitoring].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportCallMonitoring] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportcallmonitoring object.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring object to fill.</param>
        /// <returns>A query that can be used to fill the inimportcallmonitoring object.</returns>
        internal static string Fill(INImportCallMonitoring inimportcallmonitoring, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportcallmonitoring != null)
            {
                query = "SELECT [ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasDebiCheckProcessExplainedCorrectly], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [StampDate], [StampUserID] FROM [INImportCallMonitoring] WHERE [INImportCallMonitoring].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportcallmonitoring data.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportcallmonitoring data.</returns>
        internal static string FillData(INImportCallMonitoring inimportcallmonitoring, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportcallmonitoring != null)
            {
            query.Append("SELECT [INImportCallMonitoring].[ID], [INImportCallMonitoring].[FKINImportID], [INImportCallMonitoring].[IsBankingDetailsCapturedCorrectly], [INImportCallMonitoring].[WasAccountVerified], [INImportCallMonitoring].[WasDebitDateConfirmed], [INImportCallMonitoring].[IsAccountInClientsName], [INImportCallMonitoring].[DoesClientHaveSigningPower],  [INImportCallMonitoring].[WasDebiCheckProcessExplainedCorrectly], [INImportCallMonitoring].[WasCorrectClosureQuestionAsked], [INImportCallMonitoring].[WasResponseClearAndPositive], [INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPs], [INImportCallMonitoring].[WasDebitAmountMentionedCorrectly], [INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectly], [INImportCallMonitoring].[WasCorrectCoverCommencementDateMentioned], [INImportCallMonitoring].[WasNonPaymentProcedureExplained], [INImportCallMonitoring].[WasAnnualIncreaseExplained], [INImportCallMonitoring].[WasCorrectQuestionAskedBumpUpClosure], [INImportCallMonitoring].[WasResponseClearAndPositiveBumpUpClosure], [INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPsBumpUpClosure], [INImportCallMonitoring].[WasDebitAmountMentionedCorrectlyBumpUpClosure], [INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [INImportCallMonitoring].[WasCorrectCoverCommencementDateMentionedBumpUpClosure], [INImportCallMonitoring].[WasNonPaymentProcedureExplainedBumpUpClosure], [INImportCallMonitoring].[WasAnnualIncreaseExplainedBumpUpClosure], [INImportCallMonitoring].[FKINCallMonitoringOutcomeID], [INImportCallMonitoring].[Comments], [INImportCallMonitoring].[FKCallMonitoringUserID], [INImportCallMonitoring].[WasCallEvaluatedBySecondaryUser], [INImportCallMonitoring].[FKSecondaryCallMonitoringUserID], [INImportCallMonitoring].[FKINCallAssessmentOutcomeID], [INImportCallMonitoring].[ExclusionsExplained], [INImportCallMonitoring].[ExclusionsExplainedBumpUpClosure], [INImportCallMonitoring].[IsRecoveredSale], [INImportCallMonitoring].[WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [INImportCallMonitoring].[IsCallMonitored], [INImportCallMonitoring].[WasClearYesGivenInSalesQuestion], [INImportCallMonitoring].[WasPermissionQuestionAsked], [INImportCallMonitoring].[WasNextOfKinQuestionAsked], [INImportCallMonitoring].[FKTertiaryCallMonitoringUserID], [INImportCallMonitoring].[IsTSRBUSavedCarriedForward], [INImportCallMonitoring].[TSRBUSavedCarriedForwardDate], [INImportCallMonitoring].[TSRBUSavedCarriedForwardAssignedByUserID], [INImportCallMonitoring].[CallMonitoredDate], [INImportCallMonitoring].[Objections], [INImportCallMonitoring].[StampDate], [INImportCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportCallMonitoring] ");
                query.Append(" WHERE [INImportCallMonitoring].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportcallmonitoring object from history.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportcallmonitoring object from history.</returns>
        internal static string FillHistory(INImportCallMonitoring inimportcallmonitoring, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportcallmonitoring != null)
            {
                query = "SELECT [ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasDebiCheckProcessExplainedCorrectly], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [StampDate], [StampUserID] FROM [zHstINImportCallMonitoring] WHERE [zHstINImportCallMonitoring].[ID] = @ID AND [zHstINImportCallMonitoring].[StampUserID] = @StampUserID AND [zHstINImportCallMonitoring].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportcallmonitorings in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportcallmonitorings in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportCallMonitoring].[ID], [INImportCallMonitoring].[FKINImportID], [INImportCallMonitoring].[IsBankingDetailsCapturedCorrectly], [INImportCallMonitoring].[WasAccountVerified], [INImportCallMonitoring].[WasDebitDateConfirmed], [INImportCallMonitoring].[IsAccountInClientsName], [INImportCallMonitoring].[DoesClientHaveSigningPower], [INImportCallMonitoring].[WasDebiCheckProcessExplainedCorrectly], [INImportCallMonitoring].[WasCorrectClosureQuestionAsked], [INImportCallMonitoring].[WasResponseClearAndPositive], [INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPs], [INImportCallMonitoring].[WasDebitAmountMentionedCorrectly], [INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectly], [INImportCallMonitoring].[WasCorrectCoverCommencementDateMentioned], [INImportCallMonitoring].[WasNonPaymentProcedureExplained], [INImportCallMonitoring].[WasAnnualIncreaseExplained], [INImportCallMonitoring].[WasCorrectQuestionAskedBumpUpClosure], [INImportCallMonitoring].[WasResponseClearAndPositiveBumpUpClosure], [INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPsBumpUpClosure], [INImportCallMonitoring].[WasDebitAmountMentionedCorrectlyBumpUpClosure], [INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [INImportCallMonitoring].[WasCorrectCoverCommencementDateMentionedBumpUpClosure], [INImportCallMonitoring].[WasNonPaymentProcedureExplainedBumpUpClosure], [INImportCallMonitoring].[WasAnnualIncreaseExplainedBumpUpClosure], [INImportCallMonitoring].[FKINCallMonitoringOutcomeID], [INImportCallMonitoring].[Comments], [INImportCallMonitoring].[FKCallMonitoringUserID], [INImportCallMonitoring].[WasCallEvaluatedBySecondaryUser], [INImportCallMonitoring].[FKSecondaryCallMonitoringUserID], [INImportCallMonitoring].[FKINCallAssessmentOutcomeID], [INImportCallMonitoring].[ExclusionsExplained], [INImportCallMonitoring].[ExclusionsExplainedBumpUpClosure], [INImportCallMonitoring].[IsRecoveredSale], [INImportCallMonitoring].[WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [INImportCallMonitoring].[IsCallMonitored], [INImportCallMonitoring].[WasClearYesGivenInSalesQuestion], [INImportCallMonitoring].[WasPermissionQuestionAsked], [INImportCallMonitoring].[WasNextOfKinQuestionAsked], [INImportCallMonitoring].[FKTertiaryCallMonitoringUserID], [INImportCallMonitoring].[IsTSRBUSavedCarriedForward], [INImportCallMonitoring].[TSRBUSavedCarriedForwardDate], [INImportCallMonitoring].[TSRBUSavedCarriedForwardAssignedByUserID], [INImportCallMonitoring].[CallMonitoredDate], [INImportCallMonitoring].[Objections], [INImportCallMonitoring].[StampDate], [INImportCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportCallMonitoring] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportcallmonitorings in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportcallmonitorings in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportCallMonitoring].[ID], [zHstINImportCallMonitoring].[FKINImportID], [zHstINImportCallMonitoring].[IsBankingDetailsCapturedCorrectly], [zHstINImportCallMonitoring].[WasAccountVerified], [zHstINImportCallMonitoring].[WasDebitDateConfirmed], [zHstINImportCallMonitoring].[IsAccountInClientsName], [zHstINImportCallMonitoring].[DoesClientHaveSigningPower], [zHstINImportCallMonitoring].[WasDebiCheckProcessExplainedCorrectly], [zHstINImportCallMonitoring].[WasCorrectClosureQuestionAsked], [zHstINImportCallMonitoring].[WasResponseClearAndPositive], [zHstINImportCallMonitoring].[WasUDMAndPLMentionedAsFSPs], [zHstINImportCallMonitoring].[WasDebitAmountMentionedCorrectly], [zHstINImportCallMonitoring].[WasFirstDebitDateExplainedCorrectly], [zHstINImportCallMonitoring].[WasCorrectCoverCommencementDateMentioned], [zHstINImportCallMonitoring].[WasNonPaymentProcedureExplained], [zHstINImportCallMonitoring].[WasAnnualIncreaseExplained], [zHstINImportCallMonitoring].[WasCorrectQuestionAskedBumpUpClosure], [zHstINImportCallMonitoring].[WasResponseClearAndPositiveBumpUpClosure], [zHstINImportCallMonitoring].[WasUDMAndPLMentionedAsFSPsBumpUpClosure], [zHstINImportCallMonitoring].[WasDebitAmountMentionedCorrectlyBumpUpClosure], [zHstINImportCallMonitoring].[WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [zHstINImportCallMonitoring].[WasCorrectCoverCommencementDateMentionedBumpUpClosure], [zHstINImportCallMonitoring].[WasNonPaymentProcedureExplainedBumpUpClosure], [zHstINImportCallMonitoring].[WasAnnualIncreaseExplainedBumpUpClosure], [zHstINImportCallMonitoring].[FKINCallMonitoringOutcomeID], [zHstINImportCallMonitoring].[Comments], [zHstINImportCallMonitoring].[FKCallMonitoringUserID], [zHstINImportCallMonitoring].[WasCallEvaluatedBySecondaryUser], [zHstINImportCallMonitoring].[FKSecondaryCallMonitoringUserID], [zHstINImportCallMonitoring].[FKINCallAssessmentOutcomeID], [zHstINImportCallMonitoring].[ExclusionsExplained], [zHstINImportCallMonitoring].[ExclusionsExplainedBumpUpClosure], [zHstINImportCallMonitoring].[IsRecoveredSale], [zHstINImportCallMonitoring].[WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [zHstINImportCallMonitoring].[IsCallMonitored], [zHstINImportCallMonitoring].[WasClearYesGivenInSalesQuestion], [zHstINImportCallMonitoring].[WasPermissionQuestionAsked], [zHstINImportCallMonitoring].[WasNextOfKinQuestionAsked], [zHstINImportCallMonitoring].[FKTertiaryCallMonitoringUserID], [zHstINImportCallMonitoring].[IsTSRBUSavedCarriedForward], [zHstINImportCallMonitoring].[TSRBUSavedCarriedForwardDate], [zHstINImportCallMonitoring].[TSRBUSavedCarriedForwardAssignedByUserID], [zHstINImportCallMonitoring].[CallMonitoredDate], [zHstINImportCallMonitoring].[Objections], [zHstINImportCallMonitoring].[StampDate], [zHstINImportCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportCallMonitoring] ");
            query.Append("INNER JOIN (SELECT [zHstINImportCallMonitoring].[ID], MAX([zHstINImportCallMonitoring].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportCallMonitoring] ");
            query.Append("WHERE [zHstINImportCallMonitoring].[ID] NOT IN (SELECT [INImportCallMonitoring].[ID] FROM [INImportCallMonitoring]) ");
            query.Append("GROUP BY [zHstINImportCallMonitoring].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportCallMonitoring].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportCallMonitoring].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportcallmonitoring in the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportcallmonitoring in the database.</returns>
        public static string ListHistory(INImportCallMonitoring inimportcallmonitoring, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportcallmonitoring != null)
            {
            query.Append("SELECT [zHstINImportCallMonitoring].[ID], [zHstINImportCallMonitoring].[FKINImportID], [zHstINImportCallMonitoring].[IsBankingDetailsCapturedCorrectly], [zHstINImportCallMonitoring].[WasAccountVerified], [zHstINImportCallMonitoring].[WasDebitDateConfirmed], [zHstINImportCallMonitoring].[IsAccountInClientsName], [zHstINImportCallMonitoring].[DoesClientHaveSigningPower], [zHstINImportCallMonitoring].[WasDebiCheckProcessExplainedCorrectly] [zHstINImportCallMonitoring].[WasCorrectClosureQuestionAsked], [zHstINImportCallMonitoring].[WasResponseClearAndPositive], [zHstINImportCallMonitoring].[WasUDMAndPLMentionedAsFSPs], [zHstINImportCallMonitoring].[WasDebitAmountMentionedCorrectly], [zHstINImportCallMonitoring].[WasFirstDebitDateExplainedCorrectly], [zHstINImportCallMonitoring].[WasCorrectCoverCommencementDateMentioned], [zHstINImportCallMonitoring].[WasNonPaymentProcedureExplained], [zHstINImportCallMonitoring].[WasAnnualIncreaseExplained], [zHstINImportCallMonitoring].[WasCorrectQuestionAskedBumpUpClosure], [zHstINImportCallMonitoring].[WasResponseClearAndPositiveBumpUpClosure], [zHstINImportCallMonitoring].[WasUDMAndPLMentionedAsFSPsBumpUpClosure], [zHstINImportCallMonitoring].[WasDebitAmountMentionedCorrectlyBumpUpClosure], [zHstINImportCallMonitoring].[WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [zHstINImportCallMonitoring].[WasCorrectCoverCommencementDateMentionedBumpUpClosure], [zHstINImportCallMonitoring].[WasNonPaymentProcedureExplainedBumpUpClosure], [zHstINImportCallMonitoring].[WasAnnualIncreaseExplainedBumpUpClosure], [zHstINImportCallMonitoring].[FKINCallMonitoringOutcomeID], [zHstINImportCallMonitoring].[Comments], [zHstINImportCallMonitoring].[FKCallMonitoringUserID], [zHstINImportCallMonitoring].[WasCallEvaluatedBySecondaryUser], [zHstINImportCallMonitoring].[FKSecondaryCallMonitoringUserID], [zHstINImportCallMonitoring].[FKINCallAssessmentOutcomeID], [zHstINImportCallMonitoring].[ExclusionsExplained], [zHstINImportCallMonitoring].[ExclusionsExplainedBumpUpClosure], [zHstINImportCallMonitoring].[IsRecoveredSale], [zHstINImportCallMonitoring].[WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [zHstINImportCallMonitoring].[IsCallMonitored], [zHstINImportCallMonitoring].[WasClearYesGivenInSalesQuestion], [zHstINImportCallMonitoring].[WasPermissionQuestionAsked], [zHstINImportCallMonitoring].[WasNextOfKinQuestionAsked], [zHstINImportCallMonitoring].[FKTertiaryCallMonitoringUserID], [zHstINImportCallMonitoring].[IsTSRBUSavedCarriedForward], [zHstINImportCallMonitoring].[TSRBUSavedCarriedForwardDate], [zHstINImportCallMonitoring].[TSRBUSavedCarriedForwardAssignedByUserID], [zHstINImportCallMonitoring].[CallMonitoredDate], [zHstINImportCallMonitoring].[Objections], [zHstINImportCallMonitoring].[StampDate], [zHstINImportCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportCallMonitoring] ");
                query.Append(" WHERE [zHstINImportCallMonitoring].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportCallMonitoring].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportcallmonitoring to the database.
        /// </summary>
        /// <param name="inimportcallmonitoring">The inimportcallmonitoring to save.</param>
        /// <returns>A query that can be used to save the inimportcallmonitoring to the database.</returns>
        internal static string Save(INImportCallMonitoring inimportcallmonitoring, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportcallmonitoring != null)
            {
                if (inimportcallmonitoring.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportCallMonitoring] ([ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [WasDebiCheckProcessExplainedCorrectly], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [Objections], [WasDebiCheckProcessExplainedCorrectly], [StampDate], [StampUserID] FROM [zHstINImportCallMonitoring] WHERE [zHstINImportCallMonitoring].[ID] = @ID; ");
                    query.Append("UPDATE [INImportCallMonitoring]");
                    parameters = new object[44];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportcallmonitoring.FKINImportID.HasValue ? (object)inimportcallmonitoring.FKINImportID.Value : DBNull.Value);
                    query.Append(", [IsBankingDetailsCapturedCorrectly] = @IsBankingDetailsCapturedCorrectly");
                    parameters[1] = Database.GetParameter("@IsBankingDetailsCapturedCorrectly", inimportcallmonitoring.IsBankingDetailsCapturedCorrectly.HasValue ? (object)inimportcallmonitoring.IsBankingDetailsCapturedCorrectly.Value : DBNull.Value);
                    query.Append(", [WasAccountVerified] = @WasAccountVerified");
                    parameters[2] = Database.GetParameter("@WasAccountVerified", inimportcallmonitoring.WasAccountVerified.HasValue ? (object)inimportcallmonitoring.WasAccountVerified.Value : DBNull.Value);
                    query.Append(", [WasDebitDateConfirmed] = @WasDebitDateConfirmed");
                    parameters[3] = Database.GetParameter("@WasDebitDateConfirmed", inimportcallmonitoring.WasDebitDateConfirmed.HasValue ? (object)inimportcallmonitoring.WasDebitDateConfirmed.Value : DBNull.Value);
                    query.Append(", [IsAccountInClientsName] = @IsAccountInClientsName");
                    parameters[4] = Database.GetParameter("@IsAccountInClientsName", inimportcallmonitoring.IsAccountInClientsName.HasValue ? (object)inimportcallmonitoring.IsAccountInClientsName.Value : DBNull.Value);
                    query.Append(", [DoesClientHaveSigningPower] = @DoesClientHaveSigningPower");
                    parameters[5] = Database.GetParameter("@DoesClientHaveSigningPower", inimportcallmonitoring.DoesClientHaveSigningPower.HasValue ? (object)inimportcallmonitoring.DoesClientHaveSigningPower.Value : DBNull.Value);                   
                    query.Append(", [WasCorrectClosureQuestionAsked] = @WasCorrectClosureQuestionAsked");
                    parameters[6] = Database.GetParameter("@WasCorrectClosureQuestionAsked", inimportcallmonitoring.WasCorrectClosureQuestionAsked.HasValue ? (object)inimportcallmonitoring.WasCorrectClosureQuestionAsked.Value : DBNull.Value);
                    query.Append(", [WasResponseClearAndPositive] = @WasResponseClearAndPositive");
                    parameters[7] = Database.GetParameter("@WasResponseClearAndPositive", inimportcallmonitoring.WasResponseClearAndPositive.HasValue ? (object)inimportcallmonitoring.WasResponseClearAndPositive.Value : DBNull.Value);
                    query.Append(", [WasUDMAndPLMentionedAsFSPs] = @WasUDMAndPLMentionedAsFSPs");
                    parameters[8] = Database.GetParameter("@WasUDMAndPLMentionedAsFSPs", inimportcallmonitoring.WasUDMAndPLMentionedAsFSPs.HasValue ? (object)inimportcallmonitoring.WasUDMAndPLMentionedAsFSPs.Value : DBNull.Value);
                    query.Append(", [WasDebitAmountMentionedCorrectly] = @WasDebitAmountMentionedCorrectly");
                    parameters[9] = Database.GetParameter("@WasDebitAmountMentionedCorrectly", inimportcallmonitoring.WasDebitAmountMentionedCorrectly.HasValue ? (object)inimportcallmonitoring.WasDebitAmountMentionedCorrectly.Value : DBNull.Value);
                    query.Append(", [WasFirstDebitDateExplainedCorrectly] = @WasFirstDebitDateExplainedCorrectly");
                    parameters[10] = Database.GetParameter("@WasFirstDebitDateExplainedCorrectly", inimportcallmonitoring.WasFirstDebitDateExplainedCorrectly.HasValue ? (object)inimportcallmonitoring.WasFirstDebitDateExplainedCorrectly.Value : DBNull.Value);
                    query.Append(", [WasCorrectCoverCommencementDateMentioned] = @WasCorrectCoverCommencementDateMentioned");
                    parameters[11] = Database.GetParameter("@WasCorrectCoverCommencementDateMentioned", inimportcallmonitoring.WasCorrectCoverCommencementDateMentioned.HasValue ? (object)inimportcallmonitoring.WasCorrectCoverCommencementDateMentioned.Value : DBNull.Value);
                    query.Append(", [WasNonPaymentProcedureExplained] = @WasNonPaymentProcedureExplained");
                    parameters[12] = Database.GetParameter("@WasNonPaymentProcedureExplained", inimportcallmonitoring.WasNonPaymentProcedureExplained.HasValue ? (object)inimportcallmonitoring.WasNonPaymentProcedureExplained.Value : DBNull.Value);
                    query.Append(", [WasAnnualIncreaseExplained] = @WasAnnualIncreaseExplained");
                    parameters[13] = Database.GetParameter("@WasAnnualIncreaseExplained", inimportcallmonitoring.WasAnnualIncreaseExplained.HasValue ? (object)inimportcallmonitoring.WasAnnualIncreaseExplained.Value : DBNull.Value);
                    query.Append(", [WasCorrectQuestionAskedBumpUpClosure] = @WasCorrectQuestionAskedBumpUpClosure");
                    parameters[14] = Database.GetParameter("@WasCorrectQuestionAskedBumpUpClosure", inimportcallmonitoring.WasCorrectQuestionAskedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasCorrectQuestionAskedBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [WasResponseClearAndPositiveBumpUpClosure] = @WasResponseClearAndPositiveBumpUpClosure");
                    parameters[15] = Database.GetParameter("@WasResponseClearAndPositiveBumpUpClosure", inimportcallmonitoring.WasResponseClearAndPositiveBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasResponseClearAndPositiveBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [WasUDMAndPLMentionedAsFSPsBumpUpClosure] = @WasUDMAndPLMentionedAsFSPsBumpUpClosure");
                    parameters[16] = Database.GetParameter("@WasUDMAndPLMentionedAsFSPsBumpUpClosure", inimportcallmonitoring.WasUDMAndPLMentionedAsFSPsBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasUDMAndPLMentionedAsFSPsBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [WasDebitAmountMentionedCorrectlyBumpUpClosure] = @WasDebitAmountMentionedCorrectlyBumpUpClosure");
                    parameters[17] = Database.GetParameter("@WasDebitAmountMentionedCorrectlyBumpUpClosure", inimportcallmonitoring.WasDebitAmountMentionedCorrectlyBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasDebitAmountMentionedCorrectlyBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [WasFirstDebitDateExplainedCorrectlyBumpUpClosure] = @WasFirstDebitDateExplainedCorrectlyBumpUpClosure");
                    parameters[18] = Database.GetParameter("@WasFirstDebitDateExplainedCorrectlyBumpUpClosure", inimportcallmonitoring.WasFirstDebitDateExplainedCorrectlyBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasFirstDebitDateExplainedCorrectlyBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [WasCorrectCoverCommencementDateMentionedBumpUpClosure] = @WasCorrectCoverCommencementDateMentionedBumpUpClosure");
                    parameters[19] = Database.GetParameter("@WasCorrectCoverCommencementDateMentionedBumpUpClosure", inimportcallmonitoring.WasCorrectCoverCommencementDateMentionedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasCorrectCoverCommencementDateMentionedBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [WasNonPaymentProcedureExplainedBumpUpClosure] = @WasNonPaymentProcedureExplainedBumpUpClosure");
                    parameters[20] = Database.GetParameter("@WasNonPaymentProcedureExplainedBumpUpClosure", inimportcallmonitoring.WasNonPaymentProcedureExplainedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasNonPaymentProcedureExplainedBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [WasAnnualIncreaseExplainedBumpUpClosure] = @WasAnnualIncreaseExplainedBumpUpClosure");
                    parameters[21] = Database.GetParameter("@WasAnnualIncreaseExplainedBumpUpClosure", inimportcallmonitoring.WasAnnualIncreaseExplainedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasAnnualIncreaseExplainedBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [FKINCallMonitoringOutcomeID] = @FKINCallMonitoringOutcomeID");
                    parameters[22] = Database.GetParameter("@FKINCallMonitoringOutcomeID", inimportcallmonitoring.FKINCallMonitoringOutcomeID.HasValue ? (object)inimportcallmonitoring.FKINCallMonitoringOutcomeID.Value : DBNull.Value);
                    query.Append(", [Comments] = @Comments");
                    parameters[23] = Database.GetParameter("@Comments", string.IsNullOrEmpty(inimportcallmonitoring.Comments) ? DBNull.Value : (object)inimportcallmonitoring.Comments);
                    query.Append(", [FKCallMonitoringUserID] = @FKCallMonitoringUserID");
                    parameters[24] = Database.GetParameter("@FKCallMonitoringUserID", inimportcallmonitoring.FKCallMonitoringUserID.HasValue ? (object)inimportcallmonitoring.FKCallMonitoringUserID.Value : DBNull.Value);
                    query.Append(", [WasCallEvaluatedBySecondaryUser] = @WasCallEvaluatedBySecondaryUser");
                    parameters[25] = Database.GetParameter("@WasCallEvaluatedBySecondaryUser", inimportcallmonitoring.WasCallEvaluatedBySecondaryUser.HasValue ? (object)inimportcallmonitoring.WasCallEvaluatedBySecondaryUser.Value : DBNull.Value);
                    query.Append(", [FKSecondaryCallMonitoringUserID] = @FKSecondaryCallMonitoringUserID");
                    parameters[26] = Database.GetParameter("@FKSecondaryCallMonitoringUserID", inimportcallmonitoring.FKSecondaryCallMonitoringUserID.HasValue ? (object)inimportcallmonitoring.FKSecondaryCallMonitoringUserID.Value : DBNull.Value);
                    query.Append(", [FKINCallAssessmentOutcomeID] = @FKINCallAssessmentOutcomeID");
                    parameters[27] = Database.GetParameter("@FKINCallAssessmentOutcomeID", inimportcallmonitoring.FKINCallAssessmentOutcomeID.HasValue ? (object)inimportcallmonitoring.FKINCallAssessmentOutcomeID.Value : DBNull.Value);
                    query.Append(", [ExclusionsExplained] = @ExclusionsExplained");
                    parameters[28] = Database.GetParameter("@ExclusionsExplained", inimportcallmonitoring.ExclusionsExplained.HasValue ? (object)inimportcallmonitoring.ExclusionsExplained.Value : DBNull.Value);
                    query.Append(", [ExclusionsExplainedBumpUpClosure] = @ExclusionsExplainedBumpUpClosure");
                    parameters[29] = Database.GetParameter("@ExclusionsExplainedBumpUpClosure", inimportcallmonitoring.ExclusionsExplainedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.ExclusionsExplainedBumpUpClosure.Value : DBNull.Value);
                    query.Append(", [IsRecoveredSale] = @IsRecoveredSale");
                    parameters[30] = Database.GetParameter("@IsRecoveredSale", inimportcallmonitoring.IsRecoveredSale.HasValue ? (object)inimportcallmonitoring.IsRecoveredSale.Value : DBNull.Value);
                    query.Append(", [WasMoneyBackVsVoucherBenefitsExplainedCorrectly] = @WasMoneyBackVsVoucherBenefitsExplainedCorrectly");
                    parameters[31] = Database.GetParameter("@WasMoneyBackVsVoucherBenefitsExplainedCorrectly", inimportcallmonitoring.WasMoneyBackVsVoucherBenefitsExplainedCorrectly.HasValue ? (object)inimportcallmonitoring.WasMoneyBackVsVoucherBenefitsExplainedCorrectly.Value : DBNull.Value);
                    query.Append(", [IsCallMonitored] = @IsCallMonitored");
                    parameters[32] = Database.GetParameter("@IsCallMonitored", inimportcallmonitoring.IsCallMonitored.HasValue ? (object)inimportcallmonitoring.IsCallMonitored.Value : DBNull.Value);
                    query.Append(", [WasClearYesGivenInSalesQuestion] = @WasClearYesGivenInSalesQuestion");
                    parameters[33] = Database.GetParameter("@WasClearYesGivenInSalesQuestion", inimportcallmonitoring.WasClearYesGivenInSalesQuestion.HasValue ? (object)inimportcallmonitoring.WasClearYesGivenInSalesQuestion.Value : DBNull.Value);
                    query.Append(", [WasPermissionQuestionAsked] = @WasPermissionQuestionAsked");
                    parameters[34] = Database.GetParameter("@WasPermissionQuestionAsked", inimportcallmonitoring.WasPermissionQuestionAsked.HasValue ? (object)inimportcallmonitoring.WasPermissionQuestionAsked.Value : DBNull.Value);
                    query.Append(", [WasNextOfKinQuestionAsked] = @WasNextOfKinQuestionAsked");
                    parameters[35] = Database.GetParameter("@WasNextOfKinQuestionAsked", inimportcallmonitoring.WasNextOfKinQuestionAsked.HasValue ? (object)inimportcallmonitoring.WasNextOfKinQuestionAsked.Value : DBNull.Value);
                    query.Append(", [FKTertiaryCallMonitoringUserID] = @FKTertiaryCallMonitoringUserID");
                    parameters[36] = Database.GetParameter("@FKTertiaryCallMonitoringUserID", inimportcallmonitoring.FKTertiaryCallMonitoringUserID.HasValue ? (object)inimportcallmonitoring.FKTertiaryCallMonitoringUserID.Value : DBNull.Value);
                    query.Append(", [IsTSRBUSavedCarriedForward] = @IsTSRBUSavedCarriedForward");
                    parameters[37] = Database.GetParameter("@IsTSRBUSavedCarriedForward", inimportcallmonitoring.IsTSRBUSavedCarriedForward.HasValue ? (object)inimportcallmonitoring.IsTSRBUSavedCarriedForward.Value : DBNull.Value);
                    query.Append(", [TSRBUSavedCarriedForwardDate] = @TSRBUSavedCarriedForwardDate");
                    parameters[38] = Database.GetParameter("@TSRBUSavedCarriedForwardDate", inimportcallmonitoring.TSRBUSavedCarriedForwardDate.HasValue ? (object)inimportcallmonitoring.TSRBUSavedCarriedForwardDate.Value : DBNull.Value);
                    query.Append(", [TSRBUSavedCarriedForwardAssignedByUserID] = @TSRBUSavedCarriedForwardAssignedByUserID");
                    parameters[39] = Database.GetParameter("@TSRBUSavedCarriedForwardAssignedByUserID", inimportcallmonitoring.TSRBUSavedCarriedForwardAssignedByUserID.HasValue ? (object)inimportcallmonitoring.TSRBUSavedCarriedForwardAssignedByUserID.Value : DBNull.Value);
                    query.Append(", [CallMonitoredDate] = @CallMonitoredDate");
                    parameters[40] = Database.GetParameter("@CallMonitoredDate", inimportcallmonitoring.CallMonitoredDate.HasValue ? (object)inimportcallmonitoring.CallMonitoredDate.Value : DBNull.Value);
                    query.Append(", [WasDebiCheckProcessExplainedCorrectly] = @WasDebiCheckProcessExplainedCorrectly");
                    parameters[41] = Database.GetParameter("@WasDebiCheckProcessExplainedCorrectly", inimportcallmonitoring.WasDebiCheckProcessExplainedCorrectly.HasValue ? (object)inimportcallmonitoring.WasDebiCheckProcessExplainedCorrectly.Value : DBNull.Value);
                    query.Append(", [Objections] = @Objections");
                    parameters[42] = Database.GetParameter("@Objections", inimportcallmonitoring.Objections.HasValue ? (object)inimportcallmonitoring.Objections.Value : DBNull.Value);

                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append("WHERE [INImportCallMonitoring].[ID] = @ID"); 
                    parameters[43] = Database.GetParameter("@ID", inimportcallmonitoring.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportCallMonitoring] ([FKINImportID], [IsBankingDetailsCapturedCorrectly], [WasAccountVerified], [WasDebitDateConfirmed], [IsAccountInClientsName], [DoesClientHaveSigningPower], [WasCorrectClosureQuestionAsked], [WasResponseClearAndPositive], [WasUDMAndPLMentionedAsFSPs], [WasDebitAmountMentionedCorrectly], [WasFirstDebitDateExplainedCorrectly], [WasCorrectCoverCommencementDateMentioned], [WasNonPaymentProcedureExplained], [WasAnnualIncreaseExplained], [WasCorrectQuestionAskedBumpUpClosure], [WasResponseClearAndPositiveBumpUpClosure], [WasUDMAndPLMentionedAsFSPsBumpUpClosure], [WasDebitAmountMentionedCorrectlyBumpUpClosure], [WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [WasCorrectCoverCommencementDateMentionedBumpUpClosure], [WasNonPaymentProcedureExplainedBumpUpClosure], [WasAnnualIncreaseExplainedBumpUpClosure], [FKINCallMonitoringOutcomeID], [Comments], [FKCallMonitoringUserID], [WasCallEvaluatedBySecondaryUser], [FKSecondaryCallMonitoringUserID], [FKINCallAssessmentOutcomeID], [ExclusionsExplained], [ExclusionsExplainedBumpUpClosure], [IsRecoveredSale], [WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [IsCallMonitored], [WasClearYesGivenInSalesQuestion], [WasPermissionQuestionAsked], [WasNextOfKinQuestionAsked], [FKTertiaryCallMonitoringUserID], [IsTSRBUSavedCarriedForward], [TSRBUSavedCarriedForwardDate], [TSRBUSavedCarriedForwardAssignedByUserID], [CallMonitoredDate], [WasDebiCheckProcessExplainedCorrectly], [StampDate], [StampUserID]) VALUES(@FKINImportID, @IsBankingDetailsCapturedCorrectly, @WasAccountVerified, @WasDebitDateConfirmed, @IsAccountInClientsName, @DoesClientHaveSigningPower, @WasCorrectClosureQuestionAsked, @WasResponseClearAndPositive, @WasUDMAndPLMentionedAsFSPs, @WasDebitAmountMentionedCorrectly, @WasFirstDebitDateExplainedCorrectly, @WasCorrectCoverCommencementDateMentioned, @WasNonPaymentProcedureExplained, @WasAnnualIncreaseExplained, @WasCorrectQuestionAskedBumpUpClosure, @WasResponseClearAndPositiveBumpUpClosure, @WasUDMAndPLMentionedAsFSPsBumpUpClosure, @WasDebitAmountMentionedCorrectlyBumpUpClosure, @WasFirstDebitDateExplainedCorrectlyBumpUpClosure, @WasCorrectCoverCommencementDateMentionedBumpUpClosure, @WasNonPaymentProcedureExplainedBumpUpClosure, @WasAnnualIncreaseExplainedBumpUpClosure, @FKINCallMonitoringOutcomeID, @Comments, @FKCallMonitoringUserID, @WasCallEvaluatedBySecondaryUser, @FKSecondaryCallMonitoringUserID, @FKINCallAssessmentOutcomeID, @ExclusionsExplained, @ExclusionsExplainedBumpUpClosure, @IsRecoveredSale, @WasMoneyBackVsVoucherBenefitsExplainedCorrectly, @IsCallMonitored, @WasClearYesGivenInSalesQuestion, @WasPermissionQuestionAsked, @WasNextOfKinQuestionAsked, @FKTertiaryCallMonitoringUserID, @IsTSRBUSavedCarriedForward, @TSRBUSavedCarriedForwardDate, @TSRBUSavedCarriedForwardAssignedByUserID, @CallMonitoredDate, @WasDebiCheckProcessExplainedCorrectly, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[43];
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportcallmonitoring.FKINImportID.HasValue ? (object)inimportcallmonitoring.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@IsBankingDetailsCapturedCorrectly", inimportcallmonitoring.IsBankingDetailsCapturedCorrectly.HasValue ? (object)inimportcallmonitoring.IsBankingDetailsCapturedCorrectly.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@WasAccountVerified", inimportcallmonitoring.WasAccountVerified.HasValue ? (object)inimportcallmonitoring.WasAccountVerified.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@WasDebitDateConfirmed", inimportcallmonitoring.WasDebitDateConfirmed.HasValue ? (object)inimportcallmonitoring.WasDebitDateConfirmed.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@IsAccountInClientsName", inimportcallmonitoring.IsAccountInClientsName.HasValue ? (object)inimportcallmonitoring.IsAccountInClientsName.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@DoesClientHaveSigningPower", inimportcallmonitoring.DoesClientHaveSigningPower.HasValue ? (object)inimportcallmonitoring.DoesClientHaveSigningPower.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@WasCorrectClosureQuestionAsked", inimportcallmonitoring.WasCorrectClosureQuestionAsked.HasValue ? (object)inimportcallmonitoring.WasCorrectClosureQuestionAsked.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@WasResponseClearAndPositive", inimportcallmonitoring.WasResponseClearAndPositive.HasValue ? (object)inimportcallmonitoring.WasResponseClearAndPositive.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@WasUDMAndPLMentionedAsFSPs", inimportcallmonitoring.WasUDMAndPLMentionedAsFSPs.HasValue ? (object)inimportcallmonitoring.WasUDMAndPLMentionedAsFSPs.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@WasDebitAmountMentionedCorrectly", inimportcallmonitoring.WasDebitAmountMentionedCorrectly.HasValue ? (object)inimportcallmonitoring.WasDebitAmountMentionedCorrectly.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@WasFirstDebitDateExplainedCorrectly", inimportcallmonitoring.WasFirstDebitDateExplainedCorrectly.HasValue ? (object)inimportcallmonitoring.WasFirstDebitDateExplainedCorrectly.Value : DBNull.Value);
                    parameters[11] = Database.GetParameter("@WasCorrectCoverCommencementDateMentioned", inimportcallmonitoring.WasCorrectCoverCommencementDateMentioned.HasValue ? (object)inimportcallmonitoring.WasCorrectCoverCommencementDateMentioned.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@WasNonPaymentProcedureExplained", inimportcallmonitoring.WasNonPaymentProcedureExplained.HasValue ? (object)inimportcallmonitoring.WasNonPaymentProcedureExplained.Value : DBNull.Value);
                    parameters[13] = Database.GetParameter("@WasAnnualIncreaseExplained", inimportcallmonitoring.WasAnnualIncreaseExplained.HasValue ? (object)inimportcallmonitoring.WasAnnualIncreaseExplained.Value : DBNull.Value);
                    parameters[14] = Database.GetParameter("@WasCorrectQuestionAskedBumpUpClosure", inimportcallmonitoring.WasCorrectQuestionAskedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasCorrectQuestionAskedBumpUpClosure.Value : DBNull.Value);
                    parameters[15] = Database.GetParameter("@WasResponseClearAndPositiveBumpUpClosure", inimportcallmonitoring.WasResponseClearAndPositiveBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasResponseClearAndPositiveBumpUpClosure.Value : DBNull.Value);
                    parameters[16] = Database.GetParameter("@WasUDMAndPLMentionedAsFSPsBumpUpClosure", inimportcallmonitoring.WasUDMAndPLMentionedAsFSPsBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasUDMAndPLMentionedAsFSPsBumpUpClosure.Value : DBNull.Value);
                    parameters[17] = Database.GetParameter("@WasDebitAmountMentionedCorrectlyBumpUpClosure", inimportcallmonitoring.WasDebitAmountMentionedCorrectlyBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasDebitAmountMentionedCorrectlyBumpUpClosure.Value : DBNull.Value);
                    parameters[18] = Database.GetParameter("@WasFirstDebitDateExplainedCorrectlyBumpUpClosure", inimportcallmonitoring.WasFirstDebitDateExplainedCorrectlyBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasFirstDebitDateExplainedCorrectlyBumpUpClosure.Value : DBNull.Value);
                    parameters[19] = Database.GetParameter("@WasCorrectCoverCommencementDateMentionedBumpUpClosure", inimportcallmonitoring.WasCorrectCoverCommencementDateMentionedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasCorrectCoverCommencementDateMentionedBumpUpClosure.Value : DBNull.Value);
                    parameters[20] = Database.GetParameter("@WasNonPaymentProcedureExplainedBumpUpClosure", inimportcallmonitoring.WasNonPaymentProcedureExplainedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasNonPaymentProcedureExplainedBumpUpClosure.Value : DBNull.Value);
                    parameters[21] = Database.GetParameter("@WasAnnualIncreaseExplainedBumpUpClosure", inimportcallmonitoring.WasAnnualIncreaseExplainedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.WasAnnualIncreaseExplainedBumpUpClosure.Value : DBNull.Value);
                    parameters[22] = Database.GetParameter("@FKINCallMonitoringOutcomeID", inimportcallmonitoring.FKINCallMonitoringOutcomeID.HasValue ? (object)inimportcallmonitoring.FKINCallMonitoringOutcomeID.Value : DBNull.Value);
                    parameters[23] = Database.GetParameter("@Comments", string.IsNullOrEmpty(inimportcallmonitoring.Comments) ? DBNull.Value : (object)inimportcallmonitoring.Comments);
                    parameters[24] = Database.GetParameter("@FKCallMonitoringUserID", inimportcallmonitoring.FKCallMonitoringUserID.HasValue ? (object)inimportcallmonitoring.FKCallMonitoringUserID.Value : DBNull.Value);
                    parameters[25] = Database.GetParameter("@WasCallEvaluatedBySecondaryUser", inimportcallmonitoring.WasCallEvaluatedBySecondaryUser.HasValue ? (object)inimportcallmonitoring.WasCallEvaluatedBySecondaryUser.Value : DBNull.Value);
                    parameters[26] = Database.GetParameter("@FKSecondaryCallMonitoringUserID", inimportcallmonitoring.FKSecondaryCallMonitoringUserID.HasValue ? (object)inimportcallmonitoring.FKSecondaryCallMonitoringUserID.Value : DBNull.Value);
                    parameters[27] = Database.GetParameter("@FKINCallAssessmentOutcomeID", inimportcallmonitoring.FKINCallAssessmentOutcomeID.HasValue ? (object)inimportcallmonitoring.FKINCallAssessmentOutcomeID.Value : DBNull.Value);
                    parameters[28] = Database.GetParameter("@ExclusionsExplained", inimportcallmonitoring.ExclusionsExplained.HasValue ? (object)inimportcallmonitoring.ExclusionsExplained.Value : DBNull.Value);
                    parameters[29] = Database.GetParameter("@ExclusionsExplainedBumpUpClosure", inimportcallmonitoring.ExclusionsExplainedBumpUpClosure.HasValue ? (object)inimportcallmonitoring.ExclusionsExplainedBumpUpClosure.Value : DBNull.Value);
                    parameters[30] = Database.GetParameter("@IsRecoveredSale", inimportcallmonitoring.IsRecoveredSale.HasValue ? (object)inimportcallmonitoring.IsRecoveredSale.Value : DBNull.Value);
                    parameters[31] = Database.GetParameter("@WasMoneyBackVsVoucherBenefitsExplainedCorrectly", inimportcallmonitoring.WasMoneyBackVsVoucherBenefitsExplainedCorrectly.HasValue ? (object)inimportcallmonitoring.WasMoneyBackVsVoucherBenefitsExplainedCorrectly.Value : DBNull.Value);
                    parameters[32] = Database.GetParameter("@IsCallMonitored", inimportcallmonitoring.IsCallMonitored.HasValue ? (object)inimportcallmonitoring.IsCallMonitored.Value : DBNull.Value);
                    parameters[33] = Database.GetParameter("@WasClearYesGivenInSalesQuestion", inimportcallmonitoring.WasClearYesGivenInSalesQuestion.HasValue ? (object)inimportcallmonitoring.WasClearYesGivenInSalesQuestion.Value : DBNull.Value);
                    parameters[34] = Database.GetParameter("@WasPermissionQuestionAsked", inimportcallmonitoring.WasPermissionQuestionAsked.HasValue ? (object)inimportcallmonitoring.WasPermissionQuestionAsked.Value : DBNull.Value);
                    parameters[35] = Database.GetParameter("@WasNextOfKinQuestionAsked", inimportcallmonitoring.WasNextOfKinQuestionAsked.HasValue ? (object)inimportcallmonitoring.WasNextOfKinQuestionAsked.Value : DBNull.Value);
                    parameters[36] = Database.GetParameter("@FKTertiaryCallMonitoringUserID", inimportcallmonitoring.FKTertiaryCallMonitoringUserID.HasValue ? (object)inimportcallmonitoring.FKTertiaryCallMonitoringUserID.Value : DBNull.Value);
                    parameters[37] = Database.GetParameter("@IsTSRBUSavedCarriedForward", inimportcallmonitoring.IsTSRBUSavedCarriedForward.HasValue ? (object)inimportcallmonitoring.IsTSRBUSavedCarriedForward.Value : DBNull.Value);
                    parameters[38] = Database.GetParameter("@TSRBUSavedCarriedForwardDate", inimportcallmonitoring.TSRBUSavedCarriedForwardDate.HasValue ? (object)inimportcallmonitoring.TSRBUSavedCarriedForwardDate.Value : DBNull.Value);
                    parameters[39] = Database.GetParameter("@TSRBUSavedCarriedForwardAssignedByUserID", inimportcallmonitoring.TSRBUSavedCarriedForwardAssignedByUserID.HasValue ? (object)inimportcallmonitoring.TSRBUSavedCarriedForwardAssignedByUserID.Value : DBNull.Value);
                    parameters[40] = Database.GetParameter("@CallMonitoredDate", inimportcallmonitoring.CallMonitoredDate.HasValue ? (object)inimportcallmonitoring.CallMonitoredDate.Value : DBNull.Value);
                    parameters[41] = Database.GetParameter("@WasDebiCheckProcessExplainedCorrectly", inimportcallmonitoring.WasDebiCheckProcessExplainedCorrectly.HasValue ? (object)inimportcallmonitoring.WasDebiCheckProcessExplainedCorrectly.Value : DBNull.Value);
                    parameters[42] = Database.GetParameter("@Objections", inimportcallmonitoring.Objections.HasValue ? (object)inimportcallmonitoring.Objections.Value : DBNull.Value);

                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportcallmonitorings that match the search criteria.
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
        /// <param name="wasdebicheckprocessexplainedcorrectly">The wasdebicheckprocessexplainedcorrectly search criteria.</param>
        /// <returns>A query that can be used to search for inimportcallmonitorings based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, bool? isbankingdetailscapturedcorrectly, bool? wasaccountverified, bool? wasdebitdateconfirmed, bool? isaccountinclientsname, bool? doesclienthavesigningpower, bool? wasdebicheckprocessexplainedcorrectly, bool? wascorrectclosurequestionasked, bool? wasresponseclearandpositive, bool? wasudmandplmentionedasfsps, bool? wasdebitamountmentionedcorrectly, bool? wasfirstdebitdateexplainedcorrectly, bool? wascorrectcovercommencementdatementioned, bool? wasnonpaymentprocedureexplained, bool? wasannualincreaseexplained, bool? wascorrectquestionaskedbumpupclosure, bool? wasresponseclearandpositivebumpupclosure, bool? wasudmandplmentionedasfspsbumpupclosure, bool? wasdebitamountmentionedcorrectlybumpupclosure, bool? wasfirstdebitdateexplainedcorrectlybumpupclosure, bool? wascorrectcovercommencementdatementionedbumpupclosure, bool? wasnonpaymentprocedureexplainedbumpupclosure, bool? wasannualincreaseexplainedbumpupclosure, long? fkincallmonitoringoutcomeid, string comments, long? fkcallmonitoringuserid, bool? wascallevaluatedbysecondaryuser, long? fksecondarycallmonitoringuserid, long? fkincallassessmentoutcomeid, bool? exclusionsexplained, bool? exclusionsexplainedbumpupclosure, bool? isrecoveredsale, bool? wasmoneybackvsvoucherbenefitsexplainedcorrectly, bool? iscallmonitored, bool? wasclearyesgiveninsalesquestion, bool? waspermissionquestionasked, bool? wasnextofkinquestionasked, long? fktertiarycallmonitoringuserid, bool? istsrbusavedcarriedforward, DateTime? tsrbusavedcarriedforwarddate, long? tsrbusavedcarriedforwardassignedbyuserid, DateTime? callmonitoreddate, bool? objections)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[FKINImportID] = " + fkinimportid + "");
            }
            if (isbankingdetailscapturedcorrectly != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[IsBankingDetailsCapturedCorrectly] = " + ((bool)isbankingdetailscapturedcorrectly ? "1" : "0"));
            }
            if (wasaccountverified != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasAccountVerified] = " + ((bool)wasaccountverified ? "1" : "0"));
            }
            if (wasdebitdateconfirmed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasDebitDateConfirmed] = " + ((bool)wasdebitdateconfirmed ? "1" : "0"));
            }
            if (isaccountinclientsname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[IsAccountInClientsName] = " + ((bool)isaccountinclientsname ? "1" : "0"));
            }
            if (doesclienthavesigningpower != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[DoesClientHaveSigningPower] = " + ((bool)doesclienthavesigningpower ? "1" : "0"));
            }
            if (wasdebicheckprocessexplainedcorrectly != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasDebiCheckProcessExplainedCorrectly] = " + ((bool)wasdebicheckprocessexplainedcorrectly ? "1" : "0"));
            }

            
            if (wascorrectclosurequestionasked != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasCorrectClosureQuestionAsked] = " + ((bool)wascorrectclosurequestionasked ? "1" : "0"));
            }
            if (wasresponseclearandpositive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasResponseClearAndPositive] = " + ((bool)wasresponseclearandpositive ? "1" : "0"));
            }
            if (wasudmandplmentionedasfsps != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPs] = " + ((bool)wasudmandplmentionedasfsps ? "1" : "0"));
            }
            if (wasdebitamountmentionedcorrectly != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasDebitAmountMentionedCorrectly] = " + ((bool)wasdebitamountmentionedcorrectly ? "1" : "0"));
            }
            if (wasfirstdebitdateexplainedcorrectly != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectly] = " + ((bool)wasfirstdebitdateexplainedcorrectly ? "1" : "0"));
            }
            if (wascorrectcovercommencementdatementioned != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasCorrectCoverCommencementDateMentioned] = " + ((bool)wascorrectcovercommencementdatementioned ? "1" : "0"));
            }
            if (wasnonpaymentprocedureexplained != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasNonPaymentProcedureExplained] = " + ((bool)wasnonpaymentprocedureexplained ? "1" : "0"));
            }
            if (wasannualincreaseexplained != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasAnnualIncreaseExplained] = " + ((bool)wasannualincreaseexplained ? "1" : "0"));
            }
            if (wascorrectquestionaskedbumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasCorrectQuestionAskedBumpUpClosure] = " + ((bool)wascorrectquestionaskedbumpupclosure ? "1" : "0"));
            }
            if (wasresponseclearandpositivebumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasResponseClearAndPositiveBumpUpClosure] = " + ((bool)wasresponseclearandpositivebumpupclosure ? "1" : "0"));
            }
            if (wasudmandplmentionedasfspsbumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPsBumpUpClosure] = " + ((bool)wasudmandplmentionedasfspsbumpupclosure ? "1" : "0"));
            }
            if (wasdebitamountmentionedcorrectlybumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasDebitAmountMentionedCorrectlyBumpUpClosure] = " + ((bool)wasdebitamountmentionedcorrectlybumpupclosure ? "1" : "0"));
            }
            if (wasfirstdebitdateexplainedcorrectlybumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectlyBumpUpClosure] = " + ((bool)wasfirstdebitdateexplainedcorrectlybumpupclosure ? "1" : "0"));
            }
            if (wascorrectcovercommencementdatementionedbumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasCorrectCoverCommencementDateMentionedBumpUpClosure] = " + ((bool)wascorrectcovercommencementdatementionedbumpupclosure ? "1" : "0"));
            }
            if (wasnonpaymentprocedureexplainedbumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasNonPaymentProcedureExplainedBumpUpClosure] = " + ((bool)wasnonpaymentprocedureexplainedbumpupclosure ? "1" : "0"));
            }
            if (wasannualincreaseexplainedbumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasAnnualIncreaseExplainedBumpUpClosure] = " + ((bool)wasannualincreaseexplainedbumpupclosure ? "1" : "0"));
            }
            if (fkincallmonitoringoutcomeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[FKINCallMonitoringOutcomeID] = " + fkincallmonitoringoutcomeid + "");
            }
            if (comments != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[Comments] LIKE '" + comments.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkcallmonitoringuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[FKCallMonitoringUserID] = " + fkcallmonitoringuserid + "");
            }
            if (wascallevaluatedbysecondaryuser != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasCallEvaluatedBySecondaryUser] = " + ((bool)wascallevaluatedbysecondaryuser ? "1" : "0"));
            }
            if (fksecondarycallmonitoringuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[FKSecondaryCallMonitoringUserID] = " + fksecondarycallmonitoringuserid + "");
            }
            if (fkincallassessmentoutcomeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[FKINCallAssessmentOutcomeID] = " + fkincallassessmentoutcomeid + "");
            }
            if (exclusionsexplained != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[ExclusionsExplained] = " + ((bool)exclusionsexplained ? "1" : "0"));
            }
            if (exclusionsexplainedbumpupclosure != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[ExclusionsExplainedBumpUpClosure] = " + ((bool)exclusionsexplainedbumpupclosure ? "1" : "0"));
            }
            if (isrecoveredsale != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[IsRecoveredSale] = " + ((bool)isrecoveredsale ? "1" : "0"));
            }
            if (wasmoneybackvsvoucherbenefitsexplainedcorrectly != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasMoneyBackVsVoucherBenefitsExplainedCorrectly] = " + ((bool)wasmoneybackvsvoucherbenefitsexplainedcorrectly ? "1" : "0"));
            }
            if (iscallmonitored != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[IsCallMonitored] = " + ((bool)iscallmonitored ? "1" : "0"));
            }
            if (wasclearyesgiveninsalesquestion != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasClearYesGivenInSalesQuestion] = " + ((bool)wasclearyesgiveninsalesquestion ? "1" : "0"));
            }
            if (waspermissionquestionasked != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasPermissionQuestionAsked] = " + ((bool)waspermissionquestionasked ? "1" : "0"));
            }
            if (wasnextofkinquestionasked != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[WasNextOfKinQuestionAsked] = " + ((bool)wasnextofkinquestionasked ? "1" : "0"));
            }
            if (fktertiarycallmonitoringuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[FKTertiaryCallMonitoringUserID] = " + fktertiarycallmonitoringuserid + "");
            }
            if (istsrbusavedcarriedforward != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[IsTSRBUSavedCarriedForward] = " + ((bool)istsrbusavedcarriedforward ? "1" : "0"));
            }
            if (tsrbusavedcarriedforwarddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[TSRBUSavedCarriedForwardDate] = '" + tsrbusavedcarriedforwarddate.Value.ToString(Database.DateFormat) + "'");
            }
            if (tsrbusavedcarriedforwardassignedbyuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[TSRBUSavedCarriedForwardAssignedByUserID] = " + tsrbusavedcarriedforwardassignedbyuserid + "");
            }
            if (callmonitoreddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[CallMonitoredDate] = '" + callmonitoreddate.Value.ToString(Database.DateFormat) + "'");
            }
            if (objections != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportCallMonitoring].[Objections] = " + ((bool)objections ? "1" : "0"));
            }
            query.Append("SELECT [INImportCallMonitoring].[ID], [INImportCallMonitoring].[FKINImportID], [INImportCallMonitoring].[IsBankingDetailsCapturedCorrectly], [INImportCallMonitoring].[WasAccountVerified], [INImportCallMonitoring].[WasDebitDateConfirmed], [INImportCallMonitoring].[IsAccountInClientsName], [INImportCallMonitoring].[DoesClientHaveSigningPower], [INImportCallMonitoring].[WasDebiCheckProcessExplainedCorrectly], [INImportCallMonitoring].[WasCorrectClosureQuestionAsked], [INImportCallMonitoring].[WasResponseClearAndPositive], [INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPs], [INImportCallMonitoring].[WasDebitAmountMentionedCorrectly], [INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectly], [INImportCallMonitoring].[WasCorrectCoverCommencementDateMentioned], [INImportCallMonitoring].[WasNonPaymentProcedureExplained], [INImportCallMonitoring].[WasAnnualIncreaseExplained], [INImportCallMonitoring].[WasCorrectQuestionAskedBumpUpClosure], [INImportCallMonitoring].[WasResponseClearAndPositiveBumpUpClosure], [INImportCallMonitoring].[WasUDMAndPLMentionedAsFSPsBumpUpClosure], [INImportCallMonitoring].[WasDebitAmountMentionedCorrectlyBumpUpClosure], [INImportCallMonitoring].[WasFirstDebitDateExplainedCorrectlyBumpUpClosure], [INImportCallMonitoring].[WasCorrectCoverCommencementDateMentionedBumpUpClosure], [INImportCallMonitoring].[WasNonPaymentProcedureExplainedBumpUpClosure], [INImportCallMonitoring].[WasAnnualIncreaseExplainedBumpUpClosure], [INImportCallMonitoring].[FKINCallMonitoringOutcomeID], [INImportCallMonitoring].[Comments], [INImportCallMonitoring].[FKCallMonitoringUserID], [INImportCallMonitoring].[WasCallEvaluatedBySecondaryUser], [INImportCallMonitoring].[FKSecondaryCallMonitoringUserID], [INImportCallMonitoring].[FKINCallAssessmentOutcomeID], [INImportCallMonitoring].[ExclusionsExplained], [INImportCallMonitoring].[ExclusionsExplainedBumpUpClosure], [INImportCallMonitoring].[IsRecoveredSale], [INImportCallMonitoring].[WasMoneyBackVsVoucherBenefitsExplainedCorrectly], [INImportCallMonitoring].[IsCallMonitored], [INImportCallMonitoring].[WasClearYesGivenInSalesQuestion], [INImportCallMonitoring].[WasPermissionQuestionAsked], [INImportCallMonitoring].[WasNextOfKinQuestionAsked], [INImportCallMonitoring].[FKTertiaryCallMonitoringUserID], [INImportCallMonitoring].[IsTSRBUSavedCarriedForward], [INImportCallMonitoring].[TSRBUSavedCarriedForwardDate], [INImportCallMonitoring].[TSRBUSavedCarriedForwardAssignedByUserID], [INImportCallMonitoring].[CallMonitoredDate], [INImportCallMonitoring].[StampDate], [INImportCallMonitoring].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportCallMonitoring].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportCallMonitoring] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
