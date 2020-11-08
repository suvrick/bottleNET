using System;
using System.Collections.Generic;
using System.Text;

namespace bottlelib.interfaces
{
    public interface IClientPacket
    {
        byte[] ToPack();
    }
}
