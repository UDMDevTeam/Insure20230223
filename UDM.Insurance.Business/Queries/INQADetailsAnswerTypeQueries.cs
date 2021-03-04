using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inqadetailsanswertype objects.
    /// </summary>
    internal abstract partial class INQADetailsAnswerTypeQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inqadetailsanswertype from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype object to delete.</param>
        /// <returns>A query that can be used to delete the inqadetailsanswertype from the database.</returns>
        internal static string Delete(INQADetailsAnswerType inqadetailsanswertype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsanswertype != null)
            {
                query = "INSERT INTO [zHstINQADetailsAnswerType] ([ID], [Description]) SELECT [ID], [Description] FROM [INQADetailsAnswerType] WHERE [INQADetailsAnswerType].[ID] = @ID; ";
                query += "DELETE FROM [INQADetailsAnswerType] WHERE [INQADetailsAnswerType].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inqadetailsanswertype from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inqadetailsanswertype from the database.</returns>
        internal static string DeleteHistory(INQADetailsAnswerType inqadetailsanswertype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsanswertype != null)
            {
                query = "DELETE FROM [zHstINQADetailsAnswerType] WHERE [zHstINQADetailsAnswerType].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inqadetailsanswertype from the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype object to undelete.</param>
        /// <returns>A query that can be used to undelete the inqadetailsanswertype from the database.</returns>
        internal static string UnDelete(INQADetailsAnswerType inqadetailsanswertype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsanswertype != null)
            {
                query = "INSERT INTO [INQADetailsAnswerType] ([ID], [Description]) SELECT [ID], [Description] FROM [zHstINQADetailsAnswerType] WHERE [zHstINQADetailsAnswerType].[ID] = @ID AND [zHstINQADetailsAnswerType].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsAnswerType] WHERE [zHstINQADetailsAnswerType].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INQADetailsAnswerType] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINQADetailsAnswerType] WHERE [zHstINQADetailsAnswerType].[ID] = @ID AND [zHstINQADetailsAnswerType].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINQADetailsAnswerType] WHERE [zHstINQADetailsAnswerType].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INQADetailsAnswerType] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsanswertype object.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype object to fill.</param>
        /// <returns>A query that can be used to fill the inqadetailsanswertype object.</returns>
        internal static string Fill(INQADetailsAnswerType inqadetailsanswertype, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsanswertype != null)
            {
                query = "SELECT [ID], [Description] FROM [INQADetailsAnswerType] WHERE [INQADetailsAnswerType].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inqadetailsanswertype data.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inqadetailsanswertype data.</returns>
        internal static string FillData(INQADetailsAnswerType inqadetailsanswertype, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsanswertype != null)
            {
            query.Append("SELECT [INQADetailsAnswerType].[ID], [INQADetailsAnswerType].[Description]");
            query.Append(" FROM [INQADetailsAnswerType] ");
                query.Append(" WHERE [INQADetailsAnswerType].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inqadetailsanswertype object from history.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inqadetailsanswertype object from history.</returns>
        internal static string FillHistory(INQADetailsAnswerType inqadetailsanswertype, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inqadetailsanswertype != null)
            {
                query = "SELECT [ID], [Description] FROM [zHstINQADetailsAnswerType] WHERE [zHstINQADetailsAnswerType].[ID] = @ID AND [zHstINQADetailsAnswerType].[StampUserID] = @StampUserID AND [zHstINQADetailsAnswerType].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inqadetailsanswertypes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inqadetailsanswertypes in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INQADetailsAnswerType].[ID], [INQADetailsAnswerType].[Description]");
            query.Append(" FROM [INQADetailsAnswerType] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inqadetailsanswertypes in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inqadetailsanswertypes in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINQADetailsAnswerType].[ID], [zHstINQADetailsAnswerType].[Description]");
            query.Append(" FROM [zHstINQADetailsAnswerType] ");
            query.Append("INNER JOIN (SELECT [zHstINQADetailsAnswerType].[ID], MAX([zHstINQADetailsAnswerType].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINQADetailsAnswerType] ");
            query.Append("WHERE [zHstINQADetailsAnswerType].[ID] NOT IN (SELECT [INQADetailsAnswerType].[ID] FROM [INQADetailsAnswerType]) ");
            query.Append("GROUP BY [zHstINQADetailsAnswerType].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINQADetailsAnswerType].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINQADetailsAnswerType].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inqadetailsanswertype in the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inqadetailsanswertype in the database.</returns>
        public static string ListHistory(INQADetailsAnswerType inqadetailsanswertype, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsanswertype != null)
            {
            query.Append("SELECT [zHstINQADetailsAnswerType].[ID], [zHstINQADetailsAnswerType].[Description]");
            query.Append(" FROM [zHstINQADetailsAnswerType] ");
                query.Append(" WHERE [zHstINQADetailsAnswerType].[ID] = @ID");
                query.Append(" ORDER BY [zHstINQADetailsAnswerType].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inqadetailsanswertype to the database.
        /// </summary>
        /// <param name="inqadetailsanswertype">The inqadetailsanswertype to save.</param>
        /// <returns>A query that can be used to save the inqadetailsanswertype to the database.</returns>
        internal static string Save(INQADetailsAnswerType inqadetailsanswertype, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inqadetailsanswertype != null)
            {
                if (inqadetailsanswertype.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINQADetailsAnswerType] ([ID], [Description]) SELECT [ID], [Description] FROM [INQADetailsAnswerType] WHERE [INQADetailsAnswerType].[ID] = @ID; ");
                    query.Append("UPDATE [INQADetailsAnswerType]");
                    parameters = new object[2];
                    query.Append(" SET [Description] = @Description");
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inqadetailsanswertype.Description) ? DBNull.Value : (object)inqadetailsanswertype.Description);
                    query.Append(" WHERE [INQADetailsAnswerType].[ID] = @ID"); 
                    parameters[1] = Database.GetParameter("@ID", inqadetailsanswertype.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INQADetailsAnswerType] ([Description]) VALUES(@Description);");
                    parameters = new object[1];
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inqadetailsanswertype.Description) ? DBNull.Value : (object)inqadetailsanswertype.Description);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inqadetailsanswertypes that match the search criteria.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <returns>A query that can be used to search for inqadetailsanswertypes based on the search criteria.</returns>
        internal static string Search(string description)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsAnswerType].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INQADetailsAnswerType].[ID], [INQADetailsAnswerType].[Description]");
            query.Append(" FROM [INQADetailsAnswerType] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
