﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper autoMapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper autoMapper, IWalkRepository walkRepository)
        {
            this.autoMapper = autoMapper;
            this.walkRepository = walkRepository;
        }

        public IWalkRepository WalkRepository { get; }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
            // Map DTO to Domain Model
            var walkDomainModel = autoMapper.Map<Walk>(addWalkRequestDTO);
            await walkRepository.CreateAsync(walkDomainModel);

            //Map Domain model to DTO
            return Ok(autoMapper.Map<WalkDTO>(walkDomainModel));

        }

        // GET all walks
        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy , [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize=1000)
        {
            
                var walksDomainModel = await walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // Create an Exception
            //throw new Exception("this is a new exception");

            return Ok(autoMapper.Map<List<WalkDTO>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetWalkByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model To DTO
            return Ok(autoMapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalks([FromRoute] Guid id, UpdateWalksRequestDTO updateWalksRequestDTO)
        {

            // Map Dto to Domain model
            var updateDomainModel = autoMapper.Map<Walk>(updateWalksRequestDTO);
            updateDomainModel = await walkRepository.UpdateWalksAsync(id, updateDomainModel);
            if (updateDomainModel == null)
            {
                return NotFound();
            }
            return Ok(autoMapper.Map<WalkDTO>(updateDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id) 
        {
            var deletedWalkDomainModel=await walkRepository.DeleteWalkByIdAsync(id);
            if (deletedWalkDomainModel == null) 
            {
                return NotFound();
            }
            return Ok(autoMapper.Map<WalkDTO>(deletedWalkDomainModel));
        }
    }
}
