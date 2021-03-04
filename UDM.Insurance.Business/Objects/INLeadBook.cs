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
    public partial class INLeadBook : ObjectBase<long>
    {
        #region Members
        private long? _fkuserid = null;
        private long? _fkinbatchid = null;
        private string _description = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INLeadBook class.
        /// </summary>
        public INLeadBook()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INLeadBook class.
        /// </summary>
        public INLeadBook(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKUserID
        {
            get
            {
                Fill();
                return _fkuserid;
            }
            set 
            {
                Fill();
                if (value != _fkuserid)
                {
                    _fkuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINBatchID
        {
            get
            {
                Fill();
                return _fkinbatchid;
            }
            set 
            {
                Fill();
                if (value != _fkinbatchid)
                {
                    _fkinbatchid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Description
        {
            get
            {
                Fill();
                return _description;
            }
            set 
            {
                Fill();
                if (value != _description)
                {
                    _description = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INLeadBook object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INLeadBookMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INLeadBook object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBook object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INLeadBookMapper.Save(this);
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
        /// Deletes an INLeadBook object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBook object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INLeadBookMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INLeadBook.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inleadbook>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<fkinbatchid>" + FKINBatchID.ToString() + "</fkinbatchid>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("</inleadbook>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INLeadBook object from a list of parameters.
        /// </summary>
        /// <param name="fkuserid"></param>
        /// <param name="fkinbatchid"></param>
        /// <param name="description"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkuserid, long? fkinbatchid, string description)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKUserID = fkuserid;
                this.FKINBatchID = fkinbatchid;
                this.Description = description;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INLeadBook's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBook history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadBookMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INLeadBook object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBook object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadBookMapper.UnDelete(this);
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
    /// A collection of the INLeadBook object.
    /// </summary>
    public partial class INLeadBookCollection : ObjectCollection<INLeadBook>
    { 
    }
    #endregion
}
