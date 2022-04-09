using ETestUI.Common;
using ETestUI.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.ViewModels.Dialogs
{
    public class LoadProjectDialogViewModel : BindableBase, IDialogAware
    {
        #region 属性绑定
        public string Title => "加载项目";
        //ProjectList
        private ObservableCollection<ProjectItem> projectList = new ObservableCollection<ProjectItem>();
        public ObservableCollection<ProjectItem> ProjectList
        {
            get { return projectList; }
            set { SetProperty(ref projectList, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;
        private readonly IParameterService _parameterService;

        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));
        #endregion
        #region 方法绑定函数
        void ExecuteOperateCommand(object obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Index", (int)obj);
            RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, param));
           
        }
        #endregion
        #region 构造函数
        public LoadProjectDialogViewModel(IParameterService parameterService)
        {
            _parameterService = parameterService;
            for (int i = 0; i < _parameterService.MyParam.Projects.Count; i++)
            {
                ProjectList.Add(new ProjectItem() { 
                    Id = _parameterService.MyParam.Projects[i].Id,
                    Name = _parameterService.MyParam.Projects[i].Name
                });
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
    public class ProjectItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
