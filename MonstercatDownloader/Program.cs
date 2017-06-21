using System;
using System.Windows.Forms;

namespace MonstercatDownloader
{
    static class Program
    {
        /// <summary>
        /// Matthew O, GH:moldfield
        /// Vershon 1.0
        /// 
        /// </summary>
        [STAThread]
        static void Main()
        {
            Loging.Logger.Initilize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MCDownloader());
        }
    }
}
