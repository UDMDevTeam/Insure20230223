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
    public partial class INPolicyOption : ObjectBase<long>
    {
        #region Members
        private long? _fkpolicyid = null;
        private long? _fkoptionid = null;
        private Byte? _isla2selected = null;
        private Byte? _ischildselected = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INPolicyOption class.
        /// </summary>
        public INPolicyOption()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INPolicyOption class.
        /// </summary>
        public INPolicyOption(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKPolicyID
        {
            get
            {
                Fill();
                return _fkpolicyid;
            }
            set 
            {
                Fill();
                if (value != _fkpolicyid)
                {
                    _fkpolicyid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKOptionID
        {
            get
            {
                Fill();
                return _fkoptionid;
            }
            set 
            {
                Fill();
                if (value != _fkoptionid)
                {
                    _fkoptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public Byte? IsLA2Selected
        {
            get
            {
                Fill();
                return _isla2selected;
            }
            set 
            {
                Fill();
                if (value != _isla2selected)
                {
                    _isla2selected = value;
                    _hasChanged = true;
                }
            }
        }

        public Byte? IsChildSelected
        {
            get
            {
                Fill();
                return _ischildselected;
            }
            set 
            {
                Fill();
                if (value != _ischildselected)
                {
                    _ischildselected = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INPolicyOption object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPolicyOptionMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INPolicyOption object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyOption object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPolicyOptionMapper.Save(this);
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
        /// Deletes an INPolicyOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyOption object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPolicyOptionMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INPolicyOption.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inpolicyoption>");
            xml.Append("<fkpolicyid>" + FKPolicyID.ToString() + "</fkpolicyid>");
            xml.Append("<fkoptionid>" + FKOptionID.ToString() + "</fkoptionid>");
            xml.Append("<isla2selected>" + IsLA2Selected.ToString() + "</isla2selected>");
            xml.Append("<ischildselected>" + IsChildSelected.ToString() + "</ischildselected>");
            xml.Append("</inpolicyoption>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INPolicyOption object from a list of parameters.
        /// </summary>
        /// <param name="fkpolicyid"></param>
        /// <param name="fkoptionid"></param>
        /// <param name="isla2selected"></param>
        /// <param name="ischildselected"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkpolicyid, long? fkoptionid, Byte? isla2selected, Byte? ischildselected)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKPolicyID = fkpolicyid;
                this.FKOptionID = fkoptionid;
                this.IsLA2Selected = isla2selected;
                this.IsChildSelected = ischildselected;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INPolicyOption's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyOption history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyOptionMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INPolicyOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyOption object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyOptionMapper.UnDelete(this);
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
    /// A collection of the INPolicyOption object.
    /// </summary>
    public partial class INPolicyOptionCollection : ObjectCollection<INPolicyOption>
    { 
    }
    #endregion
}
