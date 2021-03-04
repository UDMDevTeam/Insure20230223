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
    public partial class INQADetailsQuestion : ObjectBase<long>
    {
        #region Members
        private string _question = null;
        private long? _fkquestiontypeid = null;
        private long? _fkanswertypeid = null;
        private long? _fkcampaigntypeid = null;
        private long? _fkcampaigngroupid = null;
        private long? _fkcampaigntypegroupid = null;
        private long? _fkcampaigngrouptypeid = null;
        private long? _fkcampaignid = null;
        private int? _weight = null;
        private int? _rank = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INQADetailsQuestion class.
        /// </summary>
        public INQADetailsQuestion()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INQADetailsQuestion class.
        /// </summary>
        public INQADetailsQuestion(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string Question
        {
            get
            {
                Fill();
                return _question;
            }
            set 
            {
                Fill();
                if (value != _question)
                {
                    _question = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKQuestionTypeID
        {
            get
            {
                Fill();
                return _fkquestiontypeid;
            }
            set 
            {
                Fill();
                if (value != _fkquestiontypeid)
                {
                    _fkquestiontypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKAnswerTypeID
        {
            get
            {
                Fill();
                return _fkanswertypeid;
            }
            set 
            {
                Fill();
                if (value != _fkanswertypeid)
                {
                    _fkanswertypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignTypeID
        {
            get
            {
                Fill();
                return _fkcampaigntypeid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigntypeid)
                {
                    _fkcampaigntypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignGroupID
        {
            get
            {
                Fill();
                return _fkcampaigngroupid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigngroupid)
                {
                    _fkcampaigngroupid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignTypeGroupID
        {
            get
            {
                Fill();
                return _fkcampaigntypegroupid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigntypegroupid)
                {
                    _fkcampaigntypegroupid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignGroupTypeID
        {
            get
            {
                Fill();
                return _fkcampaigngrouptypeid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigngrouptypeid)
                {
                    _fkcampaigngrouptypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignID
        {
            get
            {
                Fill();
                return _fkcampaignid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaignid)
                {
                    _fkcampaignid = value;
                    _hasChanged = true;
                }
            }
        }

        public int? Weight
        {
            get
            {
                Fill();
                return _weight;
            }
            set 
            {
                Fill();
                if (value != _weight)
                {
                    _weight = value;
                    _hasChanged = true;
                }
            }
        }

        public int? Rank
        {
            get
            {
                Fill();
                return _rank;
            }
            set 
            {
                Fill();
                if (value != _rank)
                {
                    _rank = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsActive
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
        /// Fills an INQADetailsQuestion object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INQADetailsQuestionMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INQADetailsQuestion object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestion object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INQADetailsQuestionMapper.Save(this);
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
        /// Deletes an INQADetailsQuestion object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestion object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INQADetailsQuestionMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INQADetailsQuestion.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inqadetailsquestion>");
            xml.Append("<question>" + Question.ToString() + "</question>");
            xml.Append("<fkquestiontypeid>" + FKQuestionTypeID.ToString() + "</fkquestiontypeid>");
            xml.Append("<fkanswertypeid>" + FKAnswerTypeID.ToString() + "</fkanswertypeid>");
            xml.Append("<fkcampaigntypeid>" + FKCampaignTypeID.ToString() + "</fkcampaigntypeid>");
            xml.Append("<fkcampaigngroupid>" + FKCampaignGroupID.ToString() + "</fkcampaigngroupid>");
            xml.Append("<fkcampaigntypegroupid>" + FKCampaignTypeGroupID.ToString() + "</fkcampaigntypegroupid>");
            xml.Append("<fkcampaigngrouptypeid>" + FKCampaignGroupTypeID.ToString() + "</fkcampaigngrouptypeid>");
            xml.Append("<fkcampaignid>" + FKCampaignID.ToString() + "</fkcampaignid>");
            xml.Append("<weight>" + Weight.ToString() + "</weight>");
            xml.Append("<rank>" + Rank.ToString() + "</rank>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</inqadetailsquestion>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INQADetailsQuestion object from a list of parameters.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="fkquestiontypeid"></param>
        /// <param name="fkanswertypeid"></param>
        /// <param name="fkcampaigntypeid"></param>
        /// <param name="fkcampaigngroupid"></param>
        /// <param name="fkcampaigntypegroupid"></param>
        /// <param name="fkcampaigngrouptypeid"></param>
        /// <param name="fkcampaignid"></param>
        /// <param name="weight"></param>
        /// <param name="rank"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string question, long? fkquestiontypeid, long? fkanswertypeid, long? fkcampaigntypeid, long? fkcampaigngroupid, long? fkcampaigntypegroupid, long? fkcampaigngrouptypeid, long? fkcampaignid, int? weight, int? rank, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Question = question;
                this.FKQuestionTypeID = fkquestiontypeid;
                this.FKAnswerTypeID = fkanswertypeid;
                this.FKCampaignTypeID = fkcampaigntypeid;
                this.FKCampaignGroupID = fkcampaigngroupid;
                this.FKCampaignTypeGroupID = fkcampaigntypegroupid;
                this.FKCampaignGroupTypeID = fkcampaigngrouptypeid;
                this.FKCampaignID = fkcampaignid;
                this.Weight = weight;
                this.Rank = rank;
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
        /// Deletes a(n) INQADetailsQuestion's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestion history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsQuestionMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INQADetailsQuestion object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestion object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsQuestionMapper.UnDelete(this);
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
    /// A collection of the INQADetailsQuestion object.
    /// </summary>
    public partial class INQADetailsQuestionCollection : ObjectCollection<INQADetailsQuestion>
    { 
    }
    #endregion
}
