using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to iNImportCallMonitoringStats objects.
    /// </summary>
    /// 



    internal abstract partial class INCallMonitoringStatsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) iNImportCallMonitoringStats from the database.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The iNImportCallMonitoringStats object to delete.</param>
        /// <returns>A query that can be used to delete the iNImportCallMonitoringStats from the database.</returns>
        /// 

        internal static string Delete(INImportCallMonitoringStats iNImportCallMonitoringStats, ref object[] parameters)
        {
            string query = string.Empty;
            if (iNImportCallMonitoringStats != null)
            {
                query = "INSERT INTO [INCallMonitoringStats] ([FKINImportID], [FKlkpINCampaignGroupType], [StartTimeOverAssessment], [EndTimeOverAssessment], [StartTimeOverAssessorOutcome], [EndTimeOverAssessorOutcome], [StartTimeCFOverAssessment], [EndTimeCFOverAssessment] FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID; ";
                query += "DELETE FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) iNImportCallMonitoringStats from the database.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The iNImportCallMonitoringStats object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the iNImportCallMonitoringStats from the database.</returns>
        internal static string DeleteHistory(INImportCallMonitoringStats iNImportCallMonitoringStats, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.AgentNotesID;

            if (iNImportCallMonitoringStats != null)
            {
                query = "DELETE FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) iNImportCallMonitoringStats from the database.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The iNImportCallMonitoringStats object to undelete.</param>
        /// <returns>A query that can be used to undelete the iNImportCallMonitoringStats from the database.</returns>
        internal static string UnDelete(INImportCallMonitoringStats iNImportCallMonitoringStats, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.AgentNotesID;

            if (iNImportCallMonitoringStats != null)
            {
                query = "INSERT INTO [INCallMonitoringStats] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID AND [INCallMonitoringStats].[StampDate] = (SELECT MAX([StampDate]) FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INCallMonitoringStats] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID AND [INCallMonitoringStats].[StampDate] = (SELECT MAX([StampDate]) FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INCallMonitoringStats] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an iNImportCallMonitoringStats object.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The iNImportCallMonitoringStats object to fill.</param>
        /// <returns>A query that can be used to fill the iNImportCallMonitoringStats object.</returns>
        internal static string Fill(INImportCallMonitoringStats iNImportCallMonitoringStats, ref object[] parameters)
        {
            string query = string.Empty;

            if (iNImportCallMonitoringStats != null)
            {
                    query = "SELECT [FKINImportID], [FKlkpINCampaignGroupType], [StartTimeOverAssessment], [EndTimeOverAssessment], [StartTimeOverAssessorOutcome], [EndTimeOverAssessorOutcome], [StartTimeCFOverAssessment], [EndTimeCFOverAssessment] [StampDate], [StampUserID] FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);
            }
                
            
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  iNImportCallMonitoringStats data.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The iNImportCallMonitoringStats to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  iNImportCallMonitoringStats data.</returns>
        internal static string FillData(INImportCallMonitoringStats iNImportCallMonitoringStats, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();

            if (iNImportCallMonitoringStats != null)
            {
              
                    query.Append("SELECT [INCallMonitoringStats].[ID], [INCallMonitoringStats].[FKINImportID], [INCallMonitoringStats].[FKlkpINCampaignGroupType], [INCallMonitoringStats].[StartTimeOverAssessment],  [INCallMonitoringStats].[EndTimeOverAssessment] , [INCallMonitoringStats].[StartTimeOverAssessorOutcome] , [INCallMonitoringStats].[StartTimeCFOverAssessment] , [INCallMonitoringStats].[EndTimeCFOverAssessment], [INCallMonitoringStats].[StampDate], [INCallMonitoringStats].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCallMonitoringStats].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INCallMonitoringStats] ");
                    query.Append(" WHERE [INCallMonitoringStats].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);
               
                

            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an iNImportCallMonitoringStats object from history.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The iNImportCallMonitoringStats object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the iNImportCallMonitoringStats object from history.</returns>
        internal static string FillHistory(INImportCallMonitoringStats iNImportCallMonitoringStats, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;

            if (iNImportCallMonitoringStats != null)
            {
               
                    query = "SELECT [ID], [FKCampaignID], [ID], [Tone], [StampDate], [StampUserID] FROM [INCallMonitoringStats] WHERE [INCallMonitoringStats].[ID] = @ID AND [INCallMonitoringStats].[StampUserID] = @StampUserID AND [INCallMonitoringStats].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
              
                

            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the iNImportCallMonitoringStats in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the iNImportCallMonitoringStats in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();


                query.Append("SELECT [INCallMonitoringStats].[ID], [INCallMonitoringStats].[FKCampaignID], [INCallMonitoringStats].[Tone], [INCallMonitoringStats].[StampDate], [INCallMonitoringStats].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCallMonitoringStats].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INCallMonitoringStats] ");
          
            

            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted INCallMonitoringStats in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted INCallMonitoringStats in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT [INCallMonitoringStats].[ID], [INCallMonitoringStats].[FKCampaignID], [INCallMonitoringStats].[FKLanguageID], [INCallMonitoringStats].[Document], [INCallMonitoringStats].[StampDate], [INCallMonitoringStats].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCallMonitoringStats].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCallMonitoringStats] ");
            query.Append("INNER JOIN (SELECT [INCallMonitoringStats].[ID], MAX([INCallMonitoringStats].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [INCallMonitoringStats] ");
            query.Append("WHERE [INCallMonitoringStats].[ID] NOT IN (SELECT [INCallMonitoringStats].[ID] FROM [INCallMonitoringStats]) ");
            query.Append("GROUP BY [INCallMonitoringStats].[ID]) AS [LastHistory] ");
            query.Append("ON [INCallMonitoringStats].[ID] = [LastHistory].[ID] ");
            query.Append("AND [INCallMonitoringStats].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) INCallMonitoringStats in the database.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The INCallMonitoringStats object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) INCallMonitoringStats in the database.</returns>
        public static string ListHistory(INImportCallMonitoringStats iNImportCallMonitoringStats, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (DocumentID == "1")
            {
                if (iNImportCallMonitoringStats != null)
                {

                    query.Append("SELECT [INCallMonitoringStats].[ID], [INCallMonitoringStats].[FKCampaignID], [INCallMonitoringStats].[Tone], [INCallMonitoringStats].[StampDate], [INCallMonitoringStats].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCallMonitoringStats].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INCallMonitoringStats] ");
                    query.Append(" WHERE [INCallMonitoringStats].[ID] = @ID");
                    query.Append(" ORDER BY [INCallMonitoringStats].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);

                }

            }
            

            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) iNImportCallMonitoringStats to the database.
        /// </summary>
        /// <param name="iNImportCallMonitoringStats">The iNImportCallMonitoringStats to save.</param>
        /// <returns>A query that can be used to save the iNImportCallMonitoringStats to the database.</returns>
        internal static string Save(INImportCallMonitoringStats iNImportCallMonitoringStats, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();

            if (iNImportCallMonitoringStats != null)
            {
               
                    if (iNImportCallMonitoringStats.IsLoaded)
                    {

                        query.Append("INSERT INTO [zHstINCallMonitoringStats] ([FKINImportID], [FKlkpINCampaignGroupType], [StartTimeOverAssessment], [EndTimeOverAssessment], [StartTimeOverAssessorOutcome], [EndTimeOverAssessorOutcome], [StartTimeCFOverAssessment], [EndTimeCFOverAssessment], [StampUserID], [StampDate]) SELECT [FKINImportID], [FKlkpINCampaignGroupType], [StartTimeOverAssessment], [EndTimeOverAssessment], [StartTimeOverAssessorOutcome], [EndTimeOverAssessorOutcome], [StartTimeCFOverAssessment], [EndTimeCFOverAssessment], [StampUserID], [StampDate] FROM [zHstINCallMonitoringStats] WHERE [zHstINCallMonitoringStats].[ID] = @ID; ");
                        query.Append("UPDATE [INCallMonitoringStats]");
                    parameters = new object[9];
                    query.Append("SET [FKINImportID] = @FKINImportID");
                    parameters[0] = Database.GetParameter("@FKINImportID", iNImportCallMonitoringStats.FKINImportID.HasValue ? (object)iNImportCallMonitoringStats.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKlkpINCampaignGroupType] = @FKlkpINCampaignGroupType");
                    parameters[1] = Database.GetParameter("@FKlkpINCampaignGroupType", string.IsNullOrEmpty(iNImportCallMonitoringStats.FKINlkpCampaignGroupType) ? DBNull.Value : (object)iNImportCallMonitoringStats.FKINlkpCampaignGroupType);
                    query.Append(", [StartTimeOverAssessment] = @StartTimeOverAssessment");
                    parameters[2] = Database.GetParameter("@StartTimeOverAssessment", iNImportCallMonitoringStats.StartTimeOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.StartTimeOverAssessment.Value : DBNull.Value);
                    query.Append(", [EndTimeOverAssessment] = @EndTimeOverAssessment");
                    parameters[3] = Database.GetParameter("@EndTimeOverAssessment", iNImportCallMonitoringStats.EndTimeOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.EndTimeOverAssessment.Value : DBNull.Value);
                    query.Append(", [StartTimeOverAssessorOutcome] = @StartTimeOverAssessorOutcome");
                    parameters[4] = Database.GetParameter("@StartTimeOverAssessorOutcome", iNImportCallMonitoringStats.StartTimeOverAssessorOutcome.HasValue ? (object)iNImportCallMonitoringStats.StartTimeOverAssessorOutcome.Value : DBNull.Value);
                    query.Append(", [EndTimeOverAssessorOutcome] = @EndTimeOverAssessorOutcome");
                    parameters[5] = Database.GetParameter("@EndTimeOverAssessorOutcome", iNImportCallMonitoringStats.EndTimeOverAssessorOutcome.HasValue ? (object)iNImportCallMonitoringStats.EndTimeOverAssessorOutcome.Value : DBNull.Value);
                    query.Append(", [StartTimeCFOverAssessment] = @StartTimeCFOverAssessment");
                    parameters[6] = Database.GetParameter("@StartTimeCFOverAssessment", iNImportCallMonitoringStats.StartTimeCFOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.StartTimeCFOverAssessment.Value : DBNull.Value);
                    query.Append(", [EndTimeCFOverAssessment] = @EndTimeCFOverAssessment");
                    parameters[7] = Database.GetParameter("@EndTimeCFOverAssessment", iNImportCallMonitoringStats.EndTimeCFOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.EndTimeCFOverAssessment.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INCallMonitoringStats].[ID] = " + iNImportCallMonitoringStats.ID);
                    parameters[8] = Database.GetParameter("@ID", iNImportCallMonitoringStats.ID);

                    }

                    else
                    {
                        query.Append("INSERT INTO [INCallMonitoringStats] ([FKINImportID], [FKlkpINCampaignGroupType], [StartTimeOverAssessment], [EndTimeOverAssessment], [StartTimeOverAssessorOutcome], [EndTimeOverAssessorOutcome], [StartTimeCFOverAssessment], [EndTimeCFOverAssessment], [StampDate], [StampUserID]) VALUES(@FKINImportID, @FKlkpINCampaignGroupType, @StartTimeOverAssessment, @EndTimeOverAssessment, @StartTimeOverAssessorOutcome, @EndTimeOverAssessorOutcome, @StartTimeCFOverAssessment, @EndTimeCFOverAssessment, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                        parameters = new object[8];
                        parameters[0] = Database.GetParameter("@FKINImportID", iNImportCallMonitoringStats.FKINImportID.HasValue ? (object)iNImportCallMonitoringStats.FKINImportID.Value : DBNull.Value);
                        parameters[1] = Database.GetParameter("@FKlkpINCampaignGroupType", iNImportCallMonitoringStats.FKINlkpCampaignGroupType);
                        parameters[2] = Database.GetParameter("@StartTimeOverAssessment", iNImportCallMonitoringStats.StartTimeOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.StartTimeOverAssessment.Value : DBNull.Value);
                        parameters[3] = Database.GetParameter("@EndTimeOverAssessment", iNImportCallMonitoringStats.EndTimeOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.EndTimeOverAssessment.Value : DBNull.Value);
                        parameters[4] = Database.GetParameter("@StartTimeOverAssessorOutcome", iNImportCallMonitoringStats.StartTimeOverAssessorOutcome.HasValue ? (object)iNImportCallMonitoringStats.StartTimeOverAssessorOutcome.Value : DBNull.Value);
                        parameters[5] = Database.GetParameter("@EndTimeOverAssessorOutcome", iNImportCallMonitoringStats.EndTimeOverAssessorOutcome.HasValue ? (object)iNImportCallMonitoringStats.EndTimeOverAssessorOutcome.Value : DBNull.Value);
                        parameters[6] = Database.GetParameter("@StartTimeCFOverAssessment", iNImportCallMonitoringStats.StartTimeCFOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.StartTimeCFOverAssessment.Value : DBNull.Value);
                        parameters[7] = Database.GetParameter("@EndTimeCFOverAssessment", iNImportCallMonitoringStats.EndTimeCFOverAssessment.HasValue ? (object)iNImportCallMonitoringStats.EndTimeCFOverAssessment.Value : DBNull.Value);
                        query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                    }
                }
                 return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for iNImportCallMonitoringStats that match the search criteria.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <returns>A query that can be used to search for iNImportCallMonitoringStats based on the search criteria.</returns>
        internal static string Search(long? fkINImportID)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();
            
            
                if (fkINImportID != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INCallMonitoringStats].[FKINImportID] = " + fkINImportID + "");
                }
                
                query.Append("SELECT [INCallMonitoringStats].[ID], [INCallMonitoringStats].[FKINImportID], [INCallMonitoringStats].[FKlkpINCampaignGroupType], [INCallMonitoringStats].[StartTimeOverAssessment], [INCallMonitoringStats].[EndTimeOverAssessment], [INCallMonitoringStats].[StartTimeOverAssessorOutcome], [INCallMonitoringStats].[EndTimeOverAssessorOutcome], [INCallMonitoringStats].[StartTimeCFOverAssessment], [INCallMonitoringStats].[EndTimeCFOverAssessment], [INCallMonitoringStats].[StampDate], [INCallMonitoringStats].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCallMonitoringStats].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INCallMonitoringStats] ");
            

            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}