using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to incampaignset objects.
    /// </summary>
    internal abstract partial class INCampaignSetQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) incampaignset from the database.
        /// </summary>
        /// <param name="incampaignset">The incampaignset object to delete.</param>
        /// <returns>A query that can be used to delete the incampaignset from the database.</returns>
        internal static string Delete(INCampaignSet incampaignset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaignset != null)
            {
                query = "INSERT INTO [zHstINCampaignSet] ([ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [INCampaignSet] WHERE [INCampaignSet].[ID] = @ID; ";
                query += "DELETE FROM [INCampaignSet] WHERE [INCampaignSet].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaignset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) incampaignset from the database.
        /// </summary>
        /// <param name="incampaignset">The incampaignset object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the incampaignset from the database.</returns>
        internal static string DeleteHistory(INCampaignSet incampaignset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaignset != null)
            {
                query = "DELETE FROM [zHstINCampaignSet] WHERE [zHstINCampaignSet].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaignset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) incampaignset from the database.
        /// </summary>
        /// <param name="incampaignset">The incampaignset object to undelete.</param>
        /// <returns>A query that can be used to undelete the incampaignset from the database.</returns>
        internal static string UnDelete(INCampaignSet incampaignset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaignset != null)
            {
                query = "INSERT INTO [INCampaignSet] ([ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [zHstINCampaignSet] WHERE [zHstINCampaignSet].[ID] = @ID AND [zHstINCampaignSet].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaignSet] WHERE [zHstINCampaignSet].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INCampaignSet] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINCampaignSet] WHERE [zHstINCampaignSet].[ID] = @ID AND [zHstINCampaignSet].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaignSet] WHERE [zHstINCampaignSet].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INCampaignSet] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaignset.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an incampaignset object.
        /// </summary>
        /// <param name="incampaignset">The incampaignset object to fill.</param>
        /// <returns>A query that can be used to fill the incampaignset object.</returns>
        internal static string Fill(INCampaignSet incampaignset, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaignset != null)
            {
                query = "SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [INCampaignSet] WHERE [INCampaignSet].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaignset.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  incampaignset data.
        /// </summary>
        /// <param name="incampaignset">The incampaignset to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  incampaignset data.</returns>
        internal static string FillData(INCampaignSet incampaignset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaignset != null)
            {
            query.Append("SELECT [INCampaignSet].[ID], [INCampaignSet].[FKlkpINCampaignGroup], [INCampaignSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [INCampaignSet] ");
                query.Append(" WHERE [INCampaignSet].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaignset.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an incampaignset object from history.
        /// </summary>
        /// <param name="incampaignset">The incampaignset object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the incampaignset object from history.</returns>
        internal static string FillHistory(INCampaignSet incampaignset, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaignset != null)
            {
                query = "SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [zHstINCampaignSet] WHERE [zHstINCampaignSet].[ID] = @ID AND [zHstINCampaignSet].[StampUserID] = @StampUserID AND [zHstINCampaignSet].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", incampaignset.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaignsets in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the incampaignsets in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INCampaignSet].[ID], [INCampaignSet].[FKlkpINCampaignGroup], [INCampaignSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [INCampaignSet] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted incampaignsets in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted incampaignsets in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINCampaignSet].[ID], [zHstINCampaignSet].[FKlkpINCampaignGroup], [zHstINCampaignSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [zHstINCampaignSet] ");
            query.Append("INNER JOIN (SELECT [zHstINCampaignSet].[ID], MAX([zHstINCampaignSet].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINCampaignSet] ");
            query.Append("WHERE [zHstINCampaignSet].[ID] NOT IN (SELECT [INCampaignSet].[ID] FROM [INCampaignSet]) ");
            query.Append("GROUP BY [zHstINCampaignSet].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINCampaignSet].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINCampaignSet].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) incampaignset in the database.
        /// </summary>
        /// <param name="incampaignset">The incampaignset object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) incampaignset in the database.</returns>
        public static string ListHistory(INCampaignSet incampaignset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaignset != null)
            {
            query.Append("SELECT [zHstINCampaignSet].[ID], [zHstINCampaignSet].[FKlkpINCampaignGroup], [zHstINCampaignSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [zHstINCampaignSet] ");
                query.Append(" WHERE [zHstINCampaignSet].[ID] = @ID");
                query.Append(" ORDER BY [zHstINCampaignSet].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaignset.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) incampaignset to the database.
        /// </summary>
        /// <param name="incampaignset">The incampaignset to save.</param>
        /// <returns>A query that can be used to save the incampaignset to the database.</returns>
        internal static string Save(INCampaignSet incampaignset, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaignset != null)
            {
                if (incampaignset.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINCampaignSet] ([ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) SELECT [ID], [FKlkpINCampaignGroup], [FKlkpINCampaignGroupType] FROM [INCampaignSet] WHERE [INCampaignSet].[ID] = @ID; ");
                    query.Append("UPDATE [INCampaignSet]");
                    parameters = new object[3];
                    query.Append(" SET [FKlkpINCampaignGroup] = @FKlkpINCampaignGroup");
                    parameters[0] = Database.GetParameter("@FKlkpINCampaignGroup", incampaignset.FKlkpINCampaignGroup);
                    query.Append(", [FKlkpINCampaignGroupType] = @FKlkpINCampaignGroupType");
                    parameters[1] = Database.GetParameter("@FKlkpINCampaignGroupType", incampaignset.FKlkpINCampaignGroupType);
                    query.Append(" WHERE [INCampaignSet].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", incampaignset.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INCampaignSet] ([FKlkpINCampaignGroup], [FKlkpINCampaignGroupType]) VALUES(@FKlkpINCampaignGroup, @FKlkpINCampaignGroupType);");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKlkpINCampaignGroup", incampaignset.FKlkpINCampaignGroup);
                    parameters[1] = Database.GetParameter("@FKlkpINCampaignGroupType", incampaignset.FKlkpINCampaignGroupType);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaignsets that match the search criteria.
        /// </summary>
        /// <param name="fklkpincampaigngroup">The fklkpincampaigngroup search criteria.</param>
        /// <param name="fklkpincampaigngrouptype">The fklkpincampaigngrouptype search criteria.</param>
        /// <returns>A query that can be used to search for incampaignsets based on the search criteria.</returns>
        internal static string Search(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fklkpincampaigngroup != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaignSet].[FKlkpINCampaignGroup] = " + fklkpincampaigngroup + "");
            }
            if (fklkpincampaigngrouptype != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaignSet].[FKlkpINCampaignGroupType] = " + fklkpincampaigngrouptype + "");
            }
            query.Append("SELECT [INCampaignSet].[ID], [INCampaignSet].[FKlkpINCampaignGroup], [INCampaignSet].[FKlkpINCampaignGroupType]");
            query.Append(" FROM [INCampaignSet] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
