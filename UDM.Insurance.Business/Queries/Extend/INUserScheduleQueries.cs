using System;
using System.Text;

namespace UDM.Insurance.Business.Queries
{
    internal abstract partial class INUserScheduleQueries
    {
        #region Search

        internal static string Search(long? fksystemid, long? fkuserid, long? fkinimportid)
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

            query.Append("SELECT [INUserSchedule].[ID], [INUserSchedule].[FKSystemID], [INUserSchedule].[FKUserID], [INUserSchedule].[FKINImportID], [INUserSchedule].[ScheduleID], [INUserSchedule].[Duration], [INUserSchedule].[Start], [INUserSchedule].[End], [INUserSchedule].[Subject], [INUserSchedule].[Description], [INUserSchedule].[Location], [INUserSchedule].[Categories], [INUserSchedule].[ReminderEnabled], [INUserSchedule].[ReminderInterval], [INUserSchedule].[StampDate], [INUserSchedule].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUserSchedule].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUserSchedule] ");
            return query + whereQuery.ToString();
        }

        #endregion
    }
}
