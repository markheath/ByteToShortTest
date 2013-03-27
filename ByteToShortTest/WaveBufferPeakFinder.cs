using System;
using NAudio.Wave;

namespace ByteToShortTest
{
    public class WaveBufferPeakFinder : IPeakFinder
    {
        private short[] peakBuffer; // only create one to avoid garbage collection
        private WaveBuffer waveBuffer;

        public short[] FindPeaks(byte[] samples, int bytes, int samplesPerPeak)
        {
            if (peakBuffer == null) peakBuffer = new short[bytes / (2 * samplesPerPeak) + 1];
            if (waveBuffer == null) waveBuffer = new WaveBuffer(samples); // nb assumes samples is always the same
            var peakOffset = 0;
            var inputSamples = bytes / 2;
            short currentMax = 0;
            var sample = 0;
            while (sample < inputSamples)
            {
                currentMax = Math.Max(waveBuffer.ShortBuffer[sample], currentMax);
                sample++;
                if (sample % samplesPerPeak == 0)
                {
                    peakBuffer[peakOffset++] = currentMax;
                    currentMax = 0;
                }
            }
            return peakBuffer;
        }
    }
}