using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to indiaryreason objects.
    /// </summary>
    internal abstract partial class INDiaryReasonQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) indiaryreason from the database.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason object to delete.</param>
        /// <returns>A query that can be used to delete the indiaryreason from the database.</returns>
        internal static string Delete(INDiaryReason indiaryreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indiaryreason != null)
            {
                query = "INSERT INTO [zHstINDiaryReason] ([ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID]) SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [INDiaryReason] WHERE [INDiaryReason].[ID] = @ID; ";
                query += "DELETE FROM [INDiaryReason] WHERE [INDiaryReason].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indiaryreason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) indiaryreason from the database.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the indiaryreason from the database.</returns>
        internal static string DeleteHistory(INDiaryReason indiaryreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indiaryreason != null)
            {
                query = "DELETE FROM [zHstINDiaryReason] WHERE [zHstINDiaryReason].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indiaryreason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) indiaryreason from the database.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason object to undelete.</param>
        /// <returns>A query that can be used to undelete the indiaryreason from the database.</returns>
        internal static string UnDelete(INDiaryReason indiaryreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indiaryreason != null)
            {
                query = "INSERT INTO [INDiaryReason] ([ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID]) SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [zHstINDiaryReason] WHERE [zHstINDiaryReason].[ID] = @ID AND [zHstINDiaryReason].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDiaryReason] WHERE [zHstINDiaryReason].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INDiaryReason] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINDiaryReason] WHERE [zHstINDiaryReason].[ID] = @ID AND [zHstINDiaryReason].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDiaryReason] WHERE [zHstINDiaryReason].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INDiaryReason] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indiaryreason.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an indiaryreason object.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason object to fill.</param>
        /// <returns>A query that can be used to fill the indiaryreason object.</returns>
        internal static string Fill(INDiaryReason indiaryreason, ref object[] parameters)
        {
            string query = string.Empty;
            if (indiaryreason != null)
            {
                query = "SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [INDiaryReason] WHERE [INDiaryReason].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indiaryreason.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  indiaryreason data.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  indiaryreason data.</returns>
        internal static string FillData(INDiaryReason indiaryreason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indiaryreason != null)
            {
            query.Append("SELECT [INDiaryReason].[ID], [INDiaryReason].[Code], [INDiaryReason].[Description], [INDiaryReason].[CodeNumber], [INDiaryReason].[StampDate], [INDiaryReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDiaryReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDiaryReason] ");
                query.Append(" WHERE [INDiaryReason].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indiaryreason.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an indiaryreason object from history.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the indiaryreason object from history.</returns>
        internal static string FillHistory(INDiaryReason indiaryreason, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (indiaryreason != null)
            {
                query = "SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [zHstINDiaryReason] WHERE [zHstINDiaryReason].[ID] = @ID AND [zHstINDiaryReason].[StampUserID] = @StampUserID AND [zHstINDiaryReason].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", indiaryreason.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the indiaryreasons in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the indiaryreasons in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INDiaryReason].[ID], [INDiaryReason].[Code], [INDiaryReason].[Description], [INDiaryReason].[CodeNumber], [INDiaryReason].[StampDate], [INDiaryReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDiaryReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDiaryReason] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted indiaryreasons in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted indiaryreasons in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINDiaryReason].[ID], [zHstINDiaryReason].[Code], [zHstINDiaryReason].[Description], [zHstINDiaryReason].[CodeNumber], [zHstINDiaryReason].[StampDate], [zHstINDiaryReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDiaryReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDiaryReason] ");
            query.Append("INNER JOIN (SELECT [zHstINDiaryReason].[ID], MAX([zHstINDiaryReason].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINDiaryReason] ");
            query.Append("WHERE [zHstINDiaryReason].[ID] NOT IN (SELECT [INDiaryReason].[ID] FROM [INDiaryReason]) ");
            query.Append("GROUP BY [zHstINDiaryReason].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINDiaryReason].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINDiaryReason].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) indiaryreason in the database.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) indiaryreason in the database.</returns>
        public static string ListHistory(INDiaryReason indiaryreason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indiaryreason != null)
            {
            query.Append("SELECT [zHstINDiaryReason].[ID], [zHstINDiaryReason].[Code], [zHstINDiaryReason].[Description], [zHstINDiaryReason].[CodeNumber], [zHstINDiaryReason].[StampDate], [zHstINDiaryReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDiaryReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDiaryReason] ");
                query.Append(" WHERE [zHstINDiaryReason].[ID] = @ID");
                query.Append(" ORDER BY [zHstINDiaryReason].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indiaryreason.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) indiaryreason to the database.
        /// </summary>
        /// <param name="indiaryreason">The indiaryreason to save.</param>
        /// <returns>A query that can be used to save the indiaryreason to the database.</returns>
        internal static string Save(INDiaryReason indiaryreason, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indiaryreason != null)
            {
                if (indiaryreason.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINDiaryReason] ([ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID]) SELECT [ID], [Code], [Description], [CodeNumber], [StampDate], [StampUserID] FROM [INDiaryReason] WHERE [INDiaryReason].[ID] = @ID; ");
                    query.Append("UPDATE [INDiaryReason]");
                    parameters = new object[4];
                    query.Append(" SET [Code] = @Code");
                    parameters[0] = Database.GetParameter("@Code", string.IsNullOrEmpty(indiaryreason.Code) ? DBNull.Value : (object)indiaryreason.Code);
                    query.Append(", [Description] = @Description");
                    parameters[1] = Database.GetParameter("@Description", string.IsNullOrEmpty(indiaryreason.Description) ? DBNull.Value : (object)indiaryreason.Description);
                    query.Append(", [CodeNumber] = @CodeNumber");
                    parameters[2] = Database.GetParameter("@CodeNumber", string.IsNullOrEmpty(indiaryreason.CodeNumber) ? DBNull.Value : (object)indiaryreason.CodeNumber);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INDiaryReason].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", indiaryreason.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INDiaryReason] ([Code], [Description], [CodeNumber], [StampDate], [StampUserID]) VALUES(@Code, @Description, @CodeNumber, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@Code", string.IsNullOrEmpty(indiaryreason.Code) ? DBNull.Value : (object)indiaryreason.Code);
                    parameters[1] = Database.GetParameter("@Description", string.IsNullOrEmpty(indiaryreason.Description) ? DBNull.Value : (object)indiaryreason.Description);
                    parameters[2] = Database.GetParameter("@CodeNumber", string.IsNullOrEmpty(indiaryreason.CodeNumber) ? DBNull.Value : (object)indiaryreason.CodeNumber);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for indiaryreasons that match the search criteria.
        /// </summary>
        /// <param name="code">The code search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <returns>A query that can be used to search for indiaryreasons based on the search criteria.</returns>
        internal static string Search(string code, string description, string codenumber)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (code != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDiaryReason].[Code] LIKE '" + code.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDiaryReason].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (codenumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDiaryReason].[CodeNumber] LIKE '" + codenumber.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INDiaryReason].[ID], [INDiaryReason].[Code], [INDiaryReason].[Description], [INDiaryReason].[CodeNumber], [INDiaryReason].[StampDate], [INDiaryReason].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDiaryReason].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDiaryReason] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
