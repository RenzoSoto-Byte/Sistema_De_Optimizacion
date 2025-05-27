using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using MySql.Data.MySqlClient;
using Control_Calidad_V1.clases.ConexionBD;
using Control_Calidad_V1.clases.Entidades;
using Control_Calidad_V1.clases.Servicios;
using System.Collections.Generic;
using System.Globalization;

namespace Control_Calidad_V1
{
    public partial class Form1 : Form
    {
        private TextBox[] txtI1;
        private TextBox[] txtI2; // Declarar las variables de los TextBox
        private TextBox txtResultadoI1;
        private TextBox txtResultadoI2;
        private TextBox txtCostoMinimo;


        public Form1()
        {
            InitializeComponent();
            this.Text = "Control de Calidad Lingo C#";
            this.Width = 495;
            this.Height = 750;
            InitializeControls();
        }

        private void InitializeControls()
        {
            GroupBox groupBoxDatos = new GroupBox
            {
                Text = "Datos para LINGO",
                Bounds = new Rectangle(20, 20, 440, 300),
                Padding = new Padding(10)  

            };
            this.Controls.Add(groupBoxDatos);

            string[] labels = { "Piezas", "Sueldos", "Disponibilidad Max.", "Margen (%)", "Gasto / pieza errónea", "Horas", "Prod. Min." };
            txtI1 = new TextBox[7];
            txtI2 = new TextBox[7];
            // Añadir las etiquetas Inspector 1 y Inspector 2 encima de las columnas
            Label lblInspector1 = new Label
            {
                Text = "Inspector 1",
                Bounds = new Rectangle(150, 20, 100, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            groupBoxDatos.Controls.Add(lblInspector1);

            Label lblInspector2 = new Label
            {
                Text = "Inspector 2",
                Bounds = new Rectangle(300, 20, 100, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            groupBoxDatos.Controls.Add(lblInspector2);

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label
                {
                    Text = labels[i],
                    Bounds = new Rectangle(20, 50 + i * 35, 100, 30)
                };
                groupBoxDatos.Controls.Add(lbl);

                txtI1[i] = new TextBox { Bounds = new Rectangle(150, 50 + i * 35, 100, 30) };
                groupBoxDatos.Controls.Add(txtI1[i]);

                if (labels[i] != "Horas" && labels[i] != "Prod. Min.")
                {
                    txtI2[i] = new TextBox { Bounds = new Rectangle(300, 50 + i * 35, 100, 30) };
                    groupBoxDatos.Controls.Add(txtI2[i]);
                }
            }

            GroupBox groupBoxMetodo = new GroupBox
            {
                Text = "Elija un método",
                Bounds = new Rectangle(20, 340, 440, 80)
            };
            this.Controls.Add(groupBoxMetodo);

            Button btnLingo = new Button
            {
                Text = "Lingo",
                Bounds = new Rectangle(30, 30, 100, 30)
            };
            btnLingo.Click += BtnLingo_Click;
            groupBoxMetodo.Controls.Add(btnLingo);

            Button btnAcces = new Button
            {
                Text = "Acces",
                Bounds = new Rectangle(150, 30, 100, 30)
            };
            btnAcces.Click += BtnAcces_Click;
            groupBoxMetodo.Controls.Add(btnAcces);

            Button btnLimpiar = new Button
            {
                Text = "Limpiar",
                Bounds = new Rectangle(270, 30, 100, 30)
            };
            btnLimpiar.Click += (s, e) => LimpiarCampos(txtI1, txtI2);
            groupBoxMetodo.Controls.Add(btnLimpiar);

            GroupBox groupBoxResultado = new GroupBox
            {
                Text = "Resultado (Cantidad)",
                Bounds = new Rectangle(20, 440, 440, 150)
            };
            this.Controls.Add(groupBoxResultado);

            Label lblI1 = new Label
            {
                Text = "I1:",
                Bounds = new Rectangle(20, 30, 50, 30)
            };
            groupBoxResultado.Controls.Add(lblI1);

            txtResultadoI1 = new TextBox
            {
                Name = "txtResultadoI1",
                Bounds = new Rectangle(100, 30, 100, 30),
                ReadOnly = true,
                Enabled = false,  // Desactiva la capacidad de seleccionarlo
                BackColor = Color.LightYellow,  // Fondo amarillo claro
                ForeColor = Color.Blue,         // Texto en azul
                Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Bold),
                TabStop = false,
                TextAlign = HorizontalAlignment.Center  // Texto centrado
            };
            groupBoxResultado.Controls.Add(txtResultadoI1);

            Label lblI2 = new Label
            {
                Text = "I2:",
                Bounds = new Rectangle(20, 80, 50, 30)
            };
            groupBoxResultado.Controls.Add(lblI2);

            txtResultadoI2 = new TextBox
            {
                Name = "txtResultadoI2",
                Bounds = new Rectangle(100, 80, 100, 30),
                ReadOnly = true,
                Enabled = false,  // Desactiva la capacidad de seleccionarlo
                BackColor = Color.LightYellow,  // Fondo amarillo claro
                ForeColor = Color.Blue,         // Texto en azul
                Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Bold),
                TabStop = false,
                TextAlign = HorizontalAlignment.Center  // Texto centrado
            };
            groupBoxResultado.Controls.Add(txtResultadoI2);

            Label lblCostoMinimo = new Label
            {
                Text = "Costo Total Mínimo:",
                Bounds = new Rectangle(220, 30, 150, 30)
            };
            groupBoxResultado.Controls.Add(lblCostoMinimo);

            txtCostoMinimo = new TextBox
            {
                Name = "txtCostoMinimo",
                Bounds = new Rectangle(220, 60, 150, 30),
                ReadOnly = true,
                Enabled = false,  // Desactiva la capacidad de seleccionarlo
                BackColor = Color.LightYellow,
                ForeColor = Color.DarkGreen,
                Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Bold),
                TabStop = false,
                TextAlign = HorizontalAlignment.Center
            };
            groupBoxResultado.Controls.Add(txtCostoMinimo);


            Button btnExit = new Button
            {
                Text = "Salir",
                BackColor = Color.LightCoral,
                Bounds = new Rectangle(250, 620, 120, 40)
            };
            btnExit.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnExit);
             // Asegurarse de que solo se ingresen números en los TextBox
    foreach (var txt in txtI1)
    {
        if (txt != null)
        {
            txt.KeyPress += Txt_KeyPress;
        }
    }

