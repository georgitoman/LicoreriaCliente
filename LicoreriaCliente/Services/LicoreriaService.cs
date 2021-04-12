using ClienteApiSeriesGeorge.Models;
using LicoreriaCliente.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LicoreriaCliente.Services
{
    public class LicoreriaService
    {
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;

        public LicoreriaService(String url)
        {
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<String> GetToken(String username, String password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                LoginModel login = new LoginModel();
                login.Username = username;
                login.Password = password;

                String json = JsonConvert.SerializeObject(login);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                String request = "/api/Auth/Login";
                HttpResponseMessage response = await
                    client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    String data = await
                        response.Content.ReadAsStringAsync();
                    JObject jobject = JObject.Parse(data);
                    String token = jobject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApi<T>(String request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApi<T>(String request, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        #region CATEGORIAS

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            String request = "/api/Categorias";
            return await this.CallApi<List<Categoria>>(request);
        }

        public async Task<String> GetNombreCategoriaAsync(int id)
        {
            String request = "/api/Categorias/GetNombreCategoria/" + id;
            return await this.CallApi<String>(request);
        }

        #endregion

        #region PRODUCTOS

        public async Task<List<Producto>> GetProductosAsync(String nombre, decimal? preciomax,
            decimal? litros, bool? stock, int? idcategoria)
        {
            String request = "/api/Productos";

            if(nombre != null)
            {
                request += "?nombre=" + nombre;
            }
            if(preciomax != null)
            {
                if (request.Contains('?'))
                {
                    request += "&preciomax=" + preciomax;
                } else
                {
                    request += "?preciomax=" + preciomax;
                }
            }
            if (litros != null)
            {
                if (request.Contains('?'))
                {
                    request += "&litros=" + litros;
                }
                else
                {
                    request += "?litros=" + litros;
                }
            }
            if (stock != null)
            {
                if (request.Contains('?'))
                {
                    request += "&stock=" + stock;
                }
                else
                {
                    request += "?stock=" + stock;
                }
            }
            if (idcategoria != null)
            {
                if (request.Contains('?'))
                {
                    request += "&idcategoria=" + idcategoria;
                }
                else
                {
                    request += "?idcategoria=" + idcategoria;
                }
            }

            return await this.CallApi<List<Producto>>(request);
        }

        public async Task<Producto> BuscarProductoAsync(int id)
        {
            String request = "/api/Productos/BuscarProducto/" + id;
            return await this.CallApi<Producto>(request);
        }

        public async Task<List<decimal>> GetListaLitrosAsync()
        {
            String request = "/api/Productos/GetListaLitros";
            return await this.CallApi<List<decimal>>(request);
        }

        public async Task<decimal> GetPrecioMaxAsync(int? idcategoria)
        {
            String request = "/api/Productos/GetPrecioMax";
            if(idcategoria != null)
            {
                request += "?id=" + idcategoria;
            }
            return await this.CallApi<decimal>(request);
        }

        public async Task<decimal> GetPrecioMinAsync(int? idcategoria)
        {
            String request = "/api/Productos/GetPrecioMin";
            if (idcategoria != null)
            {
                request += "?id=" + idcategoria;
            }
            return await this.CallApi<decimal>(request);
        }

        public async Task<List<Producto>> GetListaProductosAsync(List<int> productos)
        {
            if(productos.Count == 0)
            {
                return null;
            }
            String request = "/api/Productos/GetListaProductos?";
            String data = "";
            foreach (int id in productos)
            {
                data += "ids=" + id + "&";
            }
            data = data.Trim('&');

            return await this.CallApi<List<Producto>>(request + data);
        }

        public async Task<int> GetStockAsync(int id)
        {
            String request = "/api/Productos/GetStock/" + id;
            return await this.CallApi<int>(request);
        }

        public async Task InsertarProductoAsync(String nombre, decimal precio,
            int stock, String imagen, decimal litros, int idcategoria, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Productos/InsertarProducto";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                Producto prod = new Producto
                {
                    IdProducto = 0,
                    Nombre = nombre,
                    Precio = precio,
                    Stock = stock,
                    Imagen = imagen,
                    Litros = litros,
                    Categoria = idcategoria
                };
                String json = JsonConvert.SerializeObject(prod);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PostAsync(request, content);

            }
        }

        public async Task EditarProductoAsync(int idproducto, String nombre, decimal precio,
            int stock, String imagen, decimal litros, int idcategoria, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Productos/EditarProducto";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                Producto prod = new Producto
                {
                    IdProducto = idproducto,
                    Nombre = nombre,
                    Precio = precio,
                    Stock = stock,
                    Imagen = imagen,
                    Litros = litros,
                    Categoria = idcategoria
                };
                String json = JsonConvert.SerializeObject(prod);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PutAsync(request, content);

            }
        }

        public async Task EliminarProductoAsync(int id, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Productos/EliminarProducto/" + id;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                await client.DeleteAsync(request);

            }
        }

        public async Task RestarStockAsync(int idproducto, int cantidad)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Productos/RestarStock/" + idproducto + "/" + cantidad;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                await client.PutAsync(request, new StringContent(""));

            }
        }

        public async Task SumarStockAsync(int idproducto, int cantidad)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Productos/SumarStock/" + idproducto + "/" + cantidad;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                await client.PutAsync(request, new StringContent(""));

            }
        }

        #endregion

        #region PEDIDOS

        public async Task<Pedido> BuscarPedidoAsync(int id, String token)
        {
            String request = "/api/Pedidos/BuscarPedido/" + id;
            return await this.CallApi<Pedido>(request, token);
        }

        public async Task<List<Pedido>> GetPedidosUsuarioAsync(int id, String token)
        {
            String request = "/api/Pedidos/GetPedidosUsuario/" + id;
            return await this.CallApi<List<Pedido>>(request, token);
        }

        public async Task<Carrito> GetProductosPedidoAsync(int id, String token)
        {
            String request = "/api/Pedidos/GetProductosPedido/" + id;
            return await this.CallApi<Carrito>(request, token);
        }

        public async Task CreatePedidoAsync(int idusuario, decimal subtotal, Carrito carrito, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Pedidos/CreatePedido/" + idusuario + "/" + subtotal;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                String json = JsonConvert.SerializeObject(carrito);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PostAsync(request, content);
            }
        }

        public async Task CancelarPedidoAsync(int id, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Pedidos/CancelarPedido/" + id;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                await client.DeleteAsync(request);

            }
        }

        #endregion

        #region USUARIOS

        public async Task<Usuario> BuscarUsuarioAsync(int id, String token)
        {
            String request = "/api/Usuarios/BuscarUsuario/" + id;
            return await this.CallApi<Usuario>(request, token);
        }

        public async Task<Usuario> GetUsuarioAsync(String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "api/Usuarios/GetUsuario/";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Usuario data = await response.Content.ReadAsAsync<Usuario>();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task InsertarUsuarioAsync(String username, String nombre, String correo, String password, String direccion, String telefono)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/InsertarUsuario?password=" + password;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Usuario user = new Usuario
                {
                    IdUsuario = 0,
                    Nombre = nombre,
                    UserName = username,
                    Correo = correo,
                    Password = null,
                    Direccion = direccion,
                    Telefono = telefono
                };
                String json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PostAsync(request, content);

            }
        }

        public async Task EditarUsuarioAsync(int idusuario, String nombre, String direccion, String telefono, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/EditarUsuario?idusuario=" + idusuario + "&nombre=" + nombre
                    + "&direccion=" + direccion + "&telefono=" + telefono;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                await client.PutAsync(request, new StringContent(""));

            }
        }

        public async Task CambiarContraseñaAsync(int idusuario, String password, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String request = "/api/Usuarios/CambiarContraseña?idusuario=" + idusuario + "&password=" + password;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                await client.PutAsync(request, new StringContent(""));

            }
        }

        public async Task<bool> UserNameExistsAsync(String username)
        {
            String request = "/api/Usuarios/UserNameExists/" + username;
            return await this.CallApi<bool>(request);
        }

        #endregion

    }
}
