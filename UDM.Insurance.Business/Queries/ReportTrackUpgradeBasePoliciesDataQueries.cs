using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to reporttrackupgradebasepoliciesdata objects.
    /// </summary>
    internal abstract partial class ReportTrackUpgradeBasePoliciesDataQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) reporttrackupgradebasepoliciesdata from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata object to delete.</param>
        /// <returns>A query that can be used to delete the reporttrackupgradebasepoliciesdata from the database.</returns>
        internal static string Delete(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (reporttrackupgradebasepoliciesdata != null)
            {
                query = "INSERT INTO [zHstReportTrackUpgradeBasePoliciesData] ([ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID]) SELECT [ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID] FROM [ReportTrackUpgradeBasePoliciesData] WHERE [ReportTrackUpgradeBasePoliciesData].[ID] = @ID; ";
                query += "DELETE FROM [ReportTrackUpgradeBasePoliciesData] WHERE [ReportTrackUpgradeBasePoliciesData].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) reporttrackupgradebasepoliciesdata from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the reporttrackupgradebasepoliciesdata from the database.</returns>
        internal static string DeleteHistory(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (reporttrackupgradebasepoliciesdata != null)
            {
                query = "DELETE FROM [zHstReportTrackUpgradeBasePoliciesData] WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) reporttrackupgradebasepoliciesdata from the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata object to undelete.</param>
        /// <returns>A query that can be used to undelete the reporttrackupgradebasepoliciesdata from the database.</returns>
        internal static string UnDelete(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (reporttrackupgradebasepoliciesdata != null)
            {
                query = "INSERT INTO [ReportTrackUpgradeBasePoliciesData] ([ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID]) SELECT [ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID] FROM [zHstReportTrackUpgradeBasePoliciesData] WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] = @ID AND [zHstReportTrackUpgradeBasePoliciesData].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstReportTrackUpgradeBasePoliciesData] WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] = @ID) AND (SELECT COUNT(ID) FROM [ReportTrackUpgradeBasePoliciesData] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstReportTrackUpgradeBasePoliciesData] WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] = @ID AND [zHstReportTrackUpgradeBasePoliciesData].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstReportTrackUpgradeBasePoliciesData] WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [ReportTrackUpgradeBasePoliciesData] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an reporttrackupgradebasepoliciesdata object.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata object to fill.</param>
        /// <returns>A query that can be used to fill the reporttrackupgradebasepoliciesdata object.</returns>
        internal static string Fill(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (reporttrackupgradebasepoliciesdata != null)
            {
                query = "SELECT [ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID] FROM [ReportTrackUpgradeBasePoliciesData] WHERE [ReportTrackUpgradeBasePoliciesData].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  reporttrackupgradebasepoliciesdata data.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  reporttrackupgradebasepoliciesdata data.</returns>
        internal static string FillData(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (reporttrackupgradebasepoliciesdata != null)
            {
            query.Append("SELECT [ReportTrackUpgradeBasePoliciesData].[ID], [ReportTrackUpgradeBasePoliciesData].[Year], [ReportTrackUpgradeBasePoliciesData].[Month], [ReportTrackUpgradeBasePoliciesData].[LeadsReceived], [ReportTrackUpgradeBasePoliciesData].[SheMaccLeads], [ReportTrackUpgradeBasePoliciesData].[TargetPercentage], [ReportTrackUpgradeBasePoliciesData].[StampDate], [ReportTrackUpgradeBasePoliciesData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ReportTrackUpgradeBasePoliciesData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ReportTrackUpgradeBasePoliciesData] ");
                query.Append(" WHERE [ReportTrackUpgradeBasePoliciesData].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an reporttrackupgradebasepoliciesdata object from history.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the reporttrackupgradebasepoliciesdata object from history.</returns>
        internal static string FillHistory(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (reporttrackupgradebasepoliciesdata != null)
            {
                query = "SELECT [ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID] FROM [zHstReportTrackUpgradeBasePoliciesData] WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] = @ID AND [zHstReportTrackUpgradeBasePoliciesData].[StampUserID] = @StampUserID AND [zHstReportTrackUpgradeBasePoliciesData].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the reporttrackupgradebasepoliciesdatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the reporttrackupgradebasepoliciesdatas in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [ReportTrackUpgradeBasePoliciesData].[ID], [ReportTrackUpgradeBasePoliciesData].[Year], [ReportTrackUpgradeBasePoliciesData].[Month], [ReportTrackUpgradeBasePoliciesData].[LeadsReceived], [ReportTrackUpgradeBasePoliciesData].[SheMaccLeads], [ReportTrackUpgradeBasePoliciesData].[TargetPercentage], [ReportTrackUpgradeBasePoliciesData].[StampDate], [ReportTrackUpgradeBasePoliciesData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ReportTrackUpgradeBasePoliciesData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ReportTrackUpgradeBasePoliciesData] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted reporttrackupgradebasepoliciesdatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted reporttrackupgradebasepoliciesdatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstReportTrackUpgradeBasePoliciesData].[ID], [zHstReportTrackUpgradeBasePoliciesData].[Year], [zHstReportTrackUpgradeBasePoliciesData].[Month], [zHstReportTrackUpgradeBasePoliciesData].[LeadsReceived], [zHstReportTrackUpgradeBasePoliciesData].[SheMaccLeads], [zHstReportTrackUpgradeBasePoliciesData].[TargetPercentage], [zHstReportTrackUpgradeBasePoliciesData].[StampDate], [zHstReportTrackUpgradeBasePoliciesData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstReportTrackUpgradeBasePoliciesData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstReportTrackUpgradeBasePoliciesData] ");
            query.Append("INNER JOIN (SELECT [zHstReportTrackUpgradeBasePoliciesData].[ID], MAX([zHstReportTrackUpgradeBasePoliciesData].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstReportTrackUpgradeBasePoliciesData] ");
            query.Append("WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] NOT IN (SELECT [ReportTrackUpgradeBasePoliciesData].[ID] FROM [ReportTrackUpgradeBasePoliciesData]) ");
            query.Append("GROUP BY [zHstReportTrackUpgradeBasePoliciesData].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstReportTrackUpgradeBasePoliciesData].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstReportTrackUpgradeBasePoliciesData].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) reporttrackupgradebasepoliciesdata in the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) reporttrackupgradebasepoliciesdata in the database.</returns>
        public static string ListHistory(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (reporttrackupgradebasepoliciesdata != null)
            {
            query.Append("SELECT [zHstReportTrackUpgradeBasePoliciesData].[ID], [zHstReportTrackUpgradeBasePoliciesData].[Year], [zHstReportTrackUpgradeBasePoliciesData].[Month], [zHstReportTrackUpgradeBasePoliciesData].[LeadsReceived], [zHstReportTrackUpgradeBasePoliciesData].[SheMaccLeads], [zHstReportTrackUpgradeBasePoliciesData].[TargetPercentage], [zHstReportTrackUpgradeBasePoliciesData].[StampDate], [zHstReportTrackUpgradeBasePoliciesData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstReportTrackUpgradeBasePoliciesData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstReportTrackUpgradeBasePoliciesData] ");
                query.Append(" WHERE [zHstReportTrackUpgradeBasePoliciesData].[ID] = @ID");
                query.Append(" ORDER BY [zHstReportTrackUpgradeBasePoliciesData].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) reporttrackupgradebasepoliciesdata to the database.
        /// </summary>
        /// <param name="reporttrackupgradebasepoliciesdata">The reporttrackupgradebasepoliciesdata to save.</param>
        /// <returns>A query that can be used to save the reporttrackupgradebasepoliciesdata to the database.</returns>
        internal static string Save(ReportTrackUpgradeBasePoliciesData reporttrackupgradebasepoliciesdata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (reporttrackupgradebasepoliciesdata != null)
            {
                if (reporttrackupgradebasepoliciesdata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstReportTrackUpgradeBasePoliciesData] ([ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID]) SELECT [ID], [Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID] FROM [ReportTrackUpgradeBasePoliciesData] WHERE [ReportTrackUpgradeBasePoliciesData].[ID] = @ID; ");
                    query.Append("UPDATE [ReportTrackUpgradeBasePoliciesData]");
                    parameters = new object[6];
                    query.Append(" SET [Year] = @Year");
                    parameters[0] = Database.GetParameter("@Year", reporttrackupgradebasepoliciesdata.Year.HasValue ? (object)reporttrackupgradebasepoliciesdata.Year.Value : DBNull.Value);
                    query.Append(", [Month] = @Month");
                    parameters[1] = Database.GetParameter("@Month", string.IsNullOrEmpty(reporttrackupgradebasepoliciesdata.Month) ? DBNull.Value : (object)reporttrackupgradebasepoliciesdata.Month);
                    query.Append(", [LeadsReceived] = @LeadsReceived");
                    parameters[2] = Database.GetParameter("@LeadsReceived", reporttrackupgradebasepoliciesdata.LeadsReceived.HasValue ? (object)reporttrackupgradebasepoliciesdata.LeadsReceived.Value : DBNull.Value);
                    query.Append(", [SheMaccLeads] = @SheMaccLeads");
                    parameters[3] = Database.GetParameter("@SheMaccLeads", reporttrackupgradebasepoliciesdata.SheMaccLeads.HasValue ? (object)reporttrackupgradebasepoliciesdata.SheMaccLeads.Value : DBNull.Value);
                    query.Append(", [TargetPercentage] = @TargetPercentage");
                    parameters[4] = Database.GetParameter("@TargetPercentage", reporttrackupgradebasepoliciesdata.TargetPercentage.HasValue ? (object)reporttrackupgradebasepoliciesdata.TargetPercentage.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [ReportTrackUpgradeBasePoliciesData].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", reporttrackupgradebasepoliciesdata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [ReportTrackUpgradeBasePoliciesData] ([Year], [Month], [LeadsReceived], [SheMaccLeads], [TargetPercentage], [StampDate], [StampUserID]) VALUES(@Year, @Month, @LeadsReceived, @SheMaccLeads, @TargetPercentage, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@Year", reporttrackupgradebasepoliciesdata.Year.HasValue ? (object)reporttrackupgradebasepoliciesdata.Year.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@Month", string.IsNullOrEmpty(reporttrackupgradebasepoliciesdata.Month) ? DBNull.Value : (object)reporttrackupgradebasepoliciesdata.Month);
                    parameters[2] = Database.GetParameter("@LeadsReceived", reporttrackupgradebasepoliciesdata.LeadsReceived.HasValue ? (object)reporttrackupgradebasepoliciesdata.LeadsReceived.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@SheMaccLeads", reporttrackupgradebasepoliciesdata.SheMaccLeads.HasValue ? (object)reporttrackupgradebasepoliciesdata.SheMaccLeads.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@TargetPercentage", reporttrackupgradebasepoliciesdata.TargetPercentage.HasValue ? (object)reporttrackupgradebasepoliciesdata.TargetPercentage.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for reporttrackupgradebasepoliciesdatas that match the search criteria.
        /// </summary>
        /// <param name="year">The year search criteria.</param>
        /// <param name="month">The month search criteria.</param>
        /// <param name="leadsreceived">The leadsreceived search criteria.</param>
        /// <param name="shemaccleads">The shemaccleads search criteria.</param>
        /// <param name="targetpercentage">The targetpercentage search criteria.</param>
        /// <returns>A query that can be used to search for reporttrackupgradebasepoliciesdatas based on the search criteria.</returns>
        internal static string Search(long? year, string month, long? leadsreceived, long? shemaccleads, decimal? targetpercentage)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (year != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ReportTrackUpgradeBasePoliciesData].[Year] = " + year + "");
            }
            if (month != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ReportTrackUpgradeBasePoliciesData].[Month] LIKE '" + month.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (leadsreceived != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ReportTrackUpgradeBasePoliciesData].[LeadsReceived] = " + leadsreceived + "");
            }
            if (shemaccleads != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ReportTrackUpgradeBasePoliciesData].[SheMaccLeads] = " + shemaccleads + "");
            }
            if (targetpercentage != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[ReportTrackUpgradeBasePoliciesData].[TargetPercentage] = " + targetpercentage + "");
            }
            query.Append("SELECT [ReportTrackUpgradeBasePoliciesData].[ID], [ReportTrackUpgradeBasePoliciesData].[Year], [ReportTrackUpgradeBasePoliciesData].[Month], [ReportTrackUpgradeBasePoliciesData].[LeadsReceived], [ReportTrackUpgradeBasePoliciesData].[SheMaccLeads], [ReportTrackUpgradeBasePoliciesData].[TargetPercentage], [ReportTrackUpgradeBasePoliciesData].[StampDate], [ReportTrackUpgradeBasePoliciesData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [ReportTrackUpgradeBasePoliciesData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [ReportTrackUpgradeBasePoliciesData] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
