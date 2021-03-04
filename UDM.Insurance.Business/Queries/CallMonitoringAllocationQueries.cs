using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to callmonitoringallocation objects.
    /// </summary>
    internal abstract partial class CallMonitoringAllocationQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) callmonitoringallocation from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation object to delete.</param>
        /// <returns>A query that can be used to delete the callmonitoringallocation from the database.</returns>
        internal static string Delete(CallMonitoringAllocation callmonitoringallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringallocation != null)
            {
                query = "INSERT INTO [zHstCallMonitoringAllocation] ([ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID] FROM [CallMonitoringAllocation] WHERE [CallMonitoringAllocation].[ID] = @ID; ";
                query += "DELETE FROM [CallMonitoringAllocation] WHERE [CallMonitoringAllocation].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) callmonitoringallocation from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the callmonitoringallocation from the database.</returns>
        internal static string DeleteHistory(CallMonitoringAllocation callmonitoringallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringallocation != null)
            {
                query = "DELETE FROM [zHstCallMonitoringAllocation] WHERE [zHstCallMonitoringAllocation].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) callmonitoringallocation from the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation object to undelete.</param>
        /// <returns>A query that can be used to undelete the callmonitoringallocation from the database.</returns>
        internal static string UnDelete(CallMonitoringAllocation callmonitoringallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringallocation != null)
            {
                query = "INSERT INTO [CallMonitoringAllocation] ([ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID] FROM [zHstCallMonitoringAllocation] WHERE [zHstCallMonitoringAllocation].[ID] = @ID AND [zHstCallMonitoringAllocation].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCallMonitoringAllocation] WHERE [zHstCallMonitoringAllocation].[ID] = @ID) AND (SELECT COUNT(ID) FROM [CallMonitoringAllocation] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstCallMonitoringAllocation] WHERE [zHstCallMonitoringAllocation].[ID] = @ID AND [zHstCallMonitoringAllocation].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCallMonitoringAllocation] WHERE [zHstCallMonitoringAllocation].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [CallMonitoringAllocation] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringallocation.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an callmonitoringallocation object.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation object to fill.</param>
        /// <returns>A query that can be used to fill the callmonitoringallocation object.</returns>
        internal static string Fill(CallMonitoringAllocation callmonitoringallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringallocation != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID] FROM [CallMonitoringAllocation] WHERE [CallMonitoringAllocation].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  callmonitoringallocation data.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  callmonitoringallocation data.</returns>
        internal static string FillData(CallMonitoringAllocation callmonitoringallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (callmonitoringallocation != null)
            {
            query.Append("SELECT [CallMonitoringAllocation].[ID], [CallMonitoringAllocation].[FKINImportID], [CallMonitoringAllocation].[FKUserID], [CallMonitoringAllocation].[IsSavedCarriedForward], [CallMonitoringAllocation].[ExpiryDate], [CallMonitoringAllocation].[StampDate], [CallMonitoringAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallMonitoringAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallMonitoringAllocation] ");
                query.Append(" WHERE [CallMonitoringAllocation].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringallocation.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an callmonitoringallocation object from history.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the callmonitoringallocation object from history.</returns>
        internal static string FillHistory(CallMonitoringAllocation callmonitoringallocation, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringallocation != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID] FROM [zHstCallMonitoringAllocation] WHERE [zHstCallMonitoringAllocation].[ID] = @ID AND [zHstCallMonitoringAllocation].[StampUserID] = @StampUserID AND [zHstCallMonitoringAllocation].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", callmonitoringallocation.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the callmonitoringallocations in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the callmonitoringallocations in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [CallMonitoringAllocation].[ID], [CallMonitoringAllocation].[FKINImportID], [CallMonitoringAllocation].[FKUserID], [CallMonitoringAllocation].[IsSavedCarriedForward], [CallMonitoringAllocation].[ExpiryDate], [CallMonitoringAllocation].[StampDate], [CallMonitoringAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallMonitoringAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallMonitoringAllocation] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted callmonitoringallocations in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted callmonitoringallocations in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstCallMonitoringAllocation].[ID], [zHstCallMonitoringAllocation].[FKINImportID], [zHstCallMonitoringAllocation].[FKUserID], [zHstCallMonitoringAllocation].[IsSavedCarriedForward], [zHstCallMonitoringAllocation].[ExpiryDate], [zHstCallMonitoringAllocation].[StampDate], [zHstCallMonitoringAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCallMonitoringAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstCallMonitoringAllocation] ");
            query.Append("INNER JOIN (SELECT [zHstCallMonitoringAllocation].[ID], MAX([zHstCallMonitoringAllocation].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstCallMonitoringAllocation] ");
            query.Append("WHERE [zHstCallMonitoringAllocation].[ID] NOT IN (SELECT [CallMonitoringAllocation].[ID] FROM [CallMonitoringAllocation]) ");
            query.Append("GROUP BY [zHstCallMonitoringAllocation].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstCallMonitoringAllocation].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstCallMonitoringAllocation].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) callmonitoringallocation in the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) callmonitoringallocation in the database.</returns>
        public static string ListHistory(CallMonitoringAllocation callmonitoringallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (callmonitoringallocation != null)
            {
            query.Append("SELECT [zHstCallMonitoringAllocation].[ID], [zHstCallMonitoringAllocation].[FKINImportID], [zHstCallMonitoringAllocation].[FKUserID], [zHstCallMonitoringAllocation].[IsSavedCarriedForward], [zHstCallMonitoringAllocation].[ExpiryDate], [zHstCallMonitoringAllocation].[StampDate], [zHstCallMonitoringAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCallMonitoringAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstCallMonitoringAllocation] ");
                query.Append(" WHERE [zHstCallMonitoringAllocation].[ID] = @ID");
                query.Append(" ORDER BY [zHstCallMonitoringAllocation].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringallocation.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) callmonitoringallocation to the database.
        /// </summary>
        /// <param name="callmonitoringallocation">The callmonitoringallocation to save.</param>
        /// <returns>A query that can be used to save the callmonitoringallocation to the database.</returns>
        internal static string Save(CallMonitoringAllocation callmonitoringallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (callmonitoringallocation != null)
            {
                if (callmonitoringallocation.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstCallMonitoringAllocation] ([ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID] FROM [CallMonitoringAllocation] WHERE [CallMonitoringAllocation].[ID] = @ID; ");
                    query.Append("UPDATE [CallMonitoringAllocation]");
                    parameters = new object[5];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", callmonitoringallocation.FKINImportID.HasValue ? (object)callmonitoringallocation.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", callmonitoringallocation.FKUserID.HasValue ? (object)callmonitoringallocation.FKUserID.Value : DBNull.Value);
                    query.Append(", [IsSavedCarriedForward] = @IsSavedCarriedForward");
                    parameters[2] = Database.GetParameter("@IsSavedCarriedForward", callmonitoringallocation.IsSavedCarriedForward.HasValue ? (object)callmonitoringallocation.IsSavedCarriedForward.Value : DBNull.Value);
                    query.Append(", [ExpiryDate] = @ExpiryDate");
                    parameters[3] = Database.GetParameter("@ExpiryDate", callmonitoringallocation.ExpiryDate.HasValue ? (object)callmonitoringallocation.ExpiryDate.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [CallMonitoringAllocation].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", callmonitoringallocation.ID);
                }
                else
                {
                    query.Append("INSERT INTO [CallMonitoringAllocation] ([FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKUserID, @IsSavedCarriedForward, @ExpiryDate, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKINImportID", callmonitoringallocation.FKINImportID.HasValue ? (object)callmonitoringallocation.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", callmonitoringallocation.FKUserID.HasValue ? (object)callmonitoringallocation.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@IsSavedCarriedForward", callmonitoringallocation.IsSavedCarriedForward.HasValue ? (object)callmonitoringallocation.IsSavedCarriedForward.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@ExpiryDate", callmonitoringallocation.ExpiryDate.HasValue ? (object)callmonitoringallocation.ExpiryDate.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for callmonitoringallocations that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <returns>A query that can be used to search for callmonitoringallocations based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringAllocation].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringAllocation].[FKUserID] = " + fkuserid + "");
            }
            if (issavedcarriedforward != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringAllocation].[IsSavedCarriedForward] = " + ((bool)issavedcarriedforward ? "1" : "0"));
            }
            if (expirydate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringAllocation].[ExpiryDate] = '" + expirydate.Value.ToString(Database.DateFormat) + "'");
            }
            query.Append("SELECT [CallMonitoringAllocation].[ID], [CallMonitoringAllocation].[FKINImportID], [CallMonitoringAllocation].[FKUserID], [CallMonitoringAllocation].[IsSavedCarriedForward], [CallMonitoringAllocation].[ExpiryDate], [CallMonitoringAllocation].[StampDate], [CallMonitoringAllocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallMonitoringAllocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallMonitoringAllocation] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
