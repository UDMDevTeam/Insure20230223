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
    internal abstract partial class CallDataQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(CallData calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstCallData] ([ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID] FROM [CallData] WHERE [CallData].[ID] = @ID; ";
                query += "DELETE FROM [CallData] WHERE [CallData].[ID] = @ID; ";
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
        internal static string DeleteHistory(CallData calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstCallData] WHERE [zHstCallData].[ID] = @ID";
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
        internal static string UnDelete(CallData calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [CallData] ([ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID] FROM [zHstCallData] WHERE [zHstCallData].[ID] = @ID AND [zHstCallData].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCallData] WHERE [zHstCallData].[ID] = @ID) AND (SELECT COUNT(ID) FROM [CallData] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstCallData] WHERE [zHstCallData].[ID] = @ID AND [zHstCallData].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCallData] WHERE [zHstCallData].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [CallData] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(CallData calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID] FROM [CallData] WHERE [CallData].[ID] = @ID";
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
        internal static string FillData(CallData calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [CallData].[ID], [CallData].[FKImportID], [CallData].[Number], [CallData].[Extension], [CallData].[RecRef], [CallData].[StampDate], [CallData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallData] ");
                query.Append(" WHERE [CallData].[ID] = @ID");
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
        internal static string FillHistory(CallData calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID] FROM [zHstCallData] WHERE [zHstCallData].[ID] = @ID AND [zHstCallData].[StampUserID] = @StampUserID AND [zHstCallData].[StampDate] = @StampDate";
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
            query.Append("SELECT [CallData].[ID], [CallData].[FKImportID], [CallData].[Number], [CallData].[Extension], [CallData].[RecRef], [CallData].[StampDate], [CallData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallData] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstCallData].[ID], [zHstCallData].[FKImportID], [zHstCallData].[Number], [zHstCallData].[Extension], [zHstCallData].[RecRef], [zHstCallData].[StampDate], [zHstCallData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCallData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstCallData] ");
            query.Append("INNER JOIN (SELECT [zHstCallData].[ID], MAX([zHstCallData].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstCallData] ");
            query.Append("WHERE [zHstCallData].[ID] NOT IN (SELECT [CallData].[ID] FROM [CallData]) ");
            query.Append("GROUP BY [zHstCallData].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstCallData].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstCallData].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(CallData calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstCallData].[ID], [zHstCallData].[FKImportID], [zHstCallData].[Number], [zHstCallData].[Extension], [zHstCallData].[RecRef], [zHstCallData].[StampDate], [zHstCallData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCallData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstCallData] ");
                query.Append(" WHERE [zHstCallData].[ID] = @ID");
                query.Append(" ORDER BY [zHstCallData].[StampDate] DESC");
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
        internal static string Save(CallData calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstCallData] ([ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID] FROM [CallData] WHERE [CallData].[ID] = @ID; ");
                    query.Append("UPDATE [CallData]");
                    parameters = new object[5];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [Number] = @Number");
                    parameters[1] = Database.GetParameter("@Number", string.IsNullOrEmpty(calldata.Number) ? DBNull.Value : (object)calldata.Number);
                    query.Append(", [Extension] = @Extension");
                    parameters[2] = Database.GetParameter("@Extension", string.IsNullOrEmpty(calldata.Extension) ? DBNull.Value : (object)calldata.Extension);
                    query.Append(", [RecRef] = @RecRef");
                    parameters[3] = Database.GetParameter("@RecRef", string.IsNullOrEmpty(calldata.RecRef) ? DBNull.Value : (object)calldata.RecRef);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [CallData].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [CallData] ([FKImportID], [Number], [Extension], [RecRef], [StampDate], [StampUserID]) VALUES(@FKImportID, @Number, @Extension, @RecRef, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Number", string.IsNullOrEmpty(calldata.Number) ? DBNull.Value : (object)calldata.Number);
                    parameters[2] = Database.GetParameter("@Extension", string.IsNullOrEmpty(calldata.Extension) ? DBNull.Value : (object)calldata.Extension);
                    parameters[3] = Database.GetParameter("@RecRef", string.IsNullOrEmpty(calldata.RecRef) ? DBNull.Value : (object)calldata.RecRef);
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
        internal static string Search(long? fkimportid, string number, string extension, string recref)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallData].[FKImportID] = " + fkimportid + "");
            }
            if (number != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallData].[Number] LIKE '" + number.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (extension != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallData].[Extension] LIKE '" + extension.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (recref != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallData].[RecRef] LIKE '" + recref.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [CallData].[ID], [CallData].[FKImportID], [CallData].[Number], [CallData].[Extension], [CallData].[RecRef], [CallData].[StampDate], [CallData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallData] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
