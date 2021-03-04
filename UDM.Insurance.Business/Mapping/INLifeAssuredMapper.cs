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
    /// Contains methods to fill, save and delete inlifeassured objects.
    /// </summary>
    public partial class INLifeAssuredMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inlifeassured object from the database.
        /// </summary>
        /// <param name="inlifeassured">The id of the inlifeassured object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inlifeassured object.</param>
        /// <returns>True if the inlifeassured object was deleted successfully, else false.</returns>
        internal static bool Delete(INLifeAssured inlifeassured)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inlifeassured.ConnectionName, INLifeAssuredQueries.Delete(inlifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INLifeAssured object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inlifeassured object from the database.
        /// </summary>
        /// <param name="inlifeassured">The id of the inlifeassured object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inlifeassured history.</param>
        /// <returns>True if the inlifeassured history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INLifeAssured inlifeassured)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inlifeassured.ConnectionName, INLifeAssuredQueries.DeleteHistory(inlifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INLifeAssured history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inlifeassured object from the database.
        /// </summary>
        /// <param name="inlifeassured">The id of the inlifeassured object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inlifeassured object.</param>
        /// <returns>True if the inlifeassured object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INLifeAssured inlifeassured)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inlifeassured.ConnectionName, INLifeAssuredQueries.UnDelete(inlifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INLifeAssured object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inlifeassured object from the data reader.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INLifeAssured inlifeassured, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inlifeassured.IsLoaded = true;
                    inlifeassured.IDNo = reader["IDNo"] != DBNull.Value ? (string)reader["IDNo"] : (string)null;
                    inlifeassured.FKINTitleID = reader["FKINTitleID"] != DBNull.Value ? (long)reader["FKINTitleID"] : (long?)null;
                    inlifeassured.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : (string)null;
                    inlifeassured.Surname = reader["Surname"] != DBNull.Value ? (string)reader["Surname"] : (string)null;
                    inlifeassured.FKGenderID = reader["FKGenderID"] != DBNull.Value ? (long)reader["FKGenderID"] : (long?)null;
                    inlifeassured.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : (DateTime?)null;
                    inlifeassured.FKINRelationshipID = reader["FKINRelationshipID"] != DBNull.Value ? (long)reader["FKINRelationshipID"] : (long?)null;
                    inlifeassured.ToDeleteID = reader["ToDeleteID"] != DBNull.Value ? (long)reader["ToDeleteID"] : (long?)null;
                    inlifeassured.TelContact = reader["TelContact"] != DBNull.Value ? (string)reader["TelContact"] : (string)null;
                    inlifeassured.StampDate = (DateTime)reader["StampDate"];
                    inlifeassured.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INLifeAssured does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLifeAssured object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inlifeassured object from the database.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured to fill.</param>
        internal static void Fill(INLifeAssured inlifeassured)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inlifeassured.ConnectionName, INLifeAssuredQueries.Fill(inlifeassured, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inlifeassured, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLifeAssured object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inlifeassured object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INLifeAssured inlifeassured)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inlifeassured.ConnectionName, INLifeAssuredQueries.FillData(inlifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INLifeAssured object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inlifeassured object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inlifeassured">The inlifeassured to fill from history.</param>
        internal static void FillHistory(INLifeAssured inlifeassured, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inlifeassured.ConnectionName, INLifeAssuredQueries.FillHistory(inlifeassured, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inlifeassured, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLifeAssured object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inlifeassured objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INLifeAssuredCollection List(bool showDeleted, string connectionName)
        {
            INLifeAssuredCollection collection = new INLifeAssuredCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INLifeAssuredQueries.ListDeleted() : INLifeAssuredQueries.List(), null);
                while (reader.Read())
                {
                    INLifeAssured inlifeassured = new INLifeAssured((long)reader["ID"]);
                    inlifeassured.ConnectionName = connectionName;
                    collection.Add(inlifeassured);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLifeAssured objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inlifeassured objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INLifeAssuredQueries.ListDeleted() : INLifeAssuredQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLifeAssured objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inlifeassured object from the database.
        /// </summary>
        /// <param name="inlifeassured">The inlifeassured to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INLifeAssured inlifeassured)
        {
            INLifeAssuredCollection collection = new INLifeAssuredCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inlifeassured.ConnectionName, INLifeAssuredQueries.ListHistory(inlifeassured, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INLifeAssured in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inlifeassured object to the database.
        /// </summary>
        /// <param name="inlifeassured">The INLifeAssured object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inlifeassured was saved successfull, otherwise, false.</returns>
        internal static bool Save(INLifeAssured inlifeassured)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inlifeassured.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inlifeassured.ConnectionName, INLifeAssuredQueries.Save(inlifeassured, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inlifeassured.ConnectionName, INLifeAssuredQueries.Save(inlifeassured, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inlifeassured.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inlifeassured.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INLifeAssured object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inlifeassured objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLifeAssuredCollection Search(string idno, long? fkintitleid, string firstname, string surname, long? fkgenderid, DateTime? dateofbirth, long? fkinrelationshipid, long? todeleteid, string telcontact, string connectionName)
        {
            INLifeAssuredCollection collection = new INLifeAssuredCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLifeAssuredQueries.Search(idno, fkintitleid, firstname, surname, fkgenderid, dateofbirth, fkinrelationshipid, todeleteid, telcontact), null);
                while (reader.Read())
                {
                    INLifeAssured inlifeassured = new INLifeAssured((long)reader["ID"]);
                    inlifeassured.ConnectionName = connectionName;
                    collection.Add(inlifeassured);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLifeAssured objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inlifeassured objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string idno, long? fkintitleid, string firstname, string surname, long? fkgenderid, DateTime? dateofbirth, long? fkinrelationshipid, long? todeleteid, string telcontact, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INLifeAssuredQueries.Search(idno, fkintitleid, firstname, surname, fkgenderid, dateofbirth, fkinrelationshipid, todeleteid, telcontact), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLifeAssured objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inlifeassured objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="fkinrelationshipid">The fkinrelationshipid search criteria.</param>
        /// <param name="todeleteid">The todeleteid search criteria.</param>
        /// <param name="telcontact">The telcontact search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLifeAssured SearchOne(string idno, long? fkintitleid, string firstname, string surname, long? fkgenderid, DateTime? dateofbirth, long? fkinrelationshipid, long? todeleteid, string telcontact, string connectionName)
        {
            INLifeAssured inlifeassured = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLifeAssuredQueries.Search(idno, fkintitleid, firstname, surname, fkgenderid, dateofbirth, fkinrelationshipid, todeleteid, telcontact), null);
                if (reader.Read())
                {
                    inlifeassured = new INLifeAssured((long)reader["ID"]);
                    inlifeassured.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLifeAssured objects in the database", ex);
            }
            return inlifeassured;
        }
        #endregion
    }
}
