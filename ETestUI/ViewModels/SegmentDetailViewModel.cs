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

namespace ETestUI.ViewModels
{
    public class SegmentDetailViewModel : BindableBase, INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IDialogService _dialogService;
        int index;
        #endregion
        #region 属性绑定
        private ObservableCollection<TestSegmentItem> testSegmentItems = new ObservableCollection<TestSegmentItem>();
        public ObservableCollection<TestSegmentItem> TestSegmentItems
        {
            get { return testSegmentItems; }
            set { SetProperty(ref testSegmentItems, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> mouseDoubleClickCommand;
        public DelegateCommand<object> MouseDoubleClickCommand =>
            mouseDoubleClickCommand ?? (mouseDoubleClickCommand = new DelegateCommand<object>(ExecuteMouseDoubleClickCommand));
        private DelegateCommand<object> checkedCommand;
       
        public DelegateCommand<object> CheckedCommand =>
            checkedCommand ?? (checkedCommand = new DelegateCommand<object>(ExecuteCheckedCommand));

        void ExecuteCheckedCommand(object obj)
        {
            TestSegmentItems[(int)obj].Select = true;
            for (int i = 0; i < TestSegmentItems.Count; i++)
            {
                if (i != (int)obj)
                {
                    TestSegmentItems[i].Select = false;
                }
            }
            Segment seg = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.FirstOrDefault(t => t.Id == index);
            if (seg != null)
            {
                switch ((int)obj)
                {
                    case 0:
                        seg.segmentType = SegmentType.开路;
                        break;
                    case 1:
                        seg.segmentType = SegmentType.短路;
                        break;
                    case 2:
                        seg.segmentType = SegmentType.光敏;
                        break;
                    case 3:
                        seg.segmentType = SegmentType.电阻;
                        break;
                    case 4:
                        seg.segmentType = SegmentType.稳压管;
                        break;
                    default:
                        break;
                }
                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
            }

        }
        void ExecuteMouseDoubleClickCommand(object obj)
        {
            Segment seg = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.FirstOrDefault(t => t.Id == index);
            if (seg != null)
            {
                switch ((int)obj)
                {
                    case 0:
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("Id", seg.Id);
                            _dialogService.ShowDialog("OpenPropertyDialog", param, arg => {
                                if (arg.Result == ButtonResult.Yes)
                                {
                                    double PropUpLimit = arg.Parameters.GetValue<double>("PropUpLimit");
                                    double PropDownLimit = arg.Parameters.GetValue<double>("PropDownLimit");
                                    seg.openProperty.PropUpLimit = PropUpLimit;
                                    seg.openProperty.PropDownLimit = PropDownLimit;
                                    _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                }
                            });
                        }
                        break;
                    case 1:
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("Id", seg.Id);
                            _dialogService.ShowDialog("ShortPropertyDialog", param, arg => {
                                if (arg.Result == ButtonResult.Yes)
                                {
                                    double PropUpLimit = arg.Parameters.GetValue<double>("PropUpLimit");
                                    double PropDownLimit = arg.Parameters.GetValue<double>("PropDownLimit");
                                    seg.shortProperty.PropUpLimit = PropUpLimit;
                                    seg.shortProperty.PropDownLimit = PropDownLimit;
                                    _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                }
                            });
                        }
                        break;
                    case 2:
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("Id", seg.Id);
                            _dialogService.ShowDialog("RgPropertyDialog", param, arg => {
                                if (arg.Result == ButtonResult.Yes)
                                {
                                    double PropUpLimit = arg.Parameters.GetValue<double>("PropUpLimit");
                                    double PropDownLimit = arg.Parameters.GetValue<double>("PropDownLimit");
                                    seg.rgProperty.PropUpLimit = PropUpLimit;
                                    seg.rgProperty.PropDownLimit = PropDownLimit;
                                    _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                }
                            });
                        }
                        break;
                    case 3:
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("Id", seg.Id);
                            _dialogService.ShowDialog("ResPropertyDialog", param, arg => {
                                if (arg.Result == ButtonResult.Yes)
                                {
                                    double PropUpLimit = arg.Parameters.GetValue<double>("PropUpLimit");
                                    double PropDownLimit = arg.Parameters.GetValue<double>("PropDownLimit");
                                    seg.resProperty.PropUpLimit = PropUpLimit;
                                    seg.resProperty.PropDownLimit = PropDownLimit;
                                    _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                }
                            });
                        }
                        break;
                    case 4:
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("Id", seg.Id);
                            _dialogService.ShowDialog("TvsPropertyDialog", param, arg => {
                                if (arg.Result == ButtonResult.Yes)
                                {
                                    double PropUpLimit = arg.Parameters.GetValue<double>("PropUpLimit");
                                    double PropDownLimit = arg.Parameters.GetValue<double>("PropDownLimit");
                                    seg.tvsProperty.PropUpLimit = PropUpLimit;
                                    seg.tvsProperty.PropDownLimit = PropDownLimit;
                                    _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                }
                            });
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            index = navigationContext.Parameters.GetValue<int>("Index");
            Segment seg = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.FirstOrDefault(t => t.Id == index);
            if (seg != null)
            {
                switch (seg.segmentType)
                {
                    case SegmentType.开路:
                        TestSegmentItems[0].Select = true;
                        break;
                    case SegmentType.短路:
                        TestSegmentItems[1].Select = true;
                        break;
                    case SegmentType.光敏:
                        TestSegmentItems[2].Select = true;
                        break;
                    case SegmentType.电阻:
                        TestSegmentItems[3].Select = true;
                        break;
                    case SegmentType.稳压管:
                        TestSegmentItems[4].Select = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
        public SegmentDetailViewModel(IParameterService parameterService, IDialogService dialogService)
        {
            _parameterService = parameterService;
            _dialogService = dialogService;
            TestSegmentItems.Add(new TestSegmentItem() { Id = 0, Select = false, Name = "开路" });
            TestSegmentItems.Add(new TestSegmentItem() { Id = 1, Select = false, Name = "短路" });
            TestSegmentItems.Add(new TestSegmentItem() { Id = 2, Select = false, Name = "光敏" });
            TestSegmentItems.Add(new TestSegmentItem() { Id = 3, Select = false, Name = "电阻" });
            TestSegmentItems.Add(new TestSegmentItem() { Id = 4, Select = false, Name = "稳压管" });
        }
    }
    public class TestSegmentItem : BindableBase
    {
        public int Id { get; set; }
        private bool select;
        public bool Select
        {
            get { return select; }
            set { SetProperty(ref select, value); }
        }
        public string Name { get; set; }
    }
}
