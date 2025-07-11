using System.Collections.Generic;
using UnityEngine;

namespace CtrlSDK
{
    [CreateAssetMenu(fileName = "NewDisk", menuName = "Ctrl SDK/Disk")]
    public class Disk : CtrlObject
    {
        public string author = "Author";
        public List<Byte> bytes = new();
        public List<CtrlReference> dependencies;
        public override void Validate()
        {
            if (string.IsNullOrEmpty(author))
            {
                Debug.LogError("Author Is Empty, It's Required");
            }
            barcode = $"{author}/{name}";
            int i = 0;
            foreach (var dependency in dependencies)
            {
                if (dependency.reference)
                {
                    dependencies[i].barcode = dependency.reference.barcode;
                    dependencies[i].reference = null;
                }
                i++;
            }

            foreach (var bbyte in bytes)
            {
                if (bbyte)
                {
                    bbyte.barcode = barcode + "/" + bbyte.name;
                    bbyte.disk = this;
                }
                else Debug.LogWarning($"Missing Byte in {barcode}");
            }
        }
    }
}
