using System;
using System.Windows.Forms;
using System.IO;

namespace text_editor
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args != null && args.Length > 0)
            {
                string fileName = args[0];
                Console.WriteLine(fileName);

                if (File.Exists(fileName))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    FormMain MainForm = new FormMain();
                    MainForm.OpenDocumentContext(fileName);
                    Application.Run(MainForm);
                }
                else
                {
                    MessageBox.Show("Файл не читается!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }   
        }
    }
}
