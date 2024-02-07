using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase {
    private readonly AuctionDbContext _ctx;
    private readonly IMapper _mapper;

    public AuctionController(AuctionDbContext ctx, IMapper mapper) {
        _ctx = ctx;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAll() {
        var auction = await _ctx.Auctions.Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();
        return _mapper.Map<List<AuctionDto>>(auction);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id) {
        var auction = await _ctx.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (auction != null) {
            return _mapper.Map<AuctionDto>(auction);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto) {
        var auction = _mapper.Map<Auction>(auctionDto);
        auction.Seller = "test";
        _ctx.Auctions.Add(auction);
        var result = await _ctx.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not save to DB");
        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, _mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto) {
        var auction = await _ctx.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return NotFound();
        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        var result = await _ctx.SaveChangesAsync() > 0;
        if (result) return Ok();
        return BadRequest("Problem saving changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id) {
        var auction = await _ctx.Auctions.FindAsync(id);
        if (auction == null) return NotFound();
        _ctx.Auctions.Remove(auction);
        var result = await _ctx.SaveChangesAsync() > 0;
        if (result) return Ok();
        return BadRequest();
    }
}