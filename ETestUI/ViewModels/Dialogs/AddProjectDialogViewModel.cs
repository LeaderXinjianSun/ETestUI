using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.ViewModels.Dialogs
{
    public class AddProjectDialogViewModel : BindableBase, IDialogAware
    {
        #region 属性绑定
        public string Title => "创建项目";
        private string newName;
        public string NewName
        {
            get { return newName; }
            set { SetProperty(ref newName, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> addNewCommand;
        public DelegateCommand<object> AddNewCommand =>
            addNewCommand ?? (addNewCommand = new DelegateCommand<object>(ExecuteAddNewCommand));
        #endregion
        #region 方法绑定函数
        void ExecuteAddNewCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    DialogParameters param = new DialogParameters();
                    param.Add("NewName", NewName);
                    RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, param));
                    break;
                case "1":
                    RequestClose?.Invoke(new DialogResult(ButtonResult.No));
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 自带的
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
        #endregion


    }
}
