using LiveTest.Client.Data;
using LiveTest.Client.Repositories;
using System.Net.Http.Json;
using TnTComponents.Grid;

namespace LiveTest.Client.Internal;
public class DataGridRepository(HttpClient client) : IDataGridRepository {

    private readonly HttpClient _client = client;

    public async Task<TnTItemsProviderResult<DataGridItem>> Get(TnTItemsProviderRequest? request) {
        return await _client.GetFromJsonAsync<TnTItemsProviderResult<DataGridItem>>("/getDataGridItems");
    }
}

