using System;

namespace Javithalion.IoT.WebSite.Back.Models
{
    public class MenuItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public string Icon { get; set; }
    }
}