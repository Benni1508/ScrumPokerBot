using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public class MyList<T> : IEnumerable<T> where T: IHaveId
    {
        private readonly List<T> items;
        private readonly object lockObj;

        public MyList()
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

        public void Add(T session)
        {
            lock (lockObj)
            {
                if (session == null)
                {
                    
                }
                items.Add(session);
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