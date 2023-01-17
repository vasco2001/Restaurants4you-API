using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant4you_API.Data;
using Restaurant4you_API.Models;

namespace Restaurant4you_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImagesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;

            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Images
        [HttpGet]
        [Authorize(Roles = "User, Restaurant")]
        public async Task<ActionResult<IEnumerable<Images>>> GetImage()
        {
            return await _context.Image
                              .Include(a => a.Restaurant)
                              .OrderByDescending(a => a.Id)
                              .Select(a => new Images
                              {
                                  Id = a.Id,
                                  Path= a.Path,
                                  RestaurantFK = a.RestaurantFK,
                                  Restaurant = a.Restaurant,
                              })
                              .ToListAsync();

        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Restaurant")]
        public async Task<ActionResult<Images>> GetImages(int id)
        {
            var images = await _context.Image
                              .Include(a => a.Restaurant)
                              .OrderByDescending(a => a.Id)
                              .Select(a => new Images
                              {
                                  Id = a.Id,
                                  Path = a.Path,
                                  RestaurantFK = a.RestaurantFK,
                                  Restaurant = a.Restaurant,
                              })
                              .Where(a => a.Id == id)
                              .FirstOrDefaultAsync();

            if (images == null)
            {
                return NotFound();
            }

            return images;
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Consumes("multipart/form-data")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> PutImages(int id, [FromForm] Images images)
        {
            if (id != images.Id)
            {
                return BadRequest();
            }

            _context.Entry(images).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagesExists(id))
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

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Consumes("multipart/form-data")]
        [HttpPost]
        [Authorize(Roles = "Restaurant")]
        public async Task<ActionResult<Images>> PostImages([FromForm] Images images, IFormFile imagem)
        {
           
            if (!(imagem.ContentType == "image/jpeg" || imagem.ContentType == "image/png" || imagem.ContentType == "image/jpg"))
                {
                    // menssagem de erro
                    ModelState.AddModelError("", "Por favor, se pretende enviar um ficheiro, escolha uma imagem suportada.");
                }
                else
                {
                    // definir o nome da imagem
                    Guid g;
                    g = Guid.NewGuid();
                    string imageName = images.Id + "_" + g.ToString();
                    string extensionOfImage = Path.GetExtension(imagem.FileName).ToLower();
                    imageName += extensionOfImage;
                    // adicionar o nome da imagem aos filmes
                    images.Path = imageName;
                }

                // guardar a imagem no disco
                if (imagem != null)
                {
                    // pergunta ao servidor que endereço quer usar
                    string addressToStoreFile = _webHostEnvironment.WebRootPath;
                    string newImageLocalization = Path.Combine(addressToStoreFile, "Fotos//");
                    // ver se a diretoria existe se não cria
                    if (!Directory.Exists(newImageLocalization))
                    {
                        Directory.CreateDirectory(newImageLocalization);
                    }
                    //guarda a imagem no disco
                    newImageLocalization = Path.Combine(newImageLocalization, images.Path);
                    using var stream = new FileStream(newImageLocalization, FileMode.Create);
                    await imagem.CopyToAsync(stream);
                }
            

            _context.Image.Add(images);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImages", new { id = images.Id }, images);
        }

        // DELETE: api/Images/5
        [Consumes("multipart/form-data")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> DeleteImages(int id)
        {
            var images = await _context.Image.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }

            System.IO.File.Delete("wwwroot//Fotos//" + Path.Combine(images.Path));

            _context.Image.Remove(images);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagesExists(int id)
        {
            return _context.Image.Any(e => e.Id == id);
        }
    }
}
