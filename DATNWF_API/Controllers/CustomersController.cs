using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;
using DATNWF_API.Models.DTO;

namespace DATNWF_API.Controllers
{
    //https://localhost:7088/api/Customers API của khách hàng

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public CustomersController(ThanhnienContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabKhachhang>>> LấyDanhSáchKháchHàng()
        {
            if (_context.TabKhachhangs == null)
            {
                return NotFound("Không tìm thấy bảng Khách Hàng.");
            }
            return await _context.TabKhachhangs.ToListAsync();
        }
    }
}