using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static AssetStudioCLI.Studio;
using AssetStudio;
using CommandLine;

namespace AssetStudioCLI 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new Parser(config =>
            {
                config.AutoHelp = true;
                config.AutoVersion = true;
                config.CaseInsensitiveEnumValues = true;
                config.HelpWriter = Console.Out;
            });
            parser.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                try
                {
                    var game = GameManager.GetGame(o.GameName);

                    if (game == null)
                    {
                        Console.WriteLine("Invalid Game !!");
                        Console.WriteLine(GameManager.SupportedGames());
                        return;
                    }

                    Studio.Game = game;
                    assetsManager.Game = game;
                    Exporter.RetryIfExist = o.Override;
                    assetsManager.ResolveDependancies = false;
                    AssetBundle.Exportable = !o.ExcludeAssetBundle;
                    IndexObject.Exportable = !o.ExcludeIndexObject;

                    if (o.Verbose)
                    {
                        Logger.Default = new ConsoleLogger();
                    }

                    if (o.XORKey != default)
                    {
                        if (o.ExcludeIndexObject)
                        {
                            Logger.Warning("XOR key is set but IndexObject/MiHoYoBinData is excluded, ignoring key...");
                        }
                        else
                        {
                            MiHoYoBinData.doXOR = true;
                            MiHoYoBinData.Key = o.XORKey;
                        }
                        
                    }

                    var inputPath = o.Input;
                    var outputPath = o.Output;
                    var types = o.Type.ToArray();
                    var filtes = o.Filter.ToArray();

                    var files = Directory.Exists(inputPath) ? Directory.GetFiles(inputPath, $"*{game.Extension}", SearchOption.AllDirectories) : new string[] { inputPath };

                    if (o.Map)
                    {
                        var assets = BuildAssetMap(files.ToList(), true);
                        ExportAssetsMap(outputPath, assets, ExportListType.XML);
                        exportableAssets.Clear();
                        assetsManager.Clear();
                    }
                    else
                    {
                        foreach (var file in files)
                        {
                            assetsManager.LoadFiles(file);
                            BuildAssetData(types, filtes);
                            ExportAssets(outputPath, exportableAssets);
                            exportableAssets.Clear();
                            assetsManager.Clear();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            });

        }
    }

    public class Options
    {
        [Option('v', "verbose", HelpText = "Show log messages.")]
        public bool Verbose { get; set; }
        [Option('t', "type", HelpText = "Specify unity type(s).")]
        public IEnumerable<ClassIDType> Type { get; set; }
        [Option('f', "filter", HelpText = "Specify regex filter(s).")]
        public IEnumerable<Regex> Filter { get; set; }
        [Option('g', "game", Required = true, HelpText = "Specify Game.")]
        public string GameName { get; set; }
        [Option('m', "map", HelpText = "Build CABMap/AssetMap.")]
        public bool Map { get; set; }
        [Option('o', "override", HelpText = "Export assets even if name already exist.")]
        public bool Override { get; set; }
        [Option('a', "noassetbundle", HelpText = "Exclude AssetBundle from AssetMap/Export")]
        public bool ExcludeAssetBundle { get; set; }
        [Option('i', "noindexobject", HelpText = "Exclude IndexObject/MiHoYoBinData from AssetMap/Export")]
        public bool ExcludeIndexObject { get; set; }
        [Option('k', "xorkey", HelpText = "XOR key to decrypt MiHoYoBinData")]
        public byte XORKey { get; set; }
        [Value(0, Required = true, MetaName = "input_path", HelpText = "Input file/folder.")]
        public string Input { get; set; }
        [Value(1, Required = true, MetaName = "output_path", HelpText = "Output folder.")]
        public string Output { get; set; }
    }
}