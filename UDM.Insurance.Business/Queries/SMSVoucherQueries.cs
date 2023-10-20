using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDM.Insurance.Business.Objects;

namespace UDM.Insurance.Business.Queries
{
    internal abstract partial class SMSVoucherQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) sms from the database.
        /// </summary>
        /// <param name="sms">The sms object to delete.</param>
        /// <returns>A query that can be used to delete the sms from the database.</returns>
        internal static string Delete(SMSVoucher sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "INSERT INTO [zHstSMSVoucher] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [SMS] WHERE [SMS].[ID] = @ID; ";
                query += "DELETE FROM [SMSVoucher] WHERE [SMSVoucher].[ID] = @ID; ";
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
        internal static string DeleteHistory(SMSVoucher sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "DELETE FROM [zHstSMSVoucher] WHERE [zHstSMS].[ID] = @ID";
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
        internal static string UnDelete(SMSVoucher sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "INSERT INTO [SMSVoucher] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [zHstSMSVoucher] WHERE [zHstSMSVoucher].[ID] = @ID AND [zHstSMSVoucher].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstSMSVoucher] WHERE [zHstSMSVoucher].[ID] = @ID) AND (SELECT COUNT(ID) FROM [SMSVoucher] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstSMSVoucher] WHERE [zHstSMSVoucher].[ID] = @ID AND [zHstSMSVoucher].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstSMSVoucher] WHERE [zHstSMSVoucher].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [SMSVoucher] WHERE [ID] = @ID) = 0; ";
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
        internal static string Fill(SMSVoucher sms, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [SMSVoucher] WHERE [SMSVoucher].[ID] = @ID";
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
        internal static string FillData(SMSVoucher sms, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (sms != null)
            {
                query.Append("SELECT [SMSVoucher].[ID], [SMSVoucher].[FKSystemID], [SMSVoucher].[FKImportID], [SMSVoucher].[SMSID], [SMSVoucher].[FKlkpSMSTypeID], [SMSVoucher].[RecipientCellNum], [SMSVoucher].[SMSBody], [SMSVoucher].[FKlkpSMSEncodingID], [SMSVoucher].[SubmissionID], [SMSVoucher].[SubmissionDate], [SMSVoucher].[FKlkpSMSStatusTypeID], [SMSVoucher].[FKlkpSMSStatusSubtypeID], [SMSVoucher].[StampDate], [SMSVoucher].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SMSVoucher].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [SMSVoucher] ");
                query.Append(" WHERE [SMSVoucher].[ID] = @ID");
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
        internal static string FillHistory(SMSVoucher sms, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (sms != null)
            {
                query = "SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [zHstSMSVoucher] WHERE [zHstSMSVoucher].[ID] = @ID AND [zHstSMSVoucher].[StampUserID] = @StampUserID AND [zHstSMSVoucher].[StampDate] = @StampDate";
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
            query.Append("SELECT [SMSVoucher].[ID], [SMSVoucher].[FKSystemID], [SMSVoucher].[FKImportID], [SMSVoucher].[SMSID], [SMSVoucher].[FKlkpSMSTypeID], [SMSVoucher].[RecipientCellNum], [SMSVoucher].[SMSBody], [SMSVoucher].[FKlkpSMSEncodingID], [SMSVoucher].[SubmissionID], [SMSVoucher].[SubmissionDate], [SMSVoucher].[FKlkpSMSStatusTypeID], [SMSVoucher].[FKlkpSMSStatusSubtypeID], [SMSVoucher].[StampDate], [SMSVoucher].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SMSVoucher].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SMSVoucher] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted smss in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted smss in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstSMSVoucher].[ID], [zHstSMSVoucher].[FKSystemID], [zHstSMSVoucher].[FKImportID], [zHstSMSVoucher].[SMSID], [zHstSMSVoucher].[FKlkpSMSTypeID], [zHstSMSVoucher].[RecipientCellNum], [zHstSMSVoucher].[SMSBody], [zHstSMSVoucher].[FKlkpSMSEncodingID], [zHstSMSVoucher].[SubmissionID], [zHstSMSVoucher].[SubmissionDate], [zHstSMSVoucher].[FKlkpSMSStatusTypeID], [zHstSMSVoucher].[FKlkpSMSStatusSubtypeID], [zHstSMSVoucher].[StampDate], [zHstSMSVoucher].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstSMSVoucher].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstSMS] ");
            query.Append("INNER JOIN (SELECT [zHstSMSVoucher].[ID], MAX([zHstSMSVoucher].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstSMSVoucher] ");
            query.Append("WHERE [zHstSMSVoucher].[ID] NOT IN (SELECT [SMSVoucher].[ID] FROM [SMSVoucher]) ");
            query.Append("GROUP BY [zHstSMSVoucher].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstSMSVoucher].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstSMSVoucher].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) sms in the database.
        /// </summary>
        /// <param name="sms">The sms object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) sms in the database.</returns>
        public static string ListHistory(SMSVoucher sms, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (sms != null)
            {
                query.Append("SELECT [zHstSMSVoucher].[ID], [zHstSMSVoucher].[FKSystemID], [zHstSMSVoucher].[FKImportID], [zHstSMSVoucher].[SMSID], [zHstSMSVoucher].[FKlkpSMSTypeID], [zHstSMSVoucher].[RecipientCellNum], [zHstSMSVoucher].[SMSBody], [zHstSMSVoucher].[FKlkpSMSEncodingID], [zHstSMSVoucher].[SubmissionID], [zHstSMSVoucher].[SubmissionDate], [zHstSMSVoucher].[FKlkpSMSStatusTypeID], [zHstSMSVoucher].[FKlkpSMSStatusSubtypeID], [zHstSMSVoucher].[StampDate], [zHstSMSVoucher].[StampUserID]");
                query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstSMSVoucher].[StampUserID]) AS 'StampUser'");
                query.Append(" FROM [zHstSMSVoucher] ");
                query.Append(" WHERE [zHstSMSVoucher].[ID] = @ID");
                query.Append(" ORDER BY [zHstSMSVoucher].[StampDate] DESC");
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
        internal static string Save(SMSVoucher sms, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (sms != null)
            {
                if (sms.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstSMSVoucher] ([ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) SELECT [ID], [FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID] FROM [SMSVoucher] WHERE [SMSVoucher].[ID] = @ID; ");
                    query.Append("UPDATE [SMSVoucher]");
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
                    query.Append(" WHERE [SMSVoucher].[ID] = @ID");
                    parameters[11] = Database.GetParameter("@ID", sms.ID);
                }
                else
                {
                    query.Append("INSERT INTO [SMSVoucher] ([FKSystemID], [FKImportID], [SMSID], [FKlkpSMSTypeID], [RecipientCellNum], [SMSBody], [FKlkpSMSEncodingID], [SubmissionID], [SubmissionDate], [FKlkpSMSStatusTypeID], [FKlkpSMSStatusSubtypeID], [StampDate], [StampUserID]) VALUES(@FKSystemID, @FKImportID, @SMSID, @FKlkpSMSTypeID, @RecipientCellNum, @SMSBody, @FKlkpSMSEncodingID, @SubmissionID, @SubmissionDate, @FKlkpSMSStatusTypeID, @FKlkpSMSStatusSubtypeID, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
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
                whereQuery.Append("[SMSVoucher].[FKSystemID] = " + fksystemid + "");
            }
            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[FKImportID] = " + fkimportid + "");
            }
            if (smsid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[SMSID] LIKE '" + smsid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fklkpsmstypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[FKlkpSMSTypeID] = " + fklkpsmstypeid + "");
            }
            if (recipientcellnum != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[RecipientCellNum] LIKE '" + recipientcellnum.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (smsbody != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[SMSBody] LIKE '" + smsbody.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fklkpsmsencodingid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[FKlkpSMSEncodingID] = " + fklkpsmsencodingid + "");
            }
            if (submissionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[SubmissionID] LIKE '" + submissionid.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (submissiondate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[SubmissionDate] = '" + submissiondate.Value.ToString(Database.DateFormat) + "'");
            }
            if (fklkpsmsstatustypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[FKlkpSMSStatusTypeID] = " + fklkpsmsstatustypeid + "");
            }
            if (fklkpsmsstatussubtypeid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[SMSVoucher].[FKlkpSMSStatusSubtypeID] = " + fklkpsmsstatussubtypeid + "");
            }
            query.Append("SELECT [SMSVoucher].[ID], [SMSVoucher].[FKSystemID], [SMSVoucher].[FKImportID], [SMSVoucher].[SMSID], [SMSVoucher].[FKlkpSMSTypeID], [SMSVoucher].[RecipientCellNum], [SMSVoucher].[SMSBody], [SMSVoucher].[FKlkpSMSEncodingID], [SMSVoucher].[SubmissionID], [SMSVoucher].[SubmissionDate], [SMSVoucher].[FKlkpSMSStatusTypeID], [SMSVoucher].[FKlkpSMSStatusSubtypeID], [SMSVoucher].[StampDate], [SMSVoucher].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [SMSVoucher].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [SMSVoucher] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
