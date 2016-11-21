using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.WebSite.Back.Models
{
    public class MenuGroupDto
    {
        public MenuGroupDto()
        {
            MenuItems = new List<MenuItemDto>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public IEnumerable<MenuItemDto> MenuItems { get; set; }
    }
}
