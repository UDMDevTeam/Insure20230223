using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inlead objects.
    /// </summary>
    internal abstract partial class INLeadQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inlead from the database.
        /// </summary>
        /// <param name="inlead">The inlead object to delete.</param>
        /// <returns>A query that can be used to delete the inlead from the database.</returns>
        internal static string Delete(INLead inlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlead != null)
            {
                query = "INSERT INTO [zHstINLead] ([ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID] FROM [INLead] WHERE [INLead].[ID] = @ID; ";
                query += "DELETE FROM [INLead] WHERE [INLead].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlead.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inlead from the database.
        /// </summary>
        /// <param name="inlead">The inlead object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inlead from the database.</returns>
        internal static string DeleteHistory(INLead inlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlead != null)
            {
                query = "DELETE FROM [zHstINLead] WHERE [zHstINLead].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlead.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inlead from the database.
        /// </summary>
        /// <param name="inlead">The inlead object to undelete.</param>
        /// <returns>A query that can be used to undelete the inlead from the database.</returns>
        internal static string UnDelete(INLead inlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlead != null)
            {
                query = "INSERT INTO [INLead] ([ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID] FROM [zHstINLead] WHERE [zHstINLead].[ID] = @ID AND [zHstINLead].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLead] WHERE [zHstINLead].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INLead] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINLead] WHERE [zHstINLead].[ID] = @ID AND [zHstINLead].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINLead] WHERE [zHstINLead].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INLead] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlead.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inlead object.
        /// </summary>
        /// <param name="inlead">The inlead object to fill.</param>
        /// <returns>A query that can be used to fill the inlead object.</returns>
        internal static string Fill(INLead inlead, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlead != null)
            {
                query = "SELECT [ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID] FROM [INLead] WHERE [INLead].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlead.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inlead data.
        /// </summary>
        /// <param name="inlead">The inlead to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inlead data.</returns>
        internal static string FillData(INLead inlead, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inlead != null)
            {
            query.Append("SELECT [INLead].[ID], [INLead].[IDNo], [INLead].[PassportNo], [INLead].[FKINTitleID], [INLead].[Initials], [INLead].[FirstName], [INLead].[Surname], [INLead].[FKLanguageID], [INLead].[FKGenderID], [INLead].[DateOfBirth], [INLead].[YearOfBirth], [INLead].[TelWork], [INLead].[TelHome], [INLead].[TelCell], [INLead].[TelOther], [INLead].[Address], [INLead].[Address1], [INLead].[Address2], [INLead].[Address3], [INLead].[Address4], [INLead].[Address5], [INLead].[PostalCode], [INLead].[Email], [INLead].[Occupation], [INLead].[StampDate], [INLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLead] ");
                query.Append(" WHERE [INLead].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlead.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inlead object from history.
        /// </summary>
        /// <param name="inlead">The inlead object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inlead object from history.</returns>
        internal static string FillHistory(INLead inlead, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inlead != null)
            {
                query = "SELECT [ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID] FROM [zHstINLead] WHERE [zHstINLead].[ID] = @ID AND [zHstINLead].[StampUserID] = @StampUserID AND [zHstINLead].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inlead.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inleads in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inleads in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INLead].[ID], [INLead].[IDNo], [INLead].[PassportNo], [INLead].[FKINTitleID], [INLead].[Initials], [INLead].[FirstName], [INLead].[Surname], [INLead].[FKLanguageID], [INLead].[FKGenderID], [INLead].[DateOfBirth], [INLead].[YearOfBirth], [INLead].[TelWork], [INLead].[TelHome], [INLead].[TelCell], [INLead].[TelOther], [INLead].[Address], [INLead].[Address1], [INLead].[Address2], [INLead].[Address3], [INLead].[Address4], [INLead].[Address5], [INLead].[PostalCode], [INLead].[Email], [INLead].[Occupation], [INLead].[StampDate], [INLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLead] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inleads in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inleads in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINLead].[ID], [zHstINLead].[IDNo], [zHstINLead].[PassportNo], [zHstINLead].[FKINTitleID], [zHstINLead].[Initials], [zHstINLead].[FirstName], [zHstINLead].[Surname], [zHstINLead].[FKLanguageID], [zHstINLead].[FKGenderID], [zHstINLead].[DateOfBirth], [zHstINLead].[YearOfBirth], [zHstINLead].[TelWork], [zHstINLead].[TelHome], [zHstINLead].[TelCell], [zHstINLead].[TelOther], [zHstINLead].[Address], [zHstINLead].[Address1], [zHstINLead].[Address2], [zHstINLead].[Address3], [zHstINLead].[Address4], [zHstINLead].[Address5], [zHstINLead].[PostalCode], [zHstINLead].[Email], [zHstINLead].[Occupation], [zHstINLead].[StampDate], [zHstINLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINLead] ");
            query.Append("INNER JOIN (SELECT [zHstINLead].[ID], MAX([zHstINLead].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINLead] ");
            query.Append("WHERE [zHstINLead].[ID] NOT IN (SELECT [INLead].[ID] FROM [INLead]) ");
            query.Append("GROUP BY [zHstINLead].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINLead].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINLead].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inlead in the database.
        /// </summary>
        /// <param name="inlead">The inlead object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inlead in the database.</returns>
        public static string ListHistory(INLead inlead, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inlead != null)
            {
            query.Append("SELECT [zHstINLead].[ID], [zHstINLead].[IDNo], [zHstINLead].[PassportNo], [zHstINLead].[FKINTitleID], [zHstINLead].[Initials], [zHstINLead].[FirstName], [zHstINLead].[Surname], [zHstINLead].[FKLanguageID], [zHstINLead].[FKGenderID], [zHstINLead].[DateOfBirth], [zHstINLead].[YearOfBirth], [zHstINLead].[TelWork], [zHstINLead].[TelHome], [zHstINLead].[TelCell], [zHstINLead].[TelOther], [zHstINLead].[Address], [zHstINLead].[Address1], [zHstINLead].[Address2], [zHstINLead].[Address3], [zHstINLead].[Address4], [zHstINLead].[Address5], [zHstINLead].[PostalCode], [zHstINLead].[Email], [zHstINLead].[Occupation], [zHstINLead].[StampDate], [zHstINLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINLead] ");
                query.Append(" WHERE [zHstINLead].[ID] = @ID");
                query.Append(" ORDER BY [zHstINLead].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inlead.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inlead to the database.
        /// </summary>
        /// <param name="inlead">The inlead to save.</param>
        /// <returns>A query that can be used to save the inlead to the database.</returns>
        internal static string Save(INLead inlead, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inlead != null)
            {
                if (inlead.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINLead] ([ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID]) SELECT [ID], [IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID] FROM [INLead] WHERE [INLead].[ID] = @ID; ");
                    query.Append("UPDATE [INLead]");
                    parameters = new object[24];
                    query.Append(" SET [IDNo] = @IDNo");
                    parameters[0] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(inlead.IDNo) ? DBNull.Value : (object)inlead.IDNo);
                    query.Append(", [PassportNo] = @PassportNo");
                    parameters[1] = Database.GetParameter("@PassportNo", string.IsNullOrEmpty(inlead.PassportNo) ? DBNull.Value : (object)inlead.PassportNo);
                    query.Append(", [FKINTitleID] = @FKINTitleID");
                    parameters[2] = Database.GetParameter("@FKINTitleID", inlead.FKINTitleID.HasValue ? (object)inlead.FKINTitleID.Value : DBNull.Value);
                    query.Append(", [Initials] = @Initials");
                    parameters[3] = Database.GetParameter("@Initials", string.IsNullOrEmpty(inlead.Initials) ? DBNull.Value : (object)inlead.Initials);
                    query.Append(", [FirstName] = @FirstName");
                    parameters[4] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inlead.FirstName) ? DBNull.Value : (object)inlead.FirstName);
                    query.Append(", [Surname] = @Surname");
                    parameters[5] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inlead.Surname) ? DBNull.Value : (object)inlead.Surname);
                    query.Append(", [FKLanguageID] = @FKLanguageID");
                    parameters[6] = Database.GetParameter("@FKLanguageID", inlead.FKLanguageID.HasValue ? (object)inlead.FKLanguageID.Value : DBNull.Value);
                    query.Append(", [FKGenderID] = @FKGenderID");
                    parameters[7] = Database.GetParameter("@FKGenderID", inlead.FKGenderID.HasValue ? (object)inlead.FKGenderID.Value : DBNull.Value);
                    query.Append(", [DateOfBirth] = @DateOfBirth");
                    parameters[8] = Database.GetParameter("@DateOfBirth", inlead.DateOfBirth.HasValue ? (object)inlead.DateOfBirth.Value : DBNull.Value);
                    query.Append(", [YearOfBirth] = @YearOfBirth");
                    parameters[9] = Database.GetParameter("@YearOfBirth", string.IsNullOrEmpty(inlead.YearOfBirth) ? DBNull.Value : (object)inlead.YearOfBirth);
                    query.Append(", [TelWork] = @TelWork");
                    parameters[10] = Database.GetParameter("@TelWork", string.IsNullOrEmpty(inlead.TelWork) ? DBNull.Value : (object)inlead.TelWork);
                    query.Append(", [TelHome] = @TelHome");
                    parameters[11] = Database.GetParameter("@TelHome", string.IsNullOrEmpty(inlead.TelHome) ? DBNull.Value : (object)inlead.TelHome);
                    query.Append(", [TelCell] = @TelCell");
                    parameters[12] = Database.GetParameter("@TelCell", string.IsNullOrEmpty(inlead.TelCell) ? DBNull.Value : (object)inlead.TelCell);
                    query.Append(", [TelOther] = @TelOther");
                    parameters[13] = Database.GetParameter("@TelOther", string.IsNullOrEmpty(inlead.TelOther) ? DBNull.Value : (object)inlead.TelOther);
                    query.Append(", [Address] = @Address");
                    parameters[14] = Database.GetParameter("@Address", string.IsNullOrEmpty(inlead.Address) ? DBNull.Value : (object)inlead.Address);
                    query.Append(", [Address1] = @Address1");
                    parameters[15] = Database.GetParameter("@Address1", string.IsNullOrEmpty(inlead.Address1) ? DBNull.Value : (object)inlead.Address1);
                    query.Append(", [Address2] = @Address2");
                    parameters[16] = Database.GetParameter("@Address2", string.IsNullOrEmpty(inlead.Address2) ? DBNull.Value : (object)inlead.Address2);
                    query.Append(", [Address3] = @Address3");
                    parameters[17] = Database.GetParameter("@Address3", string.IsNullOrEmpty(inlead.Address3) ? DBNull.Value : (object)inlead.Address3);
                    query.Append(", [Address4] = @Address4");
                    parameters[18] = Database.GetParameter("@Address4", string.IsNullOrEmpty(inlead.Address4) ? DBNull.Value : (object)inlead.Address4);
                    query.Append(", [Address5] = @Address5");
                    parameters[19] = Database.GetParameter("@Address5", string.IsNullOrEmpty(inlead.Address5) ? DBNull.Value : (object)inlead.Address5);
                    query.Append(", [PostalCode] = @PostalCode");
                    parameters[20] = Database.GetParameter("@PostalCode", string.IsNullOrEmpty(inlead.PostalCode) ? DBNull.Value : (object)inlead.PostalCode);
                    query.Append(", [Email] = @Email");
                    parameters[21] = Database.GetParameter("@Email", string.IsNullOrEmpty(inlead.Email) ? DBNull.Value : (object)inlead.Email);
                    query.Append(", [Occupation] = @Occupation");
                    parameters[22] = Database.GetParameter("@Occupation", string.IsNullOrEmpty(inlead.Occupation) ? DBNull.Value : (object)inlead.Occupation);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INLead].[ID] = @ID"); 
                    parameters[23] = Database.GetParameter("@ID", inlead.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INLead] ([IDNo], [PassportNo], [FKINTitleID], [Initials], [FirstName], [Surname], [FKLanguageID], [FKGenderID], [DateOfBirth], [YearOfBirth], [TelWork], [TelHome], [TelCell], [TelOther], [Address], [Address1], [Address2], [Address3], [Address4], [Address5], [PostalCode], [Email], [Occupation], [StampDate], [StampUserID]) VALUES(@IDNo, @PassportNo, @FKINTitleID, @Initials, @FirstName, @Surname, @FKLanguageID, @FKGenderID, @DateOfBirth, @YearOfBirth, @TelWork, @TelHome, @TelCell, @TelOther, @Address, @Address1, @Address2, @Address3, @Address4, @Address5, @PostalCode, @Email, @Occupation, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[23];
                    parameters[0] = Database.GetParameter("@IDNo", string.IsNullOrEmpty(inlead.IDNo) ? DBNull.Value : (object)inlead.IDNo);
                    parameters[1] = Database.GetParameter("@PassportNo", string.IsNullOrEmpty(inlead.PassportNo) ? DBNull.Value : (object)inlead.PassportNo);
                    parameters[2] = Database.GetParameter("@FKINTitleID", inlead.FKINTitleID.HasValue ? (object)inlead.FKINTitleID.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@Initials", string.IsNullOrEmpty(inlead.Initials) ? DBNull.Value : (object)inlead.Initials);
                    parameters[4] = Database.GetParameter("@FirstName", string.IsNullOrEmpty(inlead.FirstName) ? DBNull.Value : (object)inlead.FirstName);
                    parameters[5] = Database.GetParameter("@Surname", string.IsNullOrEmpty(inlead.Surname) ? DBNull.Value : (object)inlead.Surname);
                    parameters[6] = Database.GetParameter("@FKLanguageID", inlead.FKLanguageID.HasValue ? (object)inlead.FKLanguageID.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@FKGenderID", inlead.FKGenderID.HasValue ? (object)inlead.FKGenderID.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@DateOfBirth", inlead.DateOfBirth.HasValue ? (object)inlead.DateOfBirth.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@YearOfBirth", string.IsNullOrEmpty(inlead.YearOfBirth) ? DBNull.Value : (object)inlead.YearOfBirth);
                    parameters[10] = Database.GetParameter("@TelWork", string.IsNullOrEmpty(inlead.TelWork) ? DBNull.Value : (object)inlead.TelWork);
                    parameters[11] = Database.GetParameter("@TelHome", string.IsNullOrEmpty(inlead.TelHome) ? DBNull.Value : (object)inlead.TelHome);
                    parameters[12] = Database.GetParameter("@TelCell", string.IsNullOrEmpty(inlead.TelCell) ? DBNull.Value : (object)inlead.TelCell);
                    parameters[13] = Database.GetParameter("@TelOther", string.IsNullOrEmpty(inlead.TelOther) ? DBNull.Value : (object)inlead.TelOther);
                    parameters[14] = Database.GetParameter("@Address", string.IsNullOrEmpty(inlead.Address) ? DBNull.Value : (object)inlead.Address);
                    parameters[15] = Database.GetParameter("@Address1", string.IsNullOrEmpty(inlead.Address1) ? DBNull.Value : (object)inlead.Address1);
                    parameters[16] = Database.GetParameter("@Address2", string.IsNullOrEmpty(inlead.Address2) ? DBNull.Value : (object)inlead.Address2);
                    parameters[17] = Database.GetParameter("@Address3", string.IsNullOrEmpty(inlead.Address3) ? DBNull.Value : (object)inlead.Address3);
                    parameters[18] = Database.GetParameter("@Address4", string.IsNullOrEmpty(inlead.Address4) ? DBNull.Value : (object)inlead.Address4);
                    parameters[19] = Database.GetParameter("@Address5", string.IsNullOrEmpty(inlead.Address5) ? DBNull.Value : (object)inlead.Address5);
                    parameters[20] = Database.GetParameter("@PostalCode", string.IsNullOrEmpty(inlead.PostalCode) ? DBNull.Value : (object)inlead.PostalCode);
                    parameters[21] = Database.GetParameter("@Email", string.IsNullOrEmpty(inlead.Email) ? DBNull.Value : (object)inlead.Email);
                    parameters[22] = Database.GetParameter("@Occupation", string.IsNullOrEmpty(inlead.Occupation) ? DBNull.Value : (object)inlead.Occupation);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inleads that match the search criteria.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="passportno">The passportno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="yearofbirth">The yearofbirth search criteria.</param>
        /// <param name="telwork">The telwork search criteria.</param>
        /// <param name="telhome">The telhome search criteria.</param>
        /// <param name="telcell">The telcell search criteria.</param>
        /// <param name="telother">The telother search criteria.</param>
        /// <param name="address">The address search criteria.</param>
        /// <param name="address1">The address1 search criteria.</param>
        /// <param name="address2">The address2 search criteria.</param>
        /// <param name="address3">The address3 search criteria.</param>
        /// <param name="address4">The address4 search criteria.</param>
        /// <param name="address5">The address5 search criteria.</param>
        /// <param name="postalcode">The postalcode search criteria.</param>
        /// <param name="email">The email search criteria.</param>
        /// <param name="occupation">The occupation search criteria.</param>
        /// <returns>A query that can be used to search for inleads based on the search criteria.</returns>
        internal static string Search(string idno, string passportno, long? fkintitleid, string initials, string firstname, string surname, long? fklanguageid, long? fkgenderid, DateTime? dateofbirth, string yearofbirth, string telwork, string telhome, string telcell, string telother, string address, string address1, string address2, string address3, string address4, string address5, string postalcode, string email, string occupation)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (idno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[IDNo] LIKE '" + idno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (passportno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[PassportNo] LIKE '" + passportno.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fkintitleid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[FKINTitleID] = " + fkintitleid + "");
            }
            if (initials != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Initials] LIKE '" + initials.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (firstname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[FirstName] LIKE '" + firstname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (surname != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Surname] LIKE '" + surname.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (fklanguageid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[FKLanguageID] = " + fklanguageid + "");
            }
            if (fkgenderid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[FKGenderID] = " + fkgenderid + "");
            }
            if (dateofbirth != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[DateOfBirth] = '" + dateofbirth.Value.ToString(Database.DateFormat) + "'");
            }
            if (yearofbirth != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[YearOfBirth] LIKE '" + yearofbirth.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telwork != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[TelWork] LIKE '" + telwork.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telhome != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[TelHome] LIKE '" + telhome.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telcell != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[TelCell] LIKE '" + telcell.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (telother != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[TelOther] LIKE '" + telother.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (address != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Address] LIKE '" + address.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (address1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Address1] LIKE '" + address1.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (address2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Address2] LIKE '" + address2.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (address3 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Address3] LIKE '" + address3.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (address4 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Address4] LIKE '" + address4.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (address5 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Address5] LIKE '" + address5.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (postalcode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[PostalCode] LIKE '" + postalcode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (email != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Email] LIKE '" + email.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (occupation != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[Occupation] LIKE '" + occupation.Replace("'", "''").Replace("*", "%") + "'");
            }
            query.Append("SELECT [INLead].[ID], [INLead].[IDNo], [INLead].[PassportNo], [INLead].[FKINTitleID], [INLead].[Initials], [INLead].[FirstName], [INLead].[Surname], [INLead].[FKLanguageID], [INLead].[FKGenderID], [INLead].[DateOfBirth], [INLead].[YearOfBirth], [INLead].[TelWork], [INLead].[TelHome], [INLead].[TelCell], [INLead].[TelOther], [INLead].[Address], [INLead].[Address1], [INLead].[Address2], [INLead].[Address3], [INLead].[Address4], [INLead].[Address5], [INLead].[PostalCode], [INLead].[Email], [INLead].[Occupation], [INLead].[StampDate], [INLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLead] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
