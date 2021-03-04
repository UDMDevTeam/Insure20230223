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
    public partial class PostalCode : ObjectBase<long>
    {
        #region Members
        private string _suburb = null;
        private string _boxcode = null;
        private string _streetcode = null;
        private string _city = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the PostalCode class.
        /// </summary>
        public PostalCode()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the PostalCode class.
        /// </summary>
        public PostalCode(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string Suburb
        {
            get
            {
                Fill();
                return _suburb;
            }
            set 
            {
                Fill();
                if (value != _suburb)
                {
                    _suburb = value;
                    _hasChanged = true;
                }
            }
        }

        public string BoxCode
        {
            get
            {
                Fill();
                return _boxcode;
            }
            set 
            {
                Fill();
                if (value != _boxcode)
                {
                    _boxcode = value;
                    _hasChanged = true;
                }
            }
        }

        public string StreetCode
        {
            get
            {
                Fill();
                return _streetcode;
            }
            set 
            {
                Fill();
                if (value != _streetcode)
                {
                    _streetcode = value;
                    _hasChanged = true;
                }
            }
        }

        public string City
        {
            get
            {
                Fill();
                return _city;
            }
            set 
            {
                Fill();
                if (value != _city)
                {
                    _city = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an PostalCode object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                PostalCodeMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an PostalCode object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PostalCode object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = PostalCodeMapper.Save(this);
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
        /// Deletes an PostalCode object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PostalCode object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && PostalCodeMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the PostalCode.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<postalcode>");
            xml.Append("<suburb>" + Suburb.ToString() + "</suburb>");
            xml.Append("<boxcode>" + BoxCode.ToString() + "</boxcode>");
            xml.Append("<streetcode>" + StreetCode.ToString() + "</streetcode>");
            xml.Append("<city>" + City.ToString() + "</city>");
            xml.Append("</postalcode>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the PostalCode object from a list of parameters.
        /// </summary>
        /// <param name="suburb"></param>
        /// <param name="boxcode"></param>
        /// <param name="streetcode"></param>
        /// <param name="city"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string suburb, string boxcode, string streetcode, string city)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Suburb = suburb;
                this.BoxCode = boxcode;
                this.StreetCode = streetcode;
                this.City = city;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) PostalCode's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PostalCode history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return PostalCodeMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an PostalCode object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the PostalCode object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return PostalCodeMapper.UnDelete(this);
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
    /// A collection of the PostalCode object.
    /// </summary>
    public partial class PostalCodeCollection : ObjectCollection<PostalCode>
    { 
    }
    #endregion
}
