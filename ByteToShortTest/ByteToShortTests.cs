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

        [TestCase(1234)]
        [TestCase(-1234)]
        [TestCase(Int16.MaxValue)]
        [TestCase(Int16.MinValue)]
        public void SanityCheck(short inSample)
        {
            var b = BitConverter.GetBytes(inSample);
            var outSample = (short) (b[0] | b[1] << 8);
            Assert.AreEqual(inSample, outSample);
        }


        [Test]
        public void BitManipulatorReturnsSamePeaksAsBitConverter()
        {
            var bitConverter = new BitConverterPeakFinder();
            var bitManipulator = new BitManipulationPeakFinder();
            CompareFinders(bitConverter, bitManipulator);
        }


        [Test]
        public void BlockCopyReturnsSamePeaksAsBitConverter()
        {
            var bitConverter = new BitConverterPeakFinder();
            var blockCopy = new BlockCopyPeakFinder();
            CompareFinders(bitConverter, blockCopy);
        }

        [Test]
        public void WaveBufferReturnsSamePeaksAsBitConverter()
        {
            var bitConverter = new BitConverterPeakFinder();
            var waveBuffer = new WaveBufferPeakFinder();
            CompareFinders(bitConverter, waveBuffer);
        }

        [Test]
        public void UnsafeBufferReturnsSamePeaksAsBitConverter()
        {
            var bitConverter = new BitConverterPeakFinder();
            var unsafeBuffer = new UnsafePeakFinder();
            CompareFinders(bitConverter, unsafeBuffer);
        }

        private void CompareFinders(IPeakFinder firstFinder, IPeakFinder secondFinder)
        {
            var readBuffer = new byte[reader.WaveFormat.AverageBytesPerSecond * 4];
            var read = reader.Read(readBuffer, 0, readBuffer.Length);
            var samplesPerPeak = 44100 / 10;
            var firstPeaks = firstFinder.FindPeaks(readBuffer, read, samplesPerPeak);
            var secondPeaks = secondFinder.FindPeaks(readBuffer, read, samplesPerPeak);
            Assert.AreEqual(firstPeaks, secondPeaks, String.Format("peaks from {0} don't match peaks from {1}", firstFinder.GetType().Name, secondFinder.GetType().Name));
        }

        [Test]
        public void TestBitConverter()
        {
            GeneratePeaks(4, new BitConverterPeakFinder());
        }

        [Test]
        public void TestBitManipulation()
        {
            GeneratePeaks(4, new BitManipulationPeakFinder());
        }

        [Test]
        public void TestBlockCopy()
        {
            GeneratePeaks(4, new BlockCopyPeakFinder());
        }

        [Test]
        public void TestWaveBuffer()
        {
            GeneratePeaks(4, new WaveBufferPeakFinder());
        }

        [Test]
        public void TestUnsafe()
        {
            GeneratePeaks(4, new UnsafePeakFinder());
        }

        private void GeneratePeaks(int secondsPerRead, IPeakFinder peakFinder)
        {
            var readBuffer = new byte[reader.WaveFormat.AverageBytesPerSecond*secondsPerRead];
            var stopWatch = new Stopwatch();
            while(true)
            {
                var read = reader.Read(readBuffer, 0, readBuffer.Length);
                if (read == 0)
                    break;
                stopWatch.Start();
                peakFinder.FindPeaks(readBuffer, read, 44100/10);
                stopWatch.Stop();
            }
            Console.WriteLine("{0} took {1}ms", peakFinder.GetType().Name, stopWatch.ElapsedMilliseconds);
        }
    }
}
