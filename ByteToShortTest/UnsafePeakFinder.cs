using System;

namespace ByteToShortTest
{
    public class UnsafePeakFinder : IPeakFinder
    {
        private short[] peakBuffer; // only create one to avoid garbage collection

        public unsafe short[] FindPeaks(byte[] samples, int bytes, int samplesPerPeak)
        {
            if (peakBuffer == null) peakBuffer = new short[bytes / (2 * samplesPerPeak) + 1];
            var peakOffset = 0;
            var inputSamples = bytes / 2;
            short currentMax = 0;
            var sample = 0;
            fixed (byte* pSamples = samples)
            {
                var psSamples = (short*) pSamples; // for some reason won't let us do this in one step
                while (sample < inputSamples)
                {
                    currentMax = Math.Max(psSamples[sample], currentMax);
                    sample++;
                    if (sample % samplesPerPeak == 0)
                    {
                        peakBuffer[peakOffset++] = currentMax;
                        currentMax = 0;
                    }
                }
                
            }
            return peakBuffer;
        }
    }
}