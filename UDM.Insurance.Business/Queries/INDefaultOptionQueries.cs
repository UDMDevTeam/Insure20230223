using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to indefaultoption objects.
    /// </summary>
    internal abstract partial class INDefaultOptionQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) indefaultoption from the database.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption object to delete.</param>
        /// <returns>A query that can be used to delete the indefaultoption from the database.</returns>
        internal static string Delete(INDefaultOption indefaultoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (indefaultoption != null)
            {
                query = "INSERT INTO [zHstINDefaultOption] ([ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID] FROM [INDefaultOption] WHERE [INDefaultOption].[ID] = @ID; ";
                query += "DELETE FROM [INDefaultOption] WHERE [INDefaultOption].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indefaultoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) indefaultoption from the database.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the indefaultoption from the database.</returns>
        internal static string DeleteHistory(INDefaultOption indefaultoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (indefaultoption != null)
            {
                query = "DELETE FROM [zHstINDefaultOption] WHERE [zHstINDefaultOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indefaultoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) indefaultoption from the database.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption object to undelete.</param>
        /// <returns>A query that can be used to undelete the indefaultoption from the database.</returns>
        internal static string UnDelete(INDefaultOption indefaultoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (indefaultoption != null)
            {
                query = "INSERT INTO [INDefaultOption] ([ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID] FROM [zHstINDefaultOption] WHERE [zHstINDefaultOption].[ID] = @ID AND [zHstINDefaultOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDefaultOption] WHERE [zHstINDefaultOption].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INDefaultOption] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINDefaultOption] WHERE [zHstINDefaultOption].[ID] = @ID AND [zHstINDefaultOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINDefaultOption] WHERE [zHstINDefaultOption].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INDefaultOption] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indefaultoption.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an indefaultoption object.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption object to fill.</param>
        /// <returns>A query that can be used to fill the indefaultoption object.</returns>
        internal static string Fill(INDefaultOption indefaultoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (indefaultoption != null)
            {
                query = "SELECT [ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID] FROM [INDefaultOption] WHERE [INDefaultOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indefaultoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  indefaultoption data.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  indefaultoption data.</returns>
        internal static string FillData(INDefaultOption indefaultoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indefaultoption != null)
            {
            query.Append("SELECT [INDefaultOption].[ID], [INDefaultOption].[FKINCampaign], [INDefaultOption].[FKINPlanID], [INDefaultOption].[FKHigherOptionID], [INDefaultOption].[FKLowerOptionID], [INDefaultOption].[IsActive], [INDefaultOption].[StampDate], [INDefaultOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDefaultOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDefaultOption] ");
                query.Append(" WHERE [INDefaultOption].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indefaultoption.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an indefaultoption object from history.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the indefaultoption object from history.</returns>
        internal static string FillHistory(INDefaultOption indefaultoption, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (indefaultoption != null)
            {
                query = "SELECT [ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID] FROM [zHstINDefaultOption] WHERE [zHstINDefaultOption].[ID] = @ID AND [zHstINDefaultOption].[StampUserID] = @StampUserID AND [zHstINDefaultOption].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", indefaultoption.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the indefaultoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the indefaultoptions in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INDefaultOption].[ID], [INDefaultOption].[FKINCampaign], [INDefaultOption].[FKINPlanID], [INDefaultOption].[FKHigherOptionID], [INDefaultOption].[FKLowerOptionID], [INDefaultOption].[IsActive], [INDefaultOption].[StampDate], [INDefaultOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDefaultOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDefaultOption] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted indefaultoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted indefaultoptions in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINDefaultOption].[ID], [zHstINDefaultOption].[FKINCampaign], [zHstINDefaultOption].[FKINPlanID], [zHstINDefaultOption].[FKHigherOptionID], [zHstINDefaultOption].[FKLowerOptionID], [zHstINDefaultOption].[IsActive], [zHstINDefaultOption].[StampDate], [zHstINDefaultOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDefaultOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDefaultOption] ");
            query.Append("INNER JOIN (SELECT [zHstINDefaultOption].[ID], MAX([zHstINDefaultOption].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINDefaultOption] ");
            query.Append("WHERE [zHstINDefaultOption].[ID] NOT IN (SELECT [INDefaultOption].[ID] FROM [INDefaultOption]) ");
            query.Append("GROUP BY [zHstINDefaultOption].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINDefaultOption].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINDefaultOption].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) indefaultoption in the database.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) indefaultoption in the database.</returns>
        public static string ListHistory(INDefaultOption indefaultoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indefaultoption != null)
            {
            query.Append("SELECT [zHstINDefaultOption].[ID], [zHstINDefaultOption].[FKINCampaign], [zHstINDefaultOption].[FKINPlanID], [zHstINDefaultOption].[FKHigherOptionID], [zHstINDefaultOption].[FKLowerOptionID], [zHstINDefaultOption].[IsActive], [zHstINDefaultOption].[StampDate], [zHstINDefaultOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINDefaultOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINDefaultOption] ");
                query.Append(" WHERE [zHstINDefaultOption].[ID] = @ID");
                query.Append(" ORDER BY [zHstINDefaultOption].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", indefaultoption.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) indefaultoption to the database.
        /// </summary>
        /// <param name="indefaultoption">The indefaultoption to save.</param>
        /// <returns>A query that can be used to save the indefaultoption to the database.</returns>
        internal static string Save(INDefaultOption indefaultoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (indefaultoption != null)
            {
                if (indefaultoption.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINDefaultOption] ([ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID] FROM [INDefaultOption] WHERE [INDefaultOption].[ID] = @ID; ");
                    query.Append("UPDATE [INDefaultOption]");
                    parameters = new object[6];
                    query.Append(" SET [FKINCampaign] = @FKINCampaign");
                    parameters[0] = Database.GetParameter("@FKINCampaign", indefaultoption.FKINCampaign.HasValue ? (object)indefaultoption.FKINCampaign.Value : DBNull.Value);
                    query.Append(", [FKINPlanID] = @FKINPlanID");
                    parameters[1] = Database.GetParameter("@FKINPlanID", indefaultoption.FKINPlanID.HasValue ? (object)indefaultoption.FKINPlanID.Value : DBNull.Value);
                    query.Append(", [FKHigherOptionID] = @FKHigherOptionID");
                    parameters[2] = Database.GetParameter("@FKHigherOptionID", indefaultoption.FKHigherOptionID.HasValue ? (object)indefaultoption.FKHigherOptionID.Value : DBNull.Value);
                    query.Append(", [FKLowerOptionID] = @FKLowerOptionID");
                    parameters[3] = Database.GetParameter("@FKLowerOptionID", indefaultoption.FKLowerOptionID.HasValue ? (object)indefaultoption.FKLowerOptionID.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[4] = Database.GetParameter("@IsActive", indefaultoption.IsActive.HasValue ? (object)indefaultoption.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INDefaultOption].[ID] = @ID"); 
                    parameters[5] = Database.GetParameter("@ID", indefaultoption.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INDefaultOption] ([FKINCampaign], [FKINPlanID], [FKHigherOptionID], [FKLowerOptionID], [IsActive], [StampDate], [StampUserID]) VALUES(@FKINCampaign, @FKINPlanID, @FKHigherOptionID, @FKLowerOptionID, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[5];
                    parameters[0] = Database.GetParameter("@FKINCampaign", indefaultoption.FKINCampaign.HasValue ? (object)indefaultoption.FKINCampaign.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINPlanID", indefaultoption.FKINPlanID.HasValue ? (object)indefaultoption.FKINPlanID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@FKHigherOptionID", indefaultoption.FKHigherOptionID.HasValue ? (object)indefaultoption.FKHigherOptionID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@FKLowerOptionID", indefaultoption.FKLowerOptionID.HasValue ? (object)indefaultoption.FKLowerOptionID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@IsActive", indefaultoption.IsActive.HasValue ? (object)indefaultoption.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for indefaultoptions that match the search criteria.
        /// </summary>
        /// <param name="fkincampaign">The fkincampaign search criteria.</param>
        /// <param name="fkinplanid">The fkinplanid search criteria.</param>
        /// <param name="fkhigheroptionid">The fkhigheroptionid search criteria.</param>
        /// <param name="fkloweroptionid">The fkloweroptionid search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for indefaultoptions based on the search criteria.</returns>
        internal static string Search(long? fkincampaign, long? fkinplanid, long? fkhigheroptionid, long? fkloweroptionid, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkincampaign != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDefaultOption].[FKINCampaign] = " + fkincampaign + "");
            }
            if (fkinplanid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDefaultOption].[FKINPlanID] = " + fkinplanid + "");
            }
            if (fkhigheroptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDefaultOption].[FKHigherOptionID] = " + fkhigheroptionid + "");
            }
            if (fkloweroptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDefaultOption].[FKLowerOptionID] = " + fkloweroptionid + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INDefaultOption].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INDefaultOption].[ID], [INDefaultOption].[FKINCampaign], [INDefaultOption].[FKINPlanID], [INDefaultOption].[FKHigherOptionID], [INDefaultOption].[FKLowerOptionID], [INDefaultOption].[IsActive], [INDefaultOption].[StampDate], [INDefaultOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INDefaultOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INDefaultOption] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
