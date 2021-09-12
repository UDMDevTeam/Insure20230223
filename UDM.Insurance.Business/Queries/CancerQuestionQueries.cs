using System;
using System.Text;

using UDM.Insurance.Business;
using UDM.Insurance.Business.Queries;
using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using UDM.Insurance.Business.Objects;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to cancerquestion objects.
    /// </summary>
    internal abstract partial class CancerQuestionQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) cancerquestion from the database.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion object to delete.</param>
        /// <returns>A query that can be used to delete the cancerquestion from the database.</returns>
        internal static string Delete(CancerQuestion cancerquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (cancerquestion != null)
            {
                query = "INSERT INTO [zHstCancerQuestion] ([ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate]) SELECT [ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate] FROM [CancerQuestion] WHERE [CancerQuestion].[ID] = @ID; ";
                query += "DELETE FROM [CancerQuestion] WHERE [CancerQuestion].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", cancerquestion.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) cancerquestion from the database.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the cancerquestion from the database.</returns>
        internal static string DeleteHistory(CancerQuestion cancerquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (cancerquestion != null)
            {
                query = "DELETE FROM [zHstCancerQuestion] WHERE [zHstCancerQuestion].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", cancerquestion.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) cancerquestion from the database.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion object to undelete.</param>
        /// <returns>A query that can be used to undelete the cancerquestion from the database.</returns>
        internal static string UnDelete(CancerQuestion cancerquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (cancerquestion != null)
            {
                query = "INSERT INTO [CancerQuestion] ([ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate]) SELECT [ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate] FROM [zHstCancerQuestion] WHERE [zHstCancerQuestion].[ID] = @ID AND [zHstCancerQuestion].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCancerQuestion] WHERE [zHstCancerQuestion].[ID] = @ID) AND (SELECT COUNT(ID) FROM [CancerQuestion] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstCancerQuestion] WHERE [zHstCancerQuestion].[ID] = @ID AND [zHstCancerQuestion].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstCancerQuestion] WHERE [zHstCancerQuestion].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [CancerQuestion] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", cancerquestion.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an cancerquestion object.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion object to fill.</param>
        /// <returns>A query that can be used to fill the cancerquestion object.</returns>
        internal static string Fill(CancerQuestion cancerquestion, ref object[] parameters)
        {
            string query = string.Empty;
            if (cancerquestion != null)
            {
                query = "SELECT [ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate] FROM [CancerQuestion] WHERE [CancerQuestion].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", cancerquestion.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  cancerquestion data.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  cancerquestion data.</returns>
        internal static string FillData(CancerQuestion cancerquestion, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (cancerquestion != null)
            {
                query.Append("SELECT [CancerQuestion].[ID], [CancerQuestion].[QuestionOne], [CancerQuestion].[QuestionTwo], [CancerQuestion].[FKINImportID], [CancerQuestion].[FKINCampaignID], [CancerQuestion].[StampUserID], [CancerQuestion].[StampDate]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CancerQuestion].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [CancerQuestion] ");
                query.Append(" WHERE [CancerQuestion].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", cancerquestion.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an cancerquestion object from history.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the cancerquestion object from history.</returns>
        internal static string FillHistory(CancerQuestion cancerquestion, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (cancerquestion != null)
            {
                query = "SELECT [ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate] FROM [zHstCancerQuestion] WHERE [zHstCancerQuestion].[ID] = @ID AND [zHstCancerQuestion].[StampUserID] = @StampUserID AND [zHstCancerQuestion].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", cancerquestion.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the cancerquestion in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the cancerquestion in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [CancerQuestion].[ID], [CancerQuestion].[QuestionOne], [CancerQuestion].[QuestionTwo], [CancerQuestion].[FKINImportID], [CancerQuestion].[FKINCampaignID], [CancerQuestion].[StampUserID], [CancerQuestion].[StampDate]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CancerQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CancerQuestion] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted cancerquestion in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted cancerquestion in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstCancerQuestion].[ID], [zHstCancerQuestion].[QuestionOne], [zHstCancerQuestion].[QuestionTwo], [zHstCancerQuestion].[FKINImportID], [zHstCancerQuestion].[FKINCampaignID], [zHstCancerQuestion].[StampUserID], [zHstCancerQuestion].[StampDate]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCancerQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstCancerQuestion] ");
            query.Append("INNER JOIN (SELECT [zHstCancerQuestion].[ID], MAX([zHstCancerQuestion].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstCancerQuestion] ");
            query.Append("WHERE [zHstCancerQuestion].[ID] NOT IN (SELECT [CancerQuestion].[ID] FROM [CancerQuestion]) ");
            query.Append("GROUP BY [zHstCancerQuestion].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstCancerQuestion].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstCancerQuestion].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) incancerquestion in the database.
        /// </summary>
        /// <param name="cancerquestion">The incancerquestion object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) incancerquestion in the database.</returns>
        public static string ListHistory(CancerQuestion cancerquestion, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (cancerquestion != null)
            {
                query.Append("SELECT [zHstCancerQuestion].[ID], [zHstCancerQuestion].[QuestionOne], [zHstCancerQuestion].[QuestionTwo], [zHstCancerQuestion].[FKINImportID], [zHstCancerQuestion].[FKINCampaignID], [zHstCancerQuestion].[StampUserID], [zHstCancerQuestion].[StampDate]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstCancerQuestion].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [zHstCancerQuestion] ");
                query.Append(" WHERE [zHstCancerQuestion].[ID] = @ID");
                query.Append(" ORDER BY [zHstCancerQuestion].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", cancerquestion.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) cancerquestion to the database.
        /// </summary>
        /// <param name="cancerquestion">The cancerquestion to save.</param>
        /// <returns>A query that can be used to save the cancerquestion to the database.</returns>
        internal static string Save(CancerQuestion cancerquestion, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (cancerquestion != null)
            {
                if (cancerquestion.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstCancerQuestion] ([ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate]) SELECT [ID], [QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate] FROM [CancerQuestion] WHERE [CancerQuestion].[ID] = @ID; ");
                    query.Append("UPDATE [CancerQuestion]");
                    parameters = new object[5];
                    query.Append(" SET [QuestionOne] = @QuestionOne");
                    parameters[0] = Database.GetParameter("@QuestionOne", cancerquestion.QuestionOne.HasValue ? (object)cancerquestion.QuestionOne.Value : DBNull.Value);
                    query.Append(", [QuestionTwo] = @QuestionTwo");
                    parameters[1] = Database.GetParameter("@QuestionTwo", cancerquestion.QuestionTwo.HasValue ? (object)cancerquestion.QuestionTwo.Value : DBNull.Value);
                    query.Append(", [FKINImportID] = @FKINImportID");
                    parameters[2] = Database.GetParameter("@FKINImportID", cancerquestion.FKINImportID.HasValue ? (object)cancerquestion.FKINImportID.Value : DBNull.Value);
                    query.Append(", [FKINCampaignID] = @FKINCampaignID");
                    parameters[3] = Database.GetParameter("@FKINCampaignID", cancerquestion.FKINCampaignID.HasValue ? (object)cancerquestion.FKINCampaignID.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [CancerQuestion].[ID] = @ID");
                    parameters[4] = Database.GetParameter("@ID", cancerquestion.ID);
                }
                else
                {
                    query.Append("INSERT INTO [CancerQuestion] ([QuestionOne], [QuestionTwo], [FKINImportID], [FKINCampaignID], [StampUserID], [StampDate]) VALUES(@QuestionOne, @QuestionTwo, @FKINImportID, @FKINCampaignID, " + GlobalSettings.ApplicationUser.ID  + ", " + Database.CurrentDateTime + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@QuestionOne", cancerquestion.QuestionOne.HasValue ? (object)cancerquestion.QuestionOne.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@QuestionTwo", cancerquestion.QuestionTwo.HasValue ? (object)cancerquestion.QuestionTwo.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKINImportID", cancerquestion.FKINImportID.HasValue ? (object)cancerquestion.FKINImportID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKINCampaignID", cancerquestion.FKINCampaignID.HasValue ? (object)cancerquestion.FKINCampaignID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for cancerquestion that match the search criteria.
        /// </summary>
        /// <param name="questionone">The questionone search criteria.</param>
        /// <param name="questiontwo">The questiontwo search criteria.</param>
        /// <param name="fkinimportid">The fkinimportid search criteria.</param>
        /// <param name="fkincampaignid">The fkincampaignid search criteria.</param>
        /// <returns>A query that can be used to search for cancerquestion based on the search criteria.</returns>
        internal static string Search(bool? questionone, bool? questiontwo, long? fkinimportid, long? fkincampaignid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (questionone != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CancerQuestion].[QuestionOne] = " + ((bool)questionone ? "1" : "0"));
                //whereQuery.Append("[CancerQuestion].[QuestionOne] = " + questionone + "");
            }
            if (questiontwo != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CancerQuestion].[QuestionTwo] = " + ((bool)questiontwo ? "1" : "0"));
                //whereQuery.Append("[CancerQuestion].[QuestionTwo] = " + questiontwo + "");
            }
            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CancerQuestion].[FKINImportID] = " + fkinimportid + "");
            }
            if (fkincampaignid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[CancerQuestion].[FKINCampaignID] = " + fkincampaignid + "");
            }
            query.Append("SELECT [CancerQuestion].[ID], [CancerQuestion].[QuestionOne], [CancerQuestion].[QuestionTwo], [CancerQuestion].[FKINImportID], [CancerQuestion].[FKINCampaignID], [CancerQuestion].[StampUserID], [CancerQuestion].[StampDate]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [CancerQuestion].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [CancerQuestion] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
