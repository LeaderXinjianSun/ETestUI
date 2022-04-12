using ETestUI.Common.Models;
using ETestUI.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETestUI.ViewModels
{
    public class RgTableViewModel : BindableBase, INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IDialogService _dialogService;
        int index;
        Segment seg;
        #endregion
        #region 属性绑定
        private ObservableCollection<RgItem> rgList = new ObservableCollection<RgItem>();
        public ObservableCollection<RgItem> RgList
        {
            get { return rgList; }
            set { SetProperty(ref rgList, value); }
        }
        private int rgSelectedIndex;
        public int RgSelectedIndex
        {
            get { return rgSelectedIndex; }
            set { SetProperty(ref rgSelectedIndex, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;

        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));
        private DelegateCommand<object> mouseDoubleClickCommand;
        public DelegateCommand<object> MouseDoubleClickCommand =>
            mouseDoubleClickCommand ?? (mouseDoubleClickCommand = new DelegateCommand<object>(ExecuteMouseDoubleClickCommand));
        private DelegateCommand<object> rgListCheckCommand;
        public DelegateCommand<object> RgListCheckCommand =>
            rgListCheckCommand ?? (rgListCheckCommand = new DelegateCommand<object>(ExecuteRgListCheckCommand));

        void ExecuteRgListCheckCommand(object obj)
        {
            if (seg != null)
            {
                seg.RgList[(int)obj].Select = RgList[(int)obj].Select;
                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
            }
        }
        void ExecuteMouseDoubleClickCommand(object obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Content", seg.RgList[(int)obj].Content);
            _dialogService.ShowDialog("TwoPointSelectDialog", param, arg => {
                if (arg.Result == ButtonResult.Yes)
                {
                    string NewContent = arg.Parameters.GetValue<string>("Content");
                    seg.RgList[(int)obj].Content = NewContent;
                    _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                    Reload();
                }
            });
        }
        void ExecuteOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    if (seg != null)
                    {
                        int id1 = 1;
                        if (seg.RgList.Count > 0)
                        {
                            id1 = seg.RgList.Max(t => t.Id) + 1;
                        }
                        seg.RgList.Add(new Common.Models.RgItem()
                        {
                            Id = id1,
                            Select = true
                        });
                        _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                        Reload();
                    }

                    break;
                case "1":
                    if (seg != null)
                    {
                        if (RgSelectedIndex >= 0)
                        {
                            if (MessageBox.Show($"删除光敏\"{RgList[RgSelectedIndex].Id}\"项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                seg.RgList.RemoveAt(RgSelectedIndex);
                                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                Reload();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 自带的
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            index = navigationContext.Parameters.GetValue<int>("Index");
            seg = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.FirstOrDefault(t => t.Id == index);
            Reload();
        }
        #endregion
        #region 构造函数
        public RgTableViewModel(IParameterService parameterService, IDialogService dialogService)
        {
            _parameterService = parameterService;
            _dialogService = dialogService;
        }
        #endregion
        #region 功能函数
        private void Reload()
        {

            if (seg != null)
            {
                RgList.Clear();
                for (int i = 0; i < seg.RgList.Count; i++)
                {
                    RgList.Add(new RgItem()
                    {
                        Id = seg.RgList[i].Id,
                        Content = seg.RgList[i].Content,
                        Select = seg.RgList[i].Select,
                        PropUpLimit = seg.rgProperty.PropUpLimit,
                        PropDownLimit = seg.rgProperty.PropDownLimit
                    });
                }
                
            }

        }
        #endregion
    }
    public class RgItem : BindableBase
    {
        public int Id { get; set; }
        private string content;
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }
        public bool Select { get; set; }
        public double PropUpLimit { get; set; }
        public double PropDownLimit { get; set; }
    }
}
