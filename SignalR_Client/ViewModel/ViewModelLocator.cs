/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SignalR_Client"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
//using Microsoft.Practices.ServiceLocation;

namespace SignalR_Client.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MyDataListViewModel>();
            SimpleIoc.Default.Register<MyDataSubListViewModel>();
            SimpleIoc.Default.Register<MyDataUnitViewModel>();
            SimpleIoc.Default.Register<MyDataCustomViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MyDataListViewModel MyDataList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyDataListViewModel>();
            }
        }

        public MyDataSubListViewModel MyDataSubList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyDataSubListViewModel>();
            }
        }

        public MyDataUnitViewModel MyDataUnit
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyDataUnitViewModel>();
            }
        }

        public MyDataCustomViewModel MyDataCustom
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyDataCustomViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}