using ETestUI.Common;
using Prism.Commands;
using Prism.Mvvm;
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
                    var aa =SelectedItem.ToString();
                    break;
                case "TestItem":
                    break;
                case "TestItemMember":
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 构造函数
        public ManageProjectViewModel()
        {
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
