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
    internal abstract partial class SMSQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) sms from the database.
        /// </summary>
        /// <param name="sms">The sms object to delete.</param>
        /// <returns>A query that can be used to delete the sms from the database.</returns>
        internal static string Delete(SMS sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "INSERT INTO [zHstSMS] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [SMS] WHERE [SMS].[ID] = @ID; ";
                query += "DELETE FROM [SMS] WHERE [SMS].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", sms.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) sms from the database.
        /// </summary>
        /// <param name="sms">The sms object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the sms from the database.</returns>
        internal static string DeleteHistory(SMS sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "DELETE FROM [zHstSMS] WHERE [zHstSMS].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", sms.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) sms from the database.
        /// </summary>
        /// <param name="sms">The sms object to undelete.</param>
        /// <returns>A query that can be used to undelete the sms from the database.</returns>
        internal static string UnDelete(SMS sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "INSERT INTO [SMS] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [zHstSMS] WHERE [zHstSMS].[ID] = @ID AND [zHstSMS].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstSMS] WHERE [zHstSMS].[ID] = @ID) AND (SELECT COUNT(ID) FROM [SMS] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstSMS] WHERE [zHstSMS].[ID] = @ID AND [zHstSMS].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstSMS] WHERE [zHstSMS].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [SMS] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", sms.ID);
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
        internal static string Fill(SMS sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [SMS] WHERE [SMS].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", sms.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  sms data.
        /// </summary>
        /// <param name="sms">The sms to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  sms data.</returns>
        internal static string FillData(SMS sms, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (sms != null)
            {
            query.Append("SELECT [SMS].[ID], [SMS].[FKSystemID], [SMS].[FKImportID], [SMS].[SMSID], [SMS].[FKlkpSMSTypeID], [SMS].[RecipientCellNum], [SMS].[SMSBody], [SMS].[FKlkpSMSEncodingID], [SMS].[SubmissionID], [SMS].[SubmissionDate], [SMS].[FKlkpSMSStatusTypeID], [SMS].[FKlkpSMSStatusSubtypeID], [SMS].[StampDate], [SMS].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SMS].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SMS] ");
                query.Append(" WHERE [SMS].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", sms.ID);
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
        internal static string FillHistory(SMS sms, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [zHstSMS] WHERE [zHstSMS].[ID] = @ID AND [zHstSMS].[StampUserID] = @StampUserID AND [zHstSMS].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", sms.ID);
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
            query.Append("SELECT [SMS].[ID], [SMS].[FKSystemID], [SMS].[FKImportID], [SMS].[SMSID], [SMS].[FKlkpSMSTypeID], [SMS].[RecipientCellNum], [SMS].[SMSBody], [SMS].[FKlkpSMSEncodingID], [SMS].[SubmissionID], [SMS].[SubmissionDate], [SMS].[FKlkpSMSStatusTypeID], [SMS].[FKlkpSMSStatusSubtypeID], [SMS].[StampDate], [SMS].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SMS].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SMS] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted smss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted smss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstSMS].[ID], [zHstSMS].[FKSystemID], [zHstSMS].[FKImportID], [zHstSMS].[SMSID], [zHstSMS].[FKlkpSMSTypeID], [zHstSMS].[RecipientCellNum], [zHstSMS].[SMSBody], [zHstSMS].[FKlkpSMSEncodingID], [zHstSMS].[SubmissionID], [zHstSMS].[SubmissionDate], [zHstSMS].[FKlkpSMSStatusTypeID], [zHstSMS].[FKlkpSMSStatusSubtypeID], [zHstSMS].[StampDate], [zHstSMS].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstSMS].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstSMS] ");
            query.Append("INNER JOIN (SELECT [zHstSMS].[ID], MAX([zHstSMS].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstSMS] ");
            query.Append("WHERE [zHstSMS].[ID] NOT IN (SELECT [SMS].[ID] FROM [SMS]) ");
            query.Append("GROUP BY [zHstSMS].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstSMS].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstSMS].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) sms in the database.
        /// </summary>
        /// <param name="sms">The sms object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) sms in the database.</returns>
        public static string ListHistory(SMS sms, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (sms != null)
            {
            query.Append("SELECT [zHstSMS].[ID], [zHstSMS].[FKSystemID], [zHstSMS].[FKImportID], [zHstSMS].[SMSID], [zHstSMS].[FKlkpSMSTypeID], [zHstSMS].[RecipientCellNum], [zHstSMS].[SMSBody], [zHstSMS].[FKlkpSMSEncodingID], [zHstSMS].[SubmissionID], [zHstSMS].[SubmissionDate], [zHstSMS].[FKlkpSMSStatusTypeID], [zHstSMS].[FKlkpSMSStatusSubtypeID], [zHstSMS].[StampDate], [zHstSMS].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstSMS].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstSMS] ");
                query.Append(" WHERE [zHstSMS].[ID] = @ID");
                query.Append(" ORDER BY [zHstSMS].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", sms.ID);
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
        internal static string Save(SMS sms, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (sms != null)
            {
                if (sms.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstSMS] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [SMS] WHERE [SMS].[ID] = @ID; ");
                    query.Append("UPDATE [SMS]");
                    parameters = new object[12];
                    query.Append(" SET [FKSystemID] = @FKSystemID");
                    parameters[0] = Database.GetParameter("@FKSystemID", sms.FKSystemID.HasValue ? (object)sms.FKSystemID.Value : DBNull.Value);
                    query.Append(", [FKImportID] = @FKImportID");
                    parameters[1] = Database.GetParameter("@FKImportID", sms.FKImportID.HasValue ? (object)sms.FKImportID.Value : DBNull.Value);
                    query.Append(", [SMSID] = @SMSID");
                    parameters[2] = Database.GetParameter("@SMSID", string.IsNullOrEmpty(sms.SMSID) ? DBNull.Value : (object)sms.SMSID);
                    query.Append(", [FKlkpSMSTypeID] = @FKlkpSMSTypeID");
                    parameters[3] = Database.GetParameter("@FKlkpSMSTypeID", sms.FKlkpSMSTypeID.HasValue ? (object)sms.FKlkpSMSTypeID.Value : DBNull.Value);
                    query.Append(", [RecipientCellNum] = @RecipientCellNum");
                    parameters[4] = Database.GetParameter("@RecipientCellNum", string.IsNullOrEmpty(sms.RecipientCellNum) ? DBNull.Value : (object)sms.RecipientCellNum);
                    query.Append(", [SMSBody] = @SMSBody");
                    parameters[5] = Database.GetParameter("@SMSBody", string.IsNullOrEmpty(sms.SMSBody) ? DBNull.Value : (object)sms.SMSBody);
                    query.Append(", [FKlkpSMSEncodingID] = @FKlkpSMSEncodingID");
                    parameters[6] = Database.GetParameter("@FKlkpSMSEncodingID", sms.FKlkpSMSEncodingID.HasValue ? (object)sms.FKlkpSMSEncodingID.Value : DBNull.Value);
                    query.Append(", [SubmissionID] = @SubmissionID");
                    parameters[7] = Database.GetParameter("@SubmissionID", string.IsNullOrEmpty(sms.SubmissionID) ? DBNull.Value : (object)sms.SubmissionID);
                    query.Append(", [SubmissionDate] = @SubmissionDate");
                    parameters[8] = Database.GetParameter("@SubmissionDate", sms.SubmissionDate.HasValue ? (object)sms.SubmissionDate.Value : DBNull.Value);
                    query.Append(", [FKlkpSMSStatusTypeID] = @FKlkpSMSStatusTypeID");
                    parameters[9] = Database.GetParameter("@FKlkpSMSStatusTypeID", sms.FKlkpSMSStatusTypeID.HasValue ? (object)sms.FKlkpSMSStatusTypeID.Value : DBNull.Value);
                    query.Append(", [FKlkpSMSStatusSubtypeID] = @FKlkpSMSStatusSubtypeID");
                    parameters[10] = Database.GetParameter("@FKlkpSMSStatusSubtypeID", sms.FKlkpSMSStatusSubtypeID.HasValue ? (object)sms.FKlkpSMSStatusSubtypeID.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [SMS].[ID] = @ID"); 
                    parameters[11] = Database.GetParameter("@ID", sms.ID);
                }
                else
                {
                    query.Append("INSERT INTO [SMS] ([FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) VALUES(@FKSystemID, @FKImportID, @SMSID, @FKlkpSMSTypeID, @RecipientCellNum, @SMSBody, @FKlkpSMSEncodingID, @SubmissionID, @SubmissionDate, @FKlkpSMSStatusTypeID, @FKlkpSMSStatusSubtypeID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[11];
                    parameters[0] = Database.GetParameter("@FKSystemID", sms.FKSystemID.HasValue ? (object)sms.FKSystemID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@FKImportID", sms.FKImportID.HasValue ? (object)sms.FKImportID.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@SMSID", string.IsNullOrEmpty(sms.SMSID) ? DBNull.Value : (object)sms.SMSID);
                    parameters[3] = Database.GetParameter("@FKlkpSMSTypeID", sms.FKlkpSMSTypeID.HasValue ? (object)sms.FKlkpSMSTypeID.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@RecipientCellNum", string.IsNullOrEmpty(sms.RecipientCellNum) ? DBNull.Value : (object)sms.RecipientCellNum);
                    parameters[5] = Database.GetParameter("@SMSBody", string.IsNullOrEmpty(sms.SMSBody) ? DBNull.Value : (object)sms.SMSBody);
                    parameters[6] = Database.GetParameter("@FKlkpSMSEncodingID", sms.FKlkpSMSEncodingID.HasValue ? (object)sms.FKlkpSMSEncodingID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@SubmissionID", string.IsNullOrEmpty(sms.SubmissionID) ? DBNull.Value : (object)sms.SubmissionID);
                    parameters[8] = Database.GetParameter("@SubmissionDate", sms.SubmissionDate.HasValue ? (object)sms.SubmissionDate.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@FKlkpSMSStatusTypeID", sms.FKlkpSMSStatusTypeID.HasValue ? (object)sms.FKlkpSMSStatusTypeID.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@FKlkpSMSStatusSubtypeID", sms.FKlkpSMSStatusSubtypeID.HasValue ? (object)sms.FKlkpSMSStatusSubtypeID.Value : DBNull.Value);
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
                whereQuery.Append("[SMS].[FKSystemID] = " + fksystemid + "");
            }
            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[FKImportID] = " + fkimportid + "");
            }
            if (smsid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[SMSID] LIKE '" + smsid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fklkpsmstypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[FKlkpSMSTypeID] = " + fklkpsmstypeid + "");
            }
            if (recipientcellnum != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[RecipientCellNum] LIKE '" + recipientcellnum.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (smsbody != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[SMSBody] LIKE '" + smsbody.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fklkpsmsencodingid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[FKlkpSMSEncodingID] = " + fklkpsmsencodingid + "");
            }
            if (submissionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[SubmissionID] LIKE '" + submissionid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (submissiondate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[SubmissionDate] = '" + submissiondate.Value.ToString(Database.DateFormat) + "'");
            }
            if (fklkpsmsstatustypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[FKlkpSMSStatusTypeID] = " + fklkpsmsstatustypeid + "");
            }
            if (fklkpsmsstatussubtypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMS].[FKlkpSMSStatusSubtypeID] = " + fklkpsmsstatussubtypeid + "");
            }
            query.Append("SELECT [SMS].[ID], [SMS].[FKSystemID], [SMS].[FKImportID], [SMS].[SMSID], [SMS].[FKlkpSMSTypeID], [SMS].[RecipientCellNum], [SMS].[SMSBody], [SMS].[FKlkpSMSEncodingID], [SMS].[SubmissionID], [SMS].[SubmissionDate], [SMS].[FKlkpSMSStatusTypeID], [SMS].[FKlkpSMSStatusSubtypeID], [SMS].[StampDate], [SMS].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SMS].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SMS] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
