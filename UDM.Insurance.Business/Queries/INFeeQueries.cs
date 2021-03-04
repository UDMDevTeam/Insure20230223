using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to infee objects.
    /// </summary>
    internal abstract partial class INFeeQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) infee from the database.
        /// </summary>
        /// <param name="infee">The infee object to delete.</param>
        /// <returns>A query that can be used to delete the infee from the database.</returns>
        internal static string Delete(INFee infee, ref object[] parameters)
        {
            string query = string.Empty;
            if (infee != null)
            {
                query = "INSERT INTO [zHstINFee] ([ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID]) SELECT [ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID] FROM [INFee] WHERE [INFee].[ID] = @ID; ";
                query += "DELETE FROM [INFee] WHERE [INFee].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infee.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) infee from the database.
        /// </summary>
        /// <param name="infee">The infee object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the infee from the database.</returns>
        internal static string DeleteHistory(INFee infee, ref object[] parameters)
        {
            string query = string.Empty;
            if (infee != null)
            {
                query = "DELETE FROM [zHstINFee] WHERE [zHstINFee].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infee.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) infee from the database.
        /// </summary>
        /// <param name="infee">The infee object to undelete.</param>
        /// <returns>A query that can be used to undelete the infee from the database.</returns>
        internal static string UnDelete(INFee infee, ref object[] parameters)
        {
            string query = string.Empty;
            if (infee != null)
            {
                query = "INSERT INTO [INFee] ([ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID]) SELECT [ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID] FROM [zHstINFee] WHERE [zHstINFee].[ID] = @ID AND [zHstINFee].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINFee] WHERE [zHstINFee].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INFee] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINFee] WHERE [zHstINFee].[ID] = @ID AND [zHstINFee].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINFee] WHERE [zHstINFee].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INFee] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infee.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an infee object.
        /// </summary>
        /// <param name="infee">The infee object to fill.</param>
        /// <returns>A query that can be used to fill the infee object.</returns>
        internal static string Fill(INFee infee, ref object[] parameters)
        {
            string query = string.Empty;
            if (infee != null)
            {
                query = "SELECT [ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID] FROM [INFee] WHERE [INFee].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infee.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  infee data.
        /// </summary>
        /// <param name="infee">The infee to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  infee data.</returns>
        internal static string FillData(INFee infee, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infee != null)
            {
            query.Append("SELECT [INFee].[ID], [INFee].[LA1Fee], [INFee].[LA2Fee], [INFee].[ChildFee], [INFee].[UnitFee], [INFee].[StampDate], [INFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFee] ");
                query.Append(" WHERE [INFee].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infee.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an infee object from history.
        /// </summary>
        /// <param name="infee">The infee object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the infee object from history.</returns>
        internal static string FillHistory(INFee infee, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (infee != null)
            {
                query = "SELECT [ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID] FROM [zHstINFee] WHERE [zHstINFee].[ID] = @ID AND [zHstINFee].[StampUserID] = @StampUserID AND [zHstINFee].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", infee.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the infees in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the infees in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INFee].[ID], [INFee].[LA1Fee], [INFee].[LA2Fee], [INFee].[ChildFee], [INFee].[UnitFee], [INFee].[StampDate], [INFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFee] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted infees in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted infees in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINFee].[ID], [zHstINFee].[LA1Fee], [zHstINFee].[LA2Fee], [zHstINFee].[ChildFee], [zHstINFee].[UnitFee], [zHstINFee].[StampDate], [zHstINFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINFee] ");
            query.Append("INNER JOIN (SELECT [zHstINFee].[ID], MAX([zHstINFee].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINFee] ");
            query.Append("WHERE [zHstINFee].[ID] NOT IN (SELECT [INFee].[ID] FROM [INFee]) ");
            query.Append("GROUP BY [zHstINFee].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINFee].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINFee].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) infee in the database.
        /// </summary>
        /// <param name="infee">The infee object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) infee in the database.</returns>
        public static string ListHistory(INFee infee, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infee != null)
            {
            query.Append("SELECT [zHstINFee].[ID], [zHstINFee].[LA1Fee], [zHstINFee].[LA2Fee], [zHstINFee].[ChildFee], [zHstINFee].[UnitFee], [zHstINFee].[StampDate], [zHstINFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINFee] ");
                query.Append(" WHERE [zHstINFee].[ID] = @ID");
                query.Append(" ORDER BY [zHstINFee].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infee.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) infee to the database.
        /// </summary>
        /// <param name="infee">The infee to save.</param>
        /// <returns>A query that can be used to save the infee to the database.</returns>
        internal static string Save(INFee infee, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infee != null)
            {
                if (infee.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINFee] ([ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID]) SELECT [ID], [LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID] FROM [INFee] WHERE [INFee].[ID] = @ID; ");
                    query.Append("UPDATE [INFee]");
                    parameters = new object[5];
                    query.Append(" SET [LA1Fee] = @LA1Fee");
                    parameters[0] = Database.GetParameter("@LA1Fee", infee.LA1Fee.HasValue ? (object)infee.LA1Fee.Value : DBNull.Value);
                    query.Append(", [LA2Fee] = @LA2Fee");
                    parameters[1] = Database.GetParameter("@LA2Fee", infee.LA2Fee.HasValue ? (object)infee.LA2Fee.Value : DBNull.Value);
                    query.Append(", [ChildFee] = @ChildFee");
                    parameters[2] = Database.GetParameter("@ChildFee", infee.ChildFee.HasValue ? (object)infee.ChildFee.Value : DBNull.Value);
                    query.Append(", [UnitFee] = @UnitFee");
                    parameters[3] = Database.GetParameter("@UnitFee", infee.UnitFee.HasValue ? (object)infee.UnitFee.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INFee].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", infee.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INFee] ([LA1Fee], [LA2Fee], [ChildFee], [UnitFee], [StampDate], [StampUserID]) VALUES(@LA1Fee, @LA2Fee, @ChildFee, @UnitFee, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@LA1Fee", infee.LA1Fee.HasValue ? (object)infee.LA1Fee.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@LA2Fee", infee.LA2Fee.HasValue ? (object)infee.LA2Fee.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@ChildFee", infee.ChildFee.HasValue ? (object)infee.ChildFee.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@UnitFee", infee.UnitFee.HasValue ? (object)infee.UnitFee.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for infees that match the search criteria.
        /// </summary>
        /// <param name="la1fee">The la1fee search criteria.</param>
        /// <param name="la2fee">The la2fee search criteria.</param>
        /// <param name="childfee">The childfee search criteria.</param>
        /// <param name="unitfee">The unitfee search criteria.</param>
        /// <returns>A query that can be used to search for infees based on the search criteria.</returns>
        internal static string Search(decimal? la1fee, decimal? la2fee, decimal? childfee, decimal? unitfee)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (la1fee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFee].[LA1Fee] = " + la1fee + "");
            }
            if (la2fee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFee].[LA2Fee] = " + la2fee + "");
            }
            if (childfee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFee].[ChildFee] = " + childfee + "");
            }
            if (unitfee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INFee].[UnitFee] = " + unitfee + "");
            }
            query.Append("SELECT [INFee].[ID], [INFee].[LA1Fee], [INFee].[LA2Fee], [INFee].[ChildFee], [INFee].[UnitFee], [INFee].[StampDate], [INFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INFee] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
