using ETestUI.Common;
using ETestUI.Common.Models;
using ETestUI.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETestUI.ViewModels
{
    public class TestSegmentViewModel : BindableBase, INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IEventAggregator _eventAggregator;
        #endregion
        #region 属性绑定
        private string newName;
        public string NewName
        {
            get { return newName; }
            set { SetProperty(ref newName, value); }
        }
        private ObservableCollection<string> segments = new ObservableCollection<string>();
        public ObservableCollection<string> Segments
        {
            get { return segments; }
            set { SetProperty(ref segments, value); }
        }
        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { SetProperty(ref selectedIndex, value); }
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
                    if (MessageBox.Show($"添加\"{NewName}\"项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Segment segm = new Segment();
                        if (_parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Count == 0)
                        {
                            segm.Id = 0;
                        }
                        else
                            segm.Id = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Max(t => t.Id) + 1;
                        segm.Name = NewName;
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Add(segm);
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Modify = DateTime.Now;
                        _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                        Reload();
                        _eventAggregator.GetEvent<MessageEvent>().Publish(new MessageItem { Sender = this, Message = "Reload" });
                    }
                    break;
                case "1":
                    if (Segments.Count > 0)
                    {
                        if (MessageBox.Show($"删除\"{Segments[SelectedIndex]}\"项目吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            var item = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.FirstOrDefault(t => t.Id == SelectedIndex);
                            if (item != null)
                            {
                                _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Remove(item);
                                _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Modify = DateTime.Now;
                                _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                                Reload();
                                _eventAggregator.GetEvent<MessageEvent>().Publish(new MessageItem { Sender = this, Message = "Reload" });
                            }
                        }
                    }              
                    break;
                case "2":
                    if (SelectedIndex > 0)
                    {
                        int changeBox = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex].Id;
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex].Id = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex - 1].Id;
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex - 1].Id = changeBox;
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Modify = DateTime.Now;
                        _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                        Reload();
                        _eventAggregator.GetEvent<MessageEvent>().Publish(new MessageItem { Sender = this, Message = "Reload" });
                    }
                    break;
                case "3":
                    if (SelectedIndex < _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Count - 1)
                    {
                        int changeBox = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex].Id;
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex].Id = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex + 1].Id;
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[SelectedIndex + 1].Id = changeBox;
                        _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Modify = DateTime.Now;
                        _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
                        Reload();
                        _eventAggregator.GetEvent<MessageEvent>().Publish(new MessageItem { Sender = this, Message = "Reload" });
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 方法绑定函数

        #endregion
        #region 构造函数
        public TestSegmentViewModel(IParameterService parameterService, IEventAggregator eventAggregator)
        {
            _parameterService = parameterService;
            _eventAggregator = eventAggregator;
            Reload();
        }
        #endregion
        #region 功能函数
        private void Reload()
        {
            Segments.Clear();
            for (int i = 0; i < _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Count; i++)
            {
                _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Sort((p1,p2) => p1.Id.CompareTo(p2.Id));
                Segments.Add(_parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[i].Name);
            }
        }
        #endregion
        #region 导航
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
        #endregion

    }
}
