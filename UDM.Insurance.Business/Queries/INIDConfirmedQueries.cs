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
    internal abstract partial class INIDConfirmedQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INIDConfirmed calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINIDConfirmed] ([ID], [FKImportID], [Confirmed], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Confirmed], [StampDate], [StampUserID] FROM [INIDConfirmed] WHERE [INIDConfirmed].[ID] = @ID; ";
                query += "DELETE FROM [INIDConfirmed] WHERE [INIDConfirmed].[ID] = @ID; ";
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
        internal static string DeleteHistory(INIDConfirmed calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINIDConfirmed] WHERE [zHstINIDConfirmed].[ID] = @ID";
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
        internal static string UnDelete(INIDConfirmed calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INIDConfirmed] ([ID], [FKImportID], [Confirmed], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Confirmed], [StampDate], [StampUserID] FROM [zHstINIDConfirmed] WHERE [zHstINIDConfirmed].[ID] = @ID AND [zHstINIDConfirmed].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINIDConfirmed] WHERE [zHstINIDConfirmed].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INIDConfirmed] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINIDConfirmed] WHERE [zHstINIDConfirmed].[ID] = @ID AND [zHstINIDConfirmed].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINIDConfirmed] WHERE [zHstINIDConfirmed].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INIDConfirmed] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INIDConfirmed calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [Confirmed], [StampDate], [StampUserID] FROM [INIDConfirmed] WHERE [INIDConfirmed].[ID] = @ID";
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
        internal static string FillData(INIDConfirmed calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INIDConfirmed].[ID], [INIDConfirmed].[FKImportID], [INIDConfirmed].[Confirmed], [INIDConfirmed].[StampDate], [INIDConfirmed].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INIDConfirmed].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INIDConfirmed] ");
                query.Append(" WHERE [INIDConfirmed].[ID] = @ID");
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
        internal static string FillHistory(INIDConfirmed calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [Confirmed], [StampDate], [StampUserID] FROM [zHstINIDConfirmed] WHERE [zHstINIDConfirmed].[ID] = @ID AND [zHstINIDConfirmed].[StampUserID] = @StampUserID AND [zHstINIDConfirmed].[StampDate] = @StampDate";
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
            query.Append("SELECT [INIDConfirmed].[ID], [INIDConfirmed].[FKImportID], [INIDConfirmed].[Confirmed], [INIDConfirmed].[StampDate], [INIDConfirmed].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INIDConfirmed].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INIDConfirmed] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINIDConfirmed].[ID], [zHstINIDConfirmed].[FKImportID], [zHstINIDConfirmed].[Confirmed], [zHstINIDConfirmed].[StampDate], [zHstINIDConfirmed].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINIDConfirmed].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINIDConfirmed] ");
            query.Append("INNER JOIN (SELECT [zHstINIDConfirmed].[ID], MAX([zHstINIDConfirmed].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINIDConfirmed] ");
            query.Append("WHERE [zHstINIDConfirmed].[ID] NOT IN (SELECT [INIDConfirmed].[ID] FROM [INIDConfirmed]) ");
            query.Append("GROUP BY [zHstINIDConfirmed].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINIDConfirmed].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINIDConfirmed].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INIDConfirmed calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINIDConfirmed].[ID], [zHstINIDConfirmed].[FKImportID], [zHstINIDConfirmed].[Confirmed], [zHstINIDConfirmed].[StampDate], [zHstINIDConfirmed].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINIDConfirmed].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINIDConfirmed] ");
                query.Append(" WHERE [zHstINIDConfirmed].[ID] = @ID");
                query.Append(" ORDER BY [zHstINIDConfirmed].[StampDate] DESC");
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
        internal static string Save(INIDConfirmed calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINIDConfirmed] ([ID], [FKImportID], [Confirmed], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [Confirmed], [StampDate], [StampUserID] FROM [INIDConfirmed] WHERE [INIDConfirmed].[ID] = @ID; ");
                    query.Append("UPDATE [INIDConfirmed]");
                    parameters = new object[3];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [Confirmed] = @Confirmed");
                    parameters[1] = Database.GetParameter("@Confirmed", string.IsNullOrEmpty(calldata.Confirmed) ? DBNull.Value : (object)calldata.Confirmed);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INIDConfirmed].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INIDConfirmed] ([FKImportID], [Confirmed], [StampDate], [StampUserID]) VALUES(@FKImportID, @Confirmed, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Confirmed", string.IsNullOrEmpty(calldata.Confirmed) ? DBNull.Value : (object)calldata.Confirmed);
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
        internal static string Search(long? fkimportid, string confirmed)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INIDConfirmed].[FKImportID] = " + fkimportid + "");
            }
            if (confirmed != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INIDConfirmed].[Confirmed] LIKE '" + confirmed.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INIDConfirmed].[ID], [INIDConfirmed].[FKImportID], [INIDConfirmed].[Confirmed], [INIDConfirmed].[StampDate], [INIDConfirmed].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INIDConfirmed].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INIDConfirmed] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
