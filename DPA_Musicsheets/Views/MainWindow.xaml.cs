using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Commands;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
		private ICommandListener _commandListener;

        public MainWindow()
        {
            InitializeComponent();
			_commandListener = (DataContext as MainViewModel).CommandListener;
        }

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			_commandListener.Handle();
		}
    }
}
