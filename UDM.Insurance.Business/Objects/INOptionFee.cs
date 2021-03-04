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
    public partial class INOptionFee : ObjectBase<long>
    {
        #region Members
        private long _fkinoptionid = 0;
        private long _fkinfeeid = 0;
        private DateTime _date = GlobalSettings.EmptyDate;
        private bool _isactive = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INOptionFee class.
        /// </summary>
        public INOptionFee()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INOptionFee class.
        /// </summary>
        public INOptionFee(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long FKINOptionID
        {
            get
            {
                Fill();
                return _fkinoptionid;
            }
            set 
            {
                Fill();
                if (value != _fkinoptionid)
                {
                    _fkinoptionid = value;
                    _hasChanged = true;
                }
            }
        }

        public long FKINFeeID
        {
            get
            {
                Fill();
                return _fkinfeeid;
            }
            set 
            {
                Fill();
                if (value != _fkinfeeid)
                {
                    _fkinfeeid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime Date
        {
            get
            {
                Fill();
                return _date;
            }
            set 
            {
                Fill();
                if (value != _date)
                {
                    _date = value;
                    _hasChanged = true;
                }
            }
        }

        public bool IsActive
        {
            get
            {
                Fill();
                return _isactive;
            }
            set 
            {
                Fill();
                if (value != _isactive)
                {
                    _isactive = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INOptionFee object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INOptionFeeMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INOptionFee object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionFee object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INOptionFeeMapper.Save(this);
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
        /// Deletes an INOptionFee object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionFee object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INOptionFeeMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INOptionFee.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inoptionfee>");
            xml.Append("<fkinoptionid>" + FKINOptionID.ToString() + "</fkinoptionid>");
            xml.Append("<fkinfeeid>" + FKINFeeID.ToString() + "</fkinfeeid>");
            xml.Append("<date>" + Date.ToString("dd MMM yyyy HH:mm:ss") + "</date>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</inoptionfee>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INOptionFee object from a list of parameters.
        /// </summary>
        /// <param name="fkinoptionid"></param>
        /// <param name="fkinfeeid"></param>
        /// <param name="date"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long fkinoptionid, long fkinfeeid, DateTime date, bool isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINOptionID = fkinoptionid;
                this.FKINFeeID = fkinfeeid;
                this.Date = date;
                this.IsActive = isactive;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INOptionFee's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionFee history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INOptionFeeMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INOptionFee object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INOptionFee object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INOptionFeeMapper.UnDelete(this);
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
    /// A collection of the INOptionFee object.
    /// </summary>
    public partial class INOptionFeeCollection : ObjectCollection<INOptionFee>
    { 
    }
    #endregion
}
