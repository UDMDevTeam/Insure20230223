using System;
using System.Text;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains custom methods that return data related to inlead objects.
    /// </summary>
    internal abstract partial class INLeadQueries
    {
        #region Search

        internal static string Search(string idno)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (idno != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[IDNo] LIKE '" + idno.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INLead].[ID], [INLead].[IDNo], [INLead].[FKINTitleID], [INLead].[Initials], [INLead].[FirstName], [INLead].[Surname], [INLead].[FKLanguageID], [INLead].[FKGenderID], [INLead].[DateOfBirth], [INLead].[YearOfBirth], [INLead].[TelWork], [INLead].[TelHome], [INLead].[TelCell], [INLead].[Address], [INLead].[Address1], [INLead].[Address2], [INLead].[Address3], [INLead].[Address4], [INLead].[Address5], [INLead].[PostalCode], [INLead].[Email], [INLead].[Occupation], [INLead].[StampDate], [INLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLead] ");

            return "" + query + whereQuery;
        }

        internal static string Search(string firstname, string surname, DateTime? dateofbirth)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

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
            if (dateofbirth != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[DateOfBirth] = '" + dateofbirth.Value.ToString(Database.DateFormat) + "'");
            }
            
            query.Append("SELECT [INLead].[ID], [INLead].[IDNo], [INLead].[FKINTitleID], [INLead].[Initials], [INLead].[FirstName], [INLead].[Surname], [INLead].[FKLanguageID], [INLead].[FKGenderID], [INLead].[DateOfBirth], [INLead].[YearOfBirth], [INLead].[TelWork], [INLead].[TelHome], [INLead].[TelCell], [INLead].[Address], [INLead].[Address1], [INLead].[Address2], [INLead].[Address3], [INLead].[Address4], [INLead].[Address5], [INLead].[PostalCode], [INLead].[Email], [INLead].[Occupation], [INLead].[StampDate], [INLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLead] ");

            return "" + query + whereQuery;
        }

        internal static string Search(string firstname, string surname, string strTelCell)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

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
            if (strTelCell != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INLead].[TelCell] = '" + strTelCell + "'");
            }

            query.Append("SELECT [INLead].[ID], [INLead].[IDNo], [INLead].[FKINTitleID], [INLead].[Initials], [INLead].[FirstName], [INLead].[Surname], [INLead].[FKLanguageID], [INLead].[FKGenderID], [INLead].[DateOfBirth], [INLead].[YearOfBirth], [INLead].[TelWork], [INLead].[TelHome], [INLead].[TelCell], [INLead].[Address], [INLead].[Address1], [INLead].[Address2], [INLead].[Address3], [INLead].[Address4], [INLead].[Address5], [INLead].[PostalCode], [INLead].[Email], [INLead].[Occupation], [INLead].[StampDate], [INLead].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INLead].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INLead] ");

            return "" + query + whereQuery;
        }

        #endregion
    }
}
