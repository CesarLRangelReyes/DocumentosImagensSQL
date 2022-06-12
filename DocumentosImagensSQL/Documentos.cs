using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DocumentosImagensSQL
{
    public class Documentos
    {
        private int id;
        private string nombre;
        private byte[] documento;
        private string extension;
        private string hola;

        SqlConnection conexion = new SqlConnection("server=DESKTOP-N1Q1432; database= ImagenSQLSERVER; integrated security=true");

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public byte[] Documento { get => documento; set => documento = value; }
        public string Extension { get => extension; set => extension = value; }

        public string AgregarDocumento() {
            conexion.Open();
            SqlCommand comando = new SqlCommand("insert into Document values(@nombre, @documento, @extension)", conexion);
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@nombre",Nombre);
            comando.Parameters.AddWithValue("@documento",Documento);
            comando.Parameters.AddWithValue("@extension",Extension);
            comando.ExecuteNonQuery();
            conexion.Close();
            return "Agregado con éxito";
        }

        public string ActualizarDocumentos() {
            conexion.Open();
            SqlCommand comando = new SqlCommand("Update Document set NOMBRE = @nombre, DOCUMENTO = @documento, EXTENSION = @extension where ID = @id", conexion);
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@id", Id);
            comando.Parameters.AddWithValue("@nombre", Nombre);
            comando.Parameters.AddWithValue("@documento", Documento);
            comando.Parameters.AddWithValue("@extension", Extension);
            comando.ExecuteNonQuery();
            conexion.Close();
            return "Modificado con éxito";
        }

        public string EliminarDocumento() {
            conexion.Open();
            SqlCommand comando = new SqlCommand("delete from Document where ID = @id",conexion);
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@id", Id);
            comando.ExecuteNonQuery();
            conexion.Close();
            return "Elimando con éxito";
        }
        public DataTable ListarDocumentos() {
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand comando = new SqlCommand("Select Id, Nombre from Document",conexion);
            SqlDataReader resultado = comando.ExecuteReader();

            if (resultado.HasRows) tabla.Load(resultado);

            resultado.Close();
            conexion.Close();
            return tabla;
        }

        public DataTable ArchivoPorID() {
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand comando = new SqlCommand("select * from Document where ID = @id", conexion);
            comando.CommandType = CommandType.Text;
            comando.Parameters.AddWithValue("@id", Id);
            SqlDataReader lector = comando.ExecuteReader();
            tabla.Load(lector);
            lector.Close();
            conexion.Close();
            return tabla;
        }

        public List<Documentos> filtroDocumentos() {
            var tabla = ArchivoPorID();
            var infoDocumento = new List<Documentos>();
            foreach (DataRow item in tabla.Rows) {
                infoDocumento.Add(new Documentos {
                    Id = Convert.ToInt32(item[0]),
                    Nombre = item[1].ToString(),
                    Documento = (byte[])item[2],
                    Extension = item[3].ToString()
                });
            }
            return infoDocumento;
        }
    }
}
