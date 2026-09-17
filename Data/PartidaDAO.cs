using Oracle.ManagedDataAccess.Client;
using SnakeOracle19C.Models;

namespace SnakeOracle19C.Data
{
    public class PartidaDAO
    {
        private readonly ConexionOracle conexion;

        public PartidaDAO()
        {
            conexion = new ConexionOracle();
        }

        public int ObtenerOCrearJugador(string nombre)
        {
            using OracleConnection connection = conexion.obtenerConexion();

            connection.Open();

            string sqlBuscar = @"
               SELECT ID_JUGADOR
               FROM JUGADORES 
               WHERE UPPER(NOMBRE) = UPPER(:NOMBRE)
               FECTH FIRST 1 ROW ONLY";

            using OracleCommand commandBuscar = 
                new OracleCommand(sqlBuscar, connection);

            commandBuscar.Parameters.Add(
                ":NOMBRE",
                OracleDbType.Varchar2).Value = nombre;

            object? resultado = commandBuscar.ExecuteScalar();

            if (resultado != null && resultado != DBNull.Value)
            {
                return Convert.ToInt32(resultado);
            }

            string sqlInsertar = @"
                INSERT INTO JUGADORES 
                (
                    NOMBRE, 
                    FECHA_REGISTRO
                )
                VALUES 
                (   :NOMBRE, 
                    SYSDATE
                )
                RETURNING ID_JUGADOR INTO :ID";


            using OracleCommand commandInsertar = 
                new OracleCommand(sqlInsertar, connection);

            commandInsertar.Parameters.Add(
                ":NOMBRE",
                OracleDbType.Varchar2).Value = nombre;

            OracleParameter parametroId =
                new OracleParameter(":ID", OracleDbType.Int32)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

            commandInsertar.Parameters.Add(parametroId);
            commandInsertar.ExecuteNonQuery();

            commandInsertar.ExecuteNonQuery();

            return Convert.ToInt32(parametroId.Value.ToString());

        }
    }
}
