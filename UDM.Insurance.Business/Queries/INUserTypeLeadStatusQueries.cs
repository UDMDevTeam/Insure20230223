using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inusertypeleadstatus objects.
    /// </summary>
    internal abstract partial class INUserTypeLeadStatusQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inusertypeleadstatus from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus object to delete.</param>
        /// <returns>A query that can be used to delete the inusertypeleadstatus from the database.</returns>
        internal static string Delete(INUserTypeLeadStatus inusertypeleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inusertypeleadstatus != null)
            {
                query = "INSERT INTO [zHstINUserTypeLeadStatus] ([ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID]) SELECT [ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID] FROM [INUserTypeLeadStatus] WHERE [INUserTypeLeadStatus].[ID] = @ID; ";
                query += "DELETE FROM [INUserTypeLeadStatus] WHERE [INUserTypeLeadStatus].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inusertypeleadstatus from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inusertypeleadstatus from the database.</returns>
        internal static string DeleteHistory(INUserTypeLeadStatus inusertypeleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inusertypeleadstatus != null)
            {
                query = "DELETE FROM [zHstINUserTypeLeadStatus] WHERE [zHstINUserTypeLeadStatus].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inusertypeleadstatus from the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus object to undelete.</param>
        /// <returns>A query that can be used to undelete the inusertypeleadstatus from the database.</returns>
        internal static string UnDelete(INUserTypeLeadStatus inusertypeleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inusertypeleadstatus != null)
            {
                query = "INSERT INTO [INUserTypeLeadStatus] ([ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID]) SELECT [ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID] FROM [zHstINUserTypeLeadStatus] WHERE [zHstINUserTypeLeadStatus].[ID] = @ID AND [zHstINUserTypeLeadStatus].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINUserTypeLeadStatus] WHERE [zHstINUserTypeLeadStatus].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INUserTypeLeadStatus] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINUserTypeLeadStatus] WHERE [zHstINUserTypeLeadStatus].[ID] = @ID AND [zHstINUserTypeLeadStatus].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINUserTypeLeadStatus] WHERE [zHstINUserTypeLeadStatus].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INUserTypeLeadStatus] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inusertypeleadstatus object.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus object to fill.</param>
        /// <returns>A query that can be used to fill the inusertypeleadstatus object.</returns>
        internal static string Fill(INUserTypeLeadStatus inusertypeleadstatus, ref object[] parameters)
        {
            string query = string.Empty;
            if (inusertypeleadstatus != null)
            {
                query = "SELECT [ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID] FROM [INUserTypeLeadStatus] WHERE [INUserTypeLeadStatus].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inusertypeleadstatus data.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inusertypeleadstatus data.</returns>
        internal static string FillData(INUserTypeLeadStatus inusertypeleadstatus, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inusertypeleadstatus != null)
            {
            query.Append("SELECT [INUserTypeLeadStatus].[ID], [INUserTypeLeadStatus].[FKUserTypeID], [INUserTypeLeadStatus].[FKLeadStatusID], [INUserTypeLeadStatus].[StampDate], [INUserTypeLeadStatus].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUserTypeLeadStatus].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUserTypeLeadStatus] ");
                query.Append(" WHERE [INUserTypeLeadStatus].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inusertypeleadstatus object from history.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inusertypeleadstatus object from history.</returns>
        internal static string FillHistory(INUserTypeLeadStatus inusertypeleadstatus, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inusertypeleadstatus != null)
            {
                query = "SELECT [ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID] FROM [zHstINUserTypeLeadStatus] WHERE [zHstINUserTypeLeadStatus].[ID] = @ID AND [zHstINUserTypeLeadStatus].[StampUserID] = @StampUserID AND [zHstINUserTypeLeadStatus].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inusertypeleadstatuss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inusertypeleadstatuss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INUserTypeLeadStatus].[ID], [INUserTypeLeadStatus].[FKUserTypeID], [INUserTypeLeadStatus].[FKLeadStatusID], [INUserTypeLeadStatus].[StampDate], [INUserTypeLeadStatus].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUserTypeLeadStatus].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUserTypeLeadStatus] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inusertypeleadstatuss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inusertypeleadstatuss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINUserTypeLeadStatus].[ID], [zHstINUserTypeLeadStatus].[FKUserTypeID], [zHstINUserTypeLeadStatus].[FKLeadStatusID], [zHstINUserTypeLeadStatus].[StampDate], [zHstINUserTypeLeadStatus].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINUserTypeLeadStatus].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINUserTypeLeadStatus] ");
            query.Append("INNER JOIN (SELECT [zHstINUserTypeLeadStatus].[ID], MAX([zHstINUserTypeLeadStatus].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINUserTypeLeadStatus] ");
            query.Append("WHERE [zHstINUserTypeLeadStatus].[ID] NOT IN (SELECT [INUserTypeLeadStatus].[ID] FROM [INUserTypeLeadStatus]) ");
            query.Append("GROUP BY [zHstINUserTypeLeadStatus].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINUserTypeLeadStatus].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINUserTypeLeadStatus].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inusertypeleadstatus in the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inusertypeleadstatus in the database.</returns>
        public static string ListHistory(INUserTypeLeadStatus inusertypeleadstatus, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inusertypeleadstatus != null)
            {
            query.Append("SELECT [zHstINUserTypeLeadStatus].[ID], [zHstINUserTypeLeadStatus].[FKUserTypeID], [zHstINUserTypeLeadStatus].[FKLeadStatusID], [zHstINUserTypeLeadStatus].[StampDate], [zHstINUserTypeLeadStatus].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINUserTypeLeadStatus].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINUserTypeLeadStatus] ");
                query.Append(" WHERE [zHstINUserTypeLeadStatus].[ID] = @ID");
                query.Append(" ORDER BY [zHstINUserTypeLeadStatus].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inusertypeleadstatus to the database.
        /// </summary>
        /// <param name="inusertypeleadstatus">The inusertypeleadstatus to save.</param>
        /// <returns>A query that can be used to save the inusertypeleadstatus to the database.</returns>
        internal static string Save(INUserTypeLeadStatus inusertypeleadstatus, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inusertypeleadstatus != null)
            {
                if (inusertypeleadstatus.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINUserTypeLeadStatus] ([ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID]) SELECT [ID], [FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID] FROM [INUserTypeLeadStatus] WHERE [INUserTypeLeadStatus].[ID] = @ID; ");
                    query.Append("UPDATE [INUserTypeLeadStatus]");
                    parameters = new object[3];
                    query.Append(" SET [FKUserTypeID] = @FKUserTypeID");
                    parameters[0] = Database.GetParameter("@FKUserTypeID", inusertypeleadstatus.FKUserTypeID);
                    query.Append(", [FKLeadStatusID] = @FKLeadStatusID");
                    parameters[1] = Database.GetParameter("@FKLeadStatusID", inusertypeleadstatus.FKLeadStatusID);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INUserTypeLeadStatus].[ID] = @ID"); 
                    parameters[2] = Database.GetParameter("@ID", inusertypeleadstatus.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INUserTypeLeadStatus] ([FKUserTypeID], [FKLeadStatusID], [StampDate], [StampUserID]) VALUES(@FKUserTypeID, @FKLeadStatusID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[2];
                    parameters[0] = Database.GetParameter("@FKUserTypeID", inusertypeleadstatus.FKUserTypeID);
                    parameters[1] = Database.GetParameter("@FKLeadStatusID", inusertypeleadstatus.FKLeadStatusID);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inusertypeleadstatuss that match the search criteria.
        /// </summary>
        /// <param name="fkusertypeid">The fkusertypeid search criteria.</param>
        /// <param name="fkleadstatusid">The fkleadstatusid search criteria.</param>
        /// <returns>A query that can be used to search for inusertypeleadstatuss based on the search criteria.</returns>
        internal static string Search(long? fkusertypeid, long? fkleadstatusid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkusertypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserTypeLeadStatus].[FKUserTypeID] = " + fkusertypeid + "");
            }
            if (fkleadstatusid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INUserTypeLeadStatus].[FKLeadStatusID] = " + fkleadstatusid + "");
            }
            query.Append("SELECT [INUserTypeLeadStatus].[ID], [INUserTypeLeadStatus].[FKUserTypeID], [INUserTypeLeadStatus].[FKLeadStatusID], [INUserTypeLeadStatus].[StampDate], [INUserTypeLeadStatus].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INUserTypeLeadStatus].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INUserTypeLeadStatus] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
