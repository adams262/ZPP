using PZPP_Grupa5.ViewModels;
namespace PZPP_Grupa5.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
