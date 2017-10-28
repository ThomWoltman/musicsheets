using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
		public LilypondViewerCtrl()
		{
			InitializeComponent();
			((LilypondViewModel)DataContext).TextBox = this;
		}

		public void Insert(string text)
		{
			LilyTextBox.Text = LilyTextBox.Text.Insert(LilyTextBox.CaretIndex, text);
		}
	}
}
