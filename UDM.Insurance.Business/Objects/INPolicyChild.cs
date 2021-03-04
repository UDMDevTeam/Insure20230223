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
    public partial class INPolicyChild : ObjectBase<long>
    {
        #region Members
        private long? _fkinpolicyid = null;
        private long? _fkinchildid = null;
        private int? _childrank = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INPolicyChild class.
        /// </summary>
        public INPolicyChild()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INPolicyChild class.
        /// </summary>
        public INPolicyChild(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINPolicyID
        {
            get
            {
                Fill();
                return _fkinpolicyid;
            }
            set 
            {
                Fill();
                if (value != _fkinpolicyid)
                {
                    _fkinpolicyid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINChildID
        {
            get
            {
                Fill();
                return _fkinchildid;
            }
            set 
            {
                Fill();
                if (value != _fkinchildid)
                {
                    _fkinchildid = value;
                    _hasChanged = true;
                }
            }
        }

        public int? ChildRank
        {
            get
            {
                Fill();
                return _childrank;
            }
            set 
            {
                Fill();
                if (value != _childrank)
                {
                    _childrank = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INPolicyChild object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPolicyChildMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INPolicyChild object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyChild object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPolicyChildMapper.Save(this);
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
        /// Deletes an INPolicyChild object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyChild object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPolicyChildMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INPolicyChild.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inpolicychild>");
            xml.Append("<fkinpolicyid>" + FKINPolicyID.ToString() + "</fkinpolicyid>");
            xml.Append("<fkinchildid>" + FKINChildID.ToString() + "</fkinchildid>");
            xml.Append("<childrank>" + ChildRank.ToString() + "</childrank>");
            xml.Append("</inpolicychild>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INPolicyChild object from a list of parameters.
        /// </summary>
        /// <param name="fkinpolicyid"></param>
        /// <param name="fkinchildid"></param>
        /// <param name="childrank"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinpolicyid, long? fkinchildid, int? childrank)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINPolicyID = fkinpolicyid;
                this.FKINChildID = fkinchildid;
                this.ChildRank = childrank;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INPolicyChild's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyChild history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyChildMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INPolicyChild object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPolicyChild object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPolicyChildMapper.UnDelete(this);
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
    /// A collection of the INPolicyChild object.
    /// </summary>
    public partial class INPolicyChildCollection : ObjectCollection<INPolicyChild>
    { 
    }
    #endregion
}
