using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inpolicylifeassured objects.
    /// </summary>
    internal abstract partial class INPolicyLifeAssuredQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inpolicylifeassured from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured object to delete.</param>
        /// <returns>A query that can be used to delete the inpolicylifeassured from the database.</returns>
        internal static string Delete(INPolicyLifeAssured inpolicylifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicylifeassured != null)
            {
                query = "INSERT INTO [zHstINPolicyLifeAssured] ([ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID] FROM [INPolicyLifeAssured] WHERE [INPolicyLifeAssured].[ID] = @ID; ";
                query += "DELETE FROM [INPolicyLifeAssured] WHERE [INPolicyLifeAssured].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicylifeassured.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inpolicylifeassured from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inpolicylifeassured from the database.</returns>
        internal static string DeleteHistory(INPolicyLifeAssured inpolicylifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicylifeassured != null)
            {
                query = "DELETE FROM [zHstINPolicyLifeAssured] WHERE [zHstINPolicyLifeAssured].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicylifeassured.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inpolicylifeassured from the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured object to undelete.</param>
        /// <returns>A query that can be used to undelete the inpolicylifeassured from the database.</returns>
        internal static string UnDelete(INPolicyLifeAssured inpolicylifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicylifeassured != null)
            {
                query = "INSERT INTO [INPolicyLifeAssured] ([ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID] FROM [zHstINPolicyLifeAssured] WHERE [zHstINPolicyLifeAssured].[ID] = @ID AND [zHstINPolicyLifeAssured].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyLifeAssured] WHERE [zHstINPolicyLifeAssured].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPolicyLifeAssured] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPolicyLifeAssured] WHERE [zHstINPolicyLifeAssured].[ID] = @ID AND [zHstINPolicyLifeAssured].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyLifeAssured] WHERE [zHstINPolicyLifeAssured].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPolicyLifeAssured] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicylifeassured.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inpolicylifeassured object.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured object to fill.</param>
        /// <returns>A query that can be used to fill the inpolicylifeassured object.</returns>
        internal static string Fill(INPolicyLifeAssured inpolicylifeassured, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicylifeassured != null)
            {
                query = "SELECT [ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID] FROM [INPolicyLifeAssured] WHERE [INPolicyLifeAssured].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicylifeassured.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inpolicylifeassured data.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inpolicylifeassured data.</returns>
        internal static string FillData(INPolicyLifeAssured inpolicylifeassured, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicylifeassured != null)
            {
            query.Append("SELECT [INPolicyLifeAssured].[ID], [INPolicyLifeAssured].[FKINPolicyID], [INPolicyLifeAssured].[FKINLifeAssuredID], [INPolicyLifeAssured].[LifeAssuredRank], [INPolicyLifeAssured].[StampDate], [INPolicyLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyLifeAssured] ");
                query.Append(" WHERE [INPolicyLifeAssured].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicylifeassured.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inpolicylifeassured object from history.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inpolicylifeassured object from history.</returns>
        internal static string FillHistory(INPolicyLifeAssured inpolicylifeassured, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicylifeassured != null)
            {
                query = "SELECT [ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID] FROM [zHstINPolicyLifeAssured] WHERE [zHstINPolicyLifeAssured].[ID] = @ID AND [zHstINPolicyLifeAssured].[StampUserID] = @StampUserID AND [zHstINPolicyLifeAssured].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inpolicylifeassured.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicylifeassureds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inpolicylifeassureds in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INPolicyLifeAssured].[ID], [INPolicyLifeAssured].[FKINPolicyID], [INPolicyLifeAssured].[FKINLifeAssuredID], [INPolicyLifeAssured].[LifeAssuredRank], [INPolicyLifeAssured].[StampDate], [INPolicyLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyLifeAssured] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inpolicylifeassureds in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inpolicylifeassureds in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPolicyLifeAssured].[ID], [zHstINPolicyLifeAssured].[FKINPolicyID], [zHstINPolicyLifeAssured].[FKINLifeAssuredID], [zHstINPolicyLifeAssured].[LifeAssuredRank], [zHstINPolicyLifeAssured].[StampDate], [zHstINPolicyLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyLifeAssured] ");
            query.Append("INNER JOIN (SELECT [zHstINPolicyLifeAssured].[ID], MAX([zHstINPolicyLifeAssured].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPolicyLifeAssured] ");
            query.Append("WHERE [zHstINPolicyLifeAssured].[ID] NOT IN (SELECT [INPolicyLifeAssured].[ID] FROM [INPolicyLifeAssured]) ");
            query.Append("GROUP BY [zHstINPolicyLifeAssured].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPolicyLifeAssured].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPolicyLifeAssured].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inpolicylifeassured in the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inpolicylifeassured in the database.</returns>
        public static string ListHistory(INPolicyLifeAssured inpolicylifeassured, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicylifeassured != null)
            {
            query.Append("SELECT [zHstINPolicyLifeAssured].[ID], [zHstINPolicyLifeAssured].[FKINPolicyID], [zHstINPolicyLifeAssured].[FKINLifeAssuredID], [zHstINPolicyLifeAssured].[LifeAssuredRank], [zHstINPolicyLifeAssured].[StampDate], [zHstINPolicyLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyLifeAssured] ");
                query.Append(" WHERE [zHstINPolicyLifeAssured].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPolicyLifeAssured].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicylifeassured.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inpolicylifeassured to the database.
        /// </summary>
        /// <param name="inpolicylifeassured">The inpolicylifeassured to save.</param>
        /// <returns>A query that can be used to save the inpolicylifeassured to the database.</returns>
        internal static string Save(INPolicyLifeAssured inpolicylifeassured, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicylifeassured != null)
            {
                if (inpolicylifeassured.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPolicyLifeAssured] ([ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID] FROM [INPolicyLifeAssured] WHERE [INPolicyLifeAssured].[ID] = @ID; ");
                    query.Append("UPDATE [INPolicyLifeAssured]");
                    parameters = new object[4];
                    query.Append(" SET [FKINPolicyID] = @FKINPolicyID");
                    parameters[0] = Database.GetParameter("@FKINPolicyID", inpolicylifeassured.FKINPolicyID.HasValue ? (object)inpolicylifeassured.FKINPolicyID.Value : DBNull.Value);
                    query.Append(", [FKINLifeAssuredID] = @FKINLifeAssuredID");
                    parameters[1] = Database.GetParameter("@FKINLifeAssuredID", inpolicylifeassured.FKINLifeAssuredID.HasValue ? (object)inpolicylifeassured.FKINLifeAssuredID.Value : DBNull.Value);
                    query.Append(", [LifeAssuredRank] = @LifeAssuredRank");
                    parameters[2] = Database.GetParameter("@LifeAssuredRank", inpolicylifeassured.LifeAssuredRank.HasValue ? (object)inpolicylifeassured.LifeAssuredRank.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPolicyLifeAssured].[ID] = @ID"); 
                    parameters[3] = Database.GetParameter("@ID", inpolicylifeassured.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPolicyLifeAssured] ([FKINPolicyID], [FKINLifeAssuredID], [LifeAssuredRank], [StampDate], [StampUserID]) VALUES(@FKINPolicyID, @FKINLifeAssuredID, @LifeAssuredRank, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[3];
                    parameters[0] = Database.GetParameter("@FKINPolicyID", inpolicylifeassured.FKINPolicyID.HasValue ? (object)inpolicylifeassured.FKINPolicyID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINLifeAssuredID", inpolicylifeassured.FKINLifeAssuredID.HasValue ? (object)inpolicylifeassured.FKINLifeAssuredID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@LifeAssuredRank", inpolicylifeassured.LifeAssuredRank.HasValue ? (object)inpolicylifeassured.LifeAssuredRank.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicylifeassureds that match the search criteria.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinlifeassuredid">The fkinlifeassuredid search criteria.</param>
        /// <param name="lifeassuredrank">The lifeassuredrank search criteria.</param>
        /// <returns>A query that can be used to search for inpolicylifeassureds based on the search criteria.</returns>
        internal static string Search(long? fkinpolicyid, long? fkinlifeassuredid, int? lifeassuredrank)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinpolicyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyLifeAssured].[FKINPolicyID] = " + fkinpolicyid + "");
            }
            if (fkinlifeassuredid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyLifeAssured].[FKINLifeAssuredID] = " + fkinlifeassuredid + "");
            }
            if (lifeassuredrank != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyLifeAssured].[LifeAssuredRank] = " + lifeassuredrank + "");
            }
            query.Append("SELECT [INPolicyLifeAssured].[ID], [INPolicyLifeAssured].[FKINPolicyID], [INPolicyLifeAssured].[FKINLifeAssuredID], [INPolicyLifeAssured].[LifeAssuredRank], [INPolicyLifeAssured].[StampDate], [INPolicyLifeAssured].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyLifeAssured].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyLifeAssured] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
