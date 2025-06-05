using Control_Calidad_V1.clases.Entidades;
using Control_Calidad_V1.clases.ConexionBD;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control_Calidad_V1.clases.Servicios
{
    class ServicioDatos
    {
        /// <summary>
        /// Obtiene los primeros 2 inspectores desde la base de datos.
        /// </summary>
        /// <returns>Lista de objetos Inspector</returns>
        public static List<inspector> ObtenerInspectores()
        {
            List<inspector> lista = new List<inspector>();
            Conexion ejecutarConexion = new Conexion();
            MySqlConnection conn = ejecutarConexion.EstablecerConexion();

            string query = "SELECT * FROM inspectores ORDER BY idInspectores ASC LIMIT 2";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new inspector
                {
                    InspeccionesHora = Convert.ToInt32(reader["inspeccionesHora"]),
                    SueldoHora = Convert.ToDecimal(reader["SueldoHora"]),
                    Capacidad = Convert.ToInt32(reader["Capacidad"]),
                    MargenPorcentaje = Convert.ToDecimal(reader["MargenPorcentaje"]),
                    CostoErrorxProducto = Convert.ToDecimal(reader["CostoErrorxProducto"])
                });
            }

            reader.Close();
            ejecutarConexion.cerrarConexion();

            return lista;
        }

        /// <summary>
        /// Obtiene las restricciones desde la base de datos.
        /// </summary>
        /// <returns>Objeto de la clase restricciones</returns>
        public static restricciones ObtenerRestricciones()
        {
            restricciones restriccion = null;
            Conexion ejecutarConexion = new Conexion();
            MySqlConnection conn = ejecutarConexion.EstablecerConexion();

            string query = "SELECT jornadaDiariaHoras, piezasMinimas FROM restricciones LIMIT 1";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                restriccion = new restricciones
                {
                    jornadaDiariaHoras = Convert.ToInt32(reader["jornadaDiariaHoras"]),
                    piezasMinimas = Convert.ToDecimal(reader["piezasMinimas"])
                };
            }

            reader.Close();
            ejecutarConexion.cerrarConexion();

            return restriccion;
        }
    }
}
