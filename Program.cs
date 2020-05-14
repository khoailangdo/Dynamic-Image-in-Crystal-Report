using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DynamicImage
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;

            var m = new Mutex(true, "Escreening", out createdNew);

            if (!createdNew)
            {
                // myApp is already running...
                MessageBox.Show("Chương trình đã đang chạy!", "Escreening");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FrmMain());

        }
    }

    public class MultiFormContext : ApplicationContext
    {
        private int _openForms;
        public MultiFormContext(params Form[] forms)
        {
            _openForms = forms.Length;

            foreach (var form in forms)
            {
                form.FormClosed += (s, args) =>
                {
                    //When we have closed the last of the "starting" forms, 
                    //end the program.
                    if (Interlocked.Decrement(ref _openForms) == 0)
                        ExitThread();
                };


                if (form.Name == "FrmLogin")
                {
                    form.ShowDialog();
                }
                else
                {
                    form.Show();
                }
            }
        }
    }

}
