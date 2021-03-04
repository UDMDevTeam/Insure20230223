using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to ingiftoption objects.
    /// </summary>
    internal abstract partial class INGiftOptionQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) ingiftoption from the database.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption object to delete.</param>
        /// <returns>A query that can be used to delete the ingiftoption from the database.</returns>
        internal static string Delete(INGiftOption ingiftoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (ingiftoption != null)
            {
                query = "INSERT INTO [zHstINGiftOption] ([ID], [Gift], [ActiveStartDate], [ActiveEndDate]) SELECT [ID], [Gift], [ActiveStartDate], [ActiveEndDate] FROM [INGiftOption] WHERE [INGiftOption].[ID] = @ID; ";
                query += "DELETE FROM [INGiftOption] WHERE [INGiftOption].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", ingiftoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) ingiftoption from the database.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the ingiftoption from the database.</returns>
        internal static string DeleteHistory(INGiftOption ingiftoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (ingiftoption != null)
            {
                query = "DELETE FROM [zHstINGiftOption] WHERE [zHstINGiftOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", ingiftoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) ingiftoption from the database.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption object to undelete.</param>
        /// <returns>A query that can be used to undelete the ingiftoption from the database.</returns>
        internal static string UnDelete(INGiftOption ingiftoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (ingiftoption != null)
            {
                query = "INSERT INTO [INGiftOption] ([ID], [Gift], [ActiveStartDate], [ActiveEndDate]) SELECT [ID], [Gift], [ActiveStartDate], [ActiveEndDate] FROM [zHstINGiftOption] WHERE [zHstINGiftOption].[ID] = @ID AND [zHstINGiftOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINGiftOption] WHERE [zHstINGiftOption].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INGiftOption] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINGiftOption] WHERE [zHstINGiftOption].[ID] = @ID AND [zHstINGiftOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINGiftOption] WHERE [zHstINGiftOption].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INGiftOption] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", ingiftoption.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an ingiftoption object.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption object to fill.</param>
        /// <returns>A query that can be used to fill the ingiftoption object.</returns>
        internal static string Fill(INGiftOption ingiftoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (ingiftoption != null)
            {
                query = "SELECT [ID], [Gift], [ActiveStartDate], [ActiveEndDate] FROM [INGiftOption] WHERE [INGiftOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", ingiftoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  ingiftoption data.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  ingiftoption data.</returns>
        internal static string FillData(INGiftOption ingiftoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (ingiftoption != null)
            {
            query.Append("SELECT [INGiftOption].[ID], [INGiftOption].[Gift], [INGiftOption].[ActiveStartDate], [INGiftOption].[ActiveEndDate]");
            query.Append(" FROM [INGiftOption] ");
                query.Append(" WHERE [INGiftOption].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", ingiftoption.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an ingiftoption object from history.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the ingiftoption object from history.</returns>
        internal static string FillHistory(INGiftOption ingiftoption, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (ingiftoption != null)
            {
                query = "SELECT [ID], [Gift], [ActiveStartDate], [ActiveEndDate] FROM [zHstINGiftOption] WHERE [zHstINGiftOption].[ID] = @ID AND [zHstINGiftOption].[StampUserID] = @StampUserID AND [zHstINGiftOption].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", ingiftoption.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the ingiftoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the ingiftoptions in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INGiftOption].[ID], [INGiftOption].[Gift], [INGiftOption].[ActiveStartDate], [INGiftOption].[ActiveEndDate]");
            query.Append(" FROM [INGiftOption] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted ingiftoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted ingiftoptions in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINGiftOption].[ID], [zHstINGiftOption].[Gift], [zHstINGiftOption].[ActiveStartDate], [zHstINGiftOption].[ActiveEndDate]");
            query.Append(" FROM [zHstINGiftOption] ");
            query.Append("INNER JOIN (SELECT [zHstINGiftOption].[ID], MAX([zHstINGiftOption].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINGiftOption] ");
            query.Append("WHERE [zHstINGiftOption].[ID] NOT IN (SELECT [INGiftOption].[ID] FROM [INGiftOption]) ");
            query.Append("GROUP BY [zHstINGiftOption].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINGiftOption].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINGiftOption].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) ingiftoption in the database.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) ingiftoption in the database.</returns>
        public static string ListHistory(INGiftOption ingiftoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (ingiftoption != null)
            {
            query.Append("SELECT [zHstINGiftOption].[ID], [zHstINGiftOption].[Gift], [zHstINGiftOption].[ActiveStartDate], [zHstINGiftOption].[ActiveEndDate]");
            query.Append(" FROM [zHstINGiftOption] ");
                query.Append(" WHERE [zHstINGiftOption].[ID] = @ID");
                query.Append(" ORDER BY [zHstINGiftOption].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", ingiftoption.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) ingiftoption to the database.
        /// </summary>
        /// <param name="ingiftoption">The ingiftoption to save.</param>
        /// <returns>A query that can be used to save the ingiftoption to the database.</returns>
        internal static string Save(INGiftOption ingiftoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (ingiftoption != null)
            {
                if (ingiftoption.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINGiftOption] ([ID], [Gift], [ActiveStartDate], [ActiveEndDate]) SELECT [ID], [Gift], [ActiveStartDate], [ActiveEndDate] FROM [INGiftOption] WHERE [INGiftOption].[ID] = @ID; ");
                    query.Append("UPDATE [INGiftOption]");
                    parameters = new object[4];
                    query.Append(" SET [Gift] = @Gift");
                    parameters[0] = Database.GetParameter("@Gift", string.IsNullOrEmpty(ingiftoption.Gift) ? DBNull.Value : (object)ingiftoption.Gift);
                    query.Append(", [ActiveStartDate] = @ActiveStartDate");
                    parameters[1] = Database.GetParameter("@ActiveStartDate", ingiftoption.ActiveStartDate.HasValue ? (object)ingiftoption.ActiveStartDate.Value : DBNull.Value);
                    query.Append(", [ActiveEndDate] = @ActiveEndDate");
                    parameters[2] = Database.GetParameter("@ActiveEndDate", ingiftoption.ActiveEndDate.HasValue ? (object)ingiftoption.ActiveEndDate.Value : DBNull.Value);
                    query.Append(" WHERE [INGiftOption].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", ingiftoption.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INGiftOption] ([Gift], [ActiveStartDate], [ActiveEndDate]) VALUES(@Gift, @ActiveStartDate, @ActiveEndDate);");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@Gift", string.IsNullOrEmpty(ingiftoption.Gift) ? DBNull.Value : (object)ingiftoption.Gift);
                    parameters[1] = Database.GetParameter("@ActiveStartDate", ingiftoption.ActiveStartDate.HasValue ? (object)ingiftoption.ActiveStartDate.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@ActiveEndDate", ingiftoption.ActiveEndDate.HasValue ? (object)ingiftoption.ActiveEndDate.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for ingiftoptions that match the search criteria.
        /// </summary>
        /// <param name="gift">The gift search criteria.</param>
        /// <param name="activestartdate">The activestartdate search criteria.</param>
        /// <param name="activeenddate">The activeenddate search criteria.</param>
        /// <returns>A query that can be used to search for ingiftoptions based on the search criteria.</returns>
        internal static string Search(string gift, DateTime? activestartdate, DateTime? activeenddate)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (gift != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INGiftOption].[Gift] LIKE '" + gift.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (activestartdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INGiftOption].[ActiveStartDate] = '" + activestartdate.Value.ToString(Database.DateFormat) + "'");
            }
            if (activeenddate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INGiftOption].[ActiveEndDate] = '" + activeenddate.Value.ToString(Database.DateFormat) + "'");
            }
            query.Append("SELECT [INGiftOption].[ID], [INGiftOption].[Gift], [INGiftOption].[ActiveStartDate], [INGiftOption].[ActiveEndDate]");
            query.Append(" FROM [INGiftOption] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
