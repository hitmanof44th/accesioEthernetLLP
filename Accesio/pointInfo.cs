using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesIO.Assets
{
    public class pointInfo
    {
        public int num { get; set; } = 0;
        public pointState status { get; set; } = pointState.OFF;
        public pointType type { get; set; } = pointType.Input;

    }

}
