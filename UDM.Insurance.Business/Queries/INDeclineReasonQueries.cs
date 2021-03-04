using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to indeclinereason objects.
    /// </summary>
    internal abstract partial class INDeclineReasonQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) indeclinereason from the database.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason object to delete.</param>
        /// <returns>A query that can be used to delete the indeclinereason from the database.</returns>
        internal static string Delete(INDeclineReason indeclinereason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indeclinereason != null)
            {
                query = "INSERT INTO [zHstINDeclineReason] ([ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID]) SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [INDeclineReason] WHERE [INDeclineReason].[ID] = @ID; ";
                query += "DELETE FROM [INDeclineReason] WHERE [INDeclineReason].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indeclinereason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) indeclinereason from the database.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the indeclinereason from the database.</returns>
        internal static string DeleteHistory(INDeclineReason indeclinereason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indeclinereason != null)
            {
                query = "DELETE FROM [zHstINDeclineReason] WHERE [zHstINDeclineReason].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indeclinereason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) indeclinereason from the database.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason object to undelete.</param>
        /// <returns>A query that can be used to undelete the indeclinereason from the database.</returns>
        internal static string UnDelete(INDeclineReason indeclinereason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indeclinereason != null)
            {
                query = "INSERT INTO [INDeclineReason] ([ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID]) SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [zHstINDeclineReason] WHERE [zHstINDeclineReason].[ID] = @ID AND [zHstINDeclineReason].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDeclineReason] WHERE [zHstINDeclineReason].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INDeclineReason] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINDeclineReason] WHERE [zHstINDeclineReason].[ID] = @ID AND [zHstINDeclineReason].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDeclineReason] WHERE [zHstINDeclineReason].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INDeclineReason] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indeclinereason.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an indeclinereason object.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason object to fill.</param>
        /// <returns>A query that can be used to fill the indeclinereason object.</returns>
        internal static string Fill(INDeclineReason indeclinereason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indeclinereason != null)
            {
                query = "SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [INDeclineReason] WHERE [INDeclineReason].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indeclinereason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  indeclinereason data.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  indeclinereason data.</returns>
        internal static string FillData(INDeclineReason indeclinereason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indeclinereason != null)
            {
            query.Append("SELECT [INDeclineReason].[ID], [INDeclineReason].[Code], [INDeclineReason].[Description], [INDeclineReason].[CodeNumber], [INDeclineReason].[StampDate], [INDeclineReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDeclineReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDeclineReason] ");
                query.Append(" WHERE [INDeclineReason].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indeclinereason.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an indeclinereason object from history.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the indeclinereason object from history.</returns>
        internal static string FillHistory(INDeclineReason indeclinereason, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (indeclinereason != null)
            {
                query = "SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [zHstINDeclineReason] WHERE [zHstINDeclineReason].[ID] = @ID AND [zHstINDeclineReason].[StampUserID] = @StampUserID AND [zHstINDeclineReason].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", indeclinereason.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the indeclinereasons in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the indeclinereasons in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INDeclineReason].[ID], [INDeclineReason].[Code], [INDeclineReason].[Description], [INDeclineReason].[CodeNumber], [INDeclineReason].[StampDate], [INDeclineReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDeclineReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDeclineReason] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted indeclinereasons in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted indeclinereasons in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINDeclineReason].[ID], [zHstINDeclineReason].[Code], [zHstINDeclineReason].[Description], [zHstINDeclineReason].[CodeNumber], [zHstINDeclineReason].[StampDate], [zHstINDeclineReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDeclineReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDeclineReason] ");
            query.Append("INNER JOIN (SELECT [zHstINDeclineReason].[ID], MAX([zHstINDeclineReason].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINDeclineReason] ");
            query.Append("WHERE [zHstINDeclineReason].[ID] NOT IN (SELECT [INDeclineReason].[ID] FROM [INDeclineReason]) ");
            query.Append("GROUP BY [zHstINDeclineReason].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINDeclineReason].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINDeclineReason].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) indeclinereason in the database.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) indeclinereason in the database.</returns>
        public static string ListHistory(INDeclineReason indeclinereason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indeclinereason != null)
            {
            query.Append("SELECT [zHstINDeclineReason].[ID], [zHstINDeclineReason].[Code], [zHstINDeclineReason].[Description], [zHstINDeclineReason].[CodeNumber], [zHstINDeclineReason].[StampDate], [zHstINDeclineReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDeclineReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDeclineReason] ");
                query.Append(" WHERE [zHstINDeclineReason].[ID] = @ID");
                query.Append(" ORDER BY [zHstINDeclineReason].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indeclinereason.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) indeclinereason to the database.
        /// </summary>
        /// <param name="indeclinereason">The indeclinereason to save.</param>
        /// <returns>A query that can be used to save the indeclinereason to the database.</returns>
        internal static string Save(INDeclineReason indeclinereason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indeclinereason != null)
            {
                if (indeclinereason.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINDeclineReason] ([ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID]) SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [INDeclineReason] WHERE [INDeclineReason].[ID] = @ID; ");
                    query.Append("UPDATE [INDeclineReason]");
                    parameters = new object[4];
                    query.Append(" SET [Code] = @Code");
                    parameters[0] = Database.GetParameter("@Code", string.IsNullOrEmpty(indeclinereason.Code) ? DBNull.Value : (object)indeclinereason.Code);
                    query.Append(", [Description] = @Description");
                    parameters[1] = Database.GetParameter("@Description", string.IsNullOrEmpty(indeclinereason.Description) ? DBNull.Value : (object)indeclinereason.Description);
                    query.Append(", [CodeNumber] = @CodeNumber");
                    parameters[2] = Database.GetParameter("@CodeNumber", string.IsNullOrEmpty(indeclinereason.CodeNumber) ? DBNull.Value : (object)indeclinereason.CodeNumber);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INDeclineReason].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", indeclinereason.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INDeclineReason] ([Code], [Description], [CodeNumber], [StampDate], [StampUserID]) VALUES(@Code, @Description, @CodeNumber, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@Code", string.IsNullOrEmpty(indeclinereason.Code) ? DBNull.Value : (object)indeclinereason.Code);
                    parameters[1] = Database.GetParameter("@Description", string.IsNullOrEmpty(indeclinereason.Description) ? DBNull.Value : (object)indeclinereason.Description);
                    parameters[2] = Database.GetParameter("@CodeNumber", string.IsNullOrEmpty(indeclinereason.CodeNumber) ? DBNull.Value : (object)indeclinereason.CodeNumber);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for indeclinereasons that match the search criteria.
        /// </summary>
        /// <param name="code">The code search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <returns>A query that can be used to search for indeclinereasons based on the search criteria.</returns>
        internal static string Search(string code, string description, string codenumber)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (code != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDeclineReason].[Code] LIKE '" + code.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDeclineReason].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (codenumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDeclineReason].[CodeNumber] LIKE '" + codenumber.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INDeclineReason].[ID], [INDeclineReason].[Code], [INDeclineReason].[Description], [INDeclineReason].[CodeNumber], [INDeclineReason].[StampDate], [INDeclineReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDeclineReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDeclineReason] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
