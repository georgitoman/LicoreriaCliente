using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Helpers
{
    public enum Tablas
    {
        Usuarios = 0, Productos = 1, Categorias = 2, Pedidos = 3, ProductosPedido = 4
    }

    public static class ToolkitService
    {
        public static bool CompararArrayBytes(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Equals(b[i]) == false)
                {
                    iguales = false;
                    break;
                }
            }
            return iguales;
        }

        public static String NormalizeName(String extension, String nombre, String litros)
        {
            String lit = litros.Replace(".", "");
            return nombre + " " + lit + extension;
        }

        public static String SerializeJsonObject(object objeto)
        {
            String respuesta = JsonConvert.SerializeObject(objeto);
            return respuesta;
        }

        public static T DeserializeJsonObject<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


    }
}
