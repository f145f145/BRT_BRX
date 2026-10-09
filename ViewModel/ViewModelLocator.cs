
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace BRX.ViewModel
{
    /// <summary>
    /// ViewModel注册
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //注册各ViewModel
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoadingViewModel>();
            SimpleIoc.Default.Register<CalViewModel>();

        }

        /// <summary>
        /// 主ViewModel
        /// </summary>
        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// 校准ViewModel。由主窗口创建并返回
        /// </summary>
        public CalViewModel CalVM
        {
            get
            {
                return MainVM.GetCalViewModel(); 
            }
        }

        /// <summary>
        /// LoadingViewModel
        /// </summary>
        public LoadingViewModel LoadingVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoadingViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}