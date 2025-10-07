using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laboratorio08.Data;
using Laboratorio08.Models;

namespace Laboratorio08.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LINQController : ControllerBase
    {
        private readonly DbContexto _context;

        public LINQController(DbContexto context)
        {
            _context = context;
        }

        // Ejercicio 1: Clientes con nombre específico
        [HttpGet("clientes/nombre")]
        public IActionResult GetClientesPorNombre([FromQuery] string nombre)
        {
            var clientes = _context.Clients
                .Where(c => EF.Functions.ILike(c.Name, $"{nombre}%"))
                .ToList();

            return Ok(clientes);
        }

        // Ejercicio 2: Productos con precio mayor
        [HttpGet("productos/precio-mayor")]
        public IActionResult GetProductosPorPrecio([FromQuery] decimal precio)
        {
            var productos = _context.Products
                .Where(p => p.Price > precio)
                .ToList();

            return Ok(productos);
        }

        // Ejercicio 3: Detalle de productos en una orden
        [HttpGet("ordenes/{orderId}/detalles")]
        public IActionResult GetProductosPorOrden(int orderId)
        {
            var detalles = _context.Orderdetails
                .Include(d => d.Product)
                .Where(d => d.Orderid == orderId)
                .Select(d => new
                {
                    Producto = d.Product.Name,
                    Cantidad = d.Quantity
                })
                .ToList();

            return Ok(detalles);
        }

        // Ejercicio 4: Cantidad total de productos por orden
        [HttpGet("ordenes/{orderId}/cantidad-total")]
        public IActionResult GetCantidadTotal(int orderId)
        {
            var total = _context.Orderdetails
                .Where(d => d.Orderid == orderId)
                .Sum(d => d.Quantity);

            return Ok(new { OrderId = orderId, TotalProductos = total });
        }

        // Ejercicio 5: Producto más caro
        [HttpGet("productos/mas-caro")]
        public IActionResult GetProductoMasCaro()
        {
            var producto = _context.Products
                .OrderByDescending(p => p.Price)
                .FirstOrDefault();

            return Ok(producto);
        }

        // Ejercicio 6: Pedidos después de una fecha
        [HttpGet("ordenes/fecha")]
        public IActionResult GetOrdenesDespuesDeFecha([FromQuery] DateTime fecha)
        {
            var ordenes = _context.Orders
                .Where(o => o.Orderdate > fecha)
                .ToList();

            return Ok(ordenes);
        }

        // Ejercicio 7: Promedio de precio de productos
        [HttpGet("productos/promedio-precio")]
        public IActionResult GetPromedioPrecio()
        {
            var promedio = _context.Products
                .Average(p => p.Price);

            return Ok(new { Promedio = promedio });
        }

        // Ejercicio 8: Productos sin descripción
        [HttpGet("productos/sin-descripcion")]
        public IActionResult GetProductosSinDescripcion()
        {
            var productos = _context.Products
                .Where(p => string.IsNullOrEmpty(p.Description))
                .ToList();

            return Ok(productos);
        }

        // Ejercicio 9: Cliente con más pedidos
        [HttpGet("clientes/top-pedidos")]
        public IActionResult GetClienteConMasPedidos()
        {
            var cliente = _context.Orders
                .GroupBy(o => o.Clientid)
                .Select(g => new
                {
                    ClientId = g.Key,
                    TotalPedidos = g.Count()
                })
                .OrderByDescending(x => x.TotalPedidos)
                .FirstOrDefault();

            return Ok(cliente);
        }

        // Ejercicio 10: Pedidos con detalles
        [HttpGet("ordenes/detalles-completos")]
        public IActionResult GetPedidosConDetalles()
        {
            var ordenes = _context.Orderdetails
                .Include(d => d.Product)
                .Select(d => new
                {
                    d.Orderid,
                    Producto = d.Product.Name,
                    d.Quantity
                })
                .ToList();

            return Ok(ordenes);
        }

        // Ejercicio 11: Productos vendidos a un cliente específico
        [HttpGet("clientes/{clienteId}/productos")]
        public IActionResult GetProductosPorCliente(int clienteId)
        {
            var productos = _context.Orders
                .Include(o => o.Orderdetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.Clientid == clienteId)
                .SelectMany(o => o.Orderdetails)
                .Select(d => d.Product.Name)
                .Distinct()
                .ToList();

            return Ok(productos);
        }

        // Ejercicio 12: Clientes que compraron un producto específico
        [HttpGet("productos/{productoId}/clientes")]
        public IActionResult GetClientesPorProducto(int productoId)
        {
            var clientes = _context.Orderdetails
                .Include(d => d.Order)
                .ThenInclude(o => o.Client)
                .Where(d => d.Productid == productoId)
                .Select(d => d.Order.Client.Name)
                .Distinct()
                .ToList();

            return Ok(clientes);
        }
    }
}
