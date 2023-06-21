using AutoMapper;
using AutoMobile.Application.ApplicationConstants;
using AutoMobile.Application.Contracts.Persistence;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.DTO.Vehicle;
using AutoMobile.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AutoMobile.Web.Controllers
{
    [Authorize]
    [Route("api/topspeed/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public VehicleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Get()
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicle.GetAllAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<List<VehicleDto>>(vehicles);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                //_response.AddError(ex.Message.ToString());
                _response.AddError(CommonMessage.SystemError);
            }

            return Ok(_response);

        }

        [HttpGet]
        [Route("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetById([Required] int id)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicle.GetByIdAsync(id);

                if (vehicle == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.AddError(CommonMessage.RecordNotFound);
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VehicleDetailsDto>(vehicle);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                //_response.AddError(ex.Message.ToString());
                _response.AddError(CommonMessage.SystemError);
            }

            return Ok(_response);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Create(VehicleCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var vehicle = _mapper.Map<Vehicle>(dto);

                await _unitOfWork.Vehicle.Create(vehicle);

                await _unitOfWork.SaveAsync();

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = vehicle;
                _response.DisplayMessage = CommonMessage.RecordCreated;
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                //_response.AddError(ex.Message.ToString());
                _response.AddError(CommonMessage.SystemError);
            }

            return Ok(_response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> Update([Required] int id, [FromBody] VehicleUpdateDto dto)
        {
            try
            {
                if (dto == null || id != dto.Id)
                {
                    return BadRequest();

                }

                var villa = _mapper.Map<Vehicle>(dto);

                await _unitOfWork.Vehicle.Update(villa);

                await _unitOfWork.SaveAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommonMessage.RecordUpdated;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                //_response.AddError(ex.Message.ToString());
                _response.AddError(CommonMessage.SystemError);
            }

            return Ok(_response);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var vehicle = await _unitOfWork.Vehicle.GetByIdAsync(id);

                if (vehicle == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Vehicle.Delete(vehicle);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommonMessage.RecordDeleted;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                //_response.AddError(ex.Message.ToString());
                _response.AddError(CommonMessage.SystemError);
            }

            return Ok(_response);
        }
    }
}
