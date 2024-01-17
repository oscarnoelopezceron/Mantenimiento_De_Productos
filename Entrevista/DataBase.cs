using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Conexion
namespace Entrevista
{
    internal class DataBase
    {
        
        private string connectionString = "Data Source=(local);Initial Catalog=Productos;Integrated Security=True";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Otros métodos para realizar operaciones CRUD...
        public bool ProbarConexion()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        //Procedimientos almacenados
        //Listar
        public DataTable EjecutarListar(string mostrar_productos)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conexion = GetConnection()) 
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(mostrar_productos, conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                    {
                        conexion.Close();
                    }
                }

            return dt;
        }


    }
}
