using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inpolicyoption objects.
    /// </summary>
    internal abstract partial class INPolicyOptionQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inpolicyoption from the database.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption object to delete.</param>
        /// <returns>A query that can be used to delete the inpolicyoption from the database.</returns>
        internal static string Delete(INPolicyOption inpolicyoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicyoption != null)
            {
                query = "INSERT INTO [zHstINPolicyOption] ([ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID]) SELECT [ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID] FROM [INPolicyOption] WHERE [INPolicyOption].[ID] = @ID; ";
                query += "DELETE FROM [INPolicyOption] WHERE [INPolicyOption].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicyoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inpolicyoption from the database.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inpolicyoption from the database.</returns>
        internal static string DeleteHistory(INPolicyOption inpolicyoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicyoption != null)
            {
                query = "DELETE FROM [zHstINPolicyOption] WHERE [zHstINPolicyOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicyoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inpolicyoption from the database.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption object to undelete.</param>
        /// <returns>A query that can be used to undelete the inpolicyoption from the database.</returns>
        internal static string UnDelete(INPolicyOption inpolicyoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicyoption != null)
            {
                query = "INSERT INTO [INPolicyOption] ([ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID]) SELECT [ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID] FROM [zHstINPolicyOption] WHERE [zHstINPolicyOption].[ID] = @ID AND [zHstINPolicyOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyOption] WHERE [zHstINPolicyOption].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPolicyOption] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPolicyOption] WHERE [zHstINPolicyOption].[ID] = @ID AND [zHstINPolicyOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyOption] WHERE [zHstINPolicyOption].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPolicyOption] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicyoption.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inpolicyoption object.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption object to fill.</param>
        /// <returns>A query that can be used to fill the inpolicyoption object.</returns>
        internal static string Fill(INPolicyOption inpolicyoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicyoption != null)
            {
                query = "SELECT [ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID] FROM [INPolicyOption] WHERE [INPolicyOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicyoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inpolicyoption data.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inpolicyoption data.</returns>
        internal static string FillData(INPolicyOption inpolicyoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicyoption != null)
            {
            query.Append("SELECT [INPolicyOption].[ID], [INPolicyOption].[FKPolicyID], [INPolicyOption].[FKOptionID], [INPolicyOption].[IsLA2Selected], [INPolicyOption].[IsChildSelected], [INPolicyOption].[StampDate], [INPolicyOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyOption] ");
                query.Append(" WHERE [INPolicyOption].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicyoption.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inpolicyoption object from history.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inpolicyoption object from history.</returns>
        internal static string FillHistory(INPolicyOption inpolicyoption, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicyoption != null)
            {
                query = "SELECT [ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID] FROM [zHstINPolicyOption] WHERE [zHstINPolicyOption].[ID] = @ID AND [zHstINPolicyOption].[StampUserID] = @StampUserID AND [zHstINPolicyOption].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inpolicyoption.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicyoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inpolicyoptions in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INPolicyOption].[ID], [INPolicyOption].[FKPolicyID], [INPolicyOption].[FKOptionID], [INPolicyOption].[IsLA2Selected], [INPolicyOption].[IsChildSelected], [INPolicyOption].[StampDate], [INPolicyOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyOption] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inpolicyoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inpolicyoptions in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPolicyOption].[ID], [zHstINPolicyOption].[FKPolicyID], [zHstINPolicyOption].[FKOptionID], [zHstINPolicyOption].[IsLA2Selected], [zHstINPolicyOption].[IsChildSelected], [zHstINPolicyOption].[StampDate], [zHstINPolicyOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyOption] ");
            query.Append("INNER JOIN (SELECT [zHstINPolicyOption].[ID], MAX([zHstINPolicyOption].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPolicyOption] ");
            query.Append("WHERE [zHstINPolicyOption].[ID] NOT IN (SELECT [INPolicyOption].[ID] FROM [INPolicyOption]) ");
            query.Append("GROUP BY [zHstINPolicyOption].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPolicyOption].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPolicyOption].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inpolicyoption in the database.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inpolicyoption in the database.</returns>
        public static string ListHistory(INPolicyOption inpolicyoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicyoption != null)
            {
            query.Append("SELECT [zHstINPolicyOption].[ID], [zHstINPolicyOption].[FKPolicyID], [zHstINPolicyOption].[FKOptionID], [zHstINPolicyOption].[IsLA2Selected], [zHstINPolicyOption].[IsChildSelected], [zHstINPolicyOption].[StampDate], [zHstINPolicyOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyOption] ");
                query.Append(" WHERE [zHstINPolicyOption].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPolicyOption].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicyoption.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inpolicyoption to the database.
        /// </summary>
        /// <param name="inpolicyoption">The inpolicyoption to save.</param>
        /// <returns>A query that can be used to save the inpolicyoption to the database.</returns>
        internal static string Save(INPolicyOption inpolicyoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicyoption != null)
            {
                if (inpolicyoption.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPolicyOption] ([ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID]) SELECT [ID], [FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID] FROM [INPolicyOption] WHERE [INPolicyOption].[ID] = @ID; ");
                    query.Append("UPDATE [INPolicyOption]");
                    parameters = new object[5];
                    query.Append(" SET [FKPolicyID] = @FKPolicyID");
                    parameters[0] = Database.GetParameter("@FKPolicyID", inpolicyoption.FKPolicyID.HasValue ? (object)inpolicyoption.FKPolicyID.Value : DBNull.Value);
                    query.Append(", [FKOptionID] = @FKOptionID");
                    parameters[1] = Database.GetParameter("@FKOptionID", inpolicyoption.FKOptionID.HasValue ? (object)inpolicyoption.FKOptionID.Value : DBNull.Value);
                    query.Append(", [IsLA2Selected] = @IsLA2Selected");
                    parameters[2] = Database.GetParameter("@IsLA2Selected", inpolicyoption.IsLA2Selected.HasValue ? (object)inpolicyoption.IsLA2Selected.Value : DBNull.Value);
                    query.Append(", [IsChildSelected] = @IsChildSelected");
                    parameters[3] = Database.GetParameter("@IsChildSelected", inpolicyoption.IsChildSelected.HasValue ? (object)inpolicyoption.IsChildSelected.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPolicyOption].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", inpolicyoption.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPolicyOption] ([FKPolicyID], [FKOptionID], [IsLA2Selected], [IsChildSelected], [StampDate], [StampUserID]) VALUES(@FKPolicyID, @FKOptionID, @IsLA2Selected, @IsChildSelected, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKPolicyID", inpolicyoption.FKPolicyID.HasValue ? (object)inpolicyoption.FKPolicyID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKOptionID", inpolicyoption.FKOptionID.HasValue ? (object)inpolicyoption.FKOptionID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@IsLA2Selected", inpolicyoption.IsLA2Selected.HasValue ? (object)inpolicyoption.IsLA2Selected.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@IsChildSelected", inpolicyoption.IsChildSelected.HasValue ? (object)inpolicyoption.IsChildSelected.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicyoptions that match the search criteria.
        /// </summary>
        /// <param name="fkpolicyid">The fkpolicyid search criteria.</param>
        /// <param name="fkoptionid">The fkoptionid search criteria.</param>
        /// <param name="isla2selected">The isla2selected search criteria.</param>
        /// <param name="ischildselected">The ischildselected search criteria.</param>
        /// <returns>A query that can be used to search for inpolicyoptions based on the search criteria.</returns>
        internal static string Search(long? fkpolicyid, long? fkoptionid, Byte? isla2selected, Byte? ischildselected)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkpolicyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyOption].[FKPolicyID] = " + fkpolicyid + "");
            }
            if (fkoptionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyOption].[FKOptionID] = " + fkoptionid + "");
            }
            if (isla2selected != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyOption].[IsLA2Selected] = " + isla2selected + "");
            }
            if (ischildselected != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyOption].[IsChildSelected] = " + ischildselected + "");
            }
            query.Append("SELECT [INPolicyOption].[ID], [INPolicyOption].[FKPolicyID], [INPolicyOption].[FKOptionID], [INPolicyOption].[IsLA2Selected], [INPolicyOption].[IsChildSelected], [INPolicyOption].[StampDate], [INPolicyOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyOption] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
