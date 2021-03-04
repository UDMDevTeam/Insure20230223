using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to closure objects.
    /// </summary>
    internal abstract partial class ClosureQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) closure from the database.
        /// </summary>
        /// <param name="closure">The closure object to delete.</param>
        /// <returns>A query that can be used to delete the closure from the database.</returns>
        internal static string Delete(Closure closure, ref object[] parameters)
        {
            string query = string.Empty;
            if (closure != null)
            {
                query = "INSERT INTO [zHstClosure] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [Closure] WHERE [Closure].[ID] = @ID; ";
                query += "DELETE FROM [Closure] WHERE [Closure].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", closure.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) closure from the database.
        /// </summary>
        /// <param name="closure">The closure object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the closure from the database.</returns>
        internal static string DeleteHistory(Closure closure, ref object[] parameters)
        {
            string query = string.Empty;
            if (closure != null)
            {
                query = "DELETE FROM [zHstClosure] WHERE [zHstClosure].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", closure.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) closure from the database.
        /// </summary>
        /// <param name="closure">The closure object to undelete.</param>
        /// <returns>A query that can be used to undelete the closure from the database.</returns>
        internal static string UnDelete(Closure closure, ref object[] parameters)
        {
            string query = string.Empty;
            if (closure != null)
            {
                query = "INSERT INTO [Closure] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [zHstClosure] WHERE [zHstClosure].[ID] = @ID AND [zHstClosure].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstClosure] WHERE [zHstClosure].[ID] = @ID) AND (SELECT COUNT(ID) FROM [Closure] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstClosure] WHERE [zHstClosure].[ID] = @ID AND [zHstClosure].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstClosure] WHERE [zHstClosure].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [Closure] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", closure.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an closure object.
        /// </summary>
        /// <param name="closure">The closure object to fill.</param>
        /// <returns>A query that can be used to fill the closure object.</returns>
        internal static string Fill(Closure closure, ref object[] parameters)
        {
            string query = string.Empty;
            if (closure != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [Closure] WHERE [Closure].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", closure.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  closure data.
        /// </summary>
        /// <param name="closure">The closure to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  closure data.</returns>
        internal static string FillData(Closure closure, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (closure != null)
            {
            query.Append("SELECT [Closure].[ID], [Closure].[FKSystemID], [Closure].[FKCampaignID], [Closure].[FKLanguageID], [Closure].[Document], [Closure].[IsActive], [Closure].[StampDate], [Closure].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Closure].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Closure] ");
                query.Append(" WHERE [Closure].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", closure.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an closure object from history.
        /// </summary>
        /// <param name="closure">The closure object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the closure object from history.</returns>
        internal static string FillHistory(Closure closure, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (closure != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [zHstClosure] WHERE [zHstClosure].[ID] = @ID AND [zHstClosure].[StampUserID] = @StampUserID AND [zHstClosure].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", closure.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the closures in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the closures in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [Closure].[ID], [Closure].[FKSystemID], [Closure].[FKCampaignID], [Closure].[FKLanguageID], [Closure].[Document], [Closure].[IsActive], [Closure].[StampDate], [Closure].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Closure].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Closure] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted closures in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted closures in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstClosure].[ID], [zHstClosure].[FKSystemID], [zHstClosure].[FKCampaignID], [zHstClosure].[FKLanguageID], [zHstClosure].[Document], [zHstClosure].[IsActive], [zHstClosure].[StampDate], [zHstClosure].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstClosure].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstClosure] ");
            query.Append("INNER JOIN (SELECT [zHstClosure].[ID], MAX([zHstClosure].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstClosure] ");
            query.Append("WHERE [zHstClosure].[ID] NOT IN (SELECT [Closure].[ID] FROM [Closure]) ");
            query.Append("GROUP BY [zHstClosure].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstClosure].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstClosure].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) closure in the database.
        /// </summary>
        /// <param name="closure">The closure object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) closure in the database.</returns>
        public static string ListHistory(Closure closure, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (closure != null)
            {
            query.Append("SELECT [zHstClosure].[ID], [zHstClosure].[FKSystemID], [zHstClosure].[FKCampaignID], [zHstClosure].[FKLanguageID], [zHstClosure].[Document], [zHstClosure].[IsActive], [zHstClosure].[StampDate], [zHstClosure].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstClosure].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstClosure] ");
                query.Append(" WHERE [zHstClosure].[ID] = @ID");
                query.Append(" ORDER BY [zHstClosure].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", closure.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) closure to the database.
        /// </summary>
        /// <param name="closure">The closure to save.</param>
        /// <returns>A query that can be used to save the closure to the database.</returns>
        internal static string Save(Closure closure, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (closure != null)
            {
                if (closure.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstClosure] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [Closure] WHERE [Closure].[ID] = @ID; ");
                    query.Append("UPDATE [Closure]");
                    parameters = new object[6];
                    query.Append(" SET [FKSystemID] = @FKSystemID");
                    parameters[0] = Database.GetParameter("@FKSystemID", closure.FKSystemID.HasValue ? (object)closure.FKSystemID.Value : DBNull.Value);
                    query.Append(", [FKCampaignID] = @FKCampaignID");
                    parameters[1] = Database.GetParameter("@FKCampaignID", closure.FKCampaignID.HasValue ? (object)closure.FKCampaignID.Value : DBNull.Value);
                    query.Append(", [FKLanguageID] = @FKLanguageID");
                    parameters[2] = Database.GetParameter("@FKLanguageID", closure.FKLanguageID.HasValue ? (object)closure.FKLanguageID.Value : DBNull.Value);
                    query.Append(", [Document] = @Document");
                    parameters[3] = Database.GetParameter("@Document", closure.Document);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[4] = Database.GetParameter("@IsActive", closure.IsActive.HasValue ? (object)closure.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [Closure].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", closure.ID);
                }
                else
                {
                    query.Append("INSERT INTO [Closure] ([FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) VALUES(@FKSystemID, @FKCampaignID, @FKLanguageID, @Document, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@FKSystemID", closure.FKSystemID.HasValue ? (object)closure.FKSystemID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKCampaignID", closure.FKCampaignID.HasValue ? (object)closure.FKCampaignID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKLanguageID", closure.FKLanguageID.HasValue ? (object)closure.FKLanguageID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@Document", closure.Document);
                    parameters[4] = Database.GetParameter("@IsActive", closure.IsActive.HasValue ? (object)closure.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for closures that match the search criteria.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for closures based on the search criteria.</returns>
        internal static string Search(long? fksystemid, long? fkcampaignid, long? fklanguageid, byte[] document, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fksystemid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Closure].[FKSystemID] = " + fksystemid + "");
            }
            if (fkcampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Closure].[FKCampaignID] = " + fkcampaignid + "");
            }
            if (fklanguageid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Closure].[FKLanguageID] = " + fklanguageid + "");
            }
            if (document != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Closure].[Document] = " + document + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[Closure].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [Closure].[ID], [Closure].[FKSystemID], [Closure].[FKCampaignID], [Closure].[FKLanguageID], [Closure].[Document], [Closure].[IsActive], [Closure].[StampDate], [Closure].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [Closure].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [Closure] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
