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
    public class OracleTest : DbTest
    {
        public OracleTest(ITestOutputHelper output) : base(output)
		{
		}
        [Fact]
        public void TestOracleOnCreatingGuid()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame");
            var oracle = new Oracle();
            var id = oracle.Id;
            id.ShouldBeOfType<Guid>();
        }
        [Fact]
        public void TestStartOracle()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame");
            var oracle = new Oracle();
            oracle.Start();
            oracle.ImageProcessor.ShouldNotBe(null);
            oracle.ImagePath.ShouldNotBe(null);
            oracle.Label.ShouldNotBe(null);
            oracle.PartialIndex.ShouldNotBe(null);
        }
        [Fact]
        public void TestFindNumberOfSlices()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\ImageGuessingGame");
            var oracle = new Oracle();
            DirectoryInfo dir;
            dir = new DirectoryInfo(@".\data\ILSVRC2012_val_00000086_scattered");
            var NumberOfSlices = oracle.FindNumberOfSlices(dir);
            NumberOfSlices.ShouldBe(49);
        }
    }
}
