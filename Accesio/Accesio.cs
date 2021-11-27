using AccesIO.Assets;
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

    public class AccesioEthernet
    {

        public socketClient xclient = null;
        public List<relayInfo> relays = new List<relayInfo>();
        public event EventHandler<relayInfo> relayChangedStatus;
        private Timer statusLoop;


        //=========================================================================================
        private byte[] readAllInfo = new byte[] { 0x06, 0x52, 0x53, 0x74, 0x61, 0x01, 0x01 };
        private byte[] checkOnline = new byte[] { 0x0d, 0x43, 0x68, 0x49, 0x4f, 0x06, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x01, 0xff };
        private byte[] readAllData = new byte[] { 0x04, 0x52, 0x41, 0x44, 0x49 };


        //=========================================================================================
        private byte[] Pin0on()
        {
            return new byte[] { 0x11, 0x57, 0x50, 0x44, 0x4f, 0x0c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }
        //=========================================================================================
        private byte[] Pin0off()
        {
            return new byte[] { 0x11, 0x57, 0x50, 0x44, 0x4f, 0x0c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }
        //=========================================================================================
        private byte[] ControlRelay(relay relay, relayState change)
        {
            return new byte[] { 0x11, 0x57, 0x50, 0x44, 0x4f, 0x0c, (byte)relay, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)change, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        //=========================================================================================
        private int GetReply(byte[] message)
        {
          return message[1] + message[2] + message[3] + message[4];
        }

        //=========================================================================================
        public string ChangeRelay(relay relay,relayState Change)
        {

//            xclient.Send(ControlRelay(relay, Change));
//            xclient.sendDone.WaitOne();
//            xclient.Receive();
//            xclient.receiveDone.WaitOne();


//            byte[] xx = Encoding.ASCII.GetBytes(xclient.response);

//            if (GetReply(xclient.response) == ((int)replies.R_OK))
//            {

//                foreach (Byte c in xx)
//                {

//                    Console.WriteLine(c + "| {0:X}", c);

//                }


//            }
//            else
//            {


//#if DEBUG
//                Console.WriteLine("Reading Failed");
//#endif




//            }




            return "nope";
        }
        //=========================================================================================
        public string Pinoff()
        {

//            xclient.Send(Pin0off());
//            xclient.sendDone.WaitOne();
//            xclient.Receive();
//            xclient.receiveDone.WaitOne();


//            byte[] xx = Encoding.ASCII.GetBytes(xclient.response);

//            if (GetReply(xclient.response) == ((int)replies.R_OK))
//            {

//                foreach (Byte c in xx)
//                {

//                    Console.WriteLine(c + "| {0:X}", c);

//                }


//            }
//            else
//            {


//#if DEBUG
//                Console.WriteLine("Reading Failed");
//#endif




//            }




            return "none";
        }
        //=========================================================================================
        public string Pinon()
        {

//            xclient.Send(Pin0on());
//            xclient.sendDone.WaitOne();
//            xclient.Receive();
//            xclient.receiveDone.WaitOne();


//            byte[] xx = Encoding.ASCII.GetBytes(xclient.response);

//            if (GetReply(xclient.response) == ((int)replies.R_OK))
//            {

//                foreach (Byte c in xx)
//                {

//                    Console.WriteLine(c + "| {0:X}", c);

//                }


//            }
//            else
//            {


//#if DEBUG
//                Console.WriteLine("Reading Failed");
//#endif




//            }




            return "none";
        }
        //=========================================================================================
        public void GetAllData()
        {

            xclient.Send(readAllData);
            xclient.sendDone.WaitOne();
            xclient.Receive();
            xclient.receiveDone.WaitOne();

            if(GetReply(xclient.returnBuffer) == (int)replies.R_OK)
            {

                byte[] xx = xclient.returnBuffer;
                Byte[] pData = new Byte[] { xx[6], xx[7], xx[8], xx[9] };
                for (int i = 0; i < 32; i++)
                {
                    if ((pData[i / 8] & (1 << (i % 8))) != 0)  // looks complicated but efficiet | borrowed from acessio example
                    {
                        processUpdateData(i, relayState.OFF);
                    }
                    else
                    {
                        processUpdateData(i, relayState.ON);
                    }

                }

            }
            else
            {

                Console.WriteLine("error Reading status");

            }
    

        }

        //=========================================================================================
        public string GetDeviceInfo()
        {

            xclient.Send(readAllInfo);
            xclient.sendDone.WaitOne();
            xclient.Receive();
            xclient.receiveDone.WaitOne();

    
            ////byte[] xx = Encoding.ASCII.GetBytes(xclient.response);

            //if (GetReply(xclient.response) == ((int)replies.R_OK))
            //{

            //    foreach (Byte c in xx)
            //    {

            //        Console.WriteLine(c+"| {0:X}", c);

            //    }


            //}
            //else
            //{


            //    #if DEBUG
            //             Console.WriteLine("Reading Failed");
            //    #endif




            //}




            return "ok";
        }
        //=========================================================================================
        private void processUpdateData(int num,relayState status)
        {

            if (relays.Where(x => x.num == num).Any())
            {

                if (relays.Where(x => x.num == num).Single().status != status)
                {
                    relays.Where(x => x.num == num).Single().status = status;
                    relayChangedStatus(this, relays.Where(x => x.num == num).Single());
                }

            }
            else
            {
                relays.Add(new relayInfo { num = num, status = status });
            }


        }
        //=========================================================================================
        private void doStatusUpdate(object state)
        {
            GetAllData();
        }


        //=========================================================================================
        public void StartCommunication(string ip = "192.168.1.174", int port = 51936)
        {
            if (xclient == null) 
            {
                xclient = new socketClient();
                xclient.StartClient(ip,port);
                statusLoop = new Timer(doStatusUpdate, null, 0, 500);
            }
        }

        //=========================================================================================
        public void StopCommunication()
        {
            if (xclient != null)
            {
                xclient.StopClient();
                statusLoop.Dispose();
            }

        }

    }
}