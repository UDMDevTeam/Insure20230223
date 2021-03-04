using Embriant.Framework;
using Embriant.WPF.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.PrismInfrastructure;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    class QADetailsScreenViewModel : BindableBase
    {

        public static decimal introTotalCount = 0;
        public static decimal pitchTotalCoun = 0;
        public static decimal adminTotalCoun = 0;
        public static decimal partnerTotalCoun = 0;
        public static decimal closureTotalCoun = 0;

        public static decimal introSelectedCount = 0;
        public static decimal pitchSelectedlCoun = 0;
        public static decimal adminSelectedCoun = 0;
        public static decimal partnerSelectedCoun = 0;
        public static decimal closureSelectedCoun = 0;

        public static decimal OverallPercentage = 0;

        #region Classes

        public class QAQuestion
        {
            public long ID { get; set; }
            public string Question { get; set; }
            public bool Answer { get; set; }
        }

        public class LookupType
        {
            private long iD;
            public long ID { get => iD; set => iD = value; }

            private string name;
            public string Name { get => name; set => name = value; }
        }

        #endregion



        #region Properties

        public string _overallScore = " ";
        public string OverallScore
        {
            get { return _overallScore; }
            set { SetProperty(ref _overallScore, value); }
        }

        public string _title = "QA Details";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool _isSaving = false;
        public bool IsSaving
        {
            get { return _isSaving; }
            set { SetProperty(ref _isSaving, value); }
        }

        public List<QAQuestion> _callIntroQuestions = new List<QAQuestion>();
        public List<QAQuestion> CallIntroQuestions
        {
            get { return _callIntroQuestions; }
            set { SetProperty(ref _callIntroQuestions, value); }
        }

        public List<QAQuestion> _pitchQuestions = new List<QAQuestion>();
        public List<QAQuestion> PitchQuestions
        {
            get { return _pitchQuestions; }
            set { SetProperty(ref _pitchQuestions, value); }
        }

        public List<QAQuestion> _borderlineQuestions = new List<QAQuestion>();
        public List<QAQuestion> BorderlineQuestions
        {
            get { return _borderlineQuestions; }
            set { SetProperty(ref _borderlineQuestions, value); }
        }

        public List<QAQuestion> _adminQuestions = new List<QAQuestion>();
        public List<QAQuestion> AdminQuestions
        {
            get { return _adminQuestions; }
            set { SetProperty(ref _adminQuestions, value); }
        }

        public List<QAQuestion> _partnerQuestions = new List<QAQuestion>();
        public List<QAQuestion> PartnerQuestions
        {
            get { return _partnerQuestions; }
            set { SetProperty(ref _partnerQuestions, value); }
        }

        public List<QAQuestion> _closureQuestions = new List<QAQuestion>();
        public List<QAQuestion> ClosureQuestions
        {
            get { return _closureQuestions; }
            set { SetProperty(ref _closureQuestions, value); }
        }



        public List<LookupType> _qas = new List<LookupType>();
        public List<LookupType> QAs
        {
            get { return _qas; }
            set { SetProperty(ref _qas, value); }
        }

        public LookupType _selectedQA;
        public LookupType SelectedQA
        {
            get { return _selectedQA; }
            set 
            { 
                SetProperty(ref _selectedQA, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion



        #region Members

        public IEventAggregator _ea = new EventAggregator();

        private long _importID;

        #endregion



        #region Commands

        public DelegateCommand SaveCommand { get; private set; }
        private bool SaveCommandCanExecute()
        {
            return true;
        }

        private void SaveCommandExecute()
        {
            try
            {
                IsSaving = false;

                // This is a wonderful line of code which forces the UI to update before continuing
                Application.Current?.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.Render, null);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Save;
                worker.RunWorkerCompleted += SaveCompleted;
                worker.RunWorkerAsync();

                //Message
                DialogMessage dm = new DialogMessage
                {
                    Message = $"QA Assessment details saved successfully.",
                    Title = "QA Details Saved",
                    Type = ShowMessageType.Information
                };
                
                _ea.GetEvent<SendDialogMessageEvent>().Publish(dm);
            }

            catch (Exception ex)
            {
                new BaseControl().HandleException(ex);
            }
        }

        #endregion



        #region Constructors

        public QADetailsScreenViewModel(long importID)
        {
            _importID = importID;

            SaveCommand = new DelegateCommand(SaveCommandExecute, SaveCommandCanExecute);

            LoadLookupData();
        }

        public QADetailsScreenViewModel()
        {
           // parameterless constructor needed to keep designtime viewmodel access available in xaml
           // don't call this constructor though
        }

        #endregion



        #region Methods

        public void LoadLookupData()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", _importID);
                DataSet dsQADetails = Methods.ExecuteStoredProcedure2("spINGetQADetailsScreenLookups", parameters, IsolationLevel.Snapshot, 30);

                #region Questions

                DataTable dtQuestions = dsQADetails.Tables[0];
                
                DataRow[] draCallIntro = dtQuestions.Select("QuestionTypeID = 1");
                if (draCallIntro.Any())
                {
                    DataTable dtCallIntro = draCallIntro.CopyToDataTable();
                    CallIntroQuestions = (from row in dtCallIntro.AsEnumerable()
                                            select new QAQuestion()
                                            {
                                                ID = Convert.ToInt64(row["QuestionID"]),
                                                Question = Convert.ToString(row["Question"]),
                                                Answer = Convert.ToBoolean(row["Answer"])
                                            }).ToList();
                    introTotalCount = CallIntroQuestions.Count;
                }
                
                DataRow[] draPitch = dtQuestions.Select("QuestionTypeID = 2");
                if (draPitch.Any())
                {
                    DataTable dtPitch = draPitch.CopyToDataTable();
                    PitchQuestions = (from row in dtPitch.AsEnumerable()
                                        select new QAQuestion()
                                        {
                                            ID = Convert.ToInt64(row["QuestionID"]),
                                            Question = Convert.ToString(row["Question"]),
                                            Answer = Convert.ToBoolean(row["Answer"])
                                        }).ToList();
                    pitchTotalCoun = PitchQuestions.Count;
                }
                
                DataRow[] draBorderline = dtQuestions.Select("QuestionTypeID = 3");
                if (draBorderline.Any())
                {
                    DataTable dtBorderline = draBorderline.CopyToDataTable();
                    BorderlineQuestions = (from row in dtBorderline.AsEnumerable()
                                            select new QAQuestion()
                                            {
                                                ID = Convert.ToInt64(row["QuestionID"]),
                                                Question = Convert.ToString(row["Question"]),
                                                Answer = Convert.ToBoolean(row["Answer"])
                                            }).ToList();
                }
                
                DataRow[] draAdmin= dtQuestions.Select("QuestionTypeID = 4");
                if (draAdmin.Any())
                {
                    DataTable dtAdmin = draAdmin.CopyToDataTable();
                    AdminQuestions = (from row in dtAdmin.AsEnumerable()
                                        select new QAQuestion()
                                        {
                                            ID = Convert.ToInt64(row["QuestionID"]),
                                            Question = Convert.ToString(row["Question"]),
                                            Answer = Convert.ToBoolean(row["Answer"])
                                        }).ToList();
                    adminTotalCoun = AdminQuestions.Count;
                }
                
                DataRow[] draPartner = dtQuestions.Select("QuestionTypeID = 5");
                if (draPartner.Any())
                {
                    DataTable dtPartner = draPartner.CopyToDataTable();
                    PartnerQuestions = (from row in dtPartner.AsEnumerable()
                                        select new QAQuestion()
                                        {
                                            ID = Convert.ToInt64(row["QuestionID"]),
                                            Question = Convert.ToString(row["Question"]),
                                            Answer = Convert.ToBoolean(row["Answer"])
                                        }).ToList();
                    partnerTotalCoun = PartnerQuestions.Count;
                }
                
                DataRow[] draClosure = dtQuestions.Select("QuestionTypeID = 6");
                if (draClosure.Any())
                {
                    DataTable dtClosure = draClosure.CopyToDataTable();
                    ClosureQuestions = (from row in dtClosure.AsEnumerable()
                                        select new QAQuestion()
                                        {
                                            ID = Convert.ToInt64(row["QuestionID"]),
                                            Question = Convert.ToString(row["Question"]),
                                            Answer = Convert.ToBoolean(row["Answer"])
                                        }).ToList();
                    closureTotalCoun = ClosureQuestions.Count;
                }

                #endregion

                #region Other

                DataTable dtQAs = dsQADetails.Tables[1];
                QAs = (from row in dtQAs.AsEnumerable()
                       select new LookupType()
                       {
                           ID = Convert.ToInt32(row["ID"]),
                           Name = Convert.ToString(row["Description"])
                       }).ToList();

                DataTable dtQAOverview = dsQADetails.Tables[2];
                if (dtQAOverview.Rows.Count > 0)
                {
                    SelectedQA = new LookupType()
                    {
                        ID = Convert.ToInt32(dtQAOverview.Rows[0]["AssessorID"]),
                        Name = Convert.ToString(dtQAOverview.Rows[0]["AssessorName"])
                    };
                }


                #endregion

                foreach (var item in CallIntroQuestions)
                {
                    if (item.Answer == true)
                    {
                        introSelectedCount = introSelectedCount + 1;
                    }
                }
                foreach (var item in PitchQuestions)
                {
                    if (item.Answer == true)
                    {
                        pitchSelectedlCoun = pitchSelectedlCoun + 1;
                    }
                }
                foreach (var item in AdminQuestions)
                {
                    if (item.Answer == true)
                    {
                        adminSelectedCoun = adminSelectedCoun + 1;
                    }
                }
                foreach (var item in PartnerQuestions)
                {
                    if (item.Answer == true)
                    {
                        partnerSelectedCoun = partnerSelectedCoun + 1;
                    }
                }
                foreach (var item in ClosureQuestions)
                {
                    if (item.Answer == true)
                    {
                        closureSelectedCoun = closureSelectedCoun + 1;
                    }
                }

                int introPercentage = 5;
                int pitchPercentage = 40;
                int adminPercentage = 25;
                int partnerPercentage = 20;
                int closurePercentage = 10;

                decimal IntroCalculatedPercentage = 0;
                decimal PitchCalculatedPercentage = 0;
                decimal AdminCalculatedPercenatge = 0;
                decimal PartnerCalculatedPercenatage = 0;
                decimal ClosureCalculateedPercenatge = 0;

                try
                {
                    decimal Step1 = introSelectedCount / introTotalCount;
                    IntroCalculatedPercentage = Step1 * introPercentage;
                }
                catch 
                { 
                    if(introTotalCount == 0)
                    {
                        IntroCalculatedPercentage = introPercentage;
                    }
                    else if(introSelectedCount == 0)
                    {
                        IntroCalculatedPercentage = introPercentage;
                    }
                    else
                    {
                        IntroCalculatedPercentage = 0;
                    }
                }
                try
                {
                    decimal step1 = pitchSelectedlCoun / pitchTotalCoun;
                    PitchCalculatedPercentage = step1 * pitchPercentage;
                }
                catch
                { 
                    if(pitchTotalCoun == 0)
                    {
                        PitchCalculatedPercentage = pitchPercentage;
                    }
                    else if (pitchSelectedlCoun == 0)
                    {
                        PitchCalculatedPercentage = pitchPercentage;
                    }
                    else
                    {
                        PitchCalculatedPercentage = 0;
                    }
                }
                try
                {
                    decimal step1 = adminSelectedCoun / adminTotalCoun;
                    AdminCalculatedPercenatge = step1 * adminPercentage;

                }
                catch
                {
                    if(adminTotalCoun == 0)
                    {
                        AdminCalculatedPercenatge = adminPercentage;
                    }
                    else if(adminSelectedCoun == 0)
                    {
                        AdminCalculatedPercenatge = adminPercentage;
                    }
                    else
                    {
                        AdminCalculatedPercenatge = 0;
                    }                 
                }
                try
                {
                    decimal step1 = partnerSelectedCoun / partnerTotalCoun;
                    PartnerCalculatedPercenatage = step1 * partnerPercentage;

                }
                catch
                { 
                    if(partnerTotalCoun == 0)
                    {
                        PartnerCalculatedPercenatage = partnerPercentage;
                    }
                    else if(partnerSelectedCoun == 0)
                    {
                        PartnerCalculatedPercenatage = partnerPercentage;
                    }
                    else
                    {
                        PartnerCalculatedPercenatage = 0;

                    }
                }
                try
                {
                    decimal step1 = closureSelectedCoun / closureTotalCoun;
                    ClosureCalculateedPercenatge = step1 * closurePercentage;

                }
                catch
                { 
                    if(closureTotalCoun == 0)
                    {
                        ClosureCalculateedPercenatge = closurePercentage;
                    }
                    else if(closureSelectedCoun == 0)
                    {
                        ClosureCalculateedPercenatge = closurePercentage;
                    }
                    else
                    {
                        ClosureCalculateedPercenatge = 0;
                    }
                }
                OverallPercentage = 0;
                if (QADetailsScreen.OverrideBool == true)
                {
                    OverallPercentage = 0.00m;
                }
                else if(QADetailsScreen.BorderlineBool == true)
                {
                    OverallPercentage = 50.00m;
                }
                else
                {
                    OverallPercentage = IntroCalculatedPercentage + PitchCalculatedPercentage + AdminCalculatedPercenatge + PartnerCalculatedPercenatage + ClosureCalculateedPercenatge;
                    if(OverallPercentage == 100.00m)
                    {
                        OverallPercentage = 0.00m;
                    }
                }

                OverallScore = "The score acquired is : " + OverallPercentage.ToString("#.##") + "%.";
            }

            catch (Exception ex)
            {
                new BaseControl().HandleException(ex);
            }
        }

        public void Save(object sender, DoWorkEventArgs e)
        {
            // General QA Assessment details
            INQADetailsINImport iNQADetailsINImport = INQADetailsINImportMapper.SearchOne(_importID, null, null);

            if (iNQADetailsINImport == null)
            {
                iNQADetailsINImport = new INQADetailsINImport();
            }

            iNQADetailsINImport.FKImportID = _importID;
            //iNQADetailsINImport.FKAssessorID = SelectedQA.ID;
            iNQADetailsINImport.FKAssessorID = QADetailsScreen.SelectedQAManual;
            iNQADetailsINImport.Save(null);


            //Questions
            foreach (var question in CallIntroQuestions)
            {
                INQADetailsQuestionINImport iNQADetailsQuestionINImport = INQADetailsQuestionINImportMapper.SearchOne(_importID, question.ID, null);

                if (iNQADetailsQuestionINImport == null)
                {
                    iNQADetailsQuestionINImport = new INQADetailsQuestionINImport();
                }

                iNQADetailsQuestionINImport.FKImportID = _importID;
                iNQADetailsQuestionINImport.FKQuestionID = question.ID;
                iNQADetailsQuestionINImport.AnswerInt = Convert.ToInt64(question.Answer);
                iNQADetailsQuestionINImport.Save(null);
            }

            foreach (var question in PitchQuestions)
            {
                INQADetailsQuestionINImport iNQADetailsQuestionINImport = INQADetailsQuestionINImportMapper.SearchOne(_importID, question.ID, null);

                if (iNQADetailsQuestionINImport == null)
                {
                    iNQADetailsQuestionINImport = new INQADetailsQuestionINImport();
                }

                iNQADetailsQuestionINImport.FKImportID = _importID;
                iNQADetailsQuestionINImport.FKQuestionID = question.ID;
                iNQADetailsQuestionINImport.AnswerInt = Convert.ToInt64(question.Answer);
                iNQADetailsQuestionINImport.Save(null);
            }

            foreach (var question in BorderlineQuestions)
            {
                INQADetailsQuestionINImport iNQADetailsQuestionINImport = INQADetailsQuestionINImportMapper.SearchOne(_importID, question.ID, null);

                if (iNQADetailsQuestionINImport == null)
                {
                    iNQADetailsQuestionINImport = new INQADetailsQuestionINImport();
                }

                iNQADetailsQuestionINImport.FKImportID = _importID;
                iNQADetailsQuestionINImport.FKQuestionID = question.ID;
                iNQADetailsQuestionINImport.AnswerInt = Convert.ToInt64(question.Answer);
                iNQADetailsQuestionINImport.Save(null);
            }

            foreach (var question in AdminQuestions)
            {
                INQADetailsQuestionINImport iNQADetailsQuestionINImport = INQADetailsQuestionINImportMapper.SearchOne(_importID, question.ID, null);

                if (iNQADetailsQuestionINImport == null)
                {
                    iNQADetailsQuestionINImport = new INQADetailsQuestionINImport();
                }

                iNQADetailsQuestionINImport.FKImportID = _importID;
                iNQADetailsQuestionINImport.FKQuestionID = question.ID;
                iNQADetailsQuestionINImport.AnswerInt = Convert.ToInt64(question.Answer);
                iNQADetailsQuestionINImport.Save(null);
            }

            foreach (var question in PartnerQuestions)
            {
                INQADetailsQuestionINImport iNQADetailsQuestionINImport = INQADetailsQuestionINImportMapper.SearchOne(_importID, question.ID, null);

                if (iNQADetailsQuestionINImport == null)
                {
                    iNQADetailsQuestionINImport = new INQADetailsQuestionINImport();
                }

                iNQADetailsQuestionINImport.FKImportID = _importID;
                iNQADetailsQuestionINImport.FKQuestionID = question.ID;
                iNQADetailsQuestionINImport.AnswerInt = Convert.ToInt64(question.Answer);
                iNQADetailsQuestionINImport.Save(null);
            }

            foreach (var question in ClosureQuestions)
            {
                INQADetailsQuestionINImport iNQADetailsQuestionINImport = INQADetailsQuestionINImportMapper.SearchOne(_importID, question.ID, null);

                if (iNQADetailsQuestionINImport == null)
                {
                    iNQADetailsQuestionINImport = new INQADetailsQuestionINImport();
                }

                iNQADetailsQuestionINImport.FKImportID = _importID;
                iNQADetailsQuestionINImport.FKQuestionID = question.ID;
                iNQADetailsQuestionINImport.AnswerInt = Convert.ToInt64(question.Answer);
                iNQADetailsQuestionINImport.Save(null);
            }


            if (iNQADetailsINImport == null)
            {
                iNQADetailsINImport = new INQADetailsINImport();
            }
            introSelectedCount = 0;

            foreach (var item in CallIntroQuestions)
            {
                if (item.Answer == true)
                {
                    introSelectedCount = introSelectedCount + 1;
                }
            }
            pitchSelectedlCoun = 0;

            foreach (var item in PitchQuestions)
            {
                if (item.Answer == true)
                {
                    pitchSelectedlCoun = pitchSelectedlCoun + 1;
                }
            }
            adminSelectedCoun = 0;

            foreach (var item in AdminQuestions)
            {
                if (item.Answer == true)
                {
                    adminSelectedCoun = adminSelectedCoun + 1;
                }
            }
            partnerSelectedCoun = 0;

            foreach (var item in PartnerQuestions)
            {
                if (item.Answer == true)
                {
                    partnerSelectedCoun = partnerSelectedCoun + 1;
                }
            }
            closureSelectedCoun = 0;

            foreach (var item in ClosureQuestions)
            {
                if (item.Answer == true)
                {
                    closureSelectedCoun = closureSelectedCoun + 1;
                }
            }

            int introPercentage = 5;
            int pitchPercentage = 40;
            int adminPercentage = 25;
            int partnerPercentage = 20;
            int closurePercentage = 10;

            decimal IntroCalculatedPercentage = 0;
            decimal PitchCalculatedPercentage = 0;
            decimal AdminCalculatedPercenatge = 0;
            decimal PartnerCalculatedPercenatage = 0;
            decimal ClosureCalculateedPercenatge = 0;

            try
            {
                decimal Step1 = introSelectedCount / introTotalCount;
                 IntroCalculatedPercentage = Step1 * introPercentage;
            }
            catch
            { 
                if(introTotalCount == 0)
                {
                    IntroCalculatedPercentage = introPercentage;
                }
                else if(introSelectedCount == 0)
                {
                    IntroCalculatedPercentage = introPercentage;
                }
                else
                {
                    IntroCalculatedPercentage = 0;
                }
            }
            try
            {
                decimal step1 = pitchSelectedlCoun / pitchTotalCoun;
                PitchCalculatedPercentage = step1 * pitchPercentage;
            }
            catch 
            {
                if (pitchTotalCoun == 0)
                {
                    PitchCalculatedPercentage = pitchPercentage;
                }
                else if(pitchSelectedlCoun == 0)
                {
                    PitchCalculatedPercentage = pitchPercentage;
                }
                else
                {
                    PitchCalculatedPercentage = 0;
                }
            }
            try
            {
                decimal step1 = adminSelectedCoun / adminTotalCoun;
                AdminCalculatedPercenatge = step1 * adminPercentage;

            }
            catch
            {
                if(adminTotalCoun == 0)
                {
                    AdminCalculatedPercenatge = adminPercentage;
                }
                else if(adminSelectedCoun == 0)
                {
                    AdminCalculatedPercenatge = adminPercentage;
                }
                else
                {
                    AdminCalculatedPercenatge = 0;
                }
            }
            try
            {
                decimal step1 = partnerSelectedCoun / partnerTotalCoun;
                PartnerCalculatedPercenatage = step1 * partnerPercentage;

            }
            catch
            {
                if(partnerTotalCoun == 0)
                {
                    PartnerCalculatedPercenatage = partnerPercentage;
                }
                else if(partnerSelectedCoun == 0)
                {
                    PartnerCalculatedPercenatage = partnerPercentage;
                }
                else
                {
                    PartnerCalculatedPercenatage = 0;

                }
            }
            try
            {
                decimal step1 = closureSelectedCoun / closureTotalCoun;
                ClosureCalculateedPercenatge = step1 * closurePercentage;

            }
            catch { ClosureCalculateedPercenatge = 0; }

            OverallPercentage = 0;
            if (QADetailsScreen.OverrideBool == true)
            {
                OverallPercentage = 0.00m;
            }
            else if (QADetailsScreen.BorderlineBool == true)
            {
                OverallPercentage = 50.00m;
            }
            else
            {
                OverallPercentage = IntroCalculatedPercentage + PitchCalculatedPercentage + AdminCalculatedPercenatge + PartnerCalculatedPercenatage + ClosureCalculateedPercenatge;
                if(OverallPercentage == 100.00m)
                {
                    OverallPercentage = 0.00m;
                }
            }
            OverallScore = "The score acquired is : " + OverallPercentage.ToString("#.##") + "%.";

        }

        private void SaveCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsSaving = false;
        }

        public void CalculateResult()
        {
            INQADetailsINImport iNQADetailsINImport = INQADetailsINImportMapper.SearchOne(_importID, null, null);

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        #endregion

    }
}
