using ETestUI.Common.Models;
using ETestUI.Service;
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
    public class RgPropertyDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IParameterService _parameterService;
        #region 变量

        #endregion
        #region 属性绑定
        public string Title => "光敏电阻测试属性设置";
        private double propUpLimit;
        public double PropUpLimit
        {
            get { return propUpLimit; }
            set { SetProperty(ref propUpLimit, value); }
        }
        private double propDownLimit;
        public double PropDownLimit
        {
            get { return propDownLimit; }
            set { SetProperty(ref propDownLimit, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;

        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));

        void ExecuteOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    DialogParameters param = new DialogParameters();
                    param.Add("PropUpLimit", PropUpLimit);
                    param.Add("PropDownLimit", PropDownLimit);
                    RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, param));
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
            int id = parameters.GetValue<int>("Id");
            Segment seg = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.FirstOrDefault(t => t.Id == id);
            if (seg != null)
            {
                PropUpLimit = seg.rgProperty.PropUpLimit;
                PropDownLimit = seg.rgProperty.PropDownLimit;
            }
        }
        #endregion
        #region 构造函数
        public RgPropertyDialogViewModel(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }
        #endregion
    }
}
