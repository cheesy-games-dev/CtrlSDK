using System.Collections.Generic;
using System.Linq;

namespace CtrlSDK
{
    public static class DiskDrive
    {
        public static List<CtrlObject> ctrlObjectList = new();
        public static List<Disk> diskList = new();
        public static List<Byte> byteList = new();

        public static CtrlObject GetCtrlData(string barcode)
        {
            return GetObjectFromBarcode(barcode, ctrlObjectList);
        }

        public static Disk GetDiskData(string barcode)
        {
            return (Disk)GetObjectFromBarcode(barcode, diskList.Cast<CtrlObject>().ToList());
        }

        public static Byte GetByteData(string barcode)
        {
            foreach (var disk in diskList)
            {
                return (Byte)GetObjectFromBarcode(barcode, disk.bytes.Cast<CtrlObject>().ToList());
            }
            return null;
        }

        private static CtrlObject GetObjectFromBarcode(string barcode, List<CtrlObject> objects)
        {
            foreach (CtrlObject obj in objects)
            {
                if (obj.barcode.Contains(barcode))
                {
                    return obj;
                }
            }
            return null;
        }
    }
}
