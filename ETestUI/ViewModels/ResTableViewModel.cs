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
    public class ResTableViewModel : BindableBase, INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IDialogService _dialogService;
        int index;
        Segment seg;
        #endregion
        #region 属性绑定
        private ObservableCollection<ResItem> resList = new ObservableCollection<ResItem>();
        public ObservableCollection<ResItem> ResList
        {
            get { return resList; }
            set { SetProperty(ref resList, value); }
        }
        private int resSelectedIndex;
        public int ResSelectedIndex
        {
            get { return resSelectedIndex; }
            set { SetProperty(ref resSelectedIndex, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;

        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));
        private DelegateCommand<object> mouseDoubleClickCommand;
        public DelegateCommand<object> MouseDoubleClickCommand =>
            mouseDoubleClickCommand ?? (mouseDoubleClickCommand = new DelegateCommand<object>(ExecuteMouseDoubleClickCommand));
        private DelegateCommand<object> resListCheckCommand;
        public DelegateCommand<object> ResListCheckCommand =>
            resListCheckCommand ?? (resListCheckCommand = new DelegateCommand<object>(ExecuteResListCheckCommand));

        void ExecuteResListCheckCommand(object obj)
        {
            if (seg != null)
            {
                seg.ResList[(int)obj].Select = ResList[(int)obj].Select;
                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
            }
        }
        void ExecuteMouseDoubleClickCommand(object obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Content", seg.ResList[(int)obj].Content);
            _dialogService.ShowDialog("FourPointSelectDialog", param, arg => {
                if (arg.Result == ButtonResult.Yes)
                {
                    string NewContent = arg.Parameters.GetValue<string>("Content");
                    seg.ResList[(int)obj].Content = NewContent;
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
                        if (seg.ResList.Count > 0)
                        {
                            id1 = seg.ResList.Max(t => t.Id) + 1;
                        }
                        seg.ResList.Add(new Common.Models.ResItem()
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
                        if (ResSelectedIndex >= 0)
                        {
                            if (MessageBox.Show($"删除电阻\"{ResList[ResSelectedIndex].Id}\"项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                seg.ResList.RemoveAt(ResSelectedIndex);
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
        public ResTableViewModel(IParameterService parameterService, IDialogService dialogService)
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
                ResList.Clear();
                for (int i = 0; i < seg.ResList.Count; i++)
                {
                    ResList.Add(new ResItem()
                    {
                        Id = seg.ResList[i].Id,
                        Content = seg.ResList[i].Content,
                        Select = seg.ResList[i].Select,
                        PropUpLimit = seg.resProperty.PropUpLimit,
                        PropDownLimit = seg.resProperty.PropDownLimit
                    });
                }
                
            }

        }
        #endregion
    }
    public class ResItem : BindableBase
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
