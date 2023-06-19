using AutoMapper;
using AutoMobile.Application.ApplicationConstants;
using AutoMobile.Application.Contracts.Persistence;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.DTO.Brand;
using AutoMobile.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AutoMobile.Web.Controllers
{
    [Route("api/topspeed/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public BrandController(IUnitOfWork unitOfWork, IMapper mapper)
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
                var brands = await _unitOfWork.Brand.GetAllAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<List<BrandDto>>(brands);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.AddError(ex.Message.ToString());
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
                var brand = await _unitOfWork.Brand.GetByIdAsync(id);

                if (brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.AddError(CommonMessage.RecordNotFound);
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<BrandDto>(brand);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.AddError(ex.Message.ToString());
            }

            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Create(BrandCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var brand = _mapper.Map<Brand>(dto);

                await _unitOfWork.Brand.Create(brand);

                await _unitOfWork.SaveAsync();

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = brand;
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.AddError(ex.Message.ToString());
            }

            return Ok(_response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> Update([Required] int id, [FromBody] BrandUpdateDto dto)
        {
            try
            {
                if (dto == null || id != dto.Id)
                {
                    return BadRequest();

                }

                var villa = _mapper.Map<Brand>(dto);

                await _unitOfWork.Brand.Update(villa);

                await _unitOfWork.SaveAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.AddError(ex.Message.ToString());
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

                var brand = await _unitOfWork.Brand.GetByIdAsync(id);

                if (brand == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Brand.Delete(brand);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.AddError(ex.Message.ToString());
            }

            return Ok(_response);
        }
    }
}
