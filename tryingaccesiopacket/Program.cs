using System;
using System.Threading;
using AccesIO;


namespace tryingaccesiopacket
{
    class Program
    {

            static void Main(string[] args)
            {


            AccesioEthernet client = new AccesioEthernet();
            client.StartCommunication();
            client.pointChangedStatus += Client_relayChangedStatus;

            Thread.Sleep(2000);
            client.ChangeRelay(3,AccesIO.Assets.pointState.ON);
            Thread.Sleep(2000);
            client.ChangeRelay(2, AccesIO.Assets.pointState.ON);
            Thread.Sleep(2000);
            client.ChangeRelay(1, AccesIO.Assets.pointState.ON);

            Thread.Sleep(2000);
            client.ChangeRelay(6, AccesIO.Assets.pointState.ON);

            Console.WriteLine("\n Press Enter to continue...");
                Console.Read();



            }

        private static void Client_relayChangedStatus(object sender, AccesIO.Assets.pointInfo e)
        {
            Console.WriteLine(e.type+" pointchange:"+e.num+" Status:"+e.status.ToString());
        }
    }
    }