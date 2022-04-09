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

namespace ETestUI.ViewModels
{
    public class TestPointInfoViewModel : BindableBase, INavigationAware
    {
        #region 变量
        private readonly IParameterService _parameterService;
        #endregion
        #region 属性
        private ObservableCollection<TestPoint> points = new ObservableCollection<TestPoint>();
        public ObservableCollection<TestPoint> Points
        {
            get { return points; }
            set { SetProperty(ref points, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand<object> cellChangedCommand;
        public DelegateCommand<object> CellChangedCommand =>
            cellChangedCommand ?? (cellChangedCommand = new DelegateCommand<object>(ExecuteCellChangedCommand));

        void ExecuteCellChangedCommand(object index)
        {
            Console.WriteLine(Points[(int)index].Alias);
            _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[(int)index].Alias = Points[(int)index].Alias;
            _parameterService.Save(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
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
        #region 构造函数
        public TestPointInfoViewModel(IParameterService parameterService)
        {
            _parameterService = parameterService;
            for (int i = 0; i < _parameterService.MyParam.Projects[parameterService.MyParam.SelectedIndex].TestPoints.Count; i++)
            {
                Points.Add(new TestPoint()
                {
                    Index = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Index,
                    Name = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Name,
                    Alias = _parameterService.MyParam.Projects[_parameterService.MyParam.SelectedIndex].TestPoints[i].Alias
                });
            }
        }
        #endregion
    }
    public class TestPoint : BindableBase
    {
        public int Index { get; set; }
        public string Name { get; set; }
        private string alias;
        public string Alias
        {
            get { return alias; }
            set { SetProperty(ref alias, value); }
        }
    }
}
