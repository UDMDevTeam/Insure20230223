using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to INMySuccessAgentDetails objects.
    /// </summary>
    /// 



    internal abstract partial class INMySuccessAgentDetailsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) iNMySuccessAgentDetails from the database.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The iNMySuccessAgentDetails object to delete.</param>
        /// <returns>A query that can be used to delete the iNMySuccessAgentDetails from the database.</returns>
        /// 
        public string DocumentID = GlobalSettings.AgentNotesID;

        internal static string Delete(INMySuccessAgentDetails iNMySuccessAgentDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.AgentNotesID;
            if (iNMySuccessAgentDetails != null)
            {
                query = "INSERT INTO [INMySuccessAgentsNotesDetails] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID; ";
                query += "DELETE FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) iNMySuccessAgentDetails from the database.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The iNMySuccessAgentDetails object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the iNMySuccessAgentDetails from the database.</returns>
        internal static string DeleteHistory(INMySuccessAgentDetails iNMySuccessAgentDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.AgentNotesID;

            if (iNMySuccessAgentDetails != null)
            {
                query = "DELETE FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) iNMySuccessAgentDetails from the database.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The iNMySuccessAgentDetails object to undelete.</param>
        /// <returns>A query that can be used to undelete the iNMySuccessAgentDetails from the database.</returns>
        internal static string UnDelete(INMySuccessAgentDetails iNMySuccessAgentDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.AgentNotesID;

            if (iNMySuccessAgentDetails != null)
            {
                query = "INSERT INTO [INMySuccessAgentsNotesDetails] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID AND [INMySuccessAgentsNotesDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INMySuccessAgentsNotesDetails] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID AND [INMySuccessAgentsNotesDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INMySuccessAgentsNotesDetails] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an iNMySuccessAgentDetails object.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The iNMySuccessAgentDetails object to fill.</param>
        /// <returns>A query that can be used to fill the iNMySuccessAgentDetails object.</returns>
        internal static string Fill(INMySuccessAgentDetails iNMySuccessAgentDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.AgentNotesID;

            if (iNMySuccessAgentDetails != null)
            {
                if (DocumentID == "1")
                {
                    query = "SELECT [ID], [FKCampaignID], [Tone], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                else if (DocumentID == "2")
                {
                    query = "SELECT [ID], [FKCampaignID], [SellingApproach], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                else if (DocumentID == "3")
                {
                    query = "SELECT [ID], [FKCampaignID], [Tips], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                if (DocumentID == "4")
                {
                    query = "SELECT [ID], [FKCampaignID], [Techniques], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                if (DocumentID == "5")
                {
                    query = "SELECT [ID], [FKCampaignID], [MessagesFromAgent], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  iNMySuccessAgentDetails data.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The iNMySuccessAgentDetails to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  iNMySuccessAgentDetails data.</returns>
        internal static string FillData(INMySuccessAgentDetails iNMySuccessAgentDetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (iNMySuccessAgentDetails != null)
            {
                if (DocumentID == "1")
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tone], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                else if (DocumentID == "2")
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[SellingApproach], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                else if (DocumentID == "3")
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tips], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                else if (DocumentID == "4")
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Techniques], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                else if (DocumentID == "5")
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[MessagesFromAgent], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                }
                
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an iNMySuccessAgentDetails object from history.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The iNMySuccessAgentDetails object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the iNMySuccessAgentDetails object from history.</returns>
        internal static string FillHistory(INMySuccessAgentDetails iNMySuccessAgentDetails, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (iNMySuccessAgentDetails != null)
            {
                if (DocumentID == "1")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [Tone], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID AND [INMySuccessAgentsNotesDetails].[StampUserID] = @StampUserID AND [INMySuccessAgentsNotesDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "2")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [SellingApproach], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID AND [INMySuccessAgentsNotesDetails].[StampUserID] = @StampUserID AND [INMySuccessAgentsNotesDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "3")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [Tips], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID AND [INMySuccessAgentsNotesDetails].[StampUserID] = @StampUserID AND [INMySuccessAgentsNotesDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "4")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [Techniques], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID AND [INMySuccessAgentsNotesDetails].[StampUserID] = @StampUserID AND [INMySuccessAgentsNotesDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "5")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [MessagesFromAgent], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID AND [INMySuccessAgentsNotesDetails].[StampUserID] = @StampUserID AND [INMySuccessAgentsNotesDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the INMySuccessAgentDetails in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the INMySuccessAgentDetails in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (DocumentID == "1")
            {
                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tone], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "2")
            {
                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[SellingApproach], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "3")
            {
                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tips], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "4")
            {
                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Techniques], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "5")
            {
                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[MessagesFromAgent], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
           
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted INMySuccessAgentsNotesDetails in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted INMySuccessAgentsNotesDetails in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[FKLanguageID], [INMySuccessAgentsNotesDetails].[Document], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            query.Append("INNER JOIN (SELECT [INMySuccessAgentsNotesDetails].[ID], MAX([INMySuccessAgentsNotesDetails].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [INMySuccessAgentsNotesDetails] ");
            query.Append("WHERE [INMySuccessAgentsNotesDetails].[ID] NOT IN (SELECT [INMySuccessAgentsNotesDetails].[ID] FROM [INMySuccessAgentsNotesDetails]) ");
            query.Append("GROUP BY [INMySuccessAgentsNotesDetails].[ID]) AS [LastHistory] ");
            query.Append("ON [INMySuccessAgentsNotesDetails].[ID] = [LastHistory].[ID] ");
            query.Append("AND [INMySuccessAgentsNotesDetails].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) INMySuccessAgentsNotesDetails in the database.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The INMySuccessAgentsNotesDetails object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) INMySuccessAgentsNotesDetails in the database.</returns>
        public static string ListHistory(INMySuccessAgentDetails iNMySuccessAgentDetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (DocumentID == "1")
            {
                if (iNMySuccessAgentDetails != null)
                {

                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tone], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessAgentsNotesDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);

                }

            }
            else if (DocumentID == "2")
            {
                if (iNMySuccessAgentDetails != null)
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[SellingApproach], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessAgentsNotesDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);

                }
            }
            else if (DocumentID == "3")
            {
                if (iNMySuccessAgentDetails != null)
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tips], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessAgentsNotesDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);

                }
            }
            else if (DocumentID == "4")
            {
                if (iNMySuccessAgentDetails != null)
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Techniques], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessAgentsNotesDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);

                }
            }
            else if (DocumentID == "5")
            {
                if (iNMySuccessAgentDetails != null)
                {
                    query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[MessagesFromAgent], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
                    query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessAgentsNotesDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);

                }
            }
            
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) iNMySuccessAgentDetails to the database.
        /// </summary>
        /// <param name="iNMySuccessAgentDetails">The iNMySuccessAgentDetails to save.</param>
        /// <returns>A query that can be used to save the iNMySuccessAgentDetails to the database.</returns>
        internal static string Save(INMySuccessAgentDetails iNMySuccessAgentDetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();

            string DocumentID = GlobalSettings.AgentNotesID;
            string ColumnID = GlobalSettings.ColumnIDMySuccessID;

            if (iNMySuccessAgentDetails != null)
            {
                if (DocumentID == "1")
                {
                    if (iNMySuccessAgentDetails.IsLoaded)
                    {

                        query.Append("INSERT INTO [zHstINMySuccessAgentsNotesDetails] ([FKCampaignID], [Tone], [StampDate], [StampUserID]) SELECT [FKCampaignID], [Tone], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID; ");
                        query.Append("UPDATE [INMySuccessAgentsNotesDetails]");
                        parameters = new object[3];
                        query.Append(" SET [FKCampaignID] = @FKCampaignID");
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        query.Append(", [Tone] = @Document");
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                        query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = " + ColumnID);
                        parameters[2] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    }
                    else
                    {
                        query.Append("INSERT INTO [INMySuccessAgentsNotesDetails] ([FKCampaignID], [Tone], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                        parameters = new object[2];
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                    }
                }
                else if (DocumentID == "2")
                {
                    if (iNMySuccessAgentDetails.IsLoaded)
                    {

                        query.Append("INSERT INTO [zHstINMySuccessAgentsNotesDetails] ([FKCampaignID], [SellingApproach], [StampDate], [StampUserID]) SELECT [FKCampaignID], [SellingApproach], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID; ");
                        query.Append("UPDATE [INMySuccessAgentsNotesDetails]");
                        parameters = new object[3];
                        query.Append(" SET [FKCampaignID] = @FKCampaignID");
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        query.Append(", [SellingApproach] = @Document");
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                        query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                        parameters[2] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    }
                    else
                    {
                        query.Append("INSERT INTO [INMySuccessAgentsNotesDetails] ([FKCampaignID], [SellingApproach], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                        parameters = new object[2];
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                    }
                }
                else if (DocumentID == "3")
                {
                    if (iNMySuccessAgentDetails.IsLoaded)
                    {

                        query.Append("INSERT INTO [zHstINMySuccessAgentsNotesDetails] ([FKCampaignID], [Tips], [StampDate], [StampUserID]) SELECT [FKCampaignID], [Tips], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID; ");
                        query.Append("UPDATE [INMySuccessAgentsNotesDetails]");
                        parameters = new object[3];
                        query.Append(" SET [FKCampaignID] = @FKCampaignID");
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        query.Append(", [Tips] = @Document");
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                        query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                        parameters[2] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    }
                    else
                    {
                        query.Append("INSERT INTO [INMySuccessAgentsNotesDetails] ([FKCampaignID], [Tips], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                        parameters = new object[2];
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                    }
                }
                else if (DocumentID == "4")
                {
                    if (iNMySuccessAgentDetails.IsLoaded)
                    {

                        query.Append("INSERT INTO [zHstINMySuccessAgentsNotesDetails] ([FKCampaignID], [Techniques], [StampDate], [StampUserID]) SELECT [FKCampaignID], [Techniques], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID; ");
                        query.Append("UPDATE [INMySuccessAgentsNotesDetails]");
                        parameters = new object[3];
                        query.Append(" SET [FKCampaignID] = @FKCampaignID");
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        query.Append(", [Techniques] = @Document");
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                        query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                        parameters[2] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    }
                    else
                    {
                        query.Append("INSERT INTO [INMySuccessAgentsNotesDetails] ([FKCampaignID], [Techniques], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                        parameters = new object[2];
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                    }
                }
                else if (DocumentID == "5")
                {
                    if (iNMySuccessAgentDetails.IsLoaded)
                    {

                        query.Append("INSERT INTO [zHstINMySuccessAgentsNotesDetails] ([FKCampaignID], [MessagesFromAgent], [StampDate], [StampUserID]) SELECT [FKCampaignID], [MessagesFromAgent], [StampDate], [StampUserID] FROM [INMySuccessAgentsNotesDetails] WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID; ");
                        query.Append("UPDATE [INMySuccessAgentsNotesDetails]");
                        parameters = new object[3];
                        query.Append(" SET [FKCampaignID] = @FKCampaignID");
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        query.Append(", [MessagesFromAgent] = @Document");
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                        query.Append(" WHERE [INMySuccessAgentsNotesDetails].[ID] = @ID");
                        parameters[2] = Database.GetParameter("@ID", iNMySuccessAgentDetails.ID);
                    }
                    else
                    {
                        query.Append("INSERT INTO [INMySuccessAgentsNotesDetails] ([FKCampaignID], [MessagesFromAgent], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                        parameters = new object[2];
                        parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessAgentDetails.FKCampaignID.HasValue ? (object)iNMySuccessAgentDetails.FKCampaignID.Value : DBNull.Value);
                        parameters[1] = Database.GetParameter("@Document", iNMySuccessAgentDetails.Document);
                        query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                    }
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for INMySuccessAgentDetails that match the search criteria.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <returns>A query that can be used to search for INMySuccessAgentDetails based on the search criteria.</returns>
        internal static string Search(long? fkcampaignid, byte[] document)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;


            if (DocumentID == "1")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[Tone] = " + document + "");
                }

                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tone], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "2")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[SellingApproach] = " + document + "");
                }

                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[SellingApproach], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "3")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[Tips] = " + document + "");
                }

                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Tips], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "4")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[Techniques] = " + document + "");
                }

                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[Techniques], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }
            else if (DocumentID == "5")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessAgentsNotesDetails].[MessagesFromAgent] = " + document + "");
                }

                query.Append("SELECT [INMySuccessAgentsNotesDetails].[ID], [INMySuccessAgentsNotesDetails].[FKCampaignID], [INMySuccessAgentsNotesDetails].[MessagesFromAgent], [INMySuccessAgentsNotesDetails].[StampDate], [INMySuccessAgentsNotesDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessAgentsNotesDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessAgentsNotesDetails] ");
            }

            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}