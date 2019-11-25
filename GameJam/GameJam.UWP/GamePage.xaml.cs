using MonoGame.Framework;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameJam.UWP
{
    public sealed partial class GamePage : Page
    {
        readonly GameManager GameManager;

        public GamePage()
        {
            try
            {
                InitializeComponent();
                GameManager = XamlGame<GameManager>.Create(string.Empty,
                    Window.Current.CoreWindow, swapChainPanel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException);
            }
        }
    }
}
