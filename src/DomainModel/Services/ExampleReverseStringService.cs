using System;

namespace DomainModel.Services;

public interface IExampleReverseStringService
{
    string ReverseString(string inputStr);
}
public class ExampleReverseStringService : IExampleReverseStringService
{
    public string ReverseString(string inputStr)
    {
        char[] charArray = inputStr.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}