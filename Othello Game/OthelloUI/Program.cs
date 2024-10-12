using System;
using System.Windows.Forms;

namespace Ex05.OthelloUI
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormOthelloSettings());
        }
    }
}
