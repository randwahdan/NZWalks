using AutoMapper;
using Azure.Core.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.ComponentModel;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper autoMapper;
        private readonly ILogger<RegionController> logger;

        public RegionController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper autoMapper,ILogger<RegionController>logger)
        {
            //DI
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.autoMapper = autoMapper;
            this.logger = logger;
        }
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("This is a custom exception");
                // Repository is responsible to talk with database
                var regionsDomain = await regionRepository.GetAllAsync();
                logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");

                //Map Domain Models to DTOs
                //var regionsDTO = new List<RegionDTO>();
                //foreach (var regionDomain in regionsDomain)
                //{
                //    regionsDTO.Add(new RegionDTO()
                //    {
                //        Id = regionDomain.Id,
                //        Name = regionDomain.Name,
                //        Code = regionDomain.Code,
                //        RegionImageUrl = regionDomain.RegionImageUrl,
                //    });
                //}

                //Map Domain Models to DTOs
                var regionsDTO = autoMapper.Map<List<RegionDTO>>(regionsDomain);

                //return DTOs
                return Ok(regionsDTO);
            }
            catch(Exception ex) 
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            /*logger.LogInformation("GetAllRegions Action Method was invoked");
            logger.LogWarning("This is a warning log");
            logger.LogError("This ia a error log");*/
            
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]

        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //var region=dbContext.Regions.Find(id); only can be used with id property

            // Get Region Domain Model From Database
            var regionDomain = await regionRepository.GetRegionByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Return DTo back to the client
            return Ok( autoMapper.Map<RegionDTO>(regionDomain));
        }

        // POST to create new region
        // POST: https://localhost(portNumber)/api/region
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task <IActionResult> CreateRegion([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map or Convert DTO to Domain Model

            var regionDomainModel = autoMapper.Map<Region>(addRegionRequestDTO);
            //Use Domain Model to create Region
            await regionRepository.CreateRegionAsync(regionDomainModel);
            

            //Map Domain model back to DTO
            var regionsDTO = autoMapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionsDTO);
        }

        // Update Region 
        // PUT: https://localhost(prtNumber)/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var regionDomainModel = autoMapper.Map<Region>(updateRegionRequestDTO);
            // Check if region exists
            regionDomainModel= await regionRepository.UpdateRegionAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(autoMapper.Map<RegionDTO>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task <IActionResult> DeleteRegion([FromRoute]Guid id) 
        {
            var regionDomainModel= await regionRepository.DeleteRegionAsync(id);
            if (regionDomainModel == null) 
            {
                return NotFound();
            }

            // (optional) return deleted region back
            
            return Ok(autoMapper.Map<RegionDTO>(regionDomainModel));
        }
    }
}
