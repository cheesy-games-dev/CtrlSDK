using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CtrlSDK
{
    public abstract class CtrlObject : ScriptableObject
    {
        [HideInInspector] public string barcode;

        public new string name = "Name";

        public string[] tags;

        private void OnValidate()
        {
            Validate();
        }

        public virtual void Validate()
        {
            if (string.IsNullOrEmpty(barcode))
            {
                Debug.LogError("Why tf is your barcode empty");
            }
            else if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("Name Is Empty, It's Required");
            }
            if (DiskDrive.ctrlObjectList.Contains(this)) return;
            DiskDrive.ctrlObjectList.Add(this);
            if (this as Disk != null) DiskDrive.diskList.Add(this as Disk);
            else if (this as Byte != null) DiskDrive.byteList.Add(this as Byte);
        }
    }

    [Serializable]
    public class CtrlReference
    {
        public string barcode;
        public CtrlObject reference;
    }
}
