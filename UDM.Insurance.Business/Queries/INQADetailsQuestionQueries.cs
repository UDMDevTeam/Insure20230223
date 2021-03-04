using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inqadetailsquestion objects.
    /// </summary>
    internal abstract partial class INQADetailsQuestionQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inqadetailsquestion from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion object to delete.</param>
        /// <returns>A query that can be used to delete the inqadetailsquestion from the database.</returns>
        internal static string Delete(INQADetailsQuestion inqadetailsquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestion != null)
            {
                query = "INSERT INTO [zHstINQADetailsQuestion] ([ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID] FROM [INQADetailsQuestion] WHERE [INQADetailsQuestion].[ID] = @ID; ";
                query += "DELETE FROM [INQADetailsQuestion] WHERE [INQADetailsQuestion].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestion.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inqadetailsquestion from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inqadetailsquestion from the database.</returns>
        internal static string DeleteHistory(INQADetailsQuestion inqadetailsquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestion != null)
            {
                query = "DELETE FROM [zHstINQADetailsQuestion] WHERE [zHstINQADetailsQuestion].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestion.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inqadetailsquestion from the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion object to undelete.</param>
        /// <returns>A query that can be used to undelete the inqadetailsquestion from the database.</returns>
        internal static string UnDelete(INQADetailsQuestion inqadetailsquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestion != null)
            {
                query = "INSERT INTO [INQADetailsQuestion] ([ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID] FROM [zHstINQADetailsQuestion] WHERE [zHstINQADetailsQuestion].[ID] = @ID AND [zHstINQADetailsQuestion].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsQuestion] WHERE [zHstINQADetailsQuestion].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INQADetailsQuestion] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINQADetailsQuestion] WHERE [zHstINQADetailsQuestion].[ID] = @ID AND [zHstINQADetailsQuestion].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsQuestion] WHERE [zHstINQADetailsQuestion].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INQADetailsQuestion] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestion.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsquestion object.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion object to fill.</param>
        /// <returns>A query that can be used to fill the inqadetailsquestion object.</returns>
        internal static string Fill(INQADetailsQuestion inqadetailsquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestion != null)
            {
                query = "SELECT [ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID] FROM [INQADetailsQuestion] WHERE [INQADetailsQuestion].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestion.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inqadetailsquestion data.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inqadetailsquestion data.</returns>
        internal static string FillData(INQADetailsQuestion inqadetailsquestion, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestion != null)
            {
            query.Append("SELECT [INQADetailsQuestion].[ID], [INQADetailsQuestion].[Question], [INQADetailsQuestion].[FKQuestionTypeID], [INQADetailsQuestion].[FKAnswerTypeID], [INQADetailsQuestion].[FKCampaignTypeID], [INQADetailsQuestion].[FKCampaignGroupID], [INQADetailsQuestion].[FKCampaignTypeGroupID], [INQADetailsQuestion].[FKCampaignGroupTypeID], [INQADetailsQuestion].[FKCampaignID], [INQADetailsQuestion].[Weight], [INQADetailsQuestion].[Rank], [INQADetailsQuestion].[IsActive], [INQADetailsQuestion].[StampDate], [INQADetailsQuestion].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsQuestion] ");
                query.Append(" WHERE [INQADetailsQuestion].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestion.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsquestion object from history.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inqadetailsquestion object from history.</returns>
        internal static string FillHistory(INQADetailsQuestion inqadetailsquestion, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestion != null)
            {
                query = "SELECT [ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID] FROM [zHstINQADetailsQuestion] WHERE [zHstINQADetailsQuestion].[ID] = @ID AND [zHstINQADetailsQuestion].[StampUserID] = @StampUserID AND [zHstINQADetailsQuestion].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestion.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsquestions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inqadetailsquestions in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INQADetailsQuestion].[ID], [INQADetailsQuestion].[Question], [INQADetailsQuestion].[FKQuestionTypeID], [INQADetailsQuestion].[FKAnswerTypeID], [INQADetailsQuestion].[FKCampaignTypeID], [INQADetailsQuestion].[FKCampaignGroupID], [INQADetailsQuestion].[FKCampaignTypeGroupID], [INQADetailsQuestion].[FKCampaignGroupTypeID], [INQADetailsQuestion].[FKCampaignID], [INQADetailsQuestion].[Weight], [INQADetailsQuestion].[Rank], [INQADetailsQuestion].[IsActive], [INQADetailsQuestion].[StampDate], [INQADetailsQuestion].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsQuestion] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inqadetailsquestions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inqadetailsquestions in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINQADetailsQuestion].[ID], [zHstINQADetailsQuestion].[Question], [zHstINQADetailsQuestion].[FKQuestionTypeID], [zHstINQADetailsQuestion].[FKAnswerTypeID], [zHstINQADetailsQuestion].[FKCampaignTypeID], [zHstINQADetailsQuestion].[FKCampaignGroupID], [zHstINQADetailsQuestion].[FKCampaignTypeGroupID], [zHstINQADetailsQuestion].[FKCampaignGroupTypeID], [zHstINQADetailsQuestion].[FKCampaignID], [zHstINQADetailsQuestion].[Weight], [zHstINQADetailsQuestion].[Rank], [zHstINQADetailsQuestion].[IsActive], [zHstINQADetailsQuestion].[StampDate], [zHstINQADetailsQuestion].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINQADetailsQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINQADetailsQuestion] ");
            query.Append("INNER JOIN (SELECT [zHstINQADetailsQuestion].[ID], MAX([zHstINQADetailsQuestion].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINQADetailsQuestion] ");
            query.Append("WHERE [zHstINQADetailsQuestion].[ID] NOT IN (SELECT [INQADetailsQuestion].[ID] FROM [INQADetailsQuestion]) ");
            query.Append("GROUP BY [zHstINQADetailsQuestion].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINQADetailsQuestion].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINQADetailsQuestion].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsquestion in the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inqadetailsquestion in the database.</returns>
        public static string ListHistory(INQADetailsQuestion inqadetailsquestion, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestion != null)
            {
            query.Append("SELECT [zHstINQADetailsQuestion].[ID], [zHstINQADetailsQuestion].[Question], [zHstINQADetailsQuestion].[FKQuestionTypeID], [zHstINQADetailsQuestion].[FKAnswerTypeID], [zHstINQADetailsQuestion].[FKCampaignTypeID], [zHstINQADetailsQuestion].[FKCampaignGroupID], [zHstINQADetailsQuestion].[FKCampaignTypeGroupID], [zHstINQADetailsQuestion].[FKCampaignGroupTypeID], [zHstINQADetailsQuestion].[FKCampaignID], [zHstINQADetailsQuestion].[Weight], [zHstINQADetailsQuestion].[Rank], [zHstINQADetailsQuestion].[IsActive], [zHstINQADetailsQuestion].[StampDate], [zHstINQADetailsQuestion].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINQADetailsQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINQADetailsQuestion] ");
                query.Append(" WHERE [zHstINQADetailsQuestion].[ID] = @ID");
                query.Append(" ORDER BY [zHstINQADetailsQuestion].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestion.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inqadetailsquestion to the database.
        /// </summary>
        /// <param name="inqadetailsquestion">The inqadetailsquestion to save.</param>
        /// <returns>A query that can be used to save the inqadetailsquestion to the database.</returns>
        internal static string Save(INQADetailsQuestion inqadetailsquestion, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestion != null)
            {
                if (inqadetailsquestion.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINQADetailsQuestion] ([ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID] FROM [INQADetailsQuestion] WHERE [INQADetailsQuestion].[ID] = @ID; ");
                    query.Append("UPDATE [INQADetailsQuestion]");
                    parameters = new object[12];
                    query.Append(" SET [Question] = @Question");
                    parameters[0] = Database.GetParameter("@Question", string.IsNullOrEmpty(inqadetailsquestion.Question) ? DBNull.Value : (object)inqadetailsquestion.Question);
                    query.Append(", [FKQuestionTypeID] = @FKQuestionTypeID");
                    parameters[1] = Database.GetParameter("@FKQuestionTypeID", inqadetailsquestion.FKQuestionTypeID.HasValue ? (object)inqadetailsquestion.FKQuestionTypeID.Value : DBNull.Value);
                    query.Append(", [FKAnswerTypeID] = @FKAnswerTypeID");
                    parameters[2] = Database.GetParameter("@FKAnswerTypeID", inqadetailsquestion.FKAnswerTypeID.HasValue ? (object)inqadetailsquestion.FKAnswerTypeID.Value : DBNull.Value);
                    query.Append(", [FKCampaignTypeID] = @FKCampaignTypeID");
                    parameters[3] = Database.GetParameter("@FKCampaignTypeID", inqadetailsquestion.FKCampaignTypeID.HasValue ? (object)inqadetailsquestion.FKCampaignTypeID.Value : DBNull.Value);
                    query.Append(", [FKCampaignGroupID] = @FKCampaignGroupID");
                    parameters[4] = Database.GetParameter("@FKCampaignGroupID", inqadetailsquestion.FKCampaignGroupID.HasValue ? (object)inqadetailsquestion.FKCampaignGroupID.Value : DBNull.Value);
                    query.Append(", [FKCampaignTypeGroupID] = @FKCampaignTypeGroupID");
                    parameters[5] = Database.GetParameter("@FKCampaignTypeGroupID", inqadetailsquestion.FKCampaignTypeGroupID.HasValue ? (object)inqadetailsquestion.FKCampaignTypeGroupID.Value : DBNull.Value);
                    query.Append(", [FKCampaignGroupTypeID] = @FKCampaignGroupTypeID");
                    parameters[6] = Database.GetParameter("@FKCampaignGroupTypeID", inqadetailsquestion.FKCampaignGroupTypeID.HasValue ? (object)inqadetailsquestion.FKCampaignGroupTypeID.Value : DBNull.Value);
                    query.Append(", [FKCampaignID] = @FKCampaignID");
                    parameters[7] = Database.GetParameter("@FKCampaignID", inqadetailsquestion.FKCampaignID.HasValue ? (object)inqadetailsquestion.FKCampaignID.Value : DBNull.Value);
                    query.Append(", [Weight] = @Weight");
                    parameters[8] = Database.GetParameter("@Weight", inqadetailsquestion.Weight.HasValue ? (object)inqadetailsquestion.Weight.Value : DBNull.Value);
                    query.Append(", [Rank] = @Rank");
                    parameters[9] = Database.GetParameter("@Rank", inqadetailsquestion.Rank.HasValue ? (object)inqadetailsquestion.Rank.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[10] = Database.GetParameter("@IsActive", inqadetailsquestion.IsActive.HasValue ? (object)inqadetailsquestion.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INQADetailsQuestion].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", inqadetailsquestion.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INQADetailsQuestion] ([Question], [FKQuestionTypeID], [FKAnswerTypeID], [FKCampaignTypeID], [FKCampaignGroupID], [FKCampaignTypeGroupID], [FKCampaignGroupTypeID], [FKCampaignID], [Weight], [Rank], [IsActive], [StampDate], [StampUserID]) VALUES(@Question, @FKQuestionTypeID, @FKAnswerTypeID, @FKCampaignTypeID, @FKCampaignGroupID, @FKCampaignTypeGroupID, @FKCampaignGroupTypeID, @FKCampaignID, @Weight, @Rank, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@Question", string.IsNullOrEmpty(inqadetailsquestion.Question) ? DBNull.Value : (object)inqadetailsquestion.Question);
                    parameters[1] = Database.GetParameter("@FKQuestionTypeID", inqadetailsquestion.FKQuestionTypeID.HasValue ? (object)inqadetailsquestion.FKQuestionTypeID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKAnswerTypeID", inqadetailsquestion.FKAnswerTypeID.HasValue ? (object)inqadetailsquestion.FKAnswerTypeID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKCampaignTypeID", inqadetailsquestion.FKCampaignTypeID.HasValue ? (object)inqadetailsquestion.FKCampaignTypeID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@FKCampaignGroupID", inqadetailsquestion.FKCampaignGroupID.HasValue ? (object)inqadetailsquestion.FKCampaignGroupID.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@FKCampaignTypeGroupID", inqadetailsquestion.FKCampaignTypeGroupID.HasValue ? (object)inqadetailsquestion.FKCampaignTypeGroupID.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@FKCampaignGroupTypeID", inqadetailsquestion.FKCampaignGroupTypeID.HasValue ? (object)inqadetailsquestion.FKCampaignGroupTypeID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@FKCampaignID", inqadetailsquestion.FKCampaignID.HasValue ? (object)inqadetailsquestion.FKCampaignID.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@Weight", inqadetailsquestion.Weight.HasValue ? (object)inqadetailsquestion.Weight.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@Rank", inqadetailsquestion.Rank.HasValue ? (object)inqadetailsquestion.Rank.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@IsActive", inqadetailsquestion.IsActive.HasValue ? (object)inqadetailsquestion.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsquestions that match the search criteria.
        /// </summary>
        /// <param name="question">The question search criteria.</param>
        /// <param name="fkquestiontypeid">The fkquestiontypeid search criteria.</param>
        /// <param name="fkanswertypeid">The fkanswertypeid search criteria.</param>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigngroupid">The fkcampaigngroupid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <param name="fkcampaigngrouptypeid">The fkcampaigngrouptypeid search criteria.</param>
        /// <param name="fkcampaignid">The fkcampaignid search criteria.</param>
        /// <param name="weight">The weight search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for inqadetailsquestions based on the search criteria.</returns>
        internal static string Search(string question, long? fkquestiontypeid, long? fkanswertypeid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fkcampaignid, int? weight, int? rank, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (question != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[Question] LIKE '" + question.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkquestiontypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[FKQuestionTypeID] = " + fkquestiontypeid + "");
            }
            if (fkanswertypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[FKAnswerTypeID] = " + fkanswertypeid + "");
            }
            if (fkcampaigntypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[FKCampaignTypeID] = " + fkcampaigntypeid + "");
            }
            if (fkcampaigngroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[FKCampaignGroupID] = " + fkcampaigngroupid + "");
            }
            if (fkcampaigntypegroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[FKCampaignTypeGroupID] = " + fkcampaigntypegroupid + "");
            }
            if (fkcampaigngrouptypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[FKCampaignGroupTypeID] = " + fkcampaigngrouptypeid + "");
            }
            if (fkcampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[FKCampaignID] = " + fkcampaignid + "");
            }
            if (weight != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[Weight] = " + weight + "");
            }
            if (rank != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[Rank] = " + rank + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestion].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INQADetailsQuestion].[ID], [INQADetailsQuestion].[Question], [INQADetailsQuestion].[FKQuestionTypeID], [INQADetailsQuestion].[FKAnswerTypeID], [INQADetailsQuestion].[FKCampaignTypeID], [INQADetailsQuestion].[FKCampaignGroupID], [INQADetailsQuestion].[FKCampaignTypeGroupID], [INQADetailsQuestion].[FKCampaignGroupTypeID], [INQADetailsQuestion].[FKCampaignID], [INQADetailsQuestion].[Weight], [INQADetailsQuestion].[Rank], [INQADetailsQuestion].[IsActive], [INQADetailsQuestion].[StampDate], [INQADetailsQuestion].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsQuestion] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
