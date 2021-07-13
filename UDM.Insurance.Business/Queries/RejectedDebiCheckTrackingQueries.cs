using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to calldata objects.
    /// </summary>
    internal abstract partial class RejectedDebiCheckTrackingQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(RejectedDebiCheckTracking calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstRejectedDebiCheckTracking] ([ID], [FKImportID], [DateTimeSaved], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [DateTimeSaved],[StampDate], [StampUserID] FROM [RejectedDebiCheckTracking] WHERE [RejectedDebiCheckTracking].[ID] = @ID; ";
                query += "DELETE FROM [RejectedDebiCheckTracking] WHERE [RejectedDebiCheckTracking].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the calldata from the database.</returns>
        internal static string DeleteHistory(RejectedDebiCheckTracking calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstRejectedDebiCheckTracking] WHERE [zHstRejectedDebiCheckTracking].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to undelete.</param>
        /// <returns>A query that can be used to undelete the calldata from the database.</returns>
        internal static string UnDelete(RejectedDebiCheckTracking calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [RejectedDebiCheckTracking] ([ID], [FKImportID], [DateTimeSaved], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [DateTimeSaved], [StampDate], [StampUserID] FROM [zHstRejectedDebiCheckTracking] WHERE [zHstRejectedDebiCheckTracking].[ID] = @ID AND [zHstRejectedDebiCheckTracking].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstRejectedDebiCheckTracking] WHERE [zHstRejectedDebiCheckTracking].[ID] = @ID) AND (SELECT COUNT(ID) FROM [RejectedDebiCheckTracking] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstRejectedDebiCheckTracking] WHERE [zHstRejectedDebiCheckTracking].[ID] = @ID AND [zHstRejectedDebiCheckTracking].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstRejectedDebiCheckTracking] WHERE [zHstRejectedDebiCheckTracking].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [RejectedDebiCheckTracking] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an calldata object.
        /// </summary>
        /// <param name="calldata">The calldata object to fill.</param>
        /// <returns>A query that can be used to fill the calldata object.</returns>
        internal static string Fill(RejectedDebiCheckTracking calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [DateTimeSaved], [StampDate], [StampUserID] FROM [RejectedDebiCheckTracking] WHERE [RejectedDebiCheckTracking].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  calldata data.
        /// </summary>
        /// <param name="calldata">The calldata to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  calldata data.</returns>
        internal static string FillData(RejectedDebiCheckTracking calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [RejectedDebiCheckTracking].[ID], [RejectedDebiCheckTracking].[FKImportID], [RejectedDebiCheckTracking].[DateTimeSaved], [RejectedDebiCheckTracking].[StampDate], [RejectedDebiCheckTracking].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [RejectedDebiCheckTracking].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [RejectedDebiCheckTracking] ");
                query.Append(" WHERE [RejectedDebiCheckTracking].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an calldata object from history.
        /// </summary>
        /// <param name="calldata">The calldata object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the calldata object from history.</returns>
        internal static string FillHistory(RejectedDebiCheckTracking calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKImportID], [DateTimeSaved], [StampDate], [StampUserID] FROM [zHstRejectedDebiCheckTracking] WHERE [zHstRejectedDebiCheckTracking].[ID] = @ID AND [zHstRejectedDebiCheckTracking].[StampUserID] = @StampUserID AND [zHstRejectedDebiCheckTracking].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the calldatas in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [RejectedDebiCheckTracking].[ID], [RejectedDebiCheckTracking].[FKImportID], [RejectedDebiCheckTracking].[DateTimeSaved], [RejectedDebiCheckTracking].[StampDate], [RejectedDebiCheckTracking].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [RejectedDebiCheckTracking].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [RejectedDebiCheckTracking] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstRejectedDebiCheckTracking].[ID], [zHstRejectedDebiCheckTracking].[FKImportID], [zHstRejectedDebiCheckTracking].[DateTimeSaved], [zHstRejectedDebiCheckTracking].[StampDate], [zHstRejectedDebiCheckTracking].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstRejectedDebiCheckTracking].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstRejectedDebiCheckTracking] ");
            query.Append("INNER JOIN (SELECT [zHstRejectedDebiCheckTracking].[ID], MAX([zHstRejectedDebiCheckTracking].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstRejectedDebiCheckTracking] ");
            query.Append("WHERE [zHstRejectedDebiCheckTracking].[ID] NOT IN (SELECT [RejectedDebiCheckTracking].[ID] FROM [RejectedDebiCheckTracking]) ");
            query.Append("GROUP BY [zHstRejectedDebiCheckTracking].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstRejectedDebiCheckTracking].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstRejectedDebiCheckTracking].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(RejectedDebiCheckTracking calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstRejectedDebiCheckTracking].[ID], [zHstRejectedDebiCheckTracking].[FKImportID], [zHstRejectedDebiCheckTracking].[DatetImeSaved], [zHstRejectedDebiCheckTracking].[StampDate], [zHstRejectedDebiCheckTracking].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstRejectedDebiCheckTracking].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstRejectedDebiCheckTracking] ");
                query.Append(" WHERE [zHstRejectedDebiCheckTracking].[ID] = @ID");
                query.Append(" ORDER BY [zHstRejectedDebiCheckTracking].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", calldata.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) calldata to the database.
        /// </summary>
        /// <param name="calldata">The calldata to save.</param>
        /// <returns>A query that can be used to save the calldata to the database.</returns>
        internal static string Save(RejectedDebiCheckTracking calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstCallData] ([ID], [FKImportID], [DateTimeSaved], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [DateTimeSaved], [StampDate], [StampUserID] FROM [RejectedDebiCheckTracking] WHERE [RejectedDebiCheckTracking].[ID] = @ID; ");
                    query.Append("UPDATE [RejectedDebiCheckTracking]");
                    parameters = new object[3];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    query.Append(", [DateTimeSaved] = @DateTimeSaved");
                    parameters[1] = Database.GetParameter("@RecRef", calldata.DateTimeSaved.HasValue ? (object)calldata.DateTimeSaved.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [RejectedDebiCheckTracking].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [RejectedDebiCheckTracking] ([FKImportID], [DateTimeSaved], [StampDate], [StampUserID]) VALUES(@FKImportID, @DateTimeSaved, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKImportID", calldata.FKImportID.HasValue ? (object)calldata.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@DateTimeSaved", calldata.DateTimeSaved.HasValue ? (object)calldata.DateTimeSaved.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for calldatas that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="number">The number search criteria.</param>
        /// <param name="extension">The extension search criteria.</param>
        /// <param name="recref">The recref search criteria.</param>
        /// <returns>A query that can be used to search for calldatas based on the search criteria.</returns>
        internal static string Search(long? fkimportid, DateTime? datetimesaved)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[RejectedDebiCheckTracking].[FKImportID] = " + fkimportid + "");
            }
            if (datetimesaved != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[RejectedDebiCheckTracking].[DateTimeSaved] = " + datetimesaved + "");
            }

            query.Append("SELECT [RejectedDebiCheckTracking].[ID], [RejectedDebiCheckTracking].[FKImportID], [RejectedDebiCheckTracking].[DateTimeSaved], [RejectedDebiCheckTracking].[StampDate], [RejectedDebiCheckTracking].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [RejectedDebiCheckTracking].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [RejectedDebiCheckTracking] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
