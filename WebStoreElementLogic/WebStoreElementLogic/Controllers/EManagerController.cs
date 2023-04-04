using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.AspNetCore.SignalR;
using WebStoreElementLogic.Hubs;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;

namespace WebStoreElementLogic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EManagerController : ControllerBase
    {
        private readonly IHubContext<EManagerHub> _hubContext;

        public EManagerController(IHubContext<EManagerHub> hubContext)
        {
            _hubContext = hubContext;
        }


        [HttpPost("PlacedGoods")]
        public async Task<IActionResult> PlacedGoods([FromBody]string xml)
        {
            PGBody[] receipts = PGBody.FromXml(xml);

            // Update database with PG info
            foreach (var receipt in receipts)
            {
                Console.WriteLine($"Got PG from EManager: {receipt.ExtProductId}");
            }

            // Allert connected clients TODO: replace 10 with actual data
            await _hubContext.Clients.All.SendAsync("Something", 10);

            return Ok();
        }

        // Consider as data class?
        public class PGBody
        {
            public int TransactionId { get; set; }
            public string PurchaseOrderId { get; set; }
            public int PurchaseOrderLineId { get; set; }
            public int ExtProductId { get; set; }
            public decimal Quantity { get; set; }

            public static PGBody[] FromXml(string xml)
            {
                XDocument doc = XDocument.Parse(xml);
                var goodsReceivalLines = doc.Root.Element("Lines").Elements("GoodsReceivalLine");
                List<PGBody> pgBodies = new List<PGBody>();

                foreach (var goodsReceivalLine in goodsReceivalLines)
                {
                    pgBodies.Add(new PGBody
                    {
                        TransactionId = int.Parse(goodsReceivalLine.Element("TransactionId").Value),
                        PurchaseOrderId = goodsReceivalLine.Element("PurchaseOrderId").Value,
                        PurchaseOrderLineId = int.Parse(goodsReceivalLine.Element("PurchaseOrderLineId").Value),
                        ExtProductId = int.Parse(goodsReceivalLine.Element("ExtProductId").Value),
                        Quantity = decimal.Parse(
                            goodsReceivalLine.Element("Quantity").Value, 
                            System.Globalization.CultureInfo.InvariantCulture
                        )
                    });
                }

                return pgBodies.ToArray();
            }

        }
    }
}
