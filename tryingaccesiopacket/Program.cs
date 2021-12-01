using System;
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

            //Console.WriteLine(client.ChangeRelay(AccesioEthernet.Relay.Five,AccesioEthernet.RelayStatus.ON));
           // Console.WriteLine(client.GetAllData());


            Console.WriteLine("\n Press Enter to continue...");
                Console.Read();



            }

        private static void Client_relayChangedStatus(object sender, AccesIO.Assets.pointInfo e)
        {
            Console.WriteLine(e.type+" pointchange:"+e.num+" Status:"+e.status.ToString());
        }
    }
    }