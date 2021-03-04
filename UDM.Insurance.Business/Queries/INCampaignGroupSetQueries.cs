using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to incampaigngroupset objects.
    /// </summary>
    internal abstract partial class INCampaignGroupSetQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) incampaigngroupset from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset object to delete.</param>
        /// <returns>A query that can be used to delete the incampaigngroupset from the database.</returns>
        internal static string Delete(INCampaignGroupSet incampaigngroupset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigngroupset != null)
            {
                query = "INSERT INTO [zHstINCampaignGroupSet] ([ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [INCampaignGroupSet] WHERE [INCampaignGroupSet].[ID] = @ID; ";
                query += "DELETE FROM [INCampaignGroupSet] WHERE [INCampaignGroupSet].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigngroupset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) incampaigngroupset from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the incampaigngroupset from the database.</returns>
        internal static string DeleteHistory(INCampaignGroupSet incampaigngroupset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigngroupset != null)
            {
                query = "DELETE FROM [zHstINCampaignGroupSet] WHERE [zHstINCampaignGroupSet].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigngroupset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) incampaigngroupset from the database.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset object to undelete.</param>
        /// <returns>A query that can be used to undelete the incampaigngroupset from the database.</returns>
        internal static string UnDelete(INCampaignGroupSet incampaigngroupset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigngroupset != null)
            {
                query = "INSERT INTO [INCampaignGroupSet] ([ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [zHstINCampaignGroupSet] WHERE [zHstINCampaignGroupSet].[ID] = @ID AND [zHstINCampaignGroupSet].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaignGroupSet] WHERE [zHstINCampaignGroupSet].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INCampaignGroupSet] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINCampaignGroupSet] WHERE [zHstINCampaignGroupSet].[ID] = @ID AND [zHstINCampaignGroupSet].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaignGroupSet] WHERE [zHstINCampaignGroupSet].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INCampaignGroupSet] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigngroupset.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an incampaigngroupset object.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset object to fill.</param>
        /// <returns>A query that can be used to fill the incampaigngroupset object.</returns>
        internal static string Fill(INCampaignGroupSet incampaigngroupset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigngroupset != null)
            {
                query = "SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [INCampaignGroupSet] WHERE [INCampaignGroupSet].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigngroupset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  incampaigngroupset data.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  incampaigngroupset data.</returns>
        internal static string FillData(INCampaignGroupSet incampaigngroupset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaigngroupset != null)
            {
            query.Append("SELECT [INCampaignGroupSet].[ID], [INCampaignGroupSet].[FKlkpINCampaignGroup], [INCampaignGroupSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [INCampaignGroupSet] ");
                query.Append(" WHERE [INCampaignGroupSet].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigngroupset.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an incampaigngroupset object from history.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the incampaigngroupset object from history.</returns>
        internal static string FillHistory(INCampaignGroupSet incampaigngroupset, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaigngroupset != null)
            {
                query = "SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [zHstINCampaignGroupSet] WHERE [zHstINCampaignGroupSet].[ID] = @ID AND [zHstINCampaignGroupSet].[StampUserID] = @StampUserID AND [zHstINCampaignGroupSet].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", incampaigngroupset.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaigngroupsets in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the incampaigngroupsets in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INCampaignGroupSet].[ID], [INCampaignGroupSet].[FKlkpINCampaignGroup], [INCampaignGroupSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [INCampaignGroupSet] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted incampaigngroupsets in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted incampaigngroupsets in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINCampaignGroupSet].[ID], [zHstINCampaignGroupSet].[FKlkpINCampaignGroup], [zHstINCampaignGroupSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [zHstINCampaignGroupSet] ");
            query.Append("INNER JOIN (SELECT [zHstINCampaignGroupSet].[ID], MAX([zHstINCampaignGroupSet].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINCampaignGroupSet] ");
            query.Append("WHERE [zHstINCampaignGroupSet].[ID] NOT IN (SELECT [INCampaignGroupSet].[ID] FROM [INCampaignGroupSet]) ");
            query.Append("GROUP BY [zHstINCampaignGroupSet].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINCampaignGroupSet].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINCampaignGroupSet].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) incampaigngroupset in the database.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) incampaigngroupset in the database.</returns>
        public static string ListHistory(INCampaignGroupSet incampaigngroupset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaigngroupset != null)
            {
            query.Append("SELECT [zHstINCampaignGroupSet].[ID], [zHstINCampaignGroupSet].[FKlkpINCampaignGroup], [zHstINCampaignGroupSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [zHstINCampaignGroupSet] ");
                query.Append(" WHERE [zHstINCampaignGroupSet].[ID] = @ID");
                query.Append(" ORDER BY [zHstINCampaignGroupSet].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaigngroupset.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) incampaigngroupset to the database.
        /// </summary>
        /// <param name="incampaigngroupset">The incampaigngroupset to save.</param>
        /// <returns>A query that can be used to save the incampaigngroupset to the database.</returns>
        internal static string Save(INCampaignGroupSet incampaigngroupset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaigngroupset != null)
            {
                if (incampaigngroupset.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINCampaignGroupSet] ([ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [INCampaignGroupSet] WHERE [INCampaignGroupSet].[ID] = @ID; ");
                    query.Append("UPDATE [INCampaignGroupSet]");
                    parameters = new object[3];
                    query.Append(" SET [FKlkpINCampaignGroup] = @FKlkpINCampaignGroup");
                    parameters[0] = Database.GetParameter("@FKlkpINCampaignGroup", incampaigngroupset.FKlkpINCampaignGroup.HasValue ? (object)incampaigngroupset.FKlkpINCampaignGroup.Value : DBNull.Value);
                    query.Append(", [FKlkpINCampaignGroupType] = @FKlkpINCampaignGroupType");
                    parameters[1] = Database.GetParameter("@FKlkpINCampaignGroupType", incampaigngroupset.FKlkpINCampaignGroupType.HasValue ? (object)incampaigngroupset.FKlkpINCampaignGroupType.Value : DBNull.Value);
                    query.Append(" WHERE [INCampaignGroupSet].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", incampaigngroupset.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INCampaignGroupSet] ([FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) VALUES(@FKlkpINCampaignGroup, @FKlkpINCampaignGroupType);");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKlkpINCampaignGroup", incampaigngroupset.FKlkpINCampaignGroup.HasValue ? (object)incampaigngroupset.FKlkpINCampaignGroup.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKlkpINCampaignGroupType", incampaigngroupset.FKlkpINCampaignGroupType.HasValue ? (object)incampaigngroupset.FKlkpINCampaignGroupType.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaigngroupsets that match the search criteria.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <returns>A query that can be used to search for incampaigngroupsets based on the search criteria.</returns>
        internal static string Search(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fklkpincampaigngroup != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaignGroupSet].[FKlkpINCampaignGroup] = " + fklkpincampaigngroup + "");
            }
            if (fklkpincampaigngrouptype != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaignGroupSet].[FKlkpINCampaignGroupType] = " + fklkpincampaigngrouptype + "");
            }
            query.Append("SELECT [INCampaignGroupSet].[ID], [INCampaignGroupSet].[FKlkpINCampaignGroup], [INCampaignGroupSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [INCampaignGroupSet] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
