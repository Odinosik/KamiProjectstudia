using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.ViewModels
{
    public class Klasa : ObservableObject
    {
        private const int _maxAttempts = 10;

        private int _matchAttempts { get; set; }

        private bool _gameLost { get; set; }
        private bool _gameWon { get; set; }

        public int MatchAttempts
        {
            //after
            get => _matchAttempts;

            private set
            {
                _matchAttempts = value;
                OnPropertyChanged("MatchAttempts");
            }
        }

        public Visibility LostMessage
        {
            get => _gameLost ? Visibility.Visible : Visibility.Hidden;
        }

        public Visibility WinMessage
        {
            get => _gameWon ?  Visibility.Visible : Visibility.Hidden;
        }

        public void GameStatus(bool win)
        {
            if (win)
            {
                _gameWon = true;
                OnPropertyChanged("WinMessage");

                return;
            }

            _gameLost = true;
            OnPropertyChanged("LostMessage");
        }

        public void ClearInfo()
        {
            MatchAttempts = _maxAttempts;
            _gameLost = false;
            _gameWon = false;
            OnPropertyChanged("LostMessage");
            OnPropertyChanged("WinMessage");
        }
        public void Incorrect()
        {
            MatchAttempts--;
        }
    }
}
