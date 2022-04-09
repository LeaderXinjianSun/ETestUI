using ETestUI.Common;
using ETestUI.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETestUI.ViewModels
{
    public class ProjectInfoViewModel : BindableBase , INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IDialogService _dialogService;
        #endregion
        #region 属性
        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set { SetProperty(ref projectName, value); }
        }
        private DateTime createTime;
        public DateTime CreateTime
        {
            get { return createTime; }
            set { SetProperty(ref createTime, value); }
        }
        private DateTime modifyTime;
        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { SetProperty(ref modifyTime, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;
        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));
        #endregion
        #region 方法绑定函数
        void ExecuteOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    _dialogService.ShowDialog("AddProjectDialog", arg => {
                        if (arg.Result == ButtonResult.Yes)
                        {
                            string NewName = arg.Parameters.GetValue<string>("NewName");
                            if (NewName != null)
                            {
                                _parameterService.Add(NewName);
                                _parameterService.MyParam.SelectedIndex = _parameterService.MyParam.Projects.Count - 1;
                                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                ReLoad();
                            }
                            else
                            {
                                MessageBox.Show("项目名为空","错误",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    });
                    break;
                case "1":
                    if (ProjectName != null)
                    {
                        if (MessageBox.Show($"删除\"{ProjectName}\"项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            _parameterService.MyParam.Projects.RemoveAt(_parameterService.MyParam.SelectedIndex);
                            _parameterService.MyParam.SelectedIndex = 0;
                            _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                            ProjectName = null;
                        }
                    }              
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 构造函数
        public ProjectInfoViewModel(IParameterService parameterService, IDialogService dialogService)
        {
            _parameterService = parameterService;
            ReLoad();
            _dialogService = dialogService;
        }
        #endregion
        #region 功能函数
        private void ReLoad()
        {
            ProjectName = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Name;
            CreateTime = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Create;
            ModifyTime = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Modify;
        }
        //OnNavigatedTo: 导航完成前, 此处可以传递过来的参数以及是否允许导航等动作的控制。
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
        //IsNavigationTarget: 调用以确定此实例是否可以处理导航请求。否则新建实例
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        //OnNavigatedFrom: 当导航离开当前页时, 类似打开A, 再打开B时, 该方法被触发。
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion
        #region 事件响应函数

        #endregion

    }
}
