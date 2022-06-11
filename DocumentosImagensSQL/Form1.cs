using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DocumentosImagensSQL
{
    public partial class Form1 : Form
    {
        Documentos objDoc = new Documentos();
        public Form1()
        {
            InitializeComponent();
            LlenarData();
        }
        private void LlenarData() {
            dgv.DataSource = objDoc.ListarDocumentos();
        }
        private void Agregar_Click(object sender, EventArgs e)
        {
            AgregarDocumentos agr = new AgregarDocumentos();
            agr.LbEditar.Text = "Agregar";
            agr.Size = new Size(446,174);
            agr.label2.Location = new Point(11,43);
            agr.txtRuta.Location = new Point(135,37);
            agr.label3.Location = new Point(11, 71);
            agr.txtTitulo.Location = new Point(135, 65);
            agr.Guardar.Location = new Point(141,94);
            agr.Cancelar.Location = new Point(231,94);
            agr.ShowDialog();
            LlenarData();
        }

        private void VerArchivo_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0) {
                int id = Convert.ToInt32(dgv.CurrentRow.Cells[0].Value.ToString());
                objDoc.Id = id;
                var Lista = new List<Documentos>();
                Lista = objDoc.filtroDocumentos();

                foreach (Documentos item in Lista) {
                    //Crear Carpeta temporal donde se guardaran archivos
                    string direecion = AppDomain.CurrentDomain.BaseDirectory;
                    string carpeta = direecion + "/temp/";
                    string ubicacionCompleta = carpeta + item.Extension;

                    //Validaciones 
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    if (File.Exists(ubicacionCompleta))
                        File.Delete(ubicacionCompleta);

                    File.WriteAllBytes(ubicacionCompleta, item.Documento);
                    Process.Start(ubicacionCompleta);
                }
            }
        }

        private void Editar_Click(object sender, EventArgs e)
        {
            AgregarDocumentos agr = new AgregarDocumentos();
            agr.LbEditar.Text = "Editar";
            agr.label4.Visible = true;
            agr.txtID.Visible = true;

            if (dgv.SelectedRows.Count > 0)
            {
                agr.txtID.Text = Convert.ToString(dgv.CurrentRow.Cells[0].Value.ToString());
                agr.txtTitulo.Text = Convert.ToString(dgv.CurrentRow.Cells[1].Value.ToString());
            }
            else {
                MessageBox.Show("Seleccione un campo");
            }
            agr.ShowDialog();
            LlenarData();
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0) {
                int id = Convert.ToInt32(dgv.CurrentRow.Cells[0].Value.ToString());

                objDoc.Id = id;
                var Lista = new List<Documentos>();
                Lista = objDoc.filtroDocumentos();

                foreach (Documentos item in Lista) {
                    string direecion = AppDomain.CurrentDomain.BaseDirectory;
                    string carpeta = direecion + "/temp/";
                    string ubicacionCompleta = carpeta + item.Extension;

                    if (Directory.Exists(carpeta))
                        File.Delete(ubicacionCompleta);
                }
                MessageBox.Show(objDoc.EliminarDocumento());
            }
            LlenarData();
        }
    }
}
