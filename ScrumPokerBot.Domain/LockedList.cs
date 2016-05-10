using ScrumPokerBot.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScrumPokerBot.Domain
{
    public class LockedList<T> : IEnumerable<T> where T: IHaveId
    {
        private readonly List<T> items;
        private readonly object lockObj;

        public LockedList()
        {
            items = new List<T>();
            lockObj = new object();
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (lockObj)
            {
                return items.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            lock (lockObj)
            {
                items.Add(item);
            }
        }

        public T[] ToArrayLocked()
        {
            lock (lockObj)
            {
                return this.items.ToArray();
            }
        }

        public void Remove(int sessionId)
        {
            lock (lockObj)
            {
                var session = items.FirstOrDefault(g => g.Id == sessionId);
                if (session != null)
                {
                    items.Remove(session);
                }
            }
        }
    }
}