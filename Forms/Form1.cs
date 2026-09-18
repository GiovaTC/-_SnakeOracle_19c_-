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

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void FinalizarJuego()
        {
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
