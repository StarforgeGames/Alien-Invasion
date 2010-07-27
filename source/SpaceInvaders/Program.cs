using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SpaceInvaders
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Dx10Application app = new Application();
            app.Initialize();
            app.Run();
        }
    }

}
