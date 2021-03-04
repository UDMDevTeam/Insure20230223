using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to incampaign objects.
    /// </summary>
    internal abstract partial class INCampaignQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) incampaign from the database.
        /// </summary>
        /// <param name="incampaign">The incampaign object to delete.</param>
        /// <returns>A query that can be used to delete the incampaign from the database.</returns>
        internal static string Delete(INCampaign incampaign, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaign != null)
            {
                query = "INSERT INTO [zHstINCampaign] ([ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID] FROM [INCampaign] WHERE [INCampaign].[ID] = @ID; ";
                query += "DELETE FROM [INCampaign] WHERE [INCampaign].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaign.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) incampaign from the database.
        /// </summary>
        /// <param name="incampaign">The incampaign object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the incampaign from the database.</returns>
        internal static string DeleteHistory(INCampaign incampaign, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaign != null)
            {
                query = "DELETE FROM [zHstINCampaign] WHERE [zHstINCampaign].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaign.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) incampaign from the database.
        /// </summary>
        /// <param name="incampaign">The incampaign object to undelete.</param>
        /// <returns>A query that can be used to undelete the incampaign from the database.</returns>
        internal static string UnDelete(INCampaign incampaign, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaign != null)
            {
                query = "INSERT INTO [INCampaign] ([ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID] FROM [zHstINCampaign] WHERE [zHstINCampaign].[ID] = @ID AND [zHstINCampaign].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaign] WHERE [zHstINCampaign].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INCampaign] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINCampaign] WHERE [zHstINCampaign].[ID] = @ID AND [zHstINCampaign].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINCampaign] WHERE [zHstINCampaign].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INCampaign] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaign.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an incampaign object.
        /// </summary>
        /// <param name="incampaign">The incampaign object to fill.</param>
        /// <returns>A query that can be used to fill the incampaign object.</returns>
        internal static string Fill(INCampaign incampaign, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaign != null)
            {
                query = "SELECT [ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID] FROM [INCampaign] WHERE [INCampaign].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaign.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  incampaign data.
        /// </summary>
        /// <param name="incampaign">The incampaign to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  incampaign data.</returns>
        internal static string FillData(INCampaign incampaign, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaign != null)
            {
            query.Append("SELECT [INCampaign].[ID], [INCampaign].[FKINCampaignGroupID], [INCampaign].[FKINCampaignTypeID], [INCampaign].[Name], [INCampaign].[Code], [INCampaign].[Conversion1], [INCampaign].[Conversion2], [INCampaign].[Covnersion3], [INCampaign].[Conversion4], [INCampaign].[Conversion5], [INCampaign].[Conversion6], [INCampaign].[IsActive], [INCampaign].[StampDate], [INCampaign].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCampaign].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCampaign] ");
                query.Append(" WHERE [INCampaign].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaign.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an incampaign object from history.
        /// </summary>
        /// <param name="incampaign">The incampaign object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the incampaign object from history.</returns>
        internal static string FillHistory(INCampaign incampaign, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (incampaign != null)
            {
                query = "SELECT [ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID] FROM [zHstINCampaign] WHERE [zHstINCampaign].[ID] = @ID AND [zHstINCampaign].[StampUserID] = @StampUserID AND [zHstINCampaign].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", incampaign.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the incampaigns in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the incampaigns in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INCampaign].[ID], [INCampaign].[FKINCampaignGroupID], [INCampaign].[FKINCampaignTypeID], [INCampaign].[Name], [INCampaign].[Code], [INCampaign].[Conversion1], [INCampaign].[Conversion2], [INCampaign].[Covnersion3], [INCampaign].[Conversion4], [INCampaign].[Conversion5], [INCampaign].[Conversion6], [INCampaign].[IsActive], [INCampaign].[StampDate], [INCampaign].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCampaign].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCampaign] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted incampaigns in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted incampaigns in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINCampaign].[ID], [zHstINCampaign].[FKINCampaignGroupID], [zHstINCampaign].[FKINCampaignTypeID], [zHstINCampaign].[Name], [zHstINCampaign].[Code], [zHstINCampaign].[Conversion1], [zHstINCampaign].[Conversion2], [zHstINCampaign].[Covnersion3], [zHstINCampaign].[Conversion4], [zHstINCampaign].[Conversion5], [zHstINCampaign].[Conversion6], [zHstINCampaign].[IsActive], [zHstINCampaign].[StampDate], [zHstINCampaign].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINCampaign].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINCampaign] ");
            query.Append("INNER JOIN (SELECT [zHstINCampaign].[ID], MAX([zHstINCampaign].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINCampaign] ");
            query.Append("WHERE [zHstINCampaign].[ID] NOT IN (SELECT [INCampaign].[ID] FROM [INCampaign]) ");
            query.Append("GROUP BY [zHstINCampaign].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINCampaign].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINCampaign].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) incampaign in the database.
        /// </summary>
        /// <param name="incampaign">The incampaign object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) incampaign in the database.</returns>
        public static string ListHistory(INCampaign incampaign, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaign != null)
            {
            query.Append("SELECT [zHstINCampaign].[ID], [zHstINCampaign].[FKINCampaignGroupID], [zHstINCampaign].[FKINCampaignTypeID], [zHstINCampaign].[Name], [zHstINCampaign].[Code], [zHstINCampaign].[Conversion1], [zHstINCampaign].[Conversion2], [zHstINCampaign].[Covnersion3], [zHstINCampaign].[Conversion4], [zHstINCampaign].[Conversion5], [zHstINCampaign].[Conversion6], [zHstINCampaign].[IsActive], [zHstINCampaign].[StampDate], [zHstINCampaign].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINCampaign].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINCampaign] ");
                query.Append(" WHERE [zHstINCampaign].[ID] = @ID");
                query.Append(" ORDER BY [zHstINCampaign].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", incampaign.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) incampaign to the database.
        /// </summary>
        /// <param name="incampaign">The incampaign to save.</param>
        /// <returns>A query that can be used to save the incampaign to the database.</returns>
        internal static string Save(INCampaign incampaign, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (incampaign != null)
            {
                if (incampaign.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINCampaign] ([ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID] FROM [INCampaign] WHERE [INCampaign].[ID] = @ID; ");
                    query.Append("UPDATE [INCampaign]");
                    parameters = new object[12];
                    query.Append(" SET [FKINCampaignGroupID] = @FKINCampaignGroupID");
                    parameters[0] = Database.GetParameter("@FKINCampaignGroupID", incampaign.FKINCampaignGroupID.HasValue ? (object)incampaign.FKINCampaignGroupID.Value : DBNull.Value);
                    query.Append(", [FKINCampaignTypeID] = @FKINCampaignTypeID");
                    parameters[1] = Database.GetParameter("@FKINCampaignTypeID", incampaign.FKINCampaignTypeID.HasValue ? (object)incampaign.FKINCampaignTypeID.Value : DBNull.Value);
                    query.Append(", [Name] = @Name");
                    parameters[2] = Database.GetParameter("@Name", string.IsNullOrEmpty(incampaign.Name) ? DBNull.Value : (object)incampaign.Name);
                    query.Append(", [Code] = @Code");
                    parameters[3] = Database.GetParameter("@Code", string.IsNullOrEmpty(incampaign.Code) ? DBNull.Value : (object)incampaign.Code);
                    query.Append(", [Conversion1] = @Conversion1");
                    parameters[4] = Database.GetParameter("@Conversion1", incampaign.Conversion1.HasValue ? (object)incampaign.Conversion1.Value : DBNull.Value);
                    query.Append(", [Conversion2] = @Conversion2");
                    parameters[5] = Database.GetParameter("@Conversion2", incampaign.Conversion2.HasValue ? (object)incampaign.Conversion2.Value : DBNull.Value);
                    query.Append(", [Covnersion3] = @Covnersion3");
                    parameters[6] = Database.GetParameter("@Covnersion3", incampaign.Covnersion3.HasValue ? (object)incampaign.Covnersion3.Value : DBNull.Value);
                    query.Append(", [Conversion4] = @Conversion4");
                    parameters[7] = Database.GetParameter("@Conversion4", incampaign.Conversion4.HasValue ? (object)incampaign.Conversion4.Value : DBNull.Value);
                    query.Append(", [Conversion5] = @Conversion5");
                    parameters[8] = Database.GetParameter("@Conversion5", incampaign.Conversion5.HasValue ? (object)incampaign.Conversion5.Value : DBNull.Value);
                    query.Append(", [Conversion6] = @Conversion6");
                    parameters[9] = Database.GetParameter("@Conversion6", incampaign.Conversion6.HasValue ? (object)incampaign.Conversion6.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[10] = Database.GetParameter("@IsActive", incampaign.IsActive.HasValue ? (object)incampaign.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INCampaign].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", incampaign.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INCampaign] ([FKINCampaignGroupID], [FKINCampaignTypeID], [Name], [Code], [Conversion1], [Conversion2], [Covnersion3], [Conversion4], [Conversion5], [Conversion6], [IsActive], [StampDate], [StampUserID]) VALUES(@FKINCampaignGroupID, @FKINCampaignTypeID, @Name, @Code, @Conversion1, @Conversion2, @Covnersion3, @Conversion4, @Conversion5, @Conversion6, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@FKINCampaignGroupID", incampaign.FKINCampaignGroupID.HasValue ? (object)incampaign.FKINCampaignGroupID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINCampaignTypeID", incampaign.FKINCampaignTypeID.HasValue ? (object)incampaign.FKINCampaignTypeID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@Name", string.IsNullOrEmpty(incampaign.Name) ? DBNull.Value : (object)incampaign.Name);
                    parameters[3] = Database.GetParameter("@Code", string.IsNullOrEmpty(incampaign.Code) ? DBNull.Value : (object)incampaign.Code);
                    parameters[4] = Database.GetParameter("@Conversion1", incampaign.Conversion1.HasValue ? (object)incampaign.Conversion1.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@Conversion2", incampaign.Conversion2.HasValue ? (object)incampaign.Conversion2.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@Covnersion3", incampaign.Covnersion3.HasValue ? (object)incampaign.Covnersion3.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@Conversion4", incampaign.Conversion4.HasValue ? (object)incampaign.Conversion4.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@Conversion5", incampaign.Conversion5.HasValue ? (object)incampaign.Conversion5.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@Conversion6", incampaign.Conversion6.HasValue ? (object)incampaign.Conversion6.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@IsActive", incampaign.IsActive.HasValue ? (object)incampaign.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for incampaigns that match the search criteria.
        /// </summary>
        /// <param name="fkincampaigngroupid">The fkincampaigngroupid search criteria.</param>
        /// <param name="fkincampaigntypeid">The fkincampaigntypeid search criteria.</param>
        /// <param name="name">The name search criteria.</param>
        /// <param name="code">The code search criteria.</param>
        /// <param name="conversion1">The conversion1 search criteria.</param>
        /// <param name="conversion2">The conversion2 search criteria.</param>
        /// <param name="covnersion3">The covnersion3 search criteria.</param>
        /// <param name="conversion4">The conversion4 search criteria.</param>
        /// <param name="conversion5">The conversion5 search criteria.</param>
        /// <param name="conversion6">The conversion6 search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for incampaigns based on the search criteria.</returns>
        internal static string Search(long? fkincampaigngroupid, long? fkincampaigntypeid, string name, string code, Double? conversion1, Double? conversion2, Double? covnersion3, Double? conversion4, Double? conversion5, Double? conversion6, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkincampaigngroupid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[FKINCampaignGroupID] = " + fkincampaigngroupid + "");
            }
            if (fkincampaigntypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[FKINCampaignTypeID] = " + fkincampaigntypeid + "");
            }
            if (name != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Name] LIKE '" + name.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (code != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Code] LIKE '" + code.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (conversion1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Conversion1] = " + conversion1 + "");
            }
            if (conversion2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Conversion2] = " + conversion2 + "");
            }
            if (covnersion3 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Covnersion3] = " + covnersion3 + "");
            }
            if (conversion4 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Conversion4] = " + conversion4 + "");
            }
            if (conversion5 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Conversion5] = " + conversion5 + "");
            }
            if (conversion6 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[Conversion6] = " + conversion6 + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INCampaign].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INCampaign].[ID], [INCampaign].[FKINCampaignGroupID], [INCampaign].[FKINCampaignTypeID], [INCampaign].[Name], [INCampaign].[Code], [INCampaign].[Conversion1], [INCampaign].[Conversion2], [INCampaign].[Covnersion3], [INCampaign].[Conversion4], [INCampaign].[Conversion5], [INCampaign].[Conversion6], [INCampaign].[IsActive], [INCampaign].[StampDate], [INCampaign].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INCampaign].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INCampaign] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
