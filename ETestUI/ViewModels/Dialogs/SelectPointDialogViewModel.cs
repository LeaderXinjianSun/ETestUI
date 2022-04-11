using ETestUI.Service;
using Prism.Commands;
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
    public class SelectPointDialogViewModel : BindableBase, IDialogAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        #endregion
        #region 属性
        public string Title => "选择点";
        private ObservableCollection<InnerPint> points = new ObservableCollection<InnerPint>();
        public ObservableCollection<InnerPint> Points
        {
            get { return points; }
            set { SetProperty(ref points, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;

        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));
        private DelegateCommand<object> selectAllCommand;
        public DelegateCommand<object> SelectAllCommand =>
            selectAllCommand ?? (selectAllCommand = new DelegateCommand<object>(ExecuteSelectAllCommand));

        #endregion
        #region 方法绑定函数
        void ExecuteOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    string contant = "";
                    for (int i = 0; i < Points.Count; i++)
                    {
                        if (Points[i].Select)
                        {
                            contant += Points[i].Index.ToString() + ",";
                        }
                    }
                    if (contant != "")
                    {
                        contant = contant.Substring(0, contant.Length - 1);
                    }
                    DialogParameters Param = new DialogParameters();
                    Param.Add("Content", contant);
                    RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, Param));
                    break;
                default:
                    break;
            }
        }
        void ExecuteSelectAllCommand(object obj)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i].Select = (bool)obj;
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
            string[] strs;
            string content = parameters.GetValue<string>("Content");
            if (string.IsNullOrEmpty(content))
            {
                strs = new string[0];
            }
            else
            {
                strs = content.Split(new char[] { ',' });
            }
            for (int i = 0; i < _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints.Count; i++)
            {
                Points.Add(new InnerPint()
                {
                    Select = strs.Contains(_parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Index.ToString()),
                    Index = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Index,
                    Name = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Name,
                    Alias = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Alias
                });
            }
        }
        #endregion
        #region 构造函数
        public SelectPointDialogViewModel(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }
        #endregion

    }
    public class InnerPint : BindableBase
    {
        private bool select;
        public bool Select
        {
            get { return select; }
            set { SetProperty(ref select, value); }
        }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
