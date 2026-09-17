using System.Drawing;

namespace SnakeOracle19C.Game
{
    public class Comida
    {
        private readonly Random random = new Random();

        public Point Posicion { get; private set; }

        public void Generar(int columnas, int filas, List<Point> cuerpo)
        {
            do
            {
                Posicion = new Point(
                    random.Next(0, columnas),
                    random.Next(0, filas)
                );
            }
            while (cuerpo.Contains(Posicion));
        }
    }
}   
