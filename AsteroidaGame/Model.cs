using System.Collections.ObjectModel;


namespace AsteroidaGame
{
    public class Model
    {
        private const int num = 15;
        

        public int Num { get { return num; } }



        public bool calculateCollison((double, double) p1, (double, double) player)
        {
            if (p1.Item1 < player.Item1+20 && p1.Item2 < player.Item2+20 && p1.Item1 > player.Item1-20 && p1.Item2 > player.Item2-20)
            {
                return true;
            }
            return false;
        }
    }
}
