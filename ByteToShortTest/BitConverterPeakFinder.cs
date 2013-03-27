using System;

namespace ByteToShortTest
{
    class BitConverterPeakFinder : IPeakFinder
    {
        private short[] peakBuffer; // only create one to avoid garbage collection

        public short[] FindPeaks(byte[] samples, int bytes, int samplesPerPeak)
        {
            if (peakBuffer == null) peakBuffer = new short[bytes/(2*samplesPerPeak) + 1];
            var peakOffset = 0;
            var inputSamples = bytes / 2;
            short currentMax = 0;
            var sample = 0;
            while (sample < inputSamples)
            {
                currentMax = Math.Max(BitConverter.ToInt16(samples, sample*2), currentMax);
                sample++;
                if (sample%samplesPerPeak == 0)
                {
                    peakBuffer[peakOffset++] = currentMax;
                    currentMax = 0;
                }
            }
            return peakBuffer;
        }
    }
}