using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region 属性
        private string title = "ETestUI";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private string version = "1.0";
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }
        #endregion
        #region 构造函数
        public MainWindowViewModel()
        {

        }
        #endregion
    }
}