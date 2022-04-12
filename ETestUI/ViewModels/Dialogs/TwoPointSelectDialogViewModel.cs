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
    public class TwoPointSelectDialogViewModel : BindableBase, IDialogAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        #endregion
        #region 属性
        public string Title => "选点";
        private PointItem point1;
        public PointItem Point1
        {
            get { return point1; }
            set { SetProperty(ref point1, value); }
        }
        private PointItem point2;
        public PointItem Point2
        {
            get { return point2; }
            set { SetProperty(ref point2, value); }
        }
        private ObservableCollection<PointItem> points = new ObservableCollection<PointItem>();
        public ObservableCollection<PointItem> Points
        {
            get { return points; }
            set { SetProperty(ref points, value); }
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
                    if (Point1 != null && Point2 != null)
                    {
                        DialogParameters param = new DialogParameters();
                        param.Add("Content", $"{Point1.Index},{Point2.Index}");
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, param));
                    }
                    else
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
                    }
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
            for (int i = 0; i < _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints.Count; i++)
            {
                Points.Add(new PointItem() {
                    Index = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Index,
                    Name = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Name,
                    Alias = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Alias
                });
            }
            string content = parameters.GetValue<string>("Content");
            if (!string.IsNullOrEmpty(content))
            {
                string[] strs = content.Split(',');
                if (strs.Length == 2)
                {
                    Point1 = Points.FirstOrDefault(t => t.Index == int.Parse(strs[0]));
                    Point2 = Points.FirstOrDefault(t => t.Index == int.Parse(strs[1]));
                }
            }
        }
        #endregion
        #region 构造函数
        public TwoPointSelectDialogViewModel(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }
        #endregion
        #region 功能函数

        #endregion
    }
    public class PointItem
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
