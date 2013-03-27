namespace ByteToShortTest
{
    interface IPeakFinder
    {
        short[] FindPeaks(byte[] samples, int bytes, int samplesPerPeak);
    }
}