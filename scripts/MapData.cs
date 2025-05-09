using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Godot;
using Godot.Collections;
/*
    file: MapData.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:02 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This script and the global scene is use to mantaing the data of the current selected map in SingleplayerScene.
*/

namespace Trinketos.HaciendaSimulator
{
    public partial class MapData : Node
    {
        public Texture2D heightmap;
        public Texture2D splatmap;
        public Texture2D watermask;
        public Texture2D treeDistMask;

        public Dictionary<string, Variant> mapDefinition;// Json file see data/maps/map_name.json for reference.

        //Config File data tags, i dont know how make this in less code :(
        string triggersTag = "Triggers";
        string eventsTag = "Events";
        string scriptsTag = "Scripts";
        string mapPropertiesTag = "MapProperties";

        string encryp_key = "gndfkgmdsfdkmflmkfdloemfdklmdkmf";

        ///Now the Data for the tags in config file
        public Dictionary<string, Variant> triggersData;
        public Dictionary<string, Variant> eventsData;
        public Dictionary<string, Variant> scriptsData;
        Dictionary<string, Variant> mapPropertiesData;

        public void CreateMapContainer(string fileName, string folderPath)
        {
            if (Godot.FileAccess.FileExists(folderPath))
            {
                DirAccess dir = DirAccess.Open(folderPath);
                dir.Remove(folderPath);
            }
            ZipPacker packer = new ZipPacker();
            packer.Open(folderPath + fileName + ".hmap");
            packer.StartFile("map_definition.json");
            packer.WriteFile(Json.Stringify(mapDefinition).ToUtf8Buffer());
            packer.CloseFile();
            packer.StartFile("map_configuration.cfg");
            packer.WriteFile(GenerateConfigFile().EncodeToText().ToUtf8Buffer());
            packer.CloseFile();
            packer.StartFile("heightmap.png");
            packer.WriteFile(heightmap.GetImage().SavePngToBuffer());
            packer.CloseFile();
            packer.StartFile("splatmap.png");
            packer.WriteFile(splatmap.GetImage().SavePngToBuffer());
            packer.CloseFile();
            packer.StartFile("watermask.png");
            packer.WriteFile(watermask.GetImage().SavePngToBuffer());
            packer.CloseFile();
            packer.StartFile("tree_density_map.png");
            packer.WriteFile(treeDistMask.GetImage().SavePngToBuffer());
            packer.CloseFile();
            packer.Close();
        }

        public Texture2D GetTextureFromContainer(byte[] rawImage)
        {
            Image image = new Image();
            Error err = image.LoadPngFromBuffer(rawImage);

            if (err != Error.Ok)
            {
                GD.PrintErr("Error at loading image from bytes");
                return null;
            }

            return ImageTexture.CreateFromImage(image);
        }

        public string GetMapDefinitionsFromContainer(string fileName, string folderPath)
        {
            ZipReader reader = new ZipReader();
            reader.Open(folderPath + fileName + ".hmap");
            string data = reader.ReadFile("map_definition.json").ToString();
            return data;
        }

        public Dictionary<string, Variant> GetDataFromContainer(string fileName, string folderPath, string tag)
        {
            ZipReader reader = new ZipReader();
            reader.Open(folderPath + fileName + ".hmap");
            if (!reader.GetFiles().Contains("map_configuration.cfg"))
            {
                GD.Print("Not file or corrupted.");
                return null;
            }
            string configData = reader.ReadFile("map_configuration.cfg").GetStringFromUtf8();
            reader.Close();
            ConfigFile config = new ConfigFile();

            Error err = config.Parse(configData);

            if (err != Error.Ok)
            {
                GD.PrintErr("Error to load configuration file from memory");
                return null;
            }
            Dictionary<string, Variant> data = new Dictionary<string, Variant>();

            if (config.HasSection(tag))
            {
                foreach (string key in config.GetSectionKeys(tag))
                {
                    data.Add(key, config.GetValue(tag, key));
                }
            }

            return data;
        }

        private ConfigFile GenerateConfigFile()
        {
            ConfigFile config = new ConfigFile();

            foreach (var entry in triggersData)
            {
                config.SetValue(triggersTag, entry.Key, entry.Value);
            }

            foreach (var entry in eventsData)
            {
                config.SetValue(eventsTag, entry.Key, entry.Value);
            }

            foreach (var entry in scriptsData)
            {
                config.SetValue(scriptsTag, entry.Key, entry.Value);
            }

            foreach (var entry in mapPropertiesData)
            {
                config.SetValue(mapPropertiesTag, entry.Key, entry.Value);
            }

            GD.Print("Config File is store in memory");

            return config;
        }

        /*
        byte[] encryptedConfig = EncryptData(Json.Stringify(mapDefinition).ToUtf8Buffer(), "TuClaveSecreta");
        packer.WriteFile(encryptedConfig);
        */

        public static byte[] EncryptData(byte[] data, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32)); // Clave de 256 bits
                aes.IV = new byte[16]; // IV de 16 bytes (puedes usar uno aleatorio)
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }

        /*
        PackedByteArray configData = reader.ReadFile("map_configuration.cfg");
        byte[] decryptedConfig = DecryptData(configData, "TuClaveSecreta");
        string configText = Encoding.UTF8.GetString(decryptedConfig);
        */

        public static byte[] DecryptData(byte[] encryptedData, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                }
            }
        }

        /*
        byte[] compressedConfig = CompressData(Json.Stringify(mapDefinition).ToUtf8Buffer());
        packer.WriteFile(compressedConfig);
        */

        public static byte[] CompressData(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (DeflateStream compressionStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                {
                    compressionStream.Write(data, 0, data.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public static byte[] DecompressData(byte[] compressedData)
        {
            using (MemoryStream memoryStream = new MemoryStream(compressedData))
            using (DeflateStream decompressionStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    decompressionStream.CopyTo(outputStream);
                    return outputStream.ToArray();
                }
            }
        }
    }
}
