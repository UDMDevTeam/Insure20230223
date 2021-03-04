using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inpolicybeneficiary objects.
    /// </summary>
    internal abstract partial class INPolicyBeneficiaryQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inpolicybeneficiary from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary object to delete.</param>
        /// <returns>A query that can be used to delete the inpolicybeneficiary from the database.</returns>
        internal static string Delete(INPolicyBeneficiary inpolicybeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicybeneficiary != null)
            {
                query = "INSERT INTO [zHstINPolicyBeneficiary] ([ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID] FROM [INPolicyBeneficiary] WHERE [INPolicyBeneficiary].[ID] = @ID; ";
                query += "DELETE FROM [INPolicyBeneficiary] WHERE [INPolicyBeneficiary].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inpolicybeneficiary from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inpolicybeneficiary from the database.</returns>
        internal static string DeleteHistory(INPolicyBeneficiary inpolicybeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicybeneficiary != null)
            {
                query = "DELETE FROM [zHstINPolicyBeneficiary] WHERE [zHstINPolicyBeneficiary].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inpolicybeneficiary from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary object to undelete.</param>
        /// <returns>A query that can be used to undelete the inpolicybeneficiary from the database.</returns>
        internal static string UnDelete(INPolicyBeneficiary inpolicybeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicybeneficiary != null)
            {
                query = "INSERT INTO [INPolicyBeneficiary] ([ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID] FROM [zHstINPolicyBeneficiary] WHERE [zHstINPolicyBeneficiary].[ID] = @ID AND [zHstINPolicyBeneficiary].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyBeneficiary] WHERE [zHstINPolicyBeneficiary].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INPolicyBeneficiary] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINPolicyBeneficiary] WHERE [zHstINPolicyBeneficiary].[ID] = @ID AND [zHstINPolicyBeneficiary].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINPolicyBeneficiary] WHERE [zHstINPolicyBeneficiary].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INPolicyBeneficiary] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inpolicybeneficiary object.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary object to fill.</param>
        /// <returns>A query that can be used to fill the inpolicybeneficiary object.</returns>
        internal static string Fill(INPolicyBeneficiary inpolicybeneficiary, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicybeneficiary != null)
            {
                query = "SELECT [ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID] FROM [INPolicyBeneficiary] WHERE [INPolicyBeneficiary].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inpolicybeneficiary data.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inpolicybeneficiary data.</returns>
        internal static string FillData(INPolicyBeneficiary inpolicybeneficiary, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicybeneficiary != null)
            {
            query.Append("SELECT [INPolicyBeneficiary].[ID], [INPolicyBeneficiary].[FKINPolicyID], [INPolicyBeneficiary].[FKINBeneficiaryID], [INPolicyBeneficiary].[BeneficiaryRank], [INPolicyBeneficiary].[BeneficiaryPercentage], [INPolicyBeneficiary].[StampDate], [INPolicyBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyBeneficiary] ");
                query.Append(" WHERE [INPolicyBeneficiary].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inpolicybeneficiary object from history.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inpolicybeneficiary object from history.</returns>
        internal static string FillHistory(INPolicyBeneficiary inpolicybeneficiary, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inpolicybeneficiary != null)
            {
                query = "SELECT [ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID] FROM [zHstINPolicyBeneficiary] WHERE [zHstINPolicyBeneficiary].[ID] = @ID AND [zHstINPolicyBeneficiary].[StampUserID] = @StampUserID AND [zHstINPolicyBeneficiary].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicybeneficiarys in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inpolicybeneficiarys in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INPolicyBeneficiary].[ID], [INPolicyBeneficiary].[FKINPolicyID], [INPolicyBeneficiary].[FKINBeneficiaryID], [INPolicyBeneficiary].[BeneficiaryRank], [INPolicyBeneficiary].[BeneficiaryPercentage], [INPolicyBeneficiary].[StampDate], [INPolicyBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyBeneficiary] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inpolicybeneficiarys in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inpolicybeneficiarys in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINPolicyBeneficiary].[ID], [zHstINPolicyBeneficiary].[FKINPolicyID], [zHstINPolicyBeneficiary].[FKINBeneficiaryID], [zHstINPolicyBeneficiary].[BeneficiaryRank], [zHstINPolicyBeneficiary].[BeneficiaryPercentage], [zHstINPolicyBeneficiary].[StampDate], [zHstINPolicyBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyBeneficiary] ");
            query.Append("INNER JOIN (SELECT [zHstINPolicyBeneficiary].[ID], MAX([zHstINPolicyBeneficiary].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINPolicyBeneficiary] ");
            query.Append("WHERE [zHstINPolicyBeneficiary].[ID] NOT IN (SELECT [INPolicyBeneficiary].[ID] FROM [INPolicyBeneficiary]) ");
            query.Append("GROUP BY [zHstINPolicyBeneficiary].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINPolicyBeneficiary].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINPolicyBeneficiary].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inpolicybeneficiary in the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inpolicybeneficiary in the database.</returns>
        public static string ListHistory(INPolicyBeneficiary inpolicybeneficiary, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicybeneficiary != null)
            {
            query.Append("SELECT [zHstINPolicyBeneficiary].[ID], [zHstINPolicyBeneficiary].[FKINPolicyID], [zHstINPolicyBeneficiary].[FKINBeneficiaryID], [zHstINPolicyBeneficiary].[BeneficiaryRank], [zHstINPolicyBeneficiary].[BeneficiaryPercentage], [zHstINPolicyBeneficiary].[StampDate], [zHstINPolicyBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINPolicyBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINPolicyBeneficiary] ");
                query.Append(" WHERE [zHstINPolicyBeneficiary].[ID] = @ID");
                query.Append(" ORDER BY [zHstINPolicyBeneficiary].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inpolicybeneficiary to the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary to save.</param>
        /// <returns>A query that can be used to save the inpolicybeneficiary to the database.</returns>
        internal static string Save(INPolicyBeneficiary inpolicybeneficiary, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inpolicybeneficiary != null)
            {
                if (inpolicybeneficiary.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINPolicyBeneficiary] ([ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID]) SELECT [ID], [FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID] FROM [INPolicyBeneficiary] WHERE [INPolicyBeneficiary].[ID] = @ID; ");
                    query.Append("UPDATE [INPolicyBeneficiary]");
                    parameters = new object[5];
                    query.Append(" SET [FKINPolicyID] = @FKINPolicyID");
                    parameters[0] = Database.GetParameter("@FKINPolicyID", inpolicybeneficiary.FKINPolicyID.HasValue ? (object)inpolicybeneficiary.FKINPolicyID.Value : DBNull.Value);
                    query.Append(", [FKINBeneficiaryID] = @FKINBeneficiaryID");
                    parameters[1] = Database.GetParameter("@FKINBeneficiaryID", inpolicybeneficiary.FKINBeneficiaryID.HasValue ? (object)inpolicybeneficiary.FKINBeneficiaryID.Value : DBNull.Value);
                    query.Append(", [BeneficiaryRank] = @BeneficiaryRank");
                    parameters[2] = Database.GetParameter("@BeneficiaryRank", inpolicybeneficiary.BeneficiaryRank.HasValue ? (object)inpolicybeneficiary.BeneficiaryRank.Value : DBNull.Value);
                    query.Append(", [BeneficiaryPercentage] = @BeneficiaryPercentage");
                    parameters[3] = Database.GetParameter("@BeneficiaryPercentage", inpolicybeneficiary.BeneficiaryPercentage.HasValue ? (object)inpolicybeneficiary.BeneficiaryPercentage.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INPolicyBeneficiary].[ID] = @ID"); 
                    parameters[4] = Database.GetParameter("@ID", inpolicybeneficiary.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INPolicyBeneficiary] ([FKINPolicyID], [FKINBeneficiaryID], [BeneficiaryRank], [BeneficiaryPercentage], [StampDate], [StampUserID]) VALUES(@FKINPolicyID, @FKINBeneficiaryID, @BeneficiaryRank, @BeneficiaryPercentage, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[4];
                    parameters[0] = Database.GetParameter("@FKINPolicyID", inpolicybeneficiary.FKINPolicyID.HasValue ? (object)inpolicybeneficiary.FKINPolicyID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKINBeneficiaryID", inpolicybeneficiary.FKINBeneficiaryID.HasValue ? (object)inpolicybeneficiary.FKINBeneficiaryID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@BeneficiaryRank", inpolicybeneficiary.BeneficiaryRank.HasValue ? (object)inpolicybeneficiary.BeneficiaryRank.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@BeneficiaryPercentage", inpolicybeneficiary.BeneficiaryPercentage.HasValue ? (object)inpolicybeneficiary.BeneficiaryPercentage.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicybeneficiarys that match the search criteria.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinbeneficiaryid">The fkinbeneficiaryid search criteria.</param>
        /// <param name="beneficiaryrank">The beneficiaryrank search criteria.</param>
        /// <param name="beneficiarypercentage">The beneficiarypercentage search criteria.</param>
        /// <returns>A query that can be used to search for inpolicybeneficiarys based on the search criteria.</returns>
        internal static string Search(long? fkinpolicyid, long? fkinbeneficiaryid, int? beneficiaryrank, Double? beneficiarypercentage)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinpolicyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyBeneficiary].[FKINPolicyID] = " + fkinpolicyid + "");
            }
            if (fkinbeneficiaryid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyBeneficiary].[FKINBeneficiaryID] = " + fkinbeneficiaryid + "");
            }
            if (beneficiaryrank != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyBeneficiary].[BeneficiaryRank] = " + beneficiaryrank + "");
            }
            if (beneficiarypercentage != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicyBeneficiary].[BeneficiaryPercentage] = " + beneficiarypercentage + "");
            }
            query.Append("SELECT [INPolicyBeneficiary].[ID], [INPolicyBeneficiary].[FKINPolicyID], [INPolicyBeneficiary].[FKINBeneficiaryID], [INPolicyBeneficiary].[BeneficiaryRank], [INPolicyBeneficiary].[BeneficiaryPercentage], [INPolicyBeneficiary].[StampDate], [INPolicyBeneficiary].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicyBeneficiary].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicyBeneficiary] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
