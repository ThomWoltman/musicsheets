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

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private FileHandler _fileHandler;

        private string _text;
        private CareTaker _caretaker;

        public string LilypondText
        {
            get
            {
                return _text;
            }
            set
            {
                if (!_waitingForRender && !_textChangedByLoad &&!_textChangedByCommand)
                {
                    _caretaker.Save(_text);
                }
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
            _caretaker = new CareTaker();
            _fileHandler = fileHandler;

            _fileHandler.StaffChanged += (src, args) =>
            {
                _textChangedByLoad = true;
                LilypondText = new LilypondStaffConverter().Convert(args.Staff);
                _textChangedByLoad = false;
            };

            _text = "Your lilypond text will appear here.";
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
                        if(staff != null)
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
                if (!_fileHandler.SaveFile(saveFileDialog.FileName)){
                    MessageBox.Show($"Extension {Path.GetExtension(saveFileDialog.FileName)} is not supported.");
                }
            }
        });
    }
}
