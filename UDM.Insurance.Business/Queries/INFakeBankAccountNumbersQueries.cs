using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to infakebankaccountnumbers objects.
    /// </summary>
    internal abstract partial class INFakeBankAccountNumbersQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) infakebankaccountnumbers from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers object to delete.</param>
        /// <returns>A query that can be used to delete the infakebankaccountnumbers from the database.</returns>
        internal static string Delete(INFakeBankAccountNumbers infakebankaccountnumbers, ref object[] parameters)
        {
            string query = string.Empty;
            if (infakebankaccountnumbers != null)
            {
                query = "INSERT INTO [zHstINFakeBankAccountNumbers] ([ID], [FKBankID], [AccNo], [StampDate], [StampUserID]) SELECT [ID], [FKBankID], [AccNo], [StampDate], [StampUserID] FROM [INFakeBankAccountNumbers] WHERE [INFakeBankAccountNumbers].[ID] = @ID; ";
                query += "DELETE FROM [INFakeBankAccountNumbers] WHERE [INFakeBankAccountNumbers].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) infakebankaccountnumbers from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the infakebankaccountnumbers from the database.</returns>
        internal static string DeleteHistory(INFakeBankAccountNumbers infakebankaccountnumbers, ref object[] parameters)
        {
            string query = string.Empty;
            if (infakebankaccountnumbers != null)
            {
                query = "DELETE FROM [zHstINFakeBankAccountNumbers] WHERE [zHstINFakeBankAccountNumbers].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) infakebankaccountnumbers from the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers object to undelete.</param>
        /// <returns>A query that can be used to undelete the infakebankaccountnumbers from the database.</returns>
        internal static string UnDelete(INFakeBankAccountNumbers infakebankaccountnumbers, ref object[] parameters)
        {
            string query = string.Empty;
            if (infakebankaccountnumbers != null)
            {
                query = "INSERT INTO [INFakeBankAccountNumbers] ([ID], [FKBankID], [AccNo], [StampDate], [StampUserID]) SELECT [ID], [FKBankID], [AccNo], [StampDate], [StampUserID] FROM [zHstINFakeBankAccountNumbers] WHERE [zHstINFakeBankAccountNumbers].[ID] = @ID AND [zHstINFakeBankAccountNumbers].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINFakeBankAccountNumbers] WHERE [zHstINFakeBankAccountNumbers].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INFakeBankAccountNumbers] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINFakeBankAccountNumbers] WHERE [zHstINFakeBankAccountNumbers].[ID] = @ID AND [zHstINFakeBankAccountNumbers].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINFakeBankAccountNumbers] WHERE [zHstINFakeBankAccountNumbers].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INFakeBankAccountNumbers] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an infakebankaccountnumbers object.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers object to fill.</param>
        /// <returns>A query that can be used to fill the infakebankaccountnumbers object.</returns>
        internal static string Fill(INFakeBankAccountNumbers infakebankaccountnumbers, ref object[] parameters)
        {
            string query = string.Empty;
            if (infakebankaccountnumbers != null)
            {
                query = "SELECT [ID], [FKBankID], [AccNo], [StampDate], [StampUserID] FROM [INFakeBankAccountNumbers] WHERE [INFakeBankAccountNumbers].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  infakebankaccountnumbers data.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  infakebankaccountnumbers data.</returns>
        internal static string FillData(INFakeBankAccountNumbers infakebankaccountnumbers, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infakebankaccountnumbers != null)
            {
            query.Append("SELECT [INFakeBankAccountNumbers].[ID], [INFakeBankAccountNumbers].[FKBankID], [INFakeBankAccountNumbers].[AccNo], [INFakeBankAccountNumbers].[StampDate], [INFakeBankAccountNumbers].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFakeBankAccountNumbers].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFakeBankAccountNumbers] ");
                query.Append(" WHERE [INFakeBankAccountNumbers].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an infakebankaccountnumbers object from history.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the infakebankaccountnumbers object from history.</returns>
        internal static string FillHistory(INFakeBankAccountNumbers infakebankaccountnumbers, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (infakebankaccountnumbers != null)
            {
                query = "SELECT [ID], [FKBankID], [AccNo], [StampDate], [StampUserID] FROM [zHstINFakeBankAccountNumbers] WHERE [zHstINFakeBankAccountNumbers].[ID] = @ID AND [zHstINFakeBankAccountNumbers].[StampUserID] = @StampUserID AND [zHstINFakeBankAccountNumbers].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the infakebankaccountnumberss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the infakebankaccountnumberss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INFakeBankAccountNumbers].[ID], [INFakeBankAccountNumbers].[FKBankID], [INFakeBankAccountNumbers].[AccNo], [INFakeBankAccountNumbers].[StampDate], [INFakeBankAccountNumbers].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFakeBankAccountNumbers].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFakeBankAccountNumbers] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted infakebankaccountnumberss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted infakebankaccountnumberss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINFakeBankAccountNumbers].[ID], [zHstINFakeBankAccountNumbers].[FKBankID], [zHstINFakeBankAccountNumbers].[AccNo], [zHstINFakeBankAccountNumbers].[StampDate], [zHstINFakeBankAccountNumbers].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINFakeBankAccountNumbers].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINFakeBankAccountNumbers] ");
            query.Append("INNER JOIN (SELECT [zHstINFakeBankAccountNumbers].[ID], MAX([zHstINFakeBankAccountNumbers].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINFakeBankAccountNumbers] ");
            query.Append("WHERE [zHstINFakeBankAccountNumbers].[ID] NOT IN (SELECT [INFakeBankAccountNumbers].[ID] FROM [INFakeBankAccountNumbers]) ");
            query.Append("GROUP BY [zHstINFakeBankAccountNumbers].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINFakeBankAccountNumbers].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINFakeBankAccountNumbers].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) infakebankaccountnumbers in the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) infakebankaccountnumbers in the database.</returns>
        public static string ListHistory(INFakeBankAccountNumbers infakebankaccountnumbers, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infakebankaccountnumbers != null)
            {
            query.Append("SELECT [zHstINFakeBankAccountNumbers].[ID], [zHstINFakeBankAccountNumbers].[FKBankID], [zHstINFakeBankAccountNumbers].[AccNo], [zHstINFakeBankAccountNumbers].[StampDate], [zHstINFakeBankAccountNumbers].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINFakeBankAccountNumbers].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINFakeBankAccountNumbers] ");
                query.Append(" WHERE [zHstINFakeBankAccountNumbers].[ID] = @ID");
                query.Append(" ORDER BY [zHstINFakeBankAccountNumbers].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) infakebankaccountnumbers to the database.
        /// </summary>
        /// <param name="infakebankaccountnumbers">The infakebankaccountnumbers to save.</param>
        /// <returns>A query that can be used to save the infakebankaccountnumbers to the database.</returns>
        internal static string Save(INFakeBankAccountNumbers infakebankaccountnumbers, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infakebankaccountnumbers != null)
            {
                if (infakebankaccountnumbers.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINFakeBankAccountNumbers] ([ID], [FKBankID], [AccNo], [StampDate], [StampUserID]) SELECT [ID], [FKBankID], [AccNo], [StampDate], [StampUserID] FROM [INFakeBankAccountNumbers] WHERE [INFakeBankAccountNumbers].[ID] = @ID; ");
                    query.Append("UPDATE [INFakeBankAccountNumbers]");
                    parameters = new object[3];
                    query.Append(" SET [FKBankID] = @FKBankID");
                    parameters[0] = Database.GetParameter("@FKBankID", infakebankaccountnumbers.FKBankID.HasValue ? (object)infakebankaccountnumbers.FKBankID.Value : DBNull.Value);
                    query.Append(", [AccNo] = @AccNo");
                    parameters[1] = Database.GetParameter("@AccNo", string.IsNullOrEmpty(infakebankaccountnumbers.AccNo) ? DBNull.Value : (object)infakebankaccountnumbers.AccNo);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INFakeBankAccountNumbers].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", infakebankaccountnumbers.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INFakeBankAccountNumbers] ([FKBankID], [AccNo], [StampDate], [StampUserID]) VALUES(@FKBankID, @AccNo, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKBankID", infakebankaccountnumbers.FKBankID.HasValue ? (object)infakebankaccountnumbers.FKBankID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@AccNo", string.IsNullOrEmpty(infakebankaccountnumbers.AccNo) ? DBNull.Value : (object)infakebankaccountnumbers.AccNo);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for infakebankaccountnumberss that match the search criteria.
        /// </summary>
        /// <param name="fkbankid">The fkbankid search criteria.</param>
        /// <param name="accno">The accno search criteria.</param>
        /// <returns>A query that can be used to search for infakebankaccountnumberss based on the search criteria.</returns>
        internal static string Search(long? fkbankid, string accno)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkbankid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFakeBankAccountNumbers].[FKBankID] = " + fkbankid + "");
            }
            if (accno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFakeBankAccountNumbers].[AccNo] LIKE '" + accno.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INFakeBankAccountNumbers].[ID], [INFakeBankAccountNumbers].[FKBankID], [INFakeBankAccountNumbers].[AccNo], [INFakeBankAccountNumbers].[StampDate], [INFakeBankAccountNumbers].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFakeBankAccountNumbers].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFakeBankAccountNumbers] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
