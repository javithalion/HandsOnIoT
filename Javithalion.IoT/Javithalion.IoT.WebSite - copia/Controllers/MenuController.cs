using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Javithalion.IoT.WebSite.Models.Dto;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Javithalion.IoT.WebSite.Controllers
{   
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private static IEnumerable<MenuGroupDto> MenuGroups = new List<MenuGroupDto>()
        {
            new MenuGroupDto()
            {
                Id = Guid.NewGuid(),
                Name = "Devices",
                Icon = "important_devices",
                MenuItems = new List<MenuItemDto>()
                {
                    new MenuItemDto() {Id = Guid.NewGuid(), Name = "Manage devices", Icon = "important_devices", Link = "/Devices/Index"  },
                }
            },
            new MenuGroupDto()
            {
                Id = Guid.NewGuid(),
                Name = "Profile",
                Icon = "user",
                MenuItems = new List<MenuItemDto>()
                {
                    new MenuItemDto() {Id = Guid.NewGuid(), Name = "Edit profile", Icon = "user", Link = "/Account/LogOn"  },
                    new MenuItemDto() {Id = Guid.NewGuid(), Name = "Log off", Icon = "user", Link = "/Account/LogOff"  }
                }
            }
        };

        // GET: api/values
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get()
        {
            return Ok(MenuGroups);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
