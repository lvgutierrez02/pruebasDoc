using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba2.Models;
using System.Data;
using System.Data.SqlClient;

namespace prueba2.Controllers
{
    [Route("api/Documento")]
    [ApiController]
    public class DocumentosController : ControllerBase
    {
        private readonly string _rutaServidor;
        private readonly string _cadenaSql;

        public DocumentosController(IConfiguration config)//Obtiene información del archivo appsettings.json
        {
            _rutaServidor = config.GetSection("Configuracion").GetSection("RutaServidor").Value;
            _cadenaSql = config.GetConnectionString("ConexionSql");
        }

        [HttpPost]
        [Route("Documento")]
        public IActionResult Subir([FromForm] Documento request)
        {//recibe un documento desde el cuerpo de un formulario

            string rutaDocumento = Path.Combine(_rutaServidor, request.Archivo.FileName); //combinar rutas (concatena la ruta del servidor junto con el nombre con el que se guarda el doc)

            try
            {
                using (FileStream newFile = System.IO.File.Create(rutaDocumento))//el documento recibido se va a guardar en la ruta del servidor
                {

                    request.Archivo.CopyTo(newFile);    //
                    newFile.Flush();//
                }

                using (var conexion = new SqlConnection(_cadenaSql))
                { //declara conexion con la Db
                    conexion.Open();//Abrir la Db
                    var cmd = new SqlCommand("sp_guardar_documento", conexion);//declarar comando 
                    cmd.Parameters.AddWithValue("Descripcion", request.Descripción);//al comando se le envian parametros 
                    cmd.Parameters.AddWithValue("Ruta", rutaDocumento);//se envia la ruta en donde se va a guardar por medio del parametro
                    cmd.CommandType = CommandType.StoredProcedure;//
                    cmd.ExecuteNonQuery(); // para que la operacion del comando sea ejecutada

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "documento guardado" });//codigo de todo ha sido bien

            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status200OK, new { mensaje = error.Message });//Enviar error
            }
        }

    }
}
