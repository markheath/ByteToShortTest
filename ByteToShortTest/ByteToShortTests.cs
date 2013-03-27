using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NAudio.Wave;
using NUnit.Framework;

namespace ByteToShortTest
{
    [TestFixture]
    public class ByteToShortTests
    {
        private Mp3FileReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new Mp3FileReader(@"D:\Audio\Music\Charlie Hall\The Bright Sadness\01 - Chainbreaker.mp3");
        }

        [TearDown]
        public void TearDown()
        {
            reader.Dispose();
        }

        [Test]
        public void TestBitConverter()
        {
            
        }

        private void GeneratePeaks(int secondsPerRead, IPeakFinder peakFinder)
        {
            var readBuffer = new byte[reader.WaveFormat.AverageBytesPerSecond*secondsPerRead];
            var stopWatch = new Stopwatch();
            while(true)
            {
                var read = reader.Read(readBuffer, 0, readBuffer.Length);
                if (read == 0)
                    return;
                stopWatch.Start();
                peakFinder.FindPeaks(readBuffer, read, 44100/10);
                stopWatch.Stop();
            }
            Console.WriteLine("{0} took {1}ms", peakFinder.GetType().Name, stopWatch.ElapsedMilliseconds);
        }
    }

    interface IPeakFinder
    {
        short[] FindPeaks(byte[] samples, int bytes, int samplesPerPeak);
    }
}
