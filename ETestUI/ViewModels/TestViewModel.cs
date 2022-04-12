using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.ViewModels
{
    public class TestViewModel : BindableBase
    {
        private int result = 1;
        public int Result
        {
            get { return result; }
            set { SetProperty(ref result, value); }
        }
        public TestViewModel()
        {

        }
    }
}
