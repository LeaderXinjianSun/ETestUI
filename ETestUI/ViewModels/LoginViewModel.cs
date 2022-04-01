using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IRegionManager _regionManager;
        public string Title => "请选择功能";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
        private DelegateCommand<object> operateCommand;
        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));

        void ExecuteOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    _regionManager.RegisterViewWithRegion("MainRegionContent", "TestMainView");
                    break;
                case "1":
                    _regionManager.RegisterViewWithRegion("MainRegionContent", "ConfigMainView");
                    break;
                default:
                    break;
            }
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
