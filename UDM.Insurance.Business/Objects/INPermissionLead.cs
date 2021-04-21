using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business
{
    public partial class INPermissionLead : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private string _title = null;
        private string _firstname = null;
        private string _surname = null;
        private string _cellnumber = null;
        private string _altnumber = null;
        private string _savedby = null;
        private DateTime? _datesaved = null;
        bool LoadedIS;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportNote class.
        /// </summary>
        public INPermissionLead()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportNote class.
        /// </summary>
        public INPermissionLead(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINImportID
        {
            get
            {
                Fill();
                return _fkinimportid;
            }
            set 
            {
                Fill();
                if (value != _fkinimportid)
                {
                    _fkinimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Title
        {
            get
            {
                Fill();
                return _title;
            }
            set
            {
                Fill();
                if (value != _title)
                {
                    _title = value;
                    _hasChanged = true;
                }
            }
        }

        public string Firstname
        {
            get
            {
                Fill();
                return _firstname;
            }
            set 
            {
                Fill();
                if (value != _firstname)
                {
                    _firstname = value;
                    _hasChanged = true;
                }
            }
        }

        public string Surname
        {
            get
            {
                Fill();
                return _surname;
            }
            set 
            {
                Fill();
                if (value != _surname)
                {
                    _surname = value;
                    _hasChanged = true;
                }
            }
        }

        public string Cellnumber
        {
            get
            {
                Fill();
                return _cellnumber;
            }
            set 
            {
                Fill();
                if (value != _cellnumber)
                {
                    _cellnumber = value;
                    _hasChanged = true;
                }
            }
        }

        public string AltNumber
        {
            get
            {
                Fill();
                return _altnumber;
            }
            set 
            {
                Fill();
                if (value != _altnumber)
                {
                    _altnumber = value;
                    _hasChanged = true;
                }
            }
        }

        public string SavedBy
        {
            get
            {
                Fill();
                return _savedby;
            }
            set
            {
                Fill();
                if (value != _savedby)
                {
                    _savedby = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DateSaved
        {
            get
            {
                Fill();
                return _datesaved;
            }
            set
            {
                Fill();
                if (value != _datesaved)
                {
                    _datesaved = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportNote object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPermissionLeadMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportNote object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPermissionLeadMapper.Save(this);
                    result &= _isLoaded;
                }
                _hasChanged = !result;
                return result && AfterSave(validationResult);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an INImportNote object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPermissionLeadMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportNote.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inpermissionlead>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<title>" + Title.ToString() + "</title>");
            xml.Append("<firstname>" + Firstname.ToString() + "</firstname>");
            xml.Append("<surname>" + Surname.ToString() + "</surname>");
            xml.Append("<cellnumber>" + Cellnumber.ToString() + "</note>");
            xml.Append("<altnumber>" + AltNumber.ToString() + "</altnumber>");
            xml.Append("</inimportnote>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportNote object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkuserid"></param>
        /// <param name="notedate"></param>
        /// <param name="note"></param>
        /// <param name="sequence"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, string title, string firstname, string surname, string cellnumber, string altnumber)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.Title = title;
                this.Firstname = firstname;
                this.Surname = surname;
                this.Cellnumber = cellnumber;
                this.AltNumber = altnumber;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportNote's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPermissionLeadMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportNote object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportNote object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPermissionLeadMapper.UnDelete(this);
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    #region Collection
    /// <summary>
    /// A collection of the INImportNote object.
    /// </summary>
    public partial class INPermissionLeadCollection : ObjectCollection<INPermissionLead>
    { 
    }
    #endregion
}
