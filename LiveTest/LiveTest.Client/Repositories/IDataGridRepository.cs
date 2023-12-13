using LiveTest.Client.Data;
using TnTComponents.Grid;

namespace LiveTest.Client.Repositories;
public interface IDataGridRepository {
    Task<TnTItemsProviderResult<DataGridItem>> Get(TnTItemsProviderRequest? request);
}
