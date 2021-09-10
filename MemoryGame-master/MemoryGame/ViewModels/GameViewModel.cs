using MemoryGame.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : ObservableObject
    {
   
        public SlideCollectionViewModel Slides { get; private set; }
        
        public Klasa GameInfo { get; private set; }
        

        public GameViewModel()
        {
            SetupGame();
        }

        ////Initialize game essentials
        private void SetupGame()
        {

            Slides = new SlideCollectionViewModel();
            GameInfo = new Klasa();

            //Set attempts to the maximum allowed
            GameInfo.ClearInfo();

            //Create slides from image folder then display to be memorized
            Slides.CreateSlides("../../Assets/Owoce");
            Slides.Memorize();

            //Slides have been updated
            OnPropertyChanged("Slides");
            OnPropertyChanged("GameInfo");
        }

        //Slide has been clicked
        public void ClickedSlide(object slide)
        {
            if(Slides.canSelect)
            {
                var selected = slide as PictureViewModel;
                Slides.SelectSlide(selected);
            }

            if(!Slides.areSlidesActive)
            {
                if (!Slides.CheckIfMatched())
                {
                    GameInfo.Incorrect();//Incorrect match
                }
            }
            GameStatus();
        }

        //Status of the current game
        private void GameStatus()
        {
            if(GameInfo.MatchAttempts < 1)
            {
                GameInfo.GameStatus(false);
                Slides.RevealUnmatched();
                return;
            }

            if(Slides.AllSlidesMatched)
            {
                GameInfo.GameStatus(true);
            }
        }

        //Restart game
        public void Restart()
        {
            SetupGame();
        }
    }
}
