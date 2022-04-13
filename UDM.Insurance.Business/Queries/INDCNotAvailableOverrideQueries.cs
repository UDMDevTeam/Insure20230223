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
    internal abstract partial class INDCNotAvailableOverrideQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INDCNotAvailableOverride calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINDCNotAvailableOverride] ([ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID] FROM [INDCNotAvailableOverride] WHERE [INDCNotAvailableOverride].[ID] = @ID; ";
                query += "DELETE FROM [INDCNotAvailableOverride] WHERE [INDCNotAvailableOverride].[ID] = @ID; ";
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
        internal static string DeleteHistory(INDCNotAvailableOverride calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINDCNotAvailableOverride] WHERE [zHstINDCNotAvailableOverride].[ID] = @ID";
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
        internal static string UnDelete(INDCNotAvailableOverride calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INDCNotAvailableOverride] ([ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID] FROM [zHstINDCNotAvailableOverride] WHERE [zHstINDCNotAvailableOverride].[ID] = @ID AND [zHstINDCNotAvailableOverride].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDCNotAvailableOverride] WHERE [zHstINDCNotAvailableOverride].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INDCNotAvailableOverride] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINDCNotAvailableOverride] WHERE [zHstINDCNotAvailableOverride].[ID] = @ID AND [zHstINDCNotAvailableOverride].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDCNotAvailableOverride] WHERE [zHstINDCNotAvailableOverride].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INDCNotAvailableOverride] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INDCNotAvailableOverride calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID] FROM [INDCNotAvailableOverride] WHERE [INDCNotAvailableOverride].[ID] = @ID";
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
        internal static string FillData(INDCNotAvailableOverride calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INDCNotAvailableOverride].[ID], [INDCNotAvailableOverride].[FKImportID], [INDCNotAvailableOverride].[AgentsAvailable], [INDCNotAvailableOverride].[StampDate], [INDCNotAvailableOverride].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDCNotAvailableOverride].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDCNotAvailableOverride] ");
                query.Append(" WHERE [INDCNotAvailableOverride].[ID] = @ID");
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
        internal static string FillHistory(INDCNotAvailableOverride calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID] FROM [zHstINDCNotAvailableOverride] WHERE [zHstINDCNotAvailableOverride].[ID] = @ID AND [zHstINDCNotAvailableOverride].[StampUserID] = @StampUserID AND [zHstINDCNotAvailableOverride].[StampDate] = @StampDate";
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
            query.Append("SELECT [INDCNotAvailableOverride].[ID], [INDCNotAvailableOverride].[FKImportID], [INDCNotAvailableOverride].[AgentsAvailable], [INDCNotAvailableOverride].[StampDate], [INDCNotAvailableOverride].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDCNotAvailableOverride].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDCNotAvailableOverride] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINDCNotAvailableOverride].[ID], [zHstINDCNotAvailableOverride].[FKImportID], [zHstINDCNotAvailableOverride].[AgentsAvailable], [zHstINDCNotAvailableOverride].[StampDate], [zHstINDCNotAvailableOverride].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDCNotAvailableOverride].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDCNotAvailableOverride] ");
            query.Append("INNER JOIN (SELECT [zHstINDCNotAvailableOverride].[ID], MAX([zHstINDCNotAvailableOverride].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINDCNotAvailableOverride] ");
            query.Append("WHERE [zHstINDCNotAvailableOverride].[ID] NOT IN (SELECT [INDCNotAvailableOverride].[ID] FROM [INDCNotAvailableOverride]) ");
            query.Append("GROUP BY [zHstINDCNotAvailableOverride].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINDCNotAvailableOverride].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINDCNotAvailableOverride].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INDCNotAvailableOverride calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINDCNotAvailableOverride].[ID], [zHstINDCNotAvailableOverride].[FKImportID], [zHstINDCNotAvailableOverride].[AgentsAvailable], [zHstINDCNotAvailableOverride].[StampDate], [zHstINDCNotAvailableOverride].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDCNotAvailableOverride].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDCNotAvailableOverride] ");
                query.Append(" WHERE [zHstINDCNotAvailableOverride].[ID] = @ID");
                query.Append(" ORDER BY [zHstINDCNotAvailableOverride].[StampDate] DESC");
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
        internal static string Save(INDCNotAvailableOverride calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINDCNotAvailableOverride] ([ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [AgentsAvailable], [StampDate], [StampUserID] FROM [INDCNotAvailableOverride] WHERE [INDCNotAvailableOverride].[ID] = @ID; ");
                    query.Append("UPDATE [INDCNotAvailableOverride]");
                    parameters = new object[3];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [AgentsAvailable] = @AgentsAvailable");
                    parameters[1] = Database.GetParameter("@AgentsAvailable", string.IsNullOrEmpty(calldata.AgentsAvailable) ? DBNull.Value : (object)calldata.AgentsAvailable);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INDCNotAvailableOverride].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INDCNotAvailableOverride] ([FKImportID], [AgentsAvailable], [StampDate], [StampUserID]) VALUES(@FKImportID, @AgentsAvailable, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@AgentsAvailable", string.IsNullOrEmpty(calldata.AgentsAvailable) ? DBNull.Value : (object)calldata.AgentsAvailable);
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
        internal static string Search(long? fkimportid, string AgentsAvailable)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDCNotAvailableOverride].[FKImportID] = " + fkimportid + "");
            }
            if (AgentsAvailable != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDCNotAvailableOverride].[Number] LIKE '" + AgentsAvailable.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INDCNotAvailableOverride].[ID], [INDCNotAvailableOverride].[FKImportID], [INDCNotAvailableOverride].[AgentsAvailable], [INDCNotAvailableOverride].[StampDate], [INDCNotAvailableOverride].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDCNotAvailableOverride].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDCNotAvailableOverride] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
