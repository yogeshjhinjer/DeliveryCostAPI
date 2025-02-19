using DeliveryCostAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryCostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        // Warehouse product stock mapping
        private static readonly Dictionary<string, string[]> WarehouseStock = new()
        {
            { "C1", new[] { "A", "B", "C" } },
            { "C2", new[] {  "D","E", "F" } },
            { "C3", new[] { "G", "H", "I" } }
        };

        // Distance from each warehouse to L1
        private static readonly Dictionary<string, double> DistanceToL1 = new()
        {
            { "C1", 3 },
            { "C2", 2.5 },
            { "C3", 2 }
        };

        // Cost per km
        private const int CostPerKm = 2;

        [HttpGet]
        public IActionResult GetDelivery()
        {
            // Your code here
            return Ok(new { });
        }

        [HttpPost]
        public IActionResult CalculateDeliveryCost([FromBody] Dictionary<string, double> order)
        {
            if (order == null || order.Count == 0)
            {
                return BadRequest("Invalid order request.");
            }

            // Find which warehouses contain the requested items
            var requiredWarehouses = WarehouseStock
                .Where(w => order.Keys.Any(p => w.Value.Contains(p)))
                .Select(w => w.Key)
                .ToList();

            // Find the cheapest route using one vehicle
            double minCost = CalculateMinCost(requiredWarehouses);

            return Ok(new { cost = minCost });
        }

        private double CalculateMinCost(List<string> warehouses)
        {
            if (warehouses.Count == 1)
            {
                return DistanceToL1[warehouses[0]] * CostPerKm * 2;
            }
            else
            {
                double start = warehouses.Min(w => DistanceToL1[w]);
                return start * CostPerKm * 2 + warehouses.Sum(w => DistanceToL1[w] * CostPerKm);
            }
        }
    }
}
