using System.Windows.Forms;

namespace SpaceInvaders.Input
{
    interface IKeyboardHandler
    {
        void OnKeyDown(object sender, KeyEventArgs e);
        void OnKeyUp(object sender, KeyEventArgs e);
    }
}
