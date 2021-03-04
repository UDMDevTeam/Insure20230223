using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inuserschedule objects.
    /// </summary>
    internal abstract partial class INUserScheduleQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inuserschedule from the database.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule object to delete.</param>
        /// <returns>A query that can be used to delete the inuserschedule from the database.</returns>
        internal static string Delete(INUserSchedule inuserschedule, ref object[] parameters)
        {
            string query = string.Empty;
            if (inuserschedule != null)
            {
                query = "INSERT INTO [zHstINUserSchedule] ([ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID] FROM [INUserSchedule] WHERE [INUserSchedule].[ID] = @ID; ";
                query += "DELETE FROM [INUserSchedule] WHERE [INUserSchedule].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inuserschedule.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inuserschedule from the database.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inuserschedule from the database.</returns>
        internal static string DeleteHistory(INUserSchedule inuserschedule, ref object[] parameters)
        {
            string query = string.Empty;
            if (inuserschedule != null)
            {
                query = "DELETE FROM [zHstINUserSchedule] WHERE [zHstINUserSchedule].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inuserschedule.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inuserschedule from the database.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule object to undelete.</param>
        /// <returns>A query that can be used to undelete the inuserschedule from the database.</returns>
        internal static string UnDelete(INUserSchedule inuserschedule, ref object[] parameters)
        {
            string query = string.Empty;
            if (inuserschedule != null)
            {
                query = "INSERT INTO [INUserSchedule] ([ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID] FROM [zHstINUserSchedule] WHERE [zHstINUserSchedule].[ID] = @ID AND [zHstINUserSchedule].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINUserSchedule] WHERE [zHstINUserSchedule].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INUserSchedule] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINUserSchedule] WHERE [zHstINUserSchedule].[ID] = @ID AND [zHstINUserSchedule].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINUserSchedule] WHERE [zHstINUserSchedule].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INUserSchedule] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inuserschedule.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inuserschedule object.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule object to fill.</param>
        /// <returns>A query that can be used to fill the inuserschedule object.</returns>
        internal static string Fill(INUserSchedule inuserschedule, ref object[] parameters)
        {
            string query = string.Empty;
            if (inuserschedule != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID] FROM [INUserSchedule] WHERE [INUserSchedule].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inuserschedule.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inuserschedule data.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inuserschedule data.</returns>
        internal static string FillData(INUserSchedule inuserschedule, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inuserschedule != null)
            {
            query.Append("SELECT [INUserSchedule].[ID], [INUserSchedule].[FKSystemID], [INUserSchedule].[FKUserID], [INUserSchedule].[FKINImportID], [INUserSchedule].[ScheduleID], [INUserSchedule].[Duration], [INUserSchedule].[Start], [INUserSchedule].[End], [INUserSchedule].[Subject], [INUserSchedule].[Description], [INUserSchedule].[Location], [INUserSchedule].[Categories], [INUserSchedule].[ReminderEnabled], [INUserSchedule].[ReminderInterval], [INUserSchedule].[StampDate], [INUserSchedule].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUserSchedule].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUserSchedule] ");
                query.Append(" WHERE [INUserSchedule].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inuserschedule.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inuserschedule object from history.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inuserschedule object from history.</returns>
        internal static string FillHistory(INUserSchedule inuserschedule, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inuserschedule != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID] FROM [zHstINUserSchedule] WHERE [zHstINUserSchedule].[ID] = @ID AND [zHstINUserSchedule].[StampUserID] = @StampUserID AND [zHstINUserSchedule].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inuserschedule.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inuserschedules in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inuserschedules in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INUserSchedule].[ID], [INUserSchedule].[FKSystemID], [INUserSchedule].[FKUserID], [INUserSchedule].[FKINImportID], [INUserSchedule].[ScheduleID], [INUserSchedule].[Duration], [INUserSchedule].[Start], [INUserSchedule].[End], [INUserSchedule].[Subject], [INUserSchedule].[Description], [INUserSchedule].[Location], [INUserSchedule].[Categories], [INUserSchedule].[ReminderEnabled], [INUserSchedule].[ReminderInterval], [INUserSchedule].[StampDate], [INUserSchedule].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUserSchedule].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUserSchedule] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inuserschedules in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inuserschedules in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINUserSchedule].[ID], [zHstINUserSchedule].[FKSystemID], [zHstINUserSchedule].[FKUserID], [zHstINUserSchedule].[FKINImportID], [zHstINUserSchedule].[ScheduleID], [zHstINUserSchedule].[Duration], [zHstINUserSchedule].[Start], [zHstINUserSchedule].[End], [zHstINUserSchedule].[Subject], [zHstINUserSchedule].[Description], [zHstINUserSchedule].[Location], [zHstINUserSchedule].[Categories], [zHstINUserSchedule].[ReminderEnabled], [zHstINUserSchedule].[ReminderInterval], [zHstINUserSchedule].[StampDate], [zHstINUserSchedule].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINUserSchedule].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINUserSchedule] ");
            query.Append("INNER JOIN (SELECT [zHstINUserSchedule].[ID], MAX([zHstINUserSchedule].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINUserSchedule] ");
            query.Append("WHERE [zHstINUserSchedule].[ID] NOT IN (SELECT [INUserSchedule].[ID] FROM [INUserSchedule]) ");
            query.Append("GROUP BY [zHstINUserSchedule].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINUserSchedule].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINUserSchedule].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inuserschedule in the database.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inuserschedule in the database.</returns>
        public static string ListHistory(INUserSchedule inuserschedule, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inuserschedule != null)
            {
            query.Append("SELECT [zHstINUserSchedule].[ID], [zHstINUserSchedule].[FKSystemID], [zHstINUserSchedule].[FKUserID], [zHstINUserSchedule].[FKINImportID], [zHstINUserSchedule].[ScheduleID], [zHstINUserSchedule].[Duration], [zHstINUserSchedule].[Start], [zHstINUserSchedule].[End], [zHstINUserSchedule].[Subject], [zHstINUserSchedule].[Description], [zHstINUserSchedule].[Location], [zHstINUserSchedule].[Categories], [zHstINUserSchedule].[ReminderEnabled], [zHstINUserSchedule].[ReminderInterval], [zHstINUserSchedule].[StampDate], [zHstINUserSchedule].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINUserSchedule].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINUserSchedule] ");
                query.Append(" WHERE [zHstINUserSchedule].[ID] = @ID");
                query.Append(" ORDER BY [zHstINUserSchedule].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inuserschedule.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inuserschedule to the database.
        /// </summary>
        /// <param name="inuserschedule">The inuserschedule to save.</param>
        /// <returns>A query that can be used to save the inuserschedule to the database.</returns>
        internal static string Save(INUserSchedule inuserschedule, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inuserschedule != null)
            {
                if (inuserschedule.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINUserSchedule] ([ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID] FROM [INUserSchedule] WHERE [INUserSchedule].[ID] = @ID; ");
                    query.Append("UPDATE [INUserSchedule]");
                    parameters = new object[14];
                    query.Append(" SET [FKSystemID] = @FKSystemID");
                    parameters[0] = Database.GetParameter("@FKSystemID", inuserschedule.FKSystemID.HasValue ? (object)inuserschedule.FKSystemID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", inuserschedule.FKUserID.HasValue ? (object)inuserschedule.FKUserID.Value : DBNull.Value);
                    query.Append(", [FKINImportID] = @FKINImportID");
                    parameters[2] = Database.GetParameter("@FKINImportID", inuserschedule.FKINImportID.HasValue ? (object)inuserschedule.FKINImportID.Value : DBNull.Value);
                    query.Append(", [ScheduleID] = @ScheduleID");
                    parameters[3] = Database.GetParameter("@ScheduleID", inuserschedule.ScheduleID.HasValue ? (object)inuserschedule.ScheduleID.Value : DBNull.Value);
                    query.Append(", [Duration] = @Duration");
                    parameters[4] = Database.GetParameter("@Duration", inuserschedule.Duration.HasValue ? (object)inuserschedule.Duration.Value : DBNull.Value);
                    query.Append(", [Start] = @Start");
                    parameters[5] = Database.GetParameter("@Start", inuserschedule.Start.HasValue ? (object)inuserschedule.Start.Value : DBNull.Value);
                    query.Append(", [End] = @End");
                    parameters[6] = Database.GetParameter("@End", inuserschedule.End.HasValue ? (object)inuserschedule.End.Value : DBNull.Value);
                    query.Append(", [Subject] = @Subject");
                    parameters[7] = Database.GetParameter("@Subject", string.IsNullOrEmpty(inuserschedule.Subject) ? DBNull.Value : (object)inuserschedule.Subject);
                    query.Append(", [Description] = @Description");
                    parameters[8] = Database.GetParameter("@Description", string.IsNullOrEmpty(inuserschedule.Description) ? DBNull.Value : (object)inuserschedule.Description);
                    query.Append(", [Location] = @Location");
                    parameters[9] = Database.GetParameter("@Location", string.IsNullOrEmpty(inuserschedule.Location) ? DBNull.Value : (object)inuserschedule.Location);
                    query.Append(", [Categories] = @Categories");
                    parameters[10] = Database.GetParameter("@Categories", string.IsNullOrEmpty(inuserschedule.Categories) ? DBNull.Value : (object)inuserschedule.Categories);
                    query.Append(", [ReminderEnabled] = @ReminderEnabled");
                    parameters[11] = Database.GetParameter("@ReminderEnabled", inuserschedule.ReminderEnabled.HasValue ? (object)inuserschedule.ReminderEnabled.Value : DBNull.Value);
                    query.Append(", [ReminderInterval] = @ReminderInterval");
                    parameters[12] = Database.GetParameter("@ReminderInterval", inuserschedule.ReminderInterval.HasValue ? (object)inuserschedule.ReminderInterval.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INUserSchedule].[ID] = @ID"); 
                    parameters[13] = Database.GetParameter("@ID", inuserschedule.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INUserSchedule] ([FKSystemID], [FKUserID], [FKINImportID], [ScheduleID], [Duration], [Start], [End], [Subject], [Description], [Location], [Categories], [ReminderEnabled], [ReminderInterval], [StampDate], [StampUserID]) VALUES(@FKSystemID, @FKUserID, @FKINImportID, @ScheduleID, @Duration, @Start, @End, @Subject, @Description, @Location, @Categories, @ReminderEnabled, @ReminderInterval, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[13];
                    parameters[0] = Database.GetParameter("@FKSystemID", inuserschedule.FKSystemID.HasValue ? (object)inuserschedule.FKSystemID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", inuserschedule.FKUserID.HasValue ? (object)inuserschedule.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKINImportID", inuserschedule.FKINImportID.HasValue ? (object)inuserschedule.FKINImportID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@ScheduleID", inuserschedule.ScheduleID.HasValue ? (object)inuserschedule.ScheduleID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@Duration", inuserschedule.Duration.HasValue ? (object)inuserschedule.Duration.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@Start", inuserschedule.Start.HasValue ? (object)inuserschedule.Start.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@End", inuserschedule.End.HasValue ? (object)inuserschedule.End.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@Subject", string.IsNullOrEmpty(inuserschedule.Subject) ? DBNull.Value : (object)inuserschedule.Subject);
                    parameters[8] = Database.GetParameter("@Description", string.IsNullOrEmpty(inuserschedule.Description) ? DBNull.Value : (object)inuserschedule.Description);
                    parameters[9] = Database.GetParameter("@Location", string.IsNullOrEmpty(inuserschedule.Location) ? DBNull.Value : (object)inuserschedule.Location);
                    parameters[10] = Database.GetParameter("@Categories", string.IsNullOrEmpty(inuserschedule.Categories) ? DBNull.Value : (object)inuserschedule.Categories);
                    parameters[11] = Database.GetParameter("@ReminderEnabled", inuserschedule.ReminderEnabled.HasValue ? (object)inuserschedule.ReminderEnabled.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@ReminderInterval", inuserschedule.ReminderInterval.HasValue ? (object)inuserschedule.ReminderInterval.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inuserschedules that match the search criteria.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="scheduleid">The scheduleid search criteria.</param>
        /// <param name="duration">The duration search criteria.</param>
        /// <param name="start">The start search criteria.</param>
        /// <param name="end">The end search criteria.</param>
        /// <param name="subject">The subject search criteria.</param>
        /// <param name="description">The description search criteria.</param>
        /// <param name="location">The location search criteria.</param>
        /// <param name="categories">The categories search criteria.</param>
        /// <param name="reminderenabled">The reminderenabled search criteria.</param>
        /// <param name="reminderinterval">The reminderinterval search criteria.</param>
        /// <returns>A query that can be used to search for inuserschedules based on the search criteria.</returns>
        internal static string Search(long? fksystemid, long? fkuserid, long? fkinimportid, Guid? scheduleid, TimeSpan? duration, DateTime? start, DateTime? end, string subject, string description, string location, string categories, bool? reminderenabled, long? reminderinterval)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fksystemid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[FKSystemID] = " + fksystemid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[FKUserID] = " + fkuserid + "");
            }
            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[FKINImportID] = " + fkinimportid + "");
            }
            if (scheduleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[ScheduleID] = " + scheduleid + "");
            }
            if (duration != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[Duration] = " + duration + "");
            }
            if (start != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[Start] = '" + start.Value.ToString(Database.DateFormat) + "'");
            }
            if (end != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[End] = '" + end.Value.ToString(Database.DateFormat) + "'");
            }
            if (subject != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[Subject] LIKE '" + subject.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (location != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[Location] LIKE '" + location.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (categories != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[Categories] LIKE '" + categories.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (reminderenabled != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[ReminderEnabled] = " + ((bool)reminderenabled ? "1" : "0"));
            }
            if (reminderinterval != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserSchedule].[ReminderInterval] = " + reminderinterval + "");
            }
            query.Append("SELECT [INUserSchedule].[ID], [INUserSchedule].[FKSystemID], [INUserSchedule].[FKUserID], [INUserSchedule].[FKINImportID], [INUserSchedule].[ScheduleID], [INUserSchedule].[Duration], [INUserSchedule].[Start], [INUserSchedule].[End], [INUserSchedule].[Subject], [INUserSchedule].[Description], [INUserSchedule].[Location], [INUserSchedule].[Categories], [INUserSchedule].[ReminderEnabled], [INUserSchedule].[ReminderInterval], [INUserSchedule].[StampDate], [INUserSchedule].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUserSchedule].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUserSchedule] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
