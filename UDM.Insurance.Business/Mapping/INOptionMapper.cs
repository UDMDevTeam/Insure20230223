using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;

using UDM.Insurance.Business;
using UDM.Insurance.Business.Queries;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains methods to fill, save and delete inoption objects.
    /// </summary>
    public partial class INOptionMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inoption object from the database.
        /// </summary>
        /// <param name="inoption">The id of the inoption object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inoption object.</param>
        /// <returns>True if the inoption object was deleted successfully, else false.</returns>
        internal static bool Delete(INOption inoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoption.ConnectionName, INOptionQueries.Delete(inoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inoption object from the database.
        /// </summary>
        /// <param name="inoption">The id of the inoption object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inoption history.</param>
        /// <returns>True if the inoption history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INOption inoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoption.ConnectionName, INOptionQueries.DeleteHistory(inoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INOption history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inoption object from the database.
        /// </summary>
        /// <param name="inoption">The id of the inoption object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inoption object.</param>
        /// <returns>True if the inoption object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INOption inoption)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inoption.ConnectionName, INOptionQueries.UnDelete(inoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INOption object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inoption object from the data reader.
        /// </summary>
        /// <param name="inoption">The inoption object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INOption inoption, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inoption.IsLoaded = true;
                    inoption.FKINPlanID = reader["FKINPlanID"] != DBNull.Value ? (long)reader["FKINPlanID"] : (long?)null;
                    inoption.OptionCode = reader["OptionCode"] != DBNull.Value ? (string)reader["OptionCode"] : (string)null;
                    inoption.Category = reader["Category"] != DBNull.Value ? (string)reader["Category"] : (string)null;
                    inoption.Display = reader["Display"] != DBNull.Value ? (string)reader["Display"] : (string)null;
                    inoption.PolicyFee = reader["PolicyFee"] != DBNull.Value ? (decimal)reader["PolicyFee"] : (decimal?)null;
                    inoption.LA1Component = reader["LA1Component"] != DBNull.Value ? (string)reader["LA1Component"] : (string)null;
                    inoption.LA1Cover = reader["LA1Cover"] != DBNull.Value ? (decimal)reader["LA1Cover"] : (decimal?)null;
                    inoption.LA1Premium = reader["LA1Premium"] != DBNull.Value ? (decimal)reader["LA1Premium"] : (decimal?)null;
                    inoption.LA1Cost = reader["LA1Cost"] != DBNull.Value ? (decimal)reader["LA1Cost"] : (decimal?)null;
                    inoption.LA2Component = reader["LA2Component"] != DBNull.Value ? (string)reader["LA2Component"] : (string)null;
                    inoption.LA2Cover = reader["LA2Cover"] != DBNull.Value ? (decimal)reader["LA2Cover"] : (decimal?)null;
                    inoption.LA2Premium = reader["LA2Premium"] != DBNull.Value ? (decimal)reader["LA2Premium"] : (decimal?)null;
                    inoption.LA2Cost = reader["LA2Cost"] != DBNull.Value ? (decimal)reader["LA2Cost"] : (decimal?)null;
                    inoption.ChildComponent = reader["ChildComponent"] != DBNull.Value ? (string)reader["ChildComponent"] : (string)null;
                    inoption.ChildCover = reader["ChildCover"] != DBNull.Value ? (decimal)reader["ChildCover"] : (decimal?)null;
                    inoption.ChildPremium = reader["ChildPremium"] != DBNull.Value ? (decimal)reader["ChildPremium"] : (decimal?)null;
                    inoption.ChildCost = reader["ChildCost"] != DBNull.Value ? (decimal)reader["ChildCost"] : (decimal?)null;
                    inoption.FuneralComponent = reader["FuneralComponent"] != DBNull.Value ? (string)reader["FuneralComponent"] : (string)null;
                    inoption.FuneralCover = reader["FuneralCover"] != DBNull.Value ? (decimal)reader["FuneralCover"] : (decimal?)null;
                    inoption.FuneralPremium = reader["FuneralPremium"] != DBNull.Value ? (decimal)reader["FuneralPremium"] : (decimal?)null;
                    inoption.FuneralCost = reader["FuneralCost"] != DBNull.Value ? (decimal)reader["FuneralCost"] : (decimal?)null;
                    inoption.LA1AccidentalDeathComponent = reader["LA1AccidentalDeathComponent"] != DBNull.Value ? (string)reader["LA1AccidentalDeathComponent"] : (string)null;
                    inoption.LA1AccidentalDeathCover = reader["LA1AccidentalDeathCover"] != DBNull.Value ? (decimal)reader["LA1AccidentalDeathCover"] : (decimal?)null;
                    inoption.LA1AccidentalDeathPremium = reader["LA1AccidentalDeathPremium"] != DBNull.Value ? (decimal)reader["LA1AccidentalDeathPremium"] : (decimal?)null;
                    inoption.LA1AccidentalDeathCost = reader["LA1AccidentalDeathCost"] != DBNull.Value ? (decimal)reader["LA1AccidentalDeathCost"] : (decimal?)null;
                    inoption.LA2AccidentalDeathComponent = reader["LA2AccidentalDeathComponent"] != DBNull.Value ? (string)reader["LA2AccidentalDeathComponent"] : (string)null;
                    inoption.LA2AccidentalDeathCover = reader["LA2AccidentalDeathCover"] != DBNull.Value ? (decimal)reader["LA2AccidentalDeathCover"] : (decimal?)null;
                    inoption.LA2AccidentalDeathPremium = reader["LA2AccidentalDeathPremium"] != DBNull.Value ? (decimal)reader["LA2AccidentalDeathPremium"] : (decimal?)null;
                    inoption.LA2AccidentalDeathCost = reader["LA2AccidentalDeathCost"] != DBNull.Value ? (decimal)reader["LA2AccidentalDeathCost"] : (decimal?)null;
                    inoption.LA1FuneralComponent = reader["LA1FuneralComponent"] != DBNull.Value ? (string)reader["LA1FuneralComponent"] : (string)null;
                    inoption.LA1FuneralCover = reader["LA1FuneralCover"] != DBNull.Value ? (decimal)reader["LA1FuneralCover"] : (decimal?)null;
                    inoption.LA1FuneralPremium = reader["LA1FuneralPremium"] != DBNull.Value ? (decimal)reader["LA1FuneralPremium"] : (decimal?)null;
                    inoption.LA1FuneralCost = reader["LA1FuneralCost"] != DBNull.Value ? (decimal)reader["LA1FuneralCost"] : (decimal?)null;
                    inoption.LA2FuneralComponent = reader["LA2FuneralComponent"] != DBNull.Value ? (string)reader["LA2FuneralComponent"] : (string)null;
                    inoption.LA2FuneralCover = reader["LA2FuneralCover"] != DBNull.Value ? (decimal)reader["LA2FuneralCover"] : (decimal?)null;
                    inoption.LA2FuneralPremium = reader["LA2FuneralPremium"] != DBNull.Value ? (decimal)reader["LA2FuneralPremium"] : (decimal?)null;
                    inoption.LA2FuneralCost = reader["LA2FuneralCost"] != DBNull.Value ? (decimal)reader["LA2FuneralCost"] : (decimal?)null;
                    inoption.TotalPremium1 = reader["TotalPremium1"] != DBNull.Value ? (decimal)reader["TotalPremium1"] : (decimal?)null;
                    inoption.TotalPremium1SystemUnits = reader["TotalPremium1SystemUnits"] != DBNull.Value ? (decimal)reader["TotalPremium1SystemUnits"] : (decimal?)null;
                    inoption.TotalPremium1SalaryUnits = reader["TotalPremium1SalaryUnits"] != DBNull.Value ? (decimal)reader["TotalPremium1SalaryUnits"] : (decimal?)null;
                    inoption.TotalPremium2 = reader["TotalPremium2"] != DBNull.Value ? (decimal)reader["TotalPremium2"] : (decimal?)null;
                    inoption.TotalPremium2SystemUnits = reader["TotalPremium2SystemUnits"] != DBNull.Value ? (decimal)reader["TotalPremium2SystemUnits"] : (decimal?)null;
                    inoption.TotalPremium2SalaryUnits = reader["TotalPremium2SalaryUnits"] != DBNull.Value ? (decimal)reader["TotalPremium2SalaryUnits"] : (decimal?)null;
                    inoption.IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : (bool?)null;
                    inoption.StampDate = (DateTime)reader["StampDate"];
                    inoption.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INOption does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inoption object from the database.
        /// </summary>
        /// <param name="inoption">The inoption to fill.</param>
        internal static void Fill(INOption inoption)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inoption.ConnectionName, INOptionQueries.Fill(inoption, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOption object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inoption object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INOption inoption)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inoption.ConnectionName, INOptionQueries.FillData(inoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INOption object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inoption object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inoption">The inoption to fill from history.</param>
        internal static void FillHistory(INOption inoption, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inoption.ConnectionName, INOptionQueries.FillHistory(inoption, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inoption, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INOption object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INOptionCollection List(bool showDeleted, string connectionName)
        {
            INOptionCollection collection = new INOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INOptionQueries.ListDeleted() : INOptionQueries.List(), null);
                while (reader.Read())
                {
                    INOption inoption = new INOption((long)reader["ID"]);
                    inoption.ConnectionName = connectionName;
                    collection.Add(inoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inoption objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INOptionQueries.ListDeleted() : INOptionQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inoption object from the database.
        /// </summary>
        /// <param name="inoption">The inoption to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INOption inoption)
        {
            INOptionCollection collection = new INOptionCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inoption.ConnectionName, INOptionQueries.ListHistory(inoption, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INOption in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inoption object to the database.
        /// </summary>
        /// <param name="inoption">The INOption object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inoption was saved successfull, otherwise, false.</returns>
        internal static bool Save(INOption inoption)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inoption.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inoption.ConnectionName, INOptionQueries.Save(inoption, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inoption.ConnectionName, INOptionQueries.Save(inoption, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inoption.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inoption.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INOption object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inoption objects in the database.
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
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INOptionCollection Search(long? fkinplanid, string optioncode, string category, string display, decimal? policyfee, string la1component, decimal? la1cover, decimal? la1premium, decimal? la1cost, string la2component, decimal? la2cover, decimal? la2premium, decimal? la2cost, string childcomponent, decimal? childcover, decimal? childpremium, decimal? childcost, string funeralcomponent, decimal? funeralcover, decimal? funeralpremium, decimal? funeralcost, string la1accidentaldeathcomponent, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1accidentaldeathcost, string la2accidentaldeathcomponent, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2accidentaldeathcost, string la1funeralcomponent, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la1funeralcost, string la2funeralcomponent, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? la2funeralcost, decimal? totalpremium1, decimal? totalpremium1systemunits, decimal? totalpremium1salaryunits, decimal? totalpremium2, decimal? totalpremium2systemunits, decimal? totalpremium2salaryunits, bool? isactive, string connectionName)
        {
            INOptionCollection collection = new INOptionCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INOptionQueries.Search(fkinplanid, optioncode, category, display, policyfee, la1component, la1cover, la1premium, la1cost, la2component, la2cover, la2premium, la2cost, childcomponent, childcover, childpremium, childcost, funeralcomponent, funeralcover, funeralpremium, funeralcost, la1accidentaldeathcomponent, la1accidentaldeathcover, la1accidentaldeathpremium, la1accidentaldeathcost, la2accidentaldeathcomponent, la2accidentaldeathcover, la2accidentaldeathpremium, la2accidentaldeathcost, la1funeralcomponent, la1funeralcover, la1funeralpremium, la1funeralcost, la2funeralcomponent, la2funeralcover, la2funeralpremium, la2funeralcost, totalpremium1, totalpremium1systemunits, totalpremium1salaryunits, totalpremium2, totalpremium2systemunits, totalpremium2salaryunits, isactive), null);
                while (reader.Read())
                {
                    INOption inoption = new INOption((long)reader["ID"]);
                    inoption.ConnectionName = connectionName;
                    collection.Add(inoption);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOption objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inoption objects in the database.
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
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinplanid, string optioncode, string category, string display, decimal? policyfee, string la1component, decimal? la1cover, decimal? la1premium, decimal? la1cost, string la2component, decimal? la2cover, decimal? la2premium, decimal? la2cost, string childcomponent, decimal? childcover, decimal? childpremium, decimal? childcost, string funeralcomponent, decimal? funeralcover, decimal? funeralpremium, decimal? funeralcost, string la1accidentaldeathcomponent, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1accidentaldeathcost, string la2accidentaldeathcomponent, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2accidentaldeathcost, string la1funeralcomponent, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la1funeralcost, string la2funeralcomponent, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? la2funeralcost, decimal? totalpremium1, decimal? totalpremium1systemunits, decimal? totalpremium1salaryunits, decimal? totalpremium2, decimal? totalpremium2systemunits, decimal? totalpremium2salaryunits, bool? isactive, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INOptionQueries.Search(fkinplanid, optioncode, category, display, policyfee, la1component, la1cover, la1premium, la1cost, la2component, la2cover, la2premium, la2cost, childcomponent, childcover, childpremium, childcost, funeralcomponent, funeralcover, funeralpremium, funeralcost, la1accidentaldeathcomponent, la1accidentaldeathcover, la1accidentaldeathpremium, la1accidentaldeathcost, la2accidentaldeathcomponent, la2accidentaldeathcover, la2accidentaldeathpremium, la2accidentaldeathcost, la1funeralcomponent, la1funeralcover, la1funeralpremium, la1funeralcost, la2funeralcomponent, la2funeralcover, la2funeralpremium, la2funeralcost, totalpremium1, totalpremium1systemunits, totalpremium1salaryunits, totalpremium2, totalpremium2systemunits, totalpremium2salaryunits, isactive), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOption objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inoption objects in the database.
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
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INOption SearchOne(long? fkinplanid, string optioncode, string category, string display, decimal? policyfee, string la1component, decimal? la1cover, decimal? la1premium, decimal? la1cost, string la2component, decimal? la2cover, decimal? la2premium, decimal? la2cost, string childcomponent, decimal? childcover, decimal? childpremium, decimal? childcost, string funeralcomponent, decimal? funeralcover, decimal? funeralpremium, decimal? funeralcost, string la1accidentaldeathcomponent, decimal? la1accidentaldeathcover, decimal? la1accidentaldeathpremium, decimal? la1accidentaldeathcost, string la2accidentaldeathcomponent, decimal? la2accidentaldeathcover, decimal? la2accidentaldeathpremium, decimal? la2accidentaldeathcost, string la1funeralcomponent, decimal? la1funeralcover, decimal? la1funeralpremium, decimal? la1funeralcost, string la2funeralcomponent, decimal? la2funeralcover, decimal? la2funeralpremium, decimal? la2funeralcost, decimal? totalpremium1, decimal? totalpremium1systemunits, decimal? totalpremium1salaryunits, decimal? totalpremium2, decimal? totalpremium2systemunits, decimal? totalpremium2salaryunits, bool? isactive, string connectionName)
        {
            INOption inoption = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INOptionQueries.Search(fkinplanid, optioncode, category, display, policyfee, la1component, la1cover, la1premium, la1cost, la2component, la2cover, la2premium, la2cost, childcomponent, childcover, childpremium, childcost, funeralcomponent, funeralcover, funeralpremium, funeralcost, la1accidentaldeathcomponent, la1accidentaldeathcover, la1accidentaldeathpremium, la1accidentaldeathcost, la2accidentaldeathcomponent, la2accidentaldeathcover, la2accidentaldeathpremium, la2accidentaldeathcost, la1funeralcomponent, la1funeralcover, la1funeralpremium, la1funeralcost, la2funeralcomponent, la2funeralcover, la2funeralpremium, la2funeralcost, totalpremium1, totalpremium1systemunits, totalpremium1salaryunits, totalpremium2, totalpremium2systemunits, totalpremium2salaryunits, isactive), null);
                if (reader.Read())
                {
                    inoption = new INOption((long)reader["ID"]);
                    inoption.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INOption objects in the database", ex);
            }
            return inoption;
        }
        #endregion
    }
}
