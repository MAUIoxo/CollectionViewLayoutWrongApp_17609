using CommunityToolkit.Maui.Views;

namespace CollectionViewLayoutWrongApp.Pages;

public partial class CustomPopup : Popup
{
    public CustomPopup()
    {
        InitializeComponent();
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OkButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}