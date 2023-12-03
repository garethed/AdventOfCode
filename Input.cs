using System.Reflection;

record Input (string Data) 
{

    public static Input Get(int Year, int Day)
    {
        return InputFetcher.GetInput(Year, Day);
    }

    public object GetTyped(Type type) 
    {
        return CoerceInputParameter(Data, type);    
    }

    public static object CoerceInputParameter(string data, Type targetType)
    {
        switch (targetType.Name) 
        {
            case nameof(Int32):
                return int.Parse(data);
            case nameof(String):
                return data;  
            case "String[]":
                return Utils.splitLines(data);
        }

        if (targetType.Name == "List`1")
        {
            try 
            {
                var deserializeMethodInfo = typeof(RegexDeserializable).GetMethod("Deserialize").MakeGenericMethod(targetType.GenericTypeArguments[0]);
                return deserializeMethodInfo.Invoke(null, new[] { data });
            }
            catch (InvalidOperationException)
            {            
            }            
        }
        if (targetType.BaseType == typeof(Array))
        {
            var elementType = targetType.GetElementType();
            var stringData = Utils.splitLines(data);
            var typedArray = Array.CreateInstance(elementType, stringData.Length);

            for (var i = 0; i < typedArray.Length; i++)
            {
                typedArray.SetValue(CoerceInputParameter(stringData[i], elementType), i);
            }            

            return typedArray;            
        }

        var constructor = targetType.GetConstructor(new [] { typeof(string[]) });
        if (constructor != null)
        {
            return constructor.Invoke(new[] { Utils.splitLines(data) });
        }            

        constructor = targetType.GetConstructor(new [] { typeof(String) });
        if (constructor != null)
        {
            return constructor.Invoke(new[] { data });
        }            

        throw new InvalidCastException();        

    }



    public static implicit operator string(Input i) => i.Data;
}