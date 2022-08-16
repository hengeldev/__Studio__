namespace AssetStudio
{
    public class WMVFile : HoYoFile
    {
        public WMVFile(FileReader reader)
        {
            if (reader.BundlePos.Length != 0)
            {
                foreach (var pos in reader.BundlePos)
                {
                    reader.Position = pos;
                    var bundle = new BundleFile(reader);
                    Bundles.Add(pos, bundle.FileList);
                }
            }
            else
            {
                long pos = -1;
                while (reader.Position != reader.Length)
                {
                    pos = reader.Position;
                    var bundle = new BundleFile(reader);
                    Bundles.Add(pos, bundle.FileList);
                }
            }
        }
    }
}
