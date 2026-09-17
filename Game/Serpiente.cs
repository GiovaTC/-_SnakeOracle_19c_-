using System.Drawing;

namespace SnakeOracle19C.Game
{
    public class Serpiente
    {
        public List<Point> Cuerpo { get; private set; }
        public Direccion DireccionActual { get; set; }
        public Serpiente(int x, int y)
        {
            Cuerpo = new List<Point> 
            { 
                new Point(x, y),
                new Point(x - 1, y),
                new Point(x - 2, y)
            };

            DireccionActual = Direccion.Derecha;
        }

        public Point ObtenerCabeza()
        {
            return Cuerpo[0];
        }   

        public void Mover(bool crecer)
        {
            Point cabeza = ObtenerCabeza();
            Point nuevaCabeza = cabeza;

            switch (DireccionActual)
            {
                case Direccion.Arriba:
                    nuevaCabeza = new Point(cabeza.X, cabeza.Y - 1);
                    break;

                case Direccion.Abajo:
                    nuevaCabeza = new Point(cabeza.X, cabeza.Y + 1);
                    break;

                case Direccion.Izquierda:
                    nuevaCabeza = new Point(cabeza.X - 1, cabeza.Y);
                    break;

                case Direccion.Derecha:
                    nuevaCabeza = new Point(cabeza.X + 1, cabeza.Y);
                    break;
            }

        }
    }
}
