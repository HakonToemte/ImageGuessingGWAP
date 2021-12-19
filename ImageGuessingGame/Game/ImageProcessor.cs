using System.Runtime.InteropServices;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FortuneVoronoi;

namespace ImageGuessingGame.GameContext
{
    public class ImageProcessor
    {
        public ImageProcessor()
        {
            
        }
        //ShowPartialImage tar inn en bildemappe og en array med indekser som den skal vise
        public void ShowPartialImage(string imagepath,int[] array){ 
            var imageSliceList = GetImageSlices(imagepath);
            var displayedSliceList = new List<Image<Rgba32>>();
            foreach (int index in array){
                displayedSliceList.Add(imageSliceList[index]);
            }
            Merge_slices(displayedSliceList);
        }
        public IList<Image<Rgba32>> GetImageSlices(string imagepath){
            DirectoryInfo dir;
            var os = Environment.OSVersion;
            if ((int)os.Platform == 4)
            {
                dir = new DirectoryInfo($@"./data/{imagepath}");
            }
            else
            {
                dir = new DirectoryInfo($@".\data\{imagepath}");
            }
            IList<Image<Rgba32>> liste = new List<Image<Rgba32>>();
            var width = 600; // Alle bildene blir like store.
            var height = 400;
            var ordered = dir.GetFiles().OrderBy(f => f.Name.Length).ToList();  // Sorts the indices in ascending order
            foreach (var file in ordered)
            {
                if (file.FullName.Contains(".png"))
                {
                    var img = Image.Load<Rgba32>(file.FullName);
                    img.Mutate(x=>x.Resize(width, height));
                    liste.Add(img);
                }
            }
            return liste;

        }
        public void ShowFullImage(string imagepath) // Hele bildet
        {
            var imageSliceList = GetImageSlices(imagepath);
            Merge_slices(imageSliceList);
        }
        public void Merge_slices(IList<Image<Rgba32>> slices)
        {
            Image<Rgba32> ex_img = slices.FirstOrDefault();
            Image<Rgba32> outputImage = new Image<Rgba32>(ex_img.Width, ex_img.Height); // create output image of the correct dimensions

            // take the all the image_slices and draw them onto outputImage at the same point.
            outputImage.Mutate(o =>
            {
                foreach (Image<Rgba32> image_slice in slices)
                {
                    o.DrawImage(image_slice, new Point(0, 0), 1f);
                }
            });
            outputImage.Save("wwwroot/output.png");
            ex_img.Dispose();
            outputImage.Dispose();
            return;
        }
        public string GetLabel(string image_folder)
        {
            return GetLabelForImage(image_folder);
        }
        public void AutomaticSliceVoronoi(string pathforImage, string pathforSlices){
            var points = 5;
            using (Image<Rgba32> image = Image.Load<Rgba32>(pathforImage))
            {
                var voronoiseeds = RandomPoints(5, image.Width, image.Height);
            }
        }
        private List<Point> RandomPoints(int amountOfPoints, int width, int height){
            Random r = new Random();
            var list = new List<Point>();
            for (var i = 0; i < amountOfPoints; i++){
                var intx = r.Next(0, width);
                var inty = r.Next(0, height);
                list.Add(new Point(intx,inty));
            }
            return list;
        }
        public void AutomaticSlice(string pathforImage, string pathforSlices){
            var counter = 0;
            var rows = 5;
            var columns =5 ;
            using (Image<Rgba32> image = Image.Load<Rgba32>(pathforImage))
            {
                image.Mutate(x=>x.Resize(600, 400));
                for (var i=0;i<rows;i++){
                    for (var j=0;j<columns;j++){
                        Image<Rgba32> slice = new Image<Rgba32>(image.Width, image.Height);
                        var x=i*image.Width/rows;
                        var y=j*image.Height/columns;
                        var width = (i+1)*image.Width/rows;
                        var height = (j+1)*image.Height/columns;
                        var crop = image.Clone(ima=>ima.Crop(Rectangle.FromLTRB(x,y,width,height)));
                        slice.Mutate(s=>s.DrawImage(crop,new Point(x, y), 1f));
                        slice.Save($"{pathforSlices}/{counter}.png");
                        counter++;
                    }
                }
            }
        }
        public int Find_slice_index(string imagepath, int xcor, int ycor){
            var imageSliceList = GetImageSlices(imagepath);
            var suggestedindex = -1;
            var counter = 0;
            foreach (var imageslice in imageSliceList){
                Span<Rgba32> row = imageslice.GetPixelRowSpan(ycor);
                Rgba32 pixel = row[xcor];
                if (pixel.A != 0)
                {
                    suggestedindex = counter;
                }
                
                counter ++;
            }
            return suggestedindex;
        }
        private string GetLabelForImage(string directory)
        {
            var os = Environment.OSVersion;
            string fileName_forMapping;
            string imageMappingPath = imageMappingPath = @".\data\image_mapping.csv";
            if ((int)os.Platform == 4)
            {
                imageMappingPath = imageMappingPath.Replace(@"\", "/");
            }
            if (directory.Contains("_scattered")){
                fileName_forMapping = directory[0..^10];
            }
            else{
                fileName_forMapping = directory;
            }
            string number_forLabelMapping = "-1";
            string[] lines = System.IO.File.ReadAllLines(imageMappingPath);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                foreach (string column in columns)
                {
                    string[] items = column.Split(" ");
                    if (items[0] == fileName_forMapping) // Finds a match
                    {
                        number_forLabelMapping = items[1];
                    }
                }
            }
            if (number_forLabelMapping != "-1") // If we found a match
            {
                return GetLabelForNumber(number_forLabelMapping);
                
            }
            else{return null;}
        }
        private string GetLabelForNumber(string numberToMap)
        {
            var os = Environment.OSVersion;
            string imageMappingPath = @".\data\label_mapping.csv";
            if ((int)os.Platform == 4)
            {
                imageMappingPath = imageMappingPath.Replace(@"\", "/");
            }
            string label = "unkn";
            string[] lines = System.IO.File.ReadAllLines(imageMappingPath);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                foreach (string column in columns)
                {
                    string[] items = column.Split(" ");
                    if (items[0] == numberToMap) // Finds a match
                    {
                        label = string.Join(" ", items[1..^0]);
                    }
                }
            }
            if (label != "unkn") // If we found a match
            {
                return label;
            }
            else{return null;}
        }
    }
}