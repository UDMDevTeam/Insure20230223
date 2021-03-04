using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inleadbook objects.
    /// </summary>
    internal abstract partial class INLeadBookQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inleadbook from the database.
        /// </summary>
        /// <param name="inleadbook">The inleadbook object to delete.</param>
        /// <returns>A query that can be used to delete the inleadbook from the database.</returns>
        internal static string Delete(INLeadBook inleadbook, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbook != null)
            {
                query = "INSERT INTO [zHstINLeadBook] ([ID], [FKUserID], [FKINBatchID], [Description]) SELECT [ID], [FKUserID], [FKINBatchID], [Description] FROM [INLeadBook] WHERE [INLeadBook].[ID] = @ID; ";
                query += "DELETE FROM [INLeadBook] WHERE [INLeadBook].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbook.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inleadbook from the database.
        /// </summary>
        /// <param name="inleadbook">The inleadbook object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inleadbook from the database.</returns>
        internal static string DeleteHistory(INLeadBook inleadbook, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbook != null)
            {
                query = "DELETE FROM [zHstINLeadBook] WHERE [zHstINLeadBook].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbook.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inleadbook from the database.
        /// </summary>
        /// <param name="inleadbook">The inleadbook object to undelete.</param>
        /// <returns>A query that can be used to undelete the inleadbook from the database.</returns>
        internal static string UnDelete(INLeadBook inleadbook, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbook != null)
            {
                query = "INSERT INTO [INLeadBook] ([ID], [FKUserID], [FKINBatchID], [Description]) SELECT [ID], [FKUserID], [FKINBatchID], [Description] FROM [zHstINLeadBook] WHERE [zHstINLeadBook].[ID] = @ID AND [zHstINLeadBook].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLeadBook] WHERE [zHstINLeadBook].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INLeadBook] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINLeadBook] WHERE [zHstINLeadBook].[ID] = @ID AND [zHstINLeadBook].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLeadBook] WHERE [zHstINLeadBook].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INLeadBook] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbook.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inleadbook object.
        /// </summary>
        /// <param name="inleadbook">The inleadbook object to fill.</param>
        /// <returns>A query that can be used to fill the inleadbook object.</returns>
        internal static string Fill(INLeadBook inleadbook, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbook != null)
            {
                query = "SELECT [ID], [FKUserID], [FKINBatchID], [Description] FROM [INLeadBook] WHERE [INLeadBook].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbook.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inleadbook data.
        /// </summary>
        /// <param name="inleadbook">The inleadbook to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inleadbook data.</returns>
        internal static string FillData(INLeadBook inleadbook, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadbook != null)
            {
            query.Append("SELECT [INLeadBook].[ID], [INLeadBook].[FKUserID], [INLeadBook].[FKINBatchID], [INLeadBook].[Description]");
            query.Append(" FROM [INLeadBook] ");
                query.Append(" WHERE [INLeadBook].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbook.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inleadbook object from history.
        /// </summary>
        /// <param name="inleadbook">The inleadbook object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inleadbook object from history.</returns>
        internal static string FillHistory(INLeadBook inleadbook, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbook != null)
            {
                query = "SELECT [ID], [FKUserID], [FKINBatchID], [Description] FROM [zHstINLeadBook] WHERE [zHstINLeadBook].[ID] = @ID AND [zHstINLeadBook].[StampUserID] = @StampUserID AND [zHstINLeadBook].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inleadbook.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inleadbooks in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inleadbooks in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INLeadBook].[ID], [INLeadBook].[FKUserID], [INLeadBook].[FKINBatchID], [INLeadBook].[Description]");
            query.Append(" FROM [INLeadBook] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inleadbooks in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inleadbooks in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINLeadBook].[ID], [zHstINLeadBook].[FKUserID], [zHstINLeadBook].[FKINBatchID], [zHstINLeadBook].[Description]");
            query.Append(" FROM [zHstINLeadBook] ");
            query.Append("INNER JOIN (SELECT [zHstINLeadBook].[ID], MAX([zHstINLeadBook].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINLeadBook] ");
            query.Append("WHERE [zHstINLeadBook].[ID] NOT IN (SELECT [INLeadBook].[ID] FROM [INLeadBook]) ");
            query.Append("GROUP BY [zHstINLeadBook].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINLeadBook].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINLeadBook].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inleadbook in the database.
        /// </summary>
        /// <param name="inleadbook">The inleadbook object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inleadbook in the database.</returns>
        public static string ListHistory(INLeadBook inleadbook, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadbook != null)
            {
            query.Append("SELECT [zHstINLeadBook].[ID], [zHstINLeadBook].[FKUserID], [zHstINLeadBook].[FKINBatchID], [zHstINLeadBook].[Description]");
            query.Append(" FROM [zHstINLeadBook] ");
                query.Append(" WHERE [zHstINLeadBook].[ID] = @ID");
                query.Append(" ORDER BY [zHstINLeadBook].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbook.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inleadbook to the database.
        /// </summary>
        /// <param name="inleadbook">The inleadbook to save.</param>
        /// <returns>A query that can be used to save the inleadbook to the database.</returns>
        internal static string Save(INLeadBook inleadbook, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadbook != null)
            {
                if (inleadbook.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINLeadBook] ([ID], [FKUserID], [FKINBatchID], [Description]) SELECT [ID], [FKUserID], [FKINBatchID], [Description] FROM [INLeadBook] WHERE [INLeadBook].[ID] = @ID; ");
                    query.Append("UPDATE [INLeadBook]");
                    parameters = new object[4];
                    query.Append(" SET [FKUserID] = @FKUserID");
                    parameters[0] = Database.GetParameter("@FKUserID", inleadbook.FKUserID.HasValue ? (object)inleadbook.FKUserID.Value : DBNull.Value);
                    query.Append(", [FKINBatchID] = @FKINBatchID");
                    parameters[1] = Database.GetParameter("@FKINBatchID", inleadbook.FKINBatchID.HasValue ? (object)inleadbook.FKINBatchID.Value : DBNull.Value);
                    query.Append(", [Description] = @Description");
                    parameters[2] = Database.GetParameter("@Description", string.IsNullOrEmpty(inleadbook.Description) ? DBNull.Value : (object)inleadbook.Description);
                    query.Append(" WHERE [INLeadBook].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", inleadbook.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INLeadBook] ([FKUserID], [FKINBatchID], [Description]) VALUES(@FKUserID, @FKINBatchID, @Description);");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@FKUserID", inleadbook.FKUserID.HasValue ? (object)inleadbook.FKUserID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINBatchID", inleadbook.FKINBatchID.HasValue ? (object)inleadbook.FKINBatchID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@Description", string.IsNullOrEmpty(inleadbook.Description) ? DBNull.Value : (object)inleadbook.Description);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inleadbooks that match the search criteria.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinbatchid">The fkinbatchid search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <returns>A query that can be used to search for inleadbooks based on the search criteria.</returns>
        internal static string Search(long? fkuserid, long? fkinbatchid, string description)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadBook].[FKUserID] = " + fkuserid + "");
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadBook].[FKINBatchID] = " + fkinbatchid + "");
            }
            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadBook].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INLeadBook].[ID], [INLeadBook].[FKUserID], [INLeadBook].[FKINBatchID], [INLeadBook].[Description]");
            query.Append(" FROM [INLeadBook] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
