using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inqadetailsquestiontype objects.
    /// </summary>
    internal abstract partial class INQADetailsQuestionTypeQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inqadetailsquestiontype from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype object to delete.</param>
        /// <returns>A query that can be used to delete the inqadetailsquestiontype from the database.</returns>
        internal static string Delete(INQADetailsQuestionType inqadetailsquestiontype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestiontype != null)
            {
                query = "INSERT INTO [zHstINQADetailsQuestionType] ([ID], [Description]) SELECT [ID], [Description] FROM [INQADetailsQuestionType] WHERE [INQADetailsQuestionType].[ID] = @ID; ";
                query += "DELETE FROM [INQADetailsQuestionType] WHERE [INQADetailsQuestionType].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inqadetailsquestiontype from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inqadetailsquestiontype from the database.</returns>
        internal static string DeleteHistory(INQADetailsQuestionType inqadetailsquestiontype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestiontype != null)
            {
                query = "DELETE FROM [zHstINQADetailsQuestionType] WHERE [zHstINQADetailsQuestionType].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inqadetailsquestiontype from the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype object to undelete.</param>
        /// <returns>A query that can be used to undelete the inqadetailsquestiontype from the database.</returns>
        internal static string UnDelete(INQADetailsQuestionType inqadetailsquestiontype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestiontype != null)
            {
                query = "INSERT INTO [INQADetailsQuestionType] ([ID], [Description]) SELECT [ID], [Description] FROM [zHstINQADetailsQuestionType] WHERE [zHstINQADetailsQuestionType].[ID] = @ID AND [zHstINQADetailsQuestionType].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsQuestionType] WHERE [zHstINQADetailsQuestionType].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INQADetailsQuestionType] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINQADetailsQuestionType] WHERE [zHstINQADetailsQuestionType].[ID] = @ID AND [zHstINQADetailsQuestionType].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsQuestionType] WHERE [zHstINQADetailsQuestionType].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INQADetailsQuestionType] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsquestiontype object.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype object to fill.</param>
        /// <returns>A query that can be used to fill the inqadetailsquestiontype object.</returns>
        internal static string Fill(INQADetailsQuestionType inqadetailsquestiontype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestiontype != null)
            {
                query = "SELECT [ID], [Description] FROM [INQADetailsQuestionType] WHERE [INQADetailsQuestionType].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inqadetailsquestiontype data.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inqadetailsquestiontype data.</returns>
        internal static string FillData(INQADetailsQuestionType inqadetailsquestiontype, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestiontype != null)
            {
            query.Append("SELECT [INQADetailsQuestionType].[ID], [INQADetailsQuestionType].[Description]");
            query.Append(" FROM [INQADetailsQuestionType] ");
                query.Append(" WHERE [INQADetailsQuestionType].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsquestiontype object from history.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inqadetailsquestiontype object from history.</returns>
        internal static string FillHistory(INQADetailsQuestionType inqadetailsquestiontype, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsquestiontype != null)
            {
                query = "SELECT [ID], [Description] FROM [zHstINQADetailsQuestionType] WHERE [zHstINQADetailsQuestionType].[ID] = @ID AND [zHstINQADetailsQuestionType].[StampUserID] = @StampUserID AND [zHstINQADetailsQuestionType].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsquestiontypes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inqadetailsquestiontypes in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INQADetailsQuestionType].[ID], [INQADetailsQuestionType].[Description]");
            query.Append(" FROM [INQADetailsQuestionType] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inqadetailsquestiontypes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inqadetailsquestiontypes in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINQADetailsQuestionType].[ID], [zHstINQADetailsQuestionType].[Description]");
            query.Append(" FROM [zHstINQADetailsQuestionType] ");
            query.Append("INNER JOIN (SELECT [zHstINQADetailsQuestionType].[ID], MAX([zHstINQADetailsQuestionType].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINQADetailsQuestionType] ");
            query.Append("WHERE [zHstINQADetailsQuestionType].[ID] NOT IN (SELECT [INQADetailsQuestionType].[ID] FROM [INQADetailsQuestionType]) ");
            query.Append("GROUP BY [zHstINQADetailsQuestionType].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINQADetailsQuestionType].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINQADetailsQuestionType].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsquestiontype in the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inqadetailsquestiontype in the database.</returns>
        public static string ListHistory(INQADetailsQuestionType inqadetailsquestiontype, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestiontype != null)
            {
            query.Append("SELECT [zHstINQADetailsQuestionType].[ID], [zHstINQADetailsQuestionType].[Description]");
            query.Append(" FROM [zHstINQADetailsQuestionType] ");
                query.Append(" WHERE [zHstINQADetailsQuestionType].[ID] = @ID");
                query.Append(" ORDER BY [zHstINQADetailsQuestionType].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inqadetailsquestiontype to the database.
        /// </summary>
        /// <param name="inqadetailsquestiontype">The inqadetailsquestiontype to save.</param>
        /// <returns>A query that can be used to save the inqadetailsquestiontype to the database.</returns>
        internal static string Save(INQADetailsQuestionType inqadetailsquestiontype, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsquestiontype != null)
            {
                if (inqadetailsquestiontype.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINQADetailsQuestionType] ([ID], [Description]) SELECT [ID], [Description] FROM [INQADetailsQuestionType] WHERE [INQADetailsQuestionType].[ID] = @ID; ");
                    query.Append("UPDATE [INQADetailsQuestionType]");
                    parameters = new object[2];
                    query.Append(" SET [Description] = @Description");
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inqadetailsquestiontype.Description) ? DBNull.Value : (object)inqadetailsquestiontype.Description);
                    query.Append(" WHERE [INQADetailsQuestionType].[ID] = @ID"); 
                    parameters[1] = Database.GetParameter("@ID", inqadetailsquestiontype.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INQADetailsQuestionType] ([Description]) VALUES(@Description);");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inqadetailsquestiontype.Description) ? DBNull.Value : (object)inqadetailsquestiontype.Description);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsquestiontypes that match the search criteria.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <returns>A query that can be used to search for inqadetailsquestiontypes based on the search criteria.</returns>
        internal static string Search(string description)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionType].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INQADetailsQuestionType].[ID], [INQADetailsQuestionType].[Description]");
            query.Append(" FROM [INQADetailsQuestionType] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
