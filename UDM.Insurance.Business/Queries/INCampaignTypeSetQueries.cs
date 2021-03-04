using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to incampaigntypeset objects.
    /// </summary>
    internal abstract partial class INCampaignTypeSetQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) incampaigntypeset from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset object to delete.</param>
        /// <returns>A query that can be used to delete the incampaigntypeset from the database.</returns>
        internal static string Delete(INCampaignTypeSet incampaigntypeset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigntypeset != null)
            {
                query = "INSERT INTO [zHstINCampaignTypeSet] ([ID], [FKCampaignTypeID], [FKCampaignTypeGroupID]) SELECT [ID], [FKCampaignTypeID], [FKCampaignTypeGroupID] FROM [INCampaignTypeSet] WHERE [INCampaignTypeSet].[ID] = @ID; ";
                query += "DELETE FROM [INCampaignTypeSet] WHERE [INCampaignTypeSet].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigntypeset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) incampaigntypeset from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the incampaigntypeset from the database.</returns>
        internal static string DeleteHistory(INCampaignTypeSet incampaigntypeset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigntypeset != null)
            {
                query = "DELETE FROM [zHstINCampaignTypeSet] WHERE [zHstINCampaignTypeSet].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigntypeset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) incampaigntypeset from the database.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset object to undelete.</param>
        /// <returns>A query that can be used to undelete the incampaigntypeset from the database.</returns>
        internal static string UnDelete(INCampaignTypeSet incampaigntypeset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigntypeset != null)
            {
                query = "INSERT INTO [INCampaignTypeSet] ([ID], [FKCampaignTypeID], [FKCampaignTypeGroupID]) SELECT [ID], [FKCampaignTypeID], [FKCampaignTypeGroupID] FROM [zHstINCampaignTypeSet] WHERE [zHstINCampaignTypeSet].[ID] = @ID AND [zHstINCampaignTypeSet].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaignTypeSet] WHERE [zHstINCampaignTypeSet].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INCampaignTypeSet] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINCampaignTypeSet] WHERE [zHstINCampaignTypeSet].[ID] = @ID AND [zHstINCampaignTypeSet].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaignTypeSet] WHERE [zHstINCampaignTypeSet].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INCampaignTypeSet] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigntypeset.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an incampaigntypeset object.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset object to fill.</param>
        /// <returns>A query that can be used to fill the incampaigntypeset object.</returns>
        internal static string Fill(INCampaignTypeSet incampaigntypeset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigntypeset != null)
            {
                query = "SELECT [ID], [FKCampaignTypeID], [FKCampaignTypeGroupID] FROM [INCampaignTypeSet] WHERE [INCampaignTypeSet].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigntypeset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  incampaigntypeset data.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  incampaigntypeset data.</returns>
        internal static string FillData(INCampaignTypeSet incampaigntypeset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaigntypeset != null)
            {
            query.Append("SELECT [INCampaignTypeSet].[ID], [INCampaignTypeSet].[FKCampaignTypeID], [INCampaignTypeSet].[FKCampaignTypeGroupID]");
            query.Append(" FROM [INCampaignTypeSet] ");
                query.Append(" WHERE [INCampaignTypeSet].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigntypeset.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an incampaigntypeset object from history.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the incampaigntypeset object from history.</returns>
        internal static string FillHistory(INCampaignTypeSet incampaigntypeset, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigntypeset != null)
            {
                query = "SELECT [ID], [FKCampaignTypeID], [FKCampaignTypeGroupID] FROM [zHstINCampaignTypeSet] WHERE [zHstINCampaignTypeSet].[ID] = @ID AND [zHstINCampaignTypeSet].[StampUserID] = @StampUserID AND [zHstINCampaignTypeSet].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", incampaigntypeset.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaigntypesets in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the incampaigntypesets in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INCampaignTypeSet].[ID], [INCampaignTypeSet].[FKCampaignTypeID], [INCampaignTypeSet].[FKCampaignTypeGroupID]");
            query.Append(" FROM [INCampaignTypeSet] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted incampaigntypesets in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted incampaigntypesets in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINCampaignTypeSet].[ID], [zHstINCampaignTypeSet].[FKCampaignTypeID], [zHstINCampaignTypeSet].[FKCampaignTypeGroupID]");
            query.Append(" FROM [zHstINCampaignTypeSet] ");
            query.Append("INNER JOIN (SELECT [zHstINCampaignTypeSet].[ID], MAX([zHstINCampaignTypeSet].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINCampaignTypeSet] ");
            query.Append("WHERE [zHstINCampaignTypeSet].[ID] NOT IN (SELECT [INCampaignTypeSet].[ID] FROM [INCampaignTypeSet]) ");
            query.Append("GROUP BY [zHstINCampaignTypeSet].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINCampaignTypeSet].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINCampaignTypeSet].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) incampaigntypeset in the database.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) incampaigntypeset in the database.</returns>
        public static string ListHistory(INCampaignTypeSet incampaigntypeset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaigntypeset != null)
            {
            query.Append("SELECT [zHstINCampaignTypeSet].[ID], [zHstINCampaignTypeSet].[FKCampaignTypeID], [zHstINCampaignTypeSet].[FKCampaignTypeGroupID]");
            query.Append(" FROM [zHstINCampaignTypeSet] ");
                query.Append(" WHERE [zHstINCampaignTypeSet].[ID] = @ID");
                query.Append(" ORDER BY [zHstINCampaignTypeSet].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigntypeset.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) incampaigntypeset to the database.
        /// </summary>
        /// <param name="incampaigntypeset">The incampaigntypeset to save.</param>
        /// <returns>A query that can be used to save the incampaigntypeset to the database.</returns>
        internal static string Save(INCampaignTypeSet incampaigntypeset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaigntypeset != null)
            {
                if (incampaigntypeset.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINCampaignTypeSet] ([ID], [FKCampaignTypeID], [FKCampaignTypeGroupID]) SELECT [ID], [FKCampaignTypeID], [FKCampaignTypeGroupID] FROM [INCampaignTypeSet] WHERE [INCampaignTypeSet].[ID] = @ID; ");
                    query.Append("UPDATE [INCampaignTypeSet]");
                    parameters = new object[3];
                    query.Append(" SET [FKCampaignTypeID] = @FKCampaignTypeID");
                    parameters[0] = Database.GetParameter("@FKCampaignTypeID", incampaigntypeset.FKCampaignTypeID.HasValue ? (object)incampaigntypeset.FKCampaignTypeID.Value : DBNull.Value);
                    query.Append(", [FKCampaignTypeGroupID] = @FKCampaignTypeGroupID");
                    parameters[1] = Database.GetParameter("@FKCampaignTypeGroupID", incampaigntypeset.FKCampaignTypeGroupID.HasValue ? (object)incampaigntypeset.FKCampaignTypeGroupID.Value : DBNull.Value);
                    query.Append(" WHERE [INCampaignTypeSet].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", incampaigntypeset.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INCampaignTypeSet] ([FKCampaignTypeID], [FKCampaignTypeGroupID]) VALUES(@FKCampaignTypeID, @FKCampaignTypeGroupID);");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKCampaignTypeID", incampaigntypeset.FKCampaignTypeID.HasValue ? (object)incampaigntypeset.FKCampaignTypeID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKCampaignTypeGroupID", incampaigntypeset.FKCampaignTypeGroupID.HasValue ? (object)incampaigntypeset.FKCampaignTypeGroupID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaigntypesets that match the search criteria.
        /// </summary>
        /// <param name="fkcampaigntypeid">The fkcampaigntypeid search criteria.</param>
        /// <param name="fkcampaigntypegroupid">The fkcampaigntypegroupid search criteria.</param>
        /// <returns>A query that can be used to search for incampaigntypesets based on the search criteria.</returns>
        internal static string Search(long? fkcampaigntypeid, long? fkcampaigntypegroupid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkcampaigntypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaignTypeSet].[FKCampaignTypeID] = " + fkcampaigntypeid + "");
            }
            if (fkcampaigntypegroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaignTypeSet].[FKCampaignTypeGroupID] = " + fkcampaigntypegroupid + "");
            }
            query.Append("SELECT [INCampaignTypeSet].[ID], [INCampaignTypeSet].[FKCampaignTypeID], [INCampaignTypeSet].[FKCampaignTypeGroupID]");
            query.Append(" FROM [INCampaignTypeSet] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
