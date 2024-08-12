using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRAPI.Data;
using QRAPI.Models;
using QRCoder;
using SkiaSharp;

namespace QRAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public QRsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/QRs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QR>>> GetQRCode()
        {
          if (_context.QRs == null)
          {
              return NotFound();
          }
            return await _context.QRs.ToListAsync();
        }

        // GET: api/QRs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QR>> GetQR(int id)
        {
          if (_context.QRs == null)
          {
              return NotFound();
          }
            var qR = await _context.QRs.FindAsync(id);

            if (qR == null)
            {
                return NotFound();
            }

            return qR;
        }

        // PUT: api/QRs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQR(int id, QR qR)
        {
            if (id != qR.Id)
            {
                return BadRequest();
            }

            _context.Entry(qR).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QRExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/QRs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<QR>> PostQR(QR qR)
        {
          if (_context.QRs == null)
          {
              return Problem("Entity set 'ApplicationContext.QRCode'  is null.");
          }
            _context.QRs.Add(qR);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQR", new { id = qR.Id }, qR);
        }

        // DELETE: api/QRs/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQR(int id)
        {
            if (_context.QRs == null)
            {
                return NotFound();
            }
            var qR = await _context.QRs.FindAsync(id);
            if (qR == null)
            {
                return NotFound();
            }

            _context.QRs.Remove(qR);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("QRCodeGenerator/{productType}/{categoryId}")]
        public async Task<IActionResult> Generate(string productType, short categoryId)
        {
            // QR kodu içeriğini saklamak için bir değişken
            string qrCodeContent = string.Empty;

            switch (productType.ToLower())
            {
                case "food":
                    var foods = await _context.Foods
                        .Where(f => f.CategoryID == categoryId) // Kategori ID'ye göre tüm yemekleri alın
                        .Include(f => f.Category) // İlgili kategori bilgilerini de dahil edin
                        .ToListAsync();

                    if (!foods.Any())
                    {
                        return NotFound("No foods found for this category.");
                    }

                    // QR kodu içeriğini oluşturun
                    qrCodeContent = $"Category ID: {categoryId}\n" +
                                    $"Foods:\n" +
                                    $"{string.Join("\n", foods.Select(f => $"-Name: {f.Name},Description: {f.Description}, Price: ${f.Price}"))}";
                    break;

                case "car":
                    var cars = await _context.Cars
                        .Where(c => c.CategoryID == categoryId) // Kategori ID'ye göre tüm arabaları alın
                        .Include(c => c.Category) // İlgili kategori bilgilerini de dahil edin
                        .ToListAsync();

                    if (!cars.Any())
                    {
                        return NotFound("No cars found for this category.");
                    }

                    // QR kodu içeriğini oluşturun
                    qrCodeContent = $"Category ID: {categoryId}\n" +
                                    $"Cars:\n" +
                                    $"{string.Join("\n", cars.Select(c => $"- Model: {c.Model}, Brand: {c.Brand}, Price: ${c.Price}"))}";
                    break;

                case "ticket":
                    var tickets = await _context.Tickets
                        .Where(c => c.CategoryID == categoryId) // Kategori ID'ye göre tüm arabaları alın
                        .Include(c => c.Category) // İlgili kategori bilgilerini de dahil edin
                        .ToListAsync();

                    if (!tickets.Any())
                    {
                        return NotFound("No tickets found for this category.");
                    }

                    // QR kodu içeriğini oluşturun
                    qrCodeContent = $"Category ID: {categoryId}\n" +
                                    $"Tickets:\n" +
                                    $"{string.Join("\n", tickets.Select(c => $"- Title: {c.Title}, Description: {c.Description}, Price: ${c.Price}, LocationPlace: {c.LocationPlace}, Block: {c.Block},RowNumber: {c.RowNumber}"))}";
                    break;

                default:
                    return BadRequest("Invalid product type.");
            }

            byte[] qrCodeBytes;

            // QR kodunu oluştur
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrCodeContent, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new BitmapByteQRCode(qrCodeData);

                // QR kodunu byte[] olarak al
                var qrCodeImage = qrCode.GetGraphic(5);

                // byte[]'i SKBitmap'e dönüştür
                using (var skBitmap = SKBitmap.Decode(qrCodeImage))
                {
                    using (var stream = new MemoryStream())
                    {
                        skBitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
                        qrCodeBytes = stream.ToArray();
                    }
                }
            }

            // QR kodunu veritabanına kaydet
            var qr = new QR
            {
                Content = qrCodeContent,
                QRCodeData = Convert.ToBase64String(qrCodeBytes),  //Base64 kodlu veri
                ProductType = productType,
                ProductId = categoryId, // Veya ihtiyaca göre uygun bir ID
                
            };

            _context.QRs.Add(qr);
            await _context.SaveChangesAsync();


            return File(qrCodeBytes, "image/png");
        }




        private bool QRExists(int id)
        {
            return (_context.QRs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
