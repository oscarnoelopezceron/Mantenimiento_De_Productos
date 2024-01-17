using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entrevista
{
    internal class MV_Productos
    {
        private DataBase db;

        public MV_Productos()
        {
            this.db = new DataBase();

            if (db.ProbarConexion())
            {
                MessageBox.Show("Conexión exitosa a la base de datos.");
            }
            else
            {
                MessageBox.Show("No se pudo conectar a la base de datos.");
            }

           
        }

        public List<Productos> ObtenerProductos()
        {
            List<Productos> productos = new List<Productos>();

            using (SqlConnection conn = db.GetConnection()) 
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("mostrar_productos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Productos producto = new Productos
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                                Stock = reader.GetInt32(reader.GetOrdinal("stock"))
                            };

                            productos.Add(producto);
                        }
                    }
                }
            }

            return productos;
        }

        //NEtodo buscar por id
        public Productos BuscarProductoPorId(int id)
        {
            Productos producto = null;

            using (SqlConnection conn = db.GetConnection()) // Asume que tienes un método GetConnection() en tu clase DataBase
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("buscar_p_por_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Productos
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                                Stock = reader.GetInt32(reader.GetOrdinal("stock"))
                            };
                        }
                        else
                        {
                            throw new Exception("No se encontró ningún producto con el ID proporcionado.");
                        }
                    }
                }
            }

            return producto;
        }
        //fin de metodo buscar por id
        //Metodo agregar
        public void AgregarProducto(Productos producto)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("crear_producto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        //actualizar

        public void ActualizarProducto(Productos producto)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("actualizar_producto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", producto.Id);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Eliminar
        public void EliminarProducto(int id)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("eliminar_producto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
