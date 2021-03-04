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
    public partial class INQADetailsQuestionINImport : ObjectBase<long>
    {
        #region Members
        private long? _fkimportid = null;
        private long? _fkquestionid = null;
        private long? _answerint = null;
        private DateTime? _answerdatetime = null;
        private string _answertext = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INQADetailsQuestionINImport class.
        /// </summary>
        public INQADetailsQuestionINImport()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INQADetailsQuestionINImport class.
        /// </summary>
        public INQADetailsQuestionINImport(long id)
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

        public long? FKQuestionID
        {
            get
            {
                Fill();
                return _fkquestionid;
            }
            set 
            {
                Fill();
                if (value != _fkquestionid)
                {
                    _fkquestionid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? AnswerInt
        {
            get
            {
                Fill();
                return _answerint;
            }
            set 
            {
                Fill();
                if (value != _answerint)
                {
                    _answerint = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? AnswerDateTime
        {
            get
            {
                Fill();
                return _answerdatetime;
            }
            set 
            {
                Fill();
                if (value != _answerdatetime)
                {
                    _answerdatetime = value;
                    _hasChanged = true;
                }
            }
        }

        public string AnswerText
        {
            get
            {
                Fill();
                return _answertext;
            }
            set 
            {
                Fill();
                if (value != _answertext)
                {
                    _answertext = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INQADetailsQuestionINImport object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INQADetailsQuestionINImportMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INQADetailsQuestionINImport object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionINImport object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INQADetailsQuestionINImportMapper.Save(this);
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
        /// Deletes an INQADetailsQuestionINImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionINImport object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INQADetailsQuestionINImportMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INQADetailsQuestionINImport.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inqadetailsquestioninimport>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<fkquestionid>" + FKQuestionID.ToString() + "</fkquestionid>");
            xml.Append("<answerint>" + AnswerInt.ToString() + "</answerint>");
            xml.Append("<answerdatetime>" + AnswerDateTime.Value.ToString("dd MMM yyyy HH:mm:ss") + "</answerdatetime>");
            xml.Append("<answertext>" + AnswerText.ToString() + "</answertext>");
            xml.Append("</inqadetailsquestioninimport>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INQADetailsQuestionINImport object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="fkquestionid"></param>
        /// <param name="answerint"></param>
        /// <param name="answerdatetime"></param>
        /// <param name="answertext"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkimportid, long? fkquestionid, long? answerint, DateTime? answerdatetime, string answertext)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKImportID = fkimportid;
                this.FKQuestionID = fkquestionid;
                this.AnswerInt = answerint;
                this.AnswerDateTime = answerdatetime;
                this.AnswerText = answertext;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INQADetailsQuestionINImport's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionINImport history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsQuestionINImportMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INQADetailsQuestionINImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionINImport object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsQuestionINImportMapper.UnDelete(this);
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
    /// A collection of the INQADetailsQuestionINImport object.
    /// </summary>
    public partial class INQADetailsQuestionINImportCollection : ObjectCollection<INQADetailsQuestionINImport>
    { 
    }
    #endregion
}
