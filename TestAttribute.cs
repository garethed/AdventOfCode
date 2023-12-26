namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestAttribute : Attribute 
{
    object expected;
    object[] parameters;        

    public TestAttribute(object expected, params object[] parameters) {
        this.expected = expected;
        this.parameters = parameters;
    }

    public static bool TestAnnotatedMethods(object target) {

        bool ret = true;

        foreach (var m in target.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static)) {
            foreach (var t in (TestAttribute[])m.GetCustomAttributes(typeof(TestAttribute), false)) {

                try {

                    var p2 = m.GetParameters().Zip(t.parameters, (targetParam, value) => CoerceParameter(value, targetParam.ParameterType)).ToArray(); 
                    var o = m.Invoke(target, p2);
                    var e2 = Convert.ChangeType(t.expected, m.ReturnType);
                    ret &= Utils.Assert(p2, ToString(o), ToString(e2));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ret = false;
                    Utils.Assert(t.parameters, "[test error]", t.expected.ToString());
                }
            }
        }

        return ret;
    }

    private static string ToString(object o)
    {
        if (o is long) return string.Format("{0:#,##0}", (long) o);
        return o.ToString();

    }

    private static object CoerceParameter(object value, Type targetType)
    {
        if (value is string)
        {
            return Input.CoerceInputParameter((string) value, targetType);
        }

        return Convert.ChangeType(value, targetType);

    }
}