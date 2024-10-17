using EPAM.Persistence.UnitOfWork.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeatController()
        {
            //_unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetSeatAsync()
        {
            //using(var transaction = _unitOfWork.BeginTransaction())
            //{
            //    await _unitOfWork.SeatRepository.CreateAsync(new Seat { Number = 1 }).ConfigureAwait(false);
            //    //transaction.Commit();
            //    await _unitOfWork.SeatRepository.CreateAsync(new Seat { Number = 2 }).ConfigureAwait(false);
            //    //transaction.Rollback();
            //}
            //var result = await _unitOfWork.SeatRepository.GetAllAsync().ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeatAsync([FromBody] int number)
        {
            //await _unitOfWork.SeatRepository.CreateAsync(new Seat { Number = number }).ConfigureAwait(false);
            return Ok();
        }
    }
}
