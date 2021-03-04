using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to tsrcampaigntargetdefaults objects.
    /// </summary>
    internal abstract partial class TSRCampaignTargetDefaultsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) tsrcampaigntargetdefaults from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults object to delete.</param>
        /// <returns>A query that can be used to delete the tsrcampaigntargetdefaults from the database.</returns>
        internal static string Delete(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrcampaigntargetdefaults != null)
            {
                query = "INSERT INTO [zHstTSRCampaignTargetDefaults] ([ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID]) SELECT [ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID] FROM [TSRCampaignTargetDefaults] WHERE [TSRCampaignTargetDefaults].[ID] = @ID; ";
                query += "DELETE FROM [TSRCampaignTargetDefaults] WHERE [TSRCampaignTargetDefaults].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) tsrcampaigntargetdefaults from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the tsrcampaigntargetdefaults from the database.</returns>
        internal static string DeleteHistory(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrcampaigntargetdefaults != null)
            {
                query = "DELETE FROM [zHstTSRCampaignTargetDefaults] WHERE [zHstTSRCampaignTargetDefaults].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) tsrcampaigntargetdefaults from the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults object to undelete.</param>
        /// <returns>A query that can be used to undelete the tsrcampaigntargetdefaults from the database.</returns>
        internal static string UnDelete(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrcampaigntargetdefaults != null)
            {
                query = "INSERT INTO [TSRCampaignTargetDefaults] ([ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID]) SELECT [ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID] FROM [zHstTSRCampaignTargetDefaults] WHERE [zHstTSRCampaignTargetDefaults].[ID] = @ID AND [zHstTSRCampaignTargetDefaults].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstTSRCampaignTargetDefaults] WHERE [zHstTSRCampaignTargetDefaults].[ID] = @ID) AND (SELECT COUNT(ID) FROM [TSRCampaignTargetDefaults] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstTSRCampaignTargetDefaults] WHERE [zHstTSRCampaignTargetDefaults].[ID] = @ID AND [zHstTSRCampaignTargetDefaults].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstTSRCampaignTargetDefaults] WHERE [zHstTSRCampaignTargetDefaults].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [TSRCampaignTargetDefaults] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an tsrcampaigntargetdefaults object.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults object to fill.</param>
        /// <returns>A query that can be used to fill the tsrcampaigntargetdefaults object.</returns>
        internal static string Fill(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrcampaigntargetdefaults != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID] FROM [TSRCampaignTargetDefaults] WHERE [TSRCampaignTargetDefaults].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  tsrcampaigntargetdefaults data.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  tsrcampaigntargetdefaults data.</returns>
        internal static string FillData(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (tsrcampaigntargetdefaults != null)
            {
            query.Append("SELECT [TSRCampaignTargetDefaults].[ID], [TSRCampaignTargetDefaults].[FKINCampaignID], [TSRCampaignTargetDefaults].[SalesPerHourTarget], [TSRCampaignTargetDefaults].[BasePremiumTarget], [TSRCampaignTargetDefaults].[PartnerPremiumTarget], [TSRCampaignTargetDefaults].[ChildPremiumTarget], [TSRCampaignTargetDefaults].[PartnerTarget], [TSRCampaignTargetDefaults].[ChildTarget], [TSRCampaignTargetDefaults].[DateApplicableFrom], [TSRCampaignTargetDefaults].[BaseUnitTarget], [TSRCampaignTargetDefaults].[AccDisSelectedItem], [TSRCampaignTargetDefaults].[FKINCampaignClusterID]");
            query.Append(" FROM [TSRCampaignTargetDefaults] ");
                query.Append(" WHERE [TSRCampaignTargetDefaults].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an tsrcampaigntargetdefaults object from history.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the tsrcampaigntargetdefaults object from history.</returns>
        internal static string FillHistory(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (tsrcampaigntargetdefaults != null)
            {
                query = "SELECT [ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID] FROM [zHstTSRCampaignTargetDefaults] WHERE [zHstTSRCampaignTargetDefaults].[ID] = @ID AND [zHstTSRCampaignTargetDefaults].[StampUserID] = @StampUserID AND [zHstTSRCampaignTargetDefaults].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the tsrcampaigntargetdefaultss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the tsrcampaigntargetdefaultss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [TSRCampaignTargetDefaults].[ID], [TSRCampaignTargetDefaults].[FKINCampaignID], [TSRCampaignTargetDefaults].[SalesPerHourTarget], [TSRCampaignTargetDefaults].[BasePremiumTarget], [TSRCampaignTargetDefaults].[PartnerPremiumTarget], [TSRCampaignTargetDefaults].[ChildPremiumTarget], [TSRCampaignTargetDefaults].[PartnerTarget], [TSRCampaignTargetDefaults].[ChildTarget], [TSRCampaignTargetDefaults].[DateApplicableFrom], [TSRCampaignTargetDefaults].[BaseUnitTarget], [TSRCampaignTargetDefaults].[AccDisSelectedItem], [TSRCampaignTargetDefaults].[FKINCampaignClusterID]");
            query.Append(" FROM [TSRCampaignTargetDefaults] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted tsrcampaigntargetdefaultss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted tsrcampaigntargetdefaultss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstTSRCampaignTargetDefaults].[ID], [zHstTSRCampaignTargetDefaults].[FKINCampaignID], [zHstTSRCampaignTargetDefaults].[SalesPerHourTarget], [zHstTSRCampaignTargetDefaults].[BasePremiumTarget], [zHstTSRCampaignTargetDefaults].[PartnerPremiumTarget], [zHstTSRCampaignTargetDefaults].[ChildPremiumTarget], [zHstTSRCampaignTargetDefaults].[PartnerTarget], [zHstTSRCampaignTargetDefaults].[ChildTarget], [zHstTSRCampaignTargetDefaults].[DateApplicableFrom], [zHstTSRCampaignTargetDefaults].[BaseUnitTarget], [zHstTSRCampaignTargetDefaults].[AccDisSelectedItem], [zHstTSRCampaignTargetDefaults].[FKINCampaignClusterID]");
            query.Append(" FROM [zHstTSRCampaignTargetDefaults] ");
            query.Append("INNER JOIN (SELECT [zHstTSRCampaignTargetDefaults].[ID], MAX([zHstTSRCampaignTargetDefaults].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstTSRCampaignTargetDefaults] ");
            query.Append("WHERE [zHstTSRCampaignTargetDefaults].[ID] NOT IN (SELECT [TSRCampaignTargetDefaults].[ID] FROM [TSRCampaignTargetDefaults]) ");
            query.Append("GROUP BY [zHstTSRCampaignTargetDefaults].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstTSRCampaignTargetDefaults].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstTSRCampaignTargetDefaults].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) tsrcampaigntargetdefaults in the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) tsrcampaigntargetdefaults in the database.</returns>
        public static string ListHistory(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (tsrcampaigntargetdefaults != null)
            {
            query.Append("SELECT [zHstTSRCampaignTargetDefaults].[ID], [zHstTSRCampaignTargetDefaults].[FKINCampaignID], [zHstTSRCampaignTargetDefaults].[SalesPerHourTarget], [zHstTSRCampaignTargetDefaults].[BasePremiumTarget], [zHstTSRCampaignTargetDefaults].[PartnerPremiumTarget], [zHstTSRCampaignTargetDefaults].[ChildPremiumTarget], [zHstTSRCampaignTargetDefaults].[PartnerTarget], [zHstTSRCampaignTargetDefaults].[ChildTarget], [zHstTSRCampaignTargetDefaults].[DateApplicableFrom], [zHstTSRCampaignTargetDefaults].[BaseUnitTarget], [zHstTSRCampaignTargetDefaults].[AccDisSelectedItem], [zHstTSRCampaignTargetDefaults].[FKINCampaignClusterID]");
            query.Append(" FROM [zHstTSRCampaignTargetDefaults] ");
                query.Append(" WHERE [zHstTSRCampaignTargetDefaults].[ID] = @ID");
                query.Append(" ORDER BY [zHstTSRCampaignTargetDefaults].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) tsrcampaigntargetdefaults to the database.
        /// </summary>
        /// <param name="tsrcampaigntargetdefaults">The tsrcampaigntargetdefaults to save.</param>
        /// <returns>A query that can be used to save the tsrcampaigntargetdefaults to the database.</returns>
        internal static string Save(TSRCampaignTargetDefaults tsrcampaigntargetdefaults, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (tsrcampaigntargetdefaults != null)
            {
                if (tsrcampaigntargetdefaults.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstTSRCampaignTargetDefaults] ([ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID]) SELECT [ID], [FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID] FROM [TSRCampaignTargetDefaults] WHERE [TSRCampaignTargetDefaults].[ID] = @ID; ");
                    query.Append("UPDATE [TSRCampaignTargetDefaults]");
                    parameters = new object[12];
                    query.Append(" SET [FKINCampaignID] = @FKINCampaignID");
                    parameters[0] = Database.GetParameter("@FKINCampaignID", tsrcampaigntargetdefaults.FKINCampaignID.HasValue ? (object)tsrcampaigntargetdefaults.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [SalesPerHourTarget] = @SalesPerHourTarget");
                    parameters[1] = Database.GetParameter("@SalesPerHourTarget", tsrcampaigntargetdefaults.SalesPerHourTarget.HasValue ? (object)tsrcampaigntargetdefaults.SalesPerHourTarget.Value : DBNull.Value);
                    query.Append(", [BasePremiumTarget] = @BasePremiumTarget");
                    parameters[2] = Database.GetParameter("@BasePremiumTarget", tsrcampaigntargetdefaults.BasePremiumTarget.HasValue ? (object)tsrcampaigntargetdefaults.BasePremiumTarget.Value : DBNull.Value);
                    query.Append(", [PartnerPremiumTarget] = @PartnerPremiumTarget");
                    parameters[3] = Database.GetParameter("@PartnerPremiumTarget", tsrcampaigntargetdefaults.PartnerPremiumTarget.HasValue ? (object)tsrcampaigntargetdefaults.PartnerPremiumTarget.Value : DBNull.Value);
                    query.Append(", [ChildPremiumTarget] = @ChildPremiumTarget");
                    parameters[4] = Database.GetParameter("@ChildPremiumTarget", tsrcampaigntargetdefaults.ChildPremiumTarget.HasValue ? (object)tsrcampaigntargetdefaults.ChildPremiumTarget.Value : DBNull.Value);
                    query.Append(", [PartnerTarget] = @PartnerTarget");
                    parameters[5] = Database.GetParameter("@PartnerTarget", tsrcampaigntargetdefaults.PartnerTarget.HasValue ? (object)tsrcampaigntargetdefaults.PartnerTarget.Value : DBNull.Value);
                    query.Append(", [ChildTarget] = @ChildTarget");
                    parameters[6] = Database.GetParameter("@ChildTarget", tsrcampaigntargetdefaults.ChildTarget.HasValue ? (object)tsrcampaigntargetdefaults.ChildTarget.Value : DBNull.Value);
                    query.Append(", [DateApplicableFrom] = @DateApplicableFrom");
                    parameters[7] = Database.GetParameter("@DateApplicableFrom", tsrcampaigntargetdefaults.DateApplicableFrom.HasValue ? (object)tsrcampaigntargetdefaults.DateApplicableFrom.Value : DBNull.Value);
                    query.Append(", [BaseUnitTarget] = @BaseUnitTarget");
                    parameters[8] = Database.GetParameter("@BaseUnitTarget", tsrcampaigntargetdefaults.BaseUnitTarget.HasValue ? (object)tsrcampaigntargetdefaults.BaseUnitTarget.Value : DBNull.Value);
                    query.Append(", [AccDisSelectedItem] = @AccDisSelectedItem");
                    parameters[9] = Database.GetParameter("@AccDisSelectedItem", string.IsNullOrEmpty(tsrcampaigntargetdefaults.AccDisSelectedItem) ? DBNull.Value : (object)tsrcampaigntargetdefaults.AccDisSelectedItem);
                    query.Append(", [FKINCampaignClusterID] = @FKINCampaignClusterID");
                    parameters[10] = Database.GetParameter("@FKINCampaignClusterID", tsrcampaigntargetdefaults.FKINCampaignClusterID.HasValue ? (object)tsrcampaigntargetdefaults.FKINCampaignClusterID.Value : DBNull.Value);
                    query.Append(" WHERE [TSRCampaignTargetDefaults].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", tsrcampaigntargetdefaults.ID);
                }
                else
                {
                    query.Append("INSERT INTO [TSRCampaignTargetDefaults] ([FKINCampaignID], [SalesPerHourTarget], [BasePremiumTarget], [PartnerPremiumTarget], [ChildPremiumTarget], [PartnerTarget], [ChildTarget], [DateApplicableFrom], [BaseUnitTarget], [AccDisSelectedItem], [FKINCampaignClusterID]) VALUES(@FKINCampaignID, @SalesPerHourTarget, @BasePremiumTarget, @PartnerPremiumTarget, @ChildPremiumTarget, @PartnerTarget, @ChildTarget, @DateApplicableFrom, @BaseUnitTarget, @AccDisSelectedItem, @FKINCampaignClusterID);");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@FKINCampaignID", tsrcampaigntargetdefaults.FKINCampaignID.HasValue ? (object)tsrcampaigntargetdefaults.FKINCampaignID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@SalesPerHourTarget", tsrcampaigntargetdefaults.SalesPerHourTarget.HasValue ? (object)tsrcampaigntargetdefaults.SalesPerHourTarget.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@BasePremiumTarget", tsrcampaigntargetdefaults.BasePremiumTarget.HasValue ? (object)tsrcampaigntargetdefaults.BasePremiumTarget.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@PartnerPremiumTarget", tsrcampaigntargetdefaults.PartnerPremiumTarget.HasValue ? (object)tsrcampaigntargetdefaults.PartnerPremiumTarget.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@ChildPremiumTarget", tsrcampaigntargetdefaults.ChildPremiumTarget.HasValue ? (object)tsrcampaigntargetdefaults.ChildPremiumTarget.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@PartnerTarget", tsrcampaigntargetdefaults.PartnerTarget.HasValue ? (object)tsrcampaigntargetdefaults.PartnerTarget.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@ChildTarget", tsrcampaigntargetdefaults.ChildTarget.HasValue ? (object)tsrcampaigntargetdefaults.ChildTarget.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@DateApplicableFrom", tsrcampaigntargetdefaults.DateApplicableFrom.HasValue ? (object)tsrcampaigntargetdefaults.DateApplicableFrom.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@BaseUnitTarget", tsrcampaigntargetdefaults.BaseUnitTarget.HasValue ? (object)tsrcampaigntargetdefaults.BaseUnitTarget.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@AccDisSelectedItem", string.IsNullOrEmpty(tsrcampaigntargetdefaults.AccDisSelectedItem) ? DBNull.Value : (object)tsrcampaigntargetdefaults.AccDisSelectedItem);
                    parameters[10] = Database.GetParameter("@FKINCampaignClusterID", tsrcampaigntargetdefaults.FKINCampaignClusterID.HasValue ? (object)tsrcampaigntargetdefaults.FKINCampaignClusterID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for tsrcampaigntargetdefaultss that match the search criteria.
        /// </summary>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <param name="salesperhourtarget">The salesperhourtarget search criteria.</param>
        /// <param name="basepremiumtarget">The basepremiumtarget search criteria.</param>
        /// <param name="partnerpremiumtarget">The partnerpremiumtarget search criteria.</param>
        /// <param name="childpremiumtarget">The childpremiumtarget search criteria.</param>
        /// <param name="partnertarget">The partnertarget search criteria.</param>
        /// <param name="childtarget">The childtarget search criteria.</param>
        /// <param name="dateapplicablefrom">The dateapplicablefrom search criteria.</param>
        /// <param name="baseunittarget">The baseunittarget search criteria.</param>
        /// <param name="accdisselecteditem">The accdisselecteditem search criteria.</param>
        /// <param name="fkincampaignclusterid">The fkincampaignclusterid search criteria.</param>
        /// <returns>A query that can be used to search for tsrcampaigntargetdefaultss based on the search criteria.</returns>
        internal static string Search(long? fkincampaignid, Double? salesperhourtarget, decimal? basepremiumtarget, decimal? partnerpremiumtarget, decimal? childpremiumtarget, Double? partnertarget, Double? childtarget, DateTime? dateapplicablefrom, Double? baseunittarget, string accdisselecteditem, long? fkincampaignclusterid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[FKINCampaignID] = " + fkincampaignid + "");
            }
            if (salesperhourtarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[SalesPerHourTarget] = " + salesperhourtarget + "");
            }
            if (basepremiumtarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[BasePremiumTarget] = " + basepremiumtarget + "");
            }
            if (partnerpremiumtarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[PartnerPremiumTarget] = " + partnerpremiumtarget + "");
            }
            if (childpremiumtarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[ChildPremiumTarget] = " + childpremiumtarget + "");
            }
            if (partnertarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[PartnerTarget] = " + partnertarget + "");
            }
            if (childtarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[ChildTarget] = " + childtarget + "");
            }
            if (dateapplicablefrom != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[DateApplicableFrom] = '" + dateapplicablefrom.Value.ToString(Database.DateFormat) + "'");
            }
            if (baseunittarget != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[BaseUnitTarget] = " + baseunittarget + "");
            }
            if (accdisselecteditem != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[AccDisSelectedItem] LIKE '" + accdisselecteditem.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkincampaignclusterid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[TSRCampaignTargetDefaults].[FKINCampaignClusterID] = " + fkincampaignclusterid + "");
            }
            query.Append("SELECT [TSRCampaignTargetDefaults].[ID], [TSRCampaignTargetDefaults].[FKINCampaignID], [TSRCampaignTargetDefaults].[SalesPerHourTarget], [TSRCampaignTargetDefaults].[BasePremiumTarget], [TSRCampaignTargetDefaults].[PartnerPremiumTarget], [TSRCampaignTargetDefaults].[ChildPremiumTarget], [TSRCampaignTargetDefaults].[PartnerTarget], [TSRCampaignTargetDefaults].[ChildTarget], [TSRCampaignTargetDefaults].[DateApplicableFrom], [TSRCampaignTargetDefaults].[BaseUnitTarget], [TSRCampaignTargetDefaults].[AccDisSelectedItem], [TSRCampaignTargetDefaults].[FKINCampaignClusterID]");
            query.Append(" FROM [TSRCampaignTargetDefaults] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
