﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CatLike
{
    public class PersistentStorage : MonoBehaviour
    {
        private string savePath;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "saveFile");
        }


        public void Save(PersistableObject o, int version)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
            {
                writer.Write(-version);
                o.Save(new GameDataWriter(writer));
            }
        }

        public void Load(PersistableObject o)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
            {
                o.Load(new GameDataReader(reader, -reader.ReadInt32()));
            }
        }
    }

}