using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    internal class GridGame
    {
        private Grid[,] grid = new Grid[10,10];
        private string currentPicture;

        public GridGame()
        {
            string[] collectionOfPictures = Directory.GetFiles("/pictures/");

        }

        private class Grid
        {
            bool HasBeenGuessed = false;
            bool IsTarget = false;
            Int64 guessedBy;
        }
    }
}
