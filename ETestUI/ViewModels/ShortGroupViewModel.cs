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
    public class ShortGroupViewModel : BindableBase, INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IDialogService _dialogService;
        int index;
        Segment seg;
        #endregion
        #region 属性绑定
        private ObservableCollection<ShortGroupItem> shortGroupList = new ObservableCollection<ShortGroupItem>();
        public ObservableCollection<ShortGroupItem> ShortGroupList
        {
            get { return shortGroupList; }
            set { SetProperty(ref shortGroupList, value); }
        }
        private ObservableCollection<OpenItem> openList = new ObservableCollection<OpenItem>();
        public ObservableCollection<OpenItem> OpenList
        {
            get { return openList; }
            set { SetProperty(ref openList, value); }
        }
        private ObservableCollection<ShortItem> shortList = new ObservableCollection<ShortItem>();
        public ObservableCollection<ShortItem> ShortList
        {
            get { return shortList; }
            set { SetProperty(ref shortList, value); }
        }
        private int shortGroupSelectedIndex;
        public int ShortGroupSelectedIndex
        {
            get { return shortGroupSelectedIndex; }
            set { SetProperty(ref shortGroupSelectedIndex, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;

        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));
        private DelegateCommand<object> mouseDoubleClickCommand;
        public DelegateCommand<object> MouseDoubleClickCommand =>
            mouseDoubleClickCommand ?? (mouseDoubleClickCommand = new DelegateCommand<object>(ExecuteMouseDoubleClickCommand));
        private DelegateCommand<object> cellChangedCommand;
        public DelegateCommand<object> CellChangedCommand =>
            cellChangedCommand ?? (cellChangedCommand = new DelegateCommand<object>(ExecuteCellChangedCommand));

        void ExecuteCellChangedCommand(object obj)
        {
            if (seg != null)
            {
                seg.ShortGroupList[(int)obj].Remark = ShortGroupList[(int)obj].Remark;
                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
            }
        }
        void ExecuteMouseDoubleClickCommand(object obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Content", seg.ShortGroupList[(int)obj].Content);
            _dialogService.ShowDialog("SelectPointDialog", param, arg => {
                if (arg.Result == ButtonResult.Yes)
                {
                    string NewContent = arg.Parameters.GetValue<string>("Content");
                    seg.ShortGroupList[(int)obj].Content = NewContent;
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
                        if (seg.ShortGroupList.Count > 0)
                        {
                            id1 = seg.ShortGroupList.Max(t => t.Id) + 1;
                        }
                        seg.ShortGroupList.Add(new Common.Models.ShortGroupItem()
                        {
                            Id = id1
                        });
                        _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                        Reload();
                    }

                    break;
                case "1":
                    if (seg != null)
                    {
                        if (ShortGroupSelectedIndex >= 0)
                        {
                            if (MessageBox.Show($"删除短路群\"{ShortGroupList[ShortGroupSelectedIndex].Id}\"项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                seg.ShortGroupList.RemoveAt(ShortGroupSelectedIndex);
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
        public ShortGroupViewModel(IParameterService parameterService, IDialogService dialogService)
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
                ShortGroupList.Clear();
                for (int i = 0; i < seg.ShortGroupList.Count; i++)
                {
                    ShortGroupList.Add(new ShortGroupItem()
                    {
                        Id = seg.ShortGroupList[i].Id,
                        Content = seg.ShortGroupList[i].Content,
                        Remark = seg.ShortGroupList[i].Remark
                    });
                }
                OpenList.Clear();
                for (int i = 0; i < seg.OpenList.Count; i++)
                {
                    OpenList.Add(new OpenItem()
                    {
                        Id = seg.OpenList[i].Id,
                        Content = seg.OpenList[i].Content,
                        PropUpLimit = seg.openProperty.PropUpLimit,
                        PropDownLimit = seg.openProperty.PropDownLimit,
                        Select = seg.OpenList[i].Select
                    });
                }
                ShortList.Clear();
                for (int i = 0; i < seg.ShortList.Count; i++)
                {
                    ShortList.Add(new ShortItem()
                    {
                        Id = seg.ShortList[i].Id,
                        Content = seg.ShortList[i].Content,
                        PropUpLimit = seg.shortProperty.PropUpLimit,
                        PropDownLimit = seg.shortProperty.PropDownLimit,
                        Select = seg.ShortList[i].Select
                    });
                }
            }
            
        }
        #endregion
    }
    public class ShortGroupItem : BindableBase
    {
        public int Id { get; set; }
        private string content;
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }
        public string Remark { get; set; }
    }
    public class OpenItem
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public double PropUpLimit { get; set; }
        public double PropDownLimit { get; set; }
        public bool Select { get; set; }
    }
    public class ShortItem
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public double PropUpLimit { get; set; }
        public double PropDownLimit { get; set; }
        public bool Select { get; set; }
    }
}
