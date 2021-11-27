using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace AccesIO
{
     public class socketClient
    {

        private ManualResetEvent connectDone = new ManualResetEvent(false);
        public ManualResetEvent sendDone = new ManualResetEvent(false);
        public ManualResetEvent receiveDone = new ManualResetEvent(false);
        public Byte[] returnBuffer = null;
        public Socket client;

        //=========================================================================================
        public void StopClient()
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        //=========================================================================================
        public void StartClient(string ip = "192.168.1.174", int port = 51936)
        {
            try
            {

                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
                client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //=========================================================================================
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //=========================================================================================
        public void Receive()
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //=========================================================================================
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {

                StateObject state = (StateObject)ar.AsyncState;
                Socket cli = state.workSocket;
                int bytesRead = cli.EndReceive(ar);
                if (bytesRead > 0)
                {
                    
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    byte[] bytes = Encoding.ASCII.GetBytes(state.sb.ToString());
                }

                receiveDone.Set();
                returnBuffer = state.buffer;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        //=========================================================================================
        public void Send( byte[] data)
        {
            client.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), client);
        }

        //=========================================================================================
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket cli = (Socket)ar.AsyncState;
                int bytesSent = cli.EndSend(ar);
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


    }
}
