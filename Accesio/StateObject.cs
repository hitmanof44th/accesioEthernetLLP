﻿using System.Net.Sockets;
using System.Text;


namespace AccesIO
{
    internal class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 256;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }
}
