using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Commands;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Views
{
	public interface ILilyPondTextBox
	{
		void Insert(string text);
	}

	/// <summary>
	/// Interaction logic for LilypondViewer.xaml
	/// </summary>
	public partial class LilypondViewerCtrl : UserControl, ILilyPondTextBox
	{
		private ICommandListener _commandListener;

		public LilypondViewerCtrl()
		{
			InitializeComponent();
			LilypondViewModel context = DataContext as LilypondViewModel;
			_commandListener = context.CommandListener;
			context.TextBox = this;
		}

		public void Insert(string text)
		{
			LilypondTextBox.Text = LilypondTextBox.Text.Insert(LilypondTextBox.CaretIndex, text);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			_commandListener.Handle();
		}
	}
}
