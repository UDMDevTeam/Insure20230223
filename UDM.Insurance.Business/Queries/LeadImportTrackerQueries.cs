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
    internal abstract partial class LeadImportTrackerQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(LeadImportTracker calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstConservedLeadImportTracker] ([ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID] FROM [ConservedLeadImportTracker] WHERE [ConservedLeadImportTracker].[ID] = @ID; ";
                query += "DELETE FROM [ConservedLeadImportTracker] WHERE [ConservedLeadImportTracker].[ID] = @ID; ";
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
        internal static string DeleteHistory(LeadImportTracker calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstConservedLeadImportTracker] WHERE [zHstConservedLeadImportTracker].[ID] = @ID";
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
        internal static string UnDelete(LeadImportTracker calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [ConservedLeadImportTracker] ([ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID] FROM [zHstConservedLeadImportTracker] WHERE [zHstConservedLeadImportTracker].[ID] = @ID AND [zHstConservedLeadImportTracker].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstConservedLeadImportTracker] WHERE [zHstConservedLeadImportTracker].[ID] = @ID) AND (SELECT COUNT(ID) FROM [ConservedLeadImportTracker] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstConservedLeadImportTracker] WHERE [zHstConservedLeadImportTracker].[ID] = @ID AND [zHstConservedLeadImportTracker].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstConservedLeadImportTracker] WHERE [zHstConservedLeadImportTracker].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [ConservedLeadImportTracker] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(LeadImportTracker calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID] FROM [ConservedLeadImportTracker] WHERE [ConservedLeadImportTracker].[ID] = @ID";
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
        internal static string FillData(LeadImportTracker calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [ConservedLeadImportTracker].[ID], [ConservedLeadImportTracker].[FKINImportID], [ConservedLeadImportTracker].[FKINBatchID], [ConservedLeadImportTracker].[StampDate], [ConservedLeadImportTracker].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ConservedLeadImportTracker].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ConservedLeadImportTracker] ");
                query.Append(" WHERE [ConservedLeadImportTracker].[ID] = @ID");
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
        internal static string FillHistory(LeadImportTracker calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID] FROM [zHstConservedLeadImportTracker] WHERE [zHstConservedLeadImportTracker].[ID] = @ID AND [zHstConservedLeadImportTracker].[StampUserID] = @StampUserID AND [zHstConservedLeadImportTracker].[StampDate] = @StampDate";
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
            query.Append("SELECT [ConservedLeadImportTracker].[ID], [ConservedLeadImportTracker].[FKINImportID], [ConserveedLeadImportTracker].[FKINBatchID], [ConservedLeadImportTracker].[StampDate], [ConservedLeadImportTracker].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ConservedLeadImportTracker].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ConservedLeadImportTracker] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstConservedLeadImportTracker].[ID], [zHstConservedLeadImportTracker].[FKINImportID], [zHstConservedLeadImportTracker].[FKINBatchID], [zHstConservedLeadImportTracker].[StampDate], [zHstConservedLeadImportTracker].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstConservedLeadImportTracker].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstConservedLeadImportTracker] ");
            query.Append("INNER JOIN (SELECT [zHstConservedLeadImportTracker].[ID], MAX([zHstConservedLeadImportTracker].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstConservedLeadImportTracker] ");
            query.Append("WHERE [zHstConservedLeadImportTracker].[ID] NOT IN (SELECT [ConservedLeadImportTracker].[ID] FROM [ConservedLeadImportTracker]) ");
            query.Append("GROUP BY [zHstConservedLeadImportTracker].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstConservedLeadImportTracker].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstConservedLeadImportTracker].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(LeadImportTracker calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstConservedLeadImportTracker].[ID], [zHstConservedLeadImportTracker].[FKINImportID], [zHstConservedLeadImportTracker].[FKINBatchID], [zHstConservedLeadImportTracker].[StampDate], [zHstConservedLeadImportTracker].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstConservedLeadImportTracker].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstConservedLeadImportTracker] ");
                query.Append(" WHERE [zHstConservedLeadImportTracker].[ID] = @ID");
                query.Append(" ORDER BY [zHstConservedLeadImportTracker].[StampDate] DESC");
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
        internal static string Save(LeadImportTracker calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstConservedLeadImportTracker] ([ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKINBatchID], [StampDate], [StampUserID] FROM [ConservedLeadImportTracker] WHERE [ConservedLeadImportTracker].[ID] = @ID; ");
                    query.Append("UPDATE [ConservedLeadImportTracker]");
                    parameters = new object[3];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", calldata.FKINImportID.HasValue ? (object)calldata.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKINBatchID] = @FKINBatchID");
                    parameters[1] = Database.GetParameter("@FKINBatchID", calldata.FKINBatchID.HasValue ? (object)calldata.FKINBatchID.Value : DBNull.Value);

                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [ConservedLeadImportTracker].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [ConservedLeadImportTracker] ([FKINImportID], [FKINBatchID], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKINBatchID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKINImportID", calldata.FKINImportID.HasValue ? (object)calldata.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINBatchID", calldata.FKINBatchID.HasValue ? (object)calldata.FKINBatchID.Value : DBNull.Value);
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
        internal static string Search(long? fkinimportid, long? fkinbatchid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ConservedLeadImportTracker].[FKImportID] = " + fkinimportid + "");
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ConservedLeadImportTracker].[FKINBatchID] = " + fkinbatchid + "");
            }

            query.Append("SELECT [ConservedLeadImportTracker].[ID], [ConservedLeadImportTracker].[FKINImportID], [ConservedLeadImportTracker].[FKINBatchID], [ConservedLeadImportTracker].[StampDate], [ConservedLeadImportTracker].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ConservedLeadImportTracker].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ConservedLeadImportTracker] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
