using System.Collections;

namespace UseCases.Test.Resources;

public class RolesInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "user" };
        yield return new object[] { "admin" };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
