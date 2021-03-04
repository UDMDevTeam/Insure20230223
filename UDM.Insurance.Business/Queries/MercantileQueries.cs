using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to mercantile objects.
    /// </summary>
    internal abstract partial class MercantileQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) mercantile from the database.
        /// </summary>
        /// <param name="mercantile">The mercantile object to delete.</param>
        /// <returns>A query that can be used to delete the mercantile from the database.</returns>
        internal static string Delete(Mercantile mercantile, ref object[] parameters)
        {
            string query = string.Empty;
            if (mercantile != null)
            {
                query = "INSERT INTO [zHstMercantile] ([ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [Mercantile] WHERE [Mercantile].[ID] = @ID; ";
                query += "DELETE FROM [Mercantile] WHERE [Mercantile].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", mercantile.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) mercantile from the database.
        /// </summary>
        /// <param name="mercantile">The mercantile object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the mercantile from the database.</returns>
        internal static string DeleteHistory(Mercantile mercantile, ref object[] parameters)
        {
            string query = string.Empty;
            if (mercantile != null)
            {
                query = "DELETE FROM [zHstMercantile] WHERE [zHstMercantile].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", mercantile.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) mercantile from the database.
        /// </summary>
        /// <param name="mercantile">The mercantile object to undelete.</param>
        /// <returns>A query that can be used to undelete the mercantile from the database.</returns>
        internal static string UnDelete(Mercantile mercantile, ref object[] parameters)
        {
            string query = string.Empty;
            if (mercantile != null)
            {
                query = "INSERT INTO [Mercantile] ([ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [zHstMercantile] WHERE [zHstMercantile].[ID] = @ID AND [zHstMercantile].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstMercantile] WHERE [zHstMercantile].[ID] = @ID) AND (SELECT COUNT(ID) FROM [Mercantile] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstMercantile] WHERE [zHstMercantile].[ID] = @ID AND [zHstMercantile].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstMercantile] WHERE [zHstMercantile].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [Mercantile] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", mercantile.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an mercantile object.
        /// </summary>
        /// <param name="mercantile">The mercantile object to fill.</param>
        /// <returns>A query that can be used to fill the mercantile object.</returns>
        internal static string Fill(Mercantile mercantile, ref object[] parameters)
        {
            string query = string.Empty;
            if (mercantile != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [Mercantile] WHERE [Mercantile].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", mercantile.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  mercantile data.
        /// </summary>
        /// <param name="mercantile">The mercantile to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  mercantile data.</returns>
        internal static string FillData(Mercantile mercantile, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (mercantile != null)
            {
            query.Append("SELECT [Mercantile].[ID], [Mercantile].[FKSystemID], [Mercantile].[FKImportID], [Mercantile].[FKBankID], [Mercantile].[FKBankBranchID], [Mercantile].[AccountNumber], [Mercantile].[AccNumCheckStatus], [Mercantile].[AccNumCheckMsg], [Mercantile].[AccNumCheckMsgFull], [Mercantile].[StampDate], [Mercantile].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Mercantile].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Mercantile] ");
                query.Append(" WHERE [Mercantile].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", mercantile.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an mercantile object from history.
        /// </summary>
        /// <param name="mercantile">The mercantile object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the mercantile object from history.</returns>
        internal static string FillHistory(Mercantile mercantile, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (mercantile != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [zHstMercantile] WHERE [zHstMercantile].[ID] = @ID AND [zHstMercantile].[StampUserID] = @StampUserID AND [zHstMercantile].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", mercantile.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the mercantiles in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the mercantiles in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [Mercantile].[ID], [Mercantile].[FKSystemID], [Mercantile].[FKImportID], [Mercantile].[FKBankID], [Mercantile].[FKBankBranchID], [Mercantile].[AccountNumber], [Mercantile].[AccNumCheckStatus], [Mercantile].[AccNumCheckMsg], [Mercantile].[AccNumCheckMsgFull], [Mercantile].[StampDate], [Mercantile].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Mercantile].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Mercantile] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted mercantiles in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted mercantiles in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstMercantile].[ID], [zHstMercantile].[FKSystemID], [zHstMercantile].[FKImportID], [zHstMercantile].[FKBankID], [zHstMercantile].[FKBankBranchID], [zHstMercantile].[AccountNumber], [zHstMercantile].[AccNumCheckStatus], [zHstMercantile].[AccNumCheckMsg], [zHstMercantile].[AccNumCheckMsgFull], [zHstMercantile].[StampDate], [zHstMercantile].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstMercantile].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstMercantile] ");
            query.Append("INNER JOIN (SELECT [zHstMercantile].[ID], MAX([zHstMercantile].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstMercantile] ");
            query.Append("WHERE [zHstMercantile].[ID] NOT IN (SELECT [Mercantile].[ID] FROM [Mercantile]) ");
            query.Append("GROUP BY [zHstMercantile].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstMercantile].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstMercantile].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) mercantile in the database.
        /// </summary>
        /// <param name="mercantile">The mercantile object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) mercantile in the database.</returns>
        public static string ListHistory(Mercantile mercantile, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (mercantile != null)
            {
            query.Append("SELECT [zHstMercantile].[ID], [zHstMercantile].[FKSystemID], [zHstMercantile].[FKImportID], [zHstMercantile].[FKBankID], [zHstMercantile].[FKBankBranchID], [zHstMercantile].[AccountNumber], [zHstMercantile].[AccNumCheckStatus], [zHstMercantile].[AccNumCheckMsg], [zHstMercantile].[AccNumCheckMsgFull], [zHstMercantile].[StampDate], [zHstMercantile].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstMercantile].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstMercantile] ");
                query.Append(" WHERE [zHstMercantile].[ID] = @ID");
                query.Append(" ORDER BY [zHstMercantile].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", mercantile.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) mercantile to the database.
        /// </summary>
        /// <param name="mercantile">The mercantile to save.</param>
        /// <returns>A query that can be used to save the mercantile to the database.</returns>
        internal static string Save(Mercantile mercantile, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (mercantile != null)
            {
                if (mercantile.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstMercantile] ([ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID] FROM [Mercantile] WHERE [Mercantile].[ID] = @ID; ");
                    query.Append("UPDATE [Mercantile]");
                    parameters = new object[9];
                    query.Append(" SET [FKSystemID] = @FKSystemID");
                    parameters[0] = Database.GetParameter("@FKSystemID", mercantile.FKSystemID.HasValue ? (object)mercantile.FKSystemID.Value : DBNull.Value);
                    query.Append(", [FKImportID] = @FKImportID");
                    parameters[1] = Database.GetParameter("@FKImportID", mercantile.FKImportID.HasValue ? (object)mercantile.FKImportID.Value : DBNull.Value);
                    query.Append(", [FKBankID] = @FKBankID");
                    parameters[2] = Database.GetParameter("@FKBankID", mercantile.FKBankID.HasValue ? (object)mercantile.FKBankID.Value : DBNull.Value);
                    query.Append(", [FKBankBranchID] = @FKBankBranchID");
                    parameters[3] = Database.GetParameter("@FKBankBranchID", mercantile.FKBankBranchID.HasValue ? (object)mercantile.FKBankBranchID.Value : DBNull.Value);
                    query.Append(", [AccountNumber] = @AccountNumber");
                    parameters[4] = Database.GetParameter("@AccountNumber", string.IsNullOrEmpty(mercantile.AccountNumber) ? DBNull.Value : (object)mercantile.AccountNumber);
                    query.Append(", [AccNumCheckStatus] = @AccNumCheckStatus");
                    parameters[5] = Database.GetParameter("@AccNumCheckStatus", mercantile.AccNumCheckStatus.HasValue ? (object)mercantile.AccNumCheckStatus.Value : DBNull.Value);
                    query.Append(", [AccNumCheckMsg] = @AccNumCheckMsg");
                    parameters[6] = Database.GetParameter("@AccNumCheckMsg", string.IsNullOrEmpty(mercantile.AccNumCheckMsg) ? DBNull.Value : (object)mercantile.AccNumCheckMsg);
                    query.Append(", [AccNumCheckMsgFull] = @AccNumCheckMsgFull");
                    parameters[7] = Database.GetParameter("@AccNumCheckMsgFull", string.IsNullOrEmpty(mercantile.AccNumCheckMsgFull) ? DBNull.Value : (object)mercantile.AccNumCheckMsgFull);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [Mercantile].[ID] = @ID"); 
                    parameters[8] = Database.GetParameter("@ID", mercantile.ID);
                }
                else
                {
                    query.Append("INSERT INTO [Mercantile] ([FKSystemID], [FKImportID], [FKBankID], [FKBankBranchID], [AccountNumber], [AccNumCheckStatus], [AccNumCheckMsg], [AccNumCheckMsgFull], [StampDate], [StampUserID]) VALUES(@FKSystemID, @FKImportID, @FKBankID, @FKBankBranchID, @AccountNumber, @AccNumCheckStatus, @AccNumCheckMsg, @AccNumCheckMsgFull, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[8];
                    parameters[0] = Database.GetParameter("@FKSystemID", mercantile.FKSystemID.HasValue ? (object)mercantile.FKSystemID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKImportID", mercantile.FKImportID.HasValue ? (object)mercantile.FKImportID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKBankID", mercantile.FKBankID.HasValue ? (object)mercantile.FKBankID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKBankBranchID", mercantile.FKBankBranchID.HasValue ? (object)mercantile.FKBankBranchID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@AccountNumber", string.IsNullOrEmpty(mercantile.AccountNumber) ? DBNull.Value : (object)mercantile.AccountNumber);
                    parameters[5] = Database.GetParameter("@AccNumCheckStatus", mercantile.AccNumCheckStatus.HasValue ? (object)mercantile.AccNumCheckStatus.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@AccNumCheckMsg", string.IsNullOrEmpty(mercantile.AccNumCheckMsg) ? DBNull.Value : (object)mercantile.AccNumCheckMsg);
                    parameters[7] = Database.GetParameter("@AccNumCheckMsgFull", string.IsNullOrEmpty(mercantile.AccNumCheckMsgFull) ? DBNull.Value : (object)mercantile.AccNumCheckMsgFull);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for mercantiles that match the search criteria.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="fkbankbranchid">The fkbankbranchid search criteria.</param>
        /// <param name="accountnumber">The accountnumber search criteria.</param>
        /// <param name="accnumcheckstatus">The accnumcheckstatus search criteria.</param>
        /// <param name="accnumcheckmsg">The accnumcheckmsg search criteria.</param>
        /// <param name="accnumcheckmsgfull">The accnumcheckmsgfull search criteria.</param>
        /// <returns>A query that can be used to search for mercantiles based on the search criteria.</returns>
        internal static string Search(long? fksystemid, long? fkimportid, long? fkbankid, long? fkbankbranchid, string accountnumber, Byte? accnumcheckstatus, string accnumcheckmsg, string accnumcheckmsgfull)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fksystemid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[FKSystemID] = " + fksystemid + "");
            }
            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[FKImportID] = " + fkimportid + "");
            }
            if (fkbankid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[FKBankID] = " + fkbankid + "");
            }
            if (fkbankbranchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[FKBankBranchID] = " + fkbankbranchid + "");
            }
            if (accountnumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[AccountNumber] LIKE '" + accountnumber.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (accnumcheckstatus != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[AccNumCheckStatus] = " + accnumcheckstatus + "");
            }
            if (accnumcheckmsg != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[AccNumCheckMsg] LIKE '" + accnumcheckmsg.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (accnumcheckmsgfull != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Mercantile].[AccNumCheckMsgFull] LIKE '" + accnumcheckmsgfull.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [Mercantile].[ID], [Mercantile].[FKSystemID], [Mercantile].[FKImportID], [Mercantile].[FKBankID], [Mercantile].[FKBankBranchID], [Mercantile].[AccountNumber], [Mercantile].[AccNumCheckStatus], [Mercantile].[AccNumCheckMsg], [Mercantile].[AccNumCheckMsgFull], [Mercantile].[StampDate], [Mercantile].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Mercantile].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Mercantile] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
