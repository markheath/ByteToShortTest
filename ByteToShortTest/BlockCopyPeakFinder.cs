using System;

namespace ByteToShortTest
{
    class BlockCopyPeakFinder : IPeakFinder
    {
        private short[] peakBuffer; // only create one to avoid garbage collection
        private short[] inBufferCopy;

        public short[] FindPeaks(byte[] samples, int bytes, int samplesPerPeak)
        {
            if (peakBuffer == null) peakBuffer = new short[bytes / (2 * samplesPerPeak) + 1];
            if (inBufferCopy == null) inBufferCopy = new short[bytes/2];
            Buffer.BlockCopy(samples,0,inBufferCopy,0,bytes);
            var peakOffset = 0;
            var inputSamples = bytes / 2;
            short currentMax = 0;
            var sample = 0;
            while (sample < inputSamples)
            {
                currentMax = Math.Max(inBufferCopy[sample], currentMax);
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