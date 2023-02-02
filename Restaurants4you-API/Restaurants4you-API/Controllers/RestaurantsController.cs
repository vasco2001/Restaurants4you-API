using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant4you_API.Data;
using Restaurant4you_API.Models;

namespace Restaurant4you_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase {

        private readonly ApplicationDbContext db;

        public RestaurantsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Authorize(Roles = "User, Restaurant")]
        public async Task<ActionResult<IEnumerable<Restaurants>>> ListRestaurants()
        {
            List<Restaurants> list = await db.Restaurant.Select(x => new Restaurants
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Localization = x.Localization,
                Contact = x.Contact,
                Email = x.Email,
                Time = x.Time,
                Latitude= x.Latitude,
                Longitude= x.Longitude,
                Images = db.Image.Where(y => y.RestaurantFK == x.Id).ToList()

            }).ToListAsync();
            return list;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Restaurant")]
        public async Task<ActionResult<Restaurants>> GetRestaurant(int id)
        {
            var rt = await db.Restaurant
                              .Include(a => a.Images)
                              .OrderByDescending(a => a.Id)
                              .Select(a => new Restaurants
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Description = a.Description,
                                  Latitude = a.Latitude,
                                  Longitude = a.Longitude,
                                  Localization= a.Localization,
                                  Contact = a.Contact,
                                  Email = a.Email,
                                  Time=a.Time,
                                  Images = db.Image.Where(y => y.RestaurantFK == a.Id).ToList()

                              })
                              .Where(a => a.Id == id)
                              .FirstOrDefaultAsync();

            if (rt == null)
            {
                return NotFound();
            }

            return rt;
        }

        [Consumes("multipart/form-data")]
        [HttpPost]
        [Authorize(Roles = "Restaurant")]
        public async Task<ActionResult<Restaurants>> AddRestaurant([FromForm] Restaurants rt)
        {
            var identity = User.Identity.Name;
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == identity);
            rt.UserFK = user.Id;

            db.Add(rt);
            await db.SaveChangesAsync();
            return Ok(rt);
        }

        [Consumes("multipart/form-data")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var identity = User.Identity.Name;
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == identity);

            if (!db.Restaurant.Where(x => x.UserFK == user.Id && x.Id == id).Any())
                return BadRequest();

            if (db.Restaurant.Find(id) != null)
            {
                if(db.Image.Where(x=> x.RestaurantFK == id).Any())
                {
                    List<Images> list = db.Image.Where(x=> x.RestaurantFK == id).ToList();
                    foreach (Images img in list)
                    {
                        System.IO.File.Delete("wwwroot//Fotos//" + Path.Combine(img.Path));
                    }                    
                    db.Image.RemoveRange(db.Image.Where(x => x.RestaurantFK == id));
                }

                db.Restaurant.Remove(db.Restaurant.Find(id));
                await db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        [Consumes("multipart/form-data")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<ActionResult<Restaurants>> EditRestaurant([FromForm] Restaurants rt)
        {
            var identity = User.Identity.Name;
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == identity);

            if (!db.Restaurant.Where(x => x.UserFK == user.Id && x.Id == rt.Id).Any())
                return BadRequest();

            if (db.Restaurant.Where(x => x.Id== rt.Id).Any())
            {
                Restaurants upd = db.Restaurant.Find(rt.Id);
                upd.Description = rt.Description;
                upd.Name = rt.Name;
                upd.Time = rt.Time;
                upd.Contact = rt.Contact;
                upd.Email = rt.Email;
                upd.Longitude = rt.Longitude;
                upd.Latitude = rt.Latitude;
                
                await db.SaveChangesAsync();
                return Ok(upd);
            }

            return NotFound();
        }

        [HttpGet("User")]
        [Authorize(Roles = "Restaurant")]
        public async Task<ActionResult<IEnumerable<Restaurants>>> ListRestaurantsUser()
        {
            var identity = User.Identity.Name;
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == identity);

            if (!db.Restaurant.Where(x => x.UserFK == user.Id).Any()) return Ok(new List<Restaurants>());

            List<Restaurants> list = await db.Restaurant
                              .Include(a => a.Images)
                              .OrderByDescending(a => a.Id)
                              .Select(a => new Restaurants
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Description = a.Description,
                                  Latitude = a.Latitude,
                                  Longitude = a.Longitude,
                                  Localization = a.Localization,
                                  Contact = a.Contact,
                                  Email = a.Email,
                                  Time = a.Time,
                                  UserFK = a.UserFK,
                                  Images = db.Image.Where(y => y.RestaurantFK == a.Id).ToList()

                              })
                              .Where(a => a.UserFK == user.Id)
                              .ToListAsync();

            if (list == null)
            {
                return NotFound();
            }

            return Ok(list);
        }

    }

}
