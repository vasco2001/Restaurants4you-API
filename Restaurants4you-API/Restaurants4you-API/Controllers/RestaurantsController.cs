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
        public List<Restaurants> ListRestaurants()
        {
            List<Restaurants> list = db.Restaurant.Select(x => new Restaurants
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
                Plates = db.Plates.Where(y => y.RestaurantFK == x.Id).ToList(),
                Images = db.Image.Where(y => y.RestaurantFK == x.Id).ToList()

            }).ToList();
            return list;
        }

        [HttpPost]
        [Authorize(Roles = "Restaurant")]
        public void AddRestaurant([FromForm] Restaurants rt)
        {
            db.Add(rt);
            db.SaveChanges();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Restaurant")]
        public void DeleteRestaurant(int id)
        {
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

                if (db.Plates.Where(x => x.RestaurantFK == id).Any())
                {
                    db.Plates.RemoveRange(db.Plates.Where(x => x.RestaurantFK == id));
                }

                db.Restaurant.Remove(db.Restaurant.Find(id));
                db.SaveChanges();
            }
        }

        [HttpPut]
        [Authorize(Roles = "Restaurant")]
        public void EditRestaurant([FromForm] Restaurants rt)
        {
            if(db.Restaurant.Where(x => x.Id== rt.Id).Any())
            {
                Restaurants upd = db.Restaurant.Find(rt.Id);
                upd.Description = rt.Description;
                upd.Name = rt.Name;
                upd.Time = rt.Time;
                upd.Contact = rt.Contact;
                upd.Email = rt.Email;
                upd.Longitude = rt.Longitude;
                upd.Latitude = rt.Latitude;
                
                db.SaveChanges();
            }
        }

    }

}
