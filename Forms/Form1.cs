using SnakeOracle19C.Data;
using SnakeOracle19C.Game;
using SnakeOracle19C.Models;
using System.Drawing;


namespace SnakeOracle19C
{
    public partial class Form1 : Form
    {
        private const int TamanoCelda = 20;
        private const int Columnas = 30;
        private const int Filas = 25;

        private Serpiente serpiente;
        private Comida comida;
        private System.Windows.Forms.Timer timer;
        private int puntuacion;
        private bool jugando;
        private string nombreJugador = "";
        private readonly PartidaDAO partidaDAO;

        public Form1()
        {
            InitializeComponent();
            partidaDAO = new PartidaDAO();

            serpiente = new Serpiente(
                Columnas / 2,
                Filas / 2
             );

            comida = new Comida();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 120;
            timer.Tick += Timer_Tick;
            KeyPreview = true;
            KeyDown += Form1_KeyDown;
            InicializarJuego();

        }

        private void InicializarJuego()
        {
            serpiente = new Serpiente(
                Columnas / 2,
                Filas / 2)
            ;

            comida.Generar(
                Columnas,
                Filas,
                serpiente.Cuerpo
            );
            puntuacion = 0;
            jugando = false;
            lblPuntuacion.Text = "Puntuacion: 0";

            lblLongitud.Text =
                "Longitud: " +
                serpiente.Cuerpo.Count;

            lblEstado.Text =
                "Presiona INICIAR";

            panelJuego.Invalidate();
        }

        private void btnIniciar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJugador.Text))
            {
                MessageBox.Show(
                    "Ingresa el nombre del jugador.",
                    "SNAKE",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtJugador.Focus();
                return;
            }

            nombreJugador = txtJugador.Text.Trim();
            InicializarJuego();
            jugando = true;
            timer.Start();
            lblEstado.Text = "Jugando... ";
            txtJugador.Enabled = false;
            btnIniciar.Enabled = false;
        }

        private void btnReiniciar_Click(object? sender, EventArgs e)
        {
            timer.Stop();

            txtJugador.Enabled = true;
            btnIniciar.Enabled = true;

            InicializarJuego();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!jugando)
            {
                return;
            }

            bool comer =
                serpiente.ObtenerCabeza() == comida.Posicion;

            serpiente.Mover(comer);

            if (comer)
            {
                puntuacion += 10;

                comida.Generar(
                    Columnas,
                    Filas,
                    serpiente.Cuerpo
                );
            }
            
            if (ColisionConParedes() || serpiente.ChocaConSuCuerpo())
            {
                FinalizarJuego();

                return;
            }

            lblPuntuacion.Text =
                "PUNTUACION: " + puntuacion;    

            lblLongitud.Text =
                "LONGITUD: " + serpiente.Cuerpo.Count;

            panelJuego.Invalidate();    
        }   

        private bool ColisionConParedes()
        {
            Point cabeza = serpiente.ObtenerCabeza();   

            return cabeza.X < 0 || cabeza.X >= Columnas ||
                   cabeza.Y < 0 || cabeza.Y >= Filas;
        }

        private void FinalizarJuego()
        {
            timer.Stop();
            jugando = false;

            lblEstado.Text = "GAME OVER";

            try
            {
                int idJugador =
                    partidaDAO.ObtenerOCrearJugador(
                        nombreJugador
                    );

                Partida partida = new Partida
                {
                    IdJugador = idJugador,
                    Puntuacion = puntuacion,

                    LongitudSerpiente =
                        serpiente.Cuerpo.Count,
                    Resultado = "GAME OVER",
                    FechaPartida = DateTime.Now
                };
                partidaDAO.GuardarPartida(partida);

                MessageBox.Show(
                    $"GAME OVER\n\n" +
                    $"Jugador: {nombreJugador}\n" +
                    $"Puntuacion: {puntuacion}\n" +
                    $"Longitud: {serpiente.Cuerpo.Count}\n\n" +
                    "La partida fue guardada en ORACLE.",
                    "SNAKE",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "La partida termino, pero no fue posible " +
                    "guardar la informacion en ORACLE.\n\n" +
                    ex.Message,
                    "ERROR oracle",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
            txtJugador.Enabled = true;  
            btnIniciar.Enabled = true;                
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!jugando)
            {
                return;
            }

            switch (e.KeyCode)  
            {
                case Keys.Up:

                    if (serpiente.DireccionActual 
                        != Direccion.Abajo)
                    {
                        serpiente.DireccionActual = Direccion.Arriba;   
                    }

                    break;

                case Keys.Down:

                    if (serpiente.DireccionActual 
                        != Direccion.Arriba)
                    {
                        serpiente.DireccionActual = Direccion.Abajo;
                    }

                    break;

                case Keys.Left:

                    if(serpiente.DireccionActual 
                        != Direccion.Derecha)
                    {
                        serpiente.DireccionActual = Direccion.Izquierda;
                    }

                    break;

                case Keys.Right:

                    if(serpiente.DireccionActual 
                        != Direccion.Izquierda)
                    {
                        serpiente.DireccionActual = Direccion.Derecha;
                    }

                    break;
            }   
        }
        
        private void panelJuego_Paint(Object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.Clear(Color.Black);

            using Brush brushSerpiente = 
                new SolidBrush(Color.LimeGreen);    

            using Brush brushCabeza =
                new SolidBrush(Color.Green);    

            using Brush brushComida =
                new SolidBrush(Color.Red);

            foreach (Point punto in serpiente.Cuerpo)
            {
                Rectangle rect = new Rectangle(
                    punto.X * TamanoCelda,
                    punto.Y * TamanoCelda,
                    TamanoCelda - 1,
                    TamanoCelda - 1
                );

                g.FillRectangle(
                    punto == serpiente.ObtenerCabeza()
                        ? brushCabeza
                        : brushSerpiente,
                    rect
                );  
            }

            Rectangle comidaRect = 
                new Rectangle(
                    comida.Posicion.X * TamanoCelda,
                    comida.Posicion.Y * TamanoCelda,
                    TamanoCelda - 1,
                    TamanoCelda - 1
                );

            g.FillEllipse(
                brushComida,
                comidaRect
            );
        }   

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
