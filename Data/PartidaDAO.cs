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

            return Convert.ToInt32(parametroId.Value.ToString());
        }

        public void GuardarPrtida(Partida partida)
        {
            using OracleConnection connection = conexion.obtenerConexion();
            connection.Open();

            string sql = @"
                INSERT INTO PARTIDAS 
                (
                    ID_JUGADOR, 
                    PUNTUACION, 
                    LONGITUD_SERPIENTE, 
                    RESULTADO, 
                    FECHA_PARTIDA
                )
                VALUES 
                (   :ID_JUGADOR, 
                    :PUNTUACION, 
                    :LONGITUD_SERPIENTE, 
                    :RESULTADO, 
                    SYSDATE
                )";

            using OracleCommand command =
                new OracleCommand(sql, connection);

            command.Parameters.Add(
                ":ID_JUGADOR",
                OracleDbType.Int32
                ).Value = partida.IdJugador;

            command.Parameters.Add(
                ":PUNTUACION",
                OracleDbType.Int32
                ).Value = partida.Puntuacion;
            
            command.Parameters.Add(
                ":LONGITUD",
                OracleDbType.Int32
                ).Value = partida.LongitudSerpiente;
            
            command.Parameters.Add(
                ":RESULTADO",
                OracleDbType.Varchar2
                ).Value = partida.Resultado;
            
            command.ExecuteNonQuery();
        }

        public List<PartidaRanking> ObtenerRanking()
        {
            List<PartidaRanking> lista = new();

            using OracleConnection connection = conexion.obtenerConexion();
            connection.Open();

            string sql = @"
                SELECT *
                FROM
                (
                    SELECT
                        J.NOMBRE,
                        P.PUNTUACION,
                        P.LONGITUD_SERPIENTE,
                        P.RESULTADO,
                        P.FECHA_PARTIDA
                    FROM PARTIDAS P
                    INNER JOIN JUGADORES J
                        ON J.ID_JUGADOR = P.ID_JUGADOR
                    ORDER BY P.PUNTUACION DESC
                )
                WHERE ROWNUM <= 10";

            using OracleCommand command = 
                new OracleCommand(sql, connection);

            using OracleDataReader reader = 
                command.ExecuteReader();
            
            while (reader.Read())
            {
                lista.Add(new PartidaRanking
                {
                    Nombre = reader["NOMBRE"].ToString() ?? "",
                    Puntuacion = Convert.ToInt32(reader["PUNTUACION"]),
                    Longitud = Convert.ToUInt32(
                        reader["LONGITUD_SERPIENTE"]),
                    Resultado = reader["RESULTADO"].ToString() ?? "",
                    Fecha = Convert.ToDateTime(
                        reader["FECHA_PARTIDA"])
                });
            }

            return lista;
            
        }
    }
}
