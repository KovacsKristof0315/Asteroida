using System.Collections.ObjectModel;
using System.Numerics;
using System.Timers;


namespace AsteroidaGame
{
    public class Model
    {
        private const int num = 20;
        private System.Timers.Timer timer = new System.Timers.Timer();
        private int gameTime = 0;

        public int Num { get { return num; } }
        public List<Asteroida> Asteroids { get; private set; } = new();
        private Random _random = new();
        public Player Player { get; set; }
        public int Col { get; private set; }
        public int GameTime { get { return gameTime; } set { gameTime = value; } }
        public event EventHandler<EventArgs>? LevelUp;
        public event EventHandler<EventArgs>? GameOver;

        public double Speed { get; private set; }
        public Model()
        {
            for (int i = 0; i < num; i++)
            {
                Asteroids.Add(new Asteroida(_random.Next(800), _random.Next(600), _random.Next(10, 30), (_random.Next(1, 11) % 2 == 0 ? 1 : -1)));
            }
            Player = new Player(300, 300);

            Speed = 1;
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Start();
        }


        public void calculateCollison()
        {
            foreach (var item in Asteroids)
            {
                if (item.calculateCollison(Player))
                {
                    Col++;
                }

                foreach (var item2 in Asteroids)
                {
                    if (item2 != item)
                    {
                        item.calculateCollison(item2);
                    }
                }
            }
        }

        private void Timer_Elapsed(object? sender, EventArgs e)
        {
            gameTime++;
            if (gameTime % 5 == 0)
            {
                Speed += 0.2;
                LevelUp?.Invoke(this, e);
            }

            if (Col >= 10)
            {
                GameOver?.Invoke(this, e);
            }
        }
    }
}
