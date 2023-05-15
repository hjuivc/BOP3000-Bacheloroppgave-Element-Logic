using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.AspNetCore.SignalR;
using WebStoreElementLogic.Hubs;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;
using WebStoreElementLogic.Interfaces;
using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EManagerController : ControllerBase
    {
        private readonly IHubContext<EManagerHub> _hubContext;
        private readonly IInboundService _inboundService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public EManagerController(
            IHubContext<EManagerHub> hubContext, 
            IInboundService inboundService, 
            IOrderService orderService,
            IProductService productService
        )
        {
            _hubContext = hubContext;
            _inboundService = inboundService;
            _orderService = orderService;
            _productService = productService;
        }


        [HttpPost("PlacedGoods")]
        public async Task<IActionResult> PlacedGoods()
        {
            Console.WriteLine("Message recieved form EManager:\n");

            using var reader = new StreamReader(Request.Body);
            string xml = await reader.ReadToEndAsync();

            Console.WriteLine(xml);


            PlacedGoodsBody[] receipts = PlacedGoodsBody.FromXml(xml);

            // TODO: change to no loop
            // Update database with PG info
            foreach (var receipt in receipts)
            {
                await _inboundService.Update(receipt.PurchaseOrderId);
                await _productService.UpdateQuantity(receipt.ExtProductId, receipt.Quantity);
                Console.WriteLine($"Got PG from EManager: {receipt.ExtProductId}");
            }

            // Update database

            await _hubContext.Clients.All.SendAsync("PlacedGoods", receipts);
            

            return Ok();
        }

        [HttpPost("ConfirmedPick")]
        public async Task<IActionResult> ConfirmedPick()
        {
            Console.WriteLine("Message recieved form EManager:\n");

            using var reader = new StreamReader(Request.Body);
            string xml = await reader.ReadToEndAsync();

            Console.WriteLine(xml);

            return Ok();
        }

        [HttpGet("abc")]
        public async Task<IActionResult> Test()
        {
            Console.WriteLine("Something happened!");

            return Ok();
        }

        public class CPBody
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

            // TODO: hvis transaction id ikke er int, så funker ikke parsen. Bruk try/catch
            public static CPBody[] FromXml(string xml)
            {
                XDocument doc = XDocument.Parse(xml);
                var transactionId = int.Parse(doc.Root.Element("TRANSACTIONID").Value);
                var putaways = doc.Root.Elements("PUTAWAY");
                List<CPBody> cpBodies = new List<CPBody>();

                foreach (var putaway in putaways)
                {
                    cpBodies.Add(new CPBody
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

                return cpBodies.ToArray();
            }
        }
    }
}
