using System.Text;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains custom methods that return data related to inpolicy objects.
    /// </summary>
    internal abstract partial class INPolicyQueries
    {
        #region Search
        
        internal static string Search(string policyid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (policyid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INPolicy].[PolicyID] LIKE '" + policyid.Replace("'", "''").Replace("*", "%") + "'");
            }

            query.Append("SELECT [INPolicy].[ID], [INPolicy].[PolicyID], [INPolicy].[FKPolicyTypeID], [INPolicy].[FKINPolicyHolderID], [INPolicy].[FKPaymentMethodID], [INPolicy].[FKBankID], [INPolicy].[FKBankBranchID], [INPolicy].[FKAccountTypeID], [INPolicy].[AccountNumber], [INPolicy].[AccountHolder], [INPolicy].[PolicyFee], [INPolicy].[TotalPremium], [INPolicy].[CommenceDate], [INPolicy].[DebitDay], [INPolicy].[StampDate], [INPolicy].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INPolicy].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INPolicy] ");

            return query + whereQuery.ToString();
        }

        #endregion
    }
}
