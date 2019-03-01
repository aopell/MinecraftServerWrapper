using System.Collections;
using System.Collections.Generic;

namespace MCServerWrapper.Utilities
{
    /// <inheritdoc />
    /// <summary>
    /// A variation of a doubly-linked list structure with a maximum capacity.
    /// As items are added to one side, they are removed from the other if the capacity is exceeded.
    /// </summary>
    public class DropOutLinkedList<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> list = new LinkedList<T>();
        /// <summary>
        /// Maximum size of the list
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// The number of items currently in the list
        /// </summary>
        public int Count => list.Count;
        /// <summary>
        /// The first item of the list
        /// </summary>
        public LinkedListNode<T> First => list.First;
        /// <summary>
        /// The last item of the list
        /// </summary>
        public LinkedListNode<T> Last => list.Last;

        /// <summary>
        /// Creates a new <see cref="DropOutLinkedList{T}"/> with the provided capacity
        /// </summary>
        /// <param name="capacity">The maximum size of the list</param>
        public DropOutLinkedList(int capacity)
        {
            Capacity = capacity;
        }

        /// <summary>
        /// Adds an item to the front of the list.
        /// If the list is over capacity, items are removed from
        /// the end of the list until capacity is no longer exceeded.
        /// </summary>
        /// <param name="item"></param>
        public void AddFirst(T item)
        {
            list.AddFirst(item);
            while (Count > Capacity)
            {
                list.RemoveLast();
            }
        }

        /// <summary>
        /// Removes an item from the front of the list
        /// </summary>
        public void RemoveFirst() => list.RemoveFirst();

        /// <summary>
        /// Adds an item to the end of the list.
        /// If the list is over capacity, items are removed from
        /// the front of the list until capacity is no longer exceeded.
        /// </summary>
        /// <param name="item"></param>
        public void AddLast(T item)
        {
            list.AddLast(item);
            while (Count > Capacity)
            {
                list.RemoveFirst();
            }
        }

        /// <summary>
        /// Removes an item from the end of the list
        /// </summary>
        public void RemoveLast() => list.RemoveLast();

        /// <summary>
        /// Removes all items from the list
        /// </summary>
        public void Clear() => list.Clear();

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
