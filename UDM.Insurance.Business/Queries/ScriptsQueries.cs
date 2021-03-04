using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to scripts objects.
    /// </summary>
    internal abstract partial class ScriptsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) scripts from the database.
        /// </summary>
        /// <param name="scripts">The scripts object to delete.</param>
        /// <returns>A query that can be used to delete the scripts from the database.</returns>
        internal static string Delete(Scripts scripts, ref object[] parameters)
        {
            string query = string.Empty;
            if (scripts != null)
            {
                query = "INSERT INTO [zHstScripts] ([ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID] FROM [Scripts] WHERE [Scripts].[ID] = @ID; ";
                query += "DELETE FROM [Scripts] WHERE [Scripts].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", scripts.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) scripts from the database.
        /// </summary>
        /// <param name="scripts">The scripts object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the scripts from the database.</returns>
        internal static string DeleteHistory(Scripts scripts, ref object[] parameters)
        {
            string query = string.Empty;
            if (scripts != null)
            {
                query = "DELETE FROM [zHstScripts] WHERE [zHstScripts].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", scripts.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) scripts from the database.
        /// </summary>
        /// <param name="scripts">The scripts object to undelete.</param>
        /// <returns>A query that can be used to undelete the scripts from the database.</returns>
        internal static string UnDelete(Scripts scripts, ref object[] parameters)
        {
            string query = string.Empty;
            if (scripts != null)
            {
                query = "INSERT INTO [Scripts] ([ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID] FROM [zHstScripts] WHERE [zHstScripts].[ID] = @ID AND [zHstScripts].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstScripts] WHERE [zHstScripts].[ID] = @ID) AND (SELECT COUNT(ID) FROM [Scripts] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstScripts] WHERE [zHstScripts].[ID] = @ID AND [zHstScripts].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstScripts] WHERE [zHstScripts].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [Scripts] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", scripts.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an scripts object.
        /// </summary>
        /// <param name="scripts">The scripts object to fill.</param>
        /// <returns>A query that can be used to fill the scripts object.</returns>
        internal static string Fill(Scripts scripts, ref object[] parameters)
        {
            string query = string.Empty;
            if (scripts != null)
            {
                query = "SELECT [ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID] FROM [Scripts] WHERE [Scripts].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", scripts.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  scripts data.
        /// </summary>
        /// <param name="scripts">The scripts to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  scripts data.</returns>
        internal static string FillData(Scripts scripts, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (scripts != null)
            {
            query.Append("SELECT [Scripts].[ID], [Scripts].[FKScriptTypeID], [Scripts].[FKCampaignID], [Scripts].[FKCampaignTypeID], [Scripts].[FKCampaignGroupID], [Scripts].[FKCampaignTypeGroupID], [Scripts].[FKCampaignGroupTypeID], [Scripts].[FKLanguageID], [Scripts].[Description], [Scripts].[Document], [Scripts].[Date], [Scripts].[IsActive], [Scripts].[StampDate], [Scripts].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Scripts].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Scripts] ");
                query.Append(" WHERE [Scripts].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", scripts.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an scripts object from history.
        /// </summary>
        /// <param name="scripts">The scripts object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the scripts object from history.</returns>
        internal static string FillHistory(Scripts scripts, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (scripts != null)
            {
                query = "SELECT [ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID] FROM [zHstScripts] WHERE [zHstScripts].[ID] = @ID AND [zHstScripts].[StampUserID] = @StampUserID AND [zHstScripts].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", scripts.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the scriptss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the scriptss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [Scripts].[ID], [Scripts].[FKScriptTypeID], [Scripts].[FKCampaignID], [Scripts].[FKCampaignTypeID], [Scripts].[FKCampaignGroupID], [Scripts].[FKCampaignTypeGroupID], [Scripts].[FKCampaignGroupTypeID], [Scripts].[FKLanguageID], [Scripts].[Description], [Scripts].[Document], [Scripts].[Date], [Scripts].[IsActive], [Scripts].[StampDate], [Scripts].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Scripts].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Scripts] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted scriptss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted scriptss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstScripts].[ID], [zHstScripts].[FKScriptTypeID], [zHstScripts].[FKCampaignID], [zHstScripts].[FKCampaignTypeID], [zHstScripts].[FKCampaignGroupID], [zHstScripts].[FKCampaignTypeGroupID], [zHstScripts].[FKCampaignGroupTypeID], [zHstScripts].[FKLanguageID], [zHstScripts].[Description], [zHstScripts].[Document], [zHstScripts].[Date], [zHstScripts].[IsActive], [zHstScripts].[StampDate], [zHstScripts].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstScripts].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstScripts] ");
            query.Append("INNER JOIN (SELECT [zHstScripts].[ID], MAX([zHstScripts].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstScripts] ");
            query.Append("WHERE [zHstScripts].[ID] NOT IN (SELECT [Scripts].[ID] FROM [Scripts]) ");
            query.Append("GROUP BY [zHstScripts].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstScripts].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstScripts].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) scripts in the database.
        /// </summary>
        /// <param name="scripts">The scripts object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) scripts in the database.</returns>
        public static string ListHistory(Scripts scripts, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (scripts != null)
            {
            query.Append("SELECT [zHstScripts].[ID], [zHstScripts].[FKScriptTypeID], [zHstScripts].[FKCampaignID], [zHstScripts].[FKCampaignTypeID], [zHstScripts].[FKCampaignGroupID], [zHstScripts].[FKCampaignTypeGroupID], [zHstScripts].[FKCampaignGroupTypeID], [zHstScripts].[FKLanguageID], [zHstScripts].[Description], [zHstScripts].[Document], [zHstScripts].[Date], [zHstScripts].[IsActive], [zHstScripts].[StampDate], [zHstScripts].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstScripts].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstScripts] ");
                query.Append(" WHERE [zHstScripts].[ID] = @ID");
                query.Append(" ORDER BY [zHstScripts].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", scripts.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) scripts to the database.
        /// </summary>
        /// <param name="scripts">The scripts to save.</param>
        /// <returns>A query that can be used to save the scripts to the database.</returns>
        internal static string Save(Scripts scripts, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (scripts != null)
            {
                if (scripts.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstScripts] ([ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID] FROM [Scripts] WHERE [Scripts].[ID] = @ID; ");
                    query.Append("UPDATE [Scripts]");
                    parameters = new object[12];
                    query.Append(" SET [FKScriptTypeID] = @FKScriptTypeID");
                    parameters[0] = Database.GetParameter("@FKScriptTypeID", scripts.FKScriptTypeID.HasValue ? (object)scripts.FKScriptTypeID.Value : DBNull.Value);
                    query.Append(", [FKCampaignID] = @FKCampaignID");
                    parameters[1] = Database.GetParameter("@FKCampaignID", scripts.FKCampaignID.HasValue ? (object)scripts.FKCampaignID.Value : DBNull.Value);
                    query.Append(", [FKCampaignTypeID] = @FKCampaignTypeID");
                    parameters[2] = Database.GetParameter("@FKCampaignTypeID", scripts.FKCampaignTypeID.HasValue ? (object)scripts.FKCampaignTypeID.Value : DBNull.Value);
                    query.Append(", [FKCampaignGroupID] = @FKCampaignGroupID");
                    parameters[3] = Database.GetParameter("@FKCampaignGroupID", scripts.FKCampaignGroupID.HasValue ? (object)scripts.FKCampaignGroupID.Value : DBNull.Value);
                    query.Append(", [FKCampaignTypeGroupID] = @FKCampaignTypeGroupID");
                    parameters[4] = Database.GetParameter("@FKCampaignTypeGroupID", scripts.FKCampaignTypeGroupID.HasValue ? (object)scripts.FKCampaignTypeGroupID.Value : DBNull.Value);
                    query.Append(", [FKCampaignGroupTypeID] = @FKCampaignGroupTypeID");
                    parameters[5] = Database.GetParameter("@FKCampaignGroupTypeID", scripts.FKCampaignGroupTypeID.HasValue ? (object)scripts.FKCampaignGroupTypeID.Value : DBNull.Value);
                    query.Append(", [FKLanguageID] = @FKLanguageID");
                    parameters[6] = Database.GetParameter("@FKLanguageID", scripts.FKLanguageID.HasValue ? (object)scripts.FKLanguageID.Value : DBNull.Value);
                    query.Append(", [Description] = @Description");
                    parameters[7] = Database.GetParameter("@Description", string.IsNullOrEmpty(scripts.Description) ? DBNull.Value : (object)scripts.Description);
                    query.Append(", [Document] = @Document");
                    parameters[8] = Database.GetParameter("@Document", scripts.Document);
                    query.Append(", [Date] = @Date");
                    parameters[9] = Database.GetParameter("@Date", scripts.Date.HasValue ? (object)scripts.Date.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[10] = Database.GetParameter("@IsActive", scripts.IsActive.HasValue ? (object)scripts.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [Scripts].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", scripts.ID);
                }
                else
                {
                    query.Append("INSERT INTO [Scripts] ([FKScriptTypeID], [FKCampaignID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKLanguageID], [Description], [Document], [Date], [IsActive], [StampDate], [StampUserID]) VALUES(@FKScriptTypeID, @FKCampaignID, @FKCampaignTypeID, @FKCampaignGroupID, @FKCampaignTypeGroupID, @FKCampaignGroupTypeID, @FKLanguageID, @Description, @Document, @Date, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@FKScriptTypeID", scripts.FKScriptTypeID.HasValue ? (object)scripts.FKScriptTypeID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKCampaignID", scripts.FKCampaignID.HasValue ? (object)scripts.FKCampaignID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKCampaignTypeID", scripts.FKCampaignTypeID.HasValue ? (object)scripts.FKCampaignTypeID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKCampaignGroupID", scripts.FKCampaignGroupID.HasValue ? (object)scripts.FKCampaignGroupID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@FKCampaignTypeGroupID", scripts.FKCampaignTypeGroupID.HasValue ? (object)scripts.FKCampaignTypeGroupID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@FKCampaignGroupTypeID", scripts.FKCampaignGroupTypeID.HasValue ? (object)scripts.FKCampaignGroupTypeID.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@FKLanguageID", scripts.FKLanguageID.HasValue ? (object)scripts.FKLanguageID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@Description", string.IsNullOrEmpty(scripts.Description) ? DBNull.Value : (object)scripts.Description);
                    parameters[8] = Database.GetParameter("@Document", scripts.Document);
                    parameters[9] = Database.GetParameter("@Date", scripts.Date.HasValue ? (object)scripts.Date.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@IsActive", scripts.IsActive.HasValue ? (object)scripts.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for scriptss that match the search criteria.
        /// </summary>
        /// <param name="fkscripttypeid">The fkscripttypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for scriptss based on the search criteria.</returns>
        internal static string Search(long? fkscripttypeid, long? fkcampaignid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fklanguageid, string description, byte[] document, DateTime? date, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkscripttypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[FKScriptTypeID] = " + fkscripttypeid + "");
            }
            if (fkcampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[FKCampaignID] = " + fkcampaignid + "");
            }
            if (fkcampaigntypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[FKCampaignTypeID] = " + fkcampaigntypeid + "");
            }
            if (fkcampaigngroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[FKCampaignGroupID] = " + fkcampaigngroupid + "");
            }
            if (fkcampaigntypegroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[FKCampaignTypeGroupID] = " + fkcampaigntypegroupid + "");
            }
            if (fkcampaigngrouptypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[FKCampaignGroupTypeID] = " + fkcampaigngrouptypeid + "");
            }
            if (fklanguageid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[FKLanguageID] = " + fklanguageid + "");
            }
            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (document != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[Document] = " + document + "");
            }
            if (date != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[Date] = '" + date.Value.ToString(Database.DateFormat) + "'");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Scripts].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [Scripts].[ID], [Scripts].[FKScriptTypeID], [Scripts].[FKCampaignID], [Scripts].[FKCampaignTypeID], [Scripts].[FKCampaignGroupID], [Scripts].[FKCampaignTypeGroupID], [Scripts].[FKCampaignGroupTypeID], [Scripts].[FKLanguageID], [Scripts].[Description], [Scripts].[Document], [Scripts].[Date], [Scripts].[IsActive], [Scripts].[StampDate], [Scripts].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Scripts].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Scripts] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
