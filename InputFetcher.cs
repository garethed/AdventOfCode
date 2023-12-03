static class InputFetcher
{
    static HttpClient AoCHttpClient = BuildHttpClient();

    private static HttpClient BuildHttpClient()
    {
        var client = new HttpClient(new HttpClientHandler() { UseCookies = false });
        client.BaseAddress = new Uri("https://adventofcode.com");        
        client.DefaultRequestHeaders.Add("Cookie", "session=" + Environment.GetEnvironmentVariable("aoc_sessionid"));
        return client;        
    }

    public static Input GetInput(int year, int day)
    {
        return new Input(
            DataCache.Get("input", $"{year}-{day}", () => 
        {
            return TrimSingleLineBreak(AoCHttpClient.GetStringAsync($"/{year}/day/{day}/input").Result);
        }));
    }

    private static string TrimSingleLineBreak(string input)
    {
        if (input.IndexOf('\n') == input.Length - 1)
        {
            return input.TrimEnd('\n');
        }
        return input;
    }
}