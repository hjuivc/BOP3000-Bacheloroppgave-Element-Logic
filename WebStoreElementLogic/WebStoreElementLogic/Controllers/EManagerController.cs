using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.AspNetCore.SignalR;
using WebStoreElementLogic.Hubs;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;
using WebStoreElementLogic.Interfaces;

namespace WebStoreElementLogic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EManagerController : ControllerBase
    {
        private readonly IHubContext<EManagerHub> _hubContext;
        private readonly IInboundService _inboundService;

        public EManagerController(IHubContext<EManagerHub> hubContext, IInboundService inboundService)
        {
            _hubContext = hubContext;
            _inboundService = inboundService;
        }


        [HttpPost("PlacedGoods")]
        public async Task<IActionResult> PlacedGoods()
        {
            Console.WriteLine("Message recieved form EManager:\n");

            using var reader = new StreamReader(Request.Body);
            string xml = await reader.ReadToEndAsync();

            Console.WriteLine(xml);

            
            PGBody[] receipts = PGBody.FromXml(xml);

            // Update database with PG info
            foreach (var receipt in receipts)
            {
                await _inboundService.Update(receipt.PurchaseOrderId);
                Console.WriteLine($"Got PG from EManager: {receipt.ExtProductId}");
            }

            // Alert connected clients TODO: replace 10 with actual data
            await _hubContext.Clients.All.SendAsync("PlacedGoods", receipts);
            

            return Ok();
        }

        [HttpGet("abc")]
        public async Task<IActionResult> Test()
        {
            Console.WriteLine("Something happened!");

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

            public override string ToString()
            {
                return $"PrdouctId: {ExtProductId}\nQuantity: {Quantity}";
            }

            public static PGBody[] FromXml(string xml)
            {
                XDocument doc = XDocument.Parse(xml);
                var transactionId = int.Parse(doc.Root.Element("TRANSACTIONID").Value);
                var putaways = doc.Root.Elements("PUTAWAY");
                List<PGBody> pgBodies = new List<PGBody>();

                foreach (var putaway in putaways)
                {
                    pgBodies.Add(new PGBody
                    {
                        TransactionId = transactionId,
                        PurchaseOrderId = putaway.Element("PURCHASEORDERID").Value,
                        PurchaseOrderLineId = int.Parse(putaway.Element("PURCHASEORDERLINEID").Value),
                        ExtProductId = int.Parse(putaway.Element("EXTPRODUCTID").Value),
                        Quantity = decimal.Parse(
                            putaway.Element("ACTQUANTITY").Value,
                            System.Globalization.CultureInfo.InvariantCulture
                        )
                    });
                }

                return pgBodies.ToArray();
            }
        }
    }
}
