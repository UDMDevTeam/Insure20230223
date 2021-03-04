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
    public partial class SMS : ObjectBase<long>
    {
        #region Members
        private long? _fksystemid = null;
        private long? _fkimportid = null;
        private string _smsid = null;
        private long? _fklkpsmstypeid = null;
        private string _recipientcellnum = null;
        private string _smsbody = null;
        private long? _fklkpsmsencodingid = null;
        private string _submissionid = null;
        private DateTime? _submissiondate = null;
        private long? _fklkpsmsstatustypeid = null;
        private long? _fklkpsmsstatussubtypeid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the SMS class.
        /// </summary>
        public SMS()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the SMS class.
        /// </summary>
        public SMS(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKSystemID
        {
            get
            {
                Fill();
                return _fksystemid;
            }
            set 
            {
                Fill();
                if (value != _fksystemid)
                {
                    _fksystemid = value;
                    _hasChanged = true;
                }
            }
        }

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

        public string SMSID
        {
            get
            {
                Fill();
                return _smsid;
            }
            set 
            {
                Fill();
                if (value != _smsid)
                {
                    _smsid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKlkpSMSTypeID
        {
            get
            {
                Fill();
                return _fklkpsmstypeid;
            }
            set 
            {
                Fill();
                if (value != _fklkpsmstypeid)
                {
                    _fklkpsmstypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public string RecipientCellNum
        {
            get
            {
                Fill();
                return _recipientcellnum;
            }
            set 
            {
                Fill();
                if (value != _recipientcellnum)
                {
                    _recipientcellnum = value;
                    _hasChanged = true;
                }
            }
        }

        public string SMSBody
        {
            get
            {
                Fill();
                return _smsbody;
            }
            set 
            {
                Fill();
                if (value != _smsbody)
                {
                    _smsbody = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKlkpSMSEncodingID
        {
            get
            {
                Fill();
                return _fklkpsmsencodingid;
            }
            set 
            {
                Fill();
                if (value != _fklkpsmsencodingid)
                {
                    _fklkpsmsencodingid = value;
                    _hasChanged = true;
                }
            }
        }

        public string SubmissionID
        {
            get
            {
                Fill();
                return _submissionid;
            }
            set 
            {
                Fill();
                if (value != _submissionid)
                {
                    _submissionid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? SubmissionDate
        {
            get
            {
                Fill();
                return _submissiondate;
            }
            set 
            {
                Fill();
                if (value != _submissiondate)
                {
                    _submissiondate = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKlkpSMSStatusTypeID
        {
            get
            {
                Fill();
                return _fklkpsmsstatustypeid;
            }
            set 
            {
                Fill();
                if (value != _fklkpsmsstatustypeid)
                {
                    _fklkpsmsstatustypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKlkpSMSStatusSubtypeID
        {
            get
            {
                Fill();
                return _fklkpsmsstatussubtypeid;
            }
            set 
            {
                Fill();
                if (value != _fklkpsmsstatussubtypeid)
                {
                    _fklkpsmsstatussubtypeid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an SMS object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                SMSMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an SMS object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SMS object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = SMSMapper.Save(this);
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
        /// Deletes an SMS object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SMS object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && SMSMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the SMS.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<sms>");
            xml.Append("<fksystemid>" + FKSystemID.ToString() + "</fksystemid>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<smsid>" + SMSID.ToString() + "</smsid>");
            xml.Append("<fklkpsmstypeid>" + FKlkpSMSTypeID.ToString() + "</fklkpsmstypeid>");
            xml.Append("<recipientcellnum>" + RecipientCellNum.ToString() + "</recipientcellnum>");
            xml.Append("<smsbody>" + SMSBody.ToString() + "</smsbody>");
            xml.Append("<fklkpsmsencodingid>" + FKlkpSMSEncodingID.ToString() + "</fklkpsmsencodingid>");
            xml.Append("<submissionid>" + SubmissionID.ToString() + "</submissionid>");
            xml.Append("<submissiondate>" + SubmissionDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</submissiondate>");
            xml.Append("<fklkpsmsstatustypeid>" + FKlkpSMSStatusTypeID.ToString() + "</fklkpsmsstatustypeid>");
            xml.Append("<fklkpsmsstatussubtypeid>" + FKlkpSMSStatusSubtypeID.ToString() + "</fklkpsmsstatussubtypeid>");
            xml.Append("</sms>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the SMS object from a list of parameters.
        /// </summary>
        /// <param name="fksystemid"></param>
        /// <param name="fkimportid"></param>
        /// <param name="smsid"></param>
        /// <param name="fklkpsmstypeid"></param>
        /// <param name="recipientcellnum"></param>
        /// <param name="smsbody"></param>
        /// <param name="fklkpsmsencodingid"></param>
        /// <param name="submissionid"></param>
        /// <param name="submissiondate"></param>
        /// <param name="fklkpsmsstatustypeid"></param>
        /// <param name="fklkpsmsstatussubtypeid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fksystemid, long? fkimportid, string smsid, long? fklkpsmstypeid, string recipientcellnum, string smsbody, long? fklkpsmsencodingid, string submissionid, DateTime? submissiondate, long? fklkpsmsstatustypeid, long? fklkpsmsstatussubtypeid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKSystemID = fksystemid;
                this.FKImportID = fkimportid;
                this.SMSID = smsid;
                this.FKlkpSMSTypeID = fklkpsmstypeid;
                this.RecipientCellNum = recipientcellnum;
                this.SMSBody = smsbody;
                this.FKlkpSMSEncodingID = fklkpsmsencodingid;
                this.SubmissionID = submissionid;
                this.SubmissionDate = submissiondate;
                this.FKlkpSMSStatusTypeID = fklkpsmsstatustypeid;
                this.FKlkpSMSStatusSubtypeID = fklkpsmsstatussubtypeid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) SMS's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SMS history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return SMSMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an SMS object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SMS object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return SMSMapper.UnDelete(this);
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
    /// A collection of the SMS object.
    /// </summary>
    public partial class SMSCollection : ObjectCollection<SMS>
    { 
    }
    #endregion
}
