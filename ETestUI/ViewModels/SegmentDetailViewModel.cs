using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
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
        #region 属性绑定
        private ObservableCollection<TestSegmentItem> testSegmentItems = new ObservableCollection<TestSegmentItem>();
        public ObservableCollection<TestSegmentItem> TestSegmentItems
        {
            get { return testSegmentItems; }
            set { SetProperty(ref testSegmentItems, value); }
        }
        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { SetProperty(ref selectedIndex, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand mouseDoubleClickCommand;
        public DelegateCommand MouseDoubleClickCommand =>
            mouseDoubleClickCommand ?? (mouseDoubleClickCommand = new DelegateCommand(ExecuteMouseDoubleClickCommand));
        private DelegateCommand<object> checkedCommand;
        public DelegateCommand<object> CheckedCommand =>
            checkedCommand ?? (checkedCommand = new DelegateCommand<object>(ExecuteCheckedCommand));

        void ExecuteCheckedCommand(object obj)
        {
            for (int i = 0; i < TestSegmentItems.Count; i++)
            {
                if (i != (int)obj)
                {
                    TestSegmentItems[i].Select = false;
                }
            }
        }
        void ExecuteMouseDoubleClickCommand()
        {
            var aa = SelectedIndex;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var Parameter = navigationContext.Parameters.GetValue<int>("Parameter");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion
        public SegmentDetailViewModel()
        {
            TestSegmentItems.Add(new TestSegmentItem(){ Id = 0, Select = true,Name = "开路"});
            TestSegmentItems.Add(new TestSegmentItem() { Id = 1, Select = false, Name = "短路" });
            TestSegmentItems.Add(new TestSegmentItem() { Id = 2, Select = false, Name = "LED压降" });
            TestSegmentItems.Add(new TestSegmentItem() { Id = 3, Select = false, Name = "LED亮灭" });
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
