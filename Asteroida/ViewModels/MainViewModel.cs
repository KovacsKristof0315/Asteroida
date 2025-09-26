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

    public ObservableCollection<AsteroidaGame.Asteroida> Asteroids { get; } = new();

    private Model _model;
    private DispatcherTimer _timer;
    private Random _random = new();
    private int col = 0;
    private double speed = 1;

    public Asteroida.Avalonia.ViewModels.Player player = new Asteroida.Avalonia.ViewModels.Player();
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
    public AsteroidaGame.Player Player { get; set; }

    public string StatusText => "Num of collision: " + Col;

    public String ElapsedTime
    {
        get { return " | " + TimeSpan.FromSeconds(_model.GameTime).ToString("g"); }
    }

    public MainViewModel(Model model)
    {
        _model = model;
        _model.LevelUp += new EventHandler<EventArgs>(LevelUp);
        _model.GameOver += new EventHandler<EventArgs>(GameOver);
        Player = model.Player;
        Asteroids = new ObservableCollection<AsteroidaGame.Asteroida>([.._model.Asteroids, _model.Player]);
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _timer.Tick += GameLoop;
        _timer.Start();
         
        ExitGameCommand = new RelayCommand(OnExitCommand);
        
    }

    private void LevelUp(object? sender, EventArgs e)
    {
        speed = _model.Speed;
    }

    private void GameOver(object? sender, EventArgs e)
    {
        ExitGame?.Invoke(this, EventArgs.Empty);
    }

    private void GameLoop(object? sender, EventArgs e)
    {
        foreach (var asteroid in _model.Asteroids)
        {
            asteroid.NewPosition(speed);
        }
        _model.Player.NewPosition(speed);
        _model.calculateCollison();
        Col = _model.Col;
        OnPropertyChanged(nameof(ElapsedTime));
    }

    public RelayCommand ExitGameCommand { get; private set; }
    public event EventHandler? ExitGame;



    private void OnExitCommand()
    {
        ExitGame?.Invoke(this, EventArgs.Empty);
    }
}
