using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeOracle19C.Models
{
    public class Jugador
    {
        public int IdJugador { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }   
}
