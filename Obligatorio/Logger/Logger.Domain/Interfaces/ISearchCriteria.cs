namespace Logger.Domain.Interfaces
{
    public interface ISearchCriteria<T>
    {
        public bool MatchesCriteria(T entity);
    }
}