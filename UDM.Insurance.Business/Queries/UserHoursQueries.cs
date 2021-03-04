using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to userhours objects.
    /// </summary>
    internal abstract partial class UserHoursQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) userhours from the database.
        /// </summary>
        /// <param name="userhours">The userhours object to delete.</param>
        /// <returns>A query that can be used to delete the userhours from the database.</returns>
        internal static string Delete(UserHours userhours, ref object[] parameters)
        {
            string query = string.Empty;
            if (userhours != null)
            {
                query = "INSERT INTO [zHstUserHours] ([ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID] FROM [UserHours] WHERE [UserHours].[ID] = @ID; ";
                query += "DELETE FROM [UserHours] WHERE [UserHours].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", userhours.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) userhours from the database.
        /// </summary>
        /// <param name="userhours">The userhours object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the userhours from the database.</returns>
        internal static string DeleteHistory(UserHours userhours, ref object[] parameters)
        {
            string query = string.Empty;
            if (userhours != null)
            {
                query = "DELETE FROM [zHstUserHours] WHERE [zHstUserHours].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", userhours.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) userhours from the database.
        /// </summary>
        /// <param name="userhours">The userhours object to undelete.</param>
        /// <returns>A query that can be used to undelete the userhours from the database.</returns>
        internal static string UnDelete(UserHours userhours, ref object[] parameters)
        {
            string query = string.Empty;
            if (userhours != null)
            {
                query = "INSERT INTO [UserHours] ([ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID] FROM [zHstUserHours] WHERE [zHstUserHours].[ID] = @ID AND [zHstUserHours].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstUserHours] WHERE [zHstUserHours].[ID] = @ID) AND (SELECT COUNT(ID) FROM [UserHours] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstUserHours] WHERE [zHstUserHours].[ID] = @ID AND [zHstUserHours].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstUserHours] WHERE [zHstUserHours].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [UserHours] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", userhours.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an userhours object.
        /// </summary>
        /// <param name="userhours">The userhours object to fill.</param>
        /// <returns>A query that can be used to fill the userhours object.</returns>
        internal static string Fill(UserHours userhours, ref object[] parameters)
        {
            string query = string.Empty;
            if (userhours != null)
            {
                query = "SELECT [ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID] FROM [UserHours] WHERE [UserHours].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", userhours.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  userhours data.
        /// </summary>
        /// <param name="userhours">The userhours to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  userhours data.</returns>
        internal static string FillData(UserHours userhours, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (userhours != null)
            {
            query.Append("SELECT [UserHours].[ID], [UserHours].[FKUserID], [UserHours].[FKINCampaignID], [UserHours].[WorkingDate], [UserHours].[MorningShiftStartTime], [UserHours].[MorningShiftEndTime], [UserHours].[NormalShiftStartTime], [UserHours].[NormalShiftEndTime], [UserHours].[EveningShiftStartTime], [UserHours].[EveningShiftEndTime], [UserHours].[PublicHolidayWeekendShiftStartTime], [UserHours].[PublicHolidayWeekendShiftEndTime], [UserHours].[FKShiftTypeID], [UserHours].[IsRedeemedHours], [UserHours].[StampDate], [UserHours].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [UserHours].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [UserHours] ");
                query.Append(" WHERE [UserHours].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", userhours.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an userhours object from history.
        /// </summary>
        /// <param name="userhours">The userhours object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the userhours object from history.</returns>
        internal static string FillHistory(UserHours userhours, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (userhours != null)
            {
                query = "SELECT [ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID] FROM [zHstUserHours] WHERE [zHstUserHours].[ID] = @ID AND [zHstUserHours].[StampUserID] = @StampUserID AND [zHstUserHours].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", userhours.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the userhourss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the userhourss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [UserHours].[ID], [UserHours].[FKUserID], [UserHours].[FKINCampaignID], [UserHours].[WorkingDate], [UserHours].[MorningShiftStartTime], [UserHours].[MorningShiftEndTime], [UserHours].[NormalShiftStartTime], [UserHours].[NormalShiftEndTime], [UserHours].[EveningShiftStartTime], [UserHours].[EveningShiftEndTime], [UserHours].[PublicHolidayWeekendShiftStartTime], [UserHours].[PublicHolidayWeekendShiftEndTime], [UserHours].[FKShiftTypeID], [UserHours].[IsRedeemedHours], [UserHours].[StampDate], [UserHours].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [UserHours].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [UserHours] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted userhourss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted userhourss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstUserHours].[ID], [zHstUserHours].[FKUserID], [zHstUserHours].[FKINCampaignID], [zHstUserHours].[WorkingDate], [zHstUserHours].[MorningShiftStartTime], [zHstUserHours].[MorningShiftEndTime], [zHstUserHours].[NormalShiftStartTime], [zHstUserHours].[NormalShiftEndTime], [zHstUserHours].[EveningShiftStartTime], [zHstUserHours].[EveningShiftEndTime], [zHstUserHours].[PublicHolidayWeekendShiftStartTime], [zHstUserHours].[PublicHolidayWeekendShiftEndTime], [zHstUserHours].[FKShiftTypeID], [zHstUserHours].[IsRedeemedHours], [zHstUserHours].[StampDate], [zHstUserHours].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstUserHours].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstUserHours] ");
            query.Append("INNER JOIN (SELECT [zHstUserHours].[ID], MAX([zHstUserHours].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstUserHours] ");
            query.Append("WHERE [zHstUserHours].[ID] NOT IN (SELECT [UserHours].[ID] FROM [UserHours]) ");
            query.Append("GROUP BY [zHstUserHours].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstUserHours].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstUserHours].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) userhours in the database.
        /// </summary>
        /// <param name="userhours">The userhours object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) userhours in the database.</returns>
        public static string ListHistory(UserHours userhours, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (userhours != null)
            {
            query.Append("SELECT [zHstUserHours].[ID], [zHstUserHours].[FKUserID], [zHstUserHours].[FKINCampaignID], [zHstUserHours].[WorkingDate], [zHstUserHours].[MorningShiftStartTime], [zHstUserHours].[MorningShiftEndTime], [zHstUserHours].[NormalShiftStartTime], [zHstUserHours].[NormalShiftEndTime], [zHstUserHours].[EveningShiftStartTime], [zHstUserHours].[EveningShiftEndTime], [zHstUserHours].[PublicHolidayWeekendShiftStartTime], [zHstUserHours].[PublicHolidayWeekendShiftEndTime], [zHstUserHours].[FKShiftTypeID], [zHstUserHours].[IsRedeemedHours], [zHstUserHours].[StampDate], [zHstUserHours].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstUserHours].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstUserHours] ");
                query.Append(" WHERE [zHstUserHours].[ID] = @ID");
                query.Append(" ORDER BY [zHstUserHours].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", userhours.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) userhours to the database.
        /// </summary>
        /// <param name="userhours">The userhours to save.</param>
        /// <returns>A query that can be used to save the userhours to the database.</returns>
        internal static string Save(UserHours userhours, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (userhours != null)
            {
                if (userhours.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstUserHours] ([ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID]) SELECT [ID], [FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID] FROM [UserHours] WHERE [UserHours].[ID] = @ID; ");
                    query.Append("UPDATE [UserHours]");
                    parameters = new object[14];
                    query.Append(" SET [FKUserID] = @FKUserID");
                    parameters[0] = Database.GetParameter("@FKUserID", userhours.FKUserID.HasValue ? (object)userhours.FKUserID.Value : DBNull.Value);
                    query.Append(", [FKINCampaignID] = @FKINCampaignID");
                    parameters[1] = Database.GetParameter("@FKINCampaignID", userhours.FKINCampaignID.HasValue ? (object)userhours.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [WorkingDate] = @WorkingDate");
                    parameters[2] = Database.GetParameter("@WorkingDate", userhours.WorkingDate.HasValue ? (object)userhours.WorkingDate.Value : DBNull.Value);
                    query.Append(", [MorningShiftStartTime] = @MorningShiftStartTime");
                    parameters[3] = Database.GetParameter("@MorningShiftStartTime", userhours.MorningShiftStartTime.HasValue ? (object)userhours.MorningShiftStartTime.Value : DBNull.Value);
                    query.Append(", [MorningShiftEndTime] = @MorningShiftEndTime");
                    parameters[4] = Database.GetParameter("@MorningShiftEndTime", userhours.MorningShiftEndTime.HasValue ? (object)userhours.MorningShiftEndTime.Value : DBNull.Value);
                    query.Append(", [NormalShiftStartTime] = @NormalShiftStartTime");
                    parameters[5] = Database.GetParameter("@NormalShiftStartTime", userhours.NormalShiftStartTime.HasValue ? (object)userhours.NormalShiftStartTime.Value : DBNull.Value);
                    query.Append(", [NormalShiftEndTime] = @NormalShiftEndTime");
                    parameters[6] = Database.GetParameter("@NormalShiftEndTime", userhours.NormalShiftEndTime.HasValue ? (object)userhours.NormalShiftEndTime.Value : DBNull.Value);
                    query.Append(", [EveningShiftStartTime] = @EveningShiftStartTime");
                    parameters[7] = Database.GetParameter("@EveningShiftStartTime", userhours.EveningShiftStartTime.HasValue ? (object)userhours.EveningShiftStartTime.Value : DBNull.Value);
                    query.Append(", [EveningShiftEndTime] = @EveningShiftEndTime");
                    parameters[8] = Database.GetParameter("@EveningShiftEndTime", userhours.EveningShiftEndTime.HasValue ? (object)userhours.EveningShiftEndTime.Value : DBNull.Value);
                    query.Append(", [PublicHolidayWeekendShiftStartTime] = @PublicHolidayWeekendShiftStartTime");
                    parameters[9] = Database.GetParameter("@PublicHolidayWeekendShiftStartTime", userhours.PublicHolidayWeekendShiftStartTime.HasValue ? (object)userhours.PublicHolidayWeekendShiftStartTime.Value : DBNull.Value);
                    query.Append(", [PublicHolidayWeekendShiftEndTime] = @PublicHolidayWeekendShiftEndTime");
                    parameters[10] = Database.GetParameter("@PublicHolidayWeekendShiftEndTime", userhours.PublicHolidayWeekendShiftEndTime.HasValue ? (object)userhours.PublicHolidayWeekendShiftEndTime.Value : DBNull.Value);
                    query.Append(", [FKShiftTypeID] = @FKShiftTypeID");
                    parameters[11] = Database.GetParameter("@FKShiftTypeID", userhours.FKShiftTypeID.HasValue ? (object)userhours.FKShiftTypeID.Value : DBNull.Value);
                    query.Append(", [IsRedeemedHours] = @IsRedeemedHours");
                    parameters[12] = Database.GetParameter("@IsRedeemedHours", userhours.IsRedeemedHours.HasValue ? (object)userhours.IsRedeemedHours.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [UserHours].[ID] = @ID"); 
                    parameters[13] = Database.GetParameter("@ID", userhours.ID);
                }
                else
                {
                    query.Append("INSERT INTO [UserHours] ([FKUserID], [FKINCampaignID], [WorkingDate], [MorningShiftStartTime], [MorningShiftEndTime], [NormalShiftStartTime], [NormalShiftEndTime], [EveningShiftStartTime], [EveningShiftEndTime], [PublicHolidayWeekendShiftStartTime], [PublicHolidayWeekendShiftEndTime], [FKShiftTypeID], [IsRedeemedHours], [StampDate], [StampUserID]) VALUES(@FKUserID, @FKINCampaignID, @WorkingDate, @MorningShiftStartTime, @MorningShiftEndTime, @NormalShiftStartTime, @NormalShiftEndTime, @EveningShiftStartTime, @EveningShiftEndTime, @PublicHolidayWeekendShiftStartTime, @PublicHolidayWeekendShiftEndTime, @FKShiftTypeID, @IsRedeemedHours, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[13];
                    parameters[0] = Database.GetParameter("@FKUserID", userhours.FKUserID.HasValue ? (object)userhours.FKUserID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINCampaignID", userhours.FKINCampaignID.HasValue ? (object)userhours.FKINCampaignID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@WorkingDate", userhours.WorkingDate.HasValue ? (object)userhours.WorkingDate.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@MorningShiftStartTime", userhours.MorningShiftStartTime.HasValue ? (object)userhours.MorningShiftStartTime.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@MorningShiftEndTime", userhours.MorningShiftEndTime.HasValue ? (object)userhours.MorningShiftEndTime.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@NormalShiftStartTime", userhours.NormalShiftStartTime.HasValue ? (object)userhours.NormalShiftStartTime.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@NormalShiftEndTime", userhours.NormalShiftEndTime.HasValue ? (object)userhours.NormalShiftEndTime.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@EveningShiftStartTime", userhours.EveningShiftStartTime.HasValue ? (object)userhours.EveningShiftStartTime.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@EveningShiftEndTime", userhours.EveningShiftEndTime.HasValue ? (object)userhours.EveningShiftEndTime.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@PublicHolidayWeekendShiftStartTime", userhours.PublicHolidayWeekendShiftStartTime.HasValue ? (object)userhours.PublicHolidayWeekendShiftStartTime.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@PublicHolidayWeekendShiftEndTime", userhours.PublicHolidayWeekendShiftEndTime.HasValue ? (object)userhours.PublicHolidayWeekendShiftEndTime.Value : DBNull.Value);
                    parameters[11] = Database.GetParameter("@FKShiftTypeID", userhours.FKShiftTypeID.HasValue ? (object)userhours.FKShiftTypeID.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@IsRedeemedHours", userhours.IsRedeemedHours.HasValue ? (object)userhours.IsRedeemedHours.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for userhourss that match the search criteria.
        /// </summary>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="workingdate">The workingdate search criteria.</param>
        /// <param name="morningshiftstarttime">The morningshiftstarttime search criteria.</param>
        /// <param name="morningshiftendtime">The morningshiftendtime search criteria.</param>
        /// <param name="normalshiftstarttime">The normalshiftstarttime search criteria.</param>
        /// <param name="normalshiftendtime">The normalshiftendtime search criteria.</param>
        /// <param name="eveningshiftstarttime">The eveningshiftstarttime search criteria.</param>
        /// <param name="eveningshiftendtime">The eveningshiftendtime search criteria.</param>
        /// <param name="publicholidayweekendshiftstarttime">The publicholidayweekendshiftstarttime search criteria.</param>
        /// <param name="publicholidayweekendshiftendtime">The publicholidayweekendshiftendtime search criteria.</param>
        /// <param name="fkshifttypeid">The fkshifttypeid search criteria.</param>
        /// <param name="isredeemedhours">The isredeemedhours search criteria.</param>
        /// <returns>A query that can be used to search for userhourss based on the search criteria.</returns>
        internal static string Search(long? fkuserid, long? fkincampaignid, DateTime? workingdate, TimeSpan? morningshiftstarttime, TimeSpan? morningshiftendtime, TimeSpan? normalshiftstarttime, TimeSpan? normalshiftendtime, TimeSpan? eveningshiftstarttime, TimeSpan? eveningshiftendtime, TimeSpan? publicholidayweekendshiftstarttime, TimeSpan? publicholidayweekendshiftendtime, long? fkshifttypeid, bool? isredeemedhours)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[FKUserID] = " + fkuserid + "");
            }
            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[FKINCampaignID] = " + fkincampaignid + "");
            }
            if (workingdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[WorkingDate] = '" + workingdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (morningshiftstarttime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[MorningShiftStartTime] = " + morningshiftstarttime + "");
            }
            if (morningshiftendtime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[MorningShiftEndTime] = " + morningshiftendtime + "");
            }
            if (normalshiftstarttime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[NormalShiftStartTime] = " + normalshiftstarttime + "");
            }
            if (normalshiftendtime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[NormalShiftEndTime] = " + normalshiftendtime + "");
            }
            if (eveningshiftstarttime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[EveningShiftStartTime] = " + eveningshiftstarttime + "");
            }
            if (eveningshiftendtime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[EveningShiftEndTime] = " + eveningshiftendtime + "");
            }
            if (publicholidayweekendshiftstarttime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[PublicHolidayWeekendShiftStartTime] = " + publicholidayweekendshiftstarttime + "");
            }
            if (publicholidayweekendshiftendtime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[PublicHolidayWeekendShiftEndTime] = " + publicholidayweekendshiftendtime + "");
            }
            if (fkshifttypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[FKShiftTypeID] = " + fkshifttypeid + "");
            }
            if (isredeemedhours != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[UserHours].[IsRedeemedHours] = " + ((bool)isredeemedhours ? "1" : "0"));
            }
            query.Append("SELECT [UserHours].[ID], [UserHours].[FKUserID], [UserHours].[FKINCampaignID], [UserHours].[WorkingDate], [UserHours].[MorningShiftStartTime], [UserHours].[MorningShiftEndTime], [UserHours].[NormalShiftStartTime], [UserHours].[NormalShiftEndTime], [UserHours].[EveningShiftStartTime], [UserHours].[EveningShiftEndTime], [UserHours].[PublicHolidayWeekendShiftStartTime], [UserHours].[PublicHolidayWeekendShiftEndTime], [UserHours].[FKShiftTypeID], [UserHours].[IsRedeemedHours], [UserHours].[StampDate], [UserHours].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [UserHours].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [UserHours] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
