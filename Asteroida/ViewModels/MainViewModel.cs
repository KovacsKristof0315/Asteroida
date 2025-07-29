using Avalonia;
using Asteroida.Avalonia.ViewModels;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using AsteroidaGame;


namespace Asteroida.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    public ObservableCollection<tAsteroida> Asteroids { get; } = new();

    private Model _model;
    private DispatcherTimer _timer;
    private Random _random = new();
    private int col = 0; 

    public Player player = new Player();
    public int Col
    {
        get => col;
        set
        {
            if (col != value)
            {
                col = value;
                OnPropertyChanged(nameof(Col));
                OnPropertyChanged(nameof(StatusText));
            }
        }
    }

    public string StatusText => "Num of collision: " + Col;
    public MainViewModel(Model model)
    {
        _model = model;
        
        for (int i = 0; i < _model.Num; i++)
            Asteroids.Add(new tAsteroida { X = _random.Next(800), Y = _random.Next(600) });

        Asteroids.Add(player);
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _timer.Tick += GameLoop;
        _timer.Start();

        ExitGameCommand = new RelayCommand(OnExitCommand);
        
    }

    private void GameLoop(object? sender, EventArgs e)
    {
        foreach (var asteroid in Asteroids.Where(x => !(x is Player)))
        {
            asteroid.X += 1;
            asteroid.Y += 1;

            if (asteroid.X > 1100) asteroid.X = 0;
            if (asteroid.Y > 600) asteroid.Y = 0;
        }



        foreach (var asteroid in Asteroids.Where(x => !(x is Player)))
        {
            asteroid.RaisePositionChanged();
            if (_model.calculateCollison((asteroid.X, asteroid.Y), (player.PlayerPosition.X, player.PlayerPosition.Y)))
            {
                asteroid.X = 0;
                asteroid.Y = 0;
                Col++;
            }
        }




    }

    public RelayCommand ExitGameCommand { get; private set; }
    public event EventHandler? ExitGame;



    private void OnExitCommand()
    {
        ExitGame?.Invoke(this, EventArgs.Empty);
    }
}
