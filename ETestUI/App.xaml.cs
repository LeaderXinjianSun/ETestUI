using ETestUI.Service;
using ETestUI.ViewModels;
using ETestUI.ViewModels.Dialogs;
using ETestUI.Views;
using ETestUI.Views.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ETestUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICommunicationChannelService, SerialPortChannelService>();
            containerRegistry.RegisterSingleton<IParameterService, ParameterService>();
            containerRegistry.RegisterForNavigation<ManageProjectView, ManageProjectViewModel>();
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();
            containerRegistry.RegisterForNavigation<ProjectInfoView, ProjectInfoViewModel>();
            containerRegistry.RegisterForNavigation<TestPointInfoView, TestPointInfoViewModel>();
            containerRegistry.RegisterForNavigation<TestSegmentView, TestSegmentViewModel>();
            containerRegistry.RegisterForNavigation<SegmentDetailView, SegmentDetailViewModel>();
            containerRegistry.RegisterDialog<AddProjectDialog, AddProjectDialogViewModel>();
            containerRegistry.RegisterDialog<LoadProjectDialog, LoadProjectDialogViewModel>();
        }
    }
}
