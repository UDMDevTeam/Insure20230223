using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportmining objects.
    /// </summary>
    internal abstract partial class INImportMiningQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportmining from the database.
        /// </summary>
        /// <param name="inimportmining">The inimportmining object to delete.</param>
        /// <returns>A query that can be used to delete the inimportmining from the database.</returns>
        internal static string Delete(INImportMining inimportmining, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportmining != null)
            {
                query = "INSERT INTO [zHstINImportMining] ([ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID] FROM [INImportMining] WHERE [INImportMining].[ID] = @ID; ";
                query += "DELETE FROM [INImportMining] WHERE [INImportMining].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportmining.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportmining from the database.
        /// </summary>
        /// <param name="inimportmining">The inimportmining object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportmining from the database.</returns>
        internal static string DeleteHistory(INImportMining inimportmining, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportmining != null)
            {
                query = "DELETE FROM [zHstINImportMining] WHERE [zHstINImportMining].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportmining.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportmining from the database.
        /// </summary>
        /// <param name="inimportmining">The inimportmining object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportmining from the database.</returns>
        internal static string UnDelete(INImportMining inimportmining, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportmining != null)
            {
                query = "INSERT INTO [INImportMining] ([ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID] FROM [zHstINImportMining] WHERE [zHstINImportMining].[ID] = @ID AND [zHstINImportMining].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportMining] WHERE [zHstINImportMining].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportMining] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportMining] WHERE [zHstINImportMining].[ID] = @ID AND [zHstINImportMining].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportMining] WHERE [zHstINImportMining].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportMining] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportmining.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportmining object.
        /// </summary>
        /// <param name="inimportmining">The inimportmining object to fill.</param>
        /// <returns>A query that can be used to fill the inimportmining object.</returns>
        internal static string Fill(INImportMining inimportmining, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportmining != null)
            {
                query = "SELECT [ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID] FROM [INImportMining] WHERE [INImportMining].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportmining.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportmining data.
        /// </summary>
        /// <param name="inimportmining">The inimportmining to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportmining data.</returns>
        internal static string FillData(INImportMining inimportmining, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportmining != null)
            {
            query.Append("SELECT [INImportMining].[ID], [INImportMining].[FKImportID], [INImportMining].[FKUserID], [INImportMining].[Rank], [INImportMining].[AllocationDate], [INImportMining].[StampDate], [INImportMining].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportMining].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportMining] ");
                query.Append(" WHERE [INImportMining].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportmining.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportmining object from history.
        /// </summary>
        /// <param name="inimportmining">The inimportmining object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportmining object from history.</returns>
        internal static string FillHistory(INImportMining inimportmining, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportmining != null)
            {
                query = "SELECT [ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID] FROM [zHstINImportMining] WHERE [zHstINImportMining].[ID] = @ID AND [zHstINImportMining].[StampUserID] = @StampUserID AND [zHstINImportMining].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportmining.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportminings in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportminings in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportMining].[ID], [INImportMining].[FKImportID], [INImportMining].[FKUserID], [INImportMining].[Rank], [INImportMining].[AllocationDate], [INImportMining].[StampDate], [INImportMining].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportMining].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportMining] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportminings in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportminings in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportMining].[ID], [zHstINImportMining].[FKImportID], [zHstINImportMining].[FKUserID], [zHstINImportMining].[Rank], [zHstINImportMining].[AllocationDate], [zHstINImportMining].[StampDate], [zHstINImportMining].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportMining].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportMining] ");
            query.Append("INNER JOIN (SELECT [zHstINImportMining].[ID], MAX([zHstINImportMining].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportMining] ");
            query.Append("WHERE [zHstINImportMining].[ID] NOT IN (SELECT [INImportMining].[ID] FROM [INImportMining]) ");
            query.Append("GROUP BY [zHstINImportMining].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportMining].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportMining].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportmining in the database.
        /// </summary>
        /// <param name="inimportmining">The inimportmining object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportmining in the database.</returns>
        public static string ListHistory(INImportMining inimportmining, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportmining != null)
            {
            query.Append("SELECT [zHstINImportMining].[ID], [zHstINImportMining].[FKImportID], [zHstINImportMining].[FKUserID], [zHstINImportMining].[Rank], [zHstINImportMining].[AllocationDate], [zHstINImportMining].[StampDate], [zHstINImportMining].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportMining].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportMining] ");
                query.Append(" WHERE [zHstINImportMining].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportMining].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportmining.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportmining to the database.
        /// </summary>
        /// <param name="inimportmining">The inimportmining to save.</param>
        /// <returns>A query that can be used to save the inimportmining to the database.</returns>
        internal static string Save(INImportMining inimportmining, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportmining != null)
            {
                if (inimportmining.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportMining] ([ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID]) SELECT [ID], [FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID] FROM [INImportMining] WHERE [INImportMining].[ID] = @ID; ");
                    query.Append("UPDATE [INImportMining]");
                    parameters = new object[5];
                    query.Append(" SET [FKImportID] = @FKImportID");
                    parameters[0] = Database.GetParameter("@FKImportID", inimportmining.FKImportID.HasValue ? (object)inimportmining.FKImportID.Value : DBNull.Value);
                    query.Append(", [FKUserID] = @FKUserID");
                    parameters[1] = Database.GetParameter("@FKUserID", inimportmining.FKUserID.HasValue ? (object)inimportmining.FKUserID.Value : DBNull.Value);
                    query.Append(", [Rank] = @Rank");
                    parameters[2] = Database.GetParameter("@Rank", inimportmining.Rank.HasValue ? (object)inimportmining.Rank.Value : DBNull.Value);
                    query.Append(", [AllocationDate] = @AllocationDate");
                    parameters[3] = Database.GetParameter("@AllocationDate", inimportmining.AllocationDate.HasValue ? (object)inimportmining.AllocationDate.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportMining].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", inimportmining.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportMining] ([FKImportID], [FKUserID], [Rank], [AllocationDate], [StampDate], [StampUserID]) VALUES(@FKImportID, @FKUserID, @Rank, @AllocationDate, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKImportID", inimportmining.FKImportID.HasValue ? (object)inimportmining.FKImportID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKUserID", inimportmining.FKUserID.HasValue ? (object)inimportmining.FKUserID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@Rank", inimportmining.Rank.HasValue ? (object)inimportmining.Rank.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@AllocationDate", inimportmining.AllocationDate.HasValue ? (object)inimportmining.AllocationDate.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportminings that match the search criteria.
        /// </summary>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="fkuserid">The fkuserid search criteria.</param>
        /// <param name="rank">The rank search criteria.</param>
        /// <param name="allocationdate">The allocationdate search criteria.</param>
        /// <returns>A query that can be used to search for inimportminings based on the search criteria.</returns>
        internal static string Search(long? fkimportid, long? fkuserid, short? rank, DateTime? allocationdate)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportMining].[FKImportID] = " + fkimportid + "");
            }
            if (fkuserid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportMining].[FKUserID] = " + fkuserid + "");
            }
            if (rank != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportMining].[Rank] = " + rank + "");
            }
            if (allocationdate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportMining].[AllocationDate] = '" + allocationdate.Value.ToString(Database.DateFormat) + "'");
            }
            query.Append("SELECT [INImportMining].[ID], [INImportMining].[FKImportID], [INImportMining].[FKUserID], [INImportMining].[Rank], [INImportMining].[AllocationDate], [INImportMining].[StampDate], [INImportMining].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportMining].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportMining] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
