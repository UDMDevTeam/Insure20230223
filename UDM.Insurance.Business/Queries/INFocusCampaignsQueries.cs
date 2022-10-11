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
    internal abstract partial class INFocusCampaignsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INFocusCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINFocusCampaigns] ([ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID] FROM [INFocusCampaigns] WHERE [INFocusCampaigns].[ID] = @ID; ";
                query += "DELETE FROM [INFocusCampaigns] WHERE [INFocusCampaigns].[ID] = @ID; ";
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
        internal static string DeleteHistory(INFocusCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINFocusCampaigns] WHERE [zHstINFocusCampaigns].[ID] = @ID";
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
        internal static string UnDelete(INFocusCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INFocusCampaigns] ([ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID] FROM [zHstINFocusCampaigns] WHERE [zHstINFocusCampaigns].[ID] = @ID AND [zHstINFocusCampaigns].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINFocusCampaigns] WHERE [zHstINFocusCampaigns].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INFocusCampaigns] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINFocusCampaigns] WHERE [zHstINFocusCampaigns].[ID] = @ID AND [zHstINFocusCampaigns].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINFocusCampaigns] WHERE [zHstINFocusCampaigns].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INFocusCampaigns] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INFocusCampaigns calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID] FROM [INFocusCampaigns] WHERE [INFocusCampaigns].[ID] = @ID";
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
        internal static string FillData(INFocusCampaigns calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INFocusCampaigns].[ID], [INFocusCampaigns].[FKINCampaignID], [INFocusCampaigns].[IsActive], [INFocusCampaigns].[StampDate], [INFocusCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFocusCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFocusCampaigns] ");
                query.Append(" WHERE [INFocusCampaigns].[ID] = @ID");
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
        internal static string FillHistory(INFocusCampaigns calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID] FROM [zHstINFocusCampaigns] WHERE [zHstINFocusCampaigns].[ID] = @ID AND [zHstINFocusCampaigns].[StampUserID] = @StampUserID AND [zHstINFocusCampaigns].[StampDate] = @StampDate";
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
            query.Append("SELECT [INFocusCampaigns].[ID], [INFocusCampaigns].[FKINCampaignID], [INFocusCampaigns].[IsActive], [INFocusCampaigns].[StampDate], [INFocusCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFocusCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFocusCampaigns] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINFocusCampaigns].[ID], [zHstINFocusCampaigns].[FKINCampaignID], [zHstINFocusCampaigns].[IsActive], [zHstINFocusCampaigns].[StampDate], [zHstINFocusCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINFocusCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINFocusCampaigns] ");
            query.Append("INNER JOIN (SELECT [zHstINFocusCampaigns].[ID], MAX([zHstINFocusCampaigns].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINFocusCampaigns] ");
            query.Append("WHERE [zHstINFocusCampaigns].[ID] NOT IN (SELECT [INFocusCampaigns].[ID] FROM [INFocusCampaigns]) ");
            query.Append("GROUP BY [zHstINFocusCampaigns].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINFocusCampaigns].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINFocusCampaigns].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INFocusCampaigns calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINFocusCampaigns].[ID], [zHstINFocusCampaigns].[FKINCampaignID], [zHstINFocusCampaigns].[IsActive], [zHstINFocusCampaigns].[StampDate], [zHstINFocusCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINFocusCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINFocusCampaigns] ");
                query.Append(" WHERE [zHstINFocusCampaigns].[ID] = @ID");
                query.Append(" ORDER BY [zHstINFocusCampaigns].[StampDate] DESC");
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
        internal static string Save(INFocusCampaigns calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINFocusCampaigns] ([ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [IsActive], [StampDate], [StampUserID] FROM [INFocusCampaigns] WHERE [INFocusCampaigns].[ID] = @ID; ");
                    query.Append("UPDATE [INFocusCampaigns]");
                    parameters = new object[3];
                    query.Append(" SET [FKINCampaignID] = @FKINCampaignID");
                    parameters[0] = Database.GetParameter("@FKINCampaignID", calldata.FKINCampaignID.HasValue ? (object)calldata.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[1] = Database.GetParameter("@IsActive", string.IsNullOrEmpty(calldata.IsActive) ? DBNull.Value : (object)calldata.IsActive);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INFocusCampaigns].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INFocusCampaigns] ([FKINCampaignID], [IsActive], [StampDate], [StampUserID]) VALUES(@FKINCampaignID, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKINCampaignID", calldata.FKINCampaignID.HasValue ? (object)calldata.FKINCampaignID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@IsActive", string.IsNullOrEmpty(calldata.IsActive) ? DBNull.Value : (object)calldata.IsActive);
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
        internal static string Search(long? fkincampaignid, string isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFocusCampaigns].[FKINCampaignID] = " + fkincampaignid + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFocusCampaigns].[IsActive] LIKE '" + isactive.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INFocusCampaigns].[ID], [INFocusCampaigns].[FKINCampaignID], [INFocusCampaigns].[IsActive], [INFocusCampaigns].[StampDate], [INFocusCampaigns].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFocusCampaigns].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFocusCampaigns] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
