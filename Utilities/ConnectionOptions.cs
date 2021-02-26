﻿using System;
using System.Net;

namespace iSpyApplication.Utilities
{
    public class ConnectionOptions
    {
        public string source, cookies, headers, userAgent, username, password, method, channel;
        public IWebProxy proxy;
        public bool useSeparateConnectionGroup, useHttp10;
        public int requestTimeout;
        public event EventHandler callback;
        public byte[] data;

        public void ExecuteCallback(bool success)
        {
            if (success)
                callback?.Invoke(this, EventArgs.Empty);
            callback = null;
        }
    }
}
