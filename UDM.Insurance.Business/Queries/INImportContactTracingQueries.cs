using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to iNImportContactTracing objects.
    /// </summary>
    internal abstract partial class INImportContactTracingQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) iNImportContactTracing from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing object to delete.</param>
        /// <returns>A query that can be used to delete the iNImportContactTracing from the database.</returns>
        internal static string Delete(INImportContactTracing iNImportContactTracing, ref object[] parameters)
        {
            string query = string.Empty;
            if (iNImportContactTracing != null)
            {
                query = "INSERT INTO [zHstINImportContactTracing] ([ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5], [Contact6], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5],[Contact6], [StampDate], [StampUserID] FROM [INImportContactTracing] WHERE [INImportContactTracing].[ID] = @ID; ";
                query += "DELETE FROM [INImportContactTracing] WHERE [INImportContactTracing].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportContactTracing.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) iNImportContactTracing from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the iNImportContactTracing from the database.</returns>
        internal static string DeleteHistory(INImportContactTracing iNImportContactTracing, ref object[] parameters)
        {
            string query = string.Empty;
            if (iNImportContactTracing != null)
            {
                query = "DELETE FROM [zHstINImportContactTracing] WHERE [zHstINImportContactTracing].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportContactTracing.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) iNImportContactTracing from the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing object to undelete.</param>
        /// <returns>A query that can be used to undelete the iNImportContactTracing from the database.</returns>
        internal static string UnDelete(INImportContactTracing iNImportContactTracing, ref object[] parameters)
        {
            string query = string.Empty;
            if (iNImportContactTracing != null)
            {
                query = "INSERT INTO [INImportContactTracing] ([ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4],[Contact5], [Contact6], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5], [Contact6], [StampDate], [StampUserID] FROM [zHstINImportContactTracing] WHERE [zHstINImportContactTracing].[ID] = @ID AND [zHstINImportContactTracing].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportContactTracing] WHERE [zHstINImportContactTracing].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportContactTracing] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportContactTracing WHERE [zHstINImportContactTracing].[ID] = @ID AND [zHstINImportContactTracing].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportContactTracing] WHERE [zHstINImportContactTracing].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportContactTracing] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportContactTracing.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an iNImportContactTracing object.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing object to fill.</param>
        /// <returns>A query that can be used to fill the iNImportContactTracing object.</returns>
        internal static string Fill(INImportContactTracing iNImportContactTracing, ref object[] parameters)
        {
            string query = string.Empty;
            if (iNImportContactTracing != null)
            {
                query = "SELECT [ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5], [Contact6], [StampDate], [StampUserID] FROM [INImportContactTracing] WHERE [INImportContactTracing].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportContactTracing.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  iNImportContactTracing data.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  iNImportContactTracing data.</returns>
        internal static string FillData(INImportContactTracing iNImportContactTracing, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (iNImportContactTracing != null)
            {
                query.Append("SELECT [INImportContactTracing].[ID], [INImportContactTracing].[FKINImportID], [INImportContactTracing].[Contact1], [INImportContactTracing].[Contact2], [INImportContactTracing].[Contact3], [INImportContactTracing].[Contact4],  [INImportContactTracing].[Contact5],  [INImportContactTracing].[Contact6], [INImportContactTracing].[StampDate], [INImportContactTracing].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportContactTracing].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INImportContactTracing] ");
                query.Append(" WHERE [INImportContactTracing].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportContactTracing.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an iNImportContactTracing object from history.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the iNImportContactTracing object from history.</returns>
        internal static string FillHistory(INImportContactTracing iNImportContactTracing, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (iNImportContactTracing != null)
            {
                query = "SELECT [ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5], [Contact6], [StampDate], [StampUserID] FROM [zHstINImportContactTracing] WHERE [zHstINImportContactTracing].[ID] = @ID AND [zHstINImportContactTracing].[StampUserID] = @StampUserID AND [zHstINImportContactTracing].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", iNImportContactTracing.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the iNImportContactTracing in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the iNImportContactTracing in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportContactTracing].[ID], [INImportContactTracing].[FKINImportID], [INImportContactTracing].[Contact1], [INImportContactTracing].[Contact2], [INImportContactTracing].[Contact3], [INImportContactTracing].[Contact4], [INImportContactTracing].[Contact5], [INImportContactTracing].[Contact6], [INImportContactTracing].[StampDate], [INImportContactTracing].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportContactTracing].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportContactTracing] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted iNImportContactTracing in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted iNImportContactTracing in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportContactTracing].[ID], [zHstINImportContactTracing].[FKINImportID], [zHstINImportContactTracing].[Contact1], [zHstINImportContactTracing].[Contact2], [zHstINImportContactTracing].[Contact3], [zHstINImportContactTracing].[Contact4], [zHstINImportContactTracing].[Contact5], [zHstINImportContactTracing].[Contact6], [zHstINImportContactTracing].[StampDate], [zHstINImportContactTracing].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportContactTracing].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportContactTracing] ");
            query.Append("INNER JOIN (SELECT [zHstINImportContactTracing].[ID], MAX([zHstINImportContactTracing].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportContactTracing] ");
            query.Append("WHERE [zHstINImportContactTracing].[ID] NOT IN (SELECT [INImportContactTracing].[ID] FROM [INImportContactTracing]) ");
            query.Append("GROUP BY [zHstINImportContactTracing].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportContactTracing].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportContactTracing].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) iNImportContactTracing in the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) iNImportContactTracing in the database.</returns>
        public static string ListHistory(INImportContactTracing iNImportContactTracing, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (iNImportContactTracing != null)
            {
                query.Append("SELECT [zHstINImportContactTracing].[ID], [zHstINImportContactTracing].[FKINImportID], [zHstINImportContactTracing].[Contact1], [zHstINImportContactTracing].[Contact2], [zHstINImportContactTracing].[Contact3], [zHstINImportContactTracing].[Contact4], [zHstINImportContactTracing].[Contact5], [zHstINImportContactTracing].[Contact6], [zHstINImportContactTracing].[StampDate], [zHstINImportContactTracing].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportContactTracing].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [zHstINImportContactTracing] ");
                query.Append(" WHERE [zHstINImportContactTracing].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportContactTracing].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportContactTracing.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) iNImportContactTracing to the database.
        /// </summary>
        /// <param name="iNImportContactTracing">The iNImportContactTracing to save.</param>
        /// <returns>A query that can be used to save the iNImportContactTracing to the database.</returns>
        internal static string Save(INImportContactTracing iNImportContactTracing, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (iNImportContactTracing != null)
            {
                if (iNImportContactTracing.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportContactTracing] ([ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5],[Contact6], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5], [Contact6], [StampDate], [StampUserID] FROM [INImportContactTracing] WHERE [INImportContactTracing].[ID] = @ID; ");
                    query.Append("UPDATE [INImportContactTracing]");
                    parameters = new object[8];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", iNImportContactTracing.FKINImportID.HasValue ? (object)iNImportContactTracing.FKINImportID.Value : DBNull.Value);
                    query.Append(", [Contact1] = @Contact1");
                    parameters[1] = Database.GetParameter("@Contact1", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceOne) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceOne);
                    query.Append(", [Contact2] = @Contact2");
                    parameters[2] = Database.GetParameter("@Contact2", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceTwo) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceTwo);
                    query.Append(", [Contact3] = @Contact3");
                    parameters[3] = Database.GetParameter("@Contact3", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceThree) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceThree);
                    query.Append(", [Contact4] = @Contact4");
                    parameters[4] = Database.GetParameter("@Contact4", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceFour) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceFour);
                    query.Append(", [Contact5] = @Contact5");
                    parameters[5] = Database.GetParameter("@Contact5", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceFive) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceFive);
                    query.Append(", [Contact6] = @Contact6");
                    parameters[6] = Database.GetParameter("@Contact6", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceSix) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceSix);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportContactTracing].[ID] = @ID");
                    parameters[7] = Database.GetParameter("@ID", iNImportContactTracing.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportContactTracing] ([FKINImportID], [Contact1], [Contact2], [Contact3], [Contact4], [Contact5],[Contact6], [StampDate], [StampUserID]) VALUES(@FKINImportID, @Contact1, @Contact2, @Contact3, @Contact4, @Contact5, @Contact6, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[7];
                    parameters[0] = Database.GetParameter("@FKINImportID", iNImportContactTracing.FKINImportID.HasValue ? (object)iNImportContactTracing.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Contact1", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceOne) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceOne);
                    parameters[2] = Database.GetParameter("@Contact2", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceTwo) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceTwo);
                    parameters[3] = Database.GetParameter("@Contact3", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceThree) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceThree);
                    parameters[4] = Database.GetParameter("@Contact4", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceFour) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceFour);
                    parameters[5] = Database.GetParameter("@Contact5", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceFive) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceFive);
                    parameters[6] = Database.GetParameter("@Contact6", string.IsNullOrEmpty(iNImportContactTracing.ContactTraceSix) ? DBNull.Value : (object)iNImportContactTracing.ContactTraceSix);

                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for iNImportContactTracing that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="contactTraceOne">The fkinrelationshipid search criteria.</param>
        /// <param name="contactTraceTwo">The firstname search criteria.</param>
        /// <param name="contactTraceThree">The surname search criteria.</param>
        /// <param name="contactTraceFour">The telcontact search criteria.</param>
        /// <param name="contactTraceFive">The telcontact search criteria.</param>
        /// <param name="contactTraceSix">The telcontact search criteria.</param>

        /// <returns>A query that can be used to search for iNImportContactTracing based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, string contact1, string contact2, string contact3, string contact4, string contact5, string contact6)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportContactTracing].[FKINImportID] = " + fkinimportid + "");
            }
            if (contact1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportContactTracing].[Contact1] LIKE '" + contact1.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (contact2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportContactTracing].[Contact2] LIKE '" + contact2.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (contact3 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportContactTracing].[Contact3] LIKE '" + contact3.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (contact4 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportContactTracing].[Contact4] LIKE '" + contact4.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (contact5 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportContactTracing].[Contact5] LIKE '" + contact5.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (contact6 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportContactTracing].[Contact6] LIKE '" + contact6.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INImportContactTracing].[ID], [INImportContactTracing].[FKINImportID], [INImportContactTracing].[Contact1], [INImportContactTracing].[Contact2], [INImportContactTracing].[Contact3], [INImportContactTracing].[Contact4], [INImportContactTracing].[Contact5],[INImportContactTracing].[Contact6], [INImportContactTracing].[StampDate], [INImportContactTracing].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportContactTracing].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportContactTracing] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
