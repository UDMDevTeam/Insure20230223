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
    /// Contains methods to fill, save and delete inpolicybeneficiary objects.
    /// </summary>
    public partial class INPolicyBeneficiaryMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inpolicybeneficiary object from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The id of the inpolicybeneficiary object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicybeneficiary object.</param>
        /// <returns>True if the inpolicybeneficiary object was deleted successfully, else false.</returns>
        internal static bool Delete(INPolicyBeneficiary inpolicybeneficiary)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.Delete(inpolicybeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INPolicyBeneficiary object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inpolicybeneficiary object from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The id of the inpolicybeneficiary object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inpolicybeneficiary history.</param>
        /// <returns>True if the inpolicybeneficiary history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INPolicyBeneficiary inpolicybeneficiary)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.DeleteHistory(inpolicybeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INPolicyBeneficiary history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inpolicybeneficiary object from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The id of the inpolicybeneficiary object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inpolicybeneficiary object.</param>
        /// <returns>True if the inpolicybeneficiary object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INPolicyBeneficiary inpolicybeneficiary)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.UnDelete(inpolicybeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INPolicyBeneficiary object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inpolicybeneficiary object from the data reader.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INPolicyBeneficiary inpolicybeneficiary, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inpolicybeneficiary.IsLoaded = true;
                    inpolicybeneficiary.FKINPolicyID = reader["FKINPolicyID"] != DBNull.Value ? (long)reader["FKINPolicyID"] : (long?)null;
                    inpolicybeneficiary.FKINBeneficiaryID = reader["FKINBeneficiaryID"] != DBNull.Value ? (long)reader["FKINBeneficiaryID"] : (long?)null;
                    inpolicybeneficiary.BeneficiaryRank = reader["BeneficiaryRank"] != DBNull.Value ? (int)reader["BeneficiaryRank"] : (int?)null;
                    inpolicybeneficiary.BeneficiaryPercentage = reader["BeneficiaryPercentage"] != DBNull.Value ? (Double)reader["BeneficiaryPercentage"] : (Double?)null;
                    inpolicybeneficiary.StampDate = (DateTime)reader["StampDate"];
                    inpolicybeneficiary.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INPolicyBeneficiary does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyBeneficiary object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicybeneficiary object from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary to fill.</param>
        internal static void Fill(INPolicyBeneficiary inpolicybeneficiary)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.Fill(inpolicybeneficiary, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicybeneficiary, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyBeneficiary object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inpolicybeneficiary object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INPolicyBeneficiary inpolicybeneficiary)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.FillData(inpolicybeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INPolicyBeneficiary object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inpolicybeneficiary object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary to fill from history.</param>
        internal static void FillHistory(INPolicyBeneficiary inpolicybeneficiary, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.FillHistory(inpolicybeneficiary, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inpolicybeneficiary, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INPolicyBeneficiary object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inpolicybeneficiary objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INPolicyBeneficiaryCollection List(bool showDeleted, string connectionName)
        {
            INPolicyBeneficiaryCollection collection = new INPolicyBeneficiaryCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INPolicyBeneficiaryQueries.ListDeleted() : INPolicyBeneficiaryQueries.List(), null);
                while (reader.Read())
                {
                    INPolicyBeneficiary inpolicybeneficiary = new INPolicyBeneficiary((long)reader["ID"]);
                    inpolicybeneficiary.ConnectionName = connectionName;
                    collection.Add(inpolicybeneficiary);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyBeneficiary objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inpolicybeneficiary objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INPolicyBeneficiaryQueries.ListDeleted() : INPolicyBeneficiaryQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INPolicyBeneficiary objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inpolicybeneficiary object from the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The inpolicybeneficiary to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INPolicyBeneficiary inpolicybeneficiary)
        {
            INPolicyBeneficiaryCollection collection = new INPolicyBeneficiaryCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.ListHistory(inpolicybeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INPolicyBeneficiary in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inpolicybeneficiary object to the database.
        /// </summary>
        /// <param name="inpolicybeneficiary">The INPolicyBeneficiary object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inpolicybeneficiary was saved successfull, otherwise, false.</returns>
        internal static bool Save(INPolicyBeneficiary inpolicybeneficiary)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inpolicybeneficiary.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.Save(inpolicybeneficiary, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inpolicybeneficiary.ConnectionName, INPolicyBeneficiaryQueries.Save(inpolicybeneficiary, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inpolicybeneficiary.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inpolicybeneficiary.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INPolicyBeneficiary object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inpolicybeneficiary objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinbeneficiaryid">The fkinbeneficiaryid search criteria.</param>
        /// <param name="beneficiaryrank">The beneficiaryrank search criteria.</param>
        /// <param name="beneficiarypercentage">The beneficiarypercentage search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyBeneficiaryCollection Search(long? fkinpolicyid, long? fkinbeneficiaryid, int? beneficiaryrank, Double? beneficiarypercentage, string connectionName)
        {
            INPolicyBeneficiaryCollection collection = new INPolicyBeneficiaryCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyBeneficiaryQueries.Search(fkinpolicyid, fkinbeneficiaryid, beneficiaryrank, beneficiarypercentage), null);
                while (reader.Read())
                {
                    INPolicyBeneficiary inpolicybeneficiary = new INPolicyBeneficiary((long)reader["ID"]);
                    inpolicybeneficiary.ConnectionName = connectionName;
                    collection.Add(inpolicybeneficiary);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyBeneficiary objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inpolicybeneficiary objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinbeneficiaryid">The fkinbeneficiaryid search criteria.</param>
        /// <param name="beneficiaryrank">The beneficiaryrank search criteria.</param>
        /// <param name="beneficiarypercentage">The beneficiarypercentage search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(long? fkinpolicyid, long? fkinbeneficiaryid, int? beneficiaryrank, Double? beneficiarypercentage, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INPolicyBeneficiaryQueries.Search(fkinpolicyid, fkinbeneficiaryid, beneficiaryrank, beneficiarypercentage), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyBeneficiary objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inpolicybeneficiary objects in the database.
        /// </summary>
        /// <param name="fkinpolicyid">The fkinpolicyid search criteria.</param>
        /// <param name="fkinbeneficiaryid">The fkinbeneficiaryid search criteria.</param>
        /// <param name="beneficiaryrank">The beneficiaryrank search criteria.</param>
        /// <param name="beneficiarypercentage">The beneficiarypercentage search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INPolicyBeneficiary SearchOne(long? fkinpolicyid, long? fkinbeneficiaryid, int? beneficiaryrank, Double? beneficiarypercentage, string connectionName)
        {
            INPolicyBeneficiary inpolicybeneficiary = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INPolicyBeneficiaryQueries.Search(fkinpolicyid, fkinbeneficiaryid, beneficiaryrank, beneficiarypercentage), null);
                if (reader.Read())
                {
                    inpolicybeneficiary = new INPolicyBeneficiary((long)reader["ID"]);
                    inpolicybeneficiary.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INPolicyBeneficiary objects in the database", ex);
            }
            return inpolicybeneficiary;
        }
        #endregion
    }
}
