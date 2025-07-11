using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CtrlSDK.Editor
{
    [CustomEditor(typeof(CtrlObject), true)]
    public class CtrlObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var target = this.target as CtrlObject;
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Label("Barcode:");
            EditorGUILayout.TextField(target.barcode);
            EditorGUI.EndDisabledGroup();
            base.OnInspectorGUI();
            if (target as Disk != null)
            {
                DiskEditor.OnGUI(target as Disk);
            }
        }

        public static class DiskEditor
        {
            private static bool burnToggle;
            private static bool expirementalToggle;
            public static void OnGUI(Disk disk)
            {
                bool repair = GUILayout.Button("Attempt Disk Repair");
                bool build = GUILayout.Button("Burn Disk");
                expirementalToggle = GUILayout.Toggle(expirementalToggle, "Expiremental Toggle");
                if (burnToggle)
                {
                    if (GUILayout.Button("Build to PC")) Build(disk, BuildTarget.StandaloneWindows);
                    if (expirementalToggle)
                    {
                        if (GUILayout.Button("Build to Mac")) Build(disk, BuildTarget.StandaloneOSX);
                        if (GUILayout.Button("Build to Linux")) Build(disk, BuildTarget.StandaloneLinux64);
                    }
                    if (GUILayout.Button("Build to Quest")) Build(disk, BuildTarget.Android);
                }
                if (repair)
                {
                    Repair(disk);
                }
                if (build)
                {
                    burnToggle = !burnToggle;
                }
            }

            private static string modsLocation => Application.persistentDataPath + "/Mods";

            private static void Build(Disk disk, BuildTarget platform)
            {
                Repair(disk);
                
                BuildAssetBundlesParameters parameters = new();
                parameters.outputPath = modsLocation;
                parameters.targetPlatform = platform;
                try
                {
                    BuildPipeline.BuildAssetBundles(parameters);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                    Debug.Log($"Go to {Application.persistentDataPath} and create a folder called Mods");
                    return;
                }
                Debug.Log($"Mod Built to {modsLocation}", disk);
            }

            private static void Repair(Disk disk)
            {
                Debug.Log($"Attempting to Repair Disk: [{disk.barcode}]", disk);
                RemoveBytes(disk);
                FindBytes(disk);
                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(disk.GetInstanceID()));
                importer.assetBundleName = disk.barcode;
            }

            private static void FindBytes(Disk disk)
            {
                List<Byte> bytes = new List<Byte>();
                foreach (var bbyte in DiskDrive.byteList)
                {
                    var abyte = bbyte;
                    if (bbyte.diskBarcode.Contains(disk.barcode) && !disk.bytes.Contains(abyte))
                    {
                        bytes.Add(abyte);
                    }
                }
                disk.bytes.AddRange(bytes);
                Debug.Log($"Found and Added [{bytes.Count}] Bytes", disk);
            }

            private static void RemoveBytes(Disk disk)
            {
                List<Byte> removedBytes = new();
                foreach (var bbyte in disk.bytes)
                {
                    if (bbyte == null)
                    {
                        removedBytes.Add(bbyte);
                        continue;
                    }
                }
                Debug.Log($"Removed [{removedBytes.Count}] Bytes", disk);
                foreach (var bbyte in removedBytes)
                {
                    disk.bytes.Remove(bbyte);
                }
            }
        }
    }
}
