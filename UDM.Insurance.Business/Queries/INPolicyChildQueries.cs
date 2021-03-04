using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inpolicychild objects.
    /// </summary>
    internal abstract partial class INPolicyChildQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inpolicychild from the database.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild object to delete.</param>
        /// <returns>A query that can be used to delete the inpolicychild from the database.</returns>
        internal static string Delete(INPolicyChild inpolicychild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicychild != null)
            {
                query = "INSERT INTO [zHstINPolicyChild] ([ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID] FROM [INPolicyChild] WHERE [INPolicyChild].[ID] = @ID; ";
                query += "DELETE FROM [INPolicyChild] WHERE [INPolicyChild].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicychild.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inpolicychild from the database.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inpolicychild from the database.</returns>
        internal static string DeleteHistory(INPolicyChild inpolicychild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicychild != null)
            {
                query = "DELETE FROM [zHstINPolicyChild] WHERE [zHstINPolicyChild].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicychild.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inpolicychild from the database.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild object to undelete.</param>
        /// <returns>A query that can be used to undelete the inpolicychild from the database.</returns>
        internal static string UnDelete(INPolicyChild inpolicychild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicychild != null)
            {
                query = "INSERT INTO [INPolicyChild] ([ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID] FROM [zHstINPolicyChild] WHERE [zHstINPolicyChild].[ID] = @ID AND [zHstINPolicyChild].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyChild] WHERE [zHstINPolicyChild].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPolicyChild] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPolicyChild] WHERE [zHstINPolicyChild].[ID] = @ID AND [zHstINPolicyChild].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyChild] WHERE [zHstINPolicyChild].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPolicyChild] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicychild.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inpolicychild object.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild object to fill.</param>
        /// <returns>A query that can be used to fill the inpolicychild object.</returns>
        internal static string Fill(INPolicyChild inpolicychild, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicychild != null)
            {
                query = "SELECT [ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID] FROM [INPolicyChild] WHERE [INPolicyChild].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicychild.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inpolicychild data.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inpolicychild data.</returns>
        internal static string FillData(INPolicyChild inpolicychild, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicychild != null)
            {
            query.Append("SELECT [INPolicyChild].[ID], [INPolicyChild].[FKINPolicyID], [INPolicyChild].[FKINChildID], [INPolicyChild].[ChildRank], [INPolicyChild].[StampDate], [INPolicyChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyChild] ");
                query.Append(" WHERE [INPolicyChild].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicychild.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inpolicychild object from history.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inpolicychild object from history.</returns>
        internal static string FillHistory(INPolicyChild inpolicychild, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicychild != null)
            {
                query = "SELECT [ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID] FROM [zHstINPolicyChild] WHERE [zHstINPolicyChild].[ID] = @ID AND [zHstINPolicyChild].[StampUserID] = @StampUserID AND [zHstINPolicyChild].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inpolicychild.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicychilds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inpolicychilds in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INPolicyChild].[ID], [INPolicyChild].[FKINPolicyID], [INPolicyChild].[FKINChildID], [INPolicyChild].[ChildRank], [INPolicyChild].[StampDate], [INPolicyChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyChild] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inpolicychilds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inpolicychilds in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPolicyChild].[ID], [zHstINPolicyChild].[FKINPolicyID], [zHstINPolicyChild].[FKINChildID], [zHstINPolicyChild].[ChildRank], [zHstINPolicyChild].[StampDate], [zHstINPolicyChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyChild] ");
            query.Append("INNER JOIN (SELECT [zHstINPolicyChild].[ID], MAX([zHstINPolicyChild].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPolicyChild] ");
            query.Append("WHERE [zHstINPolicyChild].[ID] NOT IN (SELECT [INPolicyChild].[ID] FROM [INPolicyChild]) ");
            query.Append("GROUP BY [zHstINPolicyChild].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPolicyChild].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPolicyChild].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inpolicychild in the database.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inpolicychild in the database.</returns>
        public static string ListHistory(INPolicyChild inpolicychild, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicychild != null)
            {
            query.Append("SELECT [zHstINPolicyChild].[ID], [zHstINPolicyChild].[FKINPolicyID], [zHstINPolicyChild].[FKINChildID], [zHstINPolicyChild].[ChildRank], [zHstINPolicyChild].[StampDate], [zHstINPolicyChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyChild] ");
                query.Append(" WHERE [zHstINPolicyChild].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPolicyChild].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicychild.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inpolicychild to the database.
        /// </summary>
        /// <param name="inpolicychild">The inpolicychild to save.</param>
        /// <returns>A query that can be used to save the inpolicychild to the database.</returns>
        internal static string Save(INPolicyChild inpolicychild, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicychild != null)
            {
                if (inpolicychild.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPolicyChild] ([ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID] FROM [INPolicyChild] WHERE [INPolicyChild].[ID] = @ID; ");
                    query.Append("UPDATE [INPolicyChild]");
                    parameters = new object[4];
                    query.Append(" SET [FKINPolicyID] = @FKINPolicyID");
                    parameters[0] = Database.GetParameter("@FKINPolicyID", inpolicychild.FKINPolicyID.HasValue ? (object)inpolicychild.FKINPolicyID.Value : DBNull.Value);
                    query.Append(", [FKINChildID] = @FKINChildID");
                    parameters[1] = Database.GetParameter("@FKINChildID", inpolicychild.FKINChildID.HasValue ? (object)inpolicychild.FKINChildID.Value : DBNull.Value);
                    query.Append(", [ChildRank] = @ChildRank");
                    parameters[2] = Database.GetParameter("@ChildRank", inpolicychild.ChildRank.HasValue ? (object)inpolicychild.ChildRank.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPolicyChild].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", inpolicychild.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPolicyChild] ([FKINPolicyID], [FKINChildID], [ChildRank], [StampDate], [StampUserID]) VALUES(@FKINPolicyID, @FKINChildID, @ChildRank, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@FKINPolicyID", inpolicychild.FKINPolicyID.HasValue ? (object)inpolicychild.FKINPolicyID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINChildID", inpolicychild.FKINChildID.HasValue ? (object)inpolicychild.FKINChildID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@ChildRank", inpolicychild.ChildRank.HasValue ? (object)inpolicychild.ChildRank.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicychilds that match the search criteria.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinchildid">The fkinchildid search criteria.</param>
        /// <param name="childrank">The childrank search criteria.</param>
        /// <returns>A query that can be used to search for inpolicychilds based on the search criteria.</returns>
        internal static string Search(long? fkinpolicyid, long? fkinchildid, int? childrank)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinpolicyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyChild].[FKINPolicyID] = " + fkinpolicyid + "");
            }
            if (fkinchildid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyChild].[FKINChildID] = " + fkinchildid + "");
            }
            if (childrank != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyChild].[ChildRank] = " + childrank + "");
            }
            query.Append("SELECT [INPolicyChild].[ID], [INPolicyChild].[FKINPolicyID], [INPolicyChild].[FKINChildID], [INPolicyChild].[ChildRank], [INPolicyChild].[StampDate], [INPolicyChild].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyChild].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyChild] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
