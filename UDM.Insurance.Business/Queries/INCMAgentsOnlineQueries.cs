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
    internal abstract partial class INCMAgentsOnlineQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INCMAgentsOnline calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINCMAgentsOnline] ([ID], [FKUserID], [Online], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [Online], [StampDate], [StampUserID] FROM [INCMAgentsOnline] WHERE [INCMAgentsOnline].[ID] = @ID; ";
                query += "DELETE FROM [INCMAgentsOnline] WHERE [INCMAgentsOnline].[ID] = @ID; ";
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
        internal static string DeleteHistory(INCMAgentsOnline calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINCMAgentsOnline] WHERE [zHstINCMAgentsOnline].[ID] = @ID";
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
        internal static string UnDelete(INCMAgentsOnline calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INCMAgentsOnline] ([ID], [FKUserID], [Online], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [Online], [StampDate], [StampUserID] FROM [zHstINCMAgentsOnline] WHERE [zHstINCMAgentsOnline].[ID] = @ID AND [zHstINCMAgentsOnline].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCMAgentsOnline] WHERE [zHstINCMAgentsOnline].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INCMAgentsOnline] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINCMAgentsOnline] WHERE [zHstINCMAgentsOnline].[ID] = @ID AND [zHstINCMAgentsOnline].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCMAgentsOnline] WHERE [zHstINCMAgentsOnline].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INCMAgentsOnline] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INCMAgentsOnline calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKUserID], [Online], [StampDate], [StampUserID] FROM [INCMAgentsOnline] WHERE [INCMAgentsOnline].[ID] = @ID";
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
        internal static string FillData(INCMAgentsOnline calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INCMAgentsOnline].[ID], [INCMAgentsOnline].[FKUserID], [INCMAgentsOnline].[Online], [INCMAgentsOnline].[StampDate], [INCMAgentsOnline].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCMAgentsOnline].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCMAgentsOnline] ");
                query.Append(" WHERE [INCMAgentsOnline].[ID] = @ID");
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
        internal static string FillHistory(INCMAgentsOnline calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKUserID], [Online], [StampDate], [StampUserID] FROM [zHstINCMAgentsOnline] WHERE [zHstINCMAgentsOnline].[ID] = @ID AND [zHstINCMAgentsOnline].[StampUserID] = @StampUserID AND [zHstINCMAgentsOnline].[StampDate] = @StampDate";
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
            query.Append("SELECT [INCMAgentsOnline].[ID], [INCMAgentsOnline].[FKUserID], [INCMAgentsOnline].[Online], [INCMAgentsOnline].[StampDate], [INCMAgentsOnline].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCMAgentsOnline].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCMAgentsOnline] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINCMAgentsOnline].[ID], [zHstINCMAgentsOnline].[FKUserID], [zHstINCMAgentsOnline].[Online], [zHstINCMAgentsOnline].[StampDate], [zHstINCMAgentsOnline].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINCMAgentsOnline].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINCMAgentsOnline] ");
            query.Append("INNER JOIN (SELECT [zHstINCMAgentsOnline].[ID], MAX([zHstINCMAgentsOnline].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINCMAgentsOnline] ");
            query.Append("WHERE [zHstINCMAgentsOnline].[ID] NOT IN (SELECT [INCMAgentsOnline].[ID] FROM [INCMAgentsOnline]) ");
            query.Append("GROUP BY [zHstINCMAgentsOnline].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINCMAgentsOnline].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINCMAgentsOnline].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INCMAgentsOnline calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINCMAgentsOnline].[ID], [zHstINCMAgentsOnline].[FKUserID], [zHstINCMAgentsOnline].[Online], [zHstINCMAgentsOnline].[StampDate], [zHstINCMAgentsOnline].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINCMAgentsOnline].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINCMAgentsOnline] ");
                query.Append(" WHERE [zHstINCMAgentsOnline].[ID] = @ID");
                query.Append(" ORDER BY [zHstINCMAgentsOnline].[StampDate] DESC");
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
        internal static string Save(INCMAgentsOnline calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINCMAgentsOnline] ([ID], [FKUserID], [Online], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [Online], [StampDate], [StampUserID] FROM [INCMAgentsOnline] WHERE [INCMAgentsOnline].[ID] = @ID; ");
                    query.Append("UPDATE [INCMAgentsOnline]");
                    parameters = new object[3];
                    query.Append(" SET [FKUserID] = @FKUserID");
                    parameters[0] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKUserID.Value : DBNull.Value);
                    query.Append(", [Online] = @Online");
                    parameters[1] = Database.GetParameter("@Online", string.IsNullOrEmpty(calldata.Online) ? DBNull.Value : (object)calldata.Online);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INCMAgentsOnline].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INCMAgentsOnline] ([FKUserID], [Online], [StampDate], [StampUserID]) VALUES(@FKUserID, @Online, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKUserID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Online", string.IsNullOrEmpty(calldata.Online) ? DBNull.Value : (object)calldata.Online);
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
        internal static string Search(long? fkuserid, string online)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCMAgentsOnline].[FKUserID] = " + fkuserid + "");
            }
            if (online != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCMAgentsOnline].[Online] LIKE '" + online.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INCMAgentsOnline].[ID], [INCMAgentsOnline].[FKUserID], [INCMAgentsOnline].[Online], [INCMAgentsOnline].[StampDate], [INCMAgentsOnline].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCMAgentsOnline].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCMAgentsOnline] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
