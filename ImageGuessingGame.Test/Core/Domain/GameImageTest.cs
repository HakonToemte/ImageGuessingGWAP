using System;
using System.Linq;
using System.Threading;
using Xunit;
using ImageGuessingGame.GameContext;
using ImageGuessingGame.Test.Helpers;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using ImageGuessingGame.GameContext.GameHandling;
using System.IO;
using Shouldly;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageGuessingGame.Test
{
    public class GameImageTest : DbTest
    {
        public GameImageTest(ITestOutputHelper output) : base(output)
		{
		}
        [Fact]
        public void TestGetLabelGameImage()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame");
            var Label = "cinema";
            var imageProcessor = new ImageProcessor();
            var LabelGiven = imageProcessor.GetLabel("ILSVRC2012_val_00000086_scattered");
            Assert.Equal(Label,LabelGiven);
        }
        [Fact]  
        public void TestGetImageSlice()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame");
            var imageProcessor = new ImageProcessor();
            var ImageSlices = imageProcessor.GetImageSlices("ILSVRC2012_val_00000086_scattered");
            ImageSlices.ShouldNotBe(null);
            
        }
        [Fact]  
        public void TestingSliceMerging()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame");
            var imageProcessor = new ImageProcessor();
            var ImageSlices = imageProcessor.GetImageSlices("ILSVRC2012_val_00000086_scattered");
            imageProcessor.Merge_slices(ImageSlices);
            var lastmodified = File.GetLastWriteTime(@".\wwwroot\output.png");
            lastmodified.ShouldBe(DateTime.Now,TimeSpan.FromSeconds(1));
        }
        [Fact]
        public void TestiShowFullImage()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame");
            var imageProcessor = new ImageProcessor();
            string ImagePath = "ILSVRC2012_val_00000086_scattered";
            imageProcessor.ShowFullImage(ImagePath);
            var lastmodified = File.GetLastWriteTime(@".\wwwroot\output.png");
            lastmodified.ShouldBe(DateTime.Now,TimeSpan.FromSeconds(1));
        }
        [Fact]
        public void TestAutomaticSliceVoronoi()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame\wwwroot\uploads");
            var imageProcessor = new ImageProcessor();
            string ImagePath = "unplash.jpg";
            string pathforslices = "test";
            imageProcessor.AutomaticSliceVoronoi(ImagePath,pathforslices);
            //var lastmodified = File.GetLastWriteTime(@".\wwwroot\output.png");
        }
    }
}
