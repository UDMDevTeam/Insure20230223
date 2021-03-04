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
    public partial class SystemInfo : ObjectBase<long>
    {
        #region Members
        private long? _fksystemid = null;
        private string _systemversion = null;
        private string _computername = null;
        private string _username = null;
        private string _userdomainname = null;
        private string _simpleosname = null;
        private string _osdescription = null;
        private string _osarchitecture = null;
        private string _processarchitecture = null;
        private string _frameworkdescription = null;
        private string _ipaddresses = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the SystemInfo class.
        /// </summary>
        public SystemInfo()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the SystemInfo class.
        /// </summary>
        public SystemInfo(long id)
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

        public string SystemVersion
        {
            get
            {
                Fill();
                return _systemversion;
            }
            set 
            {
                Fill();
                if (value != _systemversion)
                {
                    _systemversion = value;
                    _hasChanged = true;
                }
            }
        }

        public string ComputerName
        {
            get
            {
                Fill();
                return _computername;
            }
            set 
            {
                Fill();
                if (value != _computername)
                {
                    _computername = value;
                    _hasChanged = true;
                }
            }
        }

        public string UserName
        {
            get
            {
                Fill();
                return _username;
            }
            set 
            {
                Fill();
                if (value != _username)
                {
                    _username = value;
                    _hasChanged = true;
                }
            }
        }

        public string UserDomainName
        {
            get
            {
                Fill();
                return _userdomainname;
            }
            set 
            {
                Fill();
                if (value != _userdomainname)
                {
                    _userdomainname = value;
                    _hasChanged = true;
                }
            }
        }

        public string SimpleOSName
        {
            get
            {
                Fill();
                return _simpleosname;
            }
            set 
            {
                Fill();
                if (value != _simpleosname)
                {
                    _simpleosname = value;
                    _hasChanged = true;
                }
            }
        }

        public string OSDescription
        {
            get
            {
                Fill();
                return _osdescription;
            }
            set 
            {
                Fill();
                if (value != _osdescription)
                {
                    _osdescription = value;
                    _hasChanged = true;
                }
            }
        }

        public string OSArchitecture
        {
            get
            {
                Fill();
                return _osarchitecture;
            }
            set 
            {
                Fill();
                if (value != _osarchitecture)
                {
                    _osarchitecture = value;
                    _hasChanged = true;
                }
            }
        }

        public string ProcessArchitecture
        {
            get
            {
                Fill();
                return _processarchitecture;
            }
            set 
            {
                Fill();
                if (value != _processarchitecture)
                {
                    _processarchitecture = value;
                    _hasChanged = true;
                }
            }
        }

        public string FrameworkDescription
        {
            get
            {
                Fill();
                return _frameworkdescription;
            }
            set 
            {
                Fill();
                if (value != _frameworkdescription)
                {
                    _frameworkdescription = value;
                    _hasChanged = true;
                }
            }
        }

        public string IPAddresses
        {
            get
            {
                Fill();
                return _ipaddresses;
            }
            set 
            {
                Fill();
                if (value != _ipaddresses)
                {
                    _ipaddresses = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an SystemInfo object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                SystemInfoMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an SystemInfo object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SystemInfo object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = SystemInfoMapper.Save(this);
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
        /// Deletes an SystemInfo object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SystemInfo object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && SystemInfoMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the SystemInfo.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<systeminfo>");
            xml.Append("<fksystemid>" + FKSystemID.ToString() + "</fksystemid>");
            xml.Append("<systemversion>" + SystemVersion.ToString() + "</systemversion>");
            xml.Append("<computername>" + ComputerName.ToString() + "</computername>");
            xml.Append("<username>" + UserName.ToString() + "</username>");
            xml.Append("<userdomainname>" + UserDomainName.ToString() + "</userdomainname>");
            xml.Append("<simpleosname>" + SimpleOSName.ToString() + "</simpleosname>");
            xml.Append("<osdescription>" + OSDescription.ToString() + "</osdescription>");
            xml.Append("<osarchitecture>" + OSArchitecture.ToString() + "</osarchitecture>");
            xml.Append("<processarchitecture>" + ProcessArchitecture.ToString() + "</processarchitecture>");
            xml.Append("<frameworkdescription>" + FrameworkDescription.ToString() + "</frameworkdescription>");
            xml.Append("<ipaddresses>" + IPAddresses.ToString() + "</ipaddresses>");
            xml.Append("</systeminfo>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the SystemInfo object from a list of parameters.
        /// </summary>
        /// <param name="fksystemid"></param>
        /// <param name="systemversion"></param>
        /// <param name="computername"></param>
        /// <param name="username"></param>
        /// <param name="userdomainname"></param>
        /// <param name="simpleosname"></param>
        /// <param name="osdescription"></param>
        /// <param name="osarchitecture"></param>
        /// <param name="processarchitecture"></param>
        /// <param name="frameworkdescription"></param>
        /// <param name="ipaddresses"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fksystemid, string systemversion, string computername, string username, string userdomainname, string simpleosname, string osdescription, string osarchitecture, string processarchitecture, string frameworkdescription, string ipaddresses)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKSystemID = fksystemid;
                this.SystemVersion = systemversion;
                this.ComputerName = computername;
                this.UserName = username;
                this.UserDomainName = userdomainname;
                this.SimpleOSName = simpleosname;
                this.OSDescription = osdescription;
                this.OSArchitecture = osarchitecture;
                this.ProcessArchitecture = processarchitecture;
                this.FrameworkDescription = frameworkdescription;
                this.IPAddresses = ipaddresses;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) SystemInfo's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SystemInfo history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return SystemInfoMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an SystemInfo object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the SystemInfo object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return SystemInfoMapper.UnDelete(this);
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
    /// A collection of the SystemInfo object.
    /// </summary>
    public partial class SystemInfoCollection : ObjectCollection<SystemInfo>
    { 
    }
    #endregion
}
