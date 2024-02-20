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
    internal abstract partial class INDebiCheckQueriesQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INDebiCheckQueries calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINDebiCheckQueries] ([ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID], [Notes]) SELECT [ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID] FROM [INDebiCheckQueries] WHERE [INDebiCheckQueries].[ID] = @ID; ";
                query += "DELETE FROM [INDebiCheckQueries] WHERE [INDebiCheckQueries].[ID] = @ID; ";
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
        internal static string DeleteHistory(INDebiCheckQueries calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINDebiCheckQueries] WHERE [zHstINDebiCheckQueries].[ID] = @ID";
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
        internal static string UnDelete(INDebiCheckQueries calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INDebiCheckQueries] ([ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID],[Notes]) SELECT [ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID],[Notes] FROM [zHstINDebiCheckQueries] WHERE [zHstINDebiCheckQueries].[ID] = @ID AND [zHstINDebiCheckQueries].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDebiCheckQueries] WHERE [zHstINDebiCheckQueries].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INDebiCheckQueries] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINDebiCheckQueries] WHERE [zHstINDebiCheckQueries].[ID] = @ID AND [zHstINDebiCheckQueries].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDebiCheckQueries] WHERE [zHstINDebiCheckQueries].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INDebiCheckQueries] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INDebiCheckQueries calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID] , [Notes] FROM [INDebiCheckQueries] WHERE [INDebiCheckQueries].[ID] = @ID";
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
        internal static string FillData(INDebiCheckQueries calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INDebiCheckQueries].[ID], [INDebiCheckQueries].[FKImportID], [INDebiCheckQueries].[DebiCheckQueryID], [INDebiCheckQueries].[Department], [INDebiCheckQueries].[StampDate], [INDebiCheckQueries].[StampUserID],  [INDebiCheckQueries].[Notes]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDebiCheckQueries].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDebiCheckQueries] ");
                query.Append(" WHERE [INDebiCheckQueries].[ID] = @ID");
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
        internal static string FillHistory(INDebiCheckQueries calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID], [Notes] FROM [zHstINDebiCheckQueries] WHERE [zHstINDebiCheckQueries].[ID] = @ID AND [zHstINDebiCheckQueries].[StampUserID] = @StampUserID AND [zHstINDebiCheckQueries].[StampDate] = @StampDate";
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
            query.Append("SELECT [INDebiCheckQueries].[ID], [INDebiCheckQueries].[FKImportID], [INDebiCheckQueries].[DebiCheckQueryID], [INDebiCheckQueries].[Department], [INDebiCheckQueries].[StampDate], [INDebiCheckQueries].[StampUserID], [INDebiCheckQueries].[Notes]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDebiCheckQueries].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDebiCheckQueries] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINDebiCheckQueries].[ID], [zHstINDebiCheckQueries].[FKImportID], [zHstINDebiCheckQueries].[DebiCheckQueryID], [zHstINDebiCheckQueries].[Department], [zHstINDebiCheckQueries].[StampDate], [zHstINDebiCheckQueries].[StampUserID], [INDebiCheckQueries].[Notes]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDebiCheckQueries].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDebiCheckQueries] ");
            query.Append("INNER JOIN (SELECT [zHstINDebiCheckQueries].[ID], MAX([zHstINDebiCheckQueries].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINDebiCheckQueries] ");
            query.Append("WHERE [zHstINDebiCheckQueries].[ID] NOT IN (SELECT [INDebiCheckQueries].[ID] FROM [INDebiCheckQueries]) ");
            query.Append("GROUP BY [zHstINDebiCheckQueries].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINDebiCheckQueries].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINDebiCheckQueries].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INDebiCheckQueries calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINDebiCheckQueries].[ID], [zHstINDebiCheckQueries].[FKImportID], [zHstINDebiCheckQueries].[DebiCheckQueryID], [zHstINDebiCheckQueries].[Department], [zHstINDebiCheckQueries].[StampDate], [zHstINDebiCheckQueries].[StampUserID], [INDebiCheckQueries].[Notes]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDebiCheckQueries].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDebiCheckQueries] ");
                query.Append(" WHERE [zHstINDebiCheckQueries].[ID] = @ID");
                query.Append(" ORDER BY [zHstINDebiCheckQueries].[StampDate] DESC");
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
        internal static string Save(INDebiCheckQueries calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINDebiCheckQueries] ([ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID],[Notes]) SELECT [ID], [FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID], [Notes] FROM [INDebiCheckQueries] WHERE [INDebiCheckQueries].[ID] = @ID; ");
                    query.Append("UPDATE [INDebiCheckQueries]");
                    parameters = new object[5];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [DebiCheckQueryID] = @DebiCheckQueryID");
                    parameters[1] = Database.GetParameter("@DebiCheckQueryID", calldata.DebiCheckQueryID.HasValue ? (object)calldata.DebiCheckQueryID.Value : DBNull.Value);
                    query.Append(", [Department] = @Department");
                    parameters[2] = Database.GetParameter("@Department", string.IsNullOrEmpty(calldata.Department) ? DBNull.Value : (object)calldata.Department);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(", [Notes] = @Notes");
                    parameters[4] = Database.GetParameter("@Notes", string.IsNullOrEmpty(calldata.Notes) ? DBNull.Value : (object)calldata.Notes);
                    query.Append(" WHERE [INDebiCheckQueries].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INDebiCheckQueries] ([FKImportID], [DebiCheckQueryID], [Department], [StampDate], [StampUserID], [Notes]) VALUES(@FKImportID, @DebiCheckQueryID, @Department, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ",@Notes);");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@DebiCheckQueryID", calldata.DebiCheckQueryID.HasValue ? (object)calldata.DebiCheckQueryID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@Department", string.IsNullOrEmpty(calldata.Department) ? DBNull.Value : (object)calldata.Department);
                    parameters[3] = Database.GetParameter("@Notes", string.IsNullOrEmpty(calldata.Notes) ? DBNull.Value : (object)calldata.Notes);
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
        internal static string Search(long? fkimportid, long? debicheckqueryID, string department, string notes)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDebiCheckQueries].[FKImportID] = " + fkimportid + "");
            }
            if (debicheckqueryID != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDebiCheckQueries].[DebiCheckQueryID] = " + debicheckqueryID + "");
            }
            if (department != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDebiCheckQueries].[Department] LIKE '" + department.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (notes != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDebiCheckQueries].[Notes] LIKE '" + notes.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INDebiCheckQueries].[ID], [INDebiCheckQueries].[FKImportID], [INDebiCheckQueries].[DebiCheckQueryID], [INDebiCheckQueries].[Department], [INDebiCheckQueries].[StampDate], [INDebiCheckQueries].[StampUserID],[INDebiCheckQueries].[Notes]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDebiCheckQueries].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDebiCheckQueries] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
