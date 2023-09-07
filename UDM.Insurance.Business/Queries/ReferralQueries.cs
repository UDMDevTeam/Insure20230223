using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using System;
using System.Text;
using UDM.Insurance.Business.Objects;

namespace UDM.Insurance.Business.Queries
{
    internal abstract partial class ReferralQueries
    {
        #region Delete

        internal static string Delete(Referral refdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (refdata != null)
            {
                query = $"INSERT INTO [zHstINReferrals] ([ID],[FKINImportID],[ReferralNumber],[Name],[CellNumber],[FKINRelationshipID],[FKGenderID],[StampUserID],[StampDate]) " +
                        $"SELECT [ID],[FKINImportID],[ReferralNumber],[Name],[CellNumber],[FKINRelationshipID],[FKGenderID],[StampUserID],GETDATE() FROM [INReferrals] WHERE [INReferrals].[ID] = {refdata.ID}; " +
                        $"DELETE FROM [INReferrals] WHERE [INReferrals].[ID] = {refdata.ID}; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", refdata.ID);
            }
            return query;
        }

        internal static string DeleteHistory(Referral refdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (refdata != null)
            {
                query = "DELETE FROM [zHstINReferrals] WHERE [zHstINReferrals].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", refdata.ID);
            }
            return query;
        }

        internal static string UnDelete(Referral refdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (refdata != null)
            {
                query = "INSERT INTO [INReferrals] ([ID],[FKINImportID],[ReferralNumber],[Name],[CellNumber],[FKINRelationshipID],[FKGenderID],[StampUserID],[StampDate]) " +
                        "SELECT [ID],[FKINImportID],[ReferralNumber],[Name],[CellNumber],[FKINRelationshipID],[FKGenderID],[StampUserID],GETDATE() FROM [zHstINReferrals] " +
                        "WHERE [zHstINReferrals].[ID] = @ID AND [zHstINReferrals].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINReferrals] WHERE [zHstINReferrals].[ID] = @ID) " +
                        "AND (SELECT COUNT(ID) FROM [INReferrals] WHERE [ID] = @ID) = 0; " +
                        "DELETE FROM [zHstINReferrals] WHERE [zHstINReferrals].[ID] = @ID AND [zHstINReferrals].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINReferrals] WHERE [zHstINReferrals].[ID] = @ID) " +
                        "AND (SELECT COUNT([ID]) FROM [INReferrals] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", refdata.ID);
            }
            return query;
        }

        #endregion

        #region Fill

        internal static string Fill(Referral refdata, ref object[] parameters)
        {
            string query = string.Empty;
            if (refdata != null)
            {
                query = "SELECT  [ID],[FKINImportID],[ReferralNumber],[Name],[CellNumber],[FKINRelationshipID],[FKGenderID],[StampUserID],[StampDate] FROM [INReferrals] WHERE [INReferrals].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", refdata.ID);
            }
            return query;
        }

        internal static string FillData(Referral refdata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (refdata != null)
            {
                query.Append("SELECT [INReferrals].[ID], [INReferrals].[FKINImportID], [INReferrals].[ReferralNumber], [INReferrals].[Name], [INReferrals].[CellNumber],[INReferrals].[FKINRelationshipID],[INReferrals].[FKGenderID], [INReferrals].[StampUserID],[INReferrals].[StampDate], ");
                query.Append("(SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INReferrals].[StampUserID]) AS 'StampUser' ");
                query.Append("FROM [INReferrals] ");
                query.Append("WHERE [INReferrals].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", refdata.ID);
            }
            return query.ToString();
        }

        internal static string FillHistory(Referral refdata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (refdata != null)
            {
                query = "SELECT [ID],[FKINImportID],[ReferralNumber],[Name],[CellNumber],[FKINRelationshipID],[FKGenderID],[StampUserID],[StampDate] FROM [zHstINReferrals] " +
                        "WHERE [zHstINReferrals].[ID] = @ID AND [zHstINReferrals].[StampUserID] = @StampUserID AND [zHstINReferrals].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", refdata.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }

        #endregion

        #region List

        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INReferrals].[ID], [INReferrals].[FKINImportID], [INReferrals].[ReferralNumber], [INReferrals].[Name], [INReferrals].[CellNumber],[INReferrals].[FKINRelationshipID],[INReferrals].[FKGenderID],[INReferrals].[StampDate], [INReferrals].[StampUserID], ");
            query.Append("(SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INReferrals].[StampUserID]) AS 'StampUser' ");
            query.Append("FROM [INReferrals]");
            return query.ToString();
        }

        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINReferrals].[ID], [zHstINReferrals].[FKINImportID], [zHstINReferrals].[ReferralNumber], [zHstINReferrals].[Name], [zHstINReferrals].[CellNumber],[zHstINReferrals].[FKINRelationshipID],[zHstINReferrals].[FKGenderID],[zHstINReferrals].[StampDate], [zHstINReferrals].[StampUserID], ");
            query.Append("(SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINReferrals].[StampUserID]) AS 'StampUser' ");
            query.Append("FROM [zHstINReferrals] ");
            query.Append("INNER JOIN (SELECT [zHstINReferrals].[ID], MAX([zHstINReferrals].[StampDate]) AS 'StampDate' FROM [zHstINReferrals] WHERE [zHstINReferrals].[ID] NOT IN (SELECT [INReferrals].[ID] FROM [INReferrals]) GROUP BY [zHstINReferrals].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINReferrals].[ID] = [LastHistory].[ID] AND [zHstINReferrals].[StampDate] = [LastHistory].[StampDate]");
            return query.ToString();
        }

        public static string ListHistory(Referral refdata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (refdata != null)
            {
                query.Append("SELECT [zHstINReferrals].[ID], [zHstINReferrals].[FKINImportID], [zHstINReferrals].[ReferralNumber], [zHstINReferrals].[Name], [zHstINReferrals].[CellNumber],[zHstINReferrals].[FKINRelationshipID],[zHstINReferrals].[FKGenderID],[zHstINReferrals].[StampDate], [zHstINReferrals].[StampUserID], ");
                query.Append("(SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINReferrals].[StampUserID]) AS 'StampUser' ");
                query.Append("FROM [zHstINReferrals] ");
                query.Append("WHERE [zHstINReferrals].[ID] = @ID ");
                query.Append("ORDER BY [zHstINReferrals].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", refdata.ID);
            }
            return query.ToString();
        }

        #endregion

        #region Save

        internal static string Save(Referral refdata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();

            if (refdata != null)
            {
                if (refdata.RecordExists())
                {
                    // Insert the existing record to the history table before updating
                    query.Append("INSERT INTO [zHstINReferrals] ([ID], [FKINImportID], [ReferralNumber], [Name], [CellNumber], [FKINRelationshipID], [FKGenderID], [StampDate], [StampUserID]) ");
                    query.Append("SELECT [ID], [FKINImportID], [ReferralNumber], [Name], [CellNumber], [FKINRelationshipID], [FKGenderID], [StampDate], [StampUserID] ");
                    query.Append("FROM [INReferrals] WHERE [FKINImportID] = @FKINImportID AND [ReferralNumber] = @ReferralNumber; ");

                    // Update the existing record without changing the StampDate
                    query.Append("UPDATE [INReferrals] SET [FKINImportID] = @FKINImportID, [ReferralNumber] = @ReferralNumber, [Name] = @Name, [CellNumber] = @CellNumber, [FKINRelationshipID] = @FKINRelationshipID, [FKGenderID] = @FKGenderID, [StampUserID] = " + GlobalSettings.ApplicationUser.ID + " WHERE [FKINImportID] = @FKINImportID AND [ReferralNumber] = @ReferralNumber");
                }
                else
                {
                    // Insert new record
                    query.Append("INSERT INTO [INReferrals] ([FKINImportID],[ReferralNumber],[Name],[CellNumber],[FKINRelationshipID],[FKGenderID],[StampDate],[StampUserID]) ");
                    query.Append("VALUES (@FKINImportID, @ReferralNumber, @Name, @CellNumber, @FKINRelationshipID, @FKGenderID, GETDATE(), " + GlobalSettings.ApplicationUser.ID + "); ");
                }

                parameters = new object[6];
                parameters[0] = Database.GetParameter("@FKINImportID", string.IsNullOrEmpty(refdata.FKINImportID) ? DBNull.Value : (object)refdata.FKINImportID);
                parameters[1] = Database.GetParameter("@ReferralNumber", string.IsNullOrEmpty(refdata.ReferralNumber) ? DBNull.Value : (object)refdata.ReferralNumber);
                parameters[2] = Database.GetParameter("@Name", string.IsNullOrEmpty(refdata.Name) ? DBNull.Value : (object)refdata.Name);
                parameters[3] = Database.GetParameter("@CellNumber", string.IsNullOrEmpty(refdata.CellNumber) ? DBNull.Value : (object)refdata.CellNumber);
                parameters[4] = Database.GetParameter("@FKINRelationshipID", refdata.FKINRelationshipID.HasValue ? (object)refdata.FKINRelationshipID.Value : DBNull.Value);
                parameters[5] = Database.GetParameter("@FKGenderID", refdata.FKGenderID.HasValue ? (object)refdata.FKGenderID.Value : DBNull.Value);
            }

            return query.ToString();
        }

        #endregion

        #region Search

        internal static string Search(string fkinimportid, string cellnumber, string name, long? stampuserid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INReferrals].[FKINImportID] LIKE '%" + fkinimportid.Replace("'", "''") + "%'");
            }
            if (cellnumber != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INReferrals].[CellNumber] LIKE '%" + cellnumber.Replace("'", "''") + "%'");
            }
            if (name != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INReferrals].[Name] LIKE '%" + name.Replace("'", "''") + "%'");
            }
            if (stampuserid.HasValue)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INReferrals].[StampUserID] = " + stampuserid.Value);
            }

            query.Append("SELECT [INReferrals].[ID], [INReferrals].[FKINImportID], [INReferrals].[ReferralNumber], [INReferrals].[Name], [INReferrals].[CellNumber], [INReferrals].[FKINRelationshipID], [INReferrals].[FKGenderID], [INReferrals].[StampDate], [INReferrals].[StampUserID], ");
            query.Append("(SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INReferrals].[StampUserID]) AS 'StampUser' ");
            query.Append("FROM [INReferrals] ");
            query.Append(whereQuery);
            return query.ToString();
        }

        #endregion
        internal static bool RecordExists(Referral refdata)
        {
            string query = "SELECT COUNT(*) FROM [INReferrals] WHERE [FKINImportID] = @FKINImportID AND [ReferralNumber] = @ReferralNumber";
            object[] parameters = new object[2];
            parameters[0] = Database.GetParameter("@FKINImportID", refdata.FKINImportID);
            parameters[1] = Database.GetParameter("@ReferralNumber", refdata.ReferralNumber);

            int count = Convert.ToInt32(Database.ExecuteScalar(query, parameters));
            return count > 0;
        }

    }
}
