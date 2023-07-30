using System.Collections.Generic;

namespace TE.BE.City.Domain.Interfaces
{
    public interface INewsDomain<T>
    {
        public void Add(T item, int weight);
        public void Add(ICollection<WeightedListItem<T>> listItems);
        public void AddWeightToAll(int weight);
        public T Next();
        public void Clear();
    }
}
