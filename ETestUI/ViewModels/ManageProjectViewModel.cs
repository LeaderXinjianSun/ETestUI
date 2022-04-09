using ETestUI.Common;
using ETestUI.Service;
using Prism.Commands;
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
                    var param = new NavigationParameters();
                    param.Add("Parameter", 1);
                    _regionManager.RequestNavigate("ProjectContentRegion", "SegmentDetailView", param);
                    break;
                case "TestItemMember":
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 构造函数
        public ManageProjectViewModel(IParameterService parameterService, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _parameterService = parameterService;

            TestItem testItem1 = new TestItem() { Name = "段1",Index = 0};
            testItem1.Members.Add(new TestItemMember() { Name = "网络表",Index = 0});
            testItem1.Members.Add(new TestItemMember() { Name = "开路", Index = 0 });

            TestItem testItem2 = new TestItem() { Name = "段2", Index = 1 };
            testItem2.Members.Add(new TestItemMember() { Name = "网络表", Index = 1 });
            testItem2.Members.Add(new TestItemMember() { Name = "开路", Index = 1 });

            TestItems.Add(testItem1);
            TestItems.Add(testItem2);
        }
        #endregion
    }
}
