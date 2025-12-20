using UglyToad.PdfPig;

namespace UnimarFrontend.backend.Service
{
    public class PdfPigThumbNail
    {
        public void GetPdfPage(FileInfo file, DirectoryInfo output ,string originalFileName )
        {
            using (var pdf = PdfDocument.Open(file.FullName))
            {
                int pageNumber = 1;
                foreach (var page in pdf.GetPages())
                {
                    foreach (var img in page.GetImages())
                    {
                        File.WriteAllBytes(Path.Combine(output.FullName, String.Format("{0}.png" , originalFileName)), img.RawBytes.ToArray());
                    }
                    pageNumber++;
                }
            }
        }
    }
}
