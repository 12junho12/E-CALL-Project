using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoLampTest.Model
{
    class StaticModel
    {
    }


    public class DI
    {
        public static ushort Start = 0;
        public static ushort Fail = 1;
        public static ushort Detection = 2;
    }

    public class DO
    {
        public static ushort CylinderUp = 0;
        public static ushort CylinderDown = 1;
        public static ushort Select_1 = 2;
        public static ushort Select_2 = 3;
        public static ushort Select_3 = 4;
        public static ushort Select_4 = 5;
        public static ushort Select_5 = 6;
        public static ushort Select_6 = 7;

    }


    public class Program
    {
        public static bool IsRunSW = false;
        public static int MaxRowCnt = 0;
        public static string ModelNo = "";
        public static string MainTitle = "";
        public static int SiteCount = 1;
    }
}
