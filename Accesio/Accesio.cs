using AccesIO.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccesIO
{

    public class AccesioEthernet
    {

        public socketClient xclient = null;
        public List<pointInfo> points = new List<pointInfo>();
        public event EventHandler<pointInfo> pointChangedStatus;
        private Timer statusLoop;
        private Byte[] pData = new byte[] { 0, 0, 0, 0, 0, 0 };

        //=========================================================================================
        private byte[] ControlRelay(Int32 i, pointState change)
        {
  
            Int32 iOutMask = 0x0000; 
            iOutMask = ((pData[i / 8]) & (1 << (i % 8))); // get single bit
            return new byte[] { 0x11, 0x57, 0x50, 0x44, 0x4f, 0x0c, (byte)iOutMask, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)change, 0x00, 0x00, 0x00, 0x00, 0x00 };

        }

        //=========================================================================================
        private int GetReply(byte[] message)
        {
          return message[1] + message[2] + message[3] + message[4];
        }

        //=========================================================================================
        public async Task<bool> ChangeRelay(Int32 i, pointState Change)
        {

            xclient.Send(ControlRelay(i, Change));
            xclient.sendDone.WaitOne();
            xclient.Receive();
            xclient.receiveDone.WaitOne();

            bool xStat = false;
            if (GetReply(xclient.returnBuffer) == (int)replies.R_OK)
            {
                xStat = true;
            }
            else
            {
                xStat = false;
            }

            return await Task.FromResult(xStat);
        }

        //=========================================================================================
        public async Task<pointInfo> GetInputInfo(int num)
        {
            var pointdata = points.Where(x => x.num == num && x.type == pointType.Input).SingleOrDefault();
            return await Task.FromResult(pointdata);
        }

        //=========================================================================================
        public async Task<pointInfo> GetRelayInfo(int num)
        {
            var pointdata = points.Where(x => x.num == num && x.type == pointType.Relay).SingleOrDefault();
            return await Task.FromResult(pointdata);
        }

        //=========================================================================================
        public void GetAllData()
        {

                byte[] readAllData = new byte[] { 0x04, 0x52, 0x41, 0x44, 0x49 };
                xclient.Send(readAllData);
                xclient.sendDone.WaitOne();
                xclient.Receive();
                xclient.receiveDone.WaitOne();

                if (GetReply(xclient.returnBuffer) == (int)replies.R_OK)
                {

                    byte[] xx = xclient.returnBuffer;
                    pData = new Byte[] { xx[6], xx[7], xx[8], xx[9] };
                    for (int i = 0; i < 32; i++)
                    {
                        if ((pData[i / 8] & (1 << (i % 8))) != 0)  // looks complicated but efficiet | borrowed from acessio example
                        {
                            processUpdateData(i, pointState.OFF);
                        }
                        else
                        {
                            processUpdateData(i, pointState.ON);
                        }

                    }

                }
           

        }

        //=========================================================================================
        private void processUpdateData(int num,pointState status)
        {
            pointType xType;
            if(num >= 16)
            {
                xType = pointType.Input;
                num = num - 16;
                if (status == pointState.ON) // just make it more readable 
                {
                    status = pointState.OFF;
                }
                else
                {
                    status = pointState.ON;
                }
            }
            else
            {
                xType = pointType.Relay;
            }

            if (points.Where(x => x.num == num && x.type == xType).Any())
            {

                if (points.Where(x => x.num == num && x.type == xType).Single().status != status)
                {
                    points.Where(x => x.num == num && x.type == xType).Single().status = status;
                    pointChangedStatus(this, points.Where(x => x.num == num && x.type == xType).Single());
                }

            }
            else
            {
                points.Add(new pointInfo { num = num, status = status,type=xType });
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