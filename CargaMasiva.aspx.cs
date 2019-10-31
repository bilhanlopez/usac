using _IPC2_Fase2_201612369.Models;
using _IPC2_Proyecto_201612369.Model;
using ChoETL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;

namespace _IPC2_Proyecto_201612369.Views
{
    public partial class CargaMasiva : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
           LeerCsv();
        }

        string fileName = string.Empty;
        protected void Archivo_FilesUploaded(object source, DevExpress.Web.FileManagerFilesUploadedEventArgs e)
        {
            
            fileName = e.Files[0].FullName;
        }

        private async void LeerCsv()
        {
            List<User> data = new List<User> { };
            try
            {
                foreach (dynamic e in new ChoCSVReader("data.csv").WithFirstLineHeader())
                {
                    int idRol = await CargarRoles(e["Rol\n"]);

                    if (idRol != -1)
                    {
                        CargarClientes(e, idRol);
                    }
                }

            }
            catch (Exception ex)
            {
                data = new List<User> { };
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<int> CargarRoles(string nombreRol)
        {
            string query = @"if not exists (select top 1 * from rol where upper(nombreRol) = upper(" + nombreRol + ")" +
                " begin insert into rol (nombreRol) output INSERTED.ID (" + nombreRol + ") end" +
                " else select top 1 idRol from rol where upper(nombreRol) = upper(" + nombreRol + ")";

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.conexion))
                {
                    await cn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        var id = await cmd.ExecuteScalarAsync();

                        return Int32.Parse(id.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                return -1;
            }

        }


        private void CargarClientes(User user, int idRol)
        {
            string query = @"insert into usuario (dpi, nombre, apellido, fechaDeNacimiento, correoElectronico, telefono, usuario,
                            contrasena, palabraClave, rol) 
                            values('" + user.Dpi + "', '" + user.Nombre + "', " +
                            "'" + user.Apellido + "', '" + DateTime.Parse(user.FechaDeNacimiento).ToShortDateString() + "', '" +
                            user.CorreoElectronico + "', '" + user.Telefono + "', '" + user.Usuario + "', '" + user.Contrasena + "', '" +
                            user.PalabraClave + "', " + idRol + ")";

            using (SqlConnection cn = new SqlConnection(Conexion.conexion))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

//using (SqlBulkCopy bcp = new SqlBulkCopy(Conexion.conexion))
//{
//    using (var p = new ChoCSVReader(e.Files[0].FullName).WithFirstLineHeader())
//    {
//        bcp.DestinationTableName = "USUARIO";
//        bcp.EnableStreaming = true;
//        bcp.BatchSize = 10000;
//        bcp.BulkCopyTimeout = 0;
//        bcp.NotifyAfter = 100;
//        bcp.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs ev)
//        {
//            Console.WriteLine(ev.RowsCopied.ToString("#,##0") + " rows copied.");
//        };
//        bcp.WriteToServer(p.AsDataReader());
//    }
//}

//List<User> data = new List<User> { };
//            try
//            {
//                using (SqlBulkCopy bcp = new SqlBulkCopy(Conexion.conexion))
//                {
//                    using (var p = new ChoCSVReader("").WithFirstLineHeader())
//                    {
//                        bcp.DestinationTableName = "USUARIO";
//                        //bcp.EnableStreaming = true;
//                        //bcp.BatchSize = 10000;
//                        //bcp.BulkCopyTimeout = 0;
//                        //bcp.NotifyAfter = 100;
//                        //bcp.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
//                        //{
//                        //    Console.WriteLine(e.RowsCopied.ToString("#,##0") + " rows copied.");
//                        //};
//                        bcp.WriteToServer(p.AsDataReader());
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                data = new List<User> { }; ;
//                Console.WriteLine(ex.Message);
//            }