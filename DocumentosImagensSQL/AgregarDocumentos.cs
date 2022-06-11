using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace DocumentosImagensSQL
{
    public partial class AgregarDocumentos : Form
    {
        Documentos objDoc = new Documentos();
        public AgregarDocumentos()
        {
            InitializeComponent();
        }

        private void AgregarDocumentos_Load(object sender, EventArgs e)
        {

        }

        private void Examinar_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\Documentos";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                txtRuta.Text = openFileDialog1.FileName;
            }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            byte[] archivo = null;
            Stream MyStream = openFileDialog1.OpenFile();
            MemoryStream obj = new MemoryStream();
            MyStream.CopyTo(obj);
            archivo = obj.ToArray();

            //Agregar
            switch (LbEditar.Text) {
                case "Agregar":
                    objDoc.Nombre = txtTitulo.Text;
                    objDoc.Documento = archivo;
                    objDoc.Extension = openFileDialog1.SafeFileName;
                    MessageBox.Show(objDoc.AgregarDocumento());
                    break;

                case "Editar":
                    int id = Convert.ToInt32(txtID.Text);
                    objDoc.Id = id;
                    objDoc.Nombre = txtTitulo.Text;
                    objDoc.Documento = archivo;
                    var Lista = new List<Documentos>();
                    Lista = objDoc.filtroDocumentos();

                    foreach (Documentos item in Lista)
                    {
                        string direecion = AppDomain.CurrentDomain.BaseDirectory;
                        string carpeta = direecion + "/temp/";
                        string ubicacionCompleta = carpeta + item.Extension;

                        if (Directory.Exists(carpeta))
                            File.Delete(ubicacionCompleta);
                    }
                    objDoc.Extension = openFileDialog1.SafeFileName;
                    MessageBox.Show(objDoc.ActualizarDocumentos());
                    break;

                default:
                    MessageBox.Show("No existe");
                    break;
            }
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
