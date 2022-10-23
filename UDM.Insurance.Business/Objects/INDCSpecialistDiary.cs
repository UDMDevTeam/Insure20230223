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
    public partial class INDCSpecialistDiary : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fkuserid = null;
        private DateTime? _startdate = null;
        private DateTime? _enddate = null;
        private TimeSpan? _time = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public INDCSpecialistDiary()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public INDCSpecialistDiary(long id)
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

        public DateTime? StartDate
        {
            get
            {
                Fill();
                return _startdate;
            }
            set 
            {
                Fill();
                if (value != _startdate)
                {
                    _startdate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? EndDate
        {
            get
            {
                Fill();
                return _enddate;
            }
            set 
            {
                Fill();
                if (value != _enddate)
                {
                    _enddate = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? Time
        {
            get
            {
                Fill();
                return _time;
            }
            set
            {
                Fill();
                if (value != _time)
                {
                    _time = value;
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
                INDCSpecialistDiaryMapper.Fill(this);
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
                    _isLoaded = INDCSpecialistDiaryMapper.Save(this);
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
                return BeforeDelete(validationResult) && INDCSpecialistDiaryMapper.Delete(this);
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
            xml.Append("<INDCSpecialistDiary>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<startdate>" + StartDate.ToString() + "</startdate>");
            xml.Append("<enddate>" + EndDate.ToString() + "</enddate>");
            xml.Append("<time>" + Time.ToString() + "</time>");
            xml.Append("</INDCSpecialistDiary>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the CallData object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="number"></param>
        /// <param name="extension"></param>
        /// <param name="recref"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fkuserid, DateTime? startdate, DateTime? enddate)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKUserID = fkuserid;
                this.StartDate = startdate;
                this.EndDate = enddate;
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
                return INDCSpecialistDiaryMapper.DeleteHistory(this);
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
                return INDCSpecialistDiaryMapper.UnDelete(this);
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
    public partial class INDCSpecialistDiaryCollection : ObjectCollection<INDCSpecialistDiary>
    { 
    }
    #endregion
}
