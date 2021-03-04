using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inoption objects.
    /// </summary>
    internal abstract partial class INOptionQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inoption from the database.
        /// </summary>
        /// <param name="inoption">The inoption object to delete.</param>
        /// <returns>A query that can be used to delete the inoption from the database.</returns>
        internal static string Delete(INOption inoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoption != null)
            {
                query = "INSERT INTO [zHstINOption] ([ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID] FROM [INOption] WHERE [INOption].[ID] = @ID; ";
                query += "DELETE FROM [INOption] WHERE [INOption].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inoption from the database.
        /// </summary>
        /// <param name="inoption">The inoption object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inoption from the database.</returns>
        internal static string DeleteHistory(INOption inoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoption != null)
            {
                query = "DELETE FROM [zHstINOption] WHERE [zHstINOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inoption from the database.
        /// </summary>
        /// <param name="inoption">The inoption object to undelete.</param>
        /// <returns>A query that can be used to undelete the inoption from the database.</returns>
        internal static string UnDelete(INOption inoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoption != null)
            {
                query = "INSERT INTO [INOption] ([ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID] FROM [zHstINOption] WHERE [zHstINOption].[ID] = @ID AND [zHstINOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINOption] WHERE [zHstINOption].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INOption] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINOption] WHERE [zHstINOption].[ID] = @ID AND [zHstINOption].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINOption] WHERE [zHstINOption].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INOption] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoption.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inoption object.
        /// </summary>
        /// <param name="inoption">The inoption object to fill.</param>
        /// <returns>A query that can be used to fill the inoption object.</returns>
        internal static string Fill(INOption inoption, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoption != null)
            {
                query = "SELECT [ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID] FROM [INOption] WHERE [INOption].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoption.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inoption data.
        /// </summary>
        /// <param name="inoption">The inoption to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inoption data.</returns>
        internal static string FillData(INOption inoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoption != null)
            {
            query.Append("SELECT [INOption].[ID], [INOption].[FKINPlanID], [INOption].[OptionCode], [INOption].[Category], [INOption].[Display], [INOption].[PolicyFee], [INOption].[LA1Component], [INOption].[LA1Cover], [INOption].[LA1Premium], [INOption].[LA1Cost], [INOption].[LA2Component], [INOption].[LA2Cover], [INOption].[LA2Premium], [INOption].[LA2Cost], [INOption].[ChildComponent], [INOption].[ChildCover], [INOption].[ChildPremium], [INOption].[ChildCost], [INOption].[FuneralComponent], [INOption].[FuneralCover], [INOption].[FuneralPremium], [INOption].[FuneralCost], [INOption].[LA1AccidentalDeathComponent], [INOption].[LA1AccidentalDeathCover], [INOption].[LA1AccidentalDeathPremium], [INOption].[LA1AccidentalDeathCost], [INOption].[LA2AccidentalDeathComponent], [INOption].[LA2AccidentalDeathCover], [INOption].[LA2AccidentalDeathPremium], [INOption].[LA2AccidentalDeathCost], [INOption].[LA1FuneralComponent], [INOption].[LA1FuneralCover], [INOption].[LA1FuneralPremium], [INOption].[LA1FuneralCost], [INOption].[LA2FuneralComponent], [INOption].[LA2FuneralCover], [INOption].[LA2FuneralPremium], [INOption].[LA2FuneralCost], [INOption].[TotalPremium1], [INOption].[TotalPremium1SystemUnits], [INOption].[TotalPremium1SalaryUnits], [INOption].[TotalPremium2], [INOption].[TotalPremium2SystemUnits], [INOption].[TotalPremium2SalaryUnits], [INOption].[IsActive], [INOption].[StampDate], [INOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOption] ");
                query.Append(" WHERE [INOption].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoption.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inoption object from history.
        /// </summary>
        /// <param name="inoption">The inoption object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inoption object from history.</returns>
        internal static string FillHistory(INOption inoption, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inoption != null)
            {
                query = "SELECT [ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID] FROM [zHstINOption] WHERE [zHstINOption].[ID] = @ID AND [zHstINOption].[StampUserID] = @StampUserID AND [zHstINOption].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inoption.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inoptions in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INOption].[ID], [INOption].[FKINPlanID], [INOption].[OptionCode], [INOption].[Category], [INOption].[Display], [INOption].[PolicyFee], [INOption].[LA1Component], [INOption].[LA1Cover], [INOption].[LA1Premium], [INOption].[LA1Cost], [INOption].[LA2Component], [INOption].[LA2Cover], [INOption].[LA2Premium], [INOption].[LA2Cost], [INOption].[ChildComponent], [INOption].[ChildCover], [INOption].[ChildPremium], [INOption].[ChildCost], [INOption].[FuneralComponent], [INOption].[FuneralCover], [INOption].[FuneralPremium], [INOption].[FuneralCost], [INOption].[LA1AccidentalDeathComponent], [INOption].[LA1AccidentalDeathCover], [INOption].[LA1AccidentalDeathPremium], [INOption].[LA1AccidentalDeathCost], [INOption].[LA2AccidentalDeathComponent], [INOption].[LA2AccidentalDeathCover], [INOption].[LA2AccidentalDeathPremium], [INOption].[LA2AccidentalDeathCost], [INOption].[LA1FuneralComponent], [INOption].[LA1FuneralCover], [INOption].[LA1FuneralPremium], [INOption].[LA1FuneralCost], [INOption].[LA2FuneralComponent], [INOption].[LA2FuneralCover], [INOption].[LA2FuneralPremium], [INOption].[LA2FuneralCost], [INOption].[TotalPremium1], [INOption].[TotalPremium1SystemUnits], [INOption].[TotalPremium1SalaryUnits], [INOption].[TotalPremium2], [INOption].[TotalPremium2SystemUnits], [INOption].[TotalPremium2SalaryUnits], [INOption].[IsActive], [INOption].[StampDate], [INOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOption] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inoptions in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inoptions in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINOption].[ID], [zHstINOption].[FKINPlanID], [zHstINOption].[OptionCode], [zHstINOption].[Category], [zHstINOption].[Display], [zHstINOption].[PolicyFee], [zHstINOption].[LA1Component], [zHstINOption].[LA1Cover], [zHstINOption].[LA1Premium], [zHstINOption].[LA1Cost], [zHstINOption].[LA2Component], [zHstINOption].[LA2Cover], [zHstINOption].[LA2Premium], [zHstINOption].[LA2Cost], [zHstINOption].[ChildComponent], [zHstINOption].[ChildCover], [zHstINOption].[ChildPremium], [zHstINOption].[ChildCost], [zHstINOption].[FuneralComponent], [zHstINOption].[FuneralCover], [zHstINOption].[FuneralPremium], [zHstINOption].[FuneralCost], [zHstINOption].[LA1AccidentalDeathComponent], [zHstINOption].[LA1AccidentalDeathCover], [zHstINOption].[LA1AccidentalDeathPremium], [zHstINOption].[LA1AccidentalDeathCost], [zHstINOption].[LA2AccidentalDeathComponent], [zHstINOption].[LA2AccidentalDeathCover], [zHstINOption].[LA2AccidentalDeathPremium], [zHstINOption].[LA2AccidentalDeathCost], [zHstINOption].[LA1FuneralComponent], [zHstINOption].[LA1FuneralCover], [zHstINOption].[LA1FuneralPremium], [zHstINOption].[LA1FuneralCost], [zHstINOption].[LA2FuneralComponent], [zHstINOption].[LA2FuneralCover], [zHstINOption].[LA2FuneralPremium], [zHstINOption].[LA2FuneralCost], [zHstINOption].[TotalPremium1], [zHstINOption].[TotalPremium1SystemUnits], [zHstINOption].[TotalPremium1SalaryUnits], [zHstINOption].[TotalPremium2], [zHstINOption].[TotalPremium2SystemUnits], [zHstINOption].[TotalPremium2SalaryUnits], [zHstINOption].[IsActive], [zHstINOption].[StampDate], [zHstINOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINOption] ");
            query.Append("INNER JOIN (SELECT [zHstINOption].[ID], MAX([zHstINOption].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINOption] ");
            query.Append("WHERE [zHstINOption].[ID] NOT IN (SELECT [INOption].[ID] FROM [INOption]) ");
            query.Append("GROUP BY [zHstINOption].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINOption].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINOption].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inoption in the database.
        /// </summary>
        /// <param name="inoption">The inoption object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inoption in the database.</returns>
        public static string ListHistory(INOption inoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoption != null)
            {
            query.Append("SELECT [zHstINOption].[ID], [zHstINOption].[FKINPlanID], [zHstINOption].[OptionCode], [zHstINOption].[Category], [zHstINOption].[Display], [zHstINOption].[PolicyFee], [zHstINOption].[LA1Component], [zHstINOption].[LA1Cover], [zHstINOption].[LA1Premium], [zHstINOption].[LA1Cost], [zHstINOption].[LA2Component], [zHstINOption].[LA2Cover], [zHstINOption].[LA2Premium], [zHstINOption].[LA2Cost], [zHstINOption].[ChildComponent], [zHstINOption].[ChildCover], [zHstINOption].[ChildPremium], [zHstINOption].[ChildCost], [zHstINOption].[FuneralComponent], [zHstINOption].[FuneralCover], [zHstINOption].[FuneralPremium], [zHstINOption].[FuneralCost], [zHstINOption].[LA1AccidentalDeathComponent], [zHstINOption].[LA1AccidentalDeathCover], [zHstINOption].[LA1AccidentalDeathPremium], [zHstINOption].[LA1AccidentalDeathCost], [zHstINOption].[LA2AccidentalDeathComponent], [zHstINOption].[LA2AccidentalDeathCover], [zHstINOption].[LA2AccidentalDeathPremium], [zHstINOption].[LA2AccidentalDeathCost], [zHstINOption].[LA1FuneralComponent], [zHstINOption].[LA1FuneralCover], [zHstINOption].[LA1FuneralPremium], [zHstINOption].[LA1FuneralCost], [zHstINOption].[LA2FuneralComponent], [zHstINOption].[LA2FuneralCover], [zHstINOption].[LA2FuneralPremium], [zHstINOption].[LA2FuneralCost], [zHstINOption].[TotalPremium1], [zHstINOption].[TotalPremium1SystemUnits], [zHstINOption].[TotalPremium1SalaryUnits], [zHstINOption].[TotalPremium2], [zHstINOption].[TotalPremium2SystemUnits], [zHstINOption].[TotalPremium2SalaryUnits], [zHstINOption].[IsActive], [zHstINOption].[StampDate], [zHstINOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINOption] ");
                query.Append(" WHERE [zHstINOption].[ID] = @ID");
                query.Append(" ORDER BY [zHstINOption].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inoption.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inoption to the database.
        /// </summary>
        /// <param name="inoption">The inoption to save.</param>
        /// <returns>A query that can be used to save the inoption to the database.</returns>
        internal static string Save(INOption inoption, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inoption != null)
            {
                if (inoption.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINOption] ([ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID]) SELECT [ID], [FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID] FROM [INOption] WHERE [INOption].[ID] = @ID; ");
                    query.Append("UPDATE [INOption]");
                    parameters = new object[45];
                    query.Append(" SET [FKINPlanID] = @FKINPlanID");
                    parameters[0] = Database.GetParameter("@FKINPlanID", inoption.FKINPlanID.HasValue ? (object)inoption.FKINPlanID.Value : DBNull.Value);
                    query.Append(", [OptionCode] = @OptionCode");
                    parameters[1] = Database.GetParameter("@OptionCode", string.IsNullOrEmpty(inoption.OptionCode) ? DBNull.Value : (object)inoption.OptionCode);
                    query.Append(", [Category] = @Category");
                    parameters[2] = Database.GetParameter("@Category", string.IsNullOrEmpty(inoption.Category) ? DBNull.Value : (object)inoption.Category);
                    query.Append(", [Display] = @Display");
                    parameters[3] = Database.GetParameter("@Display", string.IsNullOrEmpty(inoption.Display) ? DBNull.Value : (object)inoption.Display);
                    query.Append(", [PolicyFee] = @PolicyFee");
                    parameters[4] = Database.GetParameter("@PolicyFee", inoption.PolicyFee.HasValue ? (object)inoption.PolicyFee.Value : DBNull.Value);
                    query.Append(", [LA1Component] = @LA1Component");
                    parameters[5] = Database.GetParameter("@LA1Component", string.IsNullOrEmpty(inoption.LA1Component) ? DBNull.Value : (object)inoption.LA1Component);
                    query.Append(", [LA1Cover] = @LA1Cover");
                    parameters[6] = Database.GetParameter("@LA1Cover", inoption.LA1Cover.HasValue ? (object)inoption.LA1Cover.Value : DBNull.Value);
                    query.Append(", [LA1Premium] = @LA1Premium");
                    parameters[7] = Database.GetParameter("@LA1Premium", inoption.LA1Premium.HasValue ? (object)inoption.LA1Premium.Value : DBNull.Value);
                    query.Append(", [LA1Cost] = @LA1Cost");
                    parameters[8] = Database.GetParameter("@LA1Cost", inoption.LA1Cost.HasValue ? (object)inoption.LA1Cost.Value : DBNull.Value);
                    query.Append(", [LA2Component] = @LA2Component");
                    parameters[9] = Database.GetParameter("@LA2Component", string.IsNullOrEmpty(inoption.LA2Component) ? DBNull.Value : (object)inoption.LA2Component);
                    query.Append(", [LA2Cover] = @LA2Cover");
                    parameters[10] = Database.GetParameter("@LA2Cover", inoption.LA2Cover.HasValue ? (object)inoption.LA2Cover.Value : DBNull.Value);
                    query.Append(", [LA2Premium] = @LA2Premium");
                    parameters[11] = Database.GetParameter("@LA2Premium", inoption.LA2Premium.HasValue ? (object)inoption.LA2Premium.Value : DBNull.Value);
                    query.Append(", [LA2Cost] = @LA2Cost");
                    parameters[12] = Database.GetParameter("@LA2Cost", inoption.LA2Cost.HasValue ? (object)inoption.LA2Cost.Value : DBNull.Value);
                    query.Append(", [ChildComponent] = @ChildComponent");
                    parameters[13] = Database.GetParameter("@ChildComponent", string.IsNullOrEmpty(inoption.ChildComponent) ? DBNull.Value : (object)inoption.ChildComponent);
                    query.Append(", [ChildCover] = @ChildCover");
                    parameters[14] = Database.GetParameter("@ChildCover", inoption.ChildCover.HasValue ? (object)inoption.ChildCover.Value : DBNull.Value);
                    query.Append(", [ChildPremium] = @ChildPremium");
                    parameters[15] = Database.GetParameter("@ChildPremium", inoption.ChildPremium.HasValue ? (object)inoption.ChildPremium.Value : DBNull.Value);
                    query.Append(", [ChildCost] = @ChildCost");
                    parameters[16] = Database.GetParameter("@ChildCost", inoption.ChildCost.HasValue ? (object)inoption.ChildCost.Value : DBNull.Value);
                    query.Append(", [FuneralComponent] = @FuneralComponent");
                    parameters[17] = Database.GetParameter("@FuneralComponent", string.IsNullOrEmpty(inoption.FuneralComponent) ? DBNull.Value : (object)inoption.FuneralComponent);
                    query.Append(", [FuneralCover] = @FuneralCover");
                    parameters[18] = Database.GetParameter("@FuneralCover", inoption.FuneralCover.HasValue ? (object)inoption.FuneralCover.Value : DBNull.Value);
                    query.Append(", [FuneralPremium] = @FuneralPremium");
                    parameters[19] = Database.GetParameter("@FuneralPremium", inoption.FuneralPremium.HasValue ? (object)inoption.FuneralPremium.Value : DBNull.Value);
                    query.Append(", [FuneralCost] = @FuneralCost");
                    parameters[20] = Database.GetParameter("@FuneralCost", inoption.FuneralCost.HasValue ? (object)inoption.FuneralCost.Value : DBNull.Value);
                    query.Append(", [LA1AccidentalDeathComponent] = @LA1AccidentalDeathComponent");
                    parameters[21] = Database.GetParameter("@LA1AccidentalDeathComponent", string.IsNullOrEmpty(inoption.LA1AccidentalDeathComponent) ? DBNull.Value : (object)inoption.LA1AccidentalDeathComponent);
                    query.Append(", [LA1AccidentalDeathCover] = @LA1AccidentalDeathCover");
                    parameters[22] = Database.GetParameter("@LA1AccidentalDeathCover", inoption.LA1AccidentalDeathCover.HasValue ? (object)inoption.LA1AccidentalDeathCover.Value : DBNull.Value);
                    query.Append(", [LA1AccidentalDeathPremium] = @LA1AccidentalDeathPremium");
                    parameters[23] = Database.GetParameter("@LA1AccidentalDeathPremium", inoption.LA1AccidentalDeathPremium.HasValue ? (object)inoption.LA1AccidentalDeathPremium.Value : DBNull.Value);
                    query.Append(", [LA1AccidentalDeathCost] = @LA1AccidentalDeathCost");
                    parameters[24] = Database.GetParameter("@LA1AccidentalDeathCost", inoption.LA1AccidentalDeathCost.HasValue ? (object)inoption.LA1AccidentalDeathCost.Value : DBNull.Value);
                    query.Append(", [LA2AccidentalDeathComponent] = @LA2AccidentalDeathComponent");
                    parameters[25] = Database.GetParameter("@LA2AccidentalDeathComponent", string.IsNullOrEmpty(inoption.LA2AccidentalDeathComponent) ? DBNull.Value : (object)inoption.LA2AccidentalDeathComponent);
                    query.Append(", [LA2AccidentalDeathCover] = @LA2AccidentalDeathCover");
                    parameters[26] = Database.GetParameter("@LA2AccidentalDeathCover", inoption.LA2AccidentalDeathCover.HasValue ? (object)inoption.LA2AccidentalDeathCover.Value : DBNull.Value);
                    query.Append(", [LA2AccidentalDeathPremium] = @LA2AccidentalDeathPremium");
                    parameters[27] = Database.GetParameter("@LA2AccidentalDeathPremium", inoption.LA2AccidentalDeathPremium.HasValue ? (object)inoption.LA2AccidentalDeathPremium.Value : DBNull.Value);
                    query.Append(", [LA2AccidentalDeathCost] = @LA2AccidentalDeathCost");
                    parameters[28] = Database.GetParameter("@LA2AccidentalDeathCost", inoption.LA2AccidentalDeathCost.HasValue ? (object)inoption.LA2AccidentalDeathCost.Value : DBNull.Value);
                    query.Append(", [LA1FuneralComponent] = @LA1FuneralComponent");
                    parameters[29] = Database.GetParameter("@LA1FuneralComponent", string.IsNullOrEmpty(inoption.LA1FuneralComponent) ? DBNull.Value : (object)inoption.LA1FuneralComponent);
                    query.Append(", [LA1FuneralCover] = @LA1FuneralCover");
                    parameters[30] = Database.GetParameter("@LA1FuneralCover", inoption.LA1FuneralCover.HasValue ? (object)inoption.LA1FuneralCover.Value : DBNull.Value);
                    query.Append(", [LA1FuneralPremium] = @LA1FuneralPremium");
                    parameters[31] = Database.GetParameter("@LA1FuneralPremium", inoption.LA1FuneralPremium.HasValue ? (object)inoption.LA1FuneralPremium.Value : DBNull.Value);
                    query.Append(", [LA1FuneralCost] = @LA1FuneralCost");
                    parameters[32] = Database.GetParameter("@LA1FuneralCost", inoption.LA1FuneralCost.HasValue ? (object)inoption.LA1FuneralCost.Value : DBNull.Value);
                    query.Append(", [LA2FuneralComponent] = @LA2FuneralComponent");
                    parameters[33] = Database.GetParameter("@LA2FuneralComponent", string.IsNullOrEmpty(inoption.LA2FuneralComponent) ? DBNull.Value : (object)inoption.LA2FuneralComponent);
                    query.Append(", [LA2FuneralCover] = @LA2FuneralCover");
                    parameters[34] = Database.GetParameter("@LA2FuneralCover", inoption.LA2FuneralCover.HasValue ? (object)inoption.LA2FuneralCover.Value : DBNull.Value);
                    query.Append(", [LA2FuneralPremium] = @LA2FuneralPremium");
                    parameters[35] = Database.GetParameter("@LA2FuneralPremium", inoption.LA2FuneralPremium.HasValue ? (object)inoption.LA2FuneralPremium.Value : DBNull.Value);
                    query.Append(", [LA2FuneralCost] = @LA2FuneralCost");
                    parameters[36] = Database.GetParameter("@LA2FuneralCost", inoption.LA2FuneralCost.HasValue ? (object)inoption.LA2FuneralCost.Value : DBNull.Value);
                    query.Append(", [TotalPremium1] = @TotalPremium1");
                    parameters[37] = Database.GetParameter("@TotalPremium1", inoption.TotalPremium1.HasValue ? (object)inoption.TotalPremium1.Value : DBNull.Value);
                    query.Append(", [TotalPremium1SystemUnits] = @TotalPremium1SystemUnits");
                    parameters[38] = Database.GetParameter("@TotalPremium1SystemUnits", inoption.TotalPremium1SystemUnits.HasValue ? (object)inoption.TotalPremium1SystemUnits.Value : DBNull.Value);
                    query.Append(", [TotalPremium1SalaryUnits] = @TotalPremium1SalaryUnits");
                    parameters[39] = Database.GetParameter("@TotalPremium1SalaryUnits", inoption.TotalPremium1SalaryUnits.HasValue ? (object)inoption.TotalPremium1SalaryUnits.Value : DBNull.Value);
                    query.Append(", [TotalPremium2] = @TotalPremium2");
                    parameters[40] = Database.GetParameter("@TotalPremium2", inoption.TotalPremium2.HasValue ? (object)inoption.TotalPremium2.Value : DBNull.Value);
                    query.Append(", [TotalPremium2SystemUnits] = @TotalPremium2SystemUnits");
                    parameters[41] = Database.GetParameter("@TotalPremium2SystemUnits", inoption.TotalPremium2SystemUnits.HasValue ? (object)inoption.TotalPremium2SystemUnits.Value : DBNull.Value);
                    query.Append(", [TotalPremium2SalaryUnits] = @TotalPremium2SalaryUnits");
                    parameters[42] = Database.GetParameter("@TotalPremium2SalaryUnits", inoption.TotalPremium2SalaryUnits.HasValue ? (object)inoption.TotalPremium2SalaryUnits.Value : DBNull.Value);
                    query.Append(", [IsActive] = @IsActive");
                    parameters[43] = Database.GetParameter("@IsActive", inoption.IsActive.HasValue ? (object)inoption.IsActive.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INOption].[ID] = @ID"); 
                    parameters[44] = Database.GetParameter("@ID", inoption.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INOption] ([FKINPlanID], [OptionCode], [Category], [Display], [PolicyFee], [LA1Component], [LA1Cover], [LA1Premium], [LA1Cost], [LA2Component], [LA2Cover], [LA2Premium], [LA2Cost], [ChildComponent], [ChildCover], [ChildPremium], [ChildCost], [FuneralComponent], [FuneralCover], [FuneralPremium], [FuneralCost], [LA1AccidentalDeathComponent], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1AccidentalDeathCost], [LA2AccidentalDeathComponent], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2AccidentalDeathCost], [LA1FuneralComponent], [LA1FuneralCover], [LA1FuneralPremium], [LA1FuneralCost], [LA2FuneralComponent], [LA2FuneralCover], [LA2FuneralPremium], [LA2FuneralCost], [TotalPremium1], [TotalPremium1SystemUnits], [TotalPremium1SalaryUnits], [TotalPremium2], [TotalPremium2SystemUnits], [TotalPremium2SalaryUnits], [IsActive], [StampDate], [StampUserID]) VALUES(@FKINPlanID, @OptionCode, @Category, @Display, @PolicyFee, @LA1Component, @LA1Cover, @LA1Premium, @LA1Cost, @LA2Component, @LA2Cover, @LA2Premium, @LA2Cost, @ChildComponent, @ChildCover, @ChildPremium, @ChildCost, @FuneralComponent, @FuneralCover, @FuneralPremium, @FuneralCost, @LA1AccidentalDeathComponent, @LA1AccidentalDeathCover, @LA1AccidentalDeathPremium, @LA1AccidentalDeathCost, @LA2AccidentalDeathComponent, @LA2AccidentalDeathCover, @LA2AccidentalDeathPremium, @LA2AccidentalDeathCost, @LA1FuneralComponent, @LA1FuneralCover, @LA1FuneralPremium, @LA1FuneralCost, @LA2FuneralComponent, @LA2FuneralCover, @LA2FuneralPremium, @LA2FuneralCost, @TotalPremium1, @TotalPremium1SystemUnits, @TotalPremium1SalaryUnits, @TotalPremium2, @TotalPremium2SystemUnits, @TotalPremium2SalaryUnits, @IsActive, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[44];
                    parameters[0] = Database.GetParameter("@FKINPlanID", inoption.FKINPlanID.HasValue ? (object)inoption.FKINPlanID.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@OptionCode", string.IsNullOrEmpty(inoption.OptionCode) ? DBNull.Value : (object)inoption.OptionCode);
                    parameters[2] = Database.GetParameter("@Category", string.IsNullOrEmpty(inoption.Category) ? DBNull.Value : (object)inoption.Category);
                    parameters[3] = Database.GetParameter("@Display", string.IsNullOrEmpty(inoption.Display) ? DBNull.Value : (object)inoption.Display);
                    parameters[4] = Database.GetParameter("@PolicyFee", inoption.PolicyFee.HasValue ? (object)inoption.PolicyFee.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@LA1Component", string.IsNullOrEmpty(inoption.LA1Component) ? DBNull.Value : (object)inoption.LA1Component);
                    parameters[6] = Database.GetParameter("@LA1Cover", inoption.LA1Cover.HasValue ? (object)inoption.LA1Cover.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@LA1Premium", inoption.LA1Premium.HasValue ? (object)inoption.LA1Premium.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@LA1Cost", inoption.LA1Cost.HasValue ? (object)inoption.LA1Cost.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@LA2Component", string.IsNullOrEmpty(inoption.LA2Component) ? DBNull.Value : (object)inoption.LA2Component);
                    parameters[10] = Database.GetParameter("@LA2Cover", inoption.LA2Cover.HasValue ? (object)inoption.LA2Cover.Value : DBNull.Value);
                    parameters[11] = Database.GetParameter("@LA2Premium", inoption.LA2Premium.HasValue ? (object)inoption.LA2Premium.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@LA2Cost", inoption.LA2Cost.HasValue ? (object)inoption.LA2Cost.Value : DBNull.Value);
                    parameters[13] = Database.GetParameter("@ChildComponent", string.IsNullOrEmpty(inoption.ChildComponent) ? DBNull.Value : (object)inoption.ChildComponent);
                    parameters[14] = Database.GetParameter("@ChildCover", inoption.ChildCover.HasValue ? (object)inoption.ChildCover.Value : DBNull.Value);
                    parameters[15] = Database.GetParameter("@ChildPremium", inoption.ChildPremium.HasValue ? (object)inoption.ChildPremium.Value : DBNull.Value);
                    parameters[16] = Database.GetParameter("@ChildCost", inoption.ChildCost.HasValue ? (object)inoption.ChildCost.Value : DBNull.Value);
                    parameters[17] = Database.GetParameter("@FuneralComponent", string.IsNullOrEmpty(inoption.FuneralComponent) ? DBNull.Value : (object)inoption.FuneralComponent);
                    parameters[18] = Database.GetParameter("@FuneralCover", inoption.FuneralCover.HasValue ? (object)inoption.FuneralCover.Value : DBNull.Value);
                    parameters[19] = Database.GetParameter("@FuneralPremium", inoption.FuneralPremium.HasValue ? (object)inoption.FuneralPremium.Value : DBNull.Value);
                    parameters[20] = Database.GetParameter("@FuneralCost", inoption.FuneralCost.HasValue ? (object)inoption.FuneralCost.Value : DBNull.Value);
                    parameters[21] = Database.GetParameter("@LA1AccidentalDeathComponent", string.IsNullOrEmpty(inoption.LA1AccidentalDeathComponent) ? DBNull.Value : (object)inoption.LA1AccidentalDeathComponent);
                    parameters[22] = Database.GetParameter("@LA1AccidentalDeathCover", inoption.LA1AccidentalDeathCover.HasValue ? (object)inoption.LA1AccidentalDeathCover.Value : DBNull.Value);
                    parameters[23] = Database.GetParameter("@LA1AccidentalDeathPremium", inoption.LA1AccidentalDeathPremium.HasValue ? (object)inoption.LA1AccidentalDeathPremium.Value : DBNull.Value);
                    parameters[24] = Database.GetParameter("@LA1AccidentalDeathCost", inoption.LA1AccidentalDeathCost.HasValue ? (object)inoption.LA1AccidentalDeathCost.Value : DBNull.Value);
                    parameters[25] = Database.GetParameter("@LA2AccidentalDeathComponent", string.IsNullOrEmpty(inoption.LA2AccidentalDeathComponent) ? DBNull.Value : (object)inoption.LA2AccidentalDeathComponent);
                    parameters[26] = Database.GetParameter("@LA2AccidentalDeathCover", inoption.LA2AccidentalDeathCover.HasValue ? (object)inoption.LA2AccidentalDeathCover.Value : DBNull.Value);
                    parameters[27] = Database.GetParameter("@LA2AccidentalDeathPremium", inoption.LA2AccidentalDeathPremium.HasValue ? (object)inoption.LA2AccidentalDeathPremium.Value : DBNull.Value);
                    parameters[28] = Database.GetParameter("@LA2AccidentalDeathCost", inoption.LA2AccidentalDeathCost.HasValue ? (object)inoption.LA2AccidentalDeathCost.Value : DBNull.Value);
                    parameters[29] = Database.GetParameter("@LA1FuneralComponent", string.IsNullOrEmpty(inoption.LA1FuneralComponent) ? DBNull.Value : (object)inoption.LA1FuneralComponent);
                    parameters[30] = Database.GetParameter("@LA1FuneralCover", inoption.LA1FuneralCover.HasValue ? (object)inoption.LA1FuneralCover.Value : DBNull.Value);
                    parameters[31] = Database.GetParameter("@LA1FuneralPremium", inoption.LA1FuneralPremium.HasValue ? (object)inoption.LA1FuneralPremium.Value : DBNull.Value);
                    parameters[32] = Database.GetParameter("@LA1FuneralCost", inoption.LA1FuneralCost.HasValue ? (object)inoption.LA1FuneralCost.Value : DBNull.Value);
                    parameters[33] = Database.GetParameter("@LA2FuneralComponent", string.IsNullOrEmpty(inoption.LA2FuneralComponent) ? DBNull.Value : (object)inoption.LA2FuneralComponent);
                    parameters[34] = Database.GetParameter("@LA2FuneralCover", inoption.LA2FuneralCover.HasValue ? (object)inoption.LA2FuneralCover.Value : DBNull.Value);
                    parameters[35] = Database.GetParameter("@LA2FuneralPremium", inoption.LA2FuneralPremium.HasValue ? (object)inoption.LA2FuneralPremium.Value : DBNull.Value);
                    parameters[36] = Database.GetParameter("@LA2FuneralCost", inoption.LA2FuneralCost.HasValue ? (object)inoption.LA2FuneralCost.Value : DBNull.Value);
                    parameters[37] = Database.GetParameter("@TotalPremium1", inoption.TotalPremium1.HasValue ? (object)inoption.TotalPremium1.Value : DBNull.Value);
                    parameters[38] = Database.GetParameter("@TotalPremium1SystemUnits", inoption.TotalPremium1SystemUnits.HasValue ? (object)inoption.TotalPremium1SystemUnits.Value : DBNull.Value);
                    parameters[39] = Database.GetParameter("@TotalPremium1SalaryUnits", inoption.TotalPremium1SalaryUnits.HasValue ? (object)inoption.TotalPremium1SalaryUnits.Value : DBNull.Value);
                    parameters[40] = Database.GetParameter("@TotalPremium2", inoption.TotalPremium2.HasValue ? (object)inoption.TotalPremium2.Value : DBNull.Value);
                    parameters[41] = Database.GetParameter("@TotalPremium2SystemUnits", inoption.TotalPremium2SystemUnits.HasValue ? (object)inoption.TotalPremium2SystemUnits.Value : DBNull.Value);
                    parameters[42] = Database.GetParameter("@TotalPremium2SalaryUnits", inoption.TotalPremium2SalaryUnits.HasValue ? (object)inoption.TotalPremium2SalaryUnits.Value : DBNull.Value);
                    parameters[43] = Database.GetParameter("@IsActive", inoption.IsActive.HasValue ? (object)inoption.IsActive.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inoptions that match the search criteria.
        /// </summary>
        /// <param name="fkinplanid">The fkinplanid search criteria.</param>
        /// <param name="optioncode">The optioncode search criteria.</param>
        /// <param name="category">The category search criteria.</param>
        /// <param name="display">The display search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="la1component">The la1component search criteria.</param>
        /// <param name="la1cover">The la1cover search criteria.</param>
        /// <param name="la1premium">The la1premium search criteria.</param>
        /// <param name="la1cost">The la1cost search criteria.</param>
        /// <param name="la2component">The la2component search criteria.</param>
        /// <param name="la2cover">The la2cover search criteria.</param>
        /// <param name="la2premium">The la2premium search criteria.</param>
        /// <param name="la2cost">The la2cost search criteria.</param>
        /// <param name="childcomponent">The childcomponent search criteria.</param>
        /// <param name="childcover">The childcover search criteria.</param>
        /// <param name="childpremium">The childpremium search criteria.</param>
        /// <param name="childcost">The childcost search criteria.</param>
        /// <param name="funeralcomponent">The funeralcomponent search criteria.</param>
        /// <param name="funeralcover">The funeralcover search criteria.</param>
        /// <param name="funeralpremium">The funeralpremium search criteria.</param>
        /// <param name="funeralcost">The funeralcost search criteria.</param>
        /// <param name="la1accidentaldeathcomponent">The la1accidentaldeathcomponent search criteria.</param>
        /// <param name="la1accidentaldeathcover">The la1accidentaldeathcover search criteria.</param>
        /// <param name="la1accidentaldeathpremium">The la1accidentaldeathpremium search criteria.</param>
        /// <param name="la1accidentaldeathcost">The la1accidentaldeathcost search criteria.</param>
        /// <param name="la2accidentaldeathcomponent">The la2accidentaldeathcomponent search criteria.</param>
        /// <param name="la2accidentaldeathcover">The la2accidentaldeathcover search criteria.</param>
        /// <param name="la2accidentaldeathpremium">The la2accidentaldeathpremium search criteria.</param>
        /// <param name="la2accidentaldeathcost">The la2accidentaldeathcost search criteria.</param>
        /// <param name="la1funeralcomponent">The la1funeralcomponent search criteria.</param>
        /// <param name="la1funeralcover">The la1funeralcover search criteria.</param>
        /// <param name="la1funeralpremium">The la1funeralpremium search criteria.</param>
        /// <param name="la1funeralcost">The la1funeralcost search criteria.</param>
        /// <param name="la2funeralcomponent">The la2funeralcomponent search criteria.</param>
        /// <param name="la2funeralcover">The la2funeralcover search criteria.</param>
        /// <param name="la2funeralpremium">The la2funeralpremium search criteria.</param>
        /// <param name="la2funeralcost">The la2funeralcost search criteria.</param>
        /// <param name="totalpremium1">The totalpremium1 search criteria.</param>
        /// <param name="totalpremium1systemunits">The totalpremium1systemunits search criteria.</param>
        /// <param name="totalpremium1salaryunits">The totalpremium1salaryunits search criteria.</param>
        /// <param name="totalpremium2">The totalpremium2 search criteria.</param>
        /// <param name="totalpremium2systemunits">The totalpremium2systemunits search criteria.</param>
        /// <param name="totalpremium2salaryunits">The totalpremium2salaryunits search criteria.</param>
        /// <param name="isactive">The isactive search criteria.</param>
        /// <returns>A query that can be used to search for inoptions based on the search criteria.</returns>
        internal static string Search(long? fkinplanid, string optioncode, string category, string display, decimal? policyfee, string la1component, decimal? la1cover, decimal? la1premium, decimal? la1cost, string la2component, decimal? la2cover, decimal? la2premium, decimal? la2cost, string childcomponent, decimal? childcover, decimal? childpremium, decimal? childcost, string funeralcomponent, decimal? funeralcover, decimal? funeralpremium, decimal? funeralcost, string la1accidentaldeathcomponent, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1accidentaldeathcost, string la2accidentaldeathcomponent, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2accidentaldeathcost, string la1funeralcomponent, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la1funeralcost, string la2funeralcomponent, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? la2funeralcost, decimal? totalpremium1, decimal? totalpremium1systemunits, decimal? totalpremium1salaryunits, decimal? totalpremium2, decimal? totalpremium2systemunits, decimal? totalpremium2salaryunits, bool? isactive)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (fkinplanid != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[FKINPlanID] = " + fkinplanid + "");
            }
            if (optioncode != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[OptionCode] LIKE '" + optioncode.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (category != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[Category] LIKE '" + category.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (display != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[Display] LIKE '" + display.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (policyfee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[PolicyFee] = " + policyfee + "");
            }
            if (la1component != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1Component] LIKE '" + la1component.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la1cover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1Cover] = " + la1cover + "");
            }
            if (la1premium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1Premium] = " + la1premium + "");
            }
            if (la1cost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1Cost] = " + la1cost + "");
            }
            if (la2component != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2Component] LIKE '" + la2component.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la2cover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2Cover] = " + la2cover + "");
            }
            if (la2premium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2Premium] = " + la2premium + "");
            }
            if (la2cost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2Cost] = " + la2cost + "");
            }
            if (childcomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[ChildComponent] LIKE '" + childcomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (childcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[ChildCover] = " + childcover + "");
            }
            if (childpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[ChildPremium] = " + childpremium + "");
            }
            if (childcost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[ChildCost] = " + childcost + "");
            }
            if (funeralcomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[FuneralComponent] LIKE '" + funeralcomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (funeralcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[FuneralCover] = " + funeralcover + "");
            }
            if (funeralpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[FuneralPremium] = " + funeralpremium + "");
            }
            if (funeralcost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[FuneralCost] = " + funeralcost + "");
            }
            if (la1accidentaldeathcomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1AccidentalDeathComponent] LIKE '" + la1accidentaldeathcomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la1accidentaldeathcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1AccidentalDeathCover] = " + la1accidentaldeathcover + "");
            }
            if (la1accidentaldeathpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1AccidentalDeathPremium] = " + la1accidentaldeathpremium + "");
            }
            if (la1accidentaldeathcost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1AccidentalDeathCost] = " + la1accidentaldeathcost + "");
            }
            if (la2accidentaldeathcomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2AccidentalDeathComponent] LIKE '" + la2accidentaldeathcomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la2accidentaldeathcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2AccidentalDeathCover] = " + la2accidentaldeathcover + "");
            }
            if (la2accidentaldeathpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2AccidentalDeathPremium] = " + la2accidentaldeathpremium + "");
            }
            if (la2accidentaldeathcost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2AccidentalDeathCost] = " + la2accidentaldeathcost + "");
            }
            if (la1funeralcomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1FuneralComponent] LIKE '" + la1funeralcomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la1funeralcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1FuneralCover] = " + la1funeralcover + "");
            }
            if (la1funeralpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1FuneralPremium] = " + la1funeralpremium + "");
            }
            if (la1funeralcost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA1FuneralCost] = " + la1funeralcost + "");
            }
            if (la2funeralcomponent != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2FuneralComponent] LIKE '" + la2funeralcomponent.Replace("'", "''").Replace("*", "%") + "'");
            }
            if (la2funeralcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2FuneralCover] = " + la2funeralcover + "");
            }
            if (la2funeralpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2FuneralPremium] = " + la2funeralpremium + "");
            }
            if (la2funeralcost != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[LA2FuneralCost] = " + la2funeralcost + "");
            }
            if (totalpremium1 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[TotalPremium1] = " + totalpremium1 + "");
            }
            if (totalpremium1systemunits != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[TotalPremium1SystemUnits] = " + totalpremium1systemunits + "");
            }
            if (totalpremium1salaryunits != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[TotalPremium1SalaryUnits] = " + totalpremium1salaryunits + "");
            }
            if (totalpremium2 != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[TotalPremium2] = " + totalpremium2 + "");
            }
            if (totalpremium2systemunits != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[TotalPremium2SystemUnits] = " + totalpremium2systemunits + "");
            }
            if (totalpremium2salaryunits != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[TotalPremium2SalaryUnits] = " + totalpremium2salaryunits + "");
            }
            if (isactive != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INOption].[IsActive] = " + ((bool)isactive ? "1" : "0"));
            }
            query.Append("SELECT [INOption].[ID], [INOption].[FKINPlanID], [INOption].[OptionCode], [INOption].[Category], [INOption].[Display], [INOption].[PolicyFee], [INOption].[LA1Component], [INOption].[LA1Cover], [INOption].[LA1Premium], [INOption].[LA1Cost], [INOption].[LA2Component], [INOption].[LA2Cover], [INOption].[LA2Premium], [INOption].[LA2Cost], [INOption].[ChildComponent], [INOption].[ChildCover], [INOption].[ChildPremium], [INOption].[ChildCost], [INOption].[FuneralComponent], [INOption].[FuneralCover], [INOption].[FuneralPremium], [INOption].[FuneralCost], [INOption].[LA1AccidentalDeathComponent], [INOption].[LA1AccidentalDeathCover], [INOption].[LA1AccidentalDeathPremium], [INOption].[LA1AccidentalDeathCost], [INOption].[LA2AccidentalDeathComponent], [INOption].[LA2AccidentalDeathCover], [INOption].[LA2AccidentalDeathPremium], [INOption].[LA2AccidentalDeathCost], [INOption].[LA1FuneralComponent], [INOption].[LA1FuneralCover], [INOption].[LA1FuneralPremium], [INOption].[LA1FuneralCost], [INOption].[LA2FuneralComponent], [INOption].[LA2FuneralCover], [INOption].[LA2FuneralPremium], [INOption].[LA2FuneralCost], [INOption].[TotalPremium1], [INOption].[TotalPremium1SystemUnits], [INOption].[TotalPremium1SalaryUnits], [INOption].[TotalPremium2], [INOption].[TotalPremium2SystemUnits], [INOption].[TotalPremium2SalaryUnits], [INOption].[IsActive], [INOption].[StampDate], [INOption].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INOption].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INOption] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
