/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ResolverTest"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using System;

namespace CargoLampTest.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        //public static MainViewModel _main;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            //_main = new MainViewModel();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<CurrentMeterViewModel>();
            SimpleIoc.Default.Register<USB7230ViewModel>();
            SimpleIoc.Default.Register<PTC04ViewModel>();
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance","CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public HomeViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }
        public SettingViewModel Setting
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingViewModel>();
            }
        }
        public CurrentMeterViewModel VoltMeter
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CurrentMeterViewModel>();
            }
        }
        public USB7230ViewModel USB7230
        {
            get
            {
                return ServiceLocator.Current.GetInstance<USB7230ViewModel>();
            }
        }
        public PTC04ViewModel PTC04
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PTC04ViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}