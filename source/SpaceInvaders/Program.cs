using System;

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
