using Licoreria.Helpers;
using Licoreria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Repositories
{
    public interface IRepositoryLicoreria
    {
        List<Categoria> GetCategorias();

        String GetNombreCategoria(int idcategoria);

        List<Producto> GetProductos();

        List<Producto> GetProductos(int idcategoria);

        List<Producto> GetProductosMini();

        List<Producto> GetProductosMaxi();

        List<Producto> BuscarProductosNombre(String nombre);

        Producto BuscarProducto(int idproducto);

        int GetStock(int idproducto);

        Usuario BuscarUsuario(int idusuario);

        void InsertarUsuario(String username, String nombre, String correo, String password, String telefono);

        Usuario LoginUsuario(String username, String password);

        List<Producto> GetListaProductos(List<int> idproductos);

        void CreatePedido(int idusuario, decimal subtotal, Carrito carrito);

        List<Pedido> GetPedidosUsuario(int idusuario);

        List<ProductosPedido> GetProductosPedido(int idpedido);

        int GetMaxId(Tablas tabla);

        bool UserNameExists(String username);
    }
}
