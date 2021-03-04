using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to tsrtargets objects.
    /// </summary>
    internal abstract partial class TSRTargetsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) tsrtargets from the database.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets object to delete.</param>
        /// <returns>A query that can be used to delete the tsrtargets from the database.</returns>
        internal static string Delete(TSRTargets tsrtargets, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrtargets != null)
            {
                query = "INSERT INTO [zHstTSRTargets] ([ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget]) SELECT [ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget] FROM [TSRTargets] WHERE [TSRTargets].[ID] = @ID; ";
                query += "DELETE FROM [TSRTargets] WHERE [TSRTargets].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrtargets.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) tsrtargets from the database.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the tsrtargets from the database.</returns>
        internal static string DeleteHistory(TSRTargets tsrtargets, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrtargets != null)
            {
                query = "DELETE FROM [zHstTSRTargets] WHERE [zHstTSRTargets].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrtargets.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) tsrtargets from the database.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets object to undelete.</param>
        /// <returns>A query that can be used to undelete the tsrtargets from the database.</returns>
        internal static string UnDelete(TSRTargets tsrtargets, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrtargets != null)
            {
                query = "INSERT INTO [TSRTargets] ([ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget]) SELECT [ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget] FROM [zHstTSRTargets] WHERE [zHstTSRTargets].[ID] = @ID AND [zHstTSRTargets].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstTSRTargets] WHERE [zHstTSRTargets].[ID] = @ID) AND (SELECT COUNT(ID) FROM [TSRTargets] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstTSRTargets] WHERE [zHstTSRTargets].[ID] = @ID AND [zHstTSRTargets].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstTSRTargets] WHERE [zHstTSRTargets].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [TSRTargets] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrtargets.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an tsrtargets object.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets object to fill.</param>
        /// <returns>A query that can be used to fill the tsrtargets object.</returns>
        internal static string Fill(TSRTargets tsrtargets, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrtargets != null)
            {
                query = "SELECT [ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget] FROM [TSRTargets] WHERE [TSRTargets].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrtargets.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  tsrtargets data.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  tsrtargets data.</returns>
        internal static string FillData(TSRTargets tsrtargets, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (tsrtargets != null)
            {
            query.Append("SELECT [TSRTargets].[ID], [TSRTargets].[FKINWeekID], [TSRTargets].[FKINCampaignID], [TSRTargets].[FKAgentID], [TSRTargets].[DateFrom], [TSRTargets].[DateTo], [TSRTargets].[Hours], [TSRTargets].[BaseTarget], [TSRTargets].[PremiumTarget], [TSRTargets].[AccDisSelectedItem], [TSRTargets].[UnitTarget]");
            query.Append(" FROM [TSRTargets] ");
                query.Append(" WHERE [TSRTargets].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrtargets.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an tsrtargets object from history.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the tsrtargets object from history.</returns>
        internal static string FillHistory(TSRTargets tsrtargets, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrtargets != null)
            {
                query = "SELECT [ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget] FROM [zHstTSRTargets] WHERE [zHstTSRTargets].[ID] = @ID AND [zHstTSRTargets].[StampUserID] = @StampUserID AND [zHstTSRTargets].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", tsrtargets.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the tsrtargetss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the tsrtargetss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [TSRTargets].[ID], [TSRTargets].[FKINWeekID], [TSRTargets].[FKINCampaignID], [TSRTargets].[FKAgentID], [TSRTargets].[DateFrom], [TSRTargets].[DateTo], [TSRTargets].[Hours], [TSRTargets].[BaseTarget], [TSRTargets].[PremiumTarget], [TSRTargets].[AccDisSelectedItem], [TSRTargets].[UnitTarget]");
            query.Append(" FROM [TSRTargets] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted tsrtargetss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted tsrtargetss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstTSRTargets].[ID], [zHstTSRTargets].[FKINWeekID], [zHstTSRTargets].[FKINCampaignID], [zHstTSRTargets].[FKAgentID], [zHstTSRTargets].[DateFrom], [zHstTSRTargets].[DateTo], [zHstTSRTargets].[Hours], [zHstTSRTargets].[BaseTarget], [zHstTSRTargets].[PremiumTarget], [zHstTSRTargets].[AccDisSelectedItem], [zHstTSRTargets].[UnitTarget]");
            query.Append(" FROM [zHstTSRTargets] ");
            query.Append("INNER JOIN (SELECT [zHstTSRTargets].[ID], MAX([zHstTSRTargets].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstTSRTargets] ");
            query.Append("WHERE [zHstTSRTargets].[ID] NOT IN (SELECT [TSRTargets].[ID] FROM [TSRTargets]) ");
            query.Append("GROUP BY [zHstTSRTargets].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstTSRTargets].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstTSRTargets].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) tsrtargets in the database.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) tsrtargets in the database.</returns>
        public static string ListHistory(TSRTargets tsrtargets, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (tsrtargets != null)
            {
            query.Append("SELECT [zHstTSRTargets].[ID], [zHstTSRTargets].[FKINWeekID], [zHstTSRTargets].[FKINCampaignID], [zHstTSRTargets].[FKAgentID], [zHstTSRTargets].[DateFrom], [zHstTSRTargets].[DateTo], [zHstTSRTargets].[Hours], [zHstTSRTargets].[BaseTarget], [zHstTSRTargets].[PremiumTarget], [zHstTSRTargets].[AccDisSelectedItem], [zHstTSRTargets].[UnitTarget]");
            query.Append(" FROM [zHstTSRTargets] ");
                query.Append(" WHERE [zHstTSRTargets].[ID] = @ID");
                query.Append(" ORDER BY [zHstTSRTargets].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrtargets.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) tsrtargets to the database.
        /// </summary>
        /// <param name="tsrtargets">The tsrtargets to save.</param>
        /// <returns>A query that can be used to save the tsrtargets to the database.</returns>
        internal static string Save(TSRTargets tsrtargets, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (tsrtargets != null)
            {
                if (tsrtargets.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstTSRTargets] ([ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget]) SELECT [ID], [FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget] FROM [TSRTargets] WHERE [TSRTargets].[ID] = @ID; ");
                    query.Append("UPDATE [TSRTargets]");
                    parameters = new object[11];
                    query.Append(" SET [FKINWeekID] = @FKINWeekID");
                    parameters[0] = Database.GetParameter("@FKINWeekID", tsrtargets.FKINWeekID.HasValue ? (object)tsrtargets.FKINWeekID.Value : DBNull.Value);
                    query.Append(", [FKINCampaignID] = @FKINCampaignID");
                    parameters[1] = Database.GetParameter("@FKINCampaignID", tsrtargets.FKINCampaignID.HasValue ? (object)tsrtargets.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [FKAgentID] = @FKAgentID");
                    parameters[2] = Database.GetParameter("@FKAgentID", tsrtargets.FKAgentID.HasValue ? (object)tsrtargets.FKAgentID.Value : DBNull.Value);
                    query.Append(", [DateFrom] = @DateFrom");
                    parameters[3] = Database.GetParameter("@DateFrom", tsrtargets.DateFrom.HasValue ? (object)tsrtargets.DateFrom.Value : DBNull.Value);
                    query.Append(", [DateTo] = @DateTo");
                    parameters[4] = Database.GetParameter("@DateTo", tsrtargets.DateTo.HasValue ? (object)tsrtargets.DateTo.Value : DBNull.Value);
                    query.Append(", [Hours] = @Hours");
                    parameters[5] = Database.GetParameter("@Hours", tsrtargets.Hours.HasValue ? (object)tsrtargets.Hours.Value : DBNull.Value);
                    query.Append(", [BaseTarget] = @BaseTarget");
                    parameters[6] = Database.GetParameter("@BaseTarget", tsrtargets.BaseTarget.HasValue ? (object)tsrtargets.BaseTarget.Value : DBNull.Value);
                    query.Append(", [PremiumTarget] = @PremiumTarget");
                    parameters[7] = Database.GetParameter("@PremiumTarget", tsrtargets.PremiumTarget.HasValue ? (object)tsrtargets.PremiumTarget.Value : DBNull.Value);
                    query.Append(", [AccDisSelectedItem] = @AccDisSelectedItem");
                    parameters[8] = Database.GetParameter("@AccDisSelectedItem", string.IsNullOrEmpty(tsrtargets.AccDisSelectedItem) ? DBNull.Value : (object)tsrtargets.AccDisSelectedItem);
                    query.Append(", [UnitTarget] = @UnitTarget");
                    parameters[9] = Database.GetParameter("@UnitTarget", tsrtargets.UnitTarget.HasValue ? (object)tsrtargets.UnitTarget.Value : DBNull.Value);
                    query.Append(" WHERE [TSRTargets].[ID] = @ID"); 
                    parameters[10] = Database.GetParameter("@ID", tsrtargets.ID);
                }
                else
                {
                    query.Append("INSERT INTO [TSRTargets] ([FKINWeekID], [FKINCampaignID], [FKAgentID], [DateFrom], [DateTo], [Hours], [BaseTarget], [PremiumTarget], [AccDisSelectedItem], [UnitTarget]) VALUES(@FKINWeekID, @FKINCampaignID, @FKAgentID, @DateFrom, @DateTo, @Hours, @BaseTarget, @PremiumTarget, @AccDisSelectedItem, @UnitTarget);");
                    parameters = new object[10];
                    parameters[0] = Database.GetParameter("@FKINWeekID", tsrtargets.FKINWeekID.HasValue ? (object)tsrtargets.FKINWeekID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINCampaignID", tsrtargets.FKINCampaignID.HasValue ? (object)tsrtargets.FKINCampaignID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKAgentID", tsrtargets.FKAgentID.HasValue ? (object)tsrtargets.FKAgentID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@DateFrom", tsrtargets.DateFrom.HasValue ? (object)tsrtargets.DateFrom.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@DateTo", tsrtargets.DateTo.HasValue ? (object)tsrtargets.DateTo.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@Hours", tsrtargets.Hours.HasValue ? (object)tsrtargets.Hours.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@BaseTarget", tsrtargets.BaseTarget.HasValue ? (object)tsrtargets.BaseTarget.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@PremiumTarget", tsrtargets.PremiumTarget.HasValue ? (object)tsrtargets.PremiumTarget.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@AccDisSelectedItem", string.IsNullOrEmpty(tsrtargets.AccDisSelectedItem) ? DBNull.Value : (object)tsrtargets.AccDisSelectedItem);
                    parameters[9] = Database.GetParameter("@UnitTarget", tsrtargets.UnitTarget.HasValue ? (object)tsrtargets.UnitTarget.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for tsrtargetss that match the search criteria.
        /// </summary>
        /// <param name="fkinweekid">The fkinweekid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="fkagentid">The fkagentid search criteria.</param>
        /// <param name="datefrom">The datefrom search criteria.</param>
        /// <param name="dateto">The dateto search criteria.</param>
        /// <param name="hours">The hours search criteria.</param>
        /// <param name="basetarget">The basetarget search criteria.</param>
        /// <param name="premiumtarget">The premiumtarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="unittarget">The unittarget search criteria.</param>
        /// <returns>A query that can be used to search for tsrtargetss based on the search criteria.</returns>
        internal static string Search(long? fkinweekid, long? fkincampaignid, long? fkagentid, DateTime? datefrom, DateTime? dateto, Double? hours, Double? basetarget, Double? premiumtarget, string accdisselecteditem, Double? unittarget)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinweekid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[FKINWeekID] = " + fkinweekid + "");
            }
            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[FKINCampaignID] = " + fkincampaignid + "");
            }
            if (fkagentid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[FKAgentID] = " + fkagentid + "");
            }
            if (datefrom != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[DateFrom] = '" + datefrom.Value.ToString(Database.DateFormat) + "'");
            }
            if (dateto != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[DateTo] = '" + dateto.Value.ToString(Database.DateFormat) + "'");
            }
            if (hours != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[Hours] = " + hours + "");
            }
            if (basetarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[BaseTarget] = " + basetarget + "");
            }
            if (premiumtarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[PremiumTarget] = " + premiumtarget + "");
            }
            if (accdisselecteditem != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[AccDisSelectedItem] LIKE '" + accdisselecteditem.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (unittarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRTargets].[UnitTarget] = " + unittarget + "");
            }
            query.Append("SELECT [TSRTargets].[ID], [TSRTargets].[FKINWeekID], [TSRTargets].[FKINCampaignID], [TSRTargets].[FKAgentID], [TSRTargets].[DateFrom], [TSRTargets].[DateTo], [TSRTargets].[Hours], [TSRTargets].[BaseTarget], [TSRTargets].[PremiumTarget], [TSRTargets].[AccDisSelectedItem], [TSRTargets].[UnitTarget]");
            query.Append(" FROM [TSRTargets] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