    foreach (var txt in txtI2)
    {
        if (txt != null)
        {
            txt.KeyPress += Txt_KeyPress;
        }
    }
        }

        private async void BtnLingo_Click(object sender, EventArgs e)
        {
            try
            {
                var datos = new DatosLingo
                {
                    Piezas = new double[] { Convert.ToDouble(txtI1[0].Text), Convert.ToDouble(txtI2[0].Text) },
                    Sueldos = new double[] { Convert.ToDouble(txtI1[1].Text), Convert.ToDouble(txtI2[1].Text) },
                    Capacidades = new double[] { Convert.ToDouble(txtI1[2].Text), Convert.ToDouble(txtI2[2].Text) },
                    Margenes = new double[] { Convert.ToDouble(txtI1[3].Text), Convert.ToDouble(txtI2[3].Text) },
                    CostosError = new double[] { Convert.ToDouble(txtI1[4].Text), Convert.ToDouble(txtI2[4].Text) },
                    Horas = Convert.ToDouble(txtI1[5].Text),
                    PiezasMin = Convert.ToDouble(txtI1[6].Text)
                };

                var servicio = new ServicioLingo();
                ResultadoLingo resultado = await servicio.EjecutarModeloLingoAsync(datos);

                txtResultadoI1.Text = resultado.Inspector1.ToString();
                txtResultadoI2.Text = resultado.Inspector2.ToString();
                txtCostoMinimo.Text = resultado.CostoTotalMinimo.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void BtnAcces_Click(object sender, EventArgs e)
        {
            try
            {
                var inspectores = ServicioDatos.ObtenerInspectores();
                if (inspectores.Count >= 2)
                {
                    // I1
                    txtI1[0].Text = inspectores[0].InspeccionesHora.ToString();
                    txtI1[1].Text = inspectores[0].SueldoHora.ToString();
                    txtI1[2].Text = inspectores[0].Capacidad.ToString();
                    txtI1[3].Text = inspectores[0].MargenPorcentaje.ToString(CultureInfo.InvariantCulture);
                    txtI1[4].Text = inspectores[0].CostoErrorxProducto.ToString();

                    // I2
                    txtI2[0].Text = inspectores[1].InspeccionesHora.ToString();
                    txtI2[1].Text = inspectores[1].SueldoHora.ToString();
                    txtI2[2].Text = inspectores[1].Capacidad.ToString();
                    txtI2[3].Text = inspectores[1].MargenPorcentaje.ToString(CultureInfo.InvariantCulture);
                    txtI2[4].Text = inspectores[1].CostoErrorxProducto.ToString();
                }

                var (horas, piezasMin) = ServicioDatos.ObtenerRestricciones();
                txtI1[5].Text = horas.ToString();
                txtI1[6].Text = piezasMin.ToString();

                MessageBox.Show("Datos cargados correctamente desde la base de datos.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al acceder a la base de datos: " + ex.Message);
            }
        }

        private void LimpiarCampos(TextBox[] txtI1, TextBox[] txtI2)
        {
            // Limpiar los campos de datos
            foreach (var txt in txtI1)
                if (txt != null) txt.Text = "";

            foreach (var txt in txtI2)
                if (txt != null) txt.Text = "";

            // Limpiar los campos de resultados
            txtResultadoI1.Text = "";
            txtResultadoI2.Text = "";
            txtCostoMinimo.Text = "";
        }
        // Evento para permitir solo números y el punto decimal
        private void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            // Permitir teclas de control (como retroceso)
            if (char.IsControl(e.KeyChar))
                return;

            // Evitar que el primer dígito sea un punto
            if (e.KeyChar == '.' && txt.Text.Length == 0)
            {
                e.Handled = true;  // Cancela la entrada si es el primer carácter
                return;
            }

            // Permitir números
            if (char.IsDigit(e.KeyChar))
            {
                // Limitar a 5 caracteres sin contar el punto decimal
                int numDigits = txt.Text.Replace(".", "").Length;
                if (numDigits >= 5)
                {
                    e.Handled = true;  // Cancela si ya hay 5 dígitos
                }
                return;
            }

            // Permitir el punto decimal solo si aún no existe
            if (e.KeyChar == '.' && !txt.Text.Contains('.'))
            {
                return;
            }

            // Si no cumple ninguna condición válida, se cancela la entrada
            e.Handled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
