using System;
using System.Drawing;
using System.IO;
using System.Reflection;

public class IconHelper
{
    public static Icon GetEmbeddedIcon(string resourceName)
    {
        // Assembly'i alıyoruz
        var assembly = Assembly.GetExecutingAssembly();

        // İkonu kaynaklardan (resources) çekiyoruz
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream != null)
            {
                return new Icon(stream);
            }
            else
            {
                throw new FileNotFoundException($"Kaynak bulunamadı: {resourceName}");
            }
        }
    }
}
