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
    internal abstract partial class INMySuccessAgentsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INMySuccessAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINMySuccessAgents] ([ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID]) SELECT [ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID] FROM [INMySuccessAgents] WHERE [INMySuccessAgents].[ID] = @ID; ";
                query += "DELETE FROM [INMySuccessAgents] WHERE [INMySuccessAgents].[ID] = @ID; ";
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
        internal static string DeleteHistory(INMySuccessAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINMySuccessAgents] WHERE [zHstINMySuccessAgents].[ID] = @ID";
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
        internal static string UnDelete(INMySuccessAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INMySuccessAgents] ([ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID]) SELECT [ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID] FROM [zHstINMySuccessAgents] WHERE [zHstINMySuccessAgents].[ID] = @ID AND [zHstINMySuccessAgents].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINMySuccessAgents] WHERE [zHstINMySuccessAgents].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INMySuccessAgents] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINMySuccessAgents] WHERE [zHstINMySuccessAgents].[ID] = @ID AND [zHstINMySuccessAgents].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINMySuccessAgents] WHERE [zHstINMySuccessAgents].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INMySuccessAgents] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INMySuccessAgents calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID] FROM [INMySuccessAgents] WHERE [INMySuccessAgents].[ID] = @ID";
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
        internal static string FillData(INMySuccessAgents calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INMySuccessAgents].[ID], [INMySuccessAgents].[FKCampaignID], [INMySuccessAgents].[UserID], [INMySuccessAgents].[Call1], [INMySuccessAgents].[Call2], [INMySuccessAgents].[Call3], [INMySuccessAgents].[StampDate], [INMySuccessAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMySuccessAgents] ");
                query.Append(" WHERE [INMySuccessAgents].[ID] = @ID");
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
        internal static string FillHistory(INMySuccessAgents calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID] FROM [zHstINMySuccessAgents] WHERE [zHstINMySuccessAgents].[ID] = @ID AND [zHstINMySuccessAgents].[StampUserID] = @StampUserID AND [zHstINMySuccessAgents].[StampDate] = @StampDate";
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
            query.Append("SELECT [INMySuccessAgents].[ID], [INMySuccessAgents].[FKCampaignID], [INMySuccessAgents].[UserID], [INMySuccessAgents].[Call1], [INMySuccessAgents].[Call2], [INMySuccessAgents].[Call3], [INMySuccessAgents].[StampDate], [INMySuccessAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMySuccessAgents] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINMySuccessAgents].[ID], [zHstINMySuccessAgents].[FKCampaignID], [zHstINMySuccessAgents].[UserID], [zHstINMySuccessAgents].[Call1], [zHstINMySuccessAgents].[Call2], [zHstINMySuccessAgents].[Call3], [zHstINMySuccessAgents].[StampDate], [zHstINMySuccessAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINMySuccessAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINMySuccessAgents] ");
            query.Append("INNER JOIN (SELECT [zHstINMySuccessAgents].[ID], MAX([zHstINMySuccessAgents].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINMySuccessAgents] ");
            query.Append("WHERE [zHstINMySuccessAgents].[ID] NOT IN (SELECT [INMySuccessAgents].[ID] FROM [INMySuccessAgents]) ");
            query.Append("GROUP BY [zHstINMySuccessAgents].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINMySuccessAgents].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINMySuccessAgents].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INMySuccessAgents calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINMySuccessAgents].[ID], [zHstINMySuccessAgents].[FKCampaignID], [zHstINMySuccessAgents].[UserID], [zHstINMySuccessAgents].[Call1], [zHstINMySuccessAgents].[Call2], [zHstINMySuccessAgents].[Call3], [zHstINMySuccessAgents].[StampDate], [zHstINMySuccessAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINMySuccessAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINMySuccessAgents] ");
                query.Append(" WHERE [zHstINMySuccessAgents].[ID] = @ID");
                query.Append(" ORDER BY [zHstINMySuccessAgents].[StampDate] DESC");
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
        internal static string Save(INMySuccessAgents calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINMySuccessAgents] ([ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID]) SELECT [ID], [FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID] FROM [INMySuccessAgents] WHERE [INMySuccessAgents].[ID] = @ID; ");
                    query.Append("UPDATE [INMySuccessAgents]");
                    parameters = new object[6];
                    query.Append(" SET [FKCampaignID] = @FKCampaignID");
                    parameters[0] = Database.GetParameter("@FKCampaignID", calldata.FKCampaignID.HasValue ? (object)calldata.FKCampaignID.Value : DBNull.Value);
                    query.Append(", [UserID] = @UserID");
                    parameters[1] = Database.GetParameter("@UserID", calldata.UserID.HasValue ? (object)calldata.UserID.Value : DBNull.Value);
                    query.Append(", [Call1] = @Call1");
                    parameters[2] = Database.GetParameter("@Call1", string.IsNullOrEmpty(calldata.Call1) ? DBNull.Value : (object)calldata.Call1);
                    query.Append(", [Call2] = @Call2");
                    parameters[3] = Database.GetParameter("@Call2", string.IsNullOrEmpty(calldata.Call2) ? DBNull.Value : (object)calldata.Call2);
                    query.Append(", [Call3] = @Call3");
                    parameters[4] = Database.GetParameter("@Call3", string.IsNullOrEmpty(calldata.Call3) ? DBNull.Value : (object)calldata.Call3);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INMySuccessAgents].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INMySuccessAgents] ([FKCampaignID], [UserID], [Call1], [Call2], [Call3], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @UserID, @Call1, @Call2, @Call3, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@FKCampaignID", calldata.FKCampaignID.HasValue ? (object)calldata.FKCampaignID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@UserID", calldata.UserID.HasValue ? (object)calldata.UserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@Call1", string.IsNullOrEmpty(calldata.Call1) ? DBNull.Value : (object)calldata.Call1);
                    parameters[3] = Database.GetParameter("@Call2", string.IsNullOrEmpty(calldata.Call2) ? DBNull.Value : (object)calldata.Call2);
                    parameters[4] = Database.GetParameter("@Call3", string.IsNullOrEmpty(calldata.Call3) ? DBNull.Value : (object)calldata.Call3);
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
        internal static string Search(int? fkcampaignid, int? userid, string call1, string call2, string call3)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkcampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMySuccessAgents].[FKCampaignID] = " + fkcampaignid + "");
            }
            if (userid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMySuccessAgents].[UserID] = " + userid + "");
            }
            if (call1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMySuccessAgents].[call1] LIKE '" + call1.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (call2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMySuccessAgents].[call2] LIKE '" + call2.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (call3 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMySuccessAgents].[call3] LIKE '" + call3.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INMySuccessAgents].[ID], [INMySuccessAgents].[FKCampaignID], [INMySuccessAgents].[UserID], [INMySuccessAgents].[Call1], [INMySuccessAgents].[Call2], [INMySuccessAgents].[Call3], [INMySuccessAgents].[StampDate], [INMySuccessAgents].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgents].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMySuccessAgents] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
