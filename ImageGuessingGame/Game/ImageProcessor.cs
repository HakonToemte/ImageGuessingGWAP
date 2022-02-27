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
            var points =36;
            var width=600;
            var height=400;
            using (Image<Rgba32> image = Image.Load<Rgba32>(pathforImage))
            {
                var randompoints = RandomPoints(points, width, height);
                var voronoiseeds = randompoints.Select(x => new Voronoiseed(x));
                var slices = Voronoi(voronoiseeds, width, height);
                ImageSlice(slices, pathforImage, pathforSlices);

            }
        }
        private List<System.Drawing.Point>[] Voronoi(IEnumerable<Voronoiseed> voronoiseeds, int width, int height){
            var dict = new Dictionary<Voronoiseed, List<System.Drawing.Point>>();
            foreach (var entry in voronoiseeds){
                dict[entry]=new List<System.Drawing.Point>();
            }
            for(var x = 0; x<width;x++){
                for (var y=0; y<height;y++){
                    double min_distance = 10000;
                    Voronoiseed closestseed =null;
                    var list = new List<System.Drawing.Point>();
                    foreach(var seed in dict){
                        var distance = Math.Round(Math.Sqrt((Math.Pow(seed.Key.Point.X - x, 2) + Math.Pow(seed.Key.Point.Y - y, 2))),0);
                        if (distance < min_distance){
                            min_distance = distance;
                            closestseed = seed.Key;
                        }
                    }
                    int count=0;
                    foreach(var seed2 in dict){
                        var distance2 = Math.Round(Math.Sqrt((Math.Pow(seed2.Key.Point.X - x, 2) + Math.Pow(seed2.Key.Point.Y - y, 2))),0);
                        if (distance2 == min_distance){
                            count ++;
                        }
                    }
                    if (count ==1 ){
                    dict[closestseed].Add(new System.Drawing.Point(x,y));
                    }
                }
            }
            return dict.Values.ToArray();
        }
        private void ImageSlice(List<System.Drawing.Point>[] ArrayOfSlices, string pathforImage, string pathforSlices){
            using (Image<Rgba32> image = Image.Load<Rgba32>(pathforImage))
            {
                image.Mutate(x=>x.Resize(600, 400));
                var counter = 0;
                foreach (var pixels in ArrayOfSlices){
                    Image<Rgba32> slice = new Image<Rgba32>(image.Width, image.Height);
                    foreach(var pixel in pixels){
                        slice[pixel.X,pixel.Y] = image[pixel.X,pixel.Y];
                    }
                    counter++;
                    slice.Save($"{pathforSlices}/{counter}.png");
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
            string imageMappingPath = @".\data\image_mapping.csv";
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
                    Console.WriteLine(items[0]);
                    Console.WriteLine(filename);
                    if (items[0] == filename) // Finds a match
                    {
                        label = string.Join(" ", items[1..^0]);
                    }
                }
            }
            Console.WriteLine(label, "here");
            if (label != "unkn") // If we found a match
            {
                return label;
            }
            else{return null;}
        }
    }
    public class Voronoiseed
    {
        public Voronoiseed(System.Drawing.Point point){
            Point = point;
            if (Neighbors == null){
                Neighbors = new List<System.Drawing.Point>();
            }else{
                Console.WriteLine("ELSE");
            }
        }
        public System.Drawing.Point Point{get;set;}
        public List<System.Drawing.Point> Neighbors{get;set;}
    }
}