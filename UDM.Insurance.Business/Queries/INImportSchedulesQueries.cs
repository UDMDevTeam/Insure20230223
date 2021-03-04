using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportschedules objects.
    /// </summary>
    internal abstract partial class INImportSchedulesQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportschedules from the database.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules object to delete.</param>
        /// <returns>A query that can be used to delete the inimportschedules from the database.</returns>
        internal static string Delete(INImportSchedules inimportschedules, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportschedules != null)
            {
                query = "INSERT INTO [zHstINImportSchedules] ([ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID] FROM [INImportSchedules] WHERE [INImportSchedules].[ID] = @ID; ";
                query += "DELETE FROM [INImportSchedules] WHERE [INImportSchedules].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportschedules.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportschedules from the database.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportschedules from the database.</returns>
        internal static string DeleteHistory(INImportSchedules inimportschedules, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportschedules != null)
            {
                query = "DELETE FROM [zHstINImportSchedules] WHERE [zHstINImportSchedules].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportschedules.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportschedules from the database.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportschedules from the database.</returns>
        internal static string UnDelete(INImportSchedules inimportschedules, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportschedules != null)
            {
                query = "INSERT INTO [INImportSchedules] ([ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID] FROM [zHstINImportSchedules] WHERE [zHstINImportSchedules].[ID] = @ID AND [zHstINImportSchedules].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportSchedules] WHERE [zHstINImportSchedules].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportSchedules] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportSchedules] WHERE [zHstINImportSchedules].[ID] = @ID AND [zHstINImportSchedules].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportSchedules] WHERE [zHstINImportSchedules].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportSchedules] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportschedules.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportschedules object.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules object to fill.</param>
        /// <returns>A query that can be used to fill the inimportschedules object.</returns>
        internal static string Fill(INImportSchedules inimportschedules, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportschedules != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID] FROM [INImportSchedules] WHERE [INImportSchedules].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportschedules.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportschedules data.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportschedules data.</returns>
        internal static string FillData(INImportSchedules inimportschedules, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportschedules != null)
            {
            query.Append("SELECT [INImportSchedules].[ID], [INImportSchedules].[FKINCampaignID], [INImportSchedules].[BatchName], [INImportSchedules].[UDMCode], [INImportSchedules].[ImportFile], [INImportSchedules].[ScheduleDate], [INImportSchedules].[ScheduleTime], [INImportSchedules].[HasRun], [INImportSchedules].[NumberOfLeads], [INImportSchedules].[ImportAtempts], [INImportSchedules].[DateReceived], [INImportSchedules].[StampDate], [INImportSchedules].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportSchedules].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportSchedules] ");
                query.Append(" WHERE [INImportSchedules].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportschedules.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportschedules object from history.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportschedules object from history.</returns>
        internal static string FillHistory(INImportSchedules inimportschedules, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportschedules != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID] FROM [zHstINImportSchedules] WHERE [zHstINImportSchedules].[ID] = @ID AND [zHstINImportSchedules].[StampUserID] = @StampUserID AND [zHstINImportSchedules].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportschedules.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportscheduless in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportscheduless in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportSchedules].[ID], [INImportSchedules].[FKINCampaignID], [INImportSchedules].[BatchName], [INImportSchedules].[UDMCode], [INImportSchedules].[ImportFile], [INImportSchedules].[ScheduleDate], [INImportSchedules].[ScheduleTime], [INImportSchedules].[HasRun], [INImportSchedules].[NumberOfLeads], [INImportSchedules].[ImportAtempts], [INImportSchedules].[DateReceived], [INImportSchedules].[StampDate], [INImportSchedules].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportSchedules].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportSchedules] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportscheduless in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportscheduless in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportSchedules].[ID], [zHstINImportSchedules].[FKINCampaignID], [zHstINImportSchedules].[BatchName], [zHstINImportSchedules].[UDMCode], [zHstINImportSchedules].[ImportFile], [zHstINImportSchedules].[ScheduleDate], [zHstINImportSchedules].[ScheduleTime], [zHstINImportSchedules].[HasRun], [zHstINImportSchedules].[NumberOfLeads], [zHstINImportSchedules].[ImportAtempts], [zHstINImportSchedules].[DateReceived], [zHstINImportSchedules].[StampDate], [zHstINImportSchedules].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportSchedules].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportSchedules] ");
            query.Append("INNER JOIN (SELECT [zHstINImportSchedules].[ID], MAX([zHstINImportSchedules].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportSchedules] ");
            query.Append("WHERE [zHstINImportSchedules].[ID] NOT IN (SELECT [INImportSchedules].[ID] FROM [INImportSchedules]) ");
            query.Append("GROUP BY [zHstINImportSchedules].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportSchedules].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportSchedules].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportschedules in the database.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportschedules in the database.</returns>
        public static string ListHistory(INImportSchedules inimportschedules, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportschedules != null)
            {
            query.Append("SELECT [zHstINImportSchedules].[ID], [zHstINImportSchedules].[FKINCampaignID], [zHstINImportSchedules].[BatchName], [zHstINImportSchedules].[UDMCode], [zHstINImportSchedules].[ImportFile], [zHstINImportSchedules].[ScheduleDate], [zHstINImportSchedules].[ScheduleTime], [zHstINImportSchedules].[HasRun], [zHstINImportSchedules].[NumberOfLeads], [zHstINImportSchedules].[ImportAtempts], [zHstINImportSchedules].[DateReceived], [zHstINImportSchedules].[StampDate], [zHstINImportSchedules].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportSchedules].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportSchedules] ");
                query.Append(" WHERE [zHstINImportSchedules].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportSchedules].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportschedules.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportschedules to the database.
        /// </summary>
        /// <param name="inimportschedules">The inimportschedules to save.</param>
        /// <returns>A query that can be used to save the inimportschedules to the database.</returns>
        internal static string Save(INImportSchedules inimportschedules, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportschedules != null)
            {
                if (inimportschedules.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportSchedules] ([ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID] FROM [INImportSchedules] WHERE [INImportSchedules].[ID] = @ID; ");
                    query.Append("UPDATE [INImportSchedules]");
                    parameters = new object[11];
                    query.Append(" SET [FKINCampaignID] = @FKINCampaignID");
                    parameters[0] = Database.GetParameter("@FKINCampaignID", inimportschedules.FKINCampaignID.HasValue ? (object)inimportschedules.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [BatchName] = @BatchName");
                    parameters[1] = Database.GetParameter("@BatchName", string.IsNullOrEmpty(inimportschedules.BatchName) ? DBNull.Value : (object)inimportschedules.BatchName);
                    query.Append(", [UDMCode] = @UDMCode");
                    parameters[2] = Database.GetParameter("@UDMCode", string.IsNullOrEmpty(inimportschedules.UDMCode) ? DBNull.Value : (object)inimportschedules.UDMCode);
                    query.Append(", [ImportFile] = @ImportFile");
                    parameters[3] = Database.GetParameter("@ImportFile", inimportschedules.ImportFile);
                    query.Append(", [ScheduleDate] = @ScheduleDate");
                    parameters[4] = Database.GetParameter("@ScheduleDate", inimportschedules.ScheduleDate.HasValue ? (object)inimportschedules.ScheduleDate.Value : DBNull.Value);
                    query.Append(", [ScheduleTime] = @ScheduleTime");
                    parameters[5] = Database.GetParameter("@ScheduleTime", inimportschedules.ScheduleTime.HasValue ? (object)inimportschedules.ScheduleTime.Value : DBNull.Value);
                    query.Append(", [HasRun] = @HasRun");
                    parameters[6] = Database.GetParameter("@HasRun", inimportschedules.HasRun.HasValue ? (object)inimportschedules.HasRun.Value : DBNull.Value);
                    query.Append(", [NumberOfLeads] = @NumberOfLeads");
                    parameters[7] = Database.GetParameter("@NumberOfLeads", inimportschedules.NumberOfLeads.HasValue ? (object)inimportschedules.NumberOfLeads.Value : DBNull.Value);
                    query.Append(", [ImportAtempts] = @ImportAtempts");
                    parameters[8] = Database.GetParameter("@ImportAtempts", inimportschedules.ImportAtempts.HasValue ? (object)inimportschedules.ImportAtempts.Value : DBNull.Value);
                    query.Append(", [DateReceived] = @DateReceived");
                    parameters[9] = Database.GetParameter("@DateReceived", inimportschedules.DateReceived.HasValue ? (object)inimportschedules.DateReceived.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportSchedules].[ID] = @ID"); 
                    parameters[10] = Database.GetParameter("@ID", inimportschedules.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportSchedules] ([FKINCampaignID], [BatchName], [UDMCode], [ImportFile], [ScheduleDate], [ScheduleTime], [HasRun], [NumberOfLeads], [ImportAtempts], [DateReceived], [StampDate], [StampUserID]) VALUES(@FKINCampaignID, @BatchName, @UDMCode, @ImportFile, @ScheduleDate, @ScheduleTime, @HasRun, @NumberOfLeads, @ImportAtempts, @DateReceived, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[10];
                    parameters[0] = Database.GetParameter("@FKINCampaignID", inimportschedules.FKINCampaignID.HasValue ? (object)inimportschedules.FKINCampaignID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@BatchName", string.IsNullOrEmpty(inimportschedules.BatchName) ? DBNull.Value : (object)inimportschedules.BatchName);
                    parameters[2] = Database.GetParameter("@UDMCode", string.IsNullOrEmpty(inimportschedules.UDMCode) ? DBNull.Value : (object)inimportschedules.UDMCode);
                    parameters[3] = Database.GetParameter("@ImportFile", inimportschedules.ImportFile);
                    parameters[4] = Database.GetParameter("@ScheduleDate", inimportschedules.ScheduleDate.HasValue ? (object)inimportschedules.ScheduleDate.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@ScheduleTime", inimportschedules.ScheduleTime.HasValue ? (object)inimportschedules.ScheduleTime.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@HasRun", inimportschedules.HasRun.HasValue ? (object)inimportschedules.HasRun.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@NumberOfLeads", inimportschedules.NumberOfLeads.HasValue ? (object)inimportschedules.NumberOfLeads.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@ImportAtempts", inimportschedules.ImportAtempts.HasValue ? (object)inimportschedules.ImportAtempts.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@DateReceived", inimportschedules.DateReceived.HasValue ? (object)inimportschedules.DateReceived.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportscheduless that match the search criteria.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="batchname">The batchname search criteria.</param>
        /// <param name="udmcode">The udmcode search criteria.</param>
        /// <param name="importfile">The importfile search criteria.</param>
        /// <param name="scheduledate">The scheduledate search criteria.</param>
        /// <param name="scheduletime">The scheduletime search criteria.</param>
        /// <param name="hasrun">The hasrun search criteria.</param>
        /// <param name="numberofleads">The numberofleads search criteria.</param>
        /// <param name="importatempts">The importatempts search criteria.</param>
        /// <param name="datereceived">The datereceived search criteria.</param>
        /// <returns>A query that can be used to search for inimportscheduless based on the search criteria.</returns>
        internal static string Search(long? fkincampaignid, string batchname, string udmcode, byte[] importfile, DateTime? scheduledate, TimeSpan? scheduletime, bool? hasrun, int? numberofleads, int? importatempts, DateTime? datereceived)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[FKINCampaignID] = " + fkincampaignid + "");
            }
            if (batchname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[BatchName] LIKE '" + batchname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (udmcode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[UDMCode] LIKE '" + udmcode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (importfile != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[ImportFile] = " + importfile + "");
            }
            if (scheduledate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[ScheduleDate] = '" + scheduledate.Value.ToString(Database.DateFormat) + "'");
            }
            if (scheduletime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[ScheduleTime] = " + scheduletime + "");
            }
            if (hasrun != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[HasRun] = " + ((bool)hasrun ? "1" : "0"));
            }
            if (numberofleads != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[NumberOfLeads] = " + numberofleads + "");
            }
            if (importatempts != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[ImportAtempts] = " + importatempts + "");
            }
            if (datereceived != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportSchedules].[DateReceived] = '" + datereceived.Value.ToString(Database.DateFormat) + "'");
            }
            query.Append("SELECT [INImportSchedules].[ID], [INImportSchedules].[FKINCampaignID], [INImportSchedules].[BatchName], [INImportSchedules].[UDMCode], [INImportSchedules].[ImportFile], [INImportSchedules].[ScheduleDate], [INImportSchedules].[ScheduleTime], [INImportSchedules].[HasRun], [INImportSchedules].[NumberOfLeads], [INImportSchedules].[ImportAtempts], [INImportSchedules].[DateReceived], [INImportSchedules].[StampDate], [INImportSchedules].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportSchedules].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportSchedules] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
