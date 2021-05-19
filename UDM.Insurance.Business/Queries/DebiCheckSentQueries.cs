using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to sms objects.
    /// </summary>
    internal abstract partial class DebiCheckSentQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) sms from the database.
        /// </summary>
        /// <param name="sms">The sms object to delete.</param>
        /// <returns>A query that can be used to delete the sms from the database.</returns>
        internal static string Delete(DebiCheckSent debichecksent, ref object[] parameters)
        {
            string query = string.Empty;
            if (debichecksent != null)
            {
                query = "INSERT INTO [zHstDebiCheckSent] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [DebiCheckSent] WHERE [DebiCheckSent].[ID] = @ID; ";
                query += "DELETE FROM [DebiCheckSent] WHERE [DebiCheckSent].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", debichecksent.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) sms from the database.
        /// </summary>
        /// <param name="sms">The sms object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the sms from the database.</returns>
        internal static string DeleteHistory(DebiCheckSent debichecksent, ref object[] parameters)
        {
            string query = string.Empty;
            if (debichecksent != null)
            {
                query = "DELETE FROM [zHstDebiCheckSent] WHERE [zHstDebiCheckSent].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", debichecksent.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) sms from the database.
        /// </summary>
        /// <param name="sms">The sms object to undelete.</param>
        /// <returns>A query that can be used to undelete the sms from the database.</returns>
        internal static string UnDelete(DebiCheckSent debichecksent, ref object[] parameters)
        {
            string query = string.Empty;
            if (debichecksent != null)
            {
                query = "INSERT INTO [DebiCheckSent] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [zHstDebiCheckSent] WHERE [zHstDebiCheckSent].[ID] = @ID AND [zHstDebiCheckSent].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstDebiCheckSent] WHERE [zHstDebiCheckSent].[ID] = @ID) AND (SELECT COUNT(ID) FROM [DebiCheckSent] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstDebiCheckSent] WHERE [zHstDebiCheckSent].[ID] = @ID AND [zHstDebiCheckSent].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstDebiCheckSent] WHERE [zHstDebiCheckSent].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [DebiCheckSent] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", debichecksent.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an sms object.
        /// </summary>
        /// <param name="sms">The sms object to fill.</param>
        /// <returns>A query that can be used to fill the sms object.</returns>
        internal static string Fill(DebiCheckSent debichecksent, ref object[] parameters)
        {
            string query = string.Empty;
            if (debichecksent != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [DebiCheckSent] WHERE [DebiCheckSent].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", debichecksent.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  sms data.
        /// </summary>
        /// <param name="sms">The sms to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  sms data.</returns>
        internal static string FillData(DebiCheckSent debichecksent, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (debichecksent != null)
            {
            query.Append("SELECT [DebiCheckSent].[ID], [DebiCheckSent].[FKSystemID], [DebiCheckSent].[FKImportID], [DebiCheckSent].[SMSID], [DebiCheckSent].[FKlkpSMSTypeID], [DebiCheckSent].[RecipientCellNum], [DebiCheckSent].[SMSBody], [DebiCheckSent].[FKlkpSMSEncodingID], [DebiCheckSent].[SubmissionID], [DebiCheckSent].[SubmissionDate], [DebiCheckSent].[FKlkpSMSStatusTypeID], [DebiCheckSent].[FKlkpSMSStatusSubtypeID], [DebiCheckSent].[StampDate], [DebiCheckSent].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DebiCheckSent].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DebiCheckSent] ");
                query.Append(" WHERE [DebiCheckSent].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", debichecksent.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an sms object from history.
        /// </summary>
        /// <param name="sms">The sms object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the sms object from history.</returns>
        internal static string FillHistory(DebiCheckSent debichecksent, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (debichecksent != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [zHstDebiCheckSent] WHERE [zHstDebiCheckSent].[ID] = @ID AND [zHstDebiCheckSent].[StampUserID] = @StampUserID AND [zHstDebiCheckSent].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", debichecksent.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the smss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the smss in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [DebiCheckSent].[ID], [DebiCheckSent].[FKSystemID], [DebiCheckSent].[FKImportID], [DebiCheckSent].[SMSID], [DebiCheckSent].[FKlkpSMSTypeID], [DebiCheckSent].[RecipientCellNum], [DebiCheckSent].[SMSBody], [DebiCheckSent].[FKlkpSMSEncodingID], [DebiCheckSent].[SubmissionID], [DebiCheckSent].[SubmissionDate], [DebiCheckSent].[FKlkpSMSStatusTypeID], [DebiCheckSent].[FKlkpSMSStatusSubtypeID], [DebiCheckSent].[StampDate], [DebiCheckSent].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DebiCheckSent].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DebiCheckSent] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted smss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted smss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstDebiCheckSent].[ID], [zHstDebiCheckSent].[FKSystemID], [zHstDebiCheckSent].[FKImportID], [zHstDebiCheckSent].[SMSID], [zHstDebiCheckSent].[FKlkpSMSTypeID], [zHstDebiCheckSent].[RecipientCellNum], [zHstDebiCheckSent].[SMSBody], [zHstDebiCheckSent].[FKlkpSMSEncodingID], [zHstDebiCheckSent].[SubmissionID], [zHstDebiCheckSent].[SubmissionDate], [zHstDebiCheckSent].[FKlkpSMSStatusTypeID], [zHstDebiCheckSent].[FKlkpSMSStatusSubtypeID], [zHstDebiCheckSent].[StampDate], [zHstDebiCheckSent].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstDebiCheckSent].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstDebiCheckSent] ");
            query.Append("INNER JOIN (SELECT [zHstDebiCheckSent].[ID], MAX([zHstDebiCheckSent].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstDebiCheckSentS] ");
            query.Append("WHERE [zHstDebiCheckSent].[ID] NOT IN (SELECT [DebiCheckSent].[ID] FROM [DebiCheckSent]) ");
            query.Append("GROUP BY [zHstDebiCheckSent].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstDebiCheckSent].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstDebiCheckSent].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) sms in the database.
        /// </summary>
        /// <param name="sms">The sms object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) sms in the database.</returns>
        public static string ListHistory(DebiCheckSent debichecksent, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (debichecksent != null)
            {
            query.Append("SELECT [zHstDebiCheckSent].[ID], [zHstDebiCheckSent].[FKSystemID], [zHstDebiCheckSent].[FKImportID], [zHstDebiCheckSent].[SMSID], [zHstDebiCheckSent].[FKlkpSMSTypeID], [zHstDebiCheckSent].[RecipientCellNum], [zHstDebiCheckSent].[SMSBody], [zHstDebiCheckSent].[FKlkpSMSEncodingID], [zHstDebiCheckSent].[SubmissionID], [zHstDebiCheckSent].[SubmissionDate], [zHstDebiCheckSent].[FKlkpSMSStatusTypeID], [zHstDebiCheckSent].[FKlkpSMSStatusSubtypeID], [zHstDebiCheckSent].[StampDate], [zHstDebiCheckSent].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstDebiCheckSent].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstDebiCheckSent] ");
                query.Append(" WHERE [zHstDebiCheckSent].[ID] = @ID");
                query.Append(" ORDER BY [zHstDebiCheckSent].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", debichecksent.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) sms to the database.
        /// </summary>
        /// <param name="sms">The sms to save.</param>
        /// <returns>A query that can be used to save the sms to the database.</returns>
        internal static string Save(DebiCheckSent debichecksent, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (debichecksent != null)
            {
                if (debichecksent.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstDebiCheckSent] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [DebiCheckSent] WHERE [DebiCheckSent].[ID] = @ID; ");
                    query.Append("UPDATE [DebiCheckSent]");
                    parameters = new object[12];
                    query.Append(" SET [FKSystemID] = @FKSystemID");
                    parameters[0] = Database.GetParameter("@FKSystemID", debichecksent.FKSystemID.HasValue ? (object)debichecksent.FKSystemID.Value : DBNull.Value);
                    query.Append(", [FKImportID] = @FKImportID");
                    parameters[1] = Database.GetParameter("@FKImportID", debichecksent.FKImportID.HasValue ? (object)debichecksent.FKImportID.Value : DBNull.Value);
                    query.Append(", [SMSID] = @SMSID");
                    parameters[2] = Database.GetParameter("@SMSID", string.IsNullOrEmpty(debichecksent.SMSID) ? DBNull.Value : (object)debichecksent.SMSID);
                    query.Append(", [FKlkpSMSTypeID] = @FKlkpSMSTypeID");
                    parameters[3] = Database.GetParameter("@FKlkpSMSTypeID", debichecksent.FKlkpSMSTypeID.HasValue ? (object)debichecksent.FKlkpSMSTypeID.Value : DBNull.Value);
                    query.Append(", [RecipientCellNum] = @RecipientCellNum");
                    parameters[4] = Database.GetParameter("@RecipientCellNum", string.IsNullOrEmpty(debichecksent.RecipientCellNum) ? DBNull.Value : (object)debichecksent.RecipientCellNum);
                    query.Append(", [SMSBody] = @SMSBody");
                    parameters[5] = Database.GetParameter("@SMSBody", string.IsNullOrEmpty(debichecksent.SMSBody) ? DBNull.Value : (object)debichecksent.SMSBody);
                    query.Append(", [FKlkpSMSEncodingID] = @FKlkpSMSEncodingID");
                    parameters[6] = Database.GetParameter("@FKlkpSMSEncodingID", debichecksent.FKlkpSMSEncodingID.HasValue ? (object)debichecksent.FKlkpSMSEncodingID.Value : DBNull.Value);
                    query.Append(", [SubmissionID] = @SubmissionID");
                    parameters[7] = Database.GetParameter("@SubmissionID", string.IsNullOrEmpty(debichecksent.SubmissionID) ? DBNull.Value : (object)debichecksent.SubmissionID);
                    query.Append(", [SubmissionDate] = @SubmissionDate");
                    parameters[8] = Database.GetParameter("@SubmissionDate", debichecksent.SubmissionDate.HasValue ? (object)debichecksent.SubmissionDate.Value : DBNull.Value);
                    query.Append(", [FKlkpSMSStatusTypeID] = @FKlkpSMSStatusTypeID");
                    parameters[9] = Database.GetParameter("@FKlkpSMSStatusTypeID", debichecksent.FKlkpSMSStatusTypeID.HasValue ? (object)debichecksent.FKlkpSMSStatusTypeID.Value : DBNull.Value);
                    query.Append(", [FKlkpSMSStatusSubtypeID] = @FKlkpSMSStatusSubtypeID");
                    parameters[10] = Database.GetParameter("@FKlkpSMSStatusSubtypeID", debichecksent.FKlkpSMSStatusSubtypeID.HasValue ? (object)debichecksent.FKlkpSMSStatusSubtypeID.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [DebiCheckSent].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", debichecksent.ID);
                }
                else
                {
                    query.Append("INSERT INTO [DebiCheckSent] ([FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) VALUES(@FKSystemID, @FKImportID, @SMSID, @FKlkpSMSTypeID, @RecipientCellNum, @SMSBody, @FKlkpSMSEncodingID, @SubmissionID, @SubmissionDate, @FKlkpSMSStatusTypeID, @FKlkpSMSStatusSubtypeID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@FKSystemID", debichecksent.FKSystemID.HasValue ? (object)debichecksent.FKSystemID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKImportID", debichecksent.FKImportID.HasValue ? (object)debichecksent.FKImportID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@SMSID", string.IsNullOrEmpty(debichecksent.SMSID) ? DBNull.Value : (object)debichecksent.SMSID);
                    parameters[3] = Database.GetParameter("@FKlkpSMSTypeID", debichecksent.FKlkpSMSTypeID.HasValue ? (object)debichecksent.FKlkpSMSTypeID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@RecipientCellNum", string.IsNullOrEmpty(debichecksent.RecipientCellNum) ? DBNull.Value : (object)debichecksent.RecipientCellNum);
                    parameters[5] = Database.GetParameter("@SMSBody", string.IsNullOrEmpty(debichecksent.SMSBody) ? DBNull.Value : (object)debichecksent.SMSBody);
                    parameters[6] = Database.GetParameter("@FKlkpSMSEncodingID", debichecksent.FKlkpSMSEncodingID.HasValue ? (object)debichecksent.FKlkpSMSEncodingID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@SubmissionID", string.IsNullOrEmpty(debichecksent.SubmissionID) ? DBNull.Value : (object)debichecksent.SubmissionID);
                    parameters[8] = Database.GetParameter("@SubmissionDate", debichecksent.SubmissionDate.HasValue ? (object)debichecksent.SubmissionDate.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@FKlkpSMSStatusTypeID", debichecksent.FKlkpSMSStatusTypeID.HasValue ? (object)debichecksent.FKlkpSMSStatusTypeID.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@FKlkpSMSStatusSubtypeID", debichecksent.FKlkpSMSStatusSubtypeID.HasValue ? (object)debichecksent.FKlkpSMSStatusSubtypeID.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for smss that match the search criteria.
        /// </summary>
        /// <param name="fksystemid">The fksystemid search criteria.</param>
        /// <param name="fkimportid">The fkimportid search criteria.</param>
        /// <param name="smsid">The smsid search criteria.</param>
        /// <param name="fklkpsmstypeid">The fklkpsmstypeid search criteria.</param>
        /// <param name="recipientcellnum">The recipientcellnum search criteria.</param>
        /// <param name="smsbody">The smsbody search criteria.</param>
        /// <param name="fklkpsmsencodingid">The fklkpsmsencodingid search criteria.</param>
        /// <param name="submissionid">The submissionid search criteria.</param>
        /// <param name="submissiondate">The submissiondate search criteria.</param>
        /// <param name="fklkpsmsstatustypeid">The fklkpsmsstatustypeid search criteria.</param>
        /// <param name="fklkpsmsstatussubtypeid">The fklkpsmsstatussubtypeid search criteria.</param>
        /// <returns>A query that can be used to search for smss based on the search criteria.</returns>
        internal static string Search(long? fksystemid, long? fkimportid, string smsid, long? fklkpsmstypeid, string recipientcellnum, string smsbody, long? fklkpsmsencodingid, string submissionid, DateTime? submissiondate, long? fklkpsmsstatustypeid, long? fklkpsmsstatussubtypeid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fksystemid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[FKSystemID] = " + fksystemid + "");
            }
            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[FKImportID] = " + fkimportid + "");
            }
            if (smsid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[SMSID] LIKE '" + smsid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fklkpsmstypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[FKlkpSMSTypeID] = " + fklkpsmstypeid + "");
            }
            if (recipientcellnum != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[RecipientCellNum] LIKE '" + recipientcellnum.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (smsbody != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[SMSBody] LIKE '" + smsbody.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fklkpsmsencodingid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[FKlkpSMSEncodingID] = " + fklkpsmsencodingid + "");
            }
            if (submissionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[SubmissionID] LIKE '" + submissionid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (submissiondate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[SubmissionDate] = '" + submissiondate.Value.ToString(Database.DateFormat) + "'");
            }
            if (fklkpsmsstatustypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[FKlkpSMSStatusTypeID] = " + fklkpsmsstatustypeid + "");
            }
            if (fklkpsmsstatussubtypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[DebiCheckSent].[FKlkpSMSStatusSubtypeID] = " + fklkpsmsstatussubtypeid + "");
            }
            query.Append("SELECT [DebiCheckSent].[ID], [DebiCheckSent].[FKSystemID], [DebiCheckSent].[FKImportID], [DebiCheckSent].[SMSID], [DebiCheckSent].[FKlkpSMSTypeID], [DebiCheckSent].[RecipientCellNum], [DebiCheckSent].[SMSBody], [DebiCheckSent].[FKlkpSMSEncodingID], [DebiCheckSent].[SubmissionID], [DebiCheckSent].[SubmissionDate], [DebiCheckSent].[FKlkpSMSStatusTypeID], [DebiCheckSent].[FKlkpSMSStatusSubtypeID], [DebiCheckSent].[StampDate], [DebiCheckSent].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [DebiCheckSent].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [DebiCheckSent] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
