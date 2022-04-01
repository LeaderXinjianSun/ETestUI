using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETestUI.Test.ViewModels;
using ETestUI.Test.Views;

namespace ETestUI.Test
{
    public class TestUIModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TestMainView, TestMainViewModel>();
        }

    }
}
