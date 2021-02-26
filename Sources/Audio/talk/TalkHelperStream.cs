using NAudio.Wave;
using System;

namespace iSpyApplication.Sources.Audio.talk
{
    internal class TalkHelperStream : WaveStream
    {
        private readonly WaveFormat format;
        private long position = 0;
        private readonly long length;
        private readonly byte[] _buffer;

        public TalkHelperStream(byte[] src, long length, WaveFormat format)
        {
            this.format = format;
            this.length = length;
            _buffer = src;
        }

        public override WaveFormat WaveFormat => format;

        public override long Length => length;

        public override long Position
        {
            get => position;
            set => position = value;
        }

        public override int Read(byte[] dest, int offset, int count)
        {
            if (position >= length)
            {
                return 0;
            }
            count = (int)Math.Min(count, length - position);

            Buffer.BlockCopy(_buffer, (int)position, dest, offset, count);
            position += count;
            return count;
        }
    }
}
