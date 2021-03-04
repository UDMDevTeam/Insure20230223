using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inoptionextra objects.
    /// </summary>
    internal abstract partial class INOptionExtraQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inoptionextra from the database.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra object to delete.</param>
        /// <returns>A query that can be used to delete the inoptionextra from the database.</returns>
        internal static string Delete(INOptionExtra inoptionextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionextra != null)
            {
                query = "INSERT INTO [zHstINOptionExtra] ([ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID]) SELECT [ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID] FROM [INOptionExtra] WHERE [INOptionExtra].[ID] = @ID; ";
                query += "DELETE FROM [INOptionExtra] WHERE [INOptionExtra].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionextra.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inoptionextra from the database.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inoptionextra from the database.</returns>
        internal static string DeleteHistory(INOptionExtra inoptionextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionextra != null)
            {
                query = "DELETE FROM [zHstINOptionExtra] WHERE [zHstINOptionExtra].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionextra.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inoptionextra from the database.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra object to undelete.</param>
        /// <returns>A query that can be used to undelete the inoptionextra from the database.</returns>
        internal static string UnDelete(INOptionExtra inoptionextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionextra != null)
            {
                query = "INSERT INTO [INOptionExtra] ([ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID]) SELECT [ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID] FROM [zHstINOptionExtra] WHERE [zHstINOptionExtra].[ID] = @ID AND [zHstINOptionExtra].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINOptionExtra] WHERE [zHstINOptionExtra].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INOptionExtra] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINOptionExtra] WHERE [zHstINOptionExtra].[ID] = @ID AND [zHstINOptionExtra].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINOptionExtra] WHERE [zHstINOptionExtra].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INOptionExtra] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionextra.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inoptionextra object.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra object to fill.</param>
        /// <returns>A query that can be used to fill the inoptionextra object.</returns>
        internal static string Fill(INOptionExtra inoptionextra, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionextra != null)
            {
                query = "SELECT [ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID] FROM [INOptionExtra] WHERE [INOptionExtra].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionextra.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inoptionextra data.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inoptionextra data.</returns>
        internal static string FillData(INOptionExtra inoptionextra, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoptionextra != null)
            {
            query.Append("SELECT [INOptionExtra].[ID], [INOptionExtra].[FKOptionID], [INOptionExtra].[LA1CancerComponent], [INOptionExtra].[LA1CancerCover], [INOptionExtra].[LA1CancerPremium], [INOptionExtra].[LA1CancerCost], [INOptionExtra].[LA2CancerComponent], [INOptionExtra].[LA2CancerCover], [INOptionExtra].[LA2CancerPremium], [INOptionExtra].[LA2CancerCost], [INOptionExtra].[StampDate], [INOptionExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOptionExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOptionExtra] ");
                query.Append(" WHERE [INOptionExtra].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionextra.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inoptionextra object from history.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inoptionextra object from history.</returns>
        internal static string FillHistory(INOptionExtra inoptionextra, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionextra != null)
            {
                query = "SELECT [ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID] FROM [zHstINOptionExtra] WHERE [zHstINOptionExtra].[ID] = @ID AND [zHstINOptionExtra].[StampUserID] = @StampUserID AND [zHstINOptionExtra].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inoptionextra.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inoptionextras in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inoptionextras in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INOptionExtra].[ID], [INOptionExtra].[FKOptionID], [INOptionExtra].[LA1CancerComponent], [INOptionExtra].[LA1CancerCover], [INOptionExtra].[LA1CancerPremium], [INOptionExtra].[LA1CancerCost], [INOptionExtra].[LA2CancerComponent], [INOptionExtra].[LA2CancerCover], [INOptionExtra].[LA2CancerPremium], [INOptionExtra].[LA2CancerCost], [INOptionExtra].[StampDate], [INOptionExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOptionExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOptionExtra] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inoptionextras in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inoptionextras in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINOptionExtra].[ID], [zHstINOptionExtra].[FKOptionID], [zHstINOptionExtra].[LA1CancerComponent], [zHstINOptionExtra].[LA1CancerCover], [zHstINOptionExtra].[LA1CancerPremium], [zHstINOptionExtra].[LA1CancerCost], [zHstINOptionExtra].[LA2CancerComponent], [zHstINOptionExtra].[LA2CancerCover], [zHstINOptionExtra].[LA2CancerPremium], [zHstINOptionExtra].[LA2CancerCost], [zHstINOptionExtra].[StampDate], [zHstINOptionExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINOptionExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINOptionExtra] ");
            query.Append("INNER JOIN (SELECT [zHstINOptionExtra].[ID], MAX([zHstINOptionExtra].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINOptionExtra] ");
            query.Append("WHERE [zHstINOptionExtra].[ID] NOT IN (SELECT [INOptionExtra].[ID] FROM [INOptionExtra]) ");
            query.Append("GROUP BY [zHstINOptionExtra].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINOptionExtra].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINOptionExtra].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inoptionextra in the database.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inoptionextra in the database.</returns>
        public static string ListHistory(INOptionExtra inoptionextra, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoptionextra != null)
            {
            query.Append("SELECT [zHstINOptionExtra].[ID], [zHstINOptionExtra].[FKOptionID], [zHstINOptionExtra].[LA1CancerComponent], [zHstINOptionExtra].[LA1CancerCover], [zHstINOptionExtra].[LA1CancerPremium], [zHstINOptionExtra].[LA1CancerCost], [zHstINOptionExtra].[LA2CancerComponent], [zHstINOptionExtra].[LA2CancerCover], [zHstINOptionExtra].[LA2CancerPremium], [zHstINOptionExtra].[LA2CancerCost], [zHstINOptionExtra].[StampDate], [zHstINOptionExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINOptionExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINOptionExtra] ");
                query.Append(" WHERE [zHstINOptionExtra].[ID] = @ID");
                query.Append(" ORDER BY [zHstINOptionExtra].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionextra.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inoptionextra to the database.
        /// </summary>
        /// <param name="inoptionextra">The inoptionextra to save.</param>
        /// <returns>A query that can be used to save the inoptionextra to the database.</returns>
        internal static string Save(INOptionExtra inoptionextra, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoptionextra != null)
            {
                if (inoptionextra.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINOptionExtra] ([ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID]) SELECT [ID], [FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID] FROM [INOptionExtra] WHERE [INOptionExtra].[ID] = @ID; ");
                    query.Append("UPDATE [INOptionExtra]");
                    parameters = new object[10];
                    query.Append(" SET [FKOptionID] = @FKOptionID");
                    parameters[0] = Database.GetParameter("@FKOptionID", inoptionextra.FKOptionID.HasValue ? (object)inoptionextra.FKOptionID.Value : DBNull.Value);
                    query.Append(", [LA1CancerComponent] = @LA1CancerComponent");
                    parameters[1] = Database.GetParameter("@LA1CancerComponent", string.IsNullOrEmpty(inoptionextra.LA1CancerComponent) ? DBNull.Value : (object)inoptionextra.LA1CancerComponent);
                    query.Append(", [LA1CancerCover] = @LA1CancerCover");
                    parameters[2] = Database.GetParameter("@LA1CancerCover", inoptionextra.LA1CancerCover.HasValue ? (object)inoptionextra.LA1CancerCover.Value : DBNull.Value);
                    query.Append(", [LA1CancerPremium] = @LA1CancerPremium");
                    parameters[3] = Database.GetParameter("@LA1CancerPremium", inoptionextra.LA1CancerPremium.HasValue ? (object)inoptionextra.LA1CancerPremium.Value : DBNull.Value);
                    query.Append(", [LA1CancerCost] = @LA1CancerCost");
                    parameters[4] = Database.GetParameter("@LA1CancerCost", inoptionextra.LA1CancerCost.HasValue ? (object)inoptionextra.LA1CancerCost.Value : DBNull.Value);
                    query.Append(", [LA2CancerComponent] = @LA2CancerComponent");
                    parameters[5] = Database.GetParameter("@LA2CancerComponent", string.IsNullOrEmpty(inoptionextra.LA2CancerComponent) ? DBNull.Value : (object)inoptionextra.LA2CancerComponent);
                    query.Append(", [LA2CancerCover] = @LA2CancerCover");
                    parameters[6] = Database.GetParameter("@LA2CancerCover", inoptionextra.LA2CancerCover.HasValue ? (object)inoptionextra.LA2CancerCover.Value : DBNull.Value);
                    query.Append(", [LA2CancerPremium] = @LA2CancerPremium");
                    parameters[7] = Database.GetParameter("@LA2CancerPremium", inoptionextra.LA2CancerPremium.HasValue ? (object)inoptionextra.LA2CancerPremium.Value : DBNull.Value);
                    query.Append(", [LA2CancerCost] = @LA2CancerCost");
                    parameters[8] = Database.GetParameter("@LA2CancerCost", inoptionextra.LA2CancerCost.HasValue ? (object)inoptionextra.LA2CancerCost.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INOptionExtra].[ID] = @ID"); 
                    parameters[9] = Database.GetParameter("@ID", inoptionextra.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INOptionExtra] ([FKOptionID], [LA1CancerComponent], [LA1CancerCover], [LA1CancerPremium], [LA1CancerCost], [LA2CancerComponent], [LA2CancerCover], [LA2CancerPremium], [LA2CancerCost], [StampDate], [StampUserID]) VALUES(@FKOptionID, @LA1CancerComponent, @LA1CancerCover, @LA1CancerPremium, @LA1CancerCost, @LA2CancerComponent, @LA2CancerCover, @LA2CancerPremium, @LA2CancerCost, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[9];
                    parameters[0] = Database.GetParameter("@FKOptionID", inoptionextra.FKOptionID.HasValue ? (object)inoptionextra.FKOptionID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@LA1CancerComponent", string.IsNullOrEmpty(inoptionextra.LA1CancerComponent) ? DBNull.Value : (object)inoptionextra.LA1CancerComponent);
                    parameters[2] = Database.GetParameter("@LA1CancerCover", inoptionextra.LA1CancerCover.HasValue ? (object)inoptionextra.LA1CancerCover.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@LA1CancerPremium", inoptionextra.LA1CancerPremium.HasValue ? (object)inoptionextra.LA1CancerPremium.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@LA1CancerCost", inoptionextra.LA1CancerCost.HasValue ? (object)inoptionextra.LA1CancerCost.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@LA2CancerComponent", string.IsNullOrEmpty(inoptionextra.LA2CancerComponent) ? DBNull.Value : (object)inoptionextra.LA2CancerComponent);
                    parameters[6] = Database.GetParameter("@LA2CancerCover", inoptionextra.LA2CancerCover.HasValue ? (object)inoptionextra.LA2CancerCover.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@LA2CancerPremium", inoptionextra.LA2CancerPremium.HasValue ? (object)inoptionextra.LA2CancerPremium.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@LA2CancerCost", inoptionextra.LA2CancerCost.HasValue ? (object)inoptionextra.LA2CancerCost.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inoptionextras that match the search criteria.
        /// </summary>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="la1cancercomponent">The la1cancercomponent search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1cancercost">The la1cancercost search criteria.</param>
        /// <param name="la2cancercomponent">The la2cancercomponent search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2cancercost">The la2cancercost search criteria.</param>
        /// <returns>A query that can be used to search for inoptionextras based on the search criteria.</returns>
        internal static string Search(long? fkoptionid, string la1cancercomponent, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1cancercost, string la2cancercomponent, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2cancercost)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkoptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[FKOptionID] = " + fkoptionid + "");
            }
            if (la1cancercomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA1CancerComponent] LIKE '" + la1cancercomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la1cancercover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA1CancerCover] = " + la1cancercover + "");
            }
            if (la1cancerpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA1CancerPremium] = " + la1cancerpremium + "");
            }
            if (la1cancercost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA1CancerCost] = " + la1cancercost + "");
            }
            if (la2cancercomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA2CancerComponent] LIKE '" + la2cancercomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la2cancercover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA2CancerCover] = " + la2cancercover + "");
            }
            if (la2cancerpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA2CancerPremium] = " + la2cancerpremium + "");
            }
            if (la2cancercost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionExtra].[LA2CancerCost] = " + la2cancercost + "");
            }
            query.Append("SELECT [INOptionExtra].[ID], [INOptionExtra].[FKOptionID], [INOptionExtra].[LA1CancerComponent], [INOptionExtra].[LA1CancerCover], [INOptionExtra].[LA1CancerPremium], [INOptionExtra].[LA1CancerCost], [INOptionExtra].[LA2CancerComponent], [INOptionExtra].[LA2CancerCover], [INOptionExtra].[LA2CancerPremium], [INOptionExtra].[LA2CancerCost], [INOptionExtra].[StampDate], [INOptionExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOptionExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOptionExtra] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
