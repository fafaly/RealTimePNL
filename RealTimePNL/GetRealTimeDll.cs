using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; // 用 DllImport 需用此 命名空间

namespace RealTimePNL
{
    internal static class GetRealTimeDll
    {
        const string dlib = "kmdsHQdlld.dll";
        [DllImport(dlib)]
        public static extern void SubscribeData();

        [DllImport(dlib)]
        public static extern float GetCurrentPx(string tk);
    }
}
