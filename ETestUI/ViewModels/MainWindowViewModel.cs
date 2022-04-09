using ETestUI.Common;
using ETestUI.Common.Models;
using ETestUI.Service;
using ETestUI.Views;
using Newtonsoft.Json;
using NLog;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETestUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region 变量
        private readonly ICommunicationChannelService _communicationChannelService;
        private readonly IParameterService _parameterService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        #region 属性
        private string title = "ETestUI";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private string version = "1.0";
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }
        private DateTime statusDatetime;
        public DateTime StatusDatetime
        {
            get { return statusDatetime; }
            set { SetProperty(ref statusDatetime, value); }
        }
        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set { SetProperty(ref projectName, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand appLoadedEventCommand;
        public DelegateCommand AppLoadedEventCommand =>
            appLoadedEventCommand ?? (appLoadedEventCommand = new DelegateCommand(ExecuteAppLoadedEventCommand));
        private DelegateCommand appClosedEventCommand;
        public DelegateCommand AppClosedEventCommand =>
            appClosedEventCommand ?? (appClosedEventCommand = new DelegateCommand(ExecuteAppClosedEventCommand));

        private DelegateCommand<object> menuOperateCommand;
        public DelegateCommand<object> MenuOperateCommand =>
            menuOperateCommand ?? (menuOperateCommand = new DelegateCommand<object>(ExecuteMenuOperateCommand));

     

        #endregion
        #region 方法绑定函数
        void ExecuteAppLoadedEventCommand()
        {
            try
            {
                _parameterService.Load(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));

                var r = _communicationChannelService.Open(_parameterService.MyParam.COM);
                if (_parameterService.MyParam.Projects.Count == 0)
                    ProjectName = "null";
                else
                    ProjectName = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Name;
                logger.Info("软件加载完成");
                Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化:" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(ex);
            }
        }
        void ExecuteAppClosedEventCommand()
        {
            try
            {
                logger.Info("软件关闭");
                _communicationChannelService.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        void ExecuteMenuOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    break;
                case "1":
                    _dialogService.ShowDialog("LoadProjectDialog",arg => {
                        if (arg.Result == ButtonResult.Yes)
                        {
                            int index = arg.Parameters.GetValue<int>("Index");
                            if (_parameterService.MyParam.SelectedIndex != index)
                            {
                                _parameterService.MyParam.SelectedIndex = index;
                                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                _eventAggregator.GetEvent<MessageEvent>().Publish(new MessageItem { Sender = this, Message = "Select" });
                                ProjectName = _parameterService.MyParam.Projects[index].Name;
                            }
                            _regionManager.RequestNavigate("MainContentRegion", "ManageProjectView");
                            _regionManager.RequestNavigate("ProjectContentRegion", "ProjectInfoView");
                        }
                    });
                    break;
                case "2":
                    _regionManager.RequestNavigate("MainContentRegion", "ManageProjectView");
                    break;
                case "3":
                    break;
                case "4":
                    _regionManager.RequestNavigate("MainContentRegion", "TestView");
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 构造函数
        public MainWindowViewModel(IParameterService parameterService, ICommunicationChannelService communicationChannelService,IRegionManager regionManager, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _regionManager = regionManager;
            NlogConfig();
            _communicationChannelService = communicationChannelService;
            _parameterService = parameterService;
        }
        #endregion
        #region 功能函数
        #region 运行
        private async void Run()
        {
            while (true)
            {
                try
                {
                    StatusDatetime = DateTime.Now;
                }
                catch { }
                await Task.Delay(1000);
            }
        }
        #endregion
        #region 其他
        private void NlogConfig()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "${basedir}/logs/${shortdate}.log", Layout = "${longdate}|${level:uppercase=true}|${message}" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }
        #endregion
        #endregion
    }
}