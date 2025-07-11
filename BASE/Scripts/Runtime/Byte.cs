using UnityEngine;

namespace CtrlSDK
{
    [CreateAssetMenu(fileName = "NewByte", menuName = "Ctrl SDK/Byte")]
    public class Byte : CtrlObject
    {
        public string diskBarcode => barcode.Replace("/" + name, "");
        public Disk disk;
        public Object ObjectReference;

        public override void Validate()
        {
            if (!disk)
            {
                disk = DiskDrive.GetDiskData(diskBarcode);
                if (!disk)
                {
                    Debug.LogError($"Disk Reference Null in {barcode}", this);
                }
            }
            else disk.Validate();
            if (ObjectReference == null) Debug.LogError($"Object Reference Null in {barcode}", this);
            if (ObjectReference == this || ObjectReference == disk)
            {
                Debug.LogError($"WTF, did you try to put your own disk or byte as an object reference in {barcode}, You disappoint me!", this);
                ObjectReference = null;
            }
        }
    }
}