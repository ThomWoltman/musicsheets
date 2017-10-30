using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Shortcuts;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
		private IShortcutListener _shortcutListener;

        public MainWindow()
        {
            InitializeComponent();
			_shortcutListener = (DataContext as MainViewModel).ShortcutListener;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			e.Handled = _shortcutListener.Listen();
		}
    }
}
