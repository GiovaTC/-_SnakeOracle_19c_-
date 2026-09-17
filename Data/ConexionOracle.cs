using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeOracle19C.Data
{
    public class ConexionOracle
    {
        private readonly string connectionString =
            "User Id=system;" +
            "Password=Tapiero123;" +
            "Data Source=localhost:1521/orcl;"; 

        public OracleConnection obtenerConexion()
        {
            return new OracleConnection(connectionString);
        }
    }   
}
