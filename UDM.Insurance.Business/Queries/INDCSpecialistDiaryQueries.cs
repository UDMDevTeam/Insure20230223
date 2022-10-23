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
    internal abstract partial class INDCSpecialistDiaryQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) calldata from the database.
        /// </summary>
        /// <param name="calldata">The calldata object to delete.</param>
        /// <returns>A query that can be used to delete the calldata from the database.</returns>
        internal static string Delete(INDCSpecialistDiary calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [zHstINDCSpecialistDiary] ([ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID] FROM [INDCSpecialistDiary] WHERE [INDCSpecialistDiary].[ID] = @ID; ";
                query += "DELETE FROM [INDCSpecialistDiary] WHERE [INDCSpecialistDiary].[ID] = @ID; ";
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
        internal static string DeleteHistory(INDCSpecialistDiary calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "DELETE FROM [zHstINDCSpecialistDiary] WHERE [zHstINDCSpecialistDiary].[ID] = @ID";
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
        internal static string UnDelete(INDCSpecialistDiary calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "INSERT INTO [INDCSpecialistDiary] ([ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID] FROM [zHstINDCSpecialistDiary] WHERE [zHstINDCSpecialistDiary].[ID] = @ID AND [zHstINDCSpecialistDiary].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDCSpecialistDiary] WHERE [zHstINDCSpecialistDiary].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INDCSpecialistDiary] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINDCSpecialistDiary] WHERE [zHstINDCSpecialistDiary].[ID] = @ID AND [zHstINDCSpecialistDiary].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDCSpecialistDiary] WHERE [zHstINDCSpecialistDiary].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INDCSpecialistDiary] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(INDCSpecialistDiary calldata, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID] FROM [INDCSpecialistDiary] WHERE [INDCSpecialistDiary].[ID] = @ID";
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
        internal static string FillData(INDCSpecialistDiary calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [INDCSpecialistDiary].[ID], [INDCSpecialistDiary].[FKINImportID], [INDCSpecialistDiary].[FKUserID], [INDCSpecialistDiary].[StartDate], [INDCSpecialistDiary].[EndDate], [INDCSpecialistDiary].[Time], [INDCSpecialistDiary].[StampDate], [INDCSpecialistDiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDCSpecialistDiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDCSpecialistDiary] ");
                query.Append(" WHERE [INDCSpecialistDiary].[ID] = @ID");
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
        internal static string FillHistory(INDCSpecialistDiary calldata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (calldata != null)
            {
                query = "SELECT [ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID] FROM [zHstINDCSpecialistDiary] WHERE [zHstINDCSpecialistDiary].[ID] = @ID AND [zHstINDCSpecialistDiary].[StampUserID] = @StampUserID AND [zHstINDCSpecialistDiary].[StampDate] = @StampDate";
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
            query.Append("SELECT [INDCSpecialistDiary].[ID], [INDCSpecialistDiary].[FKINImportID], [INDCSpecialistDiary].[FKUserID], [INDCSpecialistDiary].[StartDate], [INDCSpecialistDiary].[EndDate], [INDCSpecialistDiary].[Time], [INDCSpecialistDiary].[StampDate], [INDCSpecialistDiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDCSpecialistDiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDCSpecialistDiary] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted calldatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted calldatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINDCSpecialistDiary].[ID], [zHstINDCSpecialistDiary].[FKINImportID], [zHstINDCSpecialistDiary].[FKUserID], [zHstINDCSpecialistDiary].[StartDate], [zHstINDCSpecialistDiary].[EndDate], [zHstINDCSpecialistDiary].[Time], [zHstINDCSpecialistDiary].[StampDate], [zHstINDCSpecialistDiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDCSpecialistDiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDCSpecialistDiary] ");
            query.Append("INNER JOIN (SELECT [zHstINDCSpecialistDiary].[ID], MAX([zHstINDCSpecialistDiary].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINDCSpecialistDiary] ");
            query.Append("WHERE [zHstINDCSpecialistDiary].[ID] NOT IN (SELECT [INDCSpecialistDiary].[ID] FROM [INDCSpecialistDiary]) ");
            query.Append("GROUP BY [zHstINDCSpecialistDiary].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINDCSpecialistDiary].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINDCSpecialistDiary].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) calldata in the database.
        /// </summary>
        /// <param name="calldata">The calldata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) calldata in the database.</returns>
        public static string ListHistory(INDCSpecialistDiary calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
            query.Append("SELECT [zHstINDCSpecialistDiary].[ID], [zHstINDCSpecialistDiary].[FKINImportID], [zHstINDCSpecialistDiary].[FKUserID], [zHstINDCSpecialistDiary].[StartDate], [zHstINDCSpecialistDiary].[EndDate], [zHstINDCSpecialistDiary].[Time], [zHstINDCSpecialistDiary].[StampDate], [zHstINDCSpecialistDiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDCSpecialistDiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDCSpecialistDiary] ");
                query.Append(" WHERE [zHstINDCSpecialistDiary].[ID] = @ID");
                query.Append(" ORDER BY [zHstINDCSpecialistDiary].[StampDate] DESC");
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
        internal static string Save(INDCSpecialistDiary calldata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (calldata != null)
            {
                if (calldata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINDCSpecialistDiary] ([ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID]) SELECT [ID], [FKINImportID], [FKUserID], [StartDate], [EndDate], [Time], [StampDate], [StampUserID] FROM [INDCSpecialistDiary] WHERE [INDCSpecialistDiary].[ID] = @ID; ");
                    query.Append("UPDATE [INDCSpecialistDiary]");
                    parameters = new object[6];
                    query.Append(" SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", calldata.FKINImportID.HasValue ? (object)calldata.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKUserID.Value : DBNull.Value);
                    query.Append(", [StartDate] = @StartDate");
                    parameters[2] = Database.GetParameter("@StartDate", calldata.StartDate.HasValue ? (object)calldata.StartDate.Value : DBNull.Value);
                    query.Append(", [EndDate] = @EndDate");
                    parameters[3] = Database.GetParameter("@EndDate", calldata.EndDate.HasValue ? (object)calldata.EndDate.Value : DBNull.Value);
                    query.Append(", [Time] = @Time");
                    parameters[4] = Database.GetParameter("@Time", calldata.Time.HasValue ? (object)calldata.Time.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INDCSpecialistDiary].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", calldata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INDCSpecialistDiary] ([FKINImportID], [FKUserID], [StartDAte], [EndDate], [Time], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKUserID, @StartDate, @EndDate, @Time, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@FKINImportID", calldata.FKINImportID.HasValue ? (object)calldata.FKINImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", calldata.FKUserID.HasValue ? (object)calldata.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@StartDate", calldata.StartDate.HasValue ? (object)calldata.StartDate.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@EndDate", calldata.EndDate.HasValue ? (object)calldata.EndDate.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@Time", calldata.Time.HasValue ? (object)calldata.Time.Value : DBNull.Value);

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
        internal static string Search(long? fkimportid, long? fkuserid, DateTime? startdate, DateTime? enddate, TimeSpan? time)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDCSpecialistDiary].[FKINImportID] = " + fkimportid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDCSpecialistDiary].[FKUserID] LIKE '" + fkuserid + "'");
            }
            if (startdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDCSpecialistDiary].[StartDate] LIKE '" + startdate + "'");
            }
            if (enddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDCSpecialistDiary].[EndDate] LIKE '" + enddate + "'");

            }
            if (enddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDCSpecialistDiary].[Time] LIKE '" + time + "'");
            }
            query.Append("SELECT [INDCSpecialistDiary].[ID], [INDCSpecialistDiary].[FKINImportID], [INDCSpecialistDiary].[FKUserID], [INDCSpecialistDiary].[StartDate], [INDCSpecialistDiary].[EndDate], [INDCSpecialistDiary].[StampDate], [INDCSpecialistDiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDCSpecialistDiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDCSpecialistDiary] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
