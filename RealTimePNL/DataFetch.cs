using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; // 用 DllImport 需用此 命名空间

namespace RealTimePNL
{

    class DataFetch
    {
        
    }
    struct Market_Data
    {
        Int32 dte;
        Int32 tme;
        Int32 pclse;//
        Int32 opn;//
        Int32 high;
        Int32 low;
        Int32 lastPx;
        Int32 volume;
        Int32 value;//成交金额
        Int32 tcount;//成交笔数
        unsafe fixed Int32 ask[10];
        //ask size
        unsafe fixed UInt32 asize[10];

        unsafe fixed Int32 bid[10];
        //bid size
        unsafe fixed UInt32 bsize[10];
    };

    struct Transaction
    {
        Int32 dte;
        Int32 tme;
        Int32 idx;
        Int32 prc;
        Int64 qty;
        Int64 val;//成交金额
        unsafe fixed char flag[10];//成交类别
    };

    struct Market_Index
    {
        Int32 dte;
        Int32 tme;
        Int32 pclse;//
        Int32 opn;//
        Int32 high;
        Int32 low;
        Int32 lastPx;
        Int64 volume;
        Int64 value;
    };

    struct Market_Data_Futures
    {
        Int32 dte;
        Int32 tme;
        Int32 pclse;
        Int32 popi;//
        Int32 psettle;//
        Int32 opn;//
        Int32 high;
        Int32 low;
        Int32 lastPx;
        Int32 opi;//
        Int32 settle;//
        Int64 volume;
        Int64 value;

        unsafe fixed Int32 ask[5];

        unsafe fixed UInt32 asize[5];

        unsafe fixed Int32 bid[5];

        unsafe fixed UInt32 bsizeUInt32[5];
    };
}
