using System;
using System.Text;

using UDM.Insurance.Business;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business.Queries
{
    /// <summary>
    /// Contains methods that return data related to inimportedpolicydata objects.
    /// </summary>
    internal abstract partial class INImportedPolicyDataQueries
    {
        #region Delete
        /// <summary>
        /// Creates a query that will delete a(n) inimportedpolicydata from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata object to delete.</param>
        /// <returns>A query that can be used to delete the inimportedpolicydata from the database.</returns>
        internal static string Delete(INImportedPolicyData inimportedpolicydata, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportedpolicydata != null)
            {
                query = "INSERT INTO [zHstINImportedPolicyData] ([ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID]) SELECT [ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID] FROM [INImportedPolicyData] WHERE [INImportedPolicyData].[ID] = @ID; ";
                query += "DELETE FROM [INImportedPolicyData] WHERE [INImportedPolicyData].[ID] = @ID; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportedpolicydata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will delete the history for a(n) inimportedpolicydata from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata object to delete history of.</param>
        /// <returns>A query that can be used to delete the history of the inimportedpolicydata from the database.</returns>
        internal static string DeleteHistory(INImportedPolicyData inimportedpolicydata, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportedpolicydata != null)
            {
                query = "DELETE FROM [zHstINImportedPolicyData] WHERE [zHstINImportedPolicyData].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportedpolicydata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will undelete a(n) inimportedpolicydata from the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata object to undelete.</param>
        /// <returns>A query that can be used to undelete the inimportedpolicydata from the database.</returns>
        internal static string UnDelete(INImportedPolicyData inimportedpolicydata, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportedpolicydata != null)
            {
                query = "INSERT INTO [INImportedPolicyData] ([ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID]) SELECT [ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID] FROM [zHstINImportedPolicyData] WHERE [zHstINImportedPolicyData].[ID] = @ID AND [zHstINImportedPolicyData].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportedPolicyData] WHERE [zHstINImportedPolicyData].[ID] = @ID) AND (SELECT COUNT(ID) FROM [INImportedPolicyData] WHERE [ID] = @ID) = 0; ";
                query += "DELETE FROM [zHstINImportedPolicyData] WHERE [zHstINImportedPolicyData].[ID] = @ID AND [zHstINImportedPolicyData].[StampDate] = (SELECT MAX([StampDate]) FROM [zHstINImportedPolicyData] WHERE [zHstINImportedPolicyData].[ID] = @ID) AND (SELECT COUNT([ID]) FROM [INImportedPolicyData] WHERE [ID] = @ID) = 0; ";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportedpolicydata.ID);
            }
            return query;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Creates a query that will return the data to fill an inimportedpolicydata object.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata object to fill.</param>
        /// <returns>A query that can be used to fill the inimportedpolicydata object.</returns>
        internal static string Fill(INImportedPolicyData inimportedpolicydata, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportedpolicydata != null)
            {
                query = "SELECT [ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID] FROM [INImportedPolicyData] WHERE [INImportedPolicyData].[ID] = @ID";
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportedpolicydata.ID);
            }
            return query;
        }

        /// <summary>
        /// Creates a query that will return the data to fill a dataset with  inimportedpolicydata data.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata to return data for.</param>
        /// <returns>A query that can be used to fill a dataset with  inimportedpolicydata data.</returns>
        internal static string FillData(INImportedPolicyData inimportedpolicydata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportedpolicydata != null)
            {
            query.Append("SELECT [INImportedPolicyData].[ID], [INImportedPolicyData].[CommenceDate], [INImportedPolicyData].[AppSignDate], [INImportedPolicyData].[ContractPremium], [INImportedPolicyData].[ContractTerm], [INImportedPolicyData].[LapseDate], [INImportedPolicyData].[LA1CancerCover], [INImportedPolicyData].[LA1CancerPremium], [INImportedPolicyData].[LA1AccidentalDeathCover], [INImportedPolicyData].[LA1AccidentalDeathPremium], [INImportedPolicyData].[LA1DisabilityCover], [INImportedPolicyData].[LA1DisabilityPremium], [INImportedPolicyData].[LA1FuneralCover], [INImportedPolicyData].[LA1FuneralPremium], [INImportedPolicyData].[LA2CancerCover], [INImportedPolicyData].[LA2CancerPremium], [INImportedPolicyData].[LA2AccidentalDeathCover], [INImportedPolicyData].[LA2AccidentalDeathPremium], [INImportedPolicyData].[LA2DisabilityCover], [INImportedPolicyData].[LA2DisabilityPremium], [INImportedPolicyData].[LA2FuneralCover], [INImportedPolicyData].[LA2FuneralPremium], [INImportedPolicyData].[KidsCancerCover], [INImportedPolicyData].[KidsCancerPremium], [INImportedPolicyData].[KidsDisabilityCover], [INImportedPolicyData].[KidsDisabilityPremium], [INImportedPolicyData].[PolicyFee], [INImportedPolicyData].[MoneyBackPremium], [INImportedPolicyData].[MoneyBackTerm], [INImportedPolicyData].[StampDate], [INImportedPolicyData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportedPolicyData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportedPolicyData] ");
                query.Append(" WHERE [INImportedPolicyData].[ID] = @ID");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportedpolicydata.ID);
            }
            return query.ToString();
        }

        /// <summary>
        /// Creates a query that will return the data to fill an inimportedpolicydata object from history.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata object to fill.</param>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <returns>A query that can be used to fill the inimportedpolicydata object from history.</returns>
        internal static string FillHistory(INImportedPolicyData inimportedpolicydata, long stampUserID, DateTime stampDate, ref object[] parameters)
        {
            string query = string.Empty;
            if (inimportedpolicydata != null)
            {
                query = "SELECT [ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID] FROM [zHstINImportedPolicyData] WHERE [zHstINImportedPolicyData].[ID] = @ID AND [zHstINImportedPolicyData].[StampUserID] = @StampUserID AND [zHstINImportedPolicyData].[StampDate] = @StampDate";
                parameters = new object[3];
                parameters[0] = Database.GetParameter("@ID", inimportedpolicydata.ID);
                parameters[1] = Database.GetParameter("@StampUserID", stampUserID);
                parameters[2] = Database.GetParameter("@StampDate", stampDate);
            }
            return query;
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inimportedpolicydatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the inimportedpolicydatas in the database.</returns>
        internal static string List()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [INImportedPolicyData].[ID], [INImportedPolicyData].[CommenceDate], [INImportedPolicyData].[AppSignDate], [INImportedPolicyData].[ContractPremium], [INImportedPolicyData].[ContractTerm], [INImportedPolicyData].[LapseDate], [INImportedPolicyData].[LA1CancerCover], [INImportedPolicyData].[LA1CancerPremium], [INImportedPolicyData].[LA1AccidentalDeathCover], [INImportedPolicyData].[LA1AccidentalDeathPremium], [INImportedPolicyData].[LA1DisabilityCover], [INImportedPolicyData].[LA1DisabilityPremium], [INImportedPolicyData].[LA1FuneralCover], [INImportedPolicyData].[LA1FuneralPremium], [INImportedPolicyData].[LA2CancerCover], [INImportedPolicyData].[LA2CancerPremium], [INImportedPolicyData].[LA2AccidentalDeathCover], [INImportedPolicyData].[LA2AccidentalDeathPremium], [INImportedPolicyData].[LA2DisabilityCover], [INImportedPolicyData].[LA2DisabilityPremium], [INImportedPolicyData].[LA2FuneralCover], [INImportedPolicyData].[LA2FuneralPremium], [INImportedPolicyData].[KidsCancerCover], [INImportedPolicyData].[KidsCancerPremium], [INImportedPolicyData].[KidsDisabilityCover], [INImportedPolicyData].[KidsDisabilityPremium], [INImportedPolicyData].[PolicyFee], [INImportedPolicyData].[MoneyBackPremium], [INImportedPolicyData].[MoneyBackTerm], [INImportedPolicyData].[StampDate], [INImportedPolicyData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportedPolicyData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportedPolicyData] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists all the deleted inimportedpolicydatas in the database.
        /// </summary>
        /// <returns>A query that can be used to list all the deleted inimportedpolicydatas in the database.</returns>
        internal static string ListDeleted()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT [zHstINImportedPolicyData].[ID], [zHstINImportedPolicyData].[CommenceDate], [zHstINImportedPolicyData].[AppSignDate], [zHstINImportedPolicyData].[ContractPremium], [zHstINImportedPolicyData].[ContractTerm], [zHstINImportedPolicyData].[LapseDate], [zHstINImportedPolicyData].[LA1CancerCover], [zHstINImportedPolicyData].[LA1CancerPremium], [zHstINImportedPolicyData].[LA1AccidentalDeathCover], [zHstINImportedPolicyData].[LA1AccidentalDeathPremium], [zHstINImportedPolicyData].[LA1DisabilityCover], [zHstINImportedPolicyData].[LA1DisabilityPremium], [zHstINImportedPolicyData].[LA1FuneralCover], [zHstINImportedPolicyData].[LA1FuneralPremium], [zHstINImportedPolicyData].[LA2CancerCover], [zHstINImportedPolicyData].[LA2CancerPremium], [zHstINImportedPolicyData].[LA2AccidentalDeathCover], [zHstINImportedPolicyData].[LA2AccidentalDeathPremium], [zHstINImportedPolicyData].[LA2DisabilityCover], [zHstINImportedPolicyData].[LA2DisabilityPremium], [zHstINImportedPolicyData].[LA2FuneralCover], [zHstINImportedPolicyData].[LA2FuneralPremium], [zHstINImportedPolicyData].[KidsCancerCover], [zHstINImportedPolicyData].[KidsCancerPremium], [zHstINImportedPolicyData].[KidsDisabilityCover], [zHstINImportedPolicyData].[KidsDisabilityPremium], [zHstINImportedPolicyData].[PolicyFee], [zHstINImportedPolicyData].[MoneyBackPremium], [zHstINImportedPolicyData].[MoneyBackTerm], [zHstINImportedPolicyData].[StampDate], [zHstINImportedPolicyData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportedPolicyData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportedPolicyData] ");
            query.Append("INNER JOIN (SELECT [zHstINImportedPolicyData].[ID], MAX([zHstINImportedPolicyData].[StampDate]) AS 'StampDate' ");
            query.Append("FROM [zHstINImportedPolicyData] ");
            query.Append("WHERE [zHstINImportedPolicyData].[ID] NOT IN (SELECT [INImportedPolicyData].[ID] FROM [INImportedPolicyData]) ");
            query.Append("GROUP BY [zHstINImportedPolicyData].[ID]) AS [LastHistory] ");
            query.Append("ON [zHstINImportedPolicyData].[ID] = [LastHistory].[ID] ");
            query.Append("AND [zHstINImportedPolicyData].[StampDate] = [LastHistory].[StampDate] ");
            return query.ToString();
        }

        /// <summary>
        /// Lists the history for a(n) inimportedpolicydata in the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata object to list history for.</param>
        /// <returns>A query that can be used to list the history for a(n) inimportedpolicydata in the database.</returns>
        public static string ListHistory(INImportedPolicyData inimportedpolicydata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportedpolicydata != null)
            {
            query.Append("SELECT [zHstINImportedPolicyData].[ID], [zHstINImportedPolicyData].[CommenceDate], [zHstINImportedPolicyData].[AppSignDate], [zHstINImportedPolicyData].[ContractPremium], [zHstINImportedPolicyData].[ContractTerm], [zHstINImportedPolicyData].[LapseDate], [zHstINImportedPolicyData].[LA1CancerCover], [zHstINImportedPolicyData].[LA1CancerPremium], [zHstINImportedPolicyData].[LA1AccidentalDeathCover], [zHstINImportedPolicyData].[LA1AccidentalDeathPremium], [zHstINImportedPolicyData].[LA1DisabilityCover], [zHstINImportedPolicyData].[LA1DisabilityPremium], [zHstINImportedPolicyData].[LA1FuneralCover], [zHstINImportedPolicyData].[LA1FuneralPremium], [zHstINImportedPolicyData].[LA2CancerCover], [zHstINImportedPolicyData].[LA2CancerPremium], [zHstINImportedPolicyData].[LA2AccidentalDeathCover], [zHstINImportedPolicyData].[LA2AccidentalDeathPremium], [zHstINImportedPolicyData].[LA2DisabilityCover], [zHstINImportedPolicyData].[LA2DisabilityPremium], [zHstINImportedPolicyData].[LA2FuneralCover], [zHstINImportedPolicyData].[LA2FuneralPremium], [zHstINImportedPolicyData].[KidsCancerCover], [zHstINImportedPolicyData].[KidsCancerPremium], [zHstINImportedPolicyData].[KidsDisabilityCover], [zHstINImportedPolicyData].[KidsDisabilityPremium], [zHstINImportedPolicyData].[PolicyFee], [zHstINImportedPolicyData].[MoneyBackPremium], [zHstINImportedPolicyData].[MoneyBackTerm], [zHstINImportedPolicyData].[StampDate], [zHstINImportedPolicyData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [zHstINImportedPolicyData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [zHstINImportedPolicyData] ");
                query.Append(" WHERE [zHstINImportedPolicyData].[ID] = @ID");
                query.Append(" ORDER BY [zHstINImportedPolicyData].[StampDate] DESC");
                parameters = new object[1];
                parameters[0] = Database.GetParameter("@ID", inimportedpolicydata.ID);
            }
            return query.ToString();
        }
        #endregion

        #region Save
        /// <summary>
        /// Creates a query to save a(n) inimportedpolicydata to the database.
        /// </summary>
        /// <param name="inimportedpolicydata">The inimportedpolicydata to save.</param>
        /// <returns>A query that can be used to save the inimportedpolicydata to the database.</returns>
        internal static string Save(INImportedPolicyData inimportedpolicydata, ref object[] parameters)
        {
            StringBuilder query = new StringBuilder();
            if (inimportedpolicydata != null)
            {
                if (inimportedpolicydata.IsLoaded)
                {
                    query.Append("INSERT INTO [zHstINImportedPolicyData] ([ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID]) SELECT [ID], [CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID] FROM [INImportedPolicyData] WHERE [INImportedPolicyData].[ID] = @ID; ");
                    query.Append("UPDATE [INImportedPolicyData]");
                    parameters = new object[29];
                    query.Append(" SET [CommenceDate] = @CommenceDate");
                    parameters[0] = Database.GetParameter("@CommenceDate", inimportedpolicydata.CommenceDate.HasValue ? (object)inimportedpolicydata.CommenceDate.Value : DBNull.Value);
                    query.Append(", [AppSignDate] = @AppSignDate");
                    parameters[1] = Database.GetParameter("@AppSignDate", inimportedpolicydata.AppSignDate.HasValue ? (object)inimportedpolicydata.AppSignDate.Value : DBNull.Value);
                    query.Append(", [ContractPremium] = @ContractPremium");
                    parameters[2] = Database.GetParameter("@ContractPremium", inimportedpolicydata.ContractPremium.HasValue ? (object)inimportedpolicydata.ContractPremium.Value : DBNull.Value);
                    query.Append(", [ContractTerm] = @ContractTerm");
                    parameters[3] = Database.GetParameter("@ContractTerm", inimportedpolicydata.ContractTerm.HasValue ? (object)inimportedpolicydata.ContractTerm.Value : DBNull.Value);
                    query.Append(", [LapseDate] = @LapseDate");
                    parameters[4] = Database.GetParameter("@LapseDate", inimportedpolicydata.LapseDate.HasValue ? (object)inimportedpolicydata.LapseDate.Value : DBNull.Value);
                    query.Append(", [LA1CancerCover] = @LA1CancerCover");
                    parameters[5] = Database.GetParameter("@LA1CancerCover", inimportedpolicydata.LA1CancerCover.HasValue ? (object)inimportedpolicydata.LA1CancerCover.Value : DBNull.Value);
                    query.Append(", [LA1CancerPremium] = @LA1CancerPremium");
                    parameters[6] = Database.GetParameter("@LA1CancerPremium", inimportedpolicydata.LA1CancerPremium.HasValue ? (object)inimportedpolicydata.LA1CancerPremium.Value : DBNull.Value);
                    query.Append(", [LA1AccidentalDeathCover] = @LA1AccidentalDeathCover");
                    parameters[7] = Database.GetParameter("@LA1AccidentalDeathCover", inimportedpolicydata.LA1AccidentalDeathCover.HasValue ? (object)inimportedpolicydata.LA1AccidentalDeathCover.Value : DBNull.Value);
                    query.Append(", [LA1AccidentalDeathPremium] = @LA1AccidentalDeathPremium");
                    parameters[8] = Database.GetParameter("@LA1AccidentalDeathPremium", inimportedpolicydata.LA1AccidentalDeathPremium.HasValue ? (object)inimportedpolicydata.LA1AccidentalDeathPremium.Value : DBNull.Value);
                    query.Append(", [LA1DisabilityCover] = @LA1DisabilityCover");
                    parameters[9] = Database.GetParameter("@LA1DisabilityCover", inimportedpolicydata.LA1DisabilityCover.HasValue ? (object)inimportedpolicydata.LA1DisabilityCover.Value : DBNull.Value);
                    query.Append(", [LA1DisabilityPremium] = @LA1DisabilityPremium");
                    parameters[10] = Database.GetParameter("@LA1DisabilityPremium", inimportedpolicydata.LA1DisabilityPremium.HasValue ? (object)inimportedpolicydata.LA1DisabilityPremium.Value : DBNull.Value);
                    query.Append(", [LA1FuneralCover] = @LA1FuneralCover");
                    parameters[11] = Database.GetParameter("@LA1FuneralCover", inimportedpolicydata.LA1FuneralCover.HasValue ? (object)inimportedpolicydata.LA1FuneralCover.Value : DBNull.Value);
                    query.Append(", [LA1FuneralPremium] = @LA1FuneralPremium");
                    parameters[12] = Database.GetParameter("@LA1FuneralPremium", inimportedpolicydata.LA1FuneralPremium.HasValue ? (object)inimportedpolicydata.LA1FuneralPremium.Value : DBNull.Value);
                    query.Append(", [LA2CancerCover] = @LA2CancerCover");
                    parameters[13] = Database.GetParameter("@LA2CancerCover", inimportedpolicydata.LA2CancerCover.HasValue ? (object)inimportedpolicydata.LA2CancerCover.Value : DBNull.Value);
                    query.Append(", [LA2CancerPremium] = @LA2CancerPremium");
                    parameters[14] = Database.GetParameter("@LA2CancerPremium", inimportedpolicydata.LA2CancerPremium.HasValue ? (object)inimportedpolicydata.LA2CancerPremium.Value : DBNull.Value);
                    query.Append(", [LA2AccidentalDeathCover] = @LA2AccidentalDeathCover");
                    parameters[15] = Database.GetParameter("@LA2AccidentalDeathCover", inimportedpolicydata.LA2AccidentalDeathCover.HasValue ? (object)inimportedpolicydata.LA2AccidentalDeathCover.Value : DBNull.Value);
                    query.Append(", [LA2AccidentalDeathPremium] = @LA2AccidentalDeathPremium");
                    parameters[16] = Database.GetParameter("@LA2AccidentalDeathPremium", inimportedpolicydata.LA2AccidentalDeathPremium.HasValue ? (object)inimportedpolicydata.LA2AccidentalDeathPremium.Value : DBNull.Value);
                    query.Append(", [LA2DisabilityCover] = @LA2DisabilityCover");
                    parameters[17] = Database.GetParameter("@LA2DisabilityCover", inimportedpolicydata.LA2DisabilityCover.HasValue ? (object)inimportedpolicydata.LA2DisabilityCover.Value : DBNull.Value);
                    query.Append(", [LA2DisabilityPremium] = @LA2DisabilityPremium");
                    parameters[18] = Database.GetParameter("@LA2DisabilityPremium", inimportedpolicydata.LA2DisabilityPremium.HasValue ? (object)inimportedpolicydata.LA2DisabilityPremium.Value : DBNull.Value);
                    query.Append(", [LA2FuneralCover] = @LA2FuneralCover");
                    parameters[19] = Database.GetParameter("@LA2FuneralCover", inimportedpolicydata.LA2FuneralCover.HasValue ? (object)inimportedpolicydata.LA2FuneralCover.Value : DBNull.Value);
                    query.Append(", [LA2FuneralPremium] = @LA2FuneralPremium");
                    parameters[20] = Database.GetParameter("@LA2FuneralPremium", inimportedpolicydata.LA2FuneralPremium.HasValue ? (object)inimportedpolicydata.LA2FuneralPremium.Value : DBNull.Value);
                    query.Append(", [KidsCancerCover] = @KidsCancerCover");
                    parameters[21] = Database.GetParameter("@KidsCancerCover", inimportedpolicydata.KidsCancerCover.HasValue ? (object)inimportedpolicydata.KidsCancerCover.Value : DBNull.Value);
                    query.Append(", [KidsCancerPremium] = @KidsCancerPremium");
                    parameters[22] = Database.GetParameter("@KidsCancerPremium", inimportedpolicydata.KidsCancerPremium.HasValue ? (object)inimportedpolicydata.KidsCancerPremium.Value : DBNull.Value);
                    query.Append(", [KidsDisabilityCover] = @KidsDisabilityCover");
                    parameters[23] = Database.GetParameter("@KidsDisabilityCover", inimportedpolicydata.KidsDisabilityCover.HasValue ? (object)inimportedpolicydata.KidsDisabilityCover.Value : DBNull.Value);
                    query.Append(", [KidsDisabilityPremium] = @KidsDisabilityPremium");
                    parameters[24] = Database.GetParameter("@KidsDisabilityPremium", inimportedpolicydata.KidsDisabilityPremium.HasValue ? (object)inimportedpolicydata.KidsDisabilityPremium.Value : DBNull.Value);
                    query.Append(", [PolicyFee] = @PolicyFee");
                    parameters[25] = Database.GetParameter("@PolicyFee", inimportedpolicydata.PolicyFee.HasValue ? (object)inimportedpolicydata.PolicyFee.Value : DBNull.Value);
                    query.Append(", [MoneyBackPremium] = @MoneyBackPremium");
                    parameters[26] = Database.GetParameter("@MoneyBackPremium", inimportedpolicydata.MoneyBackPremium.HasValue ? (object)inimportedpolicydata.MoneyBackPremium.Value : DBNull.Value);
                    query.Append(", [MoneyBackTerm] = @MoneyBackTerm");
                    parameters[27] = Database.GetParameter("@MoneyBackTerm", inimportedpolicydata.MoneyBackTerm.HasValue ? (object)inimportedpolicydata.MoneyBackTerm.Value : DBNull.Value);
                    query.Append(", [StampDate] = " + Database.CurrentDateTime + ", [StampUserID] = " + GlobalSettings.ApplicationUser.ID);
                    query.Append(" WHERE [INImportedPolicyData].[ID] = @ID"); 
                    parameters[28] = Database.GetParameter("@ID", inimportedpolicydata.ID);
                }
                else
                {
                    query.Append("INSERT INTO [INImportedPolicyData] ([CommenceDate], [AppSignDate], [ContractPremium], [ContractTerm], [LapseDate], [LA1CancerCover], [LA1CancerPremium], [LA1AccidentalDeathCover], [LA1AccidentalDeathPremium], [LA1DisabilityCover], [LA1DisabilityPremium], [LA1FuneralCover], [LA1FuneralPremium], [LA2CancerCover], [LA2CancerPremium], [LA2AccidentalDeathCover], [LA2AccidentalDeathPremium], [LA2DisabilityCover], [LA2DisabilityPremium], [LA2FuneralCover], [LA2FuneralPremium], [KidsCancerCover], [KidsCancerPremium], [KidsDisabilityCover], [KidsDisabilityPremium], [PolicyFee], [MoneyBackPremium], [MoneyBackTerm], [StampDate], [StampUserID]) VALUES(@CommenceDate, @AppSignDate, @ContractPremium, @ContractTerm, @LapseDate, @LA1CancerCover, @LA1CancerPremium, @LA1AccidentalDeathCover, @LA1AccidentalDeathPremium, @LA1DisabilityCover, @LA1DisabilityPremium, @LA1FuneralCover, @LA1FuneralPremium, @LA2CancerCover, @LA2CancerPremium, @LA2AccidentalDeathCover, @LA2AccidentalDeathPremium, @LA2DisabilityCover, @LA2DisabilityPremium, @LA2FuneralCover, @LA2FuneralPremium, @KidsCancerCover, @KidsCancerPremium, @KidsDisabilityCover, @KidsDisabilityPremium, @PolicyFee, @MoneyBackPremium, @MoneyBackTerm, " + Database.CurrentDateTime + ", " + GlobalSettings.ApplicationUser.ID + ");");
                    parameters = new object[28];
                    parameters[0] = Database.GetParameter("@CommenceDate", inimportedpolicydata.CommenceDate.HasValue ? (object)inimportedpolicydata.CommenceDate.Value : DBNull.Value);
                    parameters[1] = Database.GetParameter("@AppSignDate", inimportedpolicydata.AppSignDate.HasValue ? (object)inimportedpolicydata.AppSignDate.Value : DBNull.Value);
                    parameters[2] = Database.GetParameter("@ContractPremium", inimportedpolicydata.ContractPremium.HasValue ? (object)inimportedpolicydata.ContractPremium.Value : DBNull.Value);
                    parameters[3] = Database.GetParameter("@ContractTerm", inimportedpolicydata.ContractTerm.HasValue ? (object)inimportedpolicydata.ContractTerm.Value : DBNull.Value);
                    parameters[4] = Database.GetParameter("@LapseDate", inimportedpolicydata.LapseDate.HasValue ? (object)inimportedpolicydata.LapseDate.Value : DBNull.Value);
                    parameters[5] = Database.GetParameter("@LA1CancerCover", inimportedpolicydata.LA1CancerCover.HasValue ? (object)inimportedpolicydata.LA1CancerCover.Value : DBNull.Value);
                    parameters[6] = Database.GetParameter("@LA1CancerPremium", inimportedpolicydata.LA1CancerPremium.HasValue ? (object)inimportedpolicydata.LA1CancerPremium.Value : DBNull.Value);
                    parameters[7] = Database.GetParameter("@LA1AccidentalDeathCover", inimportedpolicydata.LA1AccidentalDeathCover.HasValue ? (object)inimportedpolicydata.LA1AccidentalDeathCover.Value : DBNull.Value);
                    parameters[8] = Database.GetParameter("@LA1AccidentalDeathPremium", inimportedpolicydata.LA1AccidentalDeathPremium.HasValue ? (object)inimportedpolicydata.LA1AccidentalDeathPremium.Value : DBNull.Value);
                    parameters[9] = Database.GetParameter("@LA1DisabilityCover", inimportedpolicydata.LA1DisabilityCover.HasValue ? (object)inimportedpolicydata.LA1DisabilityCover.Value : DBNull.Value);
                    parameters[10] = Database.GetParameter("@LA1DisabilityPremium", inimportedpolicydata.LA1DisabilityPremium.HasValue ? (object)inimportedpolicydata.LA1DisabilityPremium.Value : DBNull.Value);
                    parameters[11] = Database.GetParameter("@LA1FuneralCover", inimportedpolicydata.LA1FuneralCover.HasValue ? (object)inimportedpolicydata.LA1FuneralCover.Value : DBNull.Value);
                    parameters[12] = Database.GetParameter("@LA1FuneralPremium", inimportedpolicydata.LA1FuneralPremium.HasValue ? (object)inimportedpolicydata.LA1FuneralPremium.Value : DBNull.Value);
                    parameters[13] = Database.GetParameter("@LA2CancerCover", inimportedpolicydata.LA2CancerCover.HasValue ? (object)inimportedpolicydata.LA2CancerCover.Value : DBNull.Value);
                    parameters[14] = Database.GetParameter("@LA2CancerPremium", inimportedpolicydata.LA2CancerPremium.HasValue ? (object)inimportedpolicydata.LA2CancerPremium.Value : DBNull.Value);
                    parameters[15] = Database.GetParameter("@LA2AccidentalDeathCover", inimportedpolicydata.LA2AccidentalDeathCover.HasValue ? (object)inimportedpolicydata.LA2AccidentalDeathCover.Value : DBNull.Value);
                    parameters[16] = Database.GetParameter("@LA2AccidentalDeathPremium", inimportedpolicydata.LA2AccidentalDeathPremium.HasValue ? (object)inimportedpolicydata.LA2AccidentalDeathPremium.Value : DBNull.Value);
                    parameters[17] = Database.GetParameter("@LA2DisabilityCover", inimportedpolicydata.LA2DisabilityCover.HasValue ? (object)inimportedpolicydata.LA2DisabilityCover.Value : DBNull.Value);
                    parameters[18] = Database.GetParameter("@LA2DisabilityPremium", inimportedpolicydata.LA2DisabilityPremium.HasValue ? (object)inimportedpolicydata.LA2DisabilityPremium.Value : DBNull.Value);
                    parameters[19] = Database.GetParameter("@LA2FuneralCover", inimportedpolicydata.LA2FuneralCover.HasValue ? (object)inimportedpolicydata.LA2FuneralCover.Value : DBNull.Value);
                    parameters[20] = Database.GetParameter("@LA2FuneralPremium", inimportedpolicydata.LA2FuneralPremium.HasValue ? (object)inimportedpolicydata.LA2FuneralPremium.Value : DBNull.Value);
                    parameters[21] = Database.GetParameter("@KidsCancerCover", inimportedpolicydata.KidsCancerCover.HasValue ? (object)inimportedpolicydata.KidsCancerCover.Value : DBNull.Value);
                    parameters[22] = Database.GetParameter("@KidsCancerPremium", inimportedpolicydata.KidsCancerPremium.HasValue ? (object)inimportedpolicydata.KidsCancerPremium.Value : DBNull.Value);
                    parameters[23] = Database.GetParameter("@KidsDisabilityCover", inimportedpolicydata.KidsDisabilityCover.HasValue ? (object)inimportedpolicydata.KidsDisabilityCover.Value : DBNull.Value);
                    parameters[24] = Database.GetParameter("@KidsDisabilityPremium", inimportedpolicydata.KidsDisabilityPremium.HasValue ? (object)inimportedpolicydata.KidsDisabilityPremium.Value : DBNull.Value);
                    parameters[25] = Database.GetParameter("@PolicyFee", inimportedpolicydata.PolicyFee.HasValue ? (object)inimportedpolicydata.PolicyFee.Value : DBNull.Value);
                    parameters[26] = Database.GetParameter("@MoneyBackPremium", inimportedpolicydata.MoneyBackPremium.HasValue ? (object)inimportedpolicydata.MoneyBackPremium.Value : DBNull.Value);
                    parameters[27] = Database.GetParameter("@MoneyBackTerm", inimportedpolicydata.MoneyBackTerm.HasValue ? (object)inimportedpolicydata.MoneyBackTerm.Value : DBNull.Value);
                    query.Append("SELECT " + Database.LastInsertedRowID + " AS 'NewID'; ");
                }
            }
            return query.ToString();
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inimportedpolicydatas that match the search criteria.
        /// </summary>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="appsigndate">The appsigndate search criteria.</param>
        /// <param name="contractpremium">The contractpremium search criteria.</param>
        /// <param name="contractterm">The contractterm search criteria.</param>
        /// <param name="lapsedate">The lapsedate search criteria.</param>
        /// <param name="la1cancercover">The la1cancercover search criteria.</param>
        /// <param name="la1cancerpremium">The la1cancerpremium search criteria.</param>
        /// <param name="la1accidentaldeathcover">The la1accidentaldeathcover search criteria.</param>
        /// <param name="la1accidentaldeathpremium">The la1accidentaldeathpremium search criteria.</param>
        /// <param name="la1disabilitycover">The la1disabilitycover search criteria.</param>
        /// <param name="la1disabilitypremium">The la1disabilitypremium search criteria.</param>
        /// <param name="la1funeralcover">The la1funeralcover search criteria.</param>
        /// <param name="la1funeralpremium">The la1funeralpremium search criteria.</param>
        /// <param name="la2cancercover">The la2cancercover search criteria.</param>
        /// <param name="la2cancerpremium">The la2cancerpremium search criteria.</param>
        /// <param name="la2accidentaldeathcover">The la2accidentaldeathcover search criteria.</param>
        /// <param name="la2accidentaldeathpremium">The la2accidentaldeathpremium search criteria.</param>
        /// <param name="la2disabilitycover">The la2disabilitycover search criteria.</param>
        /// <param name="la2disabilitypremium">The la2disabilitypremium search criteria.</param>
        /// <param name="la2funeralcover">The la2funeralcover search criteria.</param>
        /// <param name="la2funeralpremium">The la2funeralpremium search criteria.</param>
        /// <param name="kidscancercover">The kidscancercover search criteria.</param>
        /// <param name="kidscancerpremium">The kidscancerpremium search criteria.</param>
        /// <param name="kidsdisabilitycover">The kidsdisabilitycover search criteria.</param>
        /// <param name="kidsdisabilitypremium">The kidsdisabilitypremium search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="moneybackpremium">The moneybackpremium search criteria.</param>
        /// <param name="moneybackterm">The moneybackterm search criteria.</param>
        /// <returns>A query that can be used to search for inimportedpolicydatas based on the search criteria.</returns>
        internal static string Search(DateTime? commencedate, DateTime? appsigndate, decimal? contractpremium, int? contractterm, DateTime? lapsedate, decimal? la1cancercover, decimal? la1cancerpremium, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1disabilitycover, decimal? la1disabilitypremium, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la2cancercover, decimal? la2cancerpremium, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2disabilitycover, decimal? la2disabilitypremium, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? kidscancercover, decimal? kidscancerpremium, decimal? kidsdisabilitycover, decimal? kidsdisabilitypremium, decimal? policyfee, decimal? moneybackpremium, int? moneybackterm)
        {
            StringBuilder whereQuery = new StringBuilder();
            StringBuilder query = new StringBuilder();

            if (commencedate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[CommenceDate] = '" + commencedate.Value.ToString(Database.DateFormat) + "'");
            }
            if (appsigndate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[AppSignDate] = '" + appsigndate.Value.ToString(Database.DateFormat) + "'");
            }
            if (contractpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[ContractPremium] = " + contractpremium + "");
            }
            if (contractterm != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[ContractTerm] = " + contractterm + "");
            }
            if (lapsedate != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LapseDate] = '" + lapsedate.Value.ToString(Database.DateFormat) + "'");
            }
            if (la1cancercover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1CancerCover] = " + la1cancercover + "");
            }
            if (la1cancerpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1CancerPremium] = " + la1cancerpremium + "");
            }
            if (la1accidentaldeathcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1AccidentalDeathCover] = " + la1accidentaldeathcover + "");
            }
            if (la1accidentaldeathpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1AccidentalDeathPremium] = " + la1accidentaldeathpremium + "");
            }
            if (la1disabilitycover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1DisabilityCover] = " + la1disabilitycover + "");
            }
            if (la1disabilitypremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1DisabilityPremium] = " + la1disabilitypremium + "");
            }
            if (la1funeralcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1FuneralCover] = " + la1funeralcover + "");
            }
            if (la1funeralpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA1FuneralPremium] = " + la1funeralpremium + "");
            }
            if (la2cancercover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2CancerCover] = " + la2cancercover + "");
            }
            if (la2cancerpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2CancerPremium] = " + la2cancerpremium + "");
            }
            if (la2accidentaldeathcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2AccidentalDeathCover] = " + la2accidentaldeathcover + "");
            }
            if (la2accidentaldeathpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2AccidentalDeathPremium] = " + la2accidentaldeathpremium + "");
            }
            if (la2disabilitycover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2DisabilityCover] = " + la2disabilitycover + "");
            }
            if (la2disabilitypremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2DisabilityPremium] = " + la2disabilitypremium + "");
            }
            if (la2funeralcover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2FuneralCover] = " + la2funeralcover + "");
            }
            if (la2funeralpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[LA2FuneralPremium] = " + la2funeralpremium + "");
            }
            if (kidscancercover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[KidsCancerCover] = " + kidscancercover + "");
            }
            if (kidscancerpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[KidsCancerPremium] = " + kidscancerpremium + "");
            }
            if (kidsdisabilitycover != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[KidsDisabilityCover] = " + kidsdisabilitycover + "");
            }
            if (kidsdisabilitypremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[KidsDisabilityPremium] = " + kidsdisabilitypremium + "");
            }
            if (policyfee != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[PolicyFee] = " + policyfee + "");
            }
            if (moneybackpremium != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[MoneyBackPremium] = " + moneybackpremium + "");
            }
            if (moneybackterm != null)
            {
                whereQuery.Append(whereQuery.Length > 0 ? " AND " : " WHERE ");
                whereQuery.Append("[INImportedPolicyData].[MoneyBackTerm] = " + moneybackterm + "");
            }
            query.Append("SELECT [INImportedPolicyData].[ID], [INImportedPolicyData].[CommenceDate], [INImportedPolicyData].[AppSignDate], [INImportedPolicyData].[ContractPremium], [INImportedPolicyData].[ContractTerm], [INImportedPolicyData].[LapseDate], [INImportedPolicyData].[LA1CancerCover], [INImportedPolicyData].[LA1CancerPremium], [INImportedPolicyData].[LA1AccidentalDeathCover], [INImportedPolicyData].[LA1AccidentalDeathPremium], [INImportedPolicyData].[LA1DisabilityCover], [INImportedPolicyData].[LA1DisabilityPremium], [INImportedPolicyData].[LA1FuneralCover], [INImportedPolicyData].[LA1FuneralPremium], [INImportedPolicyData].[LA2CancerCover], [INImportedPolicyData].[LA2CancerPremium], [INImportedPolicyData].[LA2AccidentalDeathCover], [INImportedPolicyData].[LA2AccidentalDeathPremium], [INImportedPolicyData].[LA2DisabilityCover], [INImportedPolicyData].[LA2DisabilityPremium], [INImportedPolicyData].[LA2FuneralCover], [INImportedPolicyData].[LA2FuneralPremium], [INImportedPolicyData].[KidsCancerCover], [INImportedPolicyData].[KidsCancerPremium], [INImportedPolicyData].[KidsDisabilityCover], [INImportedPolicyData].[KidsDisabilityPremium], [INImportedPolicyData].[PolicyFee], [INImportedPolicyData].[MoneyBackPremium], [INImportedPolicyData].[MoneyBackTerm], [INImportedPolicyData].[StampDate], [INImportedPolicyData].[StampUserID]");
            query.Append(", (SELECT [Ref].[LoginName] FROM [User] AS [Ref] WHERE [Ref].[ID] = [INImportedPolicyData].[StampUserID]) AS 'StampUser'");
            query.Append(" FROM [INImportedPolicyData] ");
            return query.ToString() + whereQuery.ToString();
        }
        #endregion
    }
}
