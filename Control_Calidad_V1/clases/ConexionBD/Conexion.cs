using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_Calidad_V1.clases.ConexionBD
{
    class Conexion
    {
        MySqlConnection conexion = new MySqlConnection();
        static string servidor = "localhost";
        static string bd = "controlcalidad";
        static string usuario = "root";
        static string password = "0159874326";
        static string puerto = "3306";

        string cadenaConexion = "server=" + servidor + ";" + "port =" + puerto + ";" + "user id =" + usuario + ";" + "password=" + password + ";" + "database=" + bd + ";";
        public MySqlConnection EstablecerConexion()
        {
            try
            {
                conexion.ConnectionString = cadenaConexion;
                conexion.Open();
                
            }
            catch (MySqlException e)
            {
                MessageBox.Show("No se pudo conectar a la base de datos, /n error: " + e.ToString());
            }
            return conexion;
        }
        public void cerrarConexion()
        {
            conexion.Close();
        }
    }
}

