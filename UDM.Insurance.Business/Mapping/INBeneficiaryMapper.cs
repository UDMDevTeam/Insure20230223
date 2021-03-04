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
    /// Contains methods to fill, save and delete inbeneficiary objects.
    /// </summary>
    public partial class INBeneficiaryMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inbeneficiary object from the database.
        /// </summary>
        /// <param name="inbeneficiary">The id of the inbeneficiary object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inbeneficiary object.</param>
        /// <returns>True if the inbeneficiary object was deleted successfully, else false.</returns>
        internal static bool Delete(INBeneficiary inbeneficiary)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbeneficiary.ConnectionName, INBeneficiaryQueries.Delete(inbeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INBeneficiary object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inbeneficiary object from the database.
        /// </summary>
        /// <param name="inbeneficiary">The id of the inbeneficiary object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inbeneficiary history.</param>
        /// <returns>True if the inbeneficiary history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INBeneficiary inbeneficiary)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbeneficiary.ConnectionName, INBeneficiaryQueries.DeleteHistory(inbeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INBeneficiary history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inbeneficiary object from the database.
        /// </summary>
        /// <param name="inbeneficiary">The id of the inbeneficiary object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inbeneficiary object.</param>
        /// <returns>True if the inbeneficiary object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INBeneficiary inbeneficiary)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inbeneficiary.ConnectionName, INBeneficiaryQueries.UnDelete(inbeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INBeneficiary object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inbeneficiary object from the data reader.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INBeneficiary inbeneficiary, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inbeneficiary.IsLoaded = true;
                    inbeneficiary.IDNo = reader["IDNo"] != DBNull.Value ? (string)reader["IDNo"] : (string)null;
                    inbeneficiary.FKINTitleID = reader["FKINTitleID"] != DBNull.Value ? (long)reader["FKINTitleID"] : (long?)null;
                    inbeneficiary.FKGenderID = reader["FKGenderID"] != DBNull.Value ? (long)reader["FKGenderID"] : (long?)null;
                    inbeneficiary.Initials = reader["Initials"] != DBNull.Value ? (string)reader["Initials"] : (string)null;
                    inbeneficiary.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : (string)null;
                    inbeneficiary.Surname = reader["Surname"] != DBNull.Value ? (string)reader["Surname"] : (string)null;
                    inbeneficiary.FKINRelationshipID = reader["FKINRelationshipID"] != DBNull.Value ? (long)reader["FKINRelationshipID"] : (long?)null;
                    inbeneficiary.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : (DateTime?)null;
                    inbeneficiary.ToDeleteID = reader["ToDeleteID"] != DBNull.Value ? (long)reader["ToDeleteID"] : (long?)null;
                    inbeneficiary.Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : (string)null;
                    inbeneficiary.TelContact = reader["TelContact"] != DBNull.Value ? (string)reader["TelContact"] : (string)null;
                    inbeneficiary.StampDate = (DateTime)reader["StampDate"];
                    inbeneficiary.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INBeneficiary does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBeneficiary object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbeneficiary object from the database.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary to fill.</param>
        internal static void Fill(INBeneficiary inbeneficiary)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbeneficiary.ConnectionName, INBeneficiaryQueries.Fill(inbeneficiary, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbeneficiary, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBeneficiary object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inbeneficiary object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INBeneficiary inbeneficiary)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbeneficiary.ConnectionName, INBeneficiaryQueries.FillData(inbeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INBeneficiary object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inbeneficiary object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inbeneficiary">The inbeneficiary to fill from history.</param>
        internal static void FillHistory(INBeneficiary inbeneficiary, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inbeneficiary.ConnectionName, INBeneficiaryQueries.FillHistory(inbeneficiary, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inbeneficiary, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INBeneficiary object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inbeneficiary objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INBeneficiaryCollection List(bool showDeleted, string connectionName)
        {
            INBeneficiaryCollection collection = new INBeneficiaryCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INBeneficiaryQueries.ListDeleted() : INBeneficiaryQueries.List(), null);
                while (reader.Read())
                {
                    INBeneficiary inbeneficiary = new INBeneficiary((long)reader["ID"]);
                    inbeneficiary.ConnectionName = connectionName;
                    collection.Add(inbeneficiary);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBeneficiary objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inbeneficiary objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INBeneficiaryQueries.ListDeleted() : INBeneficiaryQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INBeneficiary objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inbeneficiary object from the database.
        /// </summary>
        /// <param name="inbeneficiary">The inbeneficiary to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INBeneficiary inbeneficiary)
        {
            INBeneficiaryCollection collection = new INBeneficiaryCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inbeneficiary.ConnectionName, INBeneficiaryQueries.ListHistory(inbeneficiary, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INBeneficiary in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inbeneficiary object to the database.
        /// </summary>
        /// <param name="inbeneficiary">The INBeneficiary object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inbeneficiary was saved successfull, otherwise, false.</returns>
        internal static bool Save(INBeneficiary inbeneficiary)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inbeneficiary.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inbeneficiary.ConnectionName, INBeneficiaryQueries.Save(inbeneficiary, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inbeneficiary.ConnectionName, INBeneficiaryQueries.Save(inbeneficiary, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inbeneficiary.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inbeneficiary.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INBeneficiary object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inbeneficiary objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="notes">The notes search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBeneficiaryCollection Search(string idno, long? fkintitleid, long? fkgenderid, string initials, string firstname, string surname, long? fkinrelationshipid, DateTime? dateofbirth, long? todeleteid, string notes, string telcontact, string connectionName)
        {
            INBeneficiaryCollection collection = new INBeneficiaryCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBeneficiaryQueries.Search(idno, fkintitleid, fkgenderid, initials, firstname, surname, fkinrelationshipid, dateofbirth, todeleteid, notes, telcontact), null);
                while (reader.Read())
                {
                    INBeneficiary inbeneficiary = new INBeneficiary((long)reader["ID"]);
                    inbeneficiary.ConnectionName = connectionName;
                    collection.Add(inbeneficiary);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBeneficiary objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inbeneficiary objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="notes">The notes search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string idno, long? fkintitleid, long? fkgenderid, string initials, string firstname, string surname, long? fkinrelationshipid, DateTime? dateofbirth, long? todeleteid, string notes, string telcontact, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INBeneficiaryQueries.Search(idno, fkintitleid, fkgenderid, initials, firstname, surname, fkinrelationshipid, dateofbirth, todeleteid, notes, telcontact), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBeneficiary objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inbeneficiary objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="notes">The notes search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INBeneficiary SearchOne(string idno, long? fkintitleid, long? fkgenderid, string initials, string firstname, string surname, long? fkinrelationshipid, DateTime? dateofbirth, long? todeleteid, string notes, string telcontact, string connectionName)
        {
            INBeneficiary inbeneficiary = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INBeneficiaryQueries.Search(idno, fkintitleid, fkgenderid, initials, firstname, surname, fkinrelationshipid, dateofbirth, todeleteid, notes, telcontact), null);
                if (reader.Read())
                {
                    inbeneficiary = new INBeneficiary((long)reader["ID"]);
                    inbeneficiary.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INBeneficiary objects in the database", ex);
            }
            return inbeneficiary;
        }
        #endregion
    }
}
