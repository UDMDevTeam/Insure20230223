using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business.Objects
{
    public partial class CancerQuestion : ObjectBase<long>
    {
        #region Members
        private bool? _questionone = null;
        private bool? _questiontwo = null;
        private long? _fkinimportid = null;
        private long? _fkincampaignid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INChild class.
        /// </summary>
        public CancerQuestion()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INChild class.
        /// </summary>
        public CancerQuestion(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public bool? QuestionOne
        {
            get
            {
                Fill();
                return _questionone;
            }
            set
            {
                Fill();
                if (value != _questionone)
                {
                    _questionone = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? QuestionTwo
        {
            get
            {
                Fill();
                return _questiontwo;
            }
            set
            {
                Fill();
                if (value != _questiontwo)
                {
                    _questiontwo = value;
                    _hasChanged = true;
                }
            }
        }


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



        public long? FKINCampaignID
        {
            get
            {
                Fill();
                return _fkincampaignid;
            }
            set
            {
                Fill();
                if (value != _fkincampaignid)
                {
                    _fkincampaignid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an CancerQuestion object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                CancerQuestionMapper.Fill(this);
            }
        }


        /// <summary>
        /// Saves an CancerQuestion object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CancerQuestion object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = CancerQuestionMapper.Save(this);
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
        /// Deletes an CancerQuestion object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CancerQuestion object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && CancerQuestionMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the CancerQuestion.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<cancerquestion>");
            xml.Append("<questionone>" + QuestionOne.ToString() + "</questionone>");
            xml.Append("<questiontwo>" + QuestionTwo.Value.ToString() + "</questiontwo>");
            xml.Append("<fkinimportid>" + FKINImportID.Value.ToString() + "</fkinimportid>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("</cancerquestion>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the CancerQuestion object from a list of parameters.
        /// </summary>
        /// <param name="questionone"></param>
        /// <param name="questiontwo"></param>
        /// <param name="fkinimportid"></param>
        /// <param name="fkincampaignid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(bool? questionone, bool? questiontwo, long? fkinimportid, long? fkincampaignid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.QuestionOne = questionone;
                this.QuestionTwo = questiontwo;
                this.FKINImportID = fkinimportid;
                this.FKINCampaignID = fkincampaignid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) CancerQuestion's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CancerQuestion history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return CancerQuestionMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an CancerQuestion object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CancerQuestion object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return CancerQuestionMapper.UnDelete(this);
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region Collection
        /// <summary>
        /// A collection of the CancerQuestion object.
        /// </summary>
        public partial class CancerQuestionCollection : ObjectCollection<CancerQuestion>
        {
        }
        #endregion
    }



}


