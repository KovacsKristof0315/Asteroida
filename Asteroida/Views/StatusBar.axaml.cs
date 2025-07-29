using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace Asteroida;

public partial class StatusBar : UserControl
{
    public StatusBar()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string StatusText { get; set; } = "Ready";
    public string Time => DateTime.Now.ToShortTimeString();
}