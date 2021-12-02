using System;
using AccesIO;
using AccesIO.Assets;

namespace tryingaccesiopacket
{
    class Program
    {

            static void Main(string[] args)
            {


            AccesioEthernet client = new AccesioEthernet();
            client.StartCommunication();
            client.pointChangedStatus += Client_relayChangedStatus;
            client.ChangeRelay(3,pointState.ON);
           

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();



            }

        private static void Client_relayChangedStatus(object sender, pointInfo e)
        {
            Console.WriteLine(e.type+" pointchange:"+e.num+" Status:"+e.status.ToString());
        }
    }
    }