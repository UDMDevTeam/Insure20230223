using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to callmonitoringunallocation objects.
    /// </summary>
    internal abstract partial class CallMonitoringUnallocationQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) callmonitoringunallocation from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation object to delete.</param>
        /// <returns>A query that can be used to delete the callmonitoringunallocation from the database.</returns>
        internal static string Delete(CallMonitoringUnallocation callmonitoringunallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringunallocation != null)
            {
                query = "INSERT INTO [zHstCallMonitoringUnallocation] ([ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID] FROM [CallMonitoringUnallocation] WHERE [CallMonitoringUnallocation].[ID] = @ID; ";
                query += "DELETE FROM [CallMonitoringUnallocation] WHERE [CallMonitoringUnallocation].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) callmonitoringunallocation from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the callmonitoringunallocation from the database.</returns>
        internal static string DeleteHistory(CallMonitoringUnallocation callmonitoringunallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringunallocation != null)
            {
                query = "DELETE FROM [zHstCallMonitoringUnallocation] WHERE [zHstCallMonitoringUnallocation].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) callmonitoringunallocation from the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation object to undelete.</param>
        /// <returns>A query that can be used to undelete the callmonitoringunallocation from the database.</returns>
        internal static string UnDelete(CallMonitoringUnallocation callmonitoringunallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringunallocation != null)
            {
                query = "INSERT INTO [CallMonitoringUnallocation] ([ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID] FROM [zHstCallMonitoringUnallocation] WHERE [zHstCallMonitoringUnallocation].[ID] = @ID AND [zHstCallMonitoringUnallocation].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCallMonitoringUnallocation] WHERE [zHstCallMonitoringUnallocation].[ID] = @ID) AND (SELECT COUNT(ID) FROM [CallMonitoringUnallocation] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstCallMonitoringUnallocation] WHERE [zHstCallMonitoringUnallocation].[ID] = @ID AND [zHstCallMonitoringUnallocation].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCallMonitoringUnallocation] WHERE [zHstCallMonitoringUnallocation].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [CallMonitoringUnallocation] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an callmonitoringunallocation object.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation object to fill.</param>
        /// <returns>A query that can be used to fill the callmonitoringunallocation object.</returns>
        internal static string Fill(CallMonitoringUnallocation callmonitoringunallocation, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringunallocation != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID] FROM [CallMonitoringUnallocation] WHERE [CallMonitoringUnallocation].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  callmonitoringunallocation data.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  callmonitoringunallocation data.</returns>
        internal static string FillData(CallMonitoringUnallocation callmonitoringunallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (callmonitoringunallocation != null)
            {
            query.Append("SELECT [CallMonitoringUnallocation].[ID], [CallMonitoringUnallocation].[FKINImportID], [CallMonitoringUnallocation].[FKUserID], [CallMonitoringUnallocation].[IsSavedCarriedForward], [CallMonitoringUnallocation].[ExpiryDate], [CallMonitoringUnallocation].[AllocatedByUserID], [CallMonitoringUnallocation].[CallMonitoringAllocationDate], [CallMonitoringUnallocation].[StampDate], [CallMonitoringUnallocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallMonitoringUnallocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallMonitoringUnallocation] ");
                query.Append(" WHERE [CallMonitoringUnallocation].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an callmonitoringunallocation object from history.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the callmonitoringunallocation object from history.</returns>
        internal static string FillHistory(CallMonitoringUnallocation callmonitoringunallocation, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (callmonitoringunallocation != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID] FROM [zHstCallMonitoringUnallocation] WHERE [zHstCallMonitoringUnallocation].[ID] = @ID AND [zHstCallMonitoringUnallocation].[StampUserID] = @StampUserID AND [zHstCallMonitoringUnallocation].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the callmonitoringunallocations in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the callmonitoringunallocations in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [CallMonitoringUnallocation].[ID], [CallMonitoringUnallocation].[FKINImportID], [CallMonitoringUnallocation].[FKUserID], [CallMonitoringUnallocation].[IsSavedCarriedForward], [CallMonitoringUnallocation].[ExpiryDate], [CallMonitoringUnallocation].[AllocatedByUserID], [CallMonitoringUnallocation].[CallMonitoringAllocationDate], [CallMonitoringUnallocation].[StampDate], [CallMonitoringUnallocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallMonitoringUnallocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallMonitoringUnallocation] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted callmonitoringunallocations in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted callmonitoringunallocations in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstCallMonitoringUnallocation].[ID], [zHstCallMonitoringUnallocation].[FKINImportID], [zHstCallMonitoringUnallocation].[FKUserID], [zHstCallMonitoringUnallocation].[IsSavedCarriedForward], [zHstCallMonitoringUnallocation].[ExpiryDate], [zHstCallMonitoringUnallocation].[AllocatedByUserID], [zHstCallMonitoringUnallocation].[CallMonitoringAllocationDate], [zHstCallMonitoringUnallocation].[StampDate], [zHstCallMonitoringUnallocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCallMonitoringUnallocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstCallMonitoringUnallocation] ");
            query.Append("INNER JOIN (SELECT [zHstCallMonitoringUnallocation].[ID], MAX([zHstCallMonitoringUnallocation].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstCallMonitoringUnallocation] ");
            query.Append("WHERE [zHstCallMonitoringUnallocation].[ID] NOT IN (SELECT [CallMonitoringUnallocation].[ID] FROM [CallMonitoringUnallocation]) ");
            query.Append("GROUP BY [zHstCallMonitoringUnallocation].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstCallMonitoringUnallocation].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstCallMonitoringUnallocation].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) callmonitoringunallocation in the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) callmonitoringunallocation in the database.</returns>
        public static string ListHistory(CallMonitoringUnallocation callmonitoringunallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (callmonitoringunallocation != null)
            {
            query.Append("SELECT [zHstCallMonitoringUnallocation].[ID], [zHstCallMonitoringUnallocation].[FKINImportID], [zHstCallMonitoringUnallocation].[FKUserID], [zHstCallMonitoringUnallocation].[IsSavedCarriedForward], [zHstCallMonitoringUnallocation].[ExpiryDate], [zHstCallMonitoringUnallocation].[AllocatedByUserID], [zHstCallMonitoringUnallocation].[CallMonitoringAllocationDate], [zHstCallMonitoringUnallocation].[StampDate], [zHstCallMonitoringUnallocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCallMonitoringUnallocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstCallMonitoringUnallocation] ");
                query.Append(" WHERE [zHstCallMonitoringUnallocation].[ID] = @ID");
                query.Append(" ORDER BY [zHstCallMonitoringUnallocation].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) callmonitoringunallocation to the database.
        /// </summary>
        /// <param name="callmonitoringunallocation">The callmonitoringunallocation to save.</param>
        /// <returns>A query that can be used to save the callmonitoringunallocation to the database.</returns>
        internal static string Save(CallMonitoringUnallocation callmonitoringunallocation, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (callmonitoringunallocation != null)
            {
                if (callmonitoringunallocation.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstCallMonitoringUnallocation] ([ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID] FROM [CallMonitoringUnallocation] WHERE [CallMonitoringUnallocation].[ID] = @ID; ");
                    query.Append("UPDATE [CallMonitoringUnallocation]");
                    parameters = new object[7];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", callmonitoringunallocation.FKINImportID.HasValue ? (object)callmonitoringunallocation.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", callmonitoringunallocation.FKUserID.HasValue ? (object)callmonitoringunallocation.FKUserID.Value : DBNull.Value);
                    query.Append(", [IsSavedCarriedForward] = @IsSavedCarriedForward");
                    parameters[2] = Database.GetParameter("@IsSavedCarriedForward", callmonitoringunallocation.IsSavedCarriedForward.HasValue ? (object)callmonitoringunallocation.IsSavedCarriedForward.Value : DBNull.Value);
                    query.Append(", [ExpiryDate] = @ExpiryDate");
                    parameters[3] = Database.GetParameter("@ExpiryDate", callmonitoringunallocation.ExpiryDate.HasValue ? (object)callmonitoringunallocation.ExpiryDate.Value : DBNull.Value);
                    query.Append(", [AllocatedByUserID] = @AllocatedByUserID");
                    parameters[4] = Database.GetParameter("@AllocatedByUserID", callmonitoringunallocation.AllocatedByUserID.HasValue ? (object)callmonitoringunallocation.AllocatedByUserID.Value : DBNull.Value);
                    query.Append(", [CallMonitoringAllocationDate] = @CallMonitoringAllocationDate");
                    parameters[5] = Database.GetParameter("@CallMonitoringAllocationDate", callmonitoringunallocation.CallMonitoringAllocationDate.HasValue ? (object)callmonitoringunallocation.CallMonitoringAllocationDate.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [CallMonitoringUnallocation].[ID] = @ID"); 
                    parameters[6] = Database.GetParameter("@ID", callmonitoringunallocation.ID);
                }
                else
                {
                    query.Append("INSERT INTO [CallMonitoringUnallocation] ([FKINImportID], [FKUserID], [IsSavedCarriedForward], [ExpiryDate], [AllocatedByUserID], [CallMonitoringAllocationDate], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKUserID, @IsSavedCarriedForward, @ExpiryDate, @AllocatedByUserID, @CallMonitoringAllocationDate, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[6];
                    parameters[0] = Database.GetParameter("@FKINImportID", callmonitoringunallocation.FKINImportID.HasValue ? (object)callmonitoringunallocation.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", callmonitoringunallocation.FKUserID.HasValue ? (object)callmonitoringunallocation.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@IsSavedCarriedForward", callmonitoringunallocation.IsSavedCarriedForward.HasValue ? (object)callmonitoringunallocation.IsSavedCarriedForward.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@ExpiryDate", callmonitoringunallocation.ExpiryDate.HasValue ? (object)callmonitoringunallocation.ExpiryDate.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@AllocatedByUserID", callmonitoringunallocation.AllocatedByUserID.HasValue ? (object)callmonitoringunallocation.AllocatedByUserID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@CallMonitoringAllocationDate", callmonitoringunallocation.CallMonitoringAllocationDate.HasValue ? (object)callmonitoringunallocation.CallMonitoringAllocationDate.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for callmonitoringunallocations that match the search criteria.
        /// </summary>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="issavedcarriedforward">The issavedcarriedforward search criteria.</param>
        /// <param name="expirydate">The expirydate search criteria.</param>
        /// <param name="allocatedbyuserid">The allocatedbyuserid search criteria.</param>
        /// <param name="callmonitoringallocationdate">The callmonitoringallocationdate search criteria.</param>
        /// <returns>A query that can be used to search for callmonitoringunallocations based on the search criteria.</returns>
        internal static string Search(long? fkinimportid, long? fkuserid, bool? issavedcarriedforward, DateTime? expirydate, long? allocatedbyuserid, DateTime? callmonitoringallocationdate)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringUnallocation].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringUnallocation].[FKUserID] = " + fkuserid + "");
            }
            if (issavedcarriedforward != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringUnallocation].[IsSavedCarriedForward] = " + ((bool)issavedcarriedforward ? "1" : "0"));
            }
            if (expirydate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringUnallocation].[ExpiryDate] = '" + expirydate.Value.ToString(Database.DateFormat) + "'");
            }
            if (allocatedbyuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringUnallocation].[AllocatedByUserID] = " + allocatedbyuserid + "");
            }
            if (callmonitoringallocationdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CallMonitoringUnallocation].[CallMonitoringAllocationDate] = '" + callmonitoringallocationdate.Value.ToString(Database.DateFormat) + "'");
            }
            query.Append("SELECT [CallMonitoringUnallocation].[ID], [CallMonitoringUnallocation].[FKINImportID], [CallMonitoringUnallocation].[FKUserID], [CallMonitoringUnallocation].[IsSavedCarriedForward], [CallMonitoringUnallocation].[ExpiryDate], [CallMonitoringUnallocation].[AllocatedByUserID], [CallMonitoringUnallocation].[CallMonitoringAllocationDate], [CallMonitoringUnallocation].[StampDate], [CallMonitoringUnallocation].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CallMonitoringUnallocation].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CallMonitoringUnallocation] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
