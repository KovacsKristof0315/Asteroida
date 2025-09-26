using Asteroida.Avalonia.ViewModels;
using Asteroida.ViewModels;
using Asteroida.Views;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;
using System.Numerics;
using AsteroidaGame;
using Asteroida.Avalonia;

namespace Asteroida;

public partial class App : Application
{
    private MainViewModel _viewModel = null!;
    private Model _model = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        _model = new Model();
        _viewModel = new MainViewModel(_model);
        
        _viewModel.ExitGame += new EventHandler(viewModel_ExitGame);
        

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new DekstopWindow
            {
                DataContext = _viewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void viewModel_ExitGame(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }

 

}
