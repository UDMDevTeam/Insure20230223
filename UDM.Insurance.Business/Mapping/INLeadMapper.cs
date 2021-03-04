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
    /// Contains methods to fill, save and delete inlead objects.
    /// </summary>
    public partial class INLeadMapper
    {
        #region Delete
        /// <summary>
        /// Detetes a(n) inlead object from the database.
        /// </summary>
        /// <param name="inlead">The id of the inlead object to delete from the database.</param>
        /// <param name="trans">The transaction object to use to delete the inlead object.</param>
        /// <returns>True if the inlead object was deleted successfully, else false.</returns>
        internal static bool Delete(INLead inlead)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inlead.ConnectionName, INLeadQueries.Delete(inlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete a INLead object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Detetes the history of a(n) inlead object from the database.
        /// </summary>
        /// <param name="inlead">The id of the inlead object to delete history of.</param>
        /// <param name="trans">The transaction object to use to delete the inlead history.</param>
        /// <returns>True if the inlead history was deleted successfully, else false.</returns>
        internal static bool DeleteHistory(INLead inlead)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inlead.ConnectionName, INLeadQueries.DeleteHistory(inlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to delete INLead history from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Undetetes a(n) inlead object from the database.
        /// </summary>
        /// <param name="inlead">The id of the inlead object to undelete.</param>
        /// <param name="trans">The transaction object to use to undelete the inlead object.</param>
        /// <returns>True if the inlead object was undeleted successfully, else false.</returns>
        internal static bool UnDelete(INLead inlead)
        {
            int affectedRows = 0;
            try
            {
                object[] parameters = null;
                affectedRows = Database.ExecuteCommand(inlead.ConnectionName, INLeadQueries.UnDelete(inlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to undelete a INLead object from the database", ex);
            }
            return affectedRows > 0 ? true : false;
        }
        #endregion

        #region Fill
        /// <summary>
        /// Populates the inlead object from the data reader.
        /// </summary>
        /// <param name="inlead">The inlead object to populate.</param>
        /// <param name="reader">The data reader with the data.</param>
        private static void Fill(INLead inlead, IDataReader reader)
        {
            try
            {
                if (reader["ID"] != DBNull.Value)
                {
                    inlead.IsLoaded = true;
                    inlead.IDNo = reader["IDNo"] != DBNull.Value ? (string)reader["IDNo"] : (string)null;
                    inlead.PassportNo = reader["PassportNo"] != DBNull.Value ? (string)reader["PassportNo"] : (string)null;
                    inlead.FKINTitleID = reader["FKINTitleID"] != DBNull.Value ? (long)reader["FKINTitleID"] : (long?)null;
                    inlead.Initials = reader["Initials"] != DBNull.Value ? (string)reader["Initials"] : (string)null;
                    inlead.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : (string)null;
                    inlead.Surname = reader["Surname"] != DBNull.Value ? (string)reader["Surname"] : (string)null;
                    inlead.FKLanguageID = reader["FKLanguageID"] != DBNull.Value ? (long)reader["FKLanguageID"] : (long?)null;
                    inlead.FKGenderID = reader["FKGenderID"] != DBNull.Value ? (long)reader["FKGenderID"] : (long?)null;
                    inlead.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : (DateTime?)null;
                    inlead.YearOfBirth = reader["YearOfBirth"] != DBNull.Value ? (string)reader["YearOfBirth"] : (string)null;
                    inlead.TelWork = reader["TelWork"] != DBNull.Value ? (string)reader["TelWork"] : (string)null;
                    inlead.TelHome = reader["TelHome"] != DBNull.Value ? (string)reader["TelHome"] : (string)null;
                    inlead.TelCell = reader["TelCell"] != DBNull.Value ? (string)reader["TelCell"] : (string)null;
                    inlead.TelOther = reader["TelOther"] != DBNull.Value ? (string)reader["TelOther"] : (string)null;
                    inlead.Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : (string)null;
                    inlead.Address1 = reader["Address1"] != DBNull.Value ? (string)reader["Address1"] : (string)null;
                    inlead.Address2 = reader["Address2"] != DBNull.Value ? (string)reader["Address2"] : (string)null;
                    inlead.Address3 = reader["Address3"] != DBNull.Value ? (string)reader["Address3"] : (string)null;
                    inlead.Address4 = reader["Address4"] != DBNull.Value ? (string)reader["Address4"] : (string)null;
                    inlead.Address5 = reader["Address5"] != DBNull.Value ? (string)reader["Address5"] : (string)null;
                    inlead.PostalCode = reader["PostalCode"] != DBNull.Value ? (string)reader["PostalCode"] : (string)null;
                    inlead.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : (string)null;
                    inlead.Occupation = reader["Occupation"] != DBNull.Value ? (string)reader["Occupation"] : (string)null;
                    inlead.StampDate = (DateTime)reader["StampDate"];
                    inlead.HasChanged = false;
                }
                else
                {
                    throw new MapperException("INLead does not exist in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLead object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inlead object from the database.
        /// </summary>
        /// <param name="inlead">The inlead to fill.</param>
        internal static void Fill(INLead inlead)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inlead.ConnectionName, INLeadQueries.Fill(inlead, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inlead, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLead object from the database", ex);
            }
        }

        /// <summary>
        /// Returns the data for a inlead object in the database.
        /// </summary>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet FillData(INLead inlead)
        {
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inlead.ConnectionName, INLeadQueries.FillData(inlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to return data for a INLead object from the database", ex);
            }
        }

        /// <summary>
        /// Fills an inlead object from the history table.
        /// </summary>
        /// <param name="stampUserID">ID of user who created the original record.</param>
        /// <param name="stampDate">Date and time when original record was created.</param>
        /// <param name="inlead">The inlead to fill from history.</param>
        internal static void FillHistory(INLead inlead, long stampUserID, DateTime stampDate)
        {
            try
            {
                object[] parameters = null;
                IDataReader reader = Database.ExecuteReader(inlead.ConnectionName, INLeadQueries.FillHistory(inlead, stampUserID, stampDate, ref parameters), parameters);
                if (reader.Read())
                {
                    Fill(inlead, reader);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to populate a INLead object from the database", ex);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Lists all the inlead objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A datareader containing the results of the query.</returns>
        public static INLeadCollection List(bool showDeleted, string connectionName)
        {
            INLeadCollection collection = new INLeadCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, showDeleted ? INLeadQueries.ListDeleted() : INLeadQueries.List(), null);
                while (reader.Read())
                {
                    INLead inlead = new INLead((long)reader["ID"]);
                    inlead.ConnectionName = connectionName;
                    collection.Add(inlead);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLead objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Lists all the inlead objects in the database.
        /// </summary>
        /// <param name="showDeleted">Indicates whether active or deleted records must be listed.</param>
        /// <param name="connectionName">Database connection name to use for the listing.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListData(bool showDeleted, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, showDeleted ? INLeadQueries.ListDeleted() : INLeadQueries.List(), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list INLead objects in the database", ex);
            }
        }

        /// <summary>
        /// Lists the history for a(n) inlead object from the database.
        /// </summary>
        /// <param name="inlead">The inlead to list history for.</param>
        /// <returns>A data set containing the results of the query.</returns>
        public static DataSet ListHistory(INLead inlead)
        {
            INLeadCollection collection = new INLeadCollection();
            try
            {
                object[] parameters = null;
                return Database.ExecuteDataSet(inlead.ConnectionName, INLeadQueries.ListHistory(inlead, ref parameters), parameters);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to list history for a(n) INLead in the database", ex);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves an inlead object to the database.
        /// </summary>
        /// <param name="inlead">The INLead object to save.</param>
        /// <param name="trans">The database object to use for the opperation.</param>
        /// <returns>True if the inlead was saved successfull, otherwise, false.</returns>
        internal static bool Save(INLead inlead)
        {
            bool result = true;
            try
            {
                object[] parameters = null;
                if (inlead.IsLoaded)
                {
                    int affectedRows = Database.ExecuteCommand(inlead.ConnectionName, INLeadQueries.Save(inlead, ref parameters), parameters);
                    result = affectedRows > 0;
                }
                else
                {
                    IDataReader reader = Database.ExecuteReader(inlead.ConnectionName, INLeadQueries.Save(inlead, ref parameters), parameters);
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            inlead.ID = (long)(decimal)reader["NewID"];
                        }
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    result &= inlead.ID != 0;
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to save a INLead object to the database", ex);
            }
            return result;
        }
        #endregion

        #region Search
        /// <summary>
        /// Searches for inlead objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="passportno">The passportno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="yearofbirth">The yearofbirth search criteria.</param>
        /// <param name="telwork">The telwork search criteria.</param>
        /// <param name="telhome">The telhome search criteria.</param>
        /// <param name="telcell">The telcell search criteria.</param>
        /// <param name="telother">The telother search criteria.</param>
        /// <param name="address">The address search criteria.</param>
        /// <param name="address1">The address1 search criteria.</param>
        /// <param name="address2">The address2 search criteria.</param>
        /// <param name="address3">The address3 search criteria.</param>
        /// <param name="address4">The address4 search criteria.</param>
        /// <param name="address5">The address5 search criteria.</param>
        /// <param name="postalcode">The postalcode search criteria.</param>
        /// <param name="email">The email search criteria.</param>
        /// <param name="occupation">The occupation search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLeadCollection Search(string idno, string passportno, long? fkintitleid, string initials, string firstname, string surname, long? fklanguageid, long? fkgenderid, DateTime? dateofbirth, string yearofbirth, string telwork, string telhome, string telcell, string telother, string address, string address1, string address2, string address3, string address4, string address5, string postalcode, string email, string occupation, string connectionName)
        {
            INLeadCollection collection = new INLeadCollection();
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadQueries.Search(idno, passportno, fkintitleid, initials, firstname, surname, fklanguageid, fkgenderid, dateofbirth, yearofbirth, telwork, telhome, telcell, telother, address, address1, address2, address3, address4, address5, postalcode, email, occupation), null);
                while (reader.Read())
                {
                    INLead inlead = new INLead((long)reader["ID"]);
                    inlead.ConnectionName = connectionName;
                    collection.Add(inlead);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLead objects in the database", ex);
            }
            return collection;
        }

        /// <summary>
        /// Searches for inlead objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="passportno">The passportno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="yearofbirth">The yearofbirth search criteria.</param>
        /// <param name="telwork">The telwork search criteria.</param>
        /// <param name="telhome">The telhome search criteria.</param>
        /// <param name="telcell">The telcell search criteria.</param>
        /// <param name="telother">The telother search criteria.</param>
        /// <param name="address">The address search criteria.</param>
        /// <param name="address1">The address1 search criteria.</param>
        /// <param name="address2">The address2 search criteria.</param>
        /// <param name="address3">The address3 search criteria.</param>
        /// <param name="address4">The address4 search criteria.</param>
        /// <param name="address5">The address5 search criteria.</param>
        /// <param name="postalcode">The postalcode search criteria.</param>
        /// <param name="email">The email search criteria.</param>
        /// <param name="occupation">The occupation search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A data set containing the results of the search.</returns>
        public static DataSet SearchData(string idno, string passportno, long? fkintitleid, string initials, string firstname, string surname, long? fklanguageid, long? fkgenderid, DateTime? dateofbirth, string yearofbirth, string telwork, string telhome, string telcell, string telother, string address, string address1, string address2, string address3, string address4, string address5, string postalcode, string email, string occupation, string connectionName)
        {
            try
            {
                return Database.ExecuteDataSet(connectionName, INLeadQueries.Search(idno, passportno, fkintitleid, initials, firstname, surname, fklanguageid, fkgenderid, dateofbirth, yearofbirth, telwork, telhome, telcell, telother, address, address1, address2, address3, address4, address5, postalcode, email, occupation), null);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLead objects in the database", ex);
            }
        }

        /// <summary>
        /// Searches for the top 1 inlead objects in the database.
        /// </summary>
        /// <param name="idno">The idno search criteria.</param>
        /// <param name="passportno">The passportno search criteria.</param>
        /// <param name="fkintitleid">The fkintitleid search criteria.</param>
        /// <param name="initials">The initials search criteria.</param>
        /// <param name="firstname">The firstname search criteria.</param>
        /// <param name="surname">The surname search criteria.</param>
        /// <param name="fklanguageid">The fklanguageid search criteria.</param>
        /// <param name="fkgenderid">The fkgenderid search criteria.</param>
        /// <param name="dateofbirth">The dateofbirth search criteria.</param>
        /// <param name="yearofbirth">The yearofbirth search criteria.</param>
        /// <param name="telwork">The telwork search criteria.</param>
        /// <param name="telhome">The telhome search criteria.</param>
        /// <param name="telcell">The telcell search criteria.</param>
        /// <param name="telother">The telother search criteria.</param>
        /// <param name="address">The address search criteria.</param>
        /// <param name="address1">The address1 search criteria.</param>
        /// <param name="address2">The address2 search criteria.</param>
        /// <param name="address3">The address3 search criteria.</param>
        /// <param name="address4">The address4 search criteria.</param>
        /// <param name="address5">The address5 search criteria.</param>
        /// <param name="postalcode">The postalcode search criteria.</param>
        /// <param name="email">The email search criteria.</param>
        /// <param name="occupation">The occupation search criteria.</param>
        /// <param name="connectionName">Database connection name to use for the search.</param>
        /// <returns>A datareader containing the results of the search.</returns>
        public static INLead SearchOne(string idno, string passportno, long? fkintitleid, string initials, string firstname, string surname, long? fklanguageid, long? fkgenderid, DateTime? dateofbirth, string yearofbirth, string telwork, string telhome, string telcell, string telother, string address, string address1, string address2, string address3, string address4, string address5, string postalcode, string email, string occupation, string connectionName)
        {
            INLead inlead = null;
            try
            {
                IDataReader reader = Database.ExecuteReader(connectionName, INLeadQueries.Search(idno, passportno, fkintitleid, initials, firstname, surname, fklanguageid, fkgenderid, dateofbirth, yearofbirth, telwork, telhome, telcell, telother, address, address1, address2, address3, address4, address5, postalcode, email, occupation), null);
                if (reader.Read())
                {
                    inlead = new INLead((long)reader["ID"]);
                    inlead.ConnectionName = connectionName;
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to search for INLead objects in the database", ex);
            }
            return inlead;
        }
        #endregion
    }
}
