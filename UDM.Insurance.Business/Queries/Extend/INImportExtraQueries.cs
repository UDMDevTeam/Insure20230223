using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDM.Insurance.Business.Queries
{
    internal abstract partial class INImportExtraQueries
    {
        #region Search

        internal static string Search(long? fkinimportid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportExtra].[FKINImportID] = " + fkinimportid + "");
            }
            //if (extension1 != null)
            //{
            //    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
            //    whereQuery.Append("[INImportExtra].[Extension1] LIKE '" + extension1.Replace("'", "''").Replace("*", "%") + "'");
            //}
            //if (extension2 != null)
            //{
            //    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
            //    whereQuery.Append("[INImportExtra].[Extension2] LIKE '" + extension2.Replace("'", "''").Replace("*", "%") + "'");
            //}
            //if (notpossiblebumpup != null)
            //{
            //    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
            //    whereQuery.Append("[INImportExtra].[NotPossibleBumpUp] = " + ((bool)notpossiblebumpup ? "1" : "0"));
            //}
            //if (fkcmcallrefuserid != null)
            //{
            //    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
            //    whereQuery.Append("[INImportExtra].[FKCMCallRefUserID] = " + fkcmcallrefuserid + "");
            //}
            //if (emailrequested != null)
            //{
            //    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
            //    whereQuery.Append("[INImportExtra].[EMailRequested] = " + ((bool)emailrequested ? "1" : "0"));
            //}
            //if (customercarerequested != null)
            //{
            //    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
            //    whereQuery.Append("[INImportExtra].[CustomerCareRequested] = " + ((bool)customercarerequested ? "1" : "0"));
            //}
            query.Append("SELECT [INImportExtra].[ID], [INImportExtra].[FKINImportID], [INImportExtra].[Extension1], [INImportExtra].[Extension2], [INImportExtra].[NotPossibleBumpUp], [INImportExtra].[FKCMCallRefUserID], [INImportExtra].[EMailRequested], [INImportExtra].[CustomerCareRequested], [INImportExtra].[IsGoldenLead], [INImportExtra].[StampDate], [INImportExtra].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportExtra].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportExtra] ");
            return query.ToString() + whereQuery.ToString();
        }

        #endregion
    }
}
