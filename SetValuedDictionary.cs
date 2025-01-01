using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class SetValuedDictionary<TKey, TValue> : IDictionary<TKey, HashSet<TValue>> where TKey : notnull
{
    private Dictionary<TKey, HashSet<TValue>> dict = new Dictionary<TKey, HashSet<TValue>>();

    public HashSet<TValue> this[TKey key] 
    { 
        get 
        {
            if (!dict.ContainsKey(key)) 
            {
                dict[key] = new HashSet<TValue>();
            }
            return dict[key];
        }
        set => dict[key] = value;
    }

    public ICollection<TKey> Keys => dict.Keys;

    public ICollection<HashSet<TValue>> Values => dict.Values;

    public int Count => dict.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, HashSet<TValue> value)
    {
        dict.Add(key, value);
    }

    public void Add(TKey key, TValue value)
    {
        this[key].Add(value);
    }

    public void Add(KeyValuePair<TKey, HashSet<TValue>> item)
    {
        throw new NotImplementedException();    
    }

    public void UnionWith(TKey key, HashSet<TValue> value)
    {
        this[key].UnionWith(value);
    }

    public void Clear()
    {
        dict.Clear();
    }

    public bool Contains(KeyValuePair<TKey, HashSet<TValue>> item)
    {
        return dict.Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        return dict.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, HashSet<TValue>>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, HashSet<TValue>>> GetEnumerator()
    {
        return dict.GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        return dict.Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, HashSet<TValue>> item)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out HashSet<TValue> value)
    {
        return dict.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}