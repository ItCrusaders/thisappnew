using System.Windows.Forms;

namespace EQS_2._0.repositories
{
    class FileRepository
    {
        OpenFileDialog fpf;


        public OpenFileDialog openFile()
        {
            if (fpf == null)
            {
                fpf = new OpenFileDialog();
            }

            fpf.Filter = "pdf files (*.pdf) |*.pdf;";
            fpf.ShowDialog();
            if (fpf.FileName != null)
            {
                return fpf;
                // use the LoadFile(ByVal fileName As String) function for open the pdf in control  
                //return fpf.FileName;
            }

            return null;//;File.Open(opf.FileName,FileMode.Open,FileAccess.Read,FileShare.Read);

        }

    }
}
