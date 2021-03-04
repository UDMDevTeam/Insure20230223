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
    public partial class INUserTypeLeadStatus : ObjectBase<long>
    {
        #region Members
        private long _fkusertypeid = 0;
        private long _fkleadstatusid = 0;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INUserTypeLeadStatus class.
        /// </summary>
        public INUserTypeLeadStatus()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INUserTypeLeadStatus class.
        /// </summary>
        public INUserTypeLeadStatus(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long FKUserTypeID
        {
            get
            {
                Fill();
                return _fkusertypeid;
            }
            set 
            {
                Fill();
                if (value != _fkusertypeid)
                {
                    _fkusertypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long FKLeadStatusID
        {
            get
            {
                Fill();
                return _fkleadstatusid;
            }
            set 
            {
                Fill();
                if (value != _fkleadstatusid)
                {
                    _fkleadstatusid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INUserTypeLeadStatus object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INUserTypeLeadStatusMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INUserTypeLeadStatus object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserTypeLeadStatus object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INUserTypeLeadStatusMapper.Save(this);
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
        /// Deletes an INUserTypeLeadStatus object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserTypeLeadStatus object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INUserTypeLeadStatusMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INUserTypeLeadStatus.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inusertypeleadstatus>");
            xml.Append("<fkusertypeid>" + FKUserTypeID.ToString() + "</fkusertypeid>");
            xml.Append("<fkleadstatusid>" + FKLeadStatusID.ToString() + "</fkleadstatusid>");
            xml.Append("</inusertypeleadstatus>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INUserTypeLeadStatus object from a list of parameters.
        /// </summary>
        /// <param name="fkusertypeid"></param>
        /// <param name="fkleadstatusid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long fkusertypeid, long fkleadstatusid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKUserTypeID = fkusertypeid;
                this.FKLeadStatusID = fkleadstatusid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INUserTypeLeadStatus's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserTypeLeadStatus history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INUserTypeLeadStatusMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INUserTypeLeadStatus object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserTypeLeadStatus object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INUserTypeLeadStatusMapper.UnDelete(this);
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
    /// A collection of the INUserTypeLeadStatus object.
    /// </summary>
    public partial class INUserTypeLeadStatusCollection : ObjectCollection<INUserTypeLeadStatus>
    { 
    }
    #endregion
}
