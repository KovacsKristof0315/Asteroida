using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Asteroida.Avalonia;

public partial class DekstopWindow : Window
{
    public DekstopWindow()
    {
        InitializeComponent();
        this.Opened += (_,_)=>
        {
            GameCanvas.Focus(); 
        };
    }
}