using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesIO.Assets
{

    public enum pointType { Relay,Input }
    public enum replies { R_OK = 331 }
    public enum relay { Zero = 0x01, One = 0x02, Two = 0x03, Three = 0x04, Four = 0x05, Five = 0x06, Six = 0x07, Seven = 0x08 }
    public enum pointState { ON = 0x00, OFF = 0x01 }

}
