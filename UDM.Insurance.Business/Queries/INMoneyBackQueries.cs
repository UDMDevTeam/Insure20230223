using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inmoneyback objects.
    /// </summary>
    internal abstract partial class INMoneyBackQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inmoneyback from the database.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback object to delete.</param>
        /// <returns>A query that can be used to delete the inmoneyback from the database.</returns>
        internal static string Delete(INMoneyBack inmoneyback, ref object[] parameters)
        {
            string query = string.Empty;
            if (inmoneyback != null)
            {
                query = "INSERT INTO [zHstINMoneyBack] ([ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID] FROM [INMoneyBack] WHERE [INMoneyBack].[ID] = @ID; ";
                query += "DELETE FROM [INMoneyBack] WHERE [INMoneyBack].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inmoneyback.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inmoneyback from the database.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inmoneyback from the database.</returns>
        internal static string DeleteHistory(INMoneyBack inmoneyback, ref object[] parameters)
        {
            string query = string.Empty;
            if (inmoneyback != null)
            {
                query = "DELETE FROM [zHstINMoneyBack] WHERE [zHstINMoneyBack].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inmoneyback.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inmoneyback from the database.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback object to undelete.</param>
        /// <returns>A query that can be used to undelete the inmoneyback from the database.</returns>
        internal static string UnDelete(INMoneyBack inmoneyback, ref object[] parameters)
        {
            string query = string.Empty;
            if (inmoneyback != null)
            {
                query = "INSERT INTO [INMoneyBack] ([ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID] FROM [zHstINMoneyBack] WHERE [zHstINMoneyBack].[ID] = @ID AND [zHstINMoneyBack].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINMoneyBack] WHERE [zHstINMoneyBack].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INMoneyBack] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINMoneyBack] WHERE [zHstINMoneyBack].[ID] = @ID AND [zHstINMoneyBack].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINMoneyBack] WHERE [zHstINMoneyBack].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INMoneyBack] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inmoneyback.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inmoneyback object.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback object to fill.</param>
        /// <returns>A query that can be used to fill the inmoneyback object.</returns>
        internal static string Fill(INMoneyBack inmoneyback, ref object[] parameters)
        {
            string query = string.Empty;
            if (inmoneyback != null)
            {
                query = "SELECT [ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID] FROM [INMoneyBack] WHERE [INMoneyBack].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inmoneyback.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inmoneyback data.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inmoneyback data.</returns>
        internal static string FillData(INMoneyBack inmoneyback, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inmoneyback != null)
            {
            query.Append("SELECT [INMoneyBack].[ID], [INMoneyBack].[FKINPolicyTypeID], [INMoneyBack].[FKINOptionID], [INMoneyBack].[AgeMin], [INMoneyBack].[AgeMax], [INMoneyBack].[Value], [INMoneyBack].[IsActive], [INMoneyBack].[StampDate], [INMoneyBack].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMoneyBack].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMoneyBack] ");
                query.Append(" WHERE [INMoneyBack].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inmoneyback.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inmoneyback object from history.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inmoneyback object from history.</returns>
        internal static string FillHistory(INMoneyBack inmoneyback, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inmoneyback != null)
            {
                query = "SELECT [ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID] FROM [zHstINMoneyBack] WHERE [zHstINMoneyBack].[ID] = @ID AND [zHstINMoneyBack].[StampUserID] = @StampUserID AND [zHstINMoneyBack].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inmoneyback.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inmoneybacks in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inmoneybacks in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INMoneyBack].[ID], [INMoneyBack].[FKINPolicyTypeID], [INMoneyBack].[FKINOptionID], [INMoneyBack].[AgeMin], [INMoneyBack].[AgeMax], [INMoneyBack].[Value], [INMoneyBack].[IsActive], [INMoneyBack].[StampDate], [INMoneyBack].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMoneyBack].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMoneyBack] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inmoneybacks in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inmoneybacks in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINMoneyBack].[ID], [zHstINMoneyBack].[FKINPolicyTypeID], [zHstINMoneyBack].[FKINOptionID], [zHstINMoneyBack].[AgeMin], [zHstINMoneyBack].[AgeMax], [zHstINMoneyBack].[Value], [zHstINMoneyBack].[IsActive], [zHstINMoneyBack].[StampDate], [zHstINMoneyBack].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINMoneyBack].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINMoneyBack] ");
            query.Append("INNER JOIN (SELECT [zHstINMoneyBack].[ID], MAX([zHstINMoneyBack].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINMoneyBack] ");
            query.Append("WHERE [zHstINMoneyBack].[ID] NOT IN (SELECT [INMoneyBack].[ID] FROM [INMoneyBack]) ");
            query.Append("GROUP BY [zHstINMoneyBack].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINMoneyBack].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINMoneyBack].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inmoneyback in the database.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inmoneyback in the database.</returns>
        public static string ListHistory(INMoneyBack inmoneyback, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inmoneyback != null)
            {
            query.Append("SELECT [zHstINMoneyBack].[ID], [zHstINMoneyBack].[FKINPolicyTypeID], [zHstINMoneyBack].[FKINOptionID], [zHstINMoneyBack].[AgeMin], [zHstINMoneyBack].[AgeMax], [zHstINMoneyBack].[Value], [zHstINMoneyBack].[IsActive], [zHstINMoneyBack].[StampDate], [zHstINMoneyBack].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINMoneyBack].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINMoneyBack] ");
                query.Append(" WHERE [zHstINMoneyBack].[ID] = @ID");
                query.Append(" ORDER BY [zHstINMoneyBack].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inmoneyback.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inmoneyback to the database.
        /// </summary>
        /// <param name="inmoneyback">The inmoneyback to save.</param>
        /// <returns>A query that can be used to save the inmoneyback to the database.</returns>
        internal static string Save(INMoneyBack inmoneyback, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inmoneyback != null)
            {
                if (inmoneyback.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINMoneyBack] ([ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID] FROM [INMoneyBack] WHERE [INMoneyBack].[ID] = @ID; ");
                    query.Append("UPDATE [INMoneyBack]");
                    parameters = new object[7];
                    query.Append(" SET [FKINPolicyTypeID] = @FKINPolicyTypeID");
                    parameters[0] = Database.GetParameter("@FKINPolicyTypeID", inmoneyback.FKINPolicyTypeID.HasValue ? (object)inmoneyback.FKINPolicyTypeID.Value : DBNull.Value);
                    query.Append(", [FKINOptionID] = @FKINOptionID");
                    parameters[1] = Database.GetParameter("@FKINOptionID", inmoneyback.FKINOptionID.HasValue ? (object)inmoneyback.FKINOptionID.Value : DBNull.Value);
                    query.Append(", [AgeMin] = @AgeMin");
                    parameters[2] = Database.GetParameter("@AgeMin", inmoneyback.AgeMin.HasValue ? (object)inmoneyback.AgeMin.Value : DBNull.Value);
                    query.Append(", [AgeMax] = @AgeMax");
                    parameters[3] = Database.GetParameter("@AgeMax", inmoneyback.AgeMax.HasValue ? (object)inmoneyback.AgeMax.Value : DBNull.Value);
                    query.Append(", [Value] = @Value");
                    parameters[4] = Database.GetParameter("@Value", inmoneyback.Value.HasValue ? (object)inmoneyback.Value.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[5] = Database.GetParameter("@IsActive", inmoneyback.IsActive.HasValue ? (object)inmoneyback.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INMoneyBack].[ID] = @ID"); 
                    parameters[6] = Database.GetParameter("@ID", inmoneyback.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INMoneyBack] ([FKINPolicyTypeID], [FKINOptionID], [AgeMin], [AgeMax], [Value], [IsActive], [StampDate], [StampUserID]) VALUES(@FKINPolicyTypeID, @FKINOptionID, @AgeMin, @AgeMax, @Value, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[6];
                    parameters[0] = Database.GetParameter("@FKINPolicyTypeID", inmoneyback.FKINPolicyTypeID.HasValue ? (object)inmoneyback.FKINPolicyTypeID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINOptionID", inmoneyback.FKINOptionID.HasValue ? (object)inmoneyback.FKINOptionID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@AgeMin", inmoneyback.AgeMin.HasValue ? (object)inmoneyback.AgeMin.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@AgeMax", inmoneyback.AgeMax.HasValue ? (object)inmoneyback.AgeMax.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@Value", inmoneyback.Value.HasValue ? (object)inmoneyback.Value.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@IsActive", inmoneyback.IsActive.HasValue ? (object)inmoneyback.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inmoneybacks that match the search criteria.
        /// </summary>
        /// <param name="fkinpolicytypeid">The fkinpolicytypeid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="value">The value search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for inmoneybacks based on the search criteria.</returns>
        internal static string Search(long? fkinpolicytypeid, long? fkinoptionid, short? agemin, short? agemax, decimal? value, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinpolicytypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMoneyBack].[FKINPolicyTypeID] = " + fkinpolicytypeid + "");
            }
            if (fkinoptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMoneyBack].[FKINOptionID] = " + fkinoptionid + "");
            }
            if (agemin != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMoneyBack].[AgeMin] = " + agemin + "");
            }
            if (agemax != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMoneyBack].[AgeMax] = " + agemax + "");
            }
            if (value != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMoneyBack].[Value] = " + value + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INMoneyBack].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INMoneyBack].[ID], [INMoneyBack].[FKINPolicyTypeID], [INMoneyBack].[FKINOptionID], [INMoneyBack].[AgeMin], [INMoneyBack].[AgeMax], [INMoneyBack].[Value], [INMoneyBack].[IsActive], [INMoneyBack].[StampDate], [INMoneyBack].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INMoneyBack].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INMoneyBack] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
