using UglyToad.PdfPig;
using UnimarFrontend.Dominio.ValueObjects;

namespace UnimarFrontend.backend.Service
{
    public class PdfPigThumbNail
    {
        public FileInfo GetPdfPage(FileInfo file, DirectoryInfo output  )
        {
            using (var pdf = PdfDocument.Open(file.FullName))
            {
                var page = pdf.GetPages().FirstOrDefault();
                var img = page.GetImages().FirstOrDefault();

                var thumbName = new FileNameWithExtension(file.Name , "png");
                var fileName = Path.Combine(output.FullName, String.Format("{0}", thumbName.Value));
                File.WriteAllBytes(Path.Combine(output.FullName, String.Format("{0}.png", thumbName.Value)), img.RawBytes.ToArray());
                return new FileInfo(fileName);

            }
        }
    }
}
