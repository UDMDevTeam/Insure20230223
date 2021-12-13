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
    public partial class DebiCheckConfiguration : ObjectBase<long>
    {
        #region Members
        private int? _debicheckpower = null;
        private int? _isbusyreports = null;
        private long? _busyby = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public DebiCheckConfiguration()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public DebiCheckConfiguration(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public int? DebiCheckPower
        {
            get
            {
                Fill();
                return _debicheckpower;
            }
            set 
            {
                Fill();
                if (value != _debicheckpower)
                {
                    _debicheckpower = value;
                    _hasChanged = true;
                }
            }
        }

        public int? IsBusyReports
        {
            get
            {
                Fill();
                return _isbusyreports;
            }
            set
            {
                Fill();
                if (value != _isbusyreports)
                {
                    _isbusyreports = value;
                    _hasChanged = true;
                }
            }
        }

        public long? BusyBy
        {
            get
            {
                Fill();
                return _busyby;
            }
            set
            {
                Fill();
                if (value != _busyby)
                {
                    _busyby = value;
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
                DebiCheckConfigurationMapper.Fill(this);
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
                    _isLoaded = DebiCheckConfigurationMapper.Save(this);
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
                return BeforeDelete(validationResult) && DebiCheckConfigurationMapper.Delete(this);
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
            xml.Append("<DebiCheckConfiguration>");
            xml.Append("<debicheckpower>" + DebiCheckPower.ToString() + "</debicheckpower>");
            xml.Append("<isbusyreports>" + IsBusyReports.ToString() + "</isbusyreports>");
            xml.Append("<busyby>" + BusyBy.ToString() + "</busyby>");
            xml.Append("</DebiCheckConfiguration>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the CallData object from a list of parameters.
        /// </summary>
        /// <param name="debicheckpower"></param>

        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(int? debicheckpower, int? isbusyreports, long? busyby)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.DebiCheckPower = debicheckpower;
                this.IsBusyReports = isbusyreports;
                this.BusyBy = busyby;

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
                return DebiCheckConfigurationMapper.DeleteHistory(this);
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
                return DebiCheckConfigurationMapper.UnDelete(this);
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
    public partial class DebiCheckConfigurationCollection : ObjectCollection<DebiCheckConfiguration>
    { 
    }
    #endregion
}
