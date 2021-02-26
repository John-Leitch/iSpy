using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.codecs
{
    internal class MicrosoftAdpcmChatCodec : AcmChatCodec
    {
        public MicrosoftAdpcmChatCodec()
            : base(new WaveFormat(8000, 16, 1), new AdpcmWaveFormat(8000, 1))
        {
        }

        public override string Name => "Microsoft ADPCM";
    }
}
