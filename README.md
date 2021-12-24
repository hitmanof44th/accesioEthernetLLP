# accesioEthernetLLP
Accesio Ethernet Low level intergace ETH-IIRO
this project uses the low level packets from accesio to send over and monitoer I/O 

simple example
            
			
			
	AccesioEthernet client = new AccesioEthernet();
	client.StartCommunication();
	client.pointChangedStatus += Client_relayChangedStatus;