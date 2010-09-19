using System;
using System.Windows.Forms;

namespace SpaceInvaders
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static Graphics.Renderer renderer;

        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            Form form = new Form();
            Button button = new Button();
            form.Controls.Add(button);

            renderer = new Graphics.Renderer(form);
            renderer.Start();

            System.Windows.Forms.Application.Run(form);
            renderer.Stop();
/*            System.Windows.Forms.Application app = new System.Windows.Forms.Application();
            app.Initialize();
            app.Run();
 * */
        }
    }

}
