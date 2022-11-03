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
    internal abstract partial class ConservedCampaignsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(ConservedCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstConservedCampaigns] ([ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID] FROM [ConservedCampaigns] WHERE [ConservedCampaigns].[ID] = @ID; ";
                query += "DELETE FROM [ConservedCampaigns] WHERE [ConservedCampaigns].[ID] = @ID; ";
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
        internal static string DeleteHistory(ConservedCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstConservedCampaigns] WHERE [zHstConservedCampaigns].[ID] = @ID";
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
        internal static string UnDelete(ConservedCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [ConservedCampaigns] ([ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID] FROM [zHstConservedCampaigns] WHERE [zHstConservedCampaigns].[ID] = @ID AND [zHstConservedCampaigns].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstConservedCampaigns] WHERE [zHstConservedCampaigns].[ID] = @ID) AND (SELECT COUNT(ID) FROM [ConservedCampaigns] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstConservedCampaigns] WHERE [zHstConservedCampaigns].[ID] = @ID AND [zHstConservedCampaigns].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstConservedCampaigns] WHERE [zHstConservedCampaigns].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [ConservedCampaigns] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(ConservedCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID] FROM [ConservedCampaigns] WHERE [ConservedCampaigns].[ID] = @ID";
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
        internal static string FillData(ConservedCampaigns calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [ConservedCampaigns].[ID], [ConservedCampaigns].[FKINCampaignID], [ConservedCampaigns].[FKINBatchID], [ConservedCampaigns].[AmountOfLeads], [ConservedCampaigns].[AmountConserved], [ConservedCampaigns].[StampDate], [ConservedCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ConservedCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ConservedCampaigns] ");
                query.Append(" WHERE [ConservedCampaigns].[ID] = @ID");
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
        internal static string FillHistory(ConservedCampaigns calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID] FROM [zHstConservedCampaigns] WHERE [zHstConservedCampaigns].[ID] = @ID AND [zHstConservedCampaigns].[StampUserID] = @StampUserID AND [zHstConservedCampaigns].[StampDate] = @StampDate";
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
            query.Append("SELECT [ConservedCampaigns].[ID], [ConservedCampaigns].[FKINCampaignID], [ConservedCampaigns].[FKINBatchID], [ConservedCampaigns].[AmountOfLeads], [ConservedCampaigns].[AmountConserved], [ConservedCampaigns].[StampDate], [ConservedCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ConservedCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ConservedCampaigns] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstConservedCampaigns].[ID], [zHstConservedCampaigns].[FKINCampaignID], [zHstConservedCampaigns].[FKINBatchID], [zHstConservedCampaigns].[AmountOfLeads], [zHstConservedCampaigns].[AmountConserved], [zHstConservedCampaigns].[StampDate], [zHstConservedCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstConservedCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstConservedCampaigns] ");
            query.Append("INNER JOIN (SELECT [zHstConservedCampaigns].[ID], MAX([zHstConservedCampaigns].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstConservedCampaigns] ");
            query.Append("WHERE [zHstConservedCampaigns].[ID] NOT IN (SELECT [ConservedCampaigns].[ID] FROM [ConservedCampaigns]) ");
            query.Append("GROUP BY [zHstConservedCampaigns].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstConservedCampaigns].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstConservedCampaigns].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(ConservedCampaigns calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstConservedCampaigns].[ID], [zHstConservedCampaigns].[FKINCampaignID], [zHstConservedCampaigns].[FKINBatchID], [zHstConservedCampaigns].[AmountOfLeads], [zHstConservedCampaigns].[AmountConserved], [zHstConservedCampaigns].[StampDate], [zHstConservedCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstConservedCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstConservedCampaigns] ");
                query.Append(" WHERE [zHstConservedCampaigns].[ID] = @ID");
                query.Append(" ORDER BY [zHstConservedCampaigns].[StampDate] DESC");
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
        internal static string Save(ConservedCampaigns calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstConservedCampaigns] ([ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID] FROM [ConservedCampaigns] WHERE [ConservedCampaigns].[ID] = @ID; ");
                    query.Append("UPDATE [ConservedCampaigns]");
                    parameters = new object[5];
                    query.Append(" SET [FKINCampaignID] = @FKINCampaignID");
                    parameters[0] = Database.GetParameter("@FKINCampaignID", calldata.FKINCampaignID.HasValue ? (object)calldata.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [FKINBatchID] = @FKINBatchID");
                    parameters[1] = Database.GetParameter("@FKINBatchID", calldata.FKINBatchID.HasValue ? (object)calldata.FKINBatchID.Value : DBNull.Value);
                    query.Append(", [AmountOfLeads] = @AmountOfLeads");
                    parameters[2] = Database.GetParameter("@AmountOfLeads", calldata.AmountOfLeads.HasValue ? (object)calldata.AmountOfLeads.Value : DBNull.Value);
                    query.Append(", [AmountConserved] = @AmountConserved");
                    parameters[3] = Database.GetParameter("@AmountConserved", calldata.AmountConserved.HasValue ? (object)calldata.AmountConserved.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [ConservedCampaigns].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [ConservedCampaigns] ([FKINCampaignID], [FKINBatchID], [AmountOfLeads], [AmountConserved], [StampDate], [StampUserID]) VALUES(@FKINCampaignID, @FKINBatchID, @AmountOfLeads, @AmountConserved, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKINCampaignID", calldata.FKINCampaignID.HasValue ? (object)calldata.FKINCampaignID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINBatchID", calldata.FKINBatchID.HasValue ? (object)calldata.FKINBatchID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@AmountOfLeads", calldata.AmountOfLeads.HasValue ? (object)calldata.AmountOfLeads.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@AmountConserved", calldata.AmountConserved.HasValue ? (object)calldata.AmountConserved.Value : DBNull.Value);
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
        internal static string Search(long? fkimportid, long? fkinbatchid, int? amountofleads, int? amountconserved)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ConservedCampaigns].[FKINCampaignID] = " + fkimportid + "");
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ConservedCampaigns].[FKINBatchID] = " + fkinbatchid + "");
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ConservedCampaigns].[AmountOfLeads] = " + amountofleads + "");
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ConservedCampaigns].[AmountConserved] = " + amountconserved + "");
            }
            query.Append("SELECT [ConservedCampaigns].[ID], [ConservedCampaigns].[FKINCampaignID], [ConservedCampaigns].[FKINBatchID], [ConservedCampaigns].[AmountOfLeads], [ConservedCampaigns].[AmountConserved], [ConservedCampaigns].[StampDate], [ConservedCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ConservedCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ConservedCampaigns] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
