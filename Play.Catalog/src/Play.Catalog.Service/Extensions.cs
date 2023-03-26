using Play.Catalog.Service.Models;
using static Play.Catalog.Service.DTOs;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDTO AsDDTO(this Item item)
        {
            return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}