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
    public partial class INImportExtra : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private string _extension1 = null;
        private string _extension2 = null;
        private bool? _notpossiblebumpup = null;
        private long? _fkcmcallrefuserid = null;
        private bool? _emailrequested = null;
        private bool? _customercarerequested = null;
        private bool? _isgoldenlead = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportExtra class.
        /// </summary>
        public INImportExtra()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportExtra class.
        /// </summary>
        public INImportExtra(long id)
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

        public string Extension1
        {
            get
            {
                Fill();
                return _extension1;
            }
            set 
            {
                Fill();
                if (value != _extension1)
                {
                    _extension1 = value;
                    _hasChanged = true;
                }
            }
        }

        public string Extension2
        {
            get
            {
                Fill();
                return _extension2;
            }
            set 
            {
                Fill();
                if (value != _extension2)
                {
                    _extension2 = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? NotPossibleBumpUp
        {
            get
            {
                Fill();
                return _notpossiblebumpup;
            }
            set 
            {
                Fill();
                if (value != _notpossiblebumpup)
                {
                    _notpossiblebumpup = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCMCallRefUserID
        {
            get
            {
                Fill();
                return _fkcmcallrefuserid;
            }
            set 
            {
                Fill();
                if (value != _fkcmcallrefuserid)
                {
                    _fkcmcallrefuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? EMailRequested
        {
            get
            {
                Fill();
                return _emailrequested;
            }
            set 
            {
                Fill();
                if (value != _emailrequested)
                {
                    _emailrequested = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? CustomerCareRequested
        {
            get
            {
                Fill();
                return _customercarerequested;
            }
            set 
            {
                Fill();
                if (value != _customercarerequested)
                {
                    _customercarerequested = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsGoldenLead
        {
            get
            {
                Fill();
                return _isgoldenlead;
            }
            set 
            {
                Fill();
                if (value != _isgoldenlead)
                {
                    _isgoldenlead = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportExtra object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportExtraMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportExtra object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportExtra object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportExtraMapper.Save(this);
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
        /// Deletes an INImportExtra object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportExtra object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportExtraMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportExtra.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportextra>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<extension1>" + Extension1.ToString() + "</extension1>");
            xml.Append("<extension2>" + Extension2.ToString() + "</extension2>");
            xml.Append("<notpossiblebumpup>" + NotPossibleBumpUp.ToString() + "</notpossiblebumpup>");
            xml.Append("<fkcmcallrefuserid>" + FKCMCallRefUserID.ToString() + "</fkcmcallrefuserid>");
            xml.Append("<emailrequested>" + EMailRequested.ToString() + "</emailrequested>");
            xml.Append("<customercarerequested>" + CustomerCareRequested.ToString() + "</customercarerequested>");
            xml.Append("<isgoldenlead>" + IsGoldenLead.ToString() + "</isgoldenlead>");
            xml.Append("</inimportextra>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportExtra object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="extension1"></param>
        /// <param name="extension2"></param>
        /// <param name="notpossiblebumpup"></param>
        /// <param name="fkcmcallrefuserid"></param>
        /// <param name="emailrequested"></param>
        /// <param name="customercarerequested"></param>
        /// <param name="isgoldenlead"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, string extension1, string extension2, bool? notpossiblebumpup, long? fkcmcallrefuserid, bool? emailrequested, bool? customercarerequested, bool? isgoldenlead)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.Extension1 = extension1;
                this.Extension2 = extension2;
                this.NotPossibleBumpUp = notpossiblebumpup;
                this.FKCMCallRefUserID = fkcmcallrefuserid;
                this.EMailRequested = emailrequested;
                this.CustomerCareRequested = customercarerequested;
                this.IsGoldenLead = isgoldenlead;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportExtra's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportExtra history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportExtraMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportExtra object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportExtra object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportExtraMapper.UnDelete(this);
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
    /// A collection of the INImportExtra object.
    /// </summary>
    public partial class INImportExtraCollection : ObjectCollection<INImportExtra>
    { 
    }
    #endregion
}
