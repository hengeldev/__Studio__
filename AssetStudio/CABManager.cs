using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AssetStudio
{
    public static class CABManager
    {
        public static Dictionary<string, Entry> CABMap = new Dictionary<string, Entry>();
        public static Dictionary<string, HashSet<long>> offsets = new Dictionary<string, HashSet<long>>();

        public static void BuildMap(List<string> files, Game game)
        {
            Logger.Info(string.Format("Building {0}", game.MapName));
            try
            {
                int collisions = 0;
                CABMap.Clear();
                Progress.Reset();
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    using (var reader = new FileReader(file, game))
                    {
                        var hoyoFile = new HoYoFile();
                        hoyoFile.LoadFile(reader);
                        foreach (var bundle in hoyoFile.Bundles)
                        {
                            foreach (var cab in bundle.Value)
                            {
                                using (var cabReader = new FileReader(cab.stream))
                                {
                                    if (cabReader.FileType == FileType.AssetsFile)
                                    {
                                        if (CABMap.ContainsKey(cab.path))
                                        {
                                            collisions++;
                                            continue;
                                        }
                                        var assetsFile = new SerializedFile(cabReader, null);
                                        var dependencies = assetsFile.m_Externals.Select(x => x.fileName).ToList();
                                        CABMap.Add(cab.path, new Entry(file, bundle.Key, dependencies));
                                    }
                                }
                            }
                        }
                    }
                    Logger.Info($"[{i + 1}/{files.Count}] Processed {Path.GetFileName(file)}");
                    Progress.Report(i + 1, files.Count);
                }

                CABMap = CABMap.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                var outputFile = new FileInfo($"Maps/{game.MapName}.bin");

                if (!outputFile.Directory.Exists)
                    outputFile.Directory.Create();

                using (var binaryFile = outputFile.Create())
                using (var writer = new BinaryWriter(binaryFile))
                {
                    writer.Write(CABMap.Count);
                    foreach (var cab in CABMap)
                    {
                        writer.Write(cab.Key);
                        writer.Write(cab.Value.Path);
                        writer.Write(cab.Value.Offset);
                        writer.Write(cab.Value.Dependencies.Count);
                        foreach (var dependancy in cab.Value.Dependencies)
                        {
                            writer.Write(dependancy);
                        }
                    }
                }
                Logger.Info($"{game.MapName} build successfully, {collisions} collisions found !!");
            }
            catch (Exception e)
            {
                Logger.Warning($"{game.MapName} was not build, {e.Message}");
            }
        }
        public static void LoadMap(Game game)
        {
            Logger.Info(string.Format("Loading {0}", game.MapName));
            try
            {
                CABMap.Clear();
                using (var binaryFile = File.OpenRead($"Maps/{game.MapName}.bin"))
                using (var reader = new BinaryReader(binaryFile))
                {
                    var count = reader.ReadInt32();
                    CABMap = new Dictionary<string, Entry>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var cab = reader.ReadString();
                        var path = reader.ReadString();
                        var offset = reader.ReadInt64();
                        var depCount = reader.ReadInt32();
                        var dependencies = new List<string>();
                        for (int j = 0; j < depCount; j++)
                        {
                            var dependancy = reader.ReadString();
                            dependencies.Add(dependancy);
                        }
                        CABMap.Add(cab, new Entry(path, offset, dependencies));
                    }
                }
                Logger.Info(string.Format("Loaded {0} !!", game.MapName));
            }
            catch (Exception e)
            {
                Logger.Warning($"{game.Name} was not loaded, {e.Message}");
            }
        }

        public static void AddCABOffset(string cab)
        {
            if (CABMap.TryGetValue(cab, out var wmvEntry))
            {
                if (!offsets.ContainsKey(wmvEntry.Path))
                {
                    offsets.Add(wmvEntry.Path, new HashSet<long>());
                }
                offsets[wmvEntry.Path].Add(wmvEntry.Offset);
                foreach (var dep in wmvEntry.Dependencies)
                {
                    AddCABOffset(dep);
                }
            }
        }

        public static bool FindCAB(string path, out List<string> cabs)
        {
            cabs = new List<string>();
            foreach (var pair in CABMap)
            {
                if (pair.Value.Path.Contains(path))
                {
                    cabs.Add(pair.Key);
                }
            }
            return cabs.Count != 0;
        }

        public static void ProcessFiles(ref string[] files)
        {
            var newFiles = files.ToList();
            foreach (var file in files)
            {
                if (!offsets.ContainsKey(file))
                {
                    offsets.Add(file, new HashSet<long>());
                }
                if (FindCAB(file, out var cabs))
                {
                    foreach (var cab in cabs)
                    {
                        AddCABOffset(cab);
                    }
                }
            }
            newFiles.AddRange(offsets.Keys.ToList());
            files = newFiles.ToArray();
        }

        public static void ProcessDependencies(ref string[] files)
        {
            if (CABMap.Count == 0)
            {
                Logger.Warning("CABMap is not build, skip resolving dependencies...");
                return;
            }


            Logger.Info("Resolving Dependencies...");
            var file = files.FirstOrDefault();
            var supportedExtensions = GameManager.GetGames().Select(x => x.Extension).ToList();
            if (supportedExtensions.Contains(Path.GetExtension(file)))
            {
                ProcessFiles(ref files);
            }
        }
    }

    public class Entry : IComparable<Entry>
    {
         public string Path;
         public long Offset;
         public List<string> Dependencies;
         public Entry(string path, long offset, List<string> dependencies)
         {
             Path = path;
             Offset = offset;
             Dependencies = dependencies;
         }
         public int CompareTo(Entry other)
         {
             if (other == null) return 1;

             int result;
             if (other == null)
                 throw new ArgumentException("Object is not an Entry");

             result = Path.CompareTo(other.Path);

             if (result == 0)
                 result = Offset.CompareTo(other.Offset);

             return result;
         }
    }
}
