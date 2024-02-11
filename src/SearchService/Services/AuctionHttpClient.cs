using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionHttpClient {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public AuctionHttpClient(HttpClient httpClient, IConfiguration config) {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<List<Item>> GetItems() {
        var lastUpdate = await DB.Find<Item, string>()
            .Sort(s => s.Descending(d => d.UpdatedAt))
            .Project(p => p.UpdatedAt.ToString()).ExecuteFirstAsync();
        return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"]
            + "/api/auctions?date=" + lastUpdate);
    }
}