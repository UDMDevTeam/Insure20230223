using System.Text;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains custom methods that return data related to InImport objects.
    /// </summary>
    internal abstract partial class INImportQueries
    {
        #region Search

        internal static string Search(long? fkuserid, bool? isCopied, long? fkinbatchid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();


            if (fkuserid == null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKUserID] IS NULL");
                if (isCopied == false || isCopied == null)
                {
                    whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                    whereQuery.Append("([INImport].[IsCopied] = 0 OR [INImport].[IsCopied] IS NULL)");
                }
            }
            else
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKUserID] = " + fkuserid.ToString());
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINBatchID] = " + fkinbatchid + "");
            }

            query.Append("SELECT [INImport].[ID], [INImport].[FKUserID], [INImport].[FKINCampaignID], [INImport].[FKINBatchID], [INImport].[FKINPolicyID], [INImport].[FKINLeadID], [INImport].[RefNo], [INImport].[ReferrorPolicyID], [INImport].[FKINReferrorTitleID], [INImport].[Referror], [INImport].[FKINReferrorRelationshipID], [INImport].[ReferrorContact], [INImport].[Gift], [INImport].[PlatinumContactDate], [INImport].[PlatinumContactTime], [INImport].[CancerOption], [INImport].[AllocationDate], [INImport].[IsPrinted], [INImport].[DateOfSale], [INImport].[StampDate], [INImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImport] ");

            return query + whereQuery.ToString();
        }

        internal static string Search(long? fkuserid, long? fkinbatchid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            
            if (fkuserid == null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKUserID] IS NULL ");
            }
            else
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKUserID] = " + fkuserid.ToString());
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINBatchID] = " + fkinbatchid + "");
            }

            query.Append("SELECT [INImport].[ID], [INImport].[FKUserID], [INImport].[FKINCampaignID], [INImport].[FKINBatchID], [INImport].[FKINPolicyID], [INImport].[FKINLeadID], [INImport].[RefNo], [INImport].[ReferrorPolicyID], [INImport].[FKINReferrorTitleID], [INImport].[Referror], [INImport].[FKINReferrorRelationshipID], [INImport].[ReferrorContact], [INImport].[Gift], [INImport].[PlatinumContactDate], [INImport].[PlatinumContactTime], [INImport].[CancerOption], [INImport].[AllocationDate], [INImport].[IsPrinted], [INImport].[DateOfSale], [INImport].[StampDate], [INImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImport] ");

            return query + whereQuery.ToString();
        }

        internal static string Search(long? fkuserid, long? fkinbatchid, bool? isprinted)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkuserid == null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKUserID] IS NULL ");
            }
            else
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKUserID] = " + fkuserid.ToString());
            }
            if (fkinbatchid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImport].[FKINBatchID] = " + fkinbatchid + "");
            }
            if (isprinted != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");

                if ((bool)isprinted)
                {
                    whereQuery.Append("[INImport].[IsPrinted] = 1");
                }
                else
                {
                    whereQuery.Append("([INImport].[IsPrinted] = 0 OR [INImport].[IsPrinted] IS NULL)");
                }
            }

            query.Append("SELECT [INImport].[ID], [INImport].[FKUserID], [INImport].[FKINCampaignID], [INImport].[FKINBatchID], [INImport].[FKINPolicyID], [INImport].[FKINLeadID], [INImport].[RefNo], [INImport].[ReferrorPolicyID], [INImport].[FKINReferrorTitleID], [INImport].[Referror], [INImport].[FKINReferrorRelationshipID], [INImport].[ReferrorContact], [INImport].[Gift], [INImport].[PlatinumContactDate], [INImport].[PlatinumContactTime], [INImport].[CancerOption], [INImport].[AllocationDate], [INImport].[IsPrinted], [INImport].[DateOfSale], [INImport].[StampDate], [INImport].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImport] ");

            return query + whereQuery.ToString();
        }

        #endregion
    }
}
