using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImageGuessingGame.GameContext
{
    public class Oracle
    {
        public Oracle()
        {
            ImageProcessor = new ImageProcessor();
        }
        public Guid Id { get; protected set; }
        public string ImagePath { get; set; }
        public int NumberOfSlices { get; set; }
        public Suggestion Suggestion{get;set;}
        public List<MultiPlayerGuess> MultiplayerGuesses { get; set; }
        public Guid GameId { get; set; }
        [NotMapped]
        public ImageProcessor ImageProcessor { get; set; }

        public string Label { get; set; }
        public List<Index> PartialIndex { get; set; }
        public void SelectTile()
        {
            if (Suggestion != null){
                var suggestion_int_list = Suggestion.IndexList.Select(i=>i.index).Distinct().ToList();
                var partial_index_int = PartialIndex.Select(x => x.index);
                for (int i = 0; i<suggestion_int_list.Count;i++){ // Puts suggestions at the start of the queue
                    if (!partial_index_int.Contains(suggestion_int_list[i])){
                        PartialIndex.Add(new GameContext.Index(suggestion_int_list[i]));
                        var partial_index_int_updated = PartialIndex.Select(x => x.index);
                        ImageProcessor.ShowPartialImage(ImagePath, partial_index_int_updated.ToArray());
                        Console.WriteLine(suggestion_int_list[i]);
                        return;
                    }
                }
            }
            var unusedIndices = GetUnusedIndices(); // Lager Ny UnusedIndicies hver gang. Som står i tilfeldig rekkefølge
            if (unusedIndices.Count() > 0)
            {
                var index = unusedIndices[0]; // Takes out the first element in our shuffled indexlist
                PartialIndex.Add(index); // adds that index to the partialindex list
                var index_int = PartialIndex.Select(x => x.index);
                ImageProcessor.ShowPartialImage(ImagePath, index_int.ToArray());
            }
        }
        private List<Index> GetUnusedIndices()
        {
            var unusedIndices = new List<Index>();
            var index_int = PartialIndex.Select(x => x.index);
            for (var i = 0; i < this.NumberOfSlices; i++)
            {
                if (!index_int.Contains(i))
                {
                    unusedIndices.Add(new Index(i));
                }
            }
            unusedIndices = unusedIndices.OrderBy(a => Guid.NewGuid()).ToList();  // Sorts the indices in random order
            return unusedIndices;
        }
        public void Start()
        {
            ImageProcessor = new ImageProcessor();
            PartialIndex = new List<Index>();
            this.FindImageFolder();
            SelectTile();
        }
        public void FindImageFolder() //Oracle finds a random image from data
        {
            DirectoryInfo dir;
            var os = Environment.OSVersion;
            if ((int)os.Platform == 4)
            {
                dir = new DirectoryInfo(@"./data");
            }
            else
            {
                dir = new DirectoryInfo(@".\data");
            }
            DirectoryInfo[] fis = dir.GetDirectories(); //Finner alle directories i data mappen. Som da er lik totalt antall bilder
            int amountOfImages = fis.Length;
            var rand = new Random();
            int randint = rand.Next(amountOfImages);
            var image_folder = fis[randint]; // Random image folder
            ImagePath = image_folder.Name;
            Label = ImageProcessor.GetLabel(ImagePath);
            Console.WriteLine(Label);
            NumberOfSlices = FindNumberOfSlices(image_folder);

        }
        public int FindNumberOfSlices(DirectoryInfo dir)
        {
            var count = 0;
            foreach (var file in dir.GetFiles())
            {
                if (file.FullName.Contains(".png"))
                {
                    count += 1;
                }
            }
            return count;
        }
    }
}