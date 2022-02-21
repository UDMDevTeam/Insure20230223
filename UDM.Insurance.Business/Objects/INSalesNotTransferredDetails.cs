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
    public partial class INSalesNotTransferredDetails : ObjectBase<long>
    {
        #region Members
        private long? _fkimportid = null;
        private string _fksalesnottransferredreason = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public INSalesNotTransferredDetails()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public INSalesNotTransferredDetails(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKImportID
        {
            get
            {
                Fill();
                return _fkimportid;
            }
            set 
            {
                Fill();
                if (value != _fkimportid)
                {
                    _fkimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public string FKSalesNotTransferredReason
        {
            get
            {
                Fill();
                return _fksalesnottransferredreason;
            }
            set 
            {
                Fill();
                if (value != _fksalesnottransferredreason)
                {
                    _fksalesnottransferredreason = value;
                    _hasChanged = true;
                }
            }
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an CallData object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INSalesNotTransferredDetailsMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an CallData object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INSalesNotTransferredDetailsMapper.Save(this);
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
        /// Deletes an CallData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INSalesNotTransferredDetailsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the CallData.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<insalesnottransferreddetails>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<fksalesnottransferredreason>" + FKSalesNotTransferredReason.ToString() + "</number>");
            xml.Append("</insalesnottransferreddetails>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the CallData object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="fksalesnottransferredreason"></param>

        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkimportid, string fksalesnotransferredreason)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKImportID = fkimportid;
                this.FKSalesNotTransferredReason = fksalesnotransferredreason ;

            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) CallData's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INSalesNotTransferredDetailsMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an CallData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INSalesNotTransferredDetailsMapper.UnDelete(this);
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
    /// A collection of the CallData object.
    /// </summary>
    public partial class INSalesNotTransferredDetailsCollection : ObjectCollection<INSalesNotTransferredDetails>
    { 
    }
    #endregion
}
