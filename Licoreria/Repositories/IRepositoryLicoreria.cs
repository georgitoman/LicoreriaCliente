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

        List<Producto> GetProductos(int posicion, ref int salida,
            String nombre, decimal? preciomax,
            decimal? litros, bool? stock, int? idcategoria);

        void InsertarProducto(String nombre, decimal precio, int stock, String imagen, decimal litros, int idcategoria);

        void EditarProducto(int idproducto, String nombre, decimal precio, int stock, String imagen, decimal litros, int idcategoria);

        Producto BuscarProducto(int idproducto);

        int GetStock(int idproducto);

        decimal GetPrecioMax(int? idcategoria);

        decimal GetPrecioMin(int? idcategoria);

        List<decimal> GetListaLitros();

        void RestarStock(int idproducto, int cantidad);

        void SumarStock(int idproducto, int cantidad);

        Usuario BuscarUsuario(int idusuario);

        void InsertarUsuario(String username, String nombre, String correo, String password, String telefono);

        Usuario LoginUsuario(String username, String password);

        List<Producto> GetListaProductos(List<int> idproductos);

        void CreatePedido(int idusuario, decimal subtotal, Carrito carrito);

        Pedido BuscarPedido(int idpedido);

        void CancelarPedido(int idpedido);

        List<Pedido> GetPedidosUsuario(int idusuario);

        List<Producto> GetProductosPedido(int idpedido, ref List<int> cantidades);

        int GetMaxId(Tablas tabla);

        bool UserNameExists(String username);
    }
}
