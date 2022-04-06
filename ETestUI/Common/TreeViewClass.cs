using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Common
{
    public class TestItem
    {
        public TestItem()
        {
            this.Members = new ObservableCollection<TestItemMember>();
        }
        public int Index { get; set; }
        public string Name { get; set; }
        public ObservableCollection<TestItemMember> Members { get; set; }
    }
    public class TestItemMember
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }
}
