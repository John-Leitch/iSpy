﻿using System;
using System.Linq;

namespace iSpyApplication.Server
{
    public class ConnectionOption
    {
        private readonly Uri _url, _audioUrl;
        private readonly int _videoSourceTypeID, _audioSourceTypeID;
        private readonly string _overrideURL;
        public int MediaIndex;
        internal ManufacturersManufacturerUrl MmUrl;

        internal int VideoSourceTypeID => _videoSourceTypeID;

        internal int AudioSourceTypeID => _audioSourceTypeID;

        public string Source => Conf.DeviceList.First(p => p.ObjectTypeID == 2 && p.SourceTypeID == _videoSourceTypeID).Name;

        internal string AudioSource => _audioSourceTypeID == -1 ? "" : Conf.DeviceList.First(p => p.ObjectTypeID == 1 && p.SourceTypeID == _audioSourceTypeID).Name;

        internal string AudioURL => _audioUrl == null ? "" : _audioUrl.ToString();
        public string URL => _url?.ToString() ?? _overrideURL;

        public static string Status => "OK";

        public ConnectionOption(Uri url, Uri audioUrl, int videoSourceTypeID, int audioSourceTypeID, ManufacturersManufacturerUrl mmUrl)
        {
            _url = url;
            _audioUrl = audioUrl;
            _videoSourceTypeID = videoSourceTypeID;
            _audioSourceTypeID = audioSourceTypeID;
            MmUrl = mmUrl;
            _overrideURL = "";
        }

        public ConnectionOption(string url, Uri audioUrl, int videoSourceTypeID, int audioSourceTypeID, ManufacturersManufacturerUrl mmUrl)
        {
            _url = null;
            _audioUrl = audioUrl;
            _videoSourceTypeID = videoSourceTypeID;
            _audioSourceTypeID = audioSourceTypeID;
            MmUrl = mmUrl;
            _overrideURL = url;
        }

    }
}
