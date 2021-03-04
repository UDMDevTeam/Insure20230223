using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inplangroup objects.
    /// </summary>
    internal abstract partial class INPlanGroupQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inplangroup from the database.
        /// </summary>
        /// <param name="inplangroup">The inplangroup object to delete.</param>
        /// <returns>A query that can be used to delete the inplangroup from the database.</returns>
        internal static string Delete(INPlanGroup inplangroup, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplangroup != null)
            {
                query = "INSERT INTO [zHstINPlanGroup] ([ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID] FROM [INPlanGroup] WHERE [INPlanGroup].[ID] = @ID; ";
                query += "DELETE FROM [INPlanGroup] WHERE [INPlanGroup].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplangroup.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inplangroup from the database.
        /// </summary>
        /// <param name="inplangroup">The inplangroup object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inplangroup from the database.</returns>
        internal static string DeleteHistory(INPlanGroup inplangroup, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplangroup != null)
            {
                query = "DELETE FROM [zHstINPlanGroup] WHERE [zHstINPlanGroup].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplangroup.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inplangroup from the database.
        /// </summary>
        /// <param name="inplangroup">The inplangroup object to undelete.</param>
        /// <returns>A query that can be used to undelete the inplangroup from the database.</returns>
        internal static string UnDelete(INPlanGroup inplangroup, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplangroup != null)
            {
                query = "INSERT INTO [INPlanGroup] ([ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID] FROM [zHstINPlanGroup] WHERE [zHstINPlanGroup].[ID] = @ID AND [zHstINPlanGroup].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPlanGroup] WHERE [zHstINPlanGroup].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPlanGroup] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPlanGroup] WHERE [zHstINPlanGroup].[ID] = @ID AND [zHstINPlanGroup].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPlanGroup] WHERE [zHstINPlanGroup].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPlanGroup] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplangroup.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inplangroup object.
        /// </summary>
        /// <param name="inplangroup">The inplangroup object to fill.</param>
        /// <returns>A query that can be used to fill the inplangroup object.</returns>
        internal static string Fill(INPlanGroup inplangroup, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplangroup != null)
            {
                query = "SELECT [ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID] FROM [INPlanGroup] WHERE [INPlanGroup].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplangroup.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inplangroup data.
        /// </summary>
        /// <param name="inplangroup">The inplangroup to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inplangroup data.</returns>
        internal static string FillData(INPlanGroup inplangroup, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inplangroup != null)
            {
            query.Append("SELECT [INPlanGroup].[ID], [INPlanGroup].[Description], [INPlanGroup].[FKINCampaignTypeID], [INPlanGroup].[FKINCampaignGroupID], [INPlanGroup].[Date], [INPlanGroup].[PolicyFee], [INPlanGroup].[IsActive], [INPlanGroup].[StampDate], [INPlanGroup].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPlanGroup].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPlanGroup] ");
                query.Append(" WHERE [INPlanGroup].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplangroup.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inplangroup object from history.
        /// </summary>
        /// <param name="inplangroup">The inplangroup object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inplangroup object from history.</returns>
        internal static string FillHistory(INPlanGroup inplangroup, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inplangroup != null)
            {
                query = "SELECT [ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID] FROM [zHstINPlanGroup] WHERE [zHstINPlanGroup].[ID] = @ID AND [zHstINPlanGroup].[StampUserID] = @StampUserID AND [zHstINPlanGroup].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inplangroup.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inplangroups in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inplangroups in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INPlanGroup].[ID], [INPlanGroup].[Description], [INPlanGroup].[FKINCampaignTypeID], [INPlanGroup].[FKINCampaignGroupID], [INPlanGroup].[Date], [INPlanGroup].[PolicyFee], [INPlanGroup].[IsActive], [INPlanGroup].[StampDate], [INPlanGroup].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPlanGroup].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPlanGroup] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inplangroups in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inplangroups in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPlanGroup].[ID], [zHstINPlanGroup].[Description], [zHstINPlanGroup].[FKINCampaignTypeID], [zHstINPlanGroup].[FKINCampaignGroupID], [zHstINPlanGroup].[Date], [zHstINPlanGroup].[PolicyFee], [zHstINPlanGroup].[IsActive], [zHstINPlanGroup].[StampDate], [zHstINPlanGroup].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPlanGroup].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPlanGroup] ");
            query.Append("INNER JOIN (SELECT [zHstINPlanGroup].[ID], MAX([zHstINPlanGroup].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPlanGroup] ");
            query.Append("WHERE [zHstINPlanGroup].[ID] NOT IN (SELECT [INPlanGroup].[ID] FROM [INPlanGroup]) ");
            query.Append("GROUP BY [zHstINPlanGroup].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPlanGroup].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPlanGroup].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inplangroup in the database.
        /// </summary>
        /// <param name="inplangroup">The inplangroup object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inplangroup in the database.</returns>
        public static string ListHistory(INPlanGroup inplangroup, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inplangroup != null)
            {
            query.Append("SELECT [zHstINPlanGroup].[ID], [zHstINPlanGroup].[Description], [zHstINPlanGroup].[FKINCampaignTypeID], [zHstINPlanGroup].[FKINCampaignGroupID], [zHstINPlanGroup].[Date], [zHstINPlanGroup].[PolicyFee], [zHstINPlanGroup].[IsActive], [zHstINPlanGroup].[StampDate], [zHstINPlanGroup].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPlanGroup].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPlanGroup] ");
                query.Append(" WHERE [zHstINPlanGroup].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPlanGroup].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inplangroup.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inplangroup to the database.
        /// </summary>
        /// <param name="inplangroup">The inplangroup to save.</param>
        /// <returns>A query that can be used to save the inplangroup to the database.</returns>
        internal static string Save(INPlanGroup inplangroup, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inplangroup != null)
            {
                if (inplangroup.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPlanGroup] ([ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID] FROM [INPlanGroup] WHERE [INPlanGroup].[ID] = @ID; ");
                    query.Append("UPDATE [INPlanGroup]");
                    parameters = new object[7];
                    query.Append(" SET [Description] = @Description");
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inplangroup.Description) ? DBNull.Value : (object)inplangroup.Description);
                    query.Append(", [FKINCampaignTypeID] = @FKINCampaignTypeID");
                    parameters[1] = Database.GetParameter("@FKINCampaignTypeID", inplangroup.FKINCampaignTypeID.HasValue ? (object)inplangroup.FKINCampaignTypeID.Value : DBNull.Value);
                    query.Append(", [FKINCampaignGroupID] = @FKINCampaignGroupID");
                    parameters[2] = Database.GetParameter("@FKINCampaignGroupID", inplangroup.FKINCampaignGroupID.HasValue ? (object)inplangroup.FKINCampaignGroupID.Value : DBNull.Value);
                    query.Append(", [Date] = @Date");
                    parameters[3] = Database.GetParameter("@Date", inplangroup.Date.HasValue ? (object)inplangroup.Date.Value : DBNull.Value);
                    query.Append(", [PolicyFee] = @PolicyFee");
                    parameters[4] = Database.GetParameter("@PolicyFee", inplangroup.PolicyFee.HasValue ? (object)inplangroup.PolicyFee.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[5] = Database.GetParameter("@IsActive", inplangroup.IsActive.HasValue ? (object)inplangroup.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPlanGroup].[ID] = @ID"); 
                    parameters[6] = Database.GetParameter("@ID", inplangroup.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPlanGroup] ([Description], [FKINCampaignTypeID], [FKINCampaignGroupID], [Date], [PolicyFee], [IsActive], [StampDate], [StampUserID]) VALUES(@Description, @FKINCampaignTypeID, @FKINCampaignGroupID, @Date, @PolicyFee, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[6];
                    parameters[0] = Database.GetParameter("@Description", string.IsNullOrEmpty(inplangroup.Description) ? DBNull.Value : (object)inplangroup.Description);
                    parameters[1] = Database.GetParameter("@FKINCampaignTypeID", inplangroup.FKINCampaignTypeID.HasValue ? (object)inplangroup.FKINCampaignTypeID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKINCampaignGroupID", inplangroup.FKINCampaignGroupID.HasValue ? (object)inplangroup.FKINCampaignGroupID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@Date", inplangroup.Date.HasValue ? (object)inplangroup.Date.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@PolicyFee", inplangroup.PolicyFee.HasValue ? (object)inplangroup.PolicyFee.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@IsActive", inplangroup.IsActive.HasValue ? (object)inplangroup.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inplangroups that match the search criteria.
        /// </summary>
        /// <param name="description">The description search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="date">The date search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for inplangroups based on the search criteria.</returns>
        internal static string Search(string description, long? fkincampaigntypeid, long? fkincampaigngroupid, DateTime? date, decimal? policyfee, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (description != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlanGroup].[Description] LIKE '" + description.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkincampaigntypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlanGroup].[FKINCampaignTypeID] = " + fkincampaigntypeid + "");
            }
            if (fkincampaigngroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlanGroup].[FKINCampaignGroupID] = " + fkincampaigngroupid + "");
            }
            if (date != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlanGroup].[Date] = '" + date.Value.ToString(Database.DateFormat) + "'");
            }
            if (policyfee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlanGroup].[PolicyFee] = " + policyfee + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPlanGroup].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INPlanGroup].[ID], [INPlanGroup].[Description], [INPlanGroup].[FKINCampaignTypeID], [INPlanGroup].[FKINCampaignGroupID], [INPlanGroup].[Date], [INPlanGroup].[PolicyFee], [INPlanGroup].[IsActive], [INPlanGroup].[StampDate], [INPlanGroup].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPlanGroup].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPlanGroup] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
