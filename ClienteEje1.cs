using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
//using System.Threading;

class Program

{
    const string HOST = "172.16.105.45"; // Cambia a la dirección IP del servidor
    const int PUERTO = 12345; // Asegúrate de que el puerto coincida con el del servidor

    static void Main(string[] args)
    {
        try
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var result = socket.BeginConnect(HOST, PUERTO, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                if (success)
                {
                    using (var stream = new NetworkStream(socket))
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string mensajeDelServidor = reader.ReadLine();
                        Console.WriteLine("Fecha y hora del servidor: " + mensajeDelServidor);
                    }
                }
                else
                {
                    Console.WriteLine("Error: El servidor no respondió en el tiempo esperado.");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al conectar con el servidor: " + e.Message);
        }
    }
}
