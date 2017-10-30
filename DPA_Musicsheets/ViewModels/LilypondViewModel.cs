using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using DPA_Musicsheets.Models.Memento;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Views;
using DPA_Musicsheets.Commands;
using System.ComponentModel;
using DPA_Musicsheets.Models.Lilypondstate;

namespace DPA_Musicsheets.ViewModels
{
	public class LilypondViewModel : ViewModelBase
	{
		private FileHandler _fileHandler;
		public ICommandListener CommandListener { get; }

		private string _text;
		private string _previousText;
		private string _nextText;
		public ILilyPondTextBox TextBox { get; set; }
		private CareTaker _caretaker;
        private ILilypondstate state;

		public string LilypondText
		{
			get
			{
				return _text;
			}
			set
			{
				if (!_waitingForRender && !_textChangedByLoad && !_textChangedByCommand)
				{
                    _caretaker.Save(_text);
				}
                state = new UnSavedState();
                _text = value;
				RaisePropertyChanged(() => LilypondText);
			}
		}

		private bool _textChangedByCommand = false;
		private bool _textChangedByLoad = false;
		private DateTime _lastChange;
		private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
		private bool _waitingForRender = false;

		public LilypondViewModel(FileHandler fileHandler)
		{
            state = new SavedState();
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
            _fileHandler = fileHandler;
			CommandListener = new CommandListener();
			_caretaker = new CareTaker();

			_fileHandler.StaffChanged += (src, args) =>
			{
				_textChangedByLoad = true;
				LilypondText = new LilypondStaffConverter().Convert(args.Staff);
				_textChangedByLoad = false;
			};

			_text = "Your lilypond text will appear here.";
			InitCommands();
		}

		public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
		{
			if (!_textChangedByLoad)
			{
				_waitingForRender = true;
				_lastChange = DateTime.Now;
				MessengerInstance.Send<CurrentStateMessage>(new CurrentStateMessage() { State = "Rendering..." });

				Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
				{
					if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
					{
						_waitingForRender = false;
						UndoCommand.RaiseCanExecuteChanged();

						//set new Staffs
						var lyText = LilypondText.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
						var staff = new LilypondStaffConverter().Convert(lyText);
						if (staff != null)
						{
							_fileHandler.ChangeStaff(staff);
							UndoCommand.RaiseCanExecuteChanged();
						}
						else
						{
							MessengerInstance.Send<CurrentStateMessage>(new CurrentStateMessage() { State = "Invalid lilypond" });
						}
					}
				}, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
			}
		});

		public RelayCommand UndoCommand => new RelayCommand(() =>
		{
			var lyText = LilypondText;

			_textChangedByCommand = true;
			LilypondText = _caretaker.Undo(lyText);
			_textChangedByCommand = false;

			RedoCommand.RaiseCanExecuteChanged();
		}, () => _caretaker.CanUndo());

		public RelayCommand RedoCommand => new RelayCommand(() =>
		{
			var lyText = LilypondText;

			_textChangedByCommand = true;
			LilypondText = _caretaker.Redo(lyText);
			_textChangedByCommand = false;

			UndoCommand.RaiseCanExecuteChanged();
		}, () => _caretaker.CanRedo());

		public ICommand SaveAsCommand => new RelayCommand(() =>
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
			if (saveFileDialog.ShowDialog() == true)
			{
				if (!_fileHandler.SaveFile(saveFileDialog.FileName))
				{
					MessageBox.Show($"Extension {Path.GetExtension(saveFileDialog.FileName)} is not supported.");
				}
                else
                {
                    state = new SavedState();
                }
			}
		});

		public ICommand SaveAsLilypondCommand => new RelayCommand(() =>
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly" };
			if (saveFileDialog.ShowDialog() == true)
			{
				if (!_fileHandler.SaveFile(saveFileDialog.FileName))
				{
					MessageBox.Show($"Extension {Path.GetExtension(saveFileDialog.FileName)} is not supported.");
				}
			}
		});

		public ICommand SaveAsPDFCommand => new RelayCommand(() =>
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "PDF|*.pdf" };
			if (saveFileDialog.ShowDialog() == true)
			{
				if (!_fileHandler.SaveFile(saveFileDialog.FileName))
				{
					MessageBox.Show($"Extension {Path.GetExtension(saveFileDialog.FileName)} is not supported.");
				}
			}
		});

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            state.Handle(this);
        }

        private void InitCommands()
		{
			CommandListener.AddCommand(new Key[] { Key.LeftCtrl, Key.S }, () =>
			{
				SaveAsLilypondCommand.Execute(null);
			});
			CommandListener.AddCommand(new Key[] { Key.LeftCtrl, Key.S, Key.P }, () =>
			{
				SaveAsPDFCommand.Execute(null);
			});
			CommandListener.AddCommand(new Key[] { Key.LeftAlt, Key.C }, () =>
			{
				TextBox.InsertAtCaretIndex("\\clef treble");
			});
			CommandListener.AddCommand(new Key[] { Key.LeftAlt, Key.S }, () =>
			{
				TextBox.InsertAtCaretIndex("\\tempo 4=120");
			});
			CommandListener.AddCommand(new Key[] { Key.LeftAlt, Key.T }, () =>
			{
				TextBox.InsertAtCaretIndex("\\time 4/4");
			});
			CommandListener.AddCommand(new Key[] { Key.LeftAlt, Key.T, Key.NumPad4 }, () =>
			{
				TextBox.InsertAtCaretIndex("\\time 4/4");
			});
			CommandListener.AddCommand(new Key[] { Key.LeftAlt, Key.T, Key.NumPad3 }, () =>
			{
				TextBox.InsertAtCaretIndex("\\time 3/4");
			});
			CommandListener.AddCommand(new Key[] { Key.LeftAlt, Key.T, Key.NumPad6 }, () =>
			{
				TextBox.InsertAtCaretIndex("\\time 6/8");
			});
		}
	}
}
