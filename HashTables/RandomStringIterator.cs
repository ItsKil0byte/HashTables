using System;
using System.Collections;
using System.Text;

public class RandomStringIterator : IEnumerable<Tuple<string, string>>
{
    private readonly int length;
    private readonly HashSet<string> generatedStrings = new HashSet<string>();
    private readonly Random _random = new Random();

    public RandomStringIterator(int length)
    {
        this.length = length;
    }

    public IEnumerator<Tuple<string, string>> GetEnumerator()
    {
        while (true)
        {
            var key = GenerateRandomString(length);
            var value = GenerateRandomString(length);

            if (generatedStrings.Add(key))
            {
                yield return new Tuple<string, string>(key, value);
            }

        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringBuilder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(chars[_random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}
