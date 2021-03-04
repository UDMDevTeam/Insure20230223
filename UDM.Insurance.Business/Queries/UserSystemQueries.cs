using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to usersystem objects.
    /// </summary>
    internal abstract partial class UserSystemQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) usersystem from the database.
        /// </summary>
        /// <param name="usersystem">The usersystem object to delete.</param>
        /// <returns>A query that can be used to delete the usersystem from the database.</returns>
        internal static string Delete(UserSystem usersystem, ref object[] parameters)
        {
            string query = string.Empty;
            if (usersystem != null)
            {
                query = "INSERT INTO [zHstUserSystem] ([ID], [FKUserID], [FKSystemID]) SELECT [ID], [FKUserID], [FKSystemID] FROM [UserSystem] WHERE [UserSystem].[ID] = @ID; ";
                query += "DELETE FROM [UserSystem] WHERE [UserSystem].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", usersystem.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) usersystem from the database.
        /// </summary>
        /// <param name="usersystem">The usersystem object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the usersystem from the database.</returns>
        internal static string DeleteHistory(UserSystem usersystem, ref object[] parameters)
        {
            string query = string.Empty;
            if (usersystem != null)
            {
                query = "DELETE FROM [zHstUserSystem] WHERE [zHstUserSystem].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", usersystem.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) usersystem from the database.
        /// </summary>
        /// <param name="usersystem">The usersystem object to undelete.</param>
        /// <returns>A query that can be used to undelete the usersystem from the database.</returns>
        internal static string UnDelete(UserSystem usersystem, ref object[] parameters)
        {
            string query = string.Empty;
            if (usersystem != null)
            {
                query = "INSERT INTO [UserSystem] ([ID], [FKUserID], [FKSystemID]) SELECT [ID], [FKUserID], [FKSystemID] FROM [zHstUserSystem] WHERE [zHstUserSystem].[ID] = @ID AND [zHstUserSystem].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstUserSystem] WHERE [zHstUserSystem].[ID] = @ID) AND (SELECT COUNT(ID) FROM [UserSystem] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstUserSystem] WHERE [zHstUserSystem].[ID] = @ID AND [zHstUserSystem].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstUserSystem] WHERE [zHstUserSystem].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [UserSystem] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", usersystem.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an usersystem object.
        /// </summary>
        /// <param name="usersystem">The usersystem object to fill.</param>
        /// <returns>A query that can be used to fill the usersystem object.</returns>
        internal static string Fill(UserSystem usersystem, ref object[] parameters)
        {
            string query = string.Empty;
            if (usersystem != null)
            {
                query = "SELECT [ID], [FKUserID], [FKSystemID] FROM [UserSystem] WHERE [UserSystem].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", usersystem.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  usersystem data.
        /// </summary>
        /// <param name="usersystem">The usersystem to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  usersystem data.</returns>
        internal static string FillData(UserSystem usersystem, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (usersystem != null)
            {
            query.Append("SELECT [UserSystem].[ID], [UserSystem].[FKUserID], [UserSystem].[FKSystemID]");
            query.Append(" FROM [UserSystem] ");
                query.Append(" WHERE [UserSystem].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", usersystem.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an usersystem object from history.
        /// </summary>
        /// <param name="usersystem">The usersystem object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the usersystem object from history.</returns>
        internal static string FillHistory(UserSystem usersystem, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (usersystem != null)
            {
                query = "SELECT [ID], [FKUserID], [FKSystemID] FROM [zHstUserSystem] WHERE [zHstUserSystem].[ID] = @ID AND [zHstUserSystem].[StampUserID] = @StampUserID AND [zHstUserSystem].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", usersystem.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the usersystems in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the usersystems in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [UserSystem].[ID], [UserSystem].[FKUserID], [UserSystem].[FKSystemID]");
            query.Append(" FROM [UserSystem] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted usersystems in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted usersystems in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstUserSystem].[ID], [zHstUserSystem].[FKUserID], [zHstUserSystem].[FKSystemID]");
            query.Append(" FROM [zHstUserSystem] ");
            query.Append("INNER JOIN (SELECT [zHstUserSystem].[ID], MAX([zHstUserSystem].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstUserSystem] ");
            query.Append("WHERE [zHstUserSystem].[ID] NOT IN (SELECT [UserSystem].[ID] FROM [UserSystem]) ");
            query.Append("GROUP BY [zHstUserSystem].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstUserSystem].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstUserSystem].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) usersystem in the database.
        /// </summary>
        /// <param name="usersystem">The usersystem object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) usersystem in the database.</returns>
        public static string ListHistory(UserSystem usersystem, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (usersystem != null)
            {
            query.Append("SELECT [zHstUserSystem].[ID], [zHstUserSystem].[FKUserID], [zHstUserSystem].[FKSystemID]");
            query.Append(" FROM [zHstUserSystem] ");
                query.Append(" WHERE [zHstUserSystem].[ID] = @ID");
                query.Append(" ORDER BY [zHstUserSystem].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", usersystem.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) usersystem to the database.
        /// </summary>
        /// <param name="usersystem">The usersystem to save.</param>
        /// <returns>A query that can be used to save the usersystem to the database.</returns>
        internal static string Save(UserSystem usersystem, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (usersystem != null)
            {
                if (usersystem.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstUserSystem] ([ID], [FKUserID], [FKSystemID]) SELECT [ID], [FKUserID], [FKSystemID] FROM [UserSystem] WHERE [UserSystem].[ID] = @ID; ");
                    query.Append("UPDATE [UserSystem]");
                    parameters = new object[3];
                    query.Append(" SET [FKUserID] = @FKUserID");
                    parameters[0] = Database.GetParameter("@FKUserID", usersystem.FKUserID);
                    query.Append(", [FKSystemID] = @FKSystemID");
                    parameters[1] = Database.GetParameter("@FKSystemID", usersystem.FKSystemID);
                    query.Append(" WHERE [UserSystem].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", usersystem.ID);
                }
                else
                {
                    query.Append("INSERT INTO [UserSystem] ([FKUserID], [FKSystemID]) VALUES(@FKUserID, @FKSystemID);");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKUserID", usersystem.FKUserID);
                    parameters[1] = Database.GetParameter("@FKSystemID", usersystem.FKSystemID);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for usersystems that match the search criteria.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <returns>A query that can be used to search for usersystems based on the search criteria.</returns>
        internal static string Search(long? fkuserid, long? fksystemid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserSystem].[FKUserID] = " + fkuserid + "");
            }
            if (fksystemid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserSystem].[FKSystemID] = " + fksystemid + "");
            }
            query.Append("SELECT [UserSystem].[ID], [UserSystem].[FKUserID], [UserSystem].[FKSystemID]");
            query.Append(" FROM [UserSystem] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
