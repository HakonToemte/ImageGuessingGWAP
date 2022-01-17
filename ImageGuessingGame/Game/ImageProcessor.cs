using System;
using System.Windows;
using System.Windows.Shapes;
using System.Drawing;
using System.Numerics;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
                    o.DrawImage(image_slice, new SixLabors.ImageSharp.Point(0, 0), 1f);
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
            var points = 10;
            using (Image<Rgba32> image = Image.Load<Rgba32>(pathforImage))
            {
                var voronoiseeds = RandomPoints(points, image.Width, image.Height);
                Voronoi(voronoiseeds, image.Width, image.Height);

            }
        }
        private void Voronoi(List<System.Drawing.Point> voronoiseeds, int width, int height){
            var dict = new Dictionary<System.Drawing.Point, System.Drawing.Point[]>();
            for(var x = 0; x<width;x++){
                for (var y=0; y<height;y++){
                    double min_distance = 10000;
                    var list = new List<System.Drawing.Point>();
                    foreach(var seed in voronoiseeds){
                        var distance = Math.Round(Math.Sqrt((Math.Pow(seed.X - x, 2) + Math.Pow(seed.Y - y, 2))),0);
                        if (distance < min_distance){
                            min_distance = distance;
                        }
                    }
                    foreach(var seed2 in voronoiseeds){
                        var distance2 = Math.Round(Math.Sqrt((Math.Pow(seed2.X - x, 2) + Math.Pow(seed2.Y - y, 2))),0);
                        if (distance2 == min_distance){
                            list.Add(seed2);
                        }
                    }
                    if (list.Count > 2){
                        dict.Add(new System.Drawing.Point(x,y),list.ToArray());
                    }
                    
                }
            }
            ComputePolygons(dict);
        }
        private void ComputePolygons(Dictionary<System.Drawing.Point,System.Drawing.Point[]> dict){
            foreach(var point_set in dict){
                Console.WriteLine("_________________");
                Console.WriteLine("key: " + point_set.Key);
                foreach(var seed in point_set.Value){
                    Console.WriteLine(seed);
                }
            }
        }
        private List<System.Drawing.Point> RandomPoints(int amountOfPoints, int width, int height){
            Random r = new Random();
            var list = new List<System.Drawing.Point>();
            for (var i = 0; i < amountOfPoints; i++){
                var intx = r.Next(0, width);
                var inty = r.Next(0, height);
                list.Add(new System.Drawing.Point(intx,inty));
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
                        var crop = image.Clone(ima=>ima.Crop(SixLabors.ImageSharp.Rectangle.FromLTRB(x,y,width,height)));
                        slice.Mutate(s=>s.DrawImage(crop,new SixLabors.ImageSharp.Point(x, y), 1f));
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
        private string GetLabelForImage(string filename)
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
                    if (items[0] == filename) // Finds a match
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