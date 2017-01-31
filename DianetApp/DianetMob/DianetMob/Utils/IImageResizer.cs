using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.Utils
{
    public interface IImageResizer
    {
        int FdpToPixHeight(float fdp);
        int FdpToPixWidth(float fdp);
        byte[] ResizeImage(byte[] imageData, double width, double height);
    }
}
