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
    public class TvsTableViewModel : BindableBase, INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IDialogService _dialogService;
        int index;
        Segment seg;
        #endregion
        #region 属性绑定
        private ObservableCollection<TvsItem> tvsList = new ObservableCollection<TvsItem>();
        public ObservableCollection<TvsItem> TvsList
        {
            get { return tvsList; }
            set { SetProperty(ref tvsList, value); }
        }
        private int tvsSelectedIndex;
        public int TvsSelectedIndex
        {
            get { return tvsSelectedIndex; }
            set { SetProperty(ref tvsSelectedIndex, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> operateCommand;

        public DelegateCommand<object> OperateCommand =>
            operateCommand ?? (operateCommand = new DelegateCommand<object>(ExecuteOperateCommand));
        private DelegateCommand<object> mouseDoubleClickCommand;
        public DelegateCommand<object> MouseDoubleClickCommand =>
            mouseDoubleClickCommand ?? (mouseDoubleClickCommand = new DelegateCommand<object>(ExecuteMouseDoubleClickCommand));
        private DelegateCommand<object> tvsListCheckCommand;
        public DelegateCommand<object> TvsListCheckCommand =>
            tvsListCheckCommand ?? (tvsListCheckCommand = new DelegateCommand<object>(ExecuteTvsListCheckCommand));

        void ExecuteTvsListCheckCommand(object obj)
        {
            if (seg != null)
            {
                seg.TvsList[(int)obj].Select = TvsList[(int)obj].Select;
                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
            }
        }
        void ExecuteMouseDoubleClickCommand(object obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Content", seg.TvsList[(int)obj].Content);
            _dialogService.ShowDialog("FourPointSelectDialog", param, arg => {
                if (arg.Result == ButtonResult.Yes)
                {
                    string NewContent = arg.Parameters.GetValue<string>("Content");
                    seg.TvsList[(int)obj].Content = NewContent;
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
                        if (seg.TvsList.Count > 0)
                        {
                            id1 = seg.TvsList.Max(t => t.Id) + 1;
                        }
                        seg.TvsList.Add(new Common.Models.TvsItem()
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
                        if (TvsSelectedIndex >= 0)
                        {
                            if (MessageBox.Show($"删除双向稳压管\"{TvsList[TvsSelectedIndex].Id}\"项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                seg.TvsList.RemoveAt(TvsSelectedIndex);
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
        public TvsTableViewModel(IParameterService parameterService, IDialogService dialogService)
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
                TvsList.Clear();
                for (int i = 0; i < seg.TvsList.Count; i++)
                {
                    TvsList.Add(new TvsItem()
                    {
                        Id = seg.TvsList[i].Id,
                        Content = seg.TvsList[i].Content,
                        Select = seg.TvsList[i].Select,
                        PropUpLimit = seg.tvsProperty.PropUpLimit,
                        PropDownLimit = seg.tvsProperty.PropDownLimit
                    });
                }
                
            }

        }
        #endregion
    }
    public class TvsItem : BindableBase
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
