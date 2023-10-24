using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Tongue.Models;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Tongue.UserControls
{
    public sealed partial class TranslationItemControl : UserControl
    {
        public TranslationHistory HistoryItem
        {
            get => (TranslationHistory)GetValue(HistoryItemProperty);
            set => SetValue(HistoryItemProperty, value);
        }

        public static DependencyProperty HistoryItemProperty = DependencyProperty.Register(
            nameof(HistoryItem),
            typeof(TranslationHistory),
            typeof(TranslationItemControl),
            new PropertyMetadata(null)
        );

        public TranslationItemControl()
        {
            InitializeComponent();
        }

        private void Grid_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _ = VisualStateManager.GoToState(this, "HoveredState", false);
        }

        private void Grid_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _ = VisualStateManager.GoToState(this, "NonHoverState", false);
        }
    }
}
