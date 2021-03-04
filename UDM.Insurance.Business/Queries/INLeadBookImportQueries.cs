using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inleadbookimport objects.
    /// </summary>
    internal abstract partial class INLeadBookImportQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inleadbookimport from the database.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport object to delete.</param>
        /// <returns>A query that can be used to delete the inleadbookimport from the database.</returns>
        internal static string Delete(INLeadBookImport inleadbookimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbookimport != null)
            {
                query = "INSERT INTO [zHstINLeadBookImport] ([ID], [FKINLeadBookID], [FKINImportID]) SELECT [ID], [FKINLeadBookID], [FKINImportID] FROM [INLeadBookImport] WHERE [INLeadBookImport].[ID] = @ID; ";
                query += "DELETE FROM [INLeadBookImport] WHERE [INLeadBookImport].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbookimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inleadbookimport from the database.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inleadbookimport from the database.</returns>
        internal static string DeleteHistory(INLeadBookImport inleadbookimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbookimport != null)
            {
                query = "DELETE FROM [zHstINLeadBookImport] WHERE [zHstINLeadBookImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbookimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inleadbookimport from the database.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport object to undelete.</param>
        /// <returns>A query that can be used to undelete the inleadbookimport from the database.</returns>
        internal static string UnDelete(INLeadBookImport inleadbookimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbookimport != null)
            {
                query = "INSERT INTO [INLeadBookImport] ([ID], [FKINLeadBookID], [FKINImportID]) SELECT [ID], [FKINLeadBookID], [FKINImportID] FROM [zHstINLeadBookImport] WHERE [zHstINLeadBookImport].[ID] = @ID AND [zHstINLeadBookImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLeadBookImport] WHERE [zHstINLeadBookImport].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INLeadBookImport] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINLeadBookImport] WHERE [zHstINLeadBookImport].[ID] = @ID AND [zHstINLeadBookImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLeadBookImport] WHERE [zHstINLeadBookImport].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INLeadBookImport] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbookimport.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inleadbookimport object.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport object to fill.</param>
        /// <returns>A query that can be used to fill the inleadbookimport object.</returns>
        internal static string Fill(INLeadBookImport inleadbookimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbookimport != null)
            {
                query = "SELECT [ID], [FKINLeadBookID], [FKINImportID] FROM [INLeadBookImport] WHERE [INLeadBookImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbookimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inleadbookimport data.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inleadbookimport data.</returns>
        internal static string FillData(INLeadBookImport inleadbookimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadbookimport != null)
            {
            query.Append("SELECT [INLeadBookImport].[ID], [INLeadBookImport].[FKINLeadBookID], [INLeadBookImport].[FKINImportID]");
            query.Append(" FROM [INLeadBookImport] ");
                query.Append(" WHERE [INLeadBookImport].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbookimport.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inleadbookimport object from history.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inleadbookimport object from history.</returns>
        internal static string FillHistory(INLeadBookImport inleadbookimport, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inleadbookimport != null)
            {
                query = "SELECT [ID], [FKINLeadBookID], [FKINImportID] FROM [zHstINLeadBookImport] WHERE [zHstINLeadBookImport].[ID] = @ID AND [zHstINLeadBookImport].[StampUserID] = @StampUserID AND [zHstINLeadBookImport].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inleadbookimport.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inleadbookimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inleadbookimports in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INLeadBookImport].[ID], [INLeadBookImport].[FKINLeadBookID], [INLeadBookImport].[FKINImportID]");
            query.Append(" FROM [INLeadBookImport] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inleadbookimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inleadbookimports in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINLeadBookImport].[ID], [zHstINLeadBookImport].[FKINLeadBookID], [zHstINLeadBookImport].[FKINImportID]");
            query.Append(" FROM [zHstINLeadBookImport] ");
            query.Append("INNER JOIN (SELECT [zHstINLeadBookImport].[ID], MAX([zHstINLeadBookImport].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINLeadBookImport] ");
            query.Append("WHERE [zHstINLeadBookImport].[ID] NOT IN (SELECT [INLeadBookImport].[ID] FROM [INLeadBookImport]) ");
            query.Append("GROUP BY [zHstINLeadBookImport].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINLeadBookImport].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINLeadBookImport].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inleadbookimport in the database.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inleadbookimport in the database.</returns>
        public static string ListHistory(INLeadBookImport inleadbookimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadbookimport != null)
            {
            query.Append("SELECT [zHstINLeadBookImport].[ID], [zHstINLeadBookImport].[FKINLeadBookID], [zHstINLeadBookImport].[FKINImportID]");
            query.Append(" FROM [zHstINLeadBookImport] ");
                query.Append(" WHERE [zHstINLeadBookImport].[ID] = @ID");
                query.Append(" ORDER BY [zHstINLeadBookImport].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inleadbookimport.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inleadbookimport to the database.
        /// </summary>
        /// <param name="inleadbookimport">The inleadbookimport to save.</param>
        /// <returns>A query that can be used to save the inleadbookimport to the database.</returns>
        internal static string Save(INLeadBookImport inleadbookimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inleadbookimport != null)
            {
                if (inleadbookimport.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINLeadBookImport] ([ID], [FKINLeadBookID], [FKINImportID]) SELECT [ID], [FKINLeadBookID], [FKINImportID] FROM [INLeadBookImport] WHERE [INLeadBookImport].[ID] = @ID; ");
                    query.Append("UPDATE [INLeadBookImport]");
                    parameters = new object[3];
                    query.Append(" SET [FKINLeadBookID] = @FKINLeadBookID");
                    parameters[0] = Database.GetParameter("@FKINLeadBookID", inleadbookimport.FKINLeadBookID.HasValue ? (object)inleadbookimport.FKINLeadBookID.Value : DBNull.Value);
                    query.Append(", [FKINImportID] = @FKINImportID");
                    parameters[1] = Database.GetParameter("@FKINImportID", inleadbookimport.FKINImportID.HasValue ? (object)inleadbookimport.FKINImportID.Value : DBNull.Value);
                    query.Append(" WHERE [INLeadBookImport].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", inleadbookimport.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INLeadBookImport] ([FKINLeadBookID], [FKINImportID]) VALUES(@FKINLeadBookID, @FKINImportID);");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKINLeadBookID", inleadbookimport.FKINLeadBookID.HasValue ? (object)inleadbookimport.FKINLeadBookID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINImportID", inleadbookimport.FKINImportID.HasValue ? (object)inleadbookimport.FKINImportID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inleadbookimports that match the search criteria.
        /// </summary>
        /// <param name="fkinleadbookid">The fkinleadbookid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <returns>A query that can be used to search for inleadbookimports based on the search criteria.</returns>
        internal static string Search(long? fkinleadbookid, long? fkinimportid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinleadbookid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadBookImport].[FKINLeadBookID] = " + fkinleadbookid + "");
            }
            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLeadBookImport].[FKINImportID] = " + fkinimportid + "");
            }
            query.Append("SELECT [INLeadBookImport].[ID], [INLeadBookImport].[FKINLeadBookID], [INLeadBookImport].[FKINImportID]");
            query.Append(" FROM [INLeadBookImport] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
