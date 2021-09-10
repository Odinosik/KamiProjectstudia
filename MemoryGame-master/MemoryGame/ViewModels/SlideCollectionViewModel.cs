using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    public class SlideCollectionViewModel : ObservableObject
    {
        public ObservableCollection<PictureViewModel> MemorySlides { get; private set; }

        private PictureViewModel SelectedSlide1;
        private PictureViewModel SelectedSlide2;

        private DispatcherTimer _peekTimer;
        private DispatcherTimer _openingTimer;

        private const int _peekSeconds = 1;
        private const int _openSeconds = 4;

        public bool areSlidesActive
        {
            get => ((SelectedSlide1 == null || SelectedSlide2 == null)) ? true : false;
        }

        public bool AllSlidesMatched
        {
            get
            {
                foreach(var slide in MemorySlides)
                {
                    if (!slide.isMatched)
                        return false;
                }
                return true;
            }
        }

        //Can user select a slide
        public bool canSelect { get; private set; }


        public SlideCollectionViewModel()
        {
            _peekTimer = new DispatcherTimer();
            _peekTimer.Interval = new TimeSpan(0, 0, _peekSeconds);
            _peekTimer.Tick += PeekTimer_Tick;

            _openingTimer = new DispatcherTimer();
            _openingTimer.Interval = new TimeSpan(0, 0, _openSeconds);
            _openingTimer.Tick += OpeningTimer_Tick;
        }

        //Create slides from images in file directory
        public void CreateSlides(string imagesPath)
        {
            //New list of slides
            MemorySlides = new ObservableCollection<PictureViewModel>();
            var models = GetModelsFrom(@imagesPath);

            //Create slides with matching pairs from models
            for (int i = 0; i < 10; i++)
            {
                //Create 2 matching slides
                var newSlide = new PictureViewModel(models[i]);
                var newSlideMatch = new PictureViewModel(models[i]);
                //Add new slides to collection
                MemorySlides.Add(newSlide);
                MemorySlides.Add(newSlideMatch);
                //Initially display images for user
                newSlide.PeekAtImage();
                newSlideMatch.PeekAtImage();
            }

            ShuffleSlides();
            OnPropertyChanged("MemorySlides");
        }

        //Select a slide to be matched
        public void SelectSlide(PictureViewModel slide)
        {
            slide.PeekAtImage();

            if (SelectedSlide1 == null )
            {
                SelectedSlide1 = slide;
            }
            else if (SelectedSlide2 == null)
            {
                SelectedSlide2 = slide;
                HideUnmatched();
            }
            OnPropertyChanged("areSlidesActive");
        }

        //Are the selected slides a match
        public bool CheckIfMatched()
        {
            if (SelectedSlide1.Id == SelectedSlide2.Id)
            {
                MatchCorrect();
                return true;
            }

            MatchFailed();
            return false;
        }

        //Selected slides did not match
        private void MatchFailed()
        {
            SelectedSlide1.MarkFailed();
            SelectedSlide2.MarkFailed();
            ClearSelected();
        }

        //Selected slides matched
        private void MatchCorrect()
        {
            SelectedSlide1.MarkMatched();
            SelectedSlide2.MarkMatched();
            ClearSelected();
        }

        //Clear selected slides
        private void ClearSelected()
        {
            SelectedSlide1 = null;
            SelectedSlide2 = null;
            canSelect = false;
        }

        //Reveal all unmatched slides
        public void RevealUnmatched()
        {
            foreach(var slide in MemorySlides)
            {
                if(!slide.isMatched)
                {
                    _peekTimer.Stop();
                    slide.MarkFailed();
                    slide.PeekAtImage();
                }
            }
        }

        //Hid all slides that are unmatched
        public void HideUnmatched()
        {
            _peekTimer.Start();
        }

        //Display slides for memorizing
        public void Memorize()
        {
            _openingTimer.Start();
        }

        //Get slide picture models for creating picture views
        private List<PictureModel> GetModelsFrom(string relativePath)
        {
            var models = new List<PictureModel>();
            var images = Directory.GetFiles(@relativePath, "*.jpg", SearchOption.AllDirectories);
            var id = 0;

            foreach (string i in images)
            {
                models.Add(new PictureModel() { Id = id, ImageSource = "/MemoryGame;component/" + i });
                id++;
            }

            return models;
        }

        //Randomize the location of the slides in collection
        private void ShuffleSlides()
        {
            var rnd = new Random();

            for (int i = MemorySlides.Count-1; i >= 0; i--)
            {
                int k = rnd.Next(i);
                var temp = MemorySlides[i];
                MemorySlides[i] = MemorySlides[k];
                MemorySlides[k] = temp;
            }
        }

        //Close slides being memorized
        private void OpeningTimer_Tick(object sender, EventArgs e)
        {
            foreach (var slide in MemorySlides)
            {
                slide.ClosePeek();
                canSelect = true;
            }
            OnPropertyChanged("areSlidesActive");
            _openingTimer.Stop();
        }

        //Display selected card
        private void PeekTimer_Tick(object sender, EventArgs e)
        {
            foreach(var slide in MemorySlides)
            {
                if(!slide.isMatched)
                {
                    slide.ClosePeek();
                    canSelect = true;
                }
            }
            OnPropertyChanged("areSlidesActive");
            _peekTimer.Stop();
        }
    }
}
