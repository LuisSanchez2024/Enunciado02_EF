using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace VideojuegoRegistros
{
    public partial class Form1 : Form
    {
        private class Registro
        {
            public string NombreJuego { get; set; }
            public string Resultado { get; set; }
            public DateTime Fecha { get; set; }
        }

        private BindingList<Registro> registros = new BindingList<Registro>();
        private int totalPuntos = 0; // Variable para almacenar el total de puntos acumulados

        public Form1()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = registros;

            DataGridViewTextBoxColumn nombreJuegoColumn = new DataGridViewTextBoxColumn();
            nombreJuegoColumn.DataPropertyName = "NombreJuego";
          
            DataGridViewTextBoxColumn resultadoColumn = new DataGridViewTextBoxColumn();
            resultadoColumn.DataPropertyName = "Resultado";
           
            DataGridViewTextBoxColumn fechaColumn = new DataGridViewTextBoxColumn();
            fechaColumn.DataPropertyName = "Fecha";
            

            CargarDatos();
            ActualizarTotalPuntos();
        }

        private void buttonRegistrar_Click(object sender, EventArgs e)
        {
            string nombreJuego = textBoxNombreJuego.Text;
            string resultado = "";
            int puntos = 0;

            if (cbGanado.Checked)
            {
                resultado = "Ganado";
                puntos = 100;
            }
            else if (cbPerdido.Checked)
            {
                resultado = "Perdido";
                puntos = -20;
            }

            DateTime fecha = dateTimePicker1.Value;

            Registro nuevoRegistro = new Registro
            {
                NombreJuego = nombreJuego,
                Resultado = $"{resultado} ({puntos} puntos)",
                Fecha = fecha
            };

            registros.Add(nuevoRegistro);
            totalPuntos += puntos; // Calcular el total de puntos acumulados

            textBoxNombreJuego.Clear();
            dateTimePicker1.Value = DateTime.Now;
            cbGanado.Checked = false;
            cbPerdido.Checked = false;

            txtTotalPuntos.Text = totalPuntos.ToString(); // Mostrar el total de puntos acumulados en el TextBox

            GuardarDatos();
        }

        private void GuardarDatos()
        {
            using (StreamWriter sw = new StreamWriter("base_de_informacion.txt"))
            {
                sw.WriteLine(totalPuntos); // Guardar los puntos acumulados en la primera línea del archivo
                foreach (Registro registro in registros)
                {
                    sw.WriteLine($"{registro.NombreJuego},{registro.Resultado},{registro.Fecha}");
                }
            }
        }

        private void CargarDatos()
        {
            if (File.Exists("base_de_informacion.txt"))
            {
                using (StreamReader sr = new StreamReader("base_de_informacion.txt"))
                {
                    string linea = sr.ReadLine();
                    if (int.TryParse(linea, out int puntos))
                    {
                        totalPuntos = puntos; // Cargar los puntos acumulados desde la primera línea del archivo
                        txtTotalPuntos.Text = totalPuntos.ToString(); // Mostrar los puntos acumulados en el TextBox
                    }

                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] campos = linea.Split(',');
                        if (campos.Length == 3)
                        {
                            Registro registro = new Registro
                            {
                                NombreJuego = campos[0],
                                Resultado = campos[1],
                                Fecha = DateTime.Parse(campos[2])
                            };

                            registros.Add(registro);
                        }
                    }
                }
            }
        }

        private void ActualizarTotalPuntos()
        {

            totalPuntos = 0;
            foreach (Registro registro in registros)
            {
                int puntos;
                if (registro.Resultado.Contains("Ganado"))
                {
                    int indexPuntos = registro.Resultado.IndexOf("(") + 1;
                    int lengthPuntos = registro.Resultado.IndexOf(" puntos)") - indexPuntos;
                    if (int.TryParse(registro.Resultado.Substring(indexPuntos, lengthPuntos), out puntos))
                    {
                        totalPuntos += puntos;
                    }
                }
                else if (registro.Resultado.Contains("Perdido"))
                {
                    int indexPuntos = registro.Resultado.IndexOf("(") + 1;
                    int lengthPuntos = registro.Resultado.IndexOf(" puntos)") - indexPuntos;
                    if (int.TryParse(registro.Resultado.Substring(indexPuntos, lengthPuntos), out puntos))
                    {
                        totalPuntos -= Math.Abs(puntos); // Tomar el valor absoluto de los puntos perdidos para restarlos
                    }
                }
            }

            txtTotalPuntos.Text = totalPuntos.ToString();
        }

        private void btnAgregarNuevosDatos_Click(object sender, EventArgs e)
        {
            {
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas borrar los datos existentes y registrar nuevos datos?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    registros.Clear();
                    totalPuntos = 0;
                    txtTotalPuntos.Text = totalPuntos.ToString();

                    MessageBox.Show("VUELVE A REGISTRAR NUEVOS DATOS", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            {
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas salir de 'REGISTRO DE VIDEOJUEGOS'?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("TE ESPERAMOS PRONTO", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cbPerdido_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNombreJuego_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
 }
    
