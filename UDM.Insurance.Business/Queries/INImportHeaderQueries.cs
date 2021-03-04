using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportheader objects.
    /// </summary>
    internal abstract partial class INImportHeaderQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportheader from the database.
        /// </summary>
        /// <param name="inimportheader">The inimportheader object to delete.</param>
        /// <returns>A query that can be used to delete the inimportheader from the database.</returns>
        internal static string Delete(INImportHeader inimportheader, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportheader != null)
            {
                query = "INSERT INTO [zHstINImportHeader] ([ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID] FROM [INImportHeader] WHERE [INImportHeader].[ID] = @ID; ";
                query += "DELETE FROM [INImportHeader] WHERE [INImportHeader].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportheader.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportheader from the database.
        /// </summary>
        /// <param name="inimportheader">The inimportheader object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportheader from the database.</returns>
        internal static string DeleteHistory(INImportHeader inimportheader, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportheader != null)
            {
                query = "DELETE FROM [zHstINImportHeader] WHERE [zHstINImportHeader].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportheader.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportheader from the database.
        /// </summary>
        /// <param name="inimportheader">The inimportheader object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportheader from the database.</returns>
        internal static string UnDelete(INImportHeader inimportheader, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportheader != null)
            {
                query = "INSERT INTO [INImportHeader] ([ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID] FROM [zHstINImportHeader] WHERE [zHstINImportHeader].[ID] = @ID AND [zHstINImportHeader].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportHeader] WHERE [zHstINImportHeader].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportHeader] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportHeader] WHERE [zHstINImportHeader].[ID] = @ID AND [zHstINImportHeader].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportHeader] WHERE [zHstINImportHeader].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportHeader] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportheader.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportheader object.
        /// </summary>
        /// <param name="inimportheader">The inimportheader object to fill.</param>
        /// <returns>A query that can be used to fill the inimportheader object.</returns>
        internal static string Fill(INImportHeader inimportheader, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportheader != null)
            {
                query = "SELECT [ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID] FROM [INImportHeader] WHERE [INImportHeader].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportheader.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportheader data.
        /// </summary>
        /// <param name="inimportheader">The inimportheader to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportheader data.</returns>
        internal static string FillData(INImportHeader inimportheader, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportheader != null)
            {
            query.Append("SELECT [INImportHeader].[ID], [INImportHeader].[Name], [INImportHeader].[Header], [INImportHeader].[HeaderAlt1], [INImportHeader].[HeaderAlt2], [INImportHeader].[HeaderAlt3], [INImportHeader].[TableName], [INImportHeader].[IgnoreIfNA], [INImportHeader].[IsActive], [INImportHeader].[StampDate], [INImportHeader].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportHeader].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportHeader] ");
                query.Append(" WHERE [INImportHeader].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportheader.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportheader object from history.
        /// </summary>
        /// <param name="inimportheader">The inimportheader object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportheader object from history.</returns>
        internal static string FillHistory(INImportHeader inimportheader, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportheader != null)
            {
                query = "SELECT [ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID] FROM [zHstINImportHeader] WHERE [zHstINImportHeader].[ID] = @ID AND [zHstINImportHeader].[StampUserID] = @StampUserID AND [zHstINImportHeader].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportheader.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportheaders in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportheaders in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportHeader].[ID], [INImportHeader].[Name], [INImportHeader].[Header], [INImportHeader].[HeaderAlt1], [INImportHeader].[HeaderAlt2], [INImportHeader].[HeaderAlt3], [INImportHeader].[TableName], [INImportHeader].[IgnoreIfNA], [INImportHeader].[IsActive], [INImportHeader].[StampDate], [INImportHeader].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportHeader].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportHeader] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportheaders in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportheaders in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportHeader].[ID], [zHstINImportHeader].[Name], [zHstINImportHeader].[Header], [zHstINImportHeader].[HeaderAlt1], [zHstINImportHeader].[HeaderAlt2], [zHstINImportHeader].[HeaderAlt3], [zHstINImportHeader].[TableName], [zHstINImportHeader].[IgnoreIfNA], [zHstINImportHeader].[IsActive], [zHstINImportHeader].[StampDate], [zHstINImportHeader].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportHeader].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportHeader] ");
            query.Append("INNER JOIN (SELECT [zHstINImportHeader].[ID], MAX([zHstINImportHeader].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportHeader] ");
            query.Append("WHERE [zHstINImportHeader].[ID] NOT IN (SELECT [INImportHeader].[ID] FROM [INImportHeader]) ");
            query.Append("GROUP BY [zHstINImportHeader].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportHeader].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportHeader].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportheader in the database.
        /// </summary>
        /// <param name="inimportheader">The inimportheader object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportheader in the database.</returns>
        public static string ListHistory(INImportHeader inimportheader, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportheader != null)
            {
            query.Append("SELECT [zHstINImportHeader].[ID], [zHstINImportHeader].[Name], [zHstINImportHeader].[Header], [zHstINImportHeader].[HeaderAlt1], [zHstINImportHeader].[HeaderAlt2], [zHstINImportHeader].[HeaderAlt3], [zHstINImportHeader].[TableName], [zHstINImportHeader].[IgnoreIfNA], [zHstINImportHeader].[IsActive], [zHstINImportHeader].[StampDate], [zHstINImportHeader].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportHeader].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportHeader] ");
                query.Append(" WHERE [zHstINImportHeader].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportHeader].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportheader.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportheader to the database.
        /// </summary>
        /// <param name="inimportheader">The inimportheader to save.</param>
        /// <returns>A query that can be used to save the inimportheader to the database.</returns>
        internal static string Save(INImportHeader inimportheader, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportheader != null)
            {
                if (inimportheader.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportHeader] ([ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID] FROM [INImportHeader] WHERE [INImportHeader].[ID] = @ID; ");
                    query.Append("UPDATE [INImportHeader]");
                    parameters = new object[9];
                    query.Append(" SET [Name] = @Name");
                    parameters[0] = Database.GetParameter("@Name", string.IsNullOrEmpty(inimportheader.Name) ? DBNull.Value : (object)inimportheader.Name);
                    query.Append(", [Header] = @Header");
                    parameters[1] = Database.GetParameter("@Header", string.IsNullOrEmpty(inimportheader.Header) ? DBNull.Value : (object)inimportheader.Header);
                    query.Append(", [HeaderAlt1] = @HeaderAlt1");
                    parameters[2] = Database.GetParameter("@HeaderAlt1", string.IsNullOrEmpty(inimportheader.HeaderAlt1) ? DBNull.Value : (object)inimportheader.HeaderAlt1);
                    query.Append(", [HeaderAlt2] = @HeaderAlt2");
                    parameters[3] = Database.GetParameter("@HeaderAlt2", string.IsNullOrEmpty(inimportheader.HeaderAlt2) ? DBNull.Value : (object)inimportheader.HeaderAlt2);
                    query.Append(", [HeaderAlt3] = @HeaderAlt3");
                    parameters[4] = Database.GetParameter("@HeaderAlt3", string.IsNullOrEmpty(inimportheader.HeaderAlt3) ? DBNull.Value : (object)inimportheader.HeaderAlt3);
                    query.Append(", [TableName] = @TableName");
                    parameters[5] = Database.GetParameter("@TableName", string.IsNullOrEmpty(inimportheader.TableName) ? DBNull.Value : (object)inimportheader.TableName);
                    query.Append(", [IgnoreIfNA] = @IgnoreIfNA");
                    parameters[6] = Database.GetParameter("@IgnoreIfNA", inimportheader.IgnoreIfNA.HasValue ? (object)inimportheader.IgnoreIfNA.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[7] = Database.GetParameter("@IsActive", inimportheader.IsActive.HasValue ? (object)inimportheader.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportHeader].[ID] = @ID"); 
                    parameters[8] = Database.GetParameter("@ID", inimportheader.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportHeader] ([Name], [Header], [HeaderAlt1], [HeaderAlt2], [HeaderAlt3], [TableName], [IgnoreIfNA], [IsActive], [StampDate], [StampUserID]) VALUES(@Name, @Header, @HeaderAlt1, @HeaderAlt2, @HeaderAlt3, @TableName, @IgnoreIfNA, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[8];
                    parameters[0] = Database.GetParameter("@Name", string.IsNullOrEmpty(inimportheader.Name) ? DBNull.Value : (object)inimportheader.Name);
                    parameters[1] = Database.GetParameter("@Header", string.IsNullOrEmpty(inimportheader.Header) ? DBNull.Value : (object)inimportheader.Header);
                    parameters[2] = Database.GetParameter("@HeaderAlt1", string.IsNullOrEmpty(inimportheader.HeaderAlt1) ? DBNull.Value : (object)inimportheader.HeaderAlt1);
                    parameters[3] = Database.GetParameter("@HeaderAlt2", string.IsNullOrEmpty(inimportheader.HeaderAlt2) ? DBNull.Value : (object)inimportheader.HeaderAlt2);
                    parameters[4] = Database.GetParameter("@HeaderAlt3", string.IsNullOrEmpty(inimportheader.HeaderAlt3) ? DBNull.Value : (object)inimportheader.HeaderAlt3);
                    parameters[5] = Database.GetParameter("@TableName", string.IsNullOrEmpty(inimportheader.TableName) ? DBNull.Value : (object)inimportheader.TableName);
                    parameters[6] = Database.GetParameter("@IgnoreIfNA", inimportheader.IgnoreIfNA.HasValue ? (object)inimportheader.IgnoreIfNA.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@IsActive", inimportheader.IsActive.HasValue ? (object)inimportheader.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportheaders that match the search criteria.
        /// </summary>
        /// <param name="name">The name search criteria.</param>
        /// <param name="header">The header search criteria.</param>
        /// <param name="headeralt1">The headeralt1 search criteria.</param>
        /// <param name="headeralt2">The headeralt2 search criteria.</param>
        /// <param name="headeralt3">The headeralt3 search criteria.</param>
        /// <param name="tablename">The tablename search criteria.</param>
        /// <param name="ignoreifna">The ignoreifna search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for inimportheaders based on the search criteria.</returns>
        internal static string Search(string name, string header, string headeralt1, string headeralt2, string headeralt3, string tablename, bool? ignoreifna, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (name != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[Name] LIKE '" + name.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (header != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[Header] LIKE '" + header.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (headeralt1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[HeaderAlt1] LIKE '" + headeralt1.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (headeralt2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[HeaderAlt2] LIKE '" + headeralt2.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (headeralt3 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[HeaderAlt3] LIKE '" + headeralt3.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (tablename != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[TableName] LIKE '" + tablename.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (ignoreifna != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[IgnoreIfNA] = " + ((bool)ignoreifna ? "1" : "0"));
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportHeader].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INImportHeader].[ID], [INImportHeader].[Name], [INImportHeader].[Header], [INImportHeader].[HeaderAlt1], [INImportHeader].[HeaderAlt2], [INImportHeader].[HeaderAlt3], [INImportHeader].[TableName], [INImportHeader].[IgnoreIfNA], [INImportHeader].[IsActive], [INImportHeader].[StampDate], [INImportHeader].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportHeader].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportHeader] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
