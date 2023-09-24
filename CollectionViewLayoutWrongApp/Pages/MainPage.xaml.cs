using CollectionViewLayoutWrongApp.ViewModels;
using CommunityToolkit.Maui.Views;

namespace CollectionViewLayoutWrongApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly CustomPopupViewModel customPopupViewModel;
    private Popup customPopup;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        customPopupViewModel = new CustomPopupViewModel();
    }

    protected override void OnAppearing()
    {
        Shell.SetTabBarIsVisible(this, false);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        customPopup = new CustomPopup();
        customPopup.BindingContext = customPopupViewModel;

        var mainPage = Application.Current.MainPage;
        mainPage.ShowPopupAsync(customPopup);

        AdaptPopupSizeBasedOnOrientation();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        AdaptPopupSizeBasedOnOrientation();
    }

    private void AdaptPopupSizeBasedOnOrientation()
    {
        if (customPopup == null)
        {
            return;
        }

        if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)       // Portrait
        {
            customPopup.Size = new Size(DeviceDisplay.MainDisplayInfo.Width * 0.3, DeviceDisplay.MainDisplayInfo.Height * 0.2);
        }
        else                                                                                        // Landscape
        {
            customPopup.Size = new Size(DeviceDisplay.MainDisplayInfo.Width * 0.3, DeviceDisplay.MainDisplayInfo.Height * 0.3);
        }
    }
}