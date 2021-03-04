using System.Text;

namespace UDM.Insurance.Business.Queries
{
    internal abstract partial class INQADetailsQuestionINImportQueries
    {
        #region Search

        internal static string Search(long? fkimportid, long? fkquestionid)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkimportid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionINImport].[FKImportID] = " + fkimportid + "");
            }
            if (fkquestionid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INQADetailsQuestionINImport].[FKQuestionID] = " + fkquestionid + "");
            }
            query.Append("SELECT [INQADetailsQuestionINImport].[ID], [INQADetailsQuestionINImport].[FKImportID], [INQADetailsQuestionINImport].[FKQuestionID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INQADetailsQuestionINImport].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INQADetailsQuestionINImport] ");
            return query.ToString() + whereQuery.ToString();
        }

        #endregion
    }
}
