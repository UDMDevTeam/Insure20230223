using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inbankdetails objects.
    /// </summary>
    internal abstract partial class INBankDetailsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inbankdetails from the database.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails object to delete.</param>
        /// <returns>A query that can be used to delete the inbankdetails from the database.</returns>
        internal static string Delete(INBankDetails inbankdetails, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbankdetails != null)
            {
                query = "INSERT INTO [zHstINBankDetails] ([ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) SELECT [ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [INBankDetails] WHERE [INBankDetails].[ID] = @ID; ";
                query += "DELETE FROM [INBankDetails] WHERE [INBankDetails].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbankdetails.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inbankdetails from the database.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inbankdetails from the database.</returns>
        internal static string DeleteHistory(INBankDetails inbankdetails, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbankdetails != null)
            {
                query = "DELETE FROM [zHstINBankDetails] WHERE [zHstINBankDetails].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbankdetails.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inbankdetails from the database.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails object to undelete.</param>
        /// <returns>A query that can be used to undelete the inbankdetails from the database.</returns>
        internal static string UnDelete(INBankDetails inbankdetails, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbankdetails != null)
            {
                query = "INSERT INTO [INBankDetails] ([ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) SELECT [ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [zHstINBankDetails] WHERE [zHstINBankDetails].[ID] = @ID AND [zHstINBankDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBankDetails] WHERE [zHstINBankDetails].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INBankDetails] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINBankDetails] WHERE [zHstINBankDetails].[ID] = @ID AND [zHstINBankDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINBankDetails] WHERE [zHstINBankDetails].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INBankDetails] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbankdetails.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inbankdetails object.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails object to fill.</param>
        /// <returns>A query that can be used to fill the inbankdetails object.</returns>
        internal static string Fill(INBankDetails inbankdetails, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbankdetails != null)
            {
                query = "SELECT [ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [INBankDetails] WHERE [INBankDetails].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbankdetails.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inbankdetails data.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inbankdetails data.</returns>
        internal static string FillData(INBankDetails inbankdetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbankdetails != null)
            {
            query.Append("SELECT [INBankDetails].[ID], [INBankDetails].[FKPaymentMethodID], [INBankDetails].[AccountHolder], [INBankDetails].[FKBankID], [INBankDetails].[FKBankBranchID], [INBankDetails].[AccountNo], [INBankDetails].[FKAccountTypeID], [INBankDetails].[DebitDay], [INBankDetails].[FKAHTitleID], [INBankDetails].[AHInitials], [INBankDetails].[AHFirstName], [INBankDetails].[AHSurname], [INBankDetails].[AHIDNo], [INBankDetails].[AHTelHome], [INBankDetails].[AHTelCell], [INBankDetails].[AHTelWork], [INBankDetails].[ToDeleteID], [INBankDetails].[FKSigningPowerID], [INBankDetails].[AccNumCheckStatus], [INBankDetails].[AccNumCheckMsg], [INBankDetails].[AccNumCheckMsgFull], [INBankDetails].[StampDate], [INBankDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBankDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBankDetails] ");
                query.Append(" WHERE [INBankDetails].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbankdetails.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inbankdetails object from history.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inbankdetails object from history.</returns>
        internal static string FillHistory(INBankDetails inbankdetails, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inbankdetails != null)
            {
                query = "SELECT [ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [zHstINBankDetails] WHERE [zHstINBankDetails].[ID] = @ID AND [zHstINBankDetails].[StampUserID] = @StampUserID AND [zHstINBankDetails].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inbankdetails.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbankdetailss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inbankdetailss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INBankDetails].[ID], [INBankDetails].[FKPaymentMethodID], [INBankDetails].[AccountHolder], [INBankDetails].[FKBankID], [INBankDetails].[FKBankBranchID], [INBankDetails].[AccountNo], [INBankDetails].[FKAccountTypeID], [INBankDetails].[DebitDay], [INBankDetails].[FKAHTitleID], [INBankDetails].[AHInitials], [INBankDetails].[AHFirstName], [INBankDetails].[AHSurname], [INBankDetails].[AHIDNo], [INBankDetails].[AHTelHome], [INBankDetails].[AHTelCell], [INBankDetails].[AHTelWork], [INBankDetails].[ToDeleteID], [INBankDetails].[FKSigningPowerID], [INBankDetails].[AccNumCheckStatus], [INBankDetails].[AccNumCheckMsg], [INBankDetails].[AccNumCheckMsgFull], [INBankDetails].[StampDate], [INBankDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBankDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBankDetails] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inbankdetailss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inbankdetailss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINBankDetails].[ID], [zHstINBankDetails].[FKPaymentMethodID], [zHstINBankDetails].[AccountHolder], [zHstINBankDetails].[FKBankID], [zHstINBankDetails].[FKBankBranchID], [zHstINBankDetails].[AccountNo], [zHstINBankDetails].[FKAccountTypeID], [zHstINBankDetails].[DebitDay], [zHstINBankDetails].[FKAHTitleID], [zHstINBankDetails].[AHInitials], [zHstINBankDetails].[AHFirstName], [zHstINBankDetails].[AHSurname], [zHstINBankDetails].[AHIDNo], [zHstINBankDetails].[AHTelHome], [zHstINBankDetails].[AHTelCell], [zHstINBankDetails].[AHTelWork], [zHstINBankDetails].[ToDeleteID], [zHstINBankDetails].[FKSigningPowerID], [zHstINBankDetails].[AccNumCheckStatus], [zHstINBankDetails].[AccNumCheckMsg], [zHstINBankDetails].[AccNumCheckMsgFull], [zHstINBankDetails].[StampDate], [zHstINBankDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINBankDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINBankDetails] ");
            query.Append("INNER JOIN (SELECT [zHstINBankDetails].[ID], MAX([zHstINBankDetails].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINBankDetails] ");
            query.Append("WHERE [zHstINBankDetails].[ID] NOT IN (SELECT [INBankDetails].[ID] FROM [INBankDetails]) ");
            query.Append("GROUP BY [zHstINBankDetails].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINBankDetails].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINBankDetails].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inbankdetails in the database.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inbankdetails in the database.</returns>
        public static string ListHistory(INBankDetails inbankdetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbankdetails != null)
            {
            query.Append("SELECT [zHstINBankDetails].[ID], [zHstINBankDetails].[FKPaymentMethodID], [zHstINBankDetails].[AccountHolder], [zHstINBankDetails].[FKBankID], [zHstINBankDetails].[FKBankBranchID], [zHstINBankDetails].[AccountNo], [zHstINBankDetails].[FKAccountTypeID], [zHstINBankDetails].[DebitDay], [zHstINBankDetails].[FKAHTitleID], [zHstINBankDetails].[AHInitials], [zHstINBankDetails].[AHFirstName], [zHstINBankDetails].[AHSurname], [zHstINBankDetails].[AHIDNo], [zHstINBankDetails].[AHTelHome], [zHstINBankDetails].[AHTelCell], [zHstINBankDetails].[AHTelWork], [zHstINBankDetails].[ToDeleteID], [zHstINBankDetails].[FKSigningPowerID], [zHstINBankDetails].[AccNumCheckStatus], [zHstINBankDetails].[AccNumCheckMsg], [zHstINBankDetails].[AccNumCheckMsgFull], [zHstINBankDetails].[StampDate], [zHstINBankDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINBankDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINBankDetails] ");
                query.Append(" WHERE [zHstINBankDetails].[ID] = @ID");
                query.Append(" ORDER BY [zHstINBankDetails].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inbankdetails.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inbankdetails to the database.
        /// </summary>
        /// <param name="inbankdetails">The inbankdetails to save.</param>
        /// <returns>A query that can be used to save the inbankdetails to the database.</returns>
        internal static string Save(INBankDetails inbankdetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inbankdetails != null)
            {
                if (inbankdetails.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINBankDetails] ([ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) SELECT [ID], [FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [INBankDetails] WHERE [INBankDetails].[ID] = @ID; ");
                    query.Append("UPDATE [INBankDetails]");
                    parameters = new object[21];
                    query.Append(" SET [FKPaymentMethodID] = @FKPaymentMethodID");
                    parameters[0] = Database.GetParameter("@FKPaymentMethodID", inbankdetails.FKPaymentMethodID.HasValue ? (object)inbankdetails.FKPaymentMethodID.Value : DBNull.Value);
                    query.Append(", [AccountHolder] = @AccountHolder");
                    parameters[1] = Database.GetParameter("@AccountHolder", string.IsNullOrEmpty(inbankdetails.AccountHolder) ? DBNull.Value : (object)inbankdetails.AccountHolder);
                    query.Append(", [FKBankID] = @FKBankID");
                    parameters[2] = Database.GetParameter("@FKBankID", inbankdetails.FKBankID.HasValue ? (object)inbankdetails.FKBankID.Value : DBNull.Value);
                    query.Append(", [FKBankBranchID] = @FKBankBranchID");
                    parameters[3] = Database.GetParameter("@FKBankBranchID", inbankdetails.FKBankBranchID.HasValue ? (object)inbankdetails.FKBankBranchID.Value : DBNull.Value);
                    query.Append(", [AccountNo] = @AccountNo");
                    parameters[4] = Database.GetParameter("@AccountNo", string.IsNullOrEmpty(inbankdetails.AccountNo) ? DBNull.Value : (object)inbankdetails.AccountNo);
                    query.Append(", [FKAccountTypeID] = @FKAccountTypeID");
                    parameters[5] = Database.GetParameter("@FKAccountTypeID", inbankdetails.FKAccountTypeID.HasValue ? (object)inbankdetails.FKAccountTypeID.Value : DBNull.Value);
                    query.Append(", [DebitDay] = @DebitDay");
                    parameters[6] = Database.GetParameter("@DebitDay", inbankdetails.DebitDay.HasValue ? (object)inbankdetails.DebitDay.Value : DBNull.Value);
                    query.Append(", [FKAHTitleID] = @FKAHTitleID");
                    parameters[7] = Database.GetParameter("@FKAHTitleID", inbankdetails.FKAHTitleID.HasValue ? (object)inbankdetails.FKAHTitleID.Value : DBNull.Value);
                    query.Append(", [AHInitials] = @AHInitials");
                    parameters[8] = Database.GetParameter("@AHInitials", string.IsNullOrEmpty(inbankdetails.AHInitials) ? DBNull.Value : (object)inbankdetails.AHInitials);
                    query.Append(", [AHFirstName] = @AHFirstName");
                    parameters[9] = Database.GetParameter("@AHFirstName", string.IsNullOrEmpty(inbankdetails.AHFirstName) ? DBNull.Value : (object)inbankdetails.AHFirstName);
                    query.Append(", [AHSurname] = @AHSurname");
                    parameters[10] = Database.GetParameter("@AHSurname", string.IsNullOrEmpty(inbankdetails.AHSurname) ? DBNull.Value : (object)inbankdetails.AHSurname);
                    query.Append(", [AHIDNo] = @AHIDNo");
                    parameters[11] = Database.GetParameter("@AHIDNo", string.IsNullOrEmpty(inbankdetails.AHIDNo) ? DBNull.Value : (object)inbankdetails.AHIDNo);
                    query.Append(", [AHTelHome] = @AHTelHome");
                    parameters[12] = Database.GetParameter("@AHTelHome", string.IsNullOrEmpty(inbankdetails.AHTelHome) ? DBNull.Value : (object)inbankdetails.AHTelHome);
                    query.Append(", [AHTelCell] = @AHTelCell");
                    parameters[13] = Database.GetParameter("@AHTelCell", string.IsNullOrEmpty(inbankdetails.AHTelCell) ? DBNull.Value : (object)inbankdetails.AHTelCell);
                    query.Append(", [AHTelWork] = @AHTelWork");
                    parameters[14] = Database.GetParameter("@AHTelWork", string.IsNullOrEmpty(inbankdetails.AHTelWork) ? DBNull.Value : (object)inbankdetails.AHTelWork);
                    query.Append(", [ToDeleteID] = @ToDeleteID");
                    parameters[15] = Database.GetParameter("@ToDeleteID", inbankdetails.ToDeleteID.HasValue ? (object)inbankdetails.ToDeleteID.Value : DBNull.Value);
                    query.Append(", [FKSigningPowerID] = @FKSigningPowerID");
                    parameters[16] = Database.GetParameter("@FKSigningPowerID", inbankdetails.FKSigningPowerID.HasValue ? (object)inbankdetails.FKSigningPowerID.Value : DBNull.Value);
                    query.Append(", [AccNumCheckStatus] = @AccNumCheckStatus");
                    parameters[17] = Database.GetParameter("@AccNumCheckStatus", inbankdetails.AccNumCheckStatus.HasValue ? (object)inbankdetails.AccNumCheckStatus.Value : DBNull.Value);
                    query.Append(", [AccNumCheckMsg] = @AccNumCheckMsg");
                    parameters[18] = Database.GetParameter("@AccNumCheckMsg", string.IsNullOrEmpty(inbankdetails.AccNumCheckMsg) ? DBNull.Value : (object)inbankdetails.AccNumCheckMsg);
                    query.Append(", [AccNumCheckMsgFull] = @AccNumCheckMsgFull");
                    parameters[19] = Database.GetParameter("@AccNumCheckMsgFull", string.IsNullOrEmpty(inbankdetails.AccNumCheckMsgFull) ? DBNull.Value : (object)inbankdetails.AccNumCheckMsgFull);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INBankDetails].[ID] = @ID"); 
                    parameters[20] = Database.GetParameter("@ID", inbankdetails.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INBankDetails] ([FKPaymentMethodID], [AccountHolder], [FKBankID], [FKBankBranchID], [AccountNo], [FKAccountTypeID], [DebitDay], [FKAHTitleID], [AHInitials], [AHFirstName], [AHSurname], [AHIDNo], [AHTelHome], [AHTelCell], [AHTelWork], [ToDeleteID], [FKSigningPowerID], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) VALUES(@FKPaymentMethodID, @AccountHolder, @FKBankID, @FKBankBranchID, @AccountNo, @FKAccountTypeID, @DebitDay, @FKAHTitleID, @AHInitials, @AHFirstName, @AHSurname, @AHIDNo, @AHTelHome, @AHTelCell, @AHTelWork, @ToDeleteID, @FKSigningPowerID, @AccNumCheckStatus, @AccNumCheckMsg, @AccNumCheckMsgFull, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[20];
                    parameters[0] = Database.GetParameter("@FKPaymentMethodID", inbankdetails.FKPaymentMethodID.HasValue ? (object)inbankdetails.FKPaymentMethodID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@AccountHolder", string.IsNullOrEmpty(inbankdetails.AccountHolder) ? DBNull.Value : (object)inbankdetails.AccountHolder);
                    parameters[2] = Database.GetParameter("@FKBankID", inbankdetails.FKBankID.HasValue ? (object)inbankdetails.FKBankID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKBankBranchID", inbankdetails.FKBankBranchID.HasValue ? (object)inbankdetails.FKBankBranchID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@AccountNo", string.IsNullOrEmpty(inbankdetails.AccountNo) ? DBNull.Value : (object)inbankdetails.AccountNo);
                    parameters[5] = Database.GetParameter("@FKAccountTypeID", inbankdetails.FKAccountTypeID.HasValue ? (object)inbankdetails.FKAccountTypeID.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@DebitDay", inbankdetails.DebitDay.HasValue ? (object)inbankdetails.DebitDay.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@FKAHTitleID", inbankdetails.FKAHTitleID.HasValue ? (object)inbankdetails.FKAHTitleID.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@AHInitials", string.IsNullOrEmpty(inbankdetails.AHInitials) ? DBNull.Value : (object)inbankdetails.AHInitials);
                    parameters[9] = Database.GetParameter("@AHFirstName", string.IsNullOrEmpty(inbankdetails.AHFirstName) ? DBNull.Value : (object)inbankdetails.AHFirstName);
                    parameters[10] = Database.GetParameter("@AHSurname", string.IsNullOrEmpty(inbankdetails.AHSurname) ? DBNull.Value : (object)inbankdetails.AHSurname);
                    parameters[11] = Database.GetParameter("@AHIDNo", string.IsNullOrEmpty(inbankdetails.AHIDNo) ? DBNull.Value : (object)inbankdetails.AHIDNo);
                    parameters[12] = Database.GetParameter("@AHTelHome", string.IsNullOrEmpty(inbankdetails.AHTelHome) ? DBNull.Value : (object)inbankdetails.AHTelHome);
                    parameters[13] = Database.GetParameter("@AHTelCell", string.IsNullOrEmpty(inbankdetails.AHTelCell) ? DBNull.Value : (object)inbankdetails.AHTelCell);
                    parameters[14] = Database.GetParameter("@AHTelWork", string.IsNullOrEmpty(inbankdetails.AHTelWork) ? DBNull.Value : (object)inbankdetails.AHTelWork);
                    parameters[15] = Database.GetParameter("@ToDeleteID", inbankdetails.ToDeleteID.HasValue ? (object)inbankdetails.ToDeleteID.Value : DBNull.Value);
                    parameters[16] = Database.GetParameter("@FKSigningPowerID", inbankdetails.FKSigningPowerID.HasValue ? (object)inbankdetails.FKSigningPowerID.Value : DBNull.Value);
                    parameters[17] = Database.GetParameter("@AccNumCheckStatus", inbankdetails.AccNumCheckStatus.HasValue ? (object)inbankdetails.AccNumCheckStatus.Value : DBNull.Value);
                    parameters[18] = Database.GetParameter("@AccNumCheckMsg", string.IsNullOrEmpty(inbankdetails.AccNumCheckMsg) ? DBNull.Value : (object)inbankdetails.AccNumCheckMsg);
                    parameters[19] = Database.GetParameter("@AccNumCheckMsgFull", string.IsNullOrEmpty(inbankdetails.AccNumCheckMsgFull) ? DBNull.Value : (object)inbankdetails.AccNumCheckMsgFull);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbankdetailss that match the search criteria.
        /// </summary>
        /// <param name="fkpaymentmethodid">The fkpaymentmethodid search criteria.</param>
        /// <param name="accountholder">The accountholder search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountno">The accountno search criteria.</param>
        /// <param name="fkaccounttypeid">The fkaccounttypeid search criteria.</param>
        /// <param name="debitday">The debitday search criteria.</param>
        /// <param name="fkahtitleid">The fkahtitleid search criteria.</param>
        /// <param name="ahinitials">The ahinitials search criteria.</param>
        /// <param name="ahfirstname">The ahfirstname search criteria.</param>
        /// <param name="ahsurname">The ahsurname search criteria.</param>
        /// <param name="ahidno">The ahidno search criteria.</param>
        /// <param name="ahtelhome">The ahtelhome search criteria.</param>
        /// <param name="ahtelcell">The ahtelcell search criteria.</param>
        /// <param name="ahtelwork">The ahtelwork search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="fksigningpowerid">The fksigningpowerid search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <returns>A query that can be used to search for inbankdetailss based on the search criteria.</returns>
        internal static string Search(long? fkpaymentmethodid, string accountholder, long? fkbankid, long? fkbankbranchid, string accountno, long? fkaccounttypeid, short? debitday, long? fkahtitleid, string ahinitials, string ahfirstname, string ahsurname, string ahidno, string ahtelhome, string ahtelcell, string ahtelwork, long? todeleteid, long? fksigningpowerid, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkpaymentmethodid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[FKPaymentMethodID] = " + fkpaymentmethodid + "");
            }
            if (accountholder != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AccountHolder] LIKE '" + accountholder.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkbankid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[FKBankID] = " + fkbankid + "");
            }
            if (fkbankbranchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[FKBankBranchID] = " + fkbankbranchid + "");
            }
            if (accountno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AccountNo] LIKE '" + accountno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkaccounttypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[FKAccountTypeID] = " + fkaccounttypeid + "");
            }
            if (debitday != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[DebitDay] = " + debitday + "");
            }
            if (fkahtitleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[FKAHTitleID] = " + fkahtitleid + "");
            }
            if (ahinitials != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AHInitials] LIKE '" + ahinitials.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ahfirstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AHFirstName] LIKE '" + ahfirstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ahsurname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AHSurname] LIKE '" + ahsurname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ahidno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AHIDNo] LIKE '" + ahidno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ahtelhome != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AHTelHome] LIKE '" + ahtelhome.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ahtelcell != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AHTelCell] LIKE '" + ahtelcell.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ahtelwork != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AHTelWork] LIKE '" + ahtelwork.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (todeleteid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[ToDeleteID] = " + todeleteid + "");
            }
            if (fksigningpowerid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[FKSigningPowerID] = " + fksigningpowerid + "");
            }
            if (accnumcheckstatus != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AccNumCheckStatus] = " + accnumcheckstatus + "");
            }
            if (accnumcheckmsg != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AccNumCheckMsg] LIKE '" + accnumcheckmsg.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (accnumcheckmsgfull != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INBankDetails].[AccNumCheckMsgFull] LIKE '" + accnumcheckmsgfull.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INBankDetails].[ID], [INBankDetails].[FKPaymentMethodID], [INBankDetails].[AccountHolder], [INBankDetails].[FKBankID], [INBankDetails].[FKBankBranchID], [INBankDetails].[AccountNo], [INBankDetails].[FKAccountTypeID], [INBankDetails].[DebitDay], [INBankDetails].[FKAHTitleID], [INBankDetails].[AHInitials], [INBankDetails].[AHFirstName], [INBankDetails].[AHSurname], [INBankDetails].[AHIDNo], [INBankDetails].[AHTelHome], [INBankDetails].[AHTelCell], [INBankDetails].[AHTelWork], [INBankDetails].[ToDeleteID], [INBankDetails].[FKSigningPowerID], [INBankDetails].[AccNumCheckStatus], [INBankDetails].[AccNumCheckMsg], [INBankDetails].[AccNumCheckMsgFull], [INBankDetails].[StampDate], [INBankDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INBankDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INBankDetails] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
