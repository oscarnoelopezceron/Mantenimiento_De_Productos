using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entrevista
{
    public partial class Inicio : Form
    {
        private MV_Productos viewModel;

        public Inicio()
        {
            InitializeComponent();
            this.viewModel = new MV_Productos();
            //Listar
            dgvProductos.SelectionChanged += DgvProductos_Selection;
            //Permitir solo enteros o decimales
            txtPrecio.KeyPress += TxtPrecio_KeyPress;
            txtStock.KeyPress += TxtStock_KeyPress;
            txtBuscar.KeyPress += TxtBuscar_KeyPress;

        }
        private void btnListar_Click(object sender, EventArgs e)
        {
            dgvProductos.DataSource = viewModel.ObtenerProductos();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnListar_Click_1(object sender, EventArgs e)
        {
            var productos = viewModel.ObtenerProductos();
            dgvProductos.DataSource = productos;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtBuscar.Text);
                var producto = viewModel.BuscarProductoPorId(id);
                dgvProductos.DataSource = new List<Productos> { producto };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void DgvProductos_Selection(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                // Obtiene el producto seleccionado
                Productos producto = dgvProductos.SelectedRows[0].DataBoundItem as Productos;

                // Llena los TextBoxes con las propiedades del producto
                txtNombre.Text = producto.Nombre;
                txtDescripcion.Text = producto.Descripcion;
                txtPrecio.Text = producto.Precio.ToString();
                txtStock.Text = producto.Stock.ToString();
            }
        }
        //Validacion de tipo de datos

        private void TxtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Si el carácter no es un dígito, ni un control (como Backspace), ni un punto decimal, cancela el evento
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            // Solo permite un punto decimal
            else if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            // Solo permite dos decimales
            else if (char.IsDigit(e.KeyChar) && (sender as TextBox).Text.IndexOf('.') > -1
                && (sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length > 2)
            {
                e.Handled = true;
            }
        }
        private void TxtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Si el carácter no es un dígito ni un control (como Backspace), cancela el evento
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Si el carácter no es un dígito ni un control (como Backspace), cancela el evento
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        //fin de validacion de tipo de datos
        //agregar productos
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Verifica si todos los campos están llenos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Por favor, llena todos los campos antes de agregar un producto.");
                return;
            }

            try
            {
                // Crea un nuevo producto a partir de los valores de tus TextBoxes
                Productos producto = new Productos
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    Stock = int.Parse(txtStock.Text)
                };

                // Llama al método para agregar el producto
                viewModel.AgregarProducto(producto);

                // Muestra un mensaje de confirmación
                MessageBox.Show("El producto se agregó correctamente.");
                // Vuelve a cargar los datos en el DataGridView
                dgvProductos.DataSource = viewModel.ObtenerProductos();

                
                // Limpiar texbox
                txtNombre.Text = "";
                txtDescripcion.Text = "";
                txtPrecio.Text = "";
                txtStock.Text = "";
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si algo sale mal
                MessageBox.Show("Ocurrió un error al agregar el producto: " + ex.Message);
            }
        }
        
        //metodo para obtener id
        private Productos GetSelectedProduct()
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                return dgvProductos.SelectedRows[0].DataBoundItem as Productos;
            }
            else
            {
                return null;
            }
        }

        //metodo para actualizar
        private void btnActualizar_Click(object sender, EventArgs e)
        {

            // Verifica si todos los campos están llenos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Por favor, llena todos los campos antes de agregar un producto.");
                return;
            }
            Productos producto = GetSelectedProduct();
            if (producto != null)
            {
                // Actualiza el producto con los valores de tus TextBoxes
                producto.Nombre = txtNombre.Text;
                producto.Descripcion = txtDescripcion.Text;
                producto.Precio = decimal.Parse(txtPrecio.Text);
                producto.Stock = int.Parse(txtStock.Text);

                DialogResult dialogResult = MessageBox.Show("¿Estás seguro de que quieres actualizar el registro " + producto.Id + "?", "Confirmar", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // Llama al método para actualizar el producto
                    viewModel.ActualizarProducto(producto);

                    MessageBox.Show("El producto se actualizó correctamente.");

                    // Vuelve a cargar los datos en el DataGridView
                    dgvProductos.DataSource = viewModel.ObtenerProductos();

                    // Limpiar texbox
                    txtNombre.Text = "";
                    txtDescripcion.Text = "";
                    txtPrecio.Text = "";
                    txtStock.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto antes de actualizar.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Productos producto = GetSelectedProduct();
            if (producto != null)
            {
                // Muestra un cuadro de diálogo de confirmación
                DialogResult dialogResult = MessageBox.Show("¿Estás seguro de que quieres eliminar el registro " + producto.Id + "?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // Si el usuario hace clic en Sí, llama al metodo eliminar producto
                    viewModel.EliminarProducto(producto.Id);

                    MessageBox.Show("El producto se eliminó correctamente.");

                    // Vuelve a cargar los datos en el DataGridView
                    dgvProductos.DataSource = viewModel.ObtenerProductos();

                    txtNombre.Text = "";
                    txtDescripcion.Text = "";
                    txtPrecio.Text = "";
                    txtStock.Text = "";
                }

            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto antes de eliminar.");
            }
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
        }
    } 
}
