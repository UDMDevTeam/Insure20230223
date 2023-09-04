using Embriant.Framework.Data;
using Embriant.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDM.Insurance.Business.Objects;
using UDM.Insurance.Business.Queries;

namespace UDM.Insurance.Business.Mapping
{
    /// <summary>
    /// Contains methods to fill, save and delete calldata objects.
    /// </summary>
    public partial class ReferralMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) referral object from the database.
        /// </summary>
        /// <param name="referral">The id of the referral object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the referral object.</param>
        /// <returns>True if the referral object was deleted successfully, else false.</returns>
        internal static bool Delete(Referral refdata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(refdata.ConnectionName, ReferralQueries.Delete(refdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a Referral object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        /// <summary>
        /// Detetes the history of a(n) Referral object from the database.
        /// </summary>
        /// <param name="Referral">The id of the Referral object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the Referral history.</param>
        /// <returns>True if the Referral history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(Referral refdata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(refdata.ConnectionName, ReferralQueries.DeleteHistory(refdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete Referral history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) Referral object from the database.
        /// </summary>
        /// <param name="Referral">The id of the Referral object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the Referral object.</param>
        /// <returns>True if the Referral object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(Referral refdata)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(refdata.ConnectionName, ReferralQueries.UnDelete(refdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a Referral object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the Referral object from the data reader.
        /// </summary>
        /// <param name="Referral">The calldata object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(Referral refdata, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    refdata.IsLoaded = true;
                    refdata.FKINImportID = reader["FKINImportID"] != DBNull.Value ? (string)reader["FKINImportID"] : (string)null;
                    refdata.ReferralNumber = reader["ReferralNumber"] != DBNull.Value ? (string)reader["ReferralNumber"] : (string)null;
                    refdata.Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : (string)null;
                    refdata.CellNumber = reader["CellNumber"] != DBNull.Value ? (string)reader["CellNumber"] : (string)null;
                    refdata.FKINRelationshipID = reader["FKINRelationshipID"] != DBNull.Value ? (long)reader["FKINRelationshipID"] : (long?)null;
                    refdata.FKGenderID = reader["FKGenderID"] != DBNull.Value ? (long)reader["FKGenderID"] : (long?)null;
                    // Just think i might add stampDate and UserId to the class 
                    refdata.HasChanged = false;
                }
                else
                {
                    throw new MapperException("Referral does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Referral object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an calldata object from the database.
        /// </summary>
        /// <param name="Referral">The calldata to fill.</param>
        internal static void Fill(Referral refdata)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(refdata.ConnectionName, ReferralQueries.Fill(refdata, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(refdata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Referral object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a Referral object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(Referral refdata)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(refdata.ConnectionName, ReferralQueries.FillData(refdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a Referral object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an Referral object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="Referral">The calldata to fill from history.</param>
        internal static void FillHistory(Referral refdata, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(refdata.ConnectionName, ReferralQueries.FillHistory(refdata, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(refdata, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a Referral object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the Referral objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static ReferralCollection List(bool showDeleted, string connectionName)
        {
            ReferralCollection collection = new ReferralCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? ReferralQueries.ListDeleted() : ReferralQueries.List(), null);
                while (reader.Read())
                {
                    Referral refdata = new Referral((long)reader["ID"]);
                    refdata.ConnectionName = connectionName;
                    collection.Add(refdata);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Referral objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the Referral objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? ReferralQueries.ListDeleted() : ReferralQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list Referral objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) Referral object from the database.
        /// </summary>
        /// <param name="Referral">The Referral to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(Referral refdata)
        {
            ReferralCollection collection = new ReferralCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(refdata.ConnectionName, ReferralQueries.ListHistory(refdata, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) Referral in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an Referral object to the database.
        /// </summary>
        /// <param name="Referral">The Referral object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the Referral was saved successfull, otherwise, false.</returns>
        internal static bool Save(Referral refdata)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (refdata.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(refdata.ConnectionName, ReferralQueries.Save(refdata, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(refdata.ConnectionName, ReferralQueries.Save(refdata, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            refdata.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= refdata.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a Referral object to the database", ex);
            }
            return result;
        }
    }
}
#endregion