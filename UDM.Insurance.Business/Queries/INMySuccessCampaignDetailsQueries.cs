using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to INMySuccessCampaignDetails objects.
    /// </summary>
    /// 

    

    internal abstract partial class INMySuccessCampaignDetailsQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) INMySuccessCampaignDetails from the database.
        /// </summary>
        /// <param name="iNMySuccessCampaignDetails">The INMySuccessCampaignDetails object to delete.</param>
        /// <returns>A query that can be used to delete the INMySuccessCampaignDetails from the database.</returns>
        /// 
        public string DocumentID = GlobalSettings.CampaignNotesID;

        internal static string Delete(INMySuccessCampaignDetails iNMySuccessCampaignDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.CampaignNotesID;
            if (iNMySuccessCampaignDetails != null)
            {
                query = "INSERT INTO [INMySuccessCampaignDetails] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID; ";
                query += "DELETE FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) INMySuccessCampaignDetails from the database.
        /// </summary>
        /// <param name="iNMySuccessCampaignDetails">The INMySuccessCampaignDetails object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the INMySuccessCampaignDetails from the database.</returns>
        internal static string DeleteHistory(INMySuccessCampaignDetails iNMySuccessCampaignDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (iNMySuccessCampaignDetails != null)
            {
                query = "DELETE FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) INMySuccessCampaignDetails from the database.
        /// </summary>
        /// <param name="iNMySuccessCampaignDetails">The INMySuccessCampaignDetails object to undelete.</param>
        /// <returns>A query that can be used to undelete the INMySuccessCampaignDetails from the database.</returns>
        internal static string UnDelete(INMySuccessCampaignDetails iNMySuccessCampaignDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (iNMySuccessCampaignDetails != null)
            {
                query = "INSERT INTO [INMySuccessCampaignDetails] ([ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKCampaignID], [FKLanguageID], [Document], [IsActive], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INMySuccessCampaignDetails] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampDate] = (SELECT MAX([StampDate]) FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INMySuccessCampaignDetails] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an INMySuccessCampaignDetails object.
        /// </summary>
        /// <param name="iNMySuccessCampaignDetails">The INMySuccessCampaignDetails object to fill.</param>
        /// <returns>A query that can be used to fill the INMySuccessCampaignDetails object.</returns>
        internal static string Fill(INMySuccessCampaignDetails iNMySuccessCampaignDetails, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.CampaignNotesID;


            if (iNMySuccessCampaignDetails != null)
            {



                if (DocumentID == "1")
                {
                    query = "SELECT [ID], [FKCampaignID], [ScriptEng], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                else if (DocumentID == "2")
                {
                    query = "SELECT [ID], [FKCampaignID], [ClosureEng], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                else if (DocumentID == "3")
                {
                    query = "SELECT [ID], [FKCampaignID], [Options], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                if (DocumentID == "4")
                {
                    query = "SELECT [ID], [FKCampaignID], [IncentiveStructure], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                if (DocumentID == "5")
                {
                    query = "SELECT [ID], [FKCampaignID], [Objectionhandling], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                if (DocumentID == "6")
                {
                    query = "SELECT [ID], [FKCampaignID], [NeedCreation], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID";
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  INMySuccessCampaignDetails data.
        /// </summary>
        /// <param name="iNMySuccessCampaignDetails">The INMySuccessCampaignDetails to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  INMySuccessCampaignDetails data.</returns>
        internal static string FillData(INMySuccessCampaignDetails iNMySuccessCampaignDetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (iNMySuccessCampaignDetails != null)
            {
                if (DocumentID == "1")
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ScriptEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                else if (DocumentID == "2")
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ClosureEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                else if (DocumentID == "3")
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Options], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                else if (DocumentID == "4")
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[IncentiveStructure], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                else if (DocumentID == "5")
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Objectionhandling], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
                else if (DocumentID == "6")
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[NeedCreation], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                }
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an INMySuccessCampaignDetails object from history.
        /// </summary>
        /// <param name="iNMySuccessCampaignDetails">The INMySuccessCampaignDetails object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the INMySuccessCampaignDetails object from history.</returns>
        internal static string FillHistory(INMySuccessCampaignDetails iNMySuccessCampaignDetails, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (iNMySuccessCampaignDetails != null)
            {
                if (DocumentID == "1")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [ScriptEng], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampUserID] = @StampUserID AND [INMySuccessCampaignDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "2")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [ClosureEng], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampUserID] = @StampUserID AND [INMySuccessCampaignDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "3")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [Options], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampUserID] = @StampUserID AND [INMySuccessCampaignDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "4")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [IncentiveStructure], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampUserID] = @StampUserID AND [INMySuccessCampaignDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "5")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [Objectionhandling], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampUserID] = @StampUserID AND [INMySuccessCampaignDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
                else if (DocumentID == "6")
                {
                    query = "SELECT [ID], [FKCampaignID], [ID], [NeedCreation], [StampDate], [StampUserID] FROM [INMySuccessCampaignDetails] WHERE [INMySuccessCampaignDetails].[ID] = @ID AND [INMySuccessCampaignDetails].[StampUserID] = @StampUserID AND [INMySuccessCampaignDetails].[StampDate] = @StampDate";
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                    parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                    parameters[2] = Database.GetParameter("@StampDate", stampDate);
                }
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the INMySuccessCampaignDetailss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the INMySuccessCampaignDetailss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (DocumentID == "1")
            {
                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ScriptEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "2")
            {
                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ClosureEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "3")
            {
                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Options], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "4")
            {   query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[IncentiveStructure], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "5")
            {
                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Objectionhandling], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "6")
            {
                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[NeedCreation], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }

            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted INMySuccessCampaignDetailss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted INMySuccessCampaignDetailss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[FKLanguageID], [INMySuccessCampaignDetails].[Document], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMySuccessCampaignDetails] ");
            query.Append("INNER JOIN (SELECT [INMySuccessCampaignDetails].[ID], MAX([INMySuccessCampaignDetails].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [INMySuccessCampaignDetails] ");
            query.Append("WHERE [INMySuccessCampaignDetails].[ID] NOT IN (SELECT [INMySuccessCampaignDetails].[ID] FROM [INMySuccessCampaignDetails]) ");
            query.Append("GROUP BY [INMySuccessCampaignDetails].[ID]) AS [LastHistory] ");
            query.Append("ON [INMySuccessCampaignDetails].[ID] = [LastHistory].[ID] ");
            query.Append("AND [INMySuccessCampaignDetails].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) INMySuccessCampaignDetails in the database.
        /// </summary>
        /// <param name="INMySuccessCampaignDetails">The INMySuccessCampaignDetails object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) INMySuccessCampaignDetails in the database.</returns>
        public static string ListHistory(INMySuccessCampaignDetails iNMySuccessCampaignDetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            string DocumentID = GlobalSettings.CampaignNotesID;

            if (DocumentID == "1")
            {
                if (iNMySuccessCampaignDetails != null)
                {

                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ScriptEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessCampaignDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);

                }
             
            }
            else if (DocumentID == "2") 
            {
                if (iNMySuccessCampaignDetails != null)
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ClosureEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessCampaignDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);

                }
            }
            else if (DocumentID == "3")
            {
                if (iNMySuccessCampaignDetails != null)
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Options], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessCampaignDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);

                }
            }
            else if (DocumentID == "4")
            {
                if (iNMySuccessCampaignDetails != null)
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[IncentiveStructure], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessCampaignDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);

                }
            }
            else if (DocumentID == "5")
            {
                if (iNMySuccessCampaignDetails != null)
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Objectionhandling], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessCampaignDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);

                }
            }
            else if (DocumentID == "6")
            {
                if (iNMySuccessCampaignDetails != null)
                {
                    query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[NeedCreation], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                    query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                    query.Append(" FROM [INMySuccessCampaignDetails] ");
                    query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                    query.Append(" ORDER BY [INMySuccessCampaignDetails].[StampDate] DESC");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);

                }
                
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) INMySuccessCampaignDetails to the database.
        /// </summary>
        /// <param name="INMySuccessCampaignDetails">The INMySuccessCampaignDetails to save.</param>
        /// <returns>A query that can be used to save the INMySuccessCampaignDetails to the database.</returns>
        internal static string Save(INMySuccessCampaignDetails iNMySuccessCampaignDetails, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();

            string DocumentID = GlobalSettings.CampaignNotesID;
            string LanguageID = GlobalSettings.LanguageNotesID;
            string ColumnID = GlobalSettings.ColumnIDMySuccessID; 

            if (iNMySuccessCampaignDetails != null)
            {
                if (DocumentID == "1")
                {


                    if (LanguageID == "1")
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [ScriptAfr], [StampDate], [StampUserID]) SELECT [FKCampaignID], [ScriptAfr], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [ScriptAfr] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = " + ColumnID);
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [ScriptAfr], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }
                    else if (LanguageID == "2 ")
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [ScriptEng], [StampDate], [StampUserID]) SELECT [FKCampaignID], [ScriptEng], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [ScriptEng] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = " + ColumnID);
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [ScriptEng], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }
                }
                else if (DocumentID == "2") 
                {

                    if (LanguageID == "1")
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [ClosureAfr], [StampDate], [StampUserID]) SELECT [FKCampaignID], [ClosureAfr], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [ClosureAfr] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [ClosureAfr], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }
                    else if (LanguageID == "2") 
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [ClosureEng], [StampDate], [StampUserID]) SELECT [FKCampaignID], [ClosureEng], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [ClosureEng] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [ClosureEng], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }

                    
                }
                else if (DocumentID == "3")
                {

                    if (LanguageID == "1")
                    {

                        try { } catch { }
                    }

                    else if (LanguageID == "2")
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [Options], [StampDate], [StampUserID]) SELECT [FKCampaignID], [Options], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [Options] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [Options], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }
                }
                else if (DocumentID == "4")
                {

                    if (LanguageID == "1")
                    {

                        try { } catch { }
                    }
                    else if (LanguageID == "2")
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [IncentiveStructure], [StampDate], [StampUserID]) SELECT [FKCampaignID], [IncentiveStructure], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [IncentiveStructure] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [IncentiveStructure], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }
                }
                else if (DocumentID == "5")
                {

                    if (LanguageID == "1")
                    {

                        try { } catch { }
                    }
                    else if (LanguageID == "2")
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [Objectionhandling], [StampDate], [StampUserID]) SELECT [FKCampaignID], [Objectionhandling], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [Objectionhandling] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [Objectionhandling], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }
                }
                else if (DocumentID == "6")
                {

                    if (LanguageID == "1")
                    {

                        try { } catch { }
                    }
                    else if (LanguageID == "2")
                    {
                        if (iNMySuccessCampaignDetails.IsLoaded)
                        {

                            query.Append("INSERT INTO [zHstINMySuccessCampaignDetails] ([FKCampaignID], [NeedCreation], [StampDate], [StampUserID]) SELECT [FKCampaignID], [NeedCreation], [StampDate], [StampUserID] FROM [zHstINMySuccessCampaignDetails] WHERE [zHstINMySuccessCampaignDetails].[ID] = @ID; ");
                            query.Append("UPDATE [INMySuccessCampaignDetails]");
                            parameters = new object[3];
                            query.Append(" SET [FKCampaignID] = @FKCampaignID");
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            query.Append(", [NeedCreation] = @Document");
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                            query.Append(" WHERE [INMySuccessCampaignDetails].[ID] = @ID");
                            parameters[2] = Database.GetParameter("@ID", iNMySuccessCampaignDetails.ID);
                        }
                        else
                        {
                            query.Append("INSERT INTO [INMySuccessCampaignDetails] ([FKCampaignID], [NeedCreation], [StampDate], [StampUserID]) VALUES(@FKCampaignID, @Document, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                            parameters = new object[2];
                            parameters[0] = Database.GetParameter("@FKCampaignID", iNMySuccessCampaignDetails.FKCampaignID.HasValue ? (object)iNMySuccessCampaignDetails.FKCampaignID.Value : DBNull.Value);
                            parameters[1] = Database.GetParameter("@Document", iNMySuccessCampaignDetails.Document);
                            query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                        }
                    }
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for INMySuccessCampaignDetailss that match the search criteria.
        /// </summary>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="document">The document search criteria.</param>
        /// <returns>A query that can be used to search for INMySuccessCampaignDetailss based on the search criteria.</returns>
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
                    whereQuery.Append("[INMySuccessCampaignDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[ScriptEng] = " + document + "");
                }

                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ScriptEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "2")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[ClosureEng] = " + document + "");
                }

                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[ClosureEng], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "3")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[Options] = " + document + "");
                }

                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Options], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "4")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[IncentiveStructure] = " + document + "");
                }

                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[IncentiveStructure], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "5")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[Objectionhandling] = " + document + "");
                }

                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[Objectionhandling], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
            }
            else if (DocumentID == "6")
            {
                if (fkcampaignid != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[FKCampaignID] = " + fkcampaignid + "");
                }
                if (document != null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("[INMySuccessCampaignDetails].[NeedCreation] = " + document + "");
                }

                query.Append("SELECT [INMySuccessCampaignDetails].[ID], [INMySuccessCampaignDetails].[FKCampaignID], [INMySuccessCampaignDetails].[NeedCreation], [INMySuccessCampaignDetails].[StampDate], [INMySuccessCampaignDetails].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMySuccessCampaignDetails].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [INMySuccessCampaignDetails] ");
                
            }

            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}