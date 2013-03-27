ByteToShortTest
===============

Test application demonstrating performance characteristics of various techniques for casting byte[] to short[] in C#.
Illustrates accompanying blog post at http://mark-dot-net.blogspot.co.uk/2013/03/how-to-convert-byte-to-short-or-float.html

The five techniques are

- BitConverter
- Bit Manipulation
- Buffer.BlockCopy
- NAudio's WaveBuffer 
- unsafe code
