using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeOracle19C.Models
{
    public class Partida
    {
        public int IdPartida { get; set; }
        public int IdJugador { get; set; }
        public int Puntuacion { get; set; }
        public int LongitudSerpiente { get; set; }
        public string Resultado { get; set; } = string.Empty;
        public DateTime FechaPartida { get; set; }
    }   
}
