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
    internal abstract partial class INCongratulationsPopUpQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INCongratulationsPopUp calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINCongratulationsPopUp] ([ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) SELECT [ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [INCongratulationsPopUp] WHERE [INCongratulationsPopUp].[ID] = @ID; ";
                query += "DELETE FROM [INCongratulationsPopUp] WHERE [INCongratulationsPopUp].[ID] = @ID; ";
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
        internal static string DeleteHistory(INCongratulationsPopUp calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINCongratulationsPopUp] WHERE [zHstINCongratulationsPopUp].[ID] = @ID";
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
        internal static string UnDelete(INCongratulationsPopUp calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INCongratulationsPopUp] ([ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) SELECT [ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [zHstINCongratulationsPopUp] WHERE [zHstINCongratulationsPopUp].[ID] = @ID AND [zHstINCongratulationsPopUp].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCongratulationsPopUp] WHERE [zHstINCongratulationsPopUp].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INCongratulationsPopUp] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINCongratulationsPopUp] WHERE [zHstINCongratulationsPopUp].[ID] = @ID AND [zHstINCongratulationsPopUp].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCongratulationsPopUp] WHERE [zHstINCongratulationsPopUp].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INCongratulationsPopUp] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INCongratulationsPopUp calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [INCongratulationsPopUp] WHERE [INCongratulationsPopUp].[ID] = @ID";
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
        internal static string FillData(INCongratulationsPopUp calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INCongratulationsPopUp].[ID], [INCongratulationsPopUp].[FKBatchID], [INCongratulationsPopUp].[FKUserID], [INCongratulationsPopUp].[IsDisplayed], [INCongratulationsPopUp].[StampDate], [INCongratulationsPopUp].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCongratulationsPopUp].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCongratulationsPopUp] ");
                query.Append(" WHERE [INCongratulationsPopUp].[ID] = @ID");
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
        internal static string FillHistory(INCongratulationsPopUp calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [zHstINCongratulationsPopUp] WHERE [zHstINCongratulationsPopUp].[ID] = @ID AND [zHstINCongratulationsPopUp].[StampUserID] = @StampUserID AND [zHstINCongratulationsPopUp].[StampDate] = @StampDate";
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
            query.Append("SELECT [INCongratulationsPopUp].[ID], [INCongratulationsPopUp].[FKBatchID], [INCongratulationsPopUp].[FKUserID], [INCongratulationsPopUp].[IsDisplayed], [INCongratulationsPopUp].[StampDate], [INCongratulationsPopUp].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCongratulationsPopUp].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCongratulationsPopUp] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINCongratulationsPopUp].[ID], [zHstINCongratulationsPopUp].[FKBatchID], [zHstINCongratulationsPopUp].[FKUserID], [zHstINCongratulationsPopUp].[IsDisplayed], [zHstINCongratulationsPopUp].[StampDate], [zHstINCongratulationsPopUp].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINCongratulationsPopUp].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINCongratulationsPopUp] ");
            query.Append("INNER JOIN (SELECT [zHstINCongratulationsPopUp].[ID], MAX([zHstINCongratulationsPopUp].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINCongratulationsPopUp] ");
            query.Append("WHERE [zHstINCongratulationsPopUp].[ID] NOT IN (SELECT [INCongratulationsPopUp].[ID] FROM [INCongratulationsPopUp]) ");
            query.Append("GROUP BY [zHstINCongratulationsPopUp].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINCongratulationsPopUp].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINCongratulationsPopUp].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INCongratulationsPopUp calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINCongratulationsPopUp].[ID], [zHstINCongratulationsPopUp].[FKBatchID], [zHstINCongratulationsPopUp].[FKUserID], [zHstINCongratulationsPopUp].[IsDisplayed], [zHstINCongratulationsPopUp].[StampDate], [zHstINCongratulationsPopUp].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINCongratulationsPopUp].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINCongratulationsPopUp] ");
                query.Append(" WHERE [zHstINCongratulationsPopUp].[ID] = @ID");
                query.Append(" ORDER BY [zHstINCongratulationsPopUp].[StampDate] DESC");
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
        internal static string Save(INCongratulationsPopUp calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINCongratulationsPopUp] ([ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) SELECT [ID], [FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID] FROM [INCongratulationsPopUp] WHERE [INCongratulationsPopUp].[ID] = @ID; ");
                    query.Append("UPDATE [INCongratulationsPopUp]");
                    parameters = new object[4];
                    query.Append(" SET [FKBatchID] = @FKBatchID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKBatchID.HasValue ? (object)calldata.FKBatchID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKBatchID.Value : DBNull.Value);
                    query.Append(", [IsDisplayed] = @IsDisplayed");
                    parameters[2] = Database.GetParameter("@IsDisplayed", string.IsNullOrEmpty(calldata.IsDisplayed) ? DBNull.Value : (object)calldata.IsDisplayed);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [CallData].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INCongratulationsPopUp] ([FKBatchID], [FKUserID], [IsDisplayed], [StampDate], [StampUserID]) VALUES(@FKBatchID, @FKUserID, @IsDisplayed, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@FKbatchID", calldata.FKBatchID.HasValue ? (object)calldata.FKBatchID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@IsDisplayed", string.IsNullOrEmpty(calldata.IsDisplayed) ? DBNull.Value : (object)calldata.IsDisplayed);
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
        internal static string Search(long? fkbatchid, long? fkuserid, string isdisplayed)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCongratulationsPopUp].[FKBatchID] = " + fkbatchid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCongratulationsPopUp].[FKUserID] = " + fkuserid + "");
            }
            if (isdisplayed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCongratulationsPopUp].[IsDisplayed] LIKE '" + isdisplayed.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INCongratulationsPopUp].[ID], [INCongratulationsPopUp].[FKBatchID], [INCongratulationsPopUp].[FKUserID], [INCongratulationsPopUp].[IsDisplayed], [INCongratulationsPopUp].[StampDate], [INCongratulationsPopUp].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCongratulationsPopUp].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCongratulationsPopUp] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
