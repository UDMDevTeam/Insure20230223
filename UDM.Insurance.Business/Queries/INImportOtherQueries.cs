using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportother objects.
    /// </summary>
    internal abstract partial class INImportOtherQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportother from the database.
        /// </summary>
        /// <param name="inimportother">The inimportother object to delete.</param>
        /// <returns>A query that can be used to delete the inimportother from the database.</returns>
        internal static string Delete(INImportOther inimportother, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportother != null)
            {
                query = "INSERT INTO [zHstINImportOther] ([ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID] FROM [INImportOther] WHERE [INImportOther].[ID] = @ID; ";
                query += "DELETE FROM [INImportOther] WHERE [INImportOther].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportother.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportother from the database.
        /// </summary>
        /// <param name="inimportother">The inimportother object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportother from the database.</returns>
        internal static string DeleteHistory(INImportOther inimportother, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportother != null)
            {
                query = "DELETE FROM [zHstINImportOther] WHERE [zHstINImportOther].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportother.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportother from the database.
        /// </summary>
        /// <param name="inimportother">The inimportother object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportother from the database.</returns>
        internal static string UnDelete(INImportOther inimportother, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportother != null)
            {
                query = "INSERT INTO [INImportOther] ([ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID] FROM [zHstINImportOther] WHERE [zHstINImportOther].[ID] = @ID AND [zHstINImportOther].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportOther] WHERE [zHstINImportOther].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportOther] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportOther] WHERE [zHstINImportOther].[ID] = @ID AND [zHstINImportOther].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportOther] WHERE [zHstINImportOther].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportOther] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportother.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportother object.
        /// </summary>
        /// <param name="inimportother">The inimportother object to fill.</param>
        /// <returns>A query that can be used to fill the inimportother object.</returns>
        internal static string Fill(INImportOther inimportother, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportother != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID] FROM [INImportOther] WHERE [INImportOther].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportother.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportother data.
        /// </summary>
        /// <param name="inimportother">The inimportother to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportother data.</returns>
        internal static string FillData(INImportOther inimportother, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportother != null)
            {
            query.Append("SELECT [INImportOther].[ID], [INImportOther].[FKINImportID], [INImportOther].[FKINBatchID], [INImportOther].[RefNo], [INImportOther].[AccountType], [INImportOther].[StartDate], [INImportOther].[EndDate], [INImportOther].[ReferralFrom], [INImportOther].[AddressFrom], [INImportOther].[TimesRemarketed], [INImportOther].[LastDateRemarketed], [INImportOther].[CollectedDate], [INImportOther].[CommencementDate], [INImportOther].[DurationInForce], [INImportOther].[DurationSinceOOF], [INImportOther].[NumColls], [INImportOther].[OOFDate], [INImportOther].[OOFType], [INImportOther].[UpgradeCount], [INImportOther].[Premium], [INImportOther].[Bank], [INImportOther].[Last4Digits], [INImportOther].[ExtendedSalesDate], [INImportOther].[StampDate], [INImportOther].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportOther].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportOther] ");
                query.Append(" WHERE [INImportOther].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportother.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportother object from history.
        /// </summary>
        /// <param name="inimportother">The inimportother object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportother object from history.</returns>
        internal static string FillHistory(INImportOther inimportother, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportother != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID] FROM [zHstINImportOther] WHERE [zHstINImportOther].[ID] = @ID AND [zHstINImportOther].[StampUserID] = @StampUserID AND [zHstINImportOther].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportother.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportothers in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportothers in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportOther].[ID], [INImportOther].[FKINImportID], [INImportOther].[FKINBatchID], [INImportOther].[RefNo], [INImportOther].[AccountType], [INImportOther].[StartDate], [INImportOther].[EndDate], [INImportOther].[ReferralFrom], [INImportOther].[AddressFrom], [INImportOther].[TimesRemarketed], [INImportOther].[LastDateRemarketed], [INImportOther].[CollectedDate], [INImportOther].[CommencementDate], [INImportOther].[DurationInForce], [INImportOther].[DurationSinceOOF], [INImportOther].[NumColls], [INImportOther].[OOFDate], [INImportOther].[OOFType], [INImportOther].[UpgradeCount], [INImportOther].[Premium], [INImportOther].[Bank], [INImportOther].[Last4Digits], [INImportOther].[ExtendedSalesDate], [INImportOther].[StampDate], [INImportOther].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportOther].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportOther] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportothers in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportothers in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportOther].[ID], [zHstINImportOther].[FKINImportID], [zHstINImportOther].[FKINBatchID], [zHstINImportOther].[RefNo], [zHstINImportOther].[AccountType], [zHstINImportOther].[StartDate], [zHstINImportOther].[EndDate], [zHstINImportOther].[ReferralFrom], [zHstINImportOther].[AddressFrom], [zHstINImportOther].[TimesRemarketed], [zHstINImportOther].[LastDateRemarketed], [zHstINImportOther].[CollectedDate], [zHstINImportOther].[CommencementDate], [zHstINImportOther].[DurationInForce], [zHstINImportOther].[DurationSinceOOF], [zHstINImportOther].[NumColls], [zHstINImportOther].[OOFDate], [zHstINImportOther].[OOFType], [zHstINImportOther].[UpgradeCount], [zHstINImportOther].[Premium], [zHstINImportOther].[Bank], [zHstINImportOther].[Last4Digits], [zHstINImportID].[ExtendedSalesDate], [zHstINImportOther].[StampDate], [zHstINImportOther].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportOther].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportOther] ");
            query.Append("INNER JOIN (SELECT [zHstINImportOther].[ID], MAX([zHstINImportOther].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportOther] ");
            query.Append("WHERE [zHstINImportOther].[ID] NOT IN (SELECT [INImportOther].[ID] FROM [INImportOther]) ");
            query.Append("GROUP BY [zHstINImportOther].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportOther].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportOther].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportother in the database.
        /// </summary>
        /// <param name="inimportother">The inimportother object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportother in the database.</returns>
        public static string ListHistory(INImportOther inimportother, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportother != null)
            {
            query.Append("SELECT [zHstINImportOther].[ID], [zHstINImportOther].[FKINImportID], [zHstINImportOther].[FKINBatchID], [zHstINImportOther].[RefNo], [zHstINImportOther].[AccountType], [zHstINImportOther].[StartDate], [zHstINImportOther].[EndDate], [zHstINImportOther].[ReferralFrom], [zHstINImportOther].[AddressFrom], [zHstINImportOther].[TimesRemarketed], [zHstINImportOther].[LastDateRemarketed], [zHstINImportOther].[CollectedDate], [zHstINImportOther].[CommencementDate], [zHstINImportOther].[DurationInForce], [zHstINImportOther].[DurationSinceOOF], [zHstINImportOther].[NumColls], [zHstINImportOther].[OOFDate], [zHstINImportOther].[OOFType], [zHstINImportOther].[UpgradeCount], [zHstINImportOther].[Premium], [zHstINImportOther].[Bank], [zHstINImportOther].[Last4Digits], [zHstINImportOther].[ExtendedSalesDate], [zHstINImportOther].[StampDate], [zHstINImportOther].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportOther].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportOther] ");
                query.Append(" WHERE [zHstINImportOther].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportOther].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportother.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportother to the database.
        /// </summary>
        /// <param name="inimportother">The inimportother to save.</param>
        /// <returns>A query that can be used to save the inimportother to the database.</returns>
        internal static string Save(INImportOther inimportother, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportother != null)
            {
                if (inimportother.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportOther] ([ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID] FROM [INImportOther] WHERE [INImportOther].[ID] = @ID; ");
                    query.Append("UPDATE [INImportOther]");
                    parameters = new object[23];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportother.FKINImportID.HasValue ? (object)inimportother.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKINBatchID] = @FKINBatchID");
                    parameters[1] = Database.GetParameter("@FKINBatchID", inimportother.FKINBatchID.HasValue ? (object)inimportother.FKINBatchID.Value : DBNull.Value);
                    query.Append(", [RefNo] = @RefNo");
                    parameters[2] = Database.GetParameter("@RefNo", string.IsNullOrEmpty(inimportother.RefNo) ? DBNull.Value : (object)inimportother.RefNo);
                    query.Append(", [AccountType] = @AccountType");
                    parameters[3] = Database.GetParameter("@AccountType", string.IsNullOrEmpty(inimportother.AccountType) ? DBNull.Value : (object)inimportother.AccountType);
                    query.Append(", [StartDate] = @StartDate");
                    parameters[4] = Database.GetParameter("@StartDate", inimportother.StartDate.HasValue ? (object)inimportother.StartDate.Value : DBNull.Value);
                    query.Append(", [EndDate] = @EndDate");
                    parameters[5] = Database.GetParameter("@EndDate", inimportother.EndDate.HasValue ? (object)inimportother.EndDate.Value : DBNull.Value);
                    query.Append(", [ReferralFrom] = @ReferralFrom");
                    parameters[6] = Database.GetParameter("@ReferralFrom", string.IsNullOrEmpty(inimportother.ReferralFrom) ? DBNull.Value : (object)inimportother.ReferralFrom);
                    query.Append(", [AddressFrom] = @AddressFrom");
                    parameters[7] = Database.GetParameter("@AddressFrom", string.IsNullOrEmpty(inimportother.AddressFrom) ? DBNull.Value : (object)inimportother.AddressFrom);
                    query.Append(", [TimesRemarketed] = @TimesRemarketed");
                    parameters[8] = Database.GetParameter("@TimesRemarketed", inimportother.TimesRemarketed.HasValue ? (object)inimportother.TimesRemarketed.Value : DBNull.Value);
                    query.Append(", [LastDateRemarketed] = @LastDateRemarketed");
                    parameters[9] = Database.GetParameter("@LastDateRemarketed", inimportother.LastDateRemarketed.HasValue ? (object)inimportother.LastDateRemarketed.Value : DBNull.Value);
                    query.Append(", [CollectedDate] = @CollectedDate");
                    parameters[10] = Database.GetParameter("@CollectedDate", inimportother.CollectedDate.HasValue ? (object)inimportother.CollectedDate.Value : DBNull.Value);
                    query.Append(", [CommencementDate] = @CommencementDate");
                    parameters[11] = Database.GetParameter("@CommencementDate", inimportother.CommencementDate.HasValue ? (object)inimportother.CommencementDate.Value : DBNull.Value);
                    query.Append(", [DurationInForce] = @DurationInForce");
                    parameters[12] = Database.GetParameter("@DurationInForce", inimportother.DurationInForce.HasValue ? (object)inimportother.DurationInForce.Value : DBNull.Value);
                    query.Append(", [DurationSinceOOF] = @DurationSinceOOF");
                    parameters[13] = Database.GetParameter("@DurationSinceOOF", inimportother.DurationSinceOOF.HasValue ? (object)inimportother.DurationSinceOOF.Value : DBNull.Value);
                    query.Append(", [NumColls] = @NumColls");
                    parameters[14] = Database.GetParameter("@NumColls", inimportother.NumColls.HasValue ? (object)inimportother.NumColls.Value : DBNull.Value);
                    query.Append(", [OOFDate] = @OOFDate");
                    parameters[15] = Database.GetParameter("@OOFDate", inimportother.OOFDate.HasValue ? (object)inimportother.OOFDate.Value : DBNull.Value);
                    query.Append(", [OOFType] = @OOFType");
                    parameters[16] = Database.GetParameter("@OOFType", string.IsNullOrEmpty(inimportother.OOFType) ? DBNull.Value : (object)inimportother.OOFType);
                    query.Append(", [UpgradeCount] = @UpgradeCount");
                    parameters[17] = Database.GetParameter("@UpgradeCount", inimportother.UpgradeCount.HasValue ? (object)inimportother.UpgradeCount.Value : DBNull.Value);
                    query.Append(", [Premium] = @Premium");
                    parameters[18] = Database.GetParameter("@Premium", inimportother.Premium.HasValue ? (object)inimportother.Premium.Value : DBNull.Value);
                    query.Append(", [Bank] = @Bank");
                    parameters[19] = Database.GetParameter("@Bank", string.IsNullOrEmpty(inimportother.Bank) ? DBNull.Value : (object)inimportother.Bank);
                    query.Append(", [Last4Digits] = @Last4Digits");
                    parameters[20] = Database.GetParameter("@Last4Digits", string.IsNullOrEmpty(inimportother.Last4Digits) ? DBNull.Value : (object)inimportother.Last4Digits);
                    query.Append(", [ExtendedSalesDate] = @ExtendedSalesDate");
                    parameters[21] = Database.GetParameter("@ExtendedSalesDate", inimportother.ExtendedSalesDate.HasValue ? (object)inimportother.ExtendedSalesDate.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportOther].[ID] = @ID"); 
                    parameters[22] = Database.GetParameter("@ID", inimportother.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportOther] ([FKINImportID], [FKINBatchID], [RefNo], [AccountType], [StartDate], [EndDate], [ReferralFrom], [AddressFrom], [TimesRemarketed], [LastDateRemarketed], [CollectedDate], [CommencementDate], [DurationInForce], [DurationSinceOOF], [NumColls], [OOFDate], [OOFType], [UpgradeCount], [Premium], [Bank], [Last4Digits], [ExtendedSalesDate], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKINBatchID, @RefNo, @AccountType, @StartDate, @EndDate, @ReferralFrom, @AddressFrom, @TimesRemarketed, @LastDateRemarketed, @CollectedDate, @CommencementDate, @DurationInForce, @DurationSinceOOF, @NumColls, @OOFDate, @OOFType, @UpgradeCount, @Premium, @Bank, @Last4Digits, @ExtendedSalesdate, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[22];
                    parameters[0] = Database.GetParameter("@FKINImportID", inimportother.FKINImportID.HasValue ? (object)inimportother.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINBatchID", inimportother.FKINBatchID.HasValue ? (object)inimportother.FKINBatchID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@RefNo", string.IsNullOrEmpty(inimportother.RefNo) ? DBNull.Value : (object)inimportother.RefNo);
                    parameters[3] = Database.GetParameter("@AccountType", string.IsNullOrEmpty(inimportother.AccountType) ? DBNull.Value : (object)inimportother.AccountType);
                    parameters[4] = Database.GetParameter("@StartDate", inimportother.StartDate.HasValue ? (object)inimportother.StartDate.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@EndDate", inimportother.EndDate.HasValue ? (object)inimportother.EndDate.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@ReferralFrom", string.IsNullOrEmpty(inimportother.ReferralFrom) ? DBNull.Value : (object)inimportother.ReferralFrom);
                    parameters[7] = Database.GetParameter("@AddressFrom", string.IsNullOrEmpty(inimportother.AddressFrom) ? DBNull.Value : (object)inimportother.AddressFrom);
                    parameters[8] = Database.GetParameter("@TimesRemarketed", inimportother.TimesRemarketed.HasValue ? (object)inimportother.TimesRemarketed.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@LastDateRemarketed", inimportother.LastDateRemarketed.HasValue ? (object)inimportother.LastDateRemarketed.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@CollectedDate", inimportother.CollectedDate.HasValue ? (object)inimportother.CollectedDate.Value : DBNull.Value);
                    parameters[11] = Database.GetParameter("@CommencementDate", inimportother.CommencementDate.HasValue ? (object)inimportother.CommencementDate.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@DurationInForce", inimportother.DurationInForce.HasValue ? (object)inimportother.DurationInForce.Value : DBNull.Value);
                    parameters[13] = Database.GetParameter("@DurationSinceOOF", inimportother.DurationSinceOOF.HasValue ? (object)inimportother.DurationSinceOOF.Value : DBNull.Value);
                    parameters[14] = Database.GetParameter("@NumColls", inimportother.NumColls.HasValue ? (object)inimportother.NumColls.Value : DBNull.Value);
                    parameters[15] = Database.GetParameter("@OOFDate", inimportother.OOFDate.HasValue ? (object)inimportother.OOFDate.Value : DBNull.Value);
                    parameters[16] = Database.GetParameter("@OOFType", string.IsNullOrEmpty(inimportother.OOFType) ? DBNull.Value : (object)inimportother.OOFType);
                    parameters[17] = Database.GetParameter("@UpgradeCount", inimportother.UpgradeCount.HasValue ? (object)inimportother.UpgradeCount.Value : DBNull.Value);
                    parameters[18] = Database.GetParameter("@Premium", inimportother.Premium.HasValue ? (object)inimportother.Premium.Value : DBNull.Value);
                    parameters[19] = Database.GetParameter("@Bank", string.IsNullOrEmpty(inimportother.Bank) ? DBNull.Value : (object)inimportother.Bank);
                    parameters[20] = Database.GetParameter("@Last4Digits", string.IsNullOrEmpty(inimportother.Last4Digits) ? DBNull.Value : (object)inimportother.Last4Digits);
                    parameters[21] = Database.GetParameter("@ExtendedSalesDate", inimportother.ExtendedSalesDate.HasValue ? (object)inimportother.ExtendedSalesDate.Value : DBNull.Value);

                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportothers that match the search criteria.
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
        /// <returns>A query that can be used to search for inimportothers based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, long? fkinbatchid, string refno, string accounttype, DateTime? startdate, DateTime? enddate, string referralfrom, string addressfrom, short? timesremarketed, DateTime? lastdateremarketed, DateTime? collecteddate, DateTime? commencementdate, int? durationinforce, int? durationsinceoof, int? numcolls, DateTime? oofdate, string ooftype, int? upgradecount, decimal? premium, string bank, string last4digits, DateTime? extendedsalesdate)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[FKINBatchID] = " + fkinbatchid + "");
            }
            if (refno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[RefNo] LIKE '" + refno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (accounttype != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[AccountType] LIKE '" + accounttype.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (startdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[StartDate] = '" + startdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (enddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[EndDate] = '" + enddate.Value.ToString(Database.DateFormat) + "'");
            }
            if (referralfrom != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[ReferralFrom] LIKE '" + referralfrom.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (addressfrom != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[AddressFrom] LIKE '" + addressfrom.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (timesremarketed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[TimesRemarketed] = " + timesremarketed + "");
            }
            if (lastdateremarketed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[LastDateRemarketed] = '" + lastdateremarketed.Value.ToString(Database.DateFormat) + "'");
            }
            if (collecteddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[CollectedDate] = '" + collecteddate.Value.ToString(Database.DateFormat) + "'");
            }
            if (commencementdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[CommencementDate] = '" + commencementdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (durationinforce != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[DurationInForce] = " + durationinforce + "");
            }
            if (durationsinceoof != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[DurationSinceOOF] = " + durationsinceoof + "");
            }
            if (numcolls != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[NumColls] = " + numcolls + "");
            }
            if (oofdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[OOFDate] = '" + oofdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (ooftype != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[OOFType] LIKE '" + ooftype.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (upgradecount != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[UpgradeCount] = " + upgradecount + "");
            }
            if (premium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[Premium] = " + premium + "");
            }
            if (bank != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[Bank] LIKE '" + bank.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (last4digits != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[Last4Digits] LIKE '" + last4digits.Replace("'", "''").Replace("*", "%") + "'");

            }
            if (extendedsalesdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportOther].[ExtendedSalesDate] = " + extendedsalesdate + "");
            }
            query.Append("SELECT [INImportOther].[ID], [INImportOther].[FKINImportID], [INImportOther].[FKINBatchID], [INImportOther].[RefNo], [INImportOther].[AccountType], [INImportOther].[StartDate], [INImportOther].[EndDate], [INImportOther].[ReferralFrom], [INImportOther].[AddressFrom], [INImportOther].[TimesRemarketed], [INImportOther].[LastDateRemarketed], [INImportOther].[CollectedDate], [INImportOther].[CommencementDate], [INImportOther].[DurationInForce], [INImportOther].[DurationSinceOOF], [INImportOther].[NumColls], [INImportOther].[OOFDate], [INImportOther].[OOFType], [INImportOther].[UpgradeCount], [INImportOther].[Premium], [INImportOther].[Bank], [INImportOther].[Last4Digits], [INImportOther].[ExtendedSalesDate], [INImportOther].[StampDate], [INImportOther].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportOther].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportOther] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
