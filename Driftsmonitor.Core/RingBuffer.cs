using System.Collections;
namespace DataStructures;

public class RingBuffer<T> : IEnumerable<T>
{
    private readonly T[] _items;
    private int _start;   // indeks i _items til det eldste elementet
    private int _count;   // hvor mange plasser som faktisk er i bruk

    public RingBuffer(int capacity)
    {
    if (capacity < 1)
        {
        throw new ArgumentOutOfRangeException(nameof(capacity));
        }
    _items = new T[capacity];
    }

    public int Capacity => _items.Length;
    public int Count => _count;
    public bool IsFull => _count == _items.Length;

    /// <summary>Legger til et element. Overskriver det eldste hvis bufferet er fullt.</summary>
    public void Add(T item)
    {
        if (_count < Capacity)
            {
            int index = (_start + _count) % Capacity;
            _items[index] = item;
            _count++; 
            }
        else
            {
            _items[_start] = item;
            _start = (_start + 1) % Capacity;
            }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        return _items[(_start + index) % Capacity];
        }
    }

    public T Oldest
    {
        get
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Buffer is empty");
            }
        return _items[_start];
        }
    }

    /// <summary>Det nyeste elementet. Thrower en passende exception hvis bufferet er tomt.</summary> 
    /// 
     public T Newest
    {
        get
        {
            if (_count == 0)
            {
            throw new InvalidOperationException("Buffer is empty");
            }
        return _items[(_count + _start - 1) % Capacity];
        }
    }

    public void Clear()
    {
        Array.Clear(_items);
        _start = 0;
        _count = 0;
    }

    public T[] ToArray()
    {
        T[] result = new T[_count];

        for (int i = 0; i < _count; i++)
        {
            result[i] = this[i];
        }
    return result;
    }
    public IEnumerator<T> GetEnumerator()
    {
    // TODO: Return elements from oldest to newest
    for (int i = 0; i < _count; i++)
        {
            yield return this[i];
        }
    }

IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}