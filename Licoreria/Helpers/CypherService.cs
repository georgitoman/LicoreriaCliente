using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Licoreria.Helpers
{
    public static class CypherService
    {
        public static byte[] CifrarContenido(String contenido, String salt)
        {
            String contenidosalt = contenido + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            salida = Encoding.UTF8.GetBytes(contenidosalt);
            for (int i = 0; i < 100; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();

            return salida;
        }

        public static String GetSalt()
        {
            Random rand = new Random();
            String salt = "";
            for (int i = 1; i <= 50; i++)
            {
                int aleat = rand.Next(0, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }

            return salt;
        }
    }
}
