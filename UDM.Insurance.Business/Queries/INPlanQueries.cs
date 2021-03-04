using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inplan objects.
    /// </summary>
    internal abstract partial class INPlanQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inplan from the database.
        /// </summary>
        /// <param name="inplan">The inplan object to delete.</param>
        /// <returns>A query that can be used to delete the inplan from the database.</returns>
        internal static string Delete(INPlan inplan, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplan != null)
            {
                query = "INSERT INTO [zHstINPlan] ([ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID] FROM [INPlan] WHERE [INPlan].[ID] = @ID; ";
                query += "DELETE FROM [INPlan] WHERE [INPlan].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplan.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inplan from the database.
        /// </summary>
        /// <param name="inplan">The inplan object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inplan from the database.</returns>
        internal static string DeleteHistory(INPlan inplan, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplan != null)
            {
                query = "DELETE FROM [zHstINPlan] WHERE [zHstINPlan].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplan.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inplan from the database.
        /// </summary>
        /// <param name="inplan">The inplan object to undelete.</param>
        /// <returns>A query that can be used to undelete the inplan from the database.</returns>
        internal static string UnDelete(INPlan inplan, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplan != null)
            {
                query = "INSERT INTO [INPlan] ([ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID] FROM [zHstINPlan] WHERE [zHstINPlan].[ID] = @ID AND [zHstINPlan].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPlan] WHERE [zHstINPlan].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPlan] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPlan] WHERE [zHstINPlan].[ID] = @ID AND [zHstINPlan].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPlan] WHERE [zHstINPlan].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPlan] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplan.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inplan object.
        /// </summary>
        /// <param name="inplan">The inplan object to fill.</param>
        /// <returns>A query that can be used to fill the inplan object.</returns>
        internal static string Fill(INPlan inplan, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplan != null)
            {
                query = "SELECT [ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID] FROM [INPlan] WHERE [INPlan].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplan.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inplan data.
        /// </summary>
        /// <param name="inplan">The inplan to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inplan data.</returns>
        internal static string FillData(INPlan inplan, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inplan != null)
            {
            query.Append("SELECT [INPlan].[ID], [INPlan].[FKINPlanGroupID], [INPlan].[PlanCode], [INPlan].[AgeMin], [INPlan].[AgeMax], [INPlan].[IGFreeCover], [INPlan].[IsActive], [INPlan].[StampDate], [INPlan].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPlan].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPlan] ");
                query.Append(" WHERE [INPlan].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplan.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inplan object from history.
        /// </summary>
        /// <param name="inplan">The inplan object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inplan object from history.</returns>
        internal static string FillHistory(INPlan inplan, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplan != null)
            {
                query = "SELECT [ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID] FROM [zHstINPlan] WHERE [zHstINPlan].[ID] = @ID AND [zHstINPlan].[StampUserID] = @StampUserID AND [zHstINPlan].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inplan.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inplans in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inplans in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INPlan].[ID], [INPlan].[FKINPlanGroupID], [INPlan].[PlanCode], [INPlan].[AgeMin], [INPlan].[AgeMax], [INPlan].[IGFreeCover], [INPlan].[IsActive], [INPlan].[StampDate], [INPlan].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPlan].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPlan] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inplans in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inplans in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPlan].[ID], [zHstINPlan].[FKINPlanGroupID], [zHstINPlan].[PlanCode], [zHstINPlan].[AgeMin], [zHstINPlan].[AgeMax], [zHstINPlan].[IGFreeCover], [zHstINPlan].[IsActive], [zHstINPlan].[StampDate], [zHstINPlan].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPlan].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPlan] ");
            query.Append("INNER JOIN (SELECT [zHstINPlan].[ID], MAX([zHstINPlan].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPlan] ");
            query.Append("WHERE [zHstINPlan].[ID] NOT IN (SELECT [INPlan].[ID] FROM [INPlan]) ");
            query.Append("GROUP BY [zHstINPlan].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPlan].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPlan].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inplan in the database.
        /// </summary>
        /// <param name="inplan">The inplan object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inplan in the database.</returns>
        public static string ListHistory(INPlan inplan, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inplan != null)
            {
            query.Append("SELECT [zHstINPlan].[ID], [zHstINPlan].[FKINPlanGroupID], [zHstINPlan].[PlanCode], [zHstINPlan].[AgeMin], [zHstINPlan].[AgeMax], [zHstINPlan].[IGFreeCover], [zHstINPlan].[IsActive], [zHstINPlan].[StampDate], [zHstINPlan].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPlan].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPlan] ");
                query.Append(" WHERE [zHstINPlan].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPlan].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplan.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inplan to the database.
        /// </summary>
        /// <param name="inplan">The inplan to save.</param>
        /// <returns>A query that can be used to save the inplan to the database.</returns>
        internal static string Save(INPlan inplan, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inplan != null)
            {
                if (inplan.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPlan] ([ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID] FROM [INPlan] WHERE [INPlan].[ID] = @ID; ");
                    query.Append("UPDATE [INPlan]");
                    parameters = new object[7];
                    query.Append(" SET [FKINPlanGroupID] = @FKINPlanGroupID");
                    parameters[0] = Database.GetParameter("@FKINPlanGroupID", inplan.FKINPlanGroupID.HasValue ? (object)inplan.FKINPlanGroupID.Value : DBNull.Value);
                    query.Append(", [PlanCode] = @PlanCode");
                    parameters[1] = Database.GetParameter("@PlanCode", string.IsNullOrEmpty(inplan.PlanCode) ? DBNull.Value : (object)inplan.PlanCode);
                    query.Append(", [AgeMin] = @AgeMin");
                    parameters[2] = Database.GetParameter("@AgeMin", inplan.AgeMin.HasValue ? (object)inplan.AgeMin.Value : DBNull.Value);
                    query.Append(", [AgeMax] = @AgeMax");
                    parameters[3] = Database.GetParameter("@AgeMax", inplan.AgeMax.HasValue ? (object)inplan.AgeMax.Value : DBNull.Value);
                    query.Append(", [IGFreeCover] = @IGFreeCover");
                    parameters[4] = Database.GetParameter("@IGFreeCover", inplan.IGFreeCover.HasValue ? (object)inplan.IGFreeCover.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[5] = Database.GetParameter("@IsActive", inplan.IsActive.HasValue ? (object)inplan.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPlan].[ID] = @ID"); 
                    parameters[6] = Database.GetParameter("@ID", inplan.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPlan] ([FKINPlanGroupID], [PlanCode], [AgeMin], [AgeMax], [IGFreeCover], [IsActive], [StampDate], [StampUserID]) VALUES(@FKINPlanGroupID, @PlanCode, @AgeMin, @AgeMax, @IGFreeCover, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[6];
                    parameters[0] = Database.GetParameter("@FKINPlanGroupID", inplan.FKINPlanGroupID.HasValue ? (object)inplan.FKINPlanGroupID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@PlanCode", string.IsNullOrEmpty(inplan.PlanCode) ? DBNull.Value : (object)inplan.PlanCode);
                    parameters[2] = Database.GetParameter("@AgeMin", inplan.AgeMin.HasValue ? (object)inplan.AgeMin.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@AgeMax", inplan.AgeMax.HasValue ? (object)inplan.AgeMax.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@IGFreeCover", inplan.IGFreeCover.HasValue ? (object)inplan.IGFreeCover.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@IsActive", inplan.IsActive.HasValue ? (object)inplan.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inplans that match the search criteria.
        /// </summary>
        /// <param name="fkinplangroupid">The fkinplangroupid search criteria.</param>
        /// <param name="plancode">The plancode search criteria.</param>
        /// <param name="agemin">The agemin search criteria.</param>
        /// <param name="agemax">The agemax search criteria.</param>
        /// <param name="igfreecover">The igfreecover search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for inplans based on the search criteria.</returns>
        internal static string Search(long? fkinplangroupid, string plancode, short? agemin, short? agemax, decimal? igfreecover, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinplangroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlan].[FKINPlanGroupID] = " + fkinplangroupid + "");
            }
            if (plancode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlan].[PlanCode] LIKE '" + plancode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (agemin != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlan].[AgeMin] = " + agemin + "");
            }
            if (agemax != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlan].[AgeMax] = " + agemax + "");
            }
            if (igfreecover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlan].[IGFreeCover] = " + igfreecover + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlan].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INPlan].[ID], [INPlan].[FKINPlanGroupID], [INPlan].[PlanCode], [INPlan].[AgeMin], [INPlan].[AgeMax], [INPlan].[IGFreeCover], [INPlan].[IsActive], [INPlan].[StampDate], [INPlan].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPlan].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPlan] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
