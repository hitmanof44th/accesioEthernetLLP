# accesioEthernetLLP
Accesio Ethernet Low level intergace ETH-IIRO

simple example
            
			
			
	AccesioEthernet client = new AccesioEthernet();
	client.StartCommunication();
	client.pointChangedStatus += Client_relayChangedStatus;