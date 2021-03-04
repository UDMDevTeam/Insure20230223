using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inoptionfee objects.
    /// </summary>
    internal abstract partial class INOptionFeeQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inoptionfee from the database.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee object to delete.</param>
        /// <returns>A query that can be used to delete the inoptionfee from the database.</returns>
        internal static string Delete(INOptionFee inoptionfee, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionfee != null)
            {
                query = "INSERT INTO [zHstINOptionFee] ([ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID] FROM [INOptionFee] WHERE [INOptionFee].[ID] = @ID; ";
                query += "DELETE FROM [INOptionFee] WHERE [INOptionFee].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionfee.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inoptionfee from the database.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inoptionfee from the database.</returns>
        internal static string DeleteHistory(INOptionFee inoptionfee, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionfee != null)
            {
                query = "DELETE FROM [zHstINOptionFee] WHERE [zHstINOptionFee].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionfee.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inoptionfee from the database.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee object to undelete.</param>
        /// <returns>A query that can be used to undelete the inoptionfee from the database.</returns>
        internal static string UnDelete(INOptionFee inoptionfee, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionfee != null)
            {
                query = "INSERT INTO [INOptionFee] ([ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID] FROM [zHstINOptionFee] WHERE [zHstINOptionFee].[ID] = @ID AND [zHstINOptionFee].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINOptionFee] WHERE [zHstINOptionFee].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INOptionFee] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINOptionFee] WHERE [zHstINOptionFee].[ID] = @ID AND [zHstINOptionFee].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINOptionFee] WHERE [zHstINOptionFee].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INOptionFee] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionfee.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inoptionfee object.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee object to fill.</param>
        /// <returns>A query that can be used to fill the inoptionfee object.</returns>
        internal static string Fill(INOptionFee inoptionfee, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionfee != null)
            {
                query = "SELECT [ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID] FROM [INOptionFee] WHERE [INOptionFee].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionfee.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inoptionfee data.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inoptionfee data.</returns>
        internal static string FillData(INOptionFee inoptionfee, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoptionfee != null)
            {
            query.Append("SELECT [INOptionFee].[ID], [INOptionFee].[FKINOptionID], [INOptionFee].[FKINFeeID], [INOptionFee].[Date], [INOptionFee].[IsActive], [INOptionFee].[StampDate], [INOptionFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOptionFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOptionFee] ");
                query.Append(" WHERE [INOptionFee].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionfee.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inoptionfee object from history.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inoptionfee object from history.</returns>
        internal static string FillHistory(INOptionFee inoptionfee, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoptionfee != null)
            {
                query = "SELECT [ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID] FROM [zHstINOptionFee] WHERE [zHstINOptionFee].[ID] = @ID AND [zHstINOptionFee].[StampUserID] = @StampUserID AND [zHstINOptionFee].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inoptionfee.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inoptionfees in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inoptionfees in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INOptionFee].[ID], [INOptionFee].[FKINOptionID], [INOptionFee].[FKINFeeID], [INOptionFee].[Date], [INOptionFee].[IsActive], [INOptionFee].[StampDate], [INOptionFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOptionFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOptionFee] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inoptionfees in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inoptionfees in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINOptionFee].[ID], [zHstINOptionFee].[FKINOptionID], [zHstINOptionFee].[FKINFeeID], [zHstINOptionFee].[Date], [zHstINOptionFee].[IsActive], [zHstINOptionFee].[StampDate], [zHstINOptionFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINOptionFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINOptionFee] ");
            query.Append("INNER JOIN (SELECT [zHstINOptionFee].[ID], MAX([zHstINOptionFee].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINOptionFee] ");
            query.Append("WHERE [zHstINOptionFee].[ID] NOT IN (SELECT [INOptionFee].[ID] FROM [INOptionFee]) ");
            query.Append("GROUP BY [zHstINOptionFee].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINOptionFee].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINOptionFee].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inoptionfee in the database.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inoptionfee in the database.</returns>
        public static string ListHistory(INOptionFee inoptionfee, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoptionfee != null)
            {
            query.Append("SELECT [zHstINOptionFee].[ID], [zHstINOptionFee].[FKINOptionID], [zHstINOptionFee].[FKINFeeID], [zHstINOptionFee].[Date], [zHstINOptionFee].[IsActive], [zHstINOptionFee].[StampDate], [zHstINOptionFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINOptionFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINOptionFee] ");
                query.Append(" WHERE [zHstINOptionFee].[ID] = @ID");
                query.Append(" ORDER BY [zHstINOptionFee].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoptionfee.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inoptionfee to the database.
        /// </summary>
        /// <param name="inoptionfee">The inoptionfee to save.</param>
        /// <returns>A query that can be used to save the inoptionfee to the database.</returns>
        internal static string Save(INOptionFee inoptionfee, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoptionfee != null)
            {
                if (inoptionfee.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINOptionFee] ([ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID] FROM [INOptionFee] WHERE [INOptionFee].[ID] = @ID; ");
                    query.Append("UPDATE [INOptionFee]");
                    parameters = new object[5];
                    query.Append(" SET [FKINOptionID] = @FKINOptionID");
                    parameters[0] = Database.GetParameter("@FKINOptionID", inoptionfee.FKINOptionID);
                    query.Append(", [FKINFeeID] = @FKINFeeID");
                    parameters[1] = Database.GetParameter("@FKINFeeID", inoptionfee.FKINFeeID);
                    query.Append(", [Date] = @Date");
                    parameters[2] = Database.GetParameter("@Date", inoptionfee.Date);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[3] = Database.GetParameter("@IsActive", inoptionfee.IsActive);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INOptionFee].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", inoptionfee.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INOptionFee] ([FKINOptionID], [FKINFeeID], [Date], [IsActive], [StampDate], [StampUserID]) VALUES(@FKINOptionID, @FKINFeeID, @Date, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKINOptionID", inoptionfee.FKINOptionID);
                    parameters[1] = Database.GetParameter("@FKINFeeID", inoptionfee.FKINFeeID);
                    parameters[2] = Database.GetParameter("@Date", inoptionfee.Date);
                    parameters[3] = Database.GetParameter("@IsActive", inoptionfee.IsActive);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inoptionfees that match the search criteria.
        /// </summary>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinfeeid">The fkinfeeid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for inoptionfees based on the search criteria.</returns>
        internal static string Search(long? fkinoptionid, long? fkinfeeid, DateTime? date, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinoptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionFee].[FKINOptionID] = " + fkinoptionid + "");
            }
            if (fkinfeeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionFee].[FKINFeeID] = " + fkinfeeid + "");
            }
            if (date != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionFee].[Date] = '" + date.Value.ToString(Database.DateFormat) + "'");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOptionFee].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INOptionFee].[ID], [INOptionFee].[FKINOptionID], [INOptionFee].[FKINFeeID], [INOptionFee].[Date], [INOptionFee].[IsActive], [INOptionFee].[StampDate], [INOptionFee].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOptionFee].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOptionFee] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
