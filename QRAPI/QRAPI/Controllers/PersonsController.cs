using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRAPI.Data;
using QRAPI.Models.LibraryAPI.Models;

namespace QRAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public PersonsController(ApplicationContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/Persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
          if (_context.Persons == null)
          {
              return NotFound();
          }
            return await _context.Persons.ToListAsync();
        }

        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(string id)
        {
          if (_context.Persons == null)
          {
              return NotFound();
          }
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin,Person")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(string id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/Persons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            if (_context.Persons == null)
            {
                return Problem("Entity set 'ApplicationContext.Persons'  is null.");
            }
            _userManager.CreateAsync(person.ApplicationUser!, person.ApplicationUser!.Password).Wait();
            _userManager.AddToRoleAsync(person.ApplicationUser!, "Person").Wait();



            person.Id = person.ApplicationUser!.Id;
            person.ApplicationUser = null;
            _context.Persons.Add(person);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonExists(person.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        [Authorize(Roles = "Admin,Person")]
        [HttpPost("upload-cover-image/{personId}")]
        public async Task<IActionResult> UploadCoverImage(string personId, IFormFile coverImage)
        {
            if (coverImage == null || coverImage.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Kişiyi bul
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found.");
            }

            // Dosya yolunu belirleyin (örneğin: wwwroot/images/{fileName})
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            // Klasörün var olup olmadığını kontrol edin ve gerekiyorsa oluşturun
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = coverImage.FileName;
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Dosyayı kaydedin
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await coverImage.CopyToAsync(stream);
            }

            // Kişinin CoverImageUrl özelliğini güncelleyin
            person.CoverImageUrl = $"/images/{fileName}";

            // Kişi nesnesini güncelleyin
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Dosyayı okuyup yanıt olarak döndürün
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "image/jpeg");
        }

        //[Authorize(Roles = "Admin,Employee")]
        [Authorize(Roles = "Admin,Person")]
        [HttpDelete("remove-cover-image/{personId}")]
        public async Task<IActionResult> RemoveCoverImage(string personId)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found.");
            }

            // Eski kapak resminin dosya yolunu belirleyin
            var oldFileName = Path.GetFileName(person.CoverImageUrl);
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", oldFileName);

            // Dosya varsa, dosyayı kaldırın
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            // Kapak resmini kaldırın
            person.CoverImageUrl = null;

            // Üye nesnesini güncelleyin
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        [HttpPut("Deactivate/{personId}")]
        public async Task<IActionResult> DeactivateMember(string personId)
        {
            var person = await _context.Persons
                .Include(p => p.ApplicationUser) // ApplicationUser ile ilişkiyi dahil ediyoruz
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
            {
                return NotFound();
            }

            // ApplicationUser nesnesinin IsActive özelliğini güncelleyin
            if (person.ApplicationUser != null)
            {
                person.ApplicationUser.IsActive = false;
                _context.Entry(person.ApplicationUser).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("Activate/{personId}")]
        public async Task<IActionResult> ActivateMember(string personId)
        {
            var person = await _context.Persons
                .Include(p => p.ApplicationUser) // ApplicationUser ile ilişkiyi dahil ediyoruz
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
            {
                return NotFound();
            }

            // ApplicationUser nesnesinin IsActive özelliğini güncelleyin
            if (person.ApplicationUser != null)
            {
                person.ApplicationUser.IsActive = true;
                _context.Entry(person.ApplicationUser).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(string id)
        {
            if (_context.Persons == null)
            {
                return NotFound();
            }
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */


        private bool PersonExists(string id)
        {
            return (_context.Persons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
