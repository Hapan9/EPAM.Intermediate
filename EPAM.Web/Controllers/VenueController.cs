using EPAM.Persistence.Entities;
using EPAM.Persistence.UnitOfWork.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public VenueController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetVenuesAsync()
        {
            //using(var transaction = _unitOfWork.BeginTransaction())
            //{
            //    await _unitOfWork.SeatRepository.CreateAsync(new Seat { Number = 1 }).ConfigureAwait(false);
            //    //transaction.Commit();
            //    await _unitOfWork.SeatRepository.CreateAsync(new Seat { Number = 2 }).ConfigureAwait(false);
            //    //transaction.Rollback();
            //}
            using var unitOfWork = _unitOfWorkFactory.Create();
            var result = await unitOfWork.VenueRepository.GetAllAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenueAsync([FromBody] string name)
        {
            using var unitOfWork = _unitOfWorkFactory.Create();
            unitOfWork.BeginTransaction();
            await unitOfWork.VenueRepository.CreateAsync(new Venue { Name = name }).ConfigureAwait(false);
            unitOfWork.RollbackTransaction();
            await unitOfWork.VenueRepository.CreateAsync(new Venue { Name = name }).ConfigureAwait(false);
            return Ok();
        }
    }
}
