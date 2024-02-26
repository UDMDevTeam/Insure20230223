using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace UDM.Insurance.Business.Queries
{
    internal abstract partial class WelcomeMessageQueries
    {
        #region Delete
        internal static string Delete(WelcomMessage welcomeMessage, ref object[] parameters)
        {
            string query = "DELETE FROM [WelcomeMessage] WHERE [ID] = @ID;";
            parameters = new[] { new SqlParameter("@ID", welcomeMessage.ID) };
            return query;
        }

        internal static string DeleteHistory(WelcomMessage welcomeMessage, ref object[] parameters)
        {
            string query = "DELETE FROM [WelcomeMessageHistory] WHERE [ID] = @ID";
            parameters = new[] { new SqlParameter("@ID", welcomeMessage.ID) };
            return query;
        }

        internal static string UnDelete(WelcomMessage welcomeMessage, ref object[] parameters)
        {
            string query = @"IF NOT EXISTS (SELECT * FROM [WelcomeMessage] WHERE [ID] = @ID)
                             BEGIN
                                 INSERT INTO [WelcomeMessage] ([ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName])
                                 SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName]
                                 FROM [WelcomeMessageHistory]
                                 WHERE [ID] = @ID;
                             END";
            parameters = new[] { new SqlParameter("@ID", welcomeMessage.ID) };
            return query;
        }
        #endregion

        #region Fill
        internal static string Fill(WelcomMessage welcomeMessage, ref object[] parameters)
        {
            string query = "SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName] FROM [WelcomeMessage] WHERE [ID] = @ID";
            parameters = new[] { new SqlParameter("@ID", welcomeMessage.ID) };
            return query;
        }

        internal static string FillData(WelcomMessage welcomeMessage, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName] ");
            query.Append("FROM [WelcomeMessage] WHERE [ID] = @ID");
            parameters = new[] { new SqlParameter("@ID", welcomeMessage.ID) };
            return query.ToString();
        }

        internal static string FillHistory(WelcomMessage welcomeMessage, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = @"SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName]
                             FROM [WelcomeMessageHistory]
                             WHERE [ID] = @ID AND [StampUserID] = @StampUserID AND [StampDate] = @StampDate";
            parameters = new[] {
                new SqlParameter("@ID", welcomeMessage.ID),
                new SqlParameter("@StampUserID", stampUserID),
                new SqlParameter("@StampDate", stampDate)
            };
            return query;
        }
        #endregion

        #region List
        internal static string List()
        {
            return "SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName] FROM [WelcomeMessage]";
        }

        internal static string ListDeleted()
        {
            return "SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName] FROM [WelcomeMessageHistory]";
        }

        public static string ListHistory(WelcomMessage welcomeMessage, ref object[] parameters)
        {
            string query = @"SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName]
                             FROM [WelcomeMessageHistory]
                             WHERE [ID] = @ID
                             ORDER BY [StampDate] DESC";
            parameters = new[] { new SqlParameter("@ID", welcomeMessage.ID) };
            return query;
        }
        #endregion

        #region Save
        internal static string Save(WelcomMessage welcomeMessage, ref object[] parameters)
        {
            if (welcomeMessage.ID == 0)
            {
                // Assuming a new record
                string query = "INSERT INTO [WelcomeMessage] ([TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName]) VALUES (@TextMessage, @StampDate, @StampUserID, @IsActive, @PDFName); SELECT SCOPE_IDENTITY();";
                parameters = new object[] {
                    new SqlParameter("@TextMessage", welcomeMessage.TextMessage ?? (object)DBNull.Value),
                    new SqlParameter("@StampDate", welcomeMessage.StampDate),
                    new SqlParameter("@StampUserID", welcomeMessage.StampUserID),
                    new SqlParameter("@IsActive", welcomeMessage.IsActive),
                    new SqlParameter("@PDFName", welcomeMessage.PDFName ?? (object)DBNull.Value)
                };
                return query;
            }
            else
            {
                // Updating an existing record
                string query = "UPDATE [WelcomeMessage] SET [TextMessage] = @TextMessage, [StampDate] = @StampDate, [StampUserID] = @StampUserID, [IsActive] = @IsActive, [PDFName] = @PDFName WHERE [ID] = @ID;";
                parameters = new object[] {
                    new SqlParameter("@ID", welcomeMessage.ID),
                    new SqlParameter("@TextMessage", welcomeMessage.TextMessage ?? (object)DBNull.Value),
                    new SqlParameter("@StampDate", welcomeMessage.StampDate),
                    new SqlParameter("@StampUserID", welcomeMessage.StampUserID),
                    new SqlParameter("@IsActive", welcomeMessage.IsActive),
                    new SqlParameter("@PDFName", welcomeMessage.PDFName ?? (object)DBNull.Value)
                };
                return query;
            }
        }
        #endregion

        #region Search
        internal static string Search(string textMessage, bool? isActive, ref object[] parameters)
        {
            List<SqlParameter> paramsList = new List<SqlParameter>();
            StringBuilder query = new StringBuilder("SELECT [ID], [TextMessage], [StampDate], [StampUserID], [IsActive], [PDFName] FROM [WelcomeMessage] WHERE 1=1");

            if (!string.IsNullOrEmpty(textMessage))
            {
                query.Append(" AND [TextMessage] LIKE @TextMessage");
                paramsList.Add(new SqlParameter("@TextMessage", $"%{textMessage}%"));
            }

            if (isActive.HasValue)
            {
                query.Append(" AND [IsActive] = @IsActive");
                paramsList.Add(new SqlParameter("@IsActive", isActive.Value));
            }

            parameters = paramsList.ToArray();
            return query.ToString();
        }

        #endregion
    }
}
