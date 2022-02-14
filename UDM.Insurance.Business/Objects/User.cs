using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;
using System.Data;
using Embriant.Framework.Data;

namespace UDM.Insurance.Business
{
    public partial class User : ObjectBase<long>
    {
        #region Members
        private string _firstname = null;
        private string _lastname = null;
        private long? _fkusertype = null;
        private string _loginname = null;
        private string _password = null;
        private DateTime? _startdate = null;
        private bool _requirespasswordchange = false;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the User class.
        /// </summary>
        public User()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the User class.
        /// </summary>
        public User(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string FirstName
        {
            get
            {
                Fill();
                return _firstname;
            }
            set 
            {
                Fill();
                if (value != _firstname)
                {
                    _firstname = value;
                    _hasChanged = true;
                }
            }
        }

        public string LastName
        {
            get
            {
                Fill();
                return _lastname;
            }
            set 
            {
                Fill();
                if (value != _lastname)
                {
                    _lastname = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKUserType
        {
            get
            {
                Fill();
                return _fkusertype;
            }
            set 
            {
                Fill();
                if (value != _fkusertype)
                {
                    _fkusertype = value;
                    _hasChanged = true;
                }
            }
        }

        public string LoginName
        {
            get
            {
                Fill();
                return _loginname;
            }
            set 
            {
                Fill();
                if (value != _loginname)
                {
                    _loginname = value;
                    _hasChanged = true;
                }
            }
        }

        public string Password
        {
            get
            {
                Fill();
                return _password;
            }
            set 
            {
                Fill();
                if (value != _password)
                {
                    _password = value;
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

        public bool RequiresPasswordChange
        {
            get
            {
                Fill();
                return _requirespasswordchange;
            }
            set 
            {
                Fill();
                if (value != _requirespasswordchange)
                {
                    _requirespasswordchange = value;
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
        /// Fills an User object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                UserMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an User object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the User object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = UserMapper.Save(this);
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
        /// Deletes an User object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the User object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && UserMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the User.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<user>");
            xml.Append("<firstname>" + FirstName.ToString() + "</firstname>");
            xml.Append("<lastname>" + LastName.ToString() + "</lastname>");
            xml.Append("<fkusertype>" + FKUserType.ToString() + "</fkusertype>");
            xml.Append("<loginname>" + LoginName.ToString() + "</loginname>");
            xml.Append("<password>" + Password.ToString() + "</password>");
            xml.Append("<startdate>" + StartDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</startdate>");
            xml.Append("<requirespasswordchange>" + RequiresPasswordChange.ToString() + "</requirespasswordchange>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</user>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the User object from a list of parameters.
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="fkusertype"></param>
        /// <param name="loginname"></param>
        /// <param name="password"></param>
        /// <param name="startdate"></param>
        /// <param name="requirespasswordchange"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string firstname, string lastname, long? fkusertype, string loginname, string password, DateTime? startdate, bool requirespasswordchange, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FirstName = firstname;
                this.LastName = lastname;
                this.FKUserType = fkusertype;
                this.LoginName = loginname;
                this.Password = password;
                this.StartDate = startdate;
                this.RequiresPasswordChange = requirespasswordchange;
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
        /// Deletes a(n) User's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the User history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return UserMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an User object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the User object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return UserMapper.UnDelete(this);
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region Version Check

        public static DataTable INGetVersionInfo(string userName)
        {

            object param1 = Database.GetParameter("@UserName", userName);

            object[] paramArray = new[] { param1 };

            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetVersionInfo", paramArray).Tables[0];
        }

        public static DataTable INGetLatestVersion()
        {
            return Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetLatestVersion", null).Tables[0];
        }


        #endregion
    }

    #region Collection
    /// <summary>
    /// A collection of the User object.
    /// </summary>
    public partial class UserCollection : ObjectCollection<User>
    { 
    }
    #endregion
}
