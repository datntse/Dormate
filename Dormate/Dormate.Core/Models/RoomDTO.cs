using Dormate.Core.Constant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Core.Models
{
    public class RoomDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public decimal Area { get; set; }
        public decimal Price { get; set; }
        public decimal MaximunSlot { get; set; }
        public decimal CurrentSlot { get; set; } = 0;
        public string MainPicture { get; set; }
        public int Status { get; set; }
        public bool? RoomType { get; set; }
        public bool? IsHide { get; set; } = false;
        public string? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
    }
    public class RoomCM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public decimal Area { get; set; }
        public decimal Price { get; set; }
        public decimal MaximunSlot { get; set; }
        public decimal? CurrentSlot { get; set; } = 0;
        public IFormFile MainPicture { get; set; }
        public IFormFile[] SubPicture { get; set; }
   
        public int Status { get; set; }
        public bool? RoomType { get; set; }
        public bool? IsHide { get; set; } = false;

    }
    public class RoomVM 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public decimal Area { get; set; }
        public decimal Price { get; set; }
        public decimal MaximunSlot { get; set; }
        public decimal CurrentSlot { get; set; } = 0;
        public string MainPictureUrl { get; set; }
        public List<SubImage> SubPictureUrl { get; set; }
        public int Status { get; set; }
        public bool? RoomType { get; set; }
        public bool? IsHide { get; set; } = false;
        public string? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public string Owner { get; set; }
    }

    public class SubImage
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
    }

    public class SubImageUM
    {
        public string Id { get; set; }
        public IFormFile SubPicture { get; set; }
    }


    public class RoomUM 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public decimal Area { get; set; }
        public decimal Price { get; set; }
        public decimal MaximunSlot { get; set; }
        public decimal CurrentSlot { get; set; } = 0;
        public IFormFile? MainPicture { get; set; }
        public List<SubImageUM>? SubImage { get; set; }
        public bool? RoomType { get; set; }
    }
    public class RoomFilterModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public decimal? FromPrice { get; set; } = 0;
        public decimal? ToPrice { get; set; }
        public int? Status { get; set; }
        public bool? RoomType { get; set; }
        public DefaultSearch defaultSearch { get; set; }
        public RoomFilterModel()
        {
            defaultSearch = new DefaultSearch();
        }

    }

}
