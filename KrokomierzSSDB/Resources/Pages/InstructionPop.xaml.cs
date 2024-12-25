using CommunityToolkit.Maui.Views;

namespace KrokomierzSSDB;

public partial class InstructionPop : Popup
{
    public InstructionPop()
    {
        InitializeComponent();
    }

    private void OnClose(object sender, EventArgs e)
    {
        Close(); // Close the popup
    }
}
