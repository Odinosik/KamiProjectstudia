using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MemoryGame.Models
{
    public class PictureModel
    {
        public int Id { get; set; }
        public string ImageSource { get; set; }

        //Slide status
        private bool _isViewed { get; set; }
        private bool _isMatched { get; set; }
        private bool _isFailed { get; set; }

    }
}
