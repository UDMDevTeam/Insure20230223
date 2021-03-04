using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inqadetailsquestioninimport objects.
    /// </summary>
    internal abstract partial class INQADetailsQuestionINImportQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inqadetailsquestioninimport from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport object to delete.</param>
        /// <returns>A query that can be used to delete the inqadetailsquestioninimport from the database.</returns>
        internal static string Delete(INQADetailsQuestionINImport inqadetailsquestioninimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestioninimport != null)
            {
                query = "INSERT INTO [zHstINQADetailsQuestionINImport] ([ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID] FROM [INQADetailsQuestionINImport] WHERE [INQADetailsQuestionINImport].[ID] = @ID; ";
                query += "DELETE FROM [INQADetailsQuestionINImport] WHERE [INQADetailsQuestionINImport].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inqadetailsquestioninimport from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inqadetailsquestioninimport from the database.</returns>
        internal static string DeleteHistory(INQADetailsQuestionINImport inqadetailsquestioninimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestioninimport != null)
            {
                query = "DELETE FROM [zHstINQADetailsQuestionINImport] WHERE [zHstINQADetailsQuestionINImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inqadetailsquestioninimport from the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport object to undelete.</param>
        /// <returns>A query that can be used to undelete the inqadetailsquestioninimport from the database.</returns>
        internal static string UnDelete(INQADetailsQuestionINImport inqadetailsquestioninimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestioninimport != null)
            {
                query = "INSERT INTO [INQADetailsQuestionINImport] ([ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID] FROM [zHstINQADetailsQuestionINImport] WHERE [zHstINQADetailsQuestionINImport].[ID] = @ID AND [zHstINQADetailsQuestionINImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsQuestionINImport] WHERE [zHstINQADetailsQuestionINImport].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INQADetailsQuestionINImport] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINQADetailsQuestionINImport] WHERE [zHstINQADetailsQuestionINImport].[ID] = @ID AND [zHstINQADetailsQuestionINImport].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsQuestionINImport] WHERE [zHstINQADetailsQuestionINImport].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INQADetailsQuestionINImport] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsquestioninimport object.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport object to fill.</param>
        /// <returns>A query that can be used to fill the inqadetailsquestioninimport object.</returns>
        internal static string Fill(INQADetailsQuestionINImport inqadetailsquestioninimport, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestioninimport != null)
            {
                query = "SELECT [ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID] FROM [INQADetailsQuestionINImport] WHERE [INQADetailsQuestionINImport].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inqadetailsquestioninimport data.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inqadetailsquestioninimport data.</returns>
        internal static string FillData(INQADetailsQuestionINImport inqadetailsquestioninimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestioninimport != null)
            {
            query.Append("SELECT [INQADetailsQuestionINImport].[ID], [INQADetailsQuestionINImport].[FKImportID], [INQADetailsQuestionINImport].[FKQuestionID], [INQADetailsQuestionINImport].[AnswerInt], [INQADetailsQuestionINImport].[AnswerDateTime], [INQADetailsQuestionINImport].[AnswerText], [INQADetailsQuestionINImport].[StampDate], [INQADetailsQuestionINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsQuestionINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsQuestionINImport] ");
                query.Append(" WHERE [INQADetailsQuestionINImport].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsquestioninimport object from history.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inqadetailsquestioninimport object from history.</returns>
        internal static string FillHistory(INQADetailsQuestionINImport inqadetailsquestioninimport, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestioninimport != null)
            {
                query = "SELECT [ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID] FROM [zHstINQADetailsQuestionINImport] WHERE [zHstINQADetailsQuestionINImport].[ID] = @ID AND [zHstINQADetailsQuestionINImport].[StampUserID] = @StampUserID AND [zHstINQADetailsQuestionINImport].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsquestioninimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inqadetailsquestioninimports in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INQADetailsQuestionINImport].[ID], [INQADetailsQuestionINImport].[FKImportID], [INQADetailsQuestionINImport].[FKQuestionID], [INQADetailsQuestionINImport].[AnswerInt], [INQADetailsQuestionINImport].[AnswerDateTime], [INQADetailsQuestionINImport].[AnswerText], [INQADetailsQuestionINImport].[StampDate], [INQADetailsQuestionINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsQuestionINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsQuestionINImport] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inqadetailsquestioninimports in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inqadetailsquestioninimports in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINQADetailsQuestionINImport].[ID], [zHstINQADetailsQuestionINImport].[FKImportID], [zHstINQADetailsQuestionINImport].[FKQuestionID], [zHstINQADetailsQuestionINImport].[AnswerInt], [zHstINQADetailsQuestionINImport].[AnswerDateTime], [zHstINQADetailsQuestionINImport].[AnswerText], [zHstINQADetailsQuestionINImport].[StampDate], [zHstINQADetailsQuestionINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINQADetailsQuestionINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINQADetailsQuestionINImport] ");
            query.Append("INNER JOIN (SELECT [zHstINQADetailsQuestionINImport].[ID], MAX([zHstINQADetailsQuestionINImport].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINQADetailsQuestionINImport] ");
            query.Append("WHERE [zHstINQADetailsQuestionINImport].[ID] NOT IN (SELECT [INQADetailsQuestionINImport].[ID] FROM [INQADetailsQuestionINImport]) ");
            query.Append("GROUP BY [zHstINQADetailsQuestionINImport].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINQADetailsQuestionINImport].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINQADetailsQuestionINImport].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsquestioninimport in the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inqadetailsquestioninimport in the database.</returns>
        public static string ListHistory(INQADetailsQuestionINImport inqadetailsquestioninimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestioninimport != null)
            {
            query.Append("SELECT [zHstINQADetailsQuestionINImport].[ID], [zHstINQADetailsQuestionINImport].[FKImportID], [zHstINQADetailsQuestionINImport].[FKQuestionID], [zHstINQADetailsQuestionINImport].[AnswerInt], [zHstINQADetailsQuestionINImport].[AnswerDateTime], [zHstINQADetailsQuestionINImport].[AnswerText], [zHstINQADetailsQuestionINImport].[StampDate], [zHstINQADetailsQuestionINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINQADetailsQuestionINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINQADetailsQuestionINImport] ");
                query.Append(" WHERE [zHstINQADetailsQuestionINImport].[ID] = @ID");
                query.Append(" ORDER BY [zHstINQADetailsQuestionINImport].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inqadetailsquestioninimport to the database.
        /// </summary>
        /// <param name="inqadetailsquestioninimport">The inqadetailsquestioninimport to save.</param>
        /// <returns>A query that can be used to save the inqadetailsquestioninimport to the database.</returns>
        internal static string Save(INQADetailsQuestionINImport inqadetailsquestioninimport, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestioninimport != null)
            {
                if (inqadetailsquestioninimport.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINQADetailsQuestionINImport] ([ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID] FROM [INQADetailsQuestionINImport] WHERE [INQADetailsQuestionINImport].[ID] = @ID; ");
                    query.Append("UPDATE [INQADetailsQuestionINImport]");
                    parameters = new object[6];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", inqadetailsquestioninimport.FKImportID.HasValue ? (object)inqadetailsquestioninimport.FKImportID.Value : DBNull.Value);
                    query.Append(", [FKQuestionID] = @FKQuestionID");
                    parameters[1] = Database.GetParameter("@FKQuestionID", inqadetailsquestioninimport.FKQuestionID.HasValue ? (object)inqadetailsquestioninimport.FKQuestionID.Value : DBNull.Value);
                    query.Append(", [AnswerInt] = @AnswerInt");
                    parameters[2] = Database.GetParameter("@AnswerInt", inqadetailsquestioninimport.AnswerInt.HasValue ? (object)inqadetailsquestioninimport.AnswerInt.Value : DBNull.Value);
                    query.Append(", [AnswerDateTime] = @AnswerDateTime");
                    parameters[3] = Database.GetParameter("@AnswerDateTime", inqadetailsquestioninimport.AnswerDateTime.HasValue ? (object)inqadetailsquestioninimport.AnswerDateTime.Value : DBNull.Value);
                    query.Append(", [AnswerText] = @AnswerText");
                    parameters[4] = Database.GetParameter("@AnswerText", string.IsNullOrEmpty(inqadetailsquestioninimport.AnswerText) ? DBNull.Value : (object)inqadetailsquestioninimport.AnswerText);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INQADetailsQuestionINImport].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", inqadetailsquestioninimport.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INQADetailsQuestionINImport] ([FKImportID], [FKQuestionID], [AnswerInt], [AnswerDateTime], [AnswerText], [StampDate], [StampUserID]) VALUES(@FKImportID, @FKQuestionID, @AnswerInt, @AnswerDateTime, @AnswerText, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@FKImportID", inqadetailsquestioninimport.FKImportID.HasValue ? (object)inqadetailsquestioninimport.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKQuestionID", inqadetailsquestioninimport.FKQuestionID.HasValue ? (object)inqadetailsquestioninimport.FKQuestionID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@AnswerInt", inqadetailsquestioninimport.AnswerInt.HasValue ? (object)inqadetailsquestioninimport.AnswerInt.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@AnswerDateTime", inqadetailsquestioninimport.AnswerDateTime.HasValue ? (object)inqadetailsquestioninimport.AnswerDateTime.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@AnswerText", string.IsNullOrEmpty(inqadetailsquestioninimport.AnswerText) ? DBNull.Value : (object)inqadetailsquestioninimport.AnswerText);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsquestioninimports that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkquestionid">The fkquestionid search criteria.</param>
        /// <param name="answerint">The answerint search criteria.</param>
        /// <param name="answerdatetime">The answerdatetime search criteria.</param>
        /// <param name="answertext">The answertext search criteria.</param>
        /// <returns>A query that can be used to search for inqadetailsquestioninimports based on the search criteria.</returns>
        internal static string Search(long? fkimportid, long? fkquestionid, long? answerint, DateTime? answerdatetime, string answertext)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionINImport].[FKImportID] = " + fkimportid + "");
            }
            if (fkquestionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionINImport].[FKQuestionID] = " + fkquestionid + "");
            }
            if (answerint != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionINImport].[AnswerInt] = " + answerint + "");
            }
            if (answerdatetime != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionINImport].[AnswerDateTime] = '" + answerdatetime.Value.ToString(Database.DateFormat) + "'");
            }
            if (answertext != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionINImport].[AnswerText] LIKE '" + answertext.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INQADetailsQuestionINImport].[ID], [INQADetailsQuestionINImport].[FKImportID], [INQADetailsQuestionINImport].[FKQuestionID], [INQADetailsQuestionINImport].[AnswerInt], [INQADetailsQuestionINImport].[AnswerDateTime], [INQADetailsQuestionINImport].[AnswerText], [INQADetailsQuestionINImport].[StampDate], [INQADetailsQuestionINImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsQuestionINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsQuestionINImport] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
