using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Program
{
    const string HOST = "172.16.105.7"; // Cambia a la dirección IP del servidor
    const int PUERTO = 12345; // Asegúrate de que el puerto coincida con el del servidor

    static void Main(string[] args)
    {
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            socket.Connect(HOST, PUERTO);
            using (var stream = new NetworkStream(socket))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                // Recibir lista de imágenes
                var listaImagenes = reader.ReadLine();
                Console.WriteLine("Imágenes disponibles: " + listaImagenes);
                var imagenes = listaImagenes.Split(',');

                // Seleccionar imagen
                Console.WriteLine("Escribe el nombre de la imagen que deseas abrir:");
                var seleccion = Console.ReadLine();

                // Solicitar imagen
                writer.WriteLine(seleccion);
                writer.Flush();

                // Recibir respuesta del servidor
                var respuesta = reader.ReadLine();
                if (respuesta == "OK")
                {
                    // Recibir imagen
                    var longitud = Convert.ToInt32(reader.ReadLine());
                    var bytesImagen = new byte[longitud];
                    stream.Read(bytesImagen, 0, longitud);

                    // Guardar y abrir imagen
                    var rutaImagen = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), seleccion);
                    File.WriteAllBytes(rutaImagen, bytesImagen);
                    Process.Start("explorer.exe", rutaImagen);
                }
                else if (respuesta == "ERROR")
                {
                    Console.WriteLine("La imagen solicitada no existe en el servidor.");
                }
            }
        }
    }
}
