using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to calldata objects.
    /// </summary>
    internal abstract partial class INSalesNotTransferredDetailsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INSalesNotTransferredDetails calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINSalesNotTransferredDetails] ([ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID] FROM [INSalesNotTransferredDetails] WHERE [INSalesNotTransferredDetails].[FKImportID] = @ID; ";
                query += "DELETE FROM [INSalesNotTransferredDetails] WHERE [INSalesNotTransferredDetails].[FKImportID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the calldata from the database.</returns>
        internal static string DeleteHistory(INSalesNotTransferredDetails calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINSalesNotTransferredDetails] WHERE [zHstINSalesNotTransferredDetails].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to undelete.</param>
        /// <returns>A query that can be used to undelete the calldata from the database.</returns>
        internal static string UnDelete(INSalesNotTransferredDetails calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INSalesNotTransferredDetails] ([ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID] FROM [zHstINSalesNotTransferredDetails] WHERE [zHstINSalesNotTransferredDetails].[ID] = @ID AND [zHstINSalesNotTransferredDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINSalesNotTransferredDetails] WHERE [zHstINSalesNotTransferredDetails].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INSalesNotTransferredDetails] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINSalesNotTransferredDetails] WHERE [zHstINSalesNotTransferredDetails].[ID] = @ID AND [zHstINSalesNotTransferredDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINSalesNotTransferredDetails] WHERE [zHstINSalesNotTransferredDetails].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INSalesNotTransferredDetails] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an calldata object.
        /// </summary>
        /// <param name="calldata">The calldata object to fill.</param>
        /// <returns>A query that can be used to fill the calldata object.</returns>
        internal static string Fill(INSalesNotTransferredDetails calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID] FROM [INSalesNotTransferredDetails] WHERE [INSalesNotTransferredDetails].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  calldata data.
        /// </summary>
        /// <param name="calldata">The calldata to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  calldata data.</returns>
        internal static string FillData(INSalesNotTransferredDetails calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INSalesNotTransferredDetails].[ID], [INSalesNotTransferredDetails].[FKImportID], [INSalesNotTransferredDetails].[FKSalesNotTransferredReason], [INSalesNotTransferredDetails].[StampDate], [INSalesNotTransferredDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INSalesNotTransferredDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INSalesNotTransferredDetails] ");
                query.Append(" WHERE [INSalesNotTransferredDetails].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an calldata object from history.
        /// </summary>
        /// <param name="calldata">The calldata object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the calldata object from history.</returns>
        internal static string FillHistory(INSalesNotTransferredDetails calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID] FROM [zHstINSalesNotTransferredDetails] WHERE [zHstINSalesNotTransferredDetails].[ID] = @ID AND [zHstINSalesNotTransferredDetails].[StampUserID] = @StampUserID AND [zHstINSalesNotTransferredDetails].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the calldatas in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INSalesNotTransferredDetails].[ID], [INSalesNotTransferredDetails].[FKImportID], [INSalesNotTransferredDetails].[FKSalesNotTransferredReason], [INSalesNotTransferredDetails].[StampDate], [INSalesNotTransferredDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INSalesNotTransferredDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INSalesNotTransferredDetails] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINSalesNotTransferredDetails].[ID], [zHstINSalesNotTransferredDetails].[FKImportID], [zHstINSalesNotTransferredDetails].[FKSalesNotTransferredReason], [zHstINSalesNotTransferredDetails].[StampDate], [zHstINSalesNotTransferredDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINSalesNotTransferredDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINSalesNotTransferredDetails] ");
            query.Append("INNER JOIN (SELECT [zHstINSalesNotTransferredDetails].[ID], MAX([zHstINSalesNotTransferredDetails].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINSalesNotTransferredDetails] ");
            query.Append("WHERE [zHstINSalesNotTransferredDetails].[ID] NOT IN (SELECT [INSalesNotTransferredDetails].[ID] FROM [INSalesNotTransferredDetails]) ");
            query.Append("GROUP BY [zHstINSalesNotTransferredDetails].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINSalesNotTransferredDetails].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINSalesNotTransferredDetails].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INSalesNotTransferredDetails calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINSalesNotTransferredDetails].[ID], [zHstINSalesNotTransferredDetails].[FKImportID], [zHstINSalesNotTransferredDetails].[FKSalesNotTransferredReason], [zHstCallData].[StampDate], [zHstCallData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINSalesNotTransferredDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINSalesNotTransferredDetails] ");
                query.Append(" WHERE [zHstINSalesNotTransferredDetails].[ID] = @ID");
                query.Append(" ORDER BY [zHstINSalesNotTransferredDetails].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) calldata to the database.
        /// </summary>
        /// <param name="calldata">The calldata to save.</param>
        /// <returns>A query that can be used to save the calldata to the database.</returns>
        internal static string Save(INSalesNotTransferredDetails calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINSalesNotTransferredDetails] ([ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID] FROM [INSalesNotTransferredDetails] WHERE [INSalesNotTransferredDetails].[ID] = @ID; ");
                    query.Append("UPDATE [INSalesNotTransferredDetails]");
                    parameters = new object[3];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [FKSalesNotTransferredReason] = @FKSalesNotTransferredReason");
                    parameters[1] = Database.GetParameter("@FKSalesNotTransferredReason", string.IsNullOrEmpty(calldata.FKSalesNotTransferredReason) ? DBNull.Value : (object)calldata.FKSalesNotTransferredReason);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INSalesNotTransferredDetails].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INSalesNotTransferredDetails] ([FKImportID], [FKSalesNotTransferredReason], [StampDate], [StampUserID]) VALUES(@FKImportID, @FKSalesNotTransferredReason, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKSalesNotTransferredReason", string.IsNullOrEmpty(calldata.FKSalesNotTransferredReason) ? DBNull.Value : (object)calldata.FKSalesNotTransferredReason);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for calldatas that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <returns>A query that can be used to search for calldatas based on the search criteria.</returns>
        internal static string Search(long? fkimportid, string fksalesnottransferredreason)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INSalesNotTransferredDetails].[FKImportID] = " + fkimportid + "");
            }
            if (fksalesnottransferredreason != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INSalesNotTransferredDetails].[FKSalesNotTransferredReason] LIKE '" + fksalesnottransferredreason.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INSalesNotTransferredDetails].[ID], [INSalesNotTransferredDetails].[FKImportID], [INSalesNotTransferredDetails].[FKSalesNotTransferredReason], [INSalesNotTransferredDetails].[StampDate], [INSalesNotTransferredDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INSalesNotTransferredDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INSalesNotTransferredDetails] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
