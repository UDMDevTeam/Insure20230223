using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to infolog1 objects.
    /// </summary>
    internal abstract partial class InfoLog1Queries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) infolog1 from the database.
        /// </summary>
        /// <param name="infolog1">The infolog1 object to delete.</param>
        /// <returns>A query that can be used to delete the infolog1 from the database.</returns>
        internal static string Delete(InfoLog1 infolog1, ref object[] parameters)
        {
            string query = string.Empty;
            if (infolog1 != null)
            {
                query = "INSERT INTO [zHstInfoLog1] ([ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID] FROM [InfoLog1] WHERE [InfoLog1].[ID] = @ID; ";
                query += "DELETE FROM [InfoLog1] WHERE [InfoLog1].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infolog1.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) infolog1 from the database.
        /// </summary>
        /// <param name="infolog1">The infolog1 object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the infolog1 from the database.</returns>
        internal static string DeleteHistory(InfoLog1 infolog1, ref object[] parameters)
        {
            string query = string.Empty;
            if (infolog1 != null)
            {
                query = "DELETE FROM [zHstInfoLog1] WHERE [zHstInfoLog1].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infolog1.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) infolog1 from the database.
        /// </summary>
        /// <param name="infolog1">The infolog1 object to undelete.</param>
        /// <returns>A query that can be used to undelete the infolog1 from the database.</returns>
        internal static string UnDelete(InfoLog1 infolog1, ref object[] parameters)
        {
            string query = string.Empty;
            if (infolog1 != null)
            {
                query = "INSERT INTO [InfoLog1] ([ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID] FROM [zHstInfoLog1] WHERE [zHstInfoLog1].[ID] = @ID AND [zHstInfoLog1].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstInfoLog1] WHERE [zHstInfoLog1].[ID] = @ID) AND (SELECT COUNT(ID) FROM [InfoLog1] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstInfoLog1] WHERE [zHstInfoLog1].[ID] = @ID AND [zHstInfoLog1].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstInfoLog1] WHERE [zHstInfoLog1].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [InfoLog1] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infolog1.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an infolog1 object.
        /// </summary>
        /// <param name="infolog1">The infolog1 object to fill.</param>
        /// <returns>A query that can be used to fill the infolog1 object.</returns>
        internal static string Fill(InfoLog1 infolog1, ref object[] parameters)
        {
            string query = string.Empty;
            if (infolog1 != null)
            {
                query = "SELECT [ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID] FROM [InfoLog1] WHERE [InfoLog1].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infolog1.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  infolog1 data.
        /// </summary>
        /// <param name="infolog1">The infolog1 to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  infolog1 data.</returns>
        internal static string FillData(InfoLog1 infolog1, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infolog1 != null)
            {
            query.Append("SELECT [InfoLog1].[ID], [InfoLog1].[FKImportID], [InfoLog1].[FKPlanID], [InfoLog1].[LA1Cover], [InfoLog1].[IsLA2Checked], [InfoLog1].[FKOptionID1], [InfoLog1].[FKOptionID2], [InfoLog1].[StampDate], [InfoLog1].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [InfoLog1].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [InfoLog1] ");
                query.Append(" WHERE [InfoLog1].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infolog1.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an infolog1 object from history.
        /// </summary>
        /// <param name="infolog1">The infolog1 object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the infolog1 object from history.</returns>
        internal static string FillHistory(InfoLog1 infolog1, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (infolog1 != null)
            {
                query = "SELECT [ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID] FROM [zHstInfoLog1] WHERE [zHstInfoLog1].[ID] = @ID AND [zHstInfoLog1].[StampUserID] = @StampUserID AND [zHstInfoLog1].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", infolog1.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the infolog1s in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the infolog1s in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [InfoLog1].[ID], [InfoLog1].[FKImportID], [InfoLog1].[FKPlanID], [InfoLog1].[LA1Cover], [InfoLog1].[IsLA2Checked], [InfoLog1].[FKOptionID1], [InfoLog1].[FKOptionID2], [InfoLog1].[StampDate], [InfoLog1].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [InfoLog1].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [InfoLog1] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted infolog1s in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted infolog1s in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstInfoLog1].[ID], [zHstInfoLog1].[FKImportID], [zHstInfoLog1].[FKPlanID], [zHstInfoLog1].[LA1Cover], [zHstInfoLog1].[IsLA2Checked], [zHstInfoLog1].[FKOptionID1], [zHstInfoLog1].[FKOptionID2], [zHstInfoLog1].[StampDate], [zHstInfoLog1].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstInfoLog1].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstInfoLog1] ");
            query.Append("INNER JOIN (SELECT [zHstInfoLog1].[ID], MAX([zHstInfoLog1].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstInfoLog1] ");
            query.Append("WHERE [zHstInfoLog1].[ID] NOT IN (SELECT [InfoLog1].[ID] FROM [InfoLog1]) ");
            query.Append("GROUP BY [zHstInfoLog1].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstInfoLog1].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstInfoLog1].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) infolog1 in the database.
        /// </summary>
        /// <param name="infolog1">The infolog1 object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) infolog1 in the database.</returns>
        public static string ListHistory(InfoLog1 infolog1, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infolog1 != null)
            {
            query.Append("SELECT [zHstInfoLog1].[ID], [zHstInfoLog1].[FKImportID], [zHstInfoLog1].[FKPlanID], [zHstInfoLog1].[LA1Cover], [zHstInfoLog1].[IsLA2Checked], [zHstInfoLog1].[FKOptionID1], [zHstInfoLog1].[FKOptionID2], [zHstInfoLog1].[StampDate], [zHstInfoLog1].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstInfoLog1].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstInfoLog1] ");
                query.Append(" WHERE [zHstInfoLog1].[ID] = @ID");
                query.Append(" ORDER BY [zHstInfoLog1].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", infolog1.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) infolog1 to the database.
        /// </summary>
        /// <param name="infolog1">The infolog1 to save.</param>
        /// <returns>A query that can be used to save the infolog1 to the database.</returns>
        internal static string Save(InfoLog1 infolog1, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (infolog1 != null)
            {
                if (infolog1.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstInfoLog1] ([ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID] FROM [InfoLog1] WHERE [InfoLog1].[ID] = @ID; ");
                    query.Append("UPDATE [InfoLog1]");
                    parameters = new object[7];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", infolog1.FKImportID.HasValue ? (object)infolog1.FKImportID.Value : DBNull.Value);
                    query.Append(", [FKPlanID] = @FKPlanID");
                    parameters[1] = Database.GetParameter("@FKPlanID", infolog1.FKPlanID.HasValue ? (object)infolog1.FKPlanID.Value : DBNull.Value);
                    query.Append(", [LA1Cover] = @LA1Cover");
                    parameters[2] = Database.GetParameter("@LA1Cover", infolog1.LA1Cover.HasValue ? (object)infolog1.LA1Cover.Value : DBNull.Value);
                    query.Append(", [IsLA2Checked] = @IsLA2Checked");
                    parameters[3] = Database.GetParameter("@IsLA2Checked", infolog1.IsLA2Checked.HasValue ? (object)infolog1.IsLA2Checked.Value : DBNull.Value);
                    query.Append(", [FKOptionID1] = @FKOptionID1");
                    parameters[4] = Database.GetParameter("@FKOptionID1", infolog1.FKOptionID1.HasValue ? (object)infolog1.FKOptionID1.Value : DBNull.Value);
                    query.Append(", [FKOptionID2] = @FKOptionID2");
                    parameters[5] = Database.GetParameter("@FKOptionID2", infolog1.FKOptionID2.HasValue ? (object)infolog1.FKOptionID2.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [InfoLog1].[ID] = @ID"); 
                    parameters[6] = Database.GetParameter("@ID", infolog1.ID);
                }
                else
                {
                    query.Append("INSERT INTO [InfoLog1] ([FKImportID], [FKPlanID], [LA1Cover], [IsLA2Checked], [FKOptionID1], [FKOptionID2], [StampDate], [StampUserID]) VALUES(@FKImportID, @FKPlanID, @LA1Cover, @IsLA2Checked, @FKOptionID1, @FKOptionID2, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[6];
                    parameters[0] = Database.GetParameter("@FKImportID", infolog1.FKImportID.HasValue ? (object)infolog1.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKPlanID", infolog1.FKPlanID.HasValue ? (object)infolog1.FKPlanID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@LA1Cover", infolog1.LA1Cover.HasValue ? (object)infolog1.LA1Cover.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@IsLA2Checked", infolog1.IsLA2Checked.HasValue ? (object)infolog1.IsLA2Checked.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@FKOptionID1", infolog1.FKOptionID1.HasValue ? (object)infolog1.FKOptionID1.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@FKOptionID2", infolog1.FKOptionID2.HasValue ? (object)infolog1.FKOptionID2.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for infolog1s that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkplanid">The fkplanid search criteria.</param>
        /// <param name="la1cover">The la1cover search criteria.</param>
        /// <param name="isla2checked">The isla2checked search criteria.</param>
        /// <param name="fkoptionid1">The fkoptionid1 search criteria.</param>
        /// <param name="fkoptionid2">The fkoptionid2 search criteria.</param>
        /// <returns>A query that can be used to search for infolog1s based on the search criteria.</returns>
        internal static string Search(long? fkimportid, long? fkplanid, decimal? la1cover, bool? isla2checked, long? fkoptionid1, long? fkoptionid2)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[InfoLog1].[FKImportID] = " + fkimportid + "");
            }
            if (fkplanid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[InfoLog1].[FKPlanID] = " + fkplanid + "");
            }
            if (la1cover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[InfoLog1].[LA1Cover] = " + la1cover + "");
            }
            if (isla2checked != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[InfoLog1].[IsLA2Checked] = " + ((bool)isla2checked ? "1" : "0"));
            }
            if (fkoptionid1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[InfoLog1].[FKOptionID1] = " + fkoptionid1 + "");
            }
            if (fkoptionid2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[InfoLog1].[FKOptionID2] = " + fkoptionid2 + "");
            }
            query.Append("SELECT [InfoLog1].[ID], [InfoLog1].[FKImportID], [InfoLog1].[FKPlanID], [InfoLog1].[LA1Cover], [InfoLog1].[IsLA2Checked], [InfoLog1].[FKOptionID1], [InfoLog1].[FKOptionID2], [InfoLog1].[StampDate], [InfoLog1].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [InfoLog1].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [InfoLog1] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
