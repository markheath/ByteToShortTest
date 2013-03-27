using System;

namespace ByteToShortTest
{
    class BitManipulationPeakFinder : IPeakFinder
    {
        private short[] peakBuffer; // only create one to avoid garbage collection

        public short[] FindPeaks(byte[] samples, int bytes, int samplesPerPeak)
        {
            if (peakBuffer == null) peakBuffer = new short[bytes / (2 * samplesPerPeak) + 1];
            var peakOffset = 0;
            var inputSamples = bytes / 2;
            short currentMax = 0;
            var sample = 0;
            var inBufferIndex = 0;
            while (sample < inputSamples)
            {
                var currentSample = (short)(samples[inBufferIndex] | samples[inBufferIndex+1] << 8);
                inBufferIndex += 2;
                currentMax = Math.Max(currentSample, currentMax);
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