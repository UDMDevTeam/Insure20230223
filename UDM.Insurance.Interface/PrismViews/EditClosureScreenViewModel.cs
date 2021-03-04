using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using UDM.Insurance.Interface.PrismInfrastructure;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UDM.WPF.Library;
using System;
using System.Linq;
using Xceed.Wpf.Toolkit;
using Prism.Commands;
using Embriant.WPF.Controls;
using Embriant.Framework;
using Embriant.Framework.Validation;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using System.Windows.Documents;

namespace UDM.Insurance.Interface.PrismViews
{
    public class EditClosureScreenViewModel : BindableBase
    {

        #region Classes

        public class Language
        {
            private long iD;
            public long ID { get => iD; set => iD = value; }

            private string description;
            public string Description { get => description; set => description = value; }
        }

        public class Campaign
        {
            private long iD;
            public long ID { get => iD; set => iD = value; }

            private string name;
            public string Name { get => name; set => name = value; }
        }

        #endregion



        #region Properties

        private IRegionManager _localRegionManager;
        public IRegionManager LocalRegionManager
        {
            get { return _localRegionManager; }
            set { SetProperty(ref _localRegionManager, value); }
        }

        private List<Language> _languages = new List<Language>();
        public List<Language> Languages
        {
            get { return _languages; }
            set { SetProperty(ref _languages, value); }
        }

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                SetProperty(ref _selectedLanguage, value);
                OpenDocCommand.RaiseCanExecuteChanged();
                SaveDocCommand.RaiseCanExecuteChanged();
                
