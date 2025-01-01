using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

public class LazyDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();
    private readonly Func<TKey, TValue> _valueFactory;

    public ICollection<TKey> Keys => _dict.Keys;

    public ICollection<TValue> Values => _dict.Values;

    public int Count => _dict.Count;

    public bool IsReadOnly => false;

    public TValue this[TKey key] 
    { 
        get => GetValue(key); 
        set => _dict[key] = value;
    }

    public LazyDictionary(Func<TKey, TValue> valueFactory)
    {
        _valueFactory = valueFactory;
    }

    public TValue GetValue(TKey key)
    {
        if (!_dict.TryGetValue(key, out TValue value))
        {
            value = _valueFactory(key);
            _dict[key] = value;
        }
        return value;
    }

    public void Add(TKey key, TValue value)
    {
        _dict.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return _dict.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return _dict.Remove(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dict.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _dict.Add(item);
    }

    public void Clear()
    {
        _dict.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dict.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _dict.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return _dict.Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dict.GetEnumerator();
    }
}

public class ListValuedDictionary<TKey, TValue> : LazyDictionary<TKey, List<TValue>>
{
    public ListValuedDictionary() : base((k) => []) {}

    public void Add(TKey key, TValue value)
    {
        this[key].Add(value);
    }

}
