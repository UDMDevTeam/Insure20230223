using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inpolicy objects.
    /// </summary>
    internal abstract partial class INPolicyQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inpolicy from the database.
        /// </summary>
        /// <param name="inpolicy">The inpolicy object to delete.</param>
        /// <returns>A query that can be used to delete the inpolicy from the database.</returns>
        internal static string Delete(INPolicy inpolicy, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicy != null)
            {
                query = "INSERT INTO [zHstINPolicy] ([ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID]) SELECT [ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID] FROM [INPolicy] WHERE [INPolicy].[ID] = @ID; ";
                query += "DELETE FROM [INPolicy] WHERE [INPolicy].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicy.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inpolicy from the database.
        /// </summary>
        /// <param name="inpolicy">The inpolicy object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inpolicy from the database.</returns>
        internal static string DeleteHistory(INPolicy inpolicy, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicy != null)
            {
                query = "DELETE FROM [zHstINPolicy] WHERE [zHstINPolicy].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicy.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inpolicy from the database.
        /// </summary>
        /// <param name="inpolicy">The inpolicy object to undelete.</param>
        /// <returns>A query that can be used to undelete the inpolicy from the database.</returns>
        internal static string UnDelete(INPolicy inpolicy, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicy != null)
            {
                query = "INSERT INTO [INPolicy] ([ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID]) SELECT [ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID] FROM [zHstINPolicy] WHERE [zHstINPolicy].[ID] = @ID AND [zHstINPolicy].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicy] WHERE [zHstINPolicy].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPolicy] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPolicy] WHERE [zHstINPolicy].[ID] = @ID AND [zHstINPolicy].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicy] WHERE [zHstINPolicy].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPolicy] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicy.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inpolicy object.
        /// </summary>
        /// <param name="inpolicy">The inpolicy object to fill.</param>
        /// <returns>A query that can be used to fill the inpolicy object.</returns>
        internal static string Fill(INPolicy inpolicy, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicy != null)
            {
                query = "SELECT [ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID] FROM [INPolicy] WHERE [INPolicy].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicy.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inpolicy data.
        /// </summary>
        /// <param name="inpolicy">The inpolicy to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inpolicy data.</returns>
        internal static string FillData(INPolicy inpolicy, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicy != null)
            {
            query.Append("SELECT [INPolicy].[ID], [INPolicy].[PolicyID], [INPolicy].[FKPolicyTypeID], [INPolicy].[FKINPolicyHolderID], [INPolicy].[FKINBankDetailsID], [INPolicy].[FKINOptionID], [INPolicy].[FKINMoneyBackID], [INPolicy].[FKINBumpUpOptionID], [INPolicy].[UDMBumpUpOption], [INPolicy].[BumpUpAmount], [INPolicy].[ReducedPremiumOption], [INPolicy].[ReducedPremiumAmount], [INPolicy].[PolicyFee], [INPolicy].[TotalPremium], [INPolicy].[CommenceDate], [INPolicy].[OptionLA2], [INPolicy].[OptionChild], [INPolicy].[OptionFuneral], [INPolicy].[BumpUpOffered], [INPolicy].[TotalInvoiceFee], [INPolicy].[FKINOptionFeesID], [INPolicy].[StampDate], [INPolicy].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicy].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicy] ");
                query.Append(" WHERE [INPolicy].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicy.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inpolicy object from history.
        /// </summary>
        /// <param name="inpolicy">The inpolicy object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inpolicy object from history.</returns>
        internal static string FillHistory(INPolicy inpolicy, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicy != null)
            {
                query = "SELECT [ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID] FROM [zHstINPolicy] WHERE [zHstINPolicy].[ID] = @ID AND [zHstINPolicy].[StampUserID] = @StampUserID AND [zHstINPolicy].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inpolicy.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicys in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inpolicys in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INPolicy].[ID], [INPolicy].[PolicyID], [INPolicy].[FKPolicyTypeID], [INPolicy].[FKINPolicyHolderID], [INPolicy].[FKINBankDetailsID], [INPolicy].[FKINOptionID], [INPolicy].[FKINMoneyBackID], [INPolicy].[FKINBumpUpOptionID], [INPolicy].[UDMBumpUpOption], [INPolicy].[BumpUpAmount], [INPolicy].[ReducedPremiumOption], [INPolicy].[ReducedPremiumAmount], [INPolicy].[PolicyFee], [INPolicy].[TotalPremium], [INPolicy].[CommenceDate], [INPolicy].[OptionLA2], [INPolicy].[OptionChild], [INPolicy].[OptionFuneral], [INPolicy].[BumpUpOffered], [INPolicy].[TotalInvoiceFee], [INPolicy].[FKINOptionFeesID], [INPolicy].[StampDate], [INPolicy].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicy].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicy] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inpolicys in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inpolicys in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPolicy].[ID], [zHstINPolicy].[PolicyID], [zHstINPolicy].[FKPolicyTypeID], [zHstINPolicy].[FKINPolicyHolderID], [zHstINPolicy].[FKINBankDetailsID], [zHstINPolicy].[FKINOptionID], [zHstINPolicy].[FKINMoneyBackID], [zHstINPolicy].[FKINBumpUpOptionID], [zHstINPolicy].[UDMBumpUpOption], [zHstINPolicy].[BumpUpAmount], [zHstINPolicy].[ReducedPremiumOption], [zHstINPolicy].[ReducedPremiumAmount], [zHstINPolicy].[PolicyFee], [zHstINPolicy].[TotalPremium], [zHstINPolicy].[CommenceDate], [zHstINPolicy].[OptionLA2], [zHstINPolicy].[OptionChild], [zHstINPolicy].[OptionFuneral], [zHstINPolicy].[BumpUpOffered], [zHstINPolicy].[TotalInvoiceFee], [zHstINPolicy].[FKINOptionFeesID], [zHstINPolicy].[StampDate], [zHstINPolicy].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicy].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicy] ");
            query.Append("INNER JOIN (SELECT [zHstINPolicy].[ID], MAX([zHstINPolicy].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPolicy] ");
            query.Append("WHERE [zHstINPolicy].[ID] NOT IN (SELECT [INPolicy].[ID] FROM [INPolicy]) ");
            query.Append("GROUP BY [zHstINPolicy].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPolicy].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPolicy].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inpolicy in the database.
        /// </summary>
        /// <param name="inpolicy">The inpolicy object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inpolicy in the database.</returns>
        public static string ListHistory(INPolicy inpolicy, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicy != null)
            {
            query.Append("SELECT [zHstINPolicy].[ID], [zHstINPolicy].[PolicyID], [zHstINPolicy].[FKPolicyTypeID], [zHstINPolicy].[FKINPolicyHolderID], [zHstINPolicy].[FKINBankDetailsID], [zHstINPolicy].[FKINOptionID], [zHstINPolicy].[FKINMoneyBackID], [zHstINPolicy].[FKINBumpUpOptionID], [zHstINPolicy].[UDMBumpUpOption], [zHstINPolicy].[BumpUpAmount], [zHstINPolicy].[ReducedPremiumOption], [zHstINPolicy].[ReducedPremiumAmount], [zHstINPolicy].[PolicyFee], [zHstINPolicy].[TotalPremium], [zHstINPolicy].[CommenceDate], [zHstINPolicy].[OptionLA2], [zHstINPolicy].[OptionChild], [zHstINPolicy].[OptionFuneral], [zHstINPolicy].[BumpUpOffered], [zHstINPolicy].[TotalInvoiceFee], [zHstINPolicy].[FKINOptionFeesID], [zHstINPolicy].[StampDate], [zHstINPolicy].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicy].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicy] ");
                query.Append(" WHERE [zHstINPolicy].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPolicy].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicy.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inpolicy to the database.
        /// </summary>
        /// <param name="inpolicy">The inpolicy to save.</param>
        /// <returns>A query that can be used to save the inpolicy to the database.</returns>
        internal static string Save(INPolicy inpolicy, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicy != null)
            {
                if (inpolicy.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPolicy] ([ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID]) SELECT [ID], [PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID] FROM [INPolicy] WHERE [INPolicy].[ID] = @ID; ");
                    query.Append("UPDATE [INPolicy]");
                    parameters = new object[21];
                    query.Append(" SET [PolicyID] = @PolicyID");
                    parameters[0] = Database.GetParameter("@PolicyID", string.IsNullOrEmpty(inpolicy.PolicyID) ? DBNull.Value : (object)inpolicy.PolicyID);
                    query.Append(", [FKPolicyTypeID] = @FKPolicyTypeID");
                    parameters[1] = Database.GetParameter("@FKPolicyTypeID", inpolicy.FKPolicyTypeID.HasValue ? (object)inpolicy.FKPolicyTypeID.Value : DBNull.Value);
                    query.Append(", [FKINPolicyHolderID] = @FKINPolicyHolderID");
                    parameters[2] = Database.GetParameter("@FKINPolicyHolderID", inpolicy.FKINPolicyHolderID.HasValue ? (object)inpolicy.FKINPolicyHolderID.Value : DBNull.Value);
                    query.Append(", [FKINBankDetailsID] = @FKINBankDetailsID");
                    parameters[3] = Database.GetParameter("@FKINBankDetailsID", inpolicy.FKINBankDetailsID.HasValue ? (object)inpolicy.FKINBankDetailsID.Value : DBNull.Value);
                    query.Append(", [FKINOptionID] = @FKINOptionID");
                    parameters[4] = Database.GetParameter("@FKINOptionID", inpolicy.FKINOptionID.HasValue ? (object)inpolicy.FKINOptionID.Value : DBNull.Value);
                    query.Append(", [FKINMoneyBackID] = @FKINMoneyBackID");
                    parameters[5] = Database.GetParameter("@FKINMoneyBackID", inpolicy.FKINMoneyBackID.HasValue ? (object)inpolicy.FKINMoneyBackID.Value : DBNull.Value);
                    query.Append(", [FKINBumpUpOptionID] = @FKINBumpUpOptionID");
                    parameters[6] = Database.GetParameter("@FKINBumpUpOptionID", inpolicy.FKINBumpUpOptionID.HasValue ? (object)inpolicy.FKINBumpUpOptionID.Value : DBNull.Value);
                    query.Append(", [UDMBumpUpOption] = @UDMBumpUpOption");
                    parameters[7] = Database.GetParameter("@UDMBumpUpOption", inpolicy.UDMBumpUpOption.HasValue ? (object)inpolicy.UDMBumpUpOption.Value : DBNull.Value);
                    query.Append(", [BumpUpAmount] = @BumpUpAmount");
                    parameters[8] = Database.GetParameter("@BumpUpAmount", inpolicy.BumpUpAmount.HasValue ? (object)inpolicy.BumpUpAmount.Value : DBNull.Value);
                    query.Append(", [ReducedPremiumOption] = @ReducedPremiumOption");
                    parameters[9] = Database.GetParameter("@ReducedPremiumOption", inpolicy.ReducedPremiumOption.HasValue ? (object)inpolicy.ReducedPremiumOption.Value : DBNull.Value);
                    query.Append(", [ReducedPremiumAmount] = @ReducedPremiumAmount");
                    parameters[10] = Database.GetParameter("@ReducedPremiumAmount", inpolicy.ReducedPremiumAmount.HasValue ? (object)inpolicy.ReducedPremiumAmount.Value : DBNull.Value);
                    query.Append(", [PolicyFee] = @PolicyFee");
                    parameters[11] = Database.GetParameter("@PolicyFee", inpolicy.PolicyFee.HasValue ? (object)inpolicy.PolicyFee.Value : DBNull.Value);
                    query.Append(", [TotalPremium] = @TotalPremium");
                    parameters[12] = Database.GetParameter("@TotalPremium", inpolicy.TotalPremium.HasValue ? (object)inpolicy.TotalPremium.Value : DBNull.Value);
                    query.Append(", [CommenceDate] = @CommenceDate");
                    parameters[13] = Database.GetParameter("@CommenceDate", inpolicy.CommenceDate.HasValue ? (object)inpolicy.CommenceDate.Value : DBNull.Value);
                    query.Append(", [OptionLA2] = @OptionLA2");
                    parameters[14] = Database.GetParameter("@OptionLA2", inpolicy.OptionLA2.HasValue ? (object)inpolicy.OptionLA2.Value : DBNull.Value);
                    query.Append(", [OptionChild] = @OptionChild");
                    parameters[15] = Database.GetParameter("@OptionChild", inpolicy.OptionChild.HasValue ? (object)inpolicy.OptionChild.Value : DBNull.Value);
                    query.Append(", [OptionFuneral] = @OptionFuneral");
                    parameters[16] = Database.GetParameter("@OptionFuneral", inpolicy.OptionFuneral.HasValue ? (object)inpolicy.OptionFuneral.Value : DBNull.Value);
                    query.Append(", [BumpUpOffered] = @BumpUpOffered");
                    parameters[17] = Database.GetParameter("@BumpUpOffered", inpolicy.BumpUpOffered.HasValue ? (object)inpolicy.BumpUpOffered.Value : DBNull.Value);
                    query.Append(", [TotalInvoiceFee] = @TotalInvoiceFee");
                    parameters[18] = Database.GetParameter("@TotalInvoiceFee", inpolicy.TotalInvoiceFee.HasValue ? (object)inpolicy.TotalInvoiceFee.Value : DBNull.Value);
                    query.Append(", [FKINOptionFeesID] = @FKINOptionFeesID");
                    parameters[19] = Database.GetParameter("@FKINOptionFeesID", inpolicy.FKINOptionFeesID.HasValue ? (object)inpolicy.FKINOptionFeesID.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPolicy].[ID] = @ID"); 
                    parameters[20] = Database.GetParameter("@ID", inpolicy.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPolicy] ([PolicyID], [FKPolicyTypeID], [FKINPolicyHolderID], [FKINBankDetailsID], [FKINOptionID], [FKINMoneyBackID], [FKINBumpUpOptionID], [UDMBumpUpOption], [BumpUpAmount], [ReducedPremiumOption], [ReducedPremiumAmount], [PolicyFee], [TotalPremium], [CommenceDate], [OptionLA2], [OptionChild], [OptionFuneral], [BumpUpOffered], [TotalInvoiceFee], [FKINOptionFeesID], [StampDate], [StampUserID]) VALUES(@PolicyID, @FKPolicyTypeID, @FKINPolicyHolderID, @FKINBankDetailsID, @FKINOptionID, @FKINMoneyBackID, @FKINBumpUpOptionID, @UDMBumpUpOption, @BumpUpAmount, @ReducedPremiumOption, @ReducedPremiumAmount, @PolicyFee, @TotalPremium, @CommenceDate, @OptionLA2, @OptionChild, @OptionFuneral, @BumpUpOffered, @TotalInvoiceFee, @FKINOptionFeesID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[20];
                    parameters[0] = Database.GetParameter("@PolicyID", string.IsNullOrEmpty(inpolicy.PolicyID) ? DBNull.Value : (object)inpolicy.PolicyID);
                    parameters[1] = Database.GetParameter("@FKPolicyTypeID", inpolicy.FKPolicyTypeID.HasValue ? (object)inpolicy.FKPolicyTypeID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKINPolicyHolderID", inpolicy.FKINPolicyHolderID.HasValue ? (object)inpolicy.FKINPolicyHolderID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKINBankDetailsID", inpolicy.FKINBankDetailsID.HasValue ? (object)inpolicy.FKINBankDetailsID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@FKINOptionID", inpolicy.FKINOptionID.HasValue ? (object)inpolicy.FKINOptionID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@FKINMoneyBackID", inpolicy.FKINMoneyBackID.HasValue ? (object)inpolicy.FKINMoneyBackID.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@FKINBumpUpOptionID", inpolicy.FKINBumpUpOptionID.HasValue ? (object)inpolicy.FKINBumpUpOptionID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@UDMBumpUpOption", inpolicy.UDMBumpUpOption.HasValue ? (object)inpolicy.UDMBumpUpOption.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@BumpUpAmount", inpolicy.BumpUpAmount.HasValue ? (object)inpolicy.BumpUpAmount.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@ReducedPremiumOption", inpolicy.ReducedPremiumOption.HasValue ? (object)inpolicy.ReducedPremiumOption.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@ReducedPremiumAmount", inpolicy.ReducedPremiumAmount.HasValue ? (object)inpolicy.ReducedPremiumAmount.Value : DBNull.Value);
                    parameters[11] = Database.GetParameter("@PolicyFee", inpolicy.PolicyFee.HasValue ? (object)inpolicy.PolicyFee.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@TotalPremium", inpolicy.TotalPremium.HasValue ? (object)inpolicy.TotalPremium.Value : DBNull.Value);
                    parameters[13] = Database.GetParameter("@CommenceDate", inpolicy.CommenceDate.HasValue ? (object)inpolicy.CommenceDate.Value : DBNull.Value);
                    parameters[14] = Database.GetParameter("@OptionLA2", inpolicy.OptionLA2.HasValue ? (object)inpolicy.OptionLA2.Value : DBNull.Value);
                    parameters[15] = Database.GetParameter("@OptionChild", inpolicy.OptionChild.HasValue ? (object)inpolicy.OptionChild.Value : DBNull.Value);
                    parameters[16] = Database.GetParameter("@OptionFuneral", inpolicy.OptionFuneral.HasValue ? (object)inpolicy.OptionFuneral.Value : DBNull.Value);
                    parameters[17] = Database.GetParameter("@BumpUpOffered", inpolicy.BumpUpOffered.HasValue ? (object)inpolicy.BumpUpOffered.Value : DBNull.Value);
                    parameters[18] = Database.GetParameter("@TotalInvoiceFee", inpolicy.TotalInvoiceFee.HasValue ? (object)inpolicy.TotalInvoiceFee.Value : DBNull.Value);
                    parameters[19] = Database.GetParameter("@FKINOptionFeesID", inpolicy.FKINOptionFeesID.HasValue ? (object)inpolicy.FKINOptionFeesID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicys that match the search criteria.
        /// </summary>
        /// <param name="policyid">The policyid search criteria.</param>
        /// <param name="fkpolicytypeid">The fkpolicytypeid search criteria.</param>
        /// <param name="fkinpolicyholderid">The fkinpolicyholderid search criteria.</param>
        /// <param name="fkinbankdetailsid">The fkinbankdetailsid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinmoneybackid">The fkinmoneybackid search criteria.</param>
        /// <param name="fkinbumpupoptionid">The fkinbumpupoptionid search criteria.</param>
        /// <param name="udmbumpupoption">The udmbumpupoption search criteria.</param>
        /// <param name="bumpupamount">The bumpupamount search criteria.</param>
        /// <param name="reducedpremiumoption">The reducedpremiumoption search criteria.</param>
        /// <param name="reducedpremiumamount">The reducedpremiumamount search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="totalpremium">The totalpremium search criteria.</param>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="optionla2">The optionla2 search criteria.</param>
        /// <param name="optionchild">The optionchild search criteria.</param>
        /// <param name="optionfuneral">The optionfuneral search criteria.</param>
        /// <param name="bumpupoffered">The bumpupoffered search criteria.</param>
        /// <param name="totalinvoicefee">The totalinvoicefee search criteria.</param>
        /// <param name="fkinoptionfeesid">The fkinoptionfeesid search criteria.</param>
        /// <returns>A query that can be used to search for inpolicys based on the search criteria.</returns>
        internal static string Search(string policyid, long? fkpolicytypeid, long? fkinpolicyholderid, long? fkinbankdetailsid, long? fkinoptionid, long? fkinmoneybackid, long? fkinbumpupoptionid, bool? udmbumpupoption, decimal? bumpupamount, bool? reducedpremiumoption, decimal? reducedpremiumamount, decimal? policyfee, decimal? totalpremium, DateTime? commencedate, bool? optionla2, bool? optionchild, bool? optionfuneral, bool? bumpupoffered, decimal? totalinvoicefee, long? fkinoptionfeesid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (policyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[PolicyID] LIKE '" + policyid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkpolicytypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[FKPolicyTypeID] = " + fkpolicytypeid + "");
            }
            if (fkinpolicyholderid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[FKINPolicyHolderID] = " + fkinpolicyholderid + "");
            }
            if (fkinbankdetailsid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[FKINBankDetailsID] = " + fkinbankdetailsid + "");
            }
            if (fkinoptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[FKINOptionID] = " + fkinoptionid + "");
            }
            if (fkinmoneybackid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[FKINMoneyBackID] = " + fkinmoneybackid + "");
            }
            if (fkinbumpupoptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[FKINBumpUpOptionID] = " + fkinbumpupoptionid + "");
            }
            if (udmbumpupoption != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[UDMBumpUpOption] = " + ((bool)udmbumpupoption ? "1" : "0"));
            }
            if (bumpupamount != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[BumpUpAmount] = " + bumpupamount + "");
            }
            if (reducedpremiumoption != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[ReducedPremiumOption] = " + ((bool)reducedpremiumoption ? "1" : "0"));
            }
            if (reducedpremiumamount != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[ReducedPremiumAmount] = " + reducedpremiumamount + "");
            }
            if (policyfee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[PolicyFee] = " + policyfee + "");
            }
            if (totalpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[TotalPremium] = " + totalpremium + "");
            }
            if (commencedate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[CommenceDate] = '" + commencedate.Value.ToString(Database.DateFormat) + "'");
            }
            if (optionla2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[OptionLA2] = " + ((bool)optionla2 ? "1" : "0"));
            }
            if (optionchild != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[OptionChild] = " + ((bool)optionchild ? "1" : "0"));
            }
            if (optionfuneral != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[OptionFuneral] = " + ((bool)optionfuneral ? "1" : "0"));
            }
            if (bumpupoffered != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[BumpUpOffered] = " + ((bool)bumpupoffered ? "1" : "0"));
            }
            if (totalinvoicefee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[TotalInvoiceFee] = " + totalinvoicefee + "");
            }
            if (fkinoptionfeesid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[FKINOptionFeesID] = " + fkinoptionfeesid + "");
            }
            query.Append("SELECT [INPolicy].[ID], [INPolicy].[PolicyID], [INPolicy].[FKPolicyTypeID], [INPolicy].[FKINPolicyHolderID], [INPolicy].[FKINBankDetailsID], [INPolicy].[FKINOptionID], [INPolicy].[FKINMoneyBackID], [INPolicy].[FKINBumpUpOptionID], [INPolicy].[UDMBumpUpOption], [INPolicy].[BumpUpAmount], [INPolicy].[ReducedPremiumOption], [INPolicy].[ReducedPremiumAmount], [INPolicy].[PolicyFee], [INPolicy].[TotalPremium], [INPolicy].[CommenceDate], [INPolicy].[OptionLA2], [INPolicy].[OptionChild], [INPolicy].[OptionFuneral], [INPolicy].[BumpUpOffered], [INPolicy].[TotalInvoiceFee], [INPolicy].[FKINOptionFeesID], [INPolicy].[StampDate], [INPolicy].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicy].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicy] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
