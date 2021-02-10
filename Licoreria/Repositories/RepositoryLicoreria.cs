using Licoreria.Data;
using Licoreria.Helpers;
using Licoreria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Repositories
{
    public class RepositoryLicoreria: IRepositoryLicoreria
    {
        LicoreriaContext context;

        public RepositoryLicoreria(LicoreriaContext context)
        {
            this.context = context;
        }

        #region CATEGORIAS

        public List<Categoria> GetCategorias()
        {
            return this.context.Categorias.ToList();
        }

        public String GetNombreCategoria(int idcategoria)
        {
            Categoria cat = this.context.Categorias.Where(z => z.IdCategoria == idcategoria).FirstOrDefault();
            return cat.Nombre;
        }

        #endregion

        #region PRODUCTOS

        public List<Producto> GetProductos(int idcategoria)
        {
            return this.context.Productos.Where(z => z.Categoria == idcategoria).ToList();
        }

        public List<Producto> GetProductosMini()
        {
            return this.context.Productos.Where(z => z.Litros <= 0.02M).ToList();
        }

        public List<Producto> GetProductosMaxi()
        {
            return this.context.Productos.Where(z => z.Litros >= 1.50M).ToList();
        }

        public List<Producto> BuscarProductos(String nombre)
        {
            List<Producto> productos = this.context.Productos.Where(z => z.Nombre.ToUpper().Contains(nombre.ToUpper())).ToList();
            if(productos.Count == 0)
            {
                return null;
            } else
            {
                return productos;
            }
        }

        public List<Producto> GetListaProductos(List<int> idproductos)
        {
            List<Producto> productos = new List<Producto>();
            foreach(int id in idproductos)
            {
                Producto prod = this.context.Productos.Where(z => z.IdProducto == id).FirstOrDefault();
                productos.Add(prod);
            }
            return productos;
        }

        #endregion

        #region USUARIOS

        public Usuario BuscarUsuario(int idusuario)
        {
            return this.context.Usuarios.Where(z => z.IdUsuario == idusuario).FirstOrDefault();
        }

        public void InsertarUsuario(String username, String nombre, String correo, String password, String telefono)
        {
            if (!this.UserNameExists(username))
            {
                Usuario user = new Usuario();
                user.IdUsuario = this.GetMaxId(Tablas.Usuarios);
                user.UserName = username;
                user.Nombre = nombre;
                user.Correo = correo;

                String salt = CypherService.GetSalt();
                user.Salt = salt;
                user.Password = CypherService.CifrarContenido(password, salt);

                user.Telefono = telefono;
                user.Validado = false;
                user.Rol = 1;

                this.context.Usuarios.Add(user);
                this.context.SaveChanges();
            }
        }

        public Usuario LoginUsuario(String username, String password)
        {
            if (this.UserNameExists(username))
            {
                Usuario user = this.context.Usuarios.Where(z => z.UserName.ToUpper() == username.ToUpper()).FirstOrDefault();
                if (user == null)
                {
                    return null;
                } else if (!user.Validado)
                {
                    return null;
                }
                else
                {
                    String salt = user.Salt;
                    byte[] passbbdd = user.Password;
                    byte[] passtemporal = CypherService.CifrarContenido(password, salt);

                    bool respuesta = ToolkitService.CompararArrayBytes(passbbdd, passtemporal);
                    if (respuesta)
                    {
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            } else
            {
                return null;
            }
            
        }

        public bool UserNameExists(String username)
        {
            bool res = this.context.Usuarios.Where(z => z.UserName == username).Any();
            if (res)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region PEDIDOS

        public void CreatePedido(int idusuario, decimal subtotal, Carrito carrito) {
            Pedido pedido = new Pedido();
            pedido.IdPedido = this.GetMaxId(Tablas.Pedidos);
            pedido.Usuario = idusuario;
            pedido.Fecha = DateTime.Now;
            pedido.Coste = subtotal;
            this.context.Pedidos.Add(pedido);
            this.context.SaveChanges();

            List<Producto> productos = this.GetListaProductos(carrito.Productos);

            int contador = 0;
            foreach(Producto prod in productos)
            {
                ProductosPedido pp = new ProductosPedido();
                pp.IdProductosPedido = this.GetMaxId(Tablas.ProductosPedido);
                pp.Pedido = pedido.IdPedido;
                pp.Producto = prod.IdProducto;
                pp.Cantidad = carrito.Cantidades[contador];

                this.context.ProductosPedidos.Add(pp);
                this.context.SaveChanges();
                contador++;
            }
        }

        public List<Pedido> GetPedidosUsuario(int idusuario)
        {
            return this.context.Pedidos.Where(z => z.Usuario == idusuario).ToList();
        }

        public List<ProductosPedido> GetProductosPedido(int idpedido)
        {
            return this.context.ProductosPedidos.Where(z => z.Pedido == idpedido).ToList();
        }

        #endregion

        #region MISC

        public int GetMaxId(Tablas tabla)
        {
            if(tabla == Tablas.Usuarios)
            {
                return this.context.Usuarios.Max(z => z.IdUsuario) + 1;
            } else if(tabla == Tablas.Productos)
            {
                return this.context.Productos.Max(z => z.IdProducto) + 1;
            } else if(tabla == Tablas.Pedidos)
            {
                return this.context.Pedidos.Max(z => z.IdPedido) + 1;
            } else if(tabla == Tablas.Categorias)
            {
                return this.context.Categorias.Max(z => z.IdCategoria) + 1;
            } else if(tabla == Tablas.ProductosPedido)
            {
                return this.context.ProductosPedidos.Max(z => z.IdProductosPedido) + 1;
            } else
            {
                return -1;
            }
        }

        #endregion

    }
}
