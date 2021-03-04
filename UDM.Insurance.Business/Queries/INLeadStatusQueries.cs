using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inleadstatus objects.
    /// </summary>
    internal abstract partial class INLeadStatusQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inleadstatus from the database.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus object to delete.</param>
        /// <returns>A query that can be used to delete the inleadstatus from the database.</returns>
        internal static string Delete(INLeadStatus inleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadstatus != null)
            {
                query = "INSERT INTO [zHstINLeadStatus] ([ID], [Description], [IsActive], [CodeNumber]) SELECT [ID], [Description], [IsActive], [CodeNumber] FROM [INLeadStatus] WHERE [INLeadStatus].[ID] = @ID; ";
                query += "DELETE FROM [INLeadStatus] WHERE [INLeadStatus].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadstatus.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inleadstatus from the database.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inleadstatus from the database.</returns>
        internal static string DeleteHistory(INLeadStatus inleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadstatus != null)
            {
                query = "DELETE FROM [zHstINLeadStatus] WHERE [zHstINLeadStatus].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadstatus.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inleadstatus from the database.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus object to undelete.</param>
        /// <returns>A query that can be used to undelete the inleadstatus from the database.</returns>
        internal static string UnDelete(INLeadStatus inleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadstatus != null)
            {
                query = "INSERT INTO [INLeadStatus] ([ID], [Description], [IsActive], [CodeNumber]) SELECT [ID], [Description], [IsActive], [CodeNumber] FROM [zHstINLeadStatus] WHERE [zHstINLeadStatus].[ID] = @ID AND [zHstINLeadStatus].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLeadStatus] WHERE [zHstINLeadStatus].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INLeadStatus] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINLeadStatus] WHERE [zHstINLeadStatus].[ID] = @ID AND [zHstINLeadStatus].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLeadStatus] WHERE [zHstINLeadStatus].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INLeadStatus] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadstatus.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inleadstatus object.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus object to fill.</param>
        /// <returns>A query that can be used to fill the inleadstatus object.</returns>
        internal static string Fill(INLeadStatus inleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadstatus != null)
            {
                query = "SELECT [ID], [Description], [IsActive], [CodeNumber] FROM [INLeadStatus] WHERE [INLeadStatus].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadstatus.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inleadstatus data.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inleadstatus data.</returns>
        internal static string FillData(INLeadStatus inleadstatus, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadstatus != null)
            {
            query.Append("SELECT [INLeadStatus].[ID], [INLeadStatus].[Description], [INLeadStatus].[IsActive], [INLeadStatus].[CodeNumber]");
            query.Append(" FROM [INLeadStatus] ");
                query.Append(" WHERE [INLeadStatus].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadstatus.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inleadstatus object from history.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inleadstatus object from history.</returns>
        internal static string FillHistory(INLeadStatus inleadstatus, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadstatus != null)
            {
                query = "SELECT [ID], [Description], [IsActive], [CodeNumber] FROM [zHstINLeadStatus] WHERE [zHstINLeadStatus].[ID] = @ID AND [zHstINLeadStatus].[StampUserID] = @StampUserID AND [zHstINLeadStatus].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inleadstatus.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inleadstatuss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inleadstatuss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INLeadStatus].[ID], [INLeadStatus].[Description], [INLeadStatus].[IsActive], [INLeadStatus].[CodeNumber]");
            query.Append(" FROM [INLeadStatus] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inleadstatuss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inleadstatuss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINLeadStatus].[ID], [zHstINLeadStatus].[Description], [zHstINLeadStatus].[IsActive], [zHstINLeadStatus].[CodeNumber]");
            query.Append(" FROM [zHstINLeadStatus] ");
            query.Append("INNER JOIN (SELECT [zHstINLeadStatus].[ID], MAX([zHstINLeadStatus].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINLeadStatus] ");
            query.Append("WHERE [zHstINLeadStatus].[ID] NOT IN (SELECT [INLeadStatus].[ID] FROM [INLeadStatus]) ");
            query.Append("GROUP BY [zHstINLeadStatus].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINLeadStatus].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINLeadStatus].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inleadstatus in the database.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inleadstatus in the database.</returns>
        public static string ListHistory(INLeadStatus inleadstatus, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadstatus != null)
            {
            query.Append("SELECT [zHstINLeadStatus].[ID], [zHstINLeadStatus].[Description], [zHstINLeadStatus].[IsActive], [zHstINLeadStatus].[CodeNumber]");
            query.Append(" FROM [zHstINLeadStatus] ");
                query.Append(" WHERE [zHstINLeadStatus].[ID] = @ID");
                query.Append(" ORDER BY [zHstINLeadStatus].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadstatus.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inleadstatus to the database.
        /// </summary>
        /// <param name="inleadstatus">The inleadstatus to save.</param>
        /// <returns>A query that can be used to save the inleadstatus to the database.</returns>
        internal static string Save(INLeadStatus inleadstatus, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadstatus != null)
            {
                if (inleadstatus.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINLeadStatus] ([ID], [Description], [IsActive], [CodeNumber]) SELECT [ID], [Description], [IsActive], [CodeNumber] FROM [INLeadStatus] WHERE [INLeadStatus].[ID] = @ID; ");
                    query.Append("UPDATE [INLeadStatus]");
                    parameters = new object[4];
                    query.Append(" SET [Description] = @Description");
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inleadstatus.Description) ? DBNull.Value : (object)inleadstatus.Description);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[1] = Database.GetParameter("@IsActive", inleadstatus.IsActive.HasValue ? (object)inleadstatus.IsActive.Value : DBNull.Value);
                    query.Append(", [CodeNumber] = @CodeNumber");
                    parameters[2] = Database.GetParameter("@CodeNumber", string.IsNullOrEmpty(inleadstatus.CodeNumber) ? DBNull.Value : (object)inleadstatus.CodeNumber);
                    query.Append(" WHERE [INLeadStatus].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", inleadstatus.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INLeadStatus] ([Description], [IsActive], [CodeNumber]) VALUES(@Description, @IsActive, @CodeNumber);");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inleadstatus.Description) ? DBNull.Value : (object)inleadstatus.Description);
                    parameters[1] = Database.GetParameter("@IsActive", inleadstatus.IsActive.HasValue ? (object)inleadstatus.IsActive.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@CodeNumber", string.IsNullOrEmpty(inleadstatus.CodeNumber) ? DBNull.Value : (object)inleadstatus.CodeNumber);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inleadstatuss that match the search criteria.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <param name="codenumber">The codenumber search criteria.</param>
        /// <returns>A query that can be used to search for inleadstatuss based on the search criteria.</returns>
        internal static string Search(string description, bool? isactive, string codenumber)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadStatus].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadStatus].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            if (codenumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadStatus].[CodeNumber] LIKE '" + codenumber.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INLeadStatus].[ID], [INLeadStatus].[Description], [INLeadStatus].[IsActive], [INLeadStatus].[CodeNumber]");
            query.Append(" FROM [INLeadStatus] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
