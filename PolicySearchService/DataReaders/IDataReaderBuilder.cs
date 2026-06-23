using FastMember;

namespace PolicySearchService.DataReaders;

public interface IDataReaderBuilder<T>
{
    ObjectReader BuildReader(IEnumerable<T> records);
}
