using System;

namespace iSpyApplication.Sources.Audio.talk
{
    public static class TalkHelper
    {
        public static ITalkTarget GetTalkTarget(objectsCamera cam, IAudioSource source)
        {
            switch (cam.settings.audiomodel)
            {
                default://local playback
                    return new TalkLocal(source);
                case "Foscam":
                    return new TalkFoscam(cam.settings.audioip, cam.settings.audioport,
                        cam.settings.audiousername, cam.settings.audiopassword, source);
                case "iSpyServer":
                    return new TalkiSpyServer(cam.settings.audioip, cam.settings.audioport,
                        source);
                case "NetworkKinect":
                    return new TalkNetworkKinect(cam.settings.audioip, cam.settings.audioport,
                        source);
                case "Axis":
                    return new TalkAxis(cam.settings.audioip, cam.settings.audioport,
                        cam.settings.audiousername, cam.settings.audiopassword, source);
                case "Doorbird":
                    return new TalkDoorbird(cam.settings.audioip, cam.settings.audioport,
                        cam.settings.audiousername, cam.settings.audiopassword, source);
                case "IP Webcam (Android)":
                    return new TalkIPWebcamAndroid(new Uri(cam.settings.videosourcestring), source);
                case "Amcrest":
                    return new TalkAmcrest(cam.settings.audioip, cam.settings.audioport, source);
            }
        }
    }
}
