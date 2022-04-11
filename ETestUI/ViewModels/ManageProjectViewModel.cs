using ETestUI.Common;
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
using System.Windows.Controls;

namespace ETestUI.ViewModels
{
    public class ManageProjectViewModel : BindableBase
    {        
        #region 变量
        private readonly IParameterService _parameterService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        #endregion
        #region 属性绑定
        private ObservableCollection<TestItem> testItems = new ObservableCollection<TestItem>();
        public ObservableCollection<TestItem> TestItems
        {
            get { return testItems; }
            set { SetProperty(ref testItems, value); }
        }
        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand treeViewCommand;

        public DelegateCommand TreeViewCommand =>
            treeViewCommand ?? (treeViewCommand = new DelegateCommand(ExecuteTreeViewCommand));
        #endregion
        #region 方法绑定函数
        void ExecuteTreeViewCommand()
        {
            switch (SelectedItem.GetType().Name)
            {
                case "TreeViewItem":
                    if (SelectedItem.ToString().Contains("项目信息"))
                    {
                        _regionManager.RequestNavigate("ProjectContentRegion", "ProjectInfoView");
                    }
                    else if (SelectedItem.ToString().Contains("测试数据") || SelectedItem.ToString().Contains("测试点信息"))
                    {
                        _regionManager.RequestNavigate("ProjectContentRegion", "TestPointInfoView");
                    }
                    else if (SelectedItem.ToString().Contains("测试段"))
                    {
                        _regionManager.RequestNavigate("ProjectContentRegion", "TestSegmentView");
                    }
                    break;
                case "TestItem":
                    {
                        var param = new NavigationParameters();
                        param.Add("Index", ((TestItem)SelectedItem).Index);
                        _regionManager.RequestNavigate("ProjectContentRegion", "SegmentDetailView", param);
                    }
                    break;
                case "TestItemMember":
                    {
                        var param = new NavigationParameters();
                        param.Add("Index", ((TestItemMember)SelectedItem).Index);
                        switch (((TestItemMember)SelectedItem).Name)
                        {
                            case "短路群":
                                _regionManager.RequestNavigate("ProjectContentRegion", "ShortGroupView", param);
                                break;
                            default:
                                break;
                        }
                        
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 构造函数
        public ManageProjectViewModel(IParameterService parameterService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _parameterService = parameterService;
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<MessageEvent>().Subscribe(MessageReceived);
            Reload();
        }
        #endregion
        #region 功能函数
        private void Reload()
        {
            TestItems.Clear();
            for (int i = 0; i < _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments.Count; i++)
            {
                int index = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[i].Id;
                TestItem testItem1 = new TestItem() { Name = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].Segments[i].Name, Index = index };
                testItem1.Members.Add(new TestItemMember() { Index = index, Name = "短路群" });
                testItem1.Members.Add(new TestItemMember() { Index = index, Name = "光敏表" });
                testItem1.Members.Add(new TestItemMember() { Index = index, Name = "电阻表" });
                testItem1.Members.Add(new TestItemMember() { Index = index, Name = "电压表" });
                TestItems.Add(testItem1);
            }
        }
        #endregion
        #region 事件响应函数
        private void MessageReceived(MessageItem msg)
        {
            switch (msg.Message)
            {
                case "Select":
                    Reload();
                    _regionManager.RequestNavigate("ProjectContentRegion", "ProjectInfoView");
                    break;
                case "Reload":
                    Reload();
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
