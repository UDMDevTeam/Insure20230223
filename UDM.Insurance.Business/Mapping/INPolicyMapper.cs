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
    /// Contains methods to fill, save and delete inpolicy objects.
    /// </summary>
    public partial class INPolicyMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inpolicy object from the database.
        /// </summary>
        /// <param name="inpolicy">The id of the inpolicy object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicy object.</param>
        /// <returns>True if the inpolicy object was deleted successfully, else false.</returns>
        internal static bool Delete(INPolicy inpolicy)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicy.ConnectionName, INPolicyQueries.Delete(inpolicy, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPolicy object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inpolicy object from the database.
        /// </summary>
        /// <param name="inpolicy">The id of the inpolicy object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicy history.</param>
        /// <returns>True if the inpolicy history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPolicy inpolicy)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicy.ConnectionName, INPolicyQueries.DeleteHistory(inpolicy, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPolicy history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inpolicy object from the database.
        /// </summary>
        /// <param name="inpolicy">The id of the inpolicy object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inpolicy object.</param>
        /// <returns>True if the inpolicy object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPolicy inpolicy)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicy.ConnectionName, INPolicyQueries.UnDelete(inpolicy, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPolicy object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inpolicy object from the data reader.
        /// </summary>
        /// <param name="inpolicy">The inpolicy object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INPolicy inpolicy, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inpolicy.IsLoaded = true;
                    inpolicy.PolicyID = reader["PolicyID"] != DBNull.Value ? (string)reader["PolicyID"] : (string)null;
                    inpolicy.FKPolicyTypeID = reader["FKPolicyTypeID"] != DBNull.Value ? (long)reader["FKPolicyTypeID"] : (long?)null;
                    inpolicy.FKINPolicyHolderID = reader["FKINPolicyHolderID"] != DBNull.Value ? (long)reader["FKINPolicyHolderID"] : (long?)null;
                    inpolicy.FKINBankDetailsID = reader["FKINBankDetailsID"] != DBNull.Value ? (long)reader["FKINBankDetailsID"] : (long?)null;
                    inpolicy.FKINOptionID = reader["FKINOptionID"] != DBNull.Value ? (long)reader["FKINOptionID"] : (long?)null;
                    inpolicy.FKINMoneyBackID = reader["FKINMoneyBackID"] != DBNull.Value ? (long)reader["FKINMoneyBackID"] : (long?)null;
                    inpolicy.FKINBumpUpOptionID = reader["FKINBumpUpOptionID"] != DBNull.Value ? (long)reader["FKINBumpUpOptionID"] : (long?)null;
                    inpolicy.UDMBumpUpOption = reader["UDMBumpUpOption"] != DBNull.Value ? (bool)reader["UDMBumpUpOption"] : (bool?)null;
                    inpolicy.BumpUpAmount = reader["BumpUpAmount"] != DBNull.Value ? (decimal)reader["BumpUpAmount"] : (decimal?)null;
                    inpolicy.ReducedPremiumOption = reader["ReducedPremiumOption"] != DBNull.Value ? (bool)reader["ReducedPremiumOption"] : (bool?)null;
                    inpolicy.ReducedPremiumAmount = reader["ReducedPremiumAmount"] != DBNull.Value ? (decimal)reader["ReducedPremiumAmount"] : (decimal?)null;
                    inpolicy.PolicyFee = reader["PolicyFee"] != DBNull.Value ? (decimal)reader["PolicyFee"] : (decimal?)null;
                    inpolicy.TotalPremium = reader["TotalPremium"] != DBNull.Value ? (decimal)reader["TotalPremium"] : (decimal?)null;
                    inpolicy.CommenceDate = reader["CommenceDate"] != DBNull.Value ? (DateTime)reader["CommenceDate"] : (DateTime?)null;
                    inpolicy.OptionLA2 = reader["OptionLA2"] != DBNull.Value ? (bool)reader["OptionLA2"] : (bool?)null;
                    inpolicy.OptionChild = reader["OptionChild"] != DBNull.Value ? (bool)reader["OptionChild"] : (bool?)null;
                    inpolicy.OptionFuneral = reader["OptionFuneral"] != DBNull.Value ? (bool)reader["OptionFuneral"] : (bool?)null;
                    inpolicy.BumpUpOffered = reader["BumpUpOffered"] != DBNull.Value ? (bool)reader["BumpUpOffered"] : (bool?)null;
                    inpolicy.TotalInvoiceFee = reader["TotalInvoiceFee"] != DBNull.Value ? (decimal)reader["TotalInvoiceFee"] : (decimal?)null;
                    inpolicy.FKINOptionFeesID = reader["FKINOptionFeesID"] != DBNull.Value ? (long)reader["FKINOptionFeesID"] : (long?)null;
                    inpolicy.StampDate = (DateTime)reader["StampDate"];
                    inpolicy.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPolicy does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicy object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicy object from the database.
        /// </summary>
        /// <param name="inpolicy">The inpolicy to fill.</param>
        internal static void Fill(INPolicy inpolicy)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicy.ConnectionName, INPolicyQueries.Fill(inpolicy, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicy, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicy object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inpolicy object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPolicy inpolicy)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicy.ConnectionName, INPolicyQueries.FillData(inpolicy, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPolicy object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicy object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inpolicy">The inpolicy to fill from history.</param>
        internal static void FillHistory(INPolicy inpolicy, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicy.ConnectionName, INPolicyQueries.FillHistory(inpolicy, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicy, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicy object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicy objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INPolicyCollection List(bool showDeleted, string connectionName)
        {
            INPolicyCollection collection = new INPolicyCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPolicyQueries.ListDeleted() : INPolicyQueries.List(), null);
                while (reader.Read())
                {
                    INPolicy inpolicy = new INPolicy((long)reader["ID"]);
                    inpolicy.ConnectionName = connectionName;
                    collection.Add(inpolicy);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicy objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inpolicy objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPolicyQueries.ListDeleted() : INPolicyQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicy objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inpolicy object from the database.
        /// </summary>
        /// <param name="inpolicy">The inpolicy to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INPolicy inpolicy)
        {
            INPolicyCollection collection = new INPolicyCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicy.ConnectionName, INPolicyQueries.ListHistory(inpolicy, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPolicy in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inpolicy object to the database.
        /// </summary>
        /// <param name="inpolicy">The INPolicy object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inpolicy was saved successfull, otherwise, false.</returns>
        internal static bool Save(INPolicy inpolicy)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inpolicy.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inpolicy.ConnectionName, INPolicyQueries.Save(inpolicy, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inpolicy.ConnectionName, INPolicyQueries.Save(inpolicy, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inpolicy.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inpolicy.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPolicy object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicy objects in the database.
        /// </summary>
        /// <param name="policyid">The policyid search criteria.</param>
        /// <param name="fkpolicytypeid">The fkpolicytypeid search criteria.</param>
        /// <param name="fkinpolicyholderid">The fkinpolicyholderid search criteria.</param>
        /// <param name="fkinbankdetailsid">The fkinbankdetailsid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinmoneybackid">The fkinmoneybackid search criteria.</param>
        /// <param name="fkinbumpupoptionid">The fkinbumpupoptionid search criteria.</param>
        /// <param name="udmbumpupoption">The udmbumpupoption search criteria.</param>
        /// <param name="bumpupamount">The bumpupamount search criteria.</param>
        /// <param name="reducedpremiumoption">The reducedpremiumoption search criteria.</param>
        /// <param name="reducedpremiumamount">The reducedpremiumamount search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="totalpremium">The totalpremium search criteria.</param>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="optionla2">The optionla2 search criteria.</param>
        /// <param name="optionchild">The optionchild search criteria.</param>
        /// <param name="optionfuneral">The optionfuneral search criteria.</param>
        /// <param name="bumpupoffered">The bumpupoffered search criteria.</param>
        /// <param name="totalinvoicefee">The totalinvoicefee search criteria.</param>
        /// <param name="fkinoptionfeesid">The fkinoptionfeesid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyCollection Search(string policyid, long? fkpolicytypeid, long? fkinpolicyholderid, long? fkinbankdetailsid, long? fkinoptionid, long? fkinmoneybackid, long? fkinbumpupoptionid, bool? udmbumpupoption, decimal? bumpupamount, bool? reducedpremiumoption, decimal? reducedpremiumamount, decimal? policyfee, decimal? totalpremium, DateTime? commencedate, bool? optionla2, bool? optionchild, bool? optionfuneral, bool? bumpupoffered, decimal? totalinvoicefee, long? fkinoptionfeesid, string connectionName)
        {
            INPolicyCollection collection = new INPolicyCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyQueries.Search(policyid, fkpolicytypeid, fkinpolicyholderid, fkinbankdetailsid, fkinoptionid, fkinmoneybackid, fkinbumpupoptionid, udmbumpupoption, bumpupamount, reducedpremiumoption, reducedpremiumamount, policyfee, totalpremium, commencedate, optionla2, optionchild, optionfuneral, bumpupoffered, totalinvoicefee, fkinoptionfeesid), null);
                while (reader.Read())
                {
                    INPolicy inpolicy = new INPolicy((long)reader["ID"]);
                    inpolicy.ConnectionName = connectionName;
                    collection.Add(inpolicy);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicy objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inpolicy objects in the database.
        /// </summary>
        /// <param name="policyid">The policyid search criteria.</param>
        /// <param name="fkpolicytypeid">The fkpolicytypeid search criteria.</param>
        /// <param name="fkinpolicyholderid">The fkinpolicyholderid search criteria.</param>
        /// <param name="fkinbankdetailsid">The fkinbankdetailsid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinmoneybackid">The fkinmoneybackid search criteria.</param>
        /// <param name="fkinbumpupoptionid">The fkinbumpupoptionid search criteria.</param>
        /// <param name="udmbumpupoption">The udmbumpupoption search criteria.</param>
        /// <param name="bumpupamount">The bumpupamount search criteria.</param>
        /// <param name="reducedpremiumoption">The reducedpremiumoption search criteria.</param>
        /// <param name="reducedpremiumamount">The reducedpremiumamount search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="totalpremium">The totalpremium search criteria.</param>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="optionla2">The optionla2 search criteria.</param>
        /// <param name="optionchild">The optionchild search criteria.</param>
        /// <param name="optionfuneral">The optionfuneral search criteria.</param>
        /// <param name="bumpupoffered">The bumpupoffered search criteria.</param>
        /// <param name="totalinvoicefee">The totalinvoicefee search criteria.</param>
        /// <param name="fkinoptionfeesid">The fkinoptionfeesid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string policyid, long? fkpolicytypeid, long? fkinpolicyholderid, long? fkinbankdetailsid, long? fkinoptionid, long? fkinmoneybackid, long? fkinbumpupoptionid, bool? udmbumpupoption, decimal? bumpupamount, bool? reducedpremiumoption, decimal? reducedpremiumamount, decimal? policyfee, decimal? totalpremium, DateTime? commencedate, bool? optionla2, bool? optionchild, bool? optionfuneral, bool? bumpupoffered, decimal? totalinvoicefee, long? fkinoptionfeesid, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPolicyQueries.Search(policyid, fkpolicytypeid, fkinpolicyholderid, fkinbankdetailsid, fkinoptionid, fkinmoneybackid, fkinbumpupoptionid, udmbumpupoption, bumpupamount, reducedpremiumoption, reducedpremiumamount, policyfee, totalpremium, commencedate, optionla2, optionchild, optionfuneral, bumpupoffered, totalinvoicefee, fkinoptionfeesid), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicy objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inpolicy objects in the database.
        /// </summary>
        /// <param name="policyid">The policyid search criteria.</param>
        /// <param name="fkpolicytypeid">The fkpolicytypeid search criteria.</param>
        /// <param name="fkinpolicyholderid">The fkinpolicyholderid search criteria.</param>
        /// <param name="fkinbankdetailsid">The fkinbankdetailsid search criteria.</param>
        /// <param name="fkinoptionid">The fkinoptionid search criteria.</param>
        /// <param name="fkinmoneybackid">The fkinmoneybackid search criteria.</param>
        /// <param name="fkinbumpupoptionid">The fkinbumpupoptionid search criteria.</param>
        /// <param name="udmbumpupoption">The udmbumpupoption search criteria.</param>
        /// <param name="bumpupamount">The bumpupamount search criteria.</param>
        /// <param name="reducedpremiumoption">The reducedpremiumoption search criteria.</param>
        /// <param name="reducedpremiumamount">The reducedpremiumamount search criteria.</param>
        /// <param name="policyfee">The policyfee search criteria.</param>
        /// <param name="totalpremium">The totalpremium search criteria.</param>
        /// <param name="commencedate">The commencedate search criteria.</param>
        /// <param name="optionla2">The optionla2 search criteria.</param>
        /// <param name="optionchild">The optionchild search criteria.</param>
        /// <param name="optionfuneral">The optionfuneral search criteria.</param>
        /// <param name="bumpupoffered">The bumpupoffered search criteria.</param>
        /// <param name="totalinvoicefee">The totalinvoicefee search criteria.</param>
        /// <param name="fkinoptionfeesid">The fkinoptionfeesid search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicy SearchOne(string policyid, long? fkpolicytypeid, long? fkinpolicyholderid, long? fkinbankdetailsid, long? fkinoptionid, long? fkinmoneybackid, long? fkinbumpupoptionid, bool? udmbumpupoption, decimal? bumpupamount, bool? reducedpremiumoption, decimal? reducedpremiumamount, decimal? policyfee, decimal? totalpremium, DateTime? commencedate, bool? optionla2, bool? optionchild, bool? optionfuneral, bool? bumpupoffered, decimal? totalinvoicefee, long? fkinoptionfeesid, string connectionName)
        {
            INPolicy inpolicy = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyQueries.Search(policyid, fkpolicytypeid, fkinpolicyholderid, fkinbankdetailsid, fkinoptionid, fkinmoneybackid, fkinbumpupoptionid, udmbumpupoption, bumpupamount, reducedpremiumoption, reducedpremiumamount, policyfee, totalpremium, commencedate, optionla2, optionchild, optionfuneral, bumpupoffered, totalinvoicefee, fkinoptionfeesid), null);
                if (reader.Read())
                {
                    inpolicy = new INPolicy((long)reader["ID"]);
                    inpolicy.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicy objects in the database", ex);
            }
            return inpolicy;
        }
        #endregion
    }
}