                ClosureText = null;
            }
        }

        private List<Campaign> _campaigns = new List<Campaign>();
        public List<Campaign> Campaigns
        {
            get { return _campaigns; }
            set { SetProperty(ref _campaigns, value); }
        }

        private Campaign _selectedCampaign;
        public Campaign SelectedCampaign
        {
            get { return _selectedCampaign; }
            set
            {
                SetProperty(ref _selectedCampaign, value);
                OpenDocCommand.RaiseCanExecuteChanged();
                SaveDocCommand.RaiseCanExecuteChanged();

                ClosureText = null;
            }
        }

        private long _closureID;
        public long ClosureID
        {
            get
            {
                return _closureID;
            }

            set
            {
                SetProperty(ref _closureID, value);
            }
        }

        private string _closureText;
        public string ClosureText
        {
            get { return _closureText; }
            set
            {
                SetProperty(ref _closureText, value);
                SaveDocCommand.RaiseCanExecuteChanged();
                CloseDocCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isNewClosure;
        public bool IsNewClosure
        {
            get
            {
                return _isNewClosure;
            }

            set
            {
                SetProperty(ref _isNewClosure, value);
            }
        }

        #endregion



        #region Members

        IContainerExtension _container;
        IRegionManager _globalRegionManager;
        IEventAggregator _ea;

        IRegion _contentRegion;
        IRegion _toolbarRegion;

        EditClosureScreenContentView _editClosureScreenContentView;
        EditClosureScreenToolbarView _editClosureScreenToolbarView;

        #endregion



        #region Constructors

        public EditClosureScreenViewModel(IContainerExtension container, IRegionManager regionManager, IEventAggregator ea)
        {
            _ea = ea;
            _container = container;
            _globalRegionManager = regionManager;

            CloseCommand = new DelegateCommand(CloseCommandExecute, CloseCommandCanExecute);
            OpenDocCommand = new DelegateCommand(OpenDocCommandExecute, OpenDocCommandCanExecute);
            CloseDocCommand = new DelegateCommand(CloseDocCommandExecute, CloseDocCommandCanExecute);
            SaveDocCommand = new DelegateCommand(SaveDocCommandExecute, SaveDocCommandCanExecute);
        }

        #endregion



        #region Commands

        public DelegateCommand CloseCommand { get; private set; }
        private bool CloseCommandCanExecute()
        {
            return true;
        }
        private void CloseCommandExecute()
        {
            _ea.GetEvent<CloseDialogEvent>().Publish();
        }

        public DelegateCommand OpenDocCommand { get; private set; }
        private bool OpenDocCommandCanExecute()
        {
            return SelectedCampaign != null && SelectedLanguage != null;
        }
        private void OpenDocCommandExecute()
        {
            try
            {
                IsNewClosure = false;

                Closure closure = ClosureMapper.SearchOne(2, SelectedCampaign.ID, SelectedLanguage.ID, null, null, null);

                if (closure != null)
                {
                    ClosureText = Encoding.UTF8.GetString(closure.Document);
                    ClosureID = closure.ID;
                }
                else
                {
                    DialogMessage dm = new DialogMessage
                    {
                        Message = "No Closure for the selected Campaign and Language.\n\nCreate a new Closure.",
                        Title = "Open Closure",
                        Type = ShowMessageType.Exclamation
                    };
                    _ea.GetEvent<SendDialogMessageEvent>().Publish(dm);

                    ClosureText = "New";
                    IsNewClosure = true;
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        public DelegateCommand CloseDocCommand { get; private set; }
        private bool CloseDocCommandCanExecute()
        {
            return ClosureText != null;
        }
        private void CloseDocCommandExecute()
        {
            try
            {
                IsNewClosure = false;
                ClosureText = null;
                _ea.GetEvent<CloseDocumentEvent>().Publish();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        public DelegateCommand SaveDocCommand { get; private set; }
        private bool SaveDocCommandCanExecute()
        {
            return SelectedCampaign != null && SelectedLanguage != null && ClosureText != null;
        }
        private void SaveDocCommandExecute()
        {
            try
            {
                if (IsNewClosure)
                {
                    Closure closure = new Closure();

                    closure.FKSystemID = 2;
                    closure.FKCampaignID = SelectedCampaign.ID;
                    closure.FKLanguageID = SelectedLanguage.ID;
                    closure.Document = Encoding.UTF8.GetBytes(ClosureText);
                    closure.IsActive = true;

                    closure.Save(new ValidationResult());
                }
                else
                {
                    Closure closure = new Closure(ClosureID);
                    closure.Document = Encoding.UTF8.GetBytes(ClosureText);

                    closure.Save(new ValidationResult());
                }

                DialogMessage dm = new DialogMessage
                {
                    Message = "Closure saved to Insure database.",
                    Title = "Save Closure",
                    Type = ShowMessageType.Information
                };
                _ea.GetEvent<SendDialogMessageEvent>().Publish(dm);

                ClosureText = null;
                IsNewClosure = false;
            }

            catch (Exception ex)
            {
                new BaseControl().HandleException(ex);
            }
        }

        #endregion



        #region Methods

        public void Initialize()
        {
            _toolbarRegion = LocalRegionManager.Regions["ToolbarRegionA"];
            _editClosureScreenToolbarView = _container.Resolve<EditClosureScreenToolbarView>();
            _toolbarRegion.Add(_editClosureScreenToolbarView);

            _contentRegion = LocalRegionManager.Regions["ContentRegionA"];
            _editClosureScreenContentView = _container.Resolve<EditClosureScreenContentView>();
            _contentRegion.Add(_editClosureScreenContentView);

            LoadLookupData();
        }

        public void LoadLookupData()
        {
            Languages.Add(new Language { ID = 1, Description = "Afrikaans" });
            Languages.Add(new Language { ID = 2, Description = "English" });

            StringBuilder strQuery = new StringBuilder();
            strQuery.Append("SELECT ID, Name FROM INCampaign WHERE IsActive = '1'");
            DataTable dt = Methods.GetTableData(strQuery.ToString(), IsolationLevel.ReadUncommitted);

            Campaigns = (from row in dt.AsEnumerable()
                         select new Campaign()
                         {
                             ID = Convert.ToInt32(row["ID"]),
                             Name = Convert.ToString(row["Name"])
                         }).ToList();

            Campaigns = Campaigns.OrderBy(o => o.Name).ToList();
        }

        #endregion

    }
}
