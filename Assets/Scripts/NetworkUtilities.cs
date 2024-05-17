using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

public static partial class NetworkUtilities
{
    public static async Task<IPAddress> GetPublicIPv4AddressAsync()
    {
        var urlContent =
          await GetUrlContentAsStringAsync("http://ipv4.icanhazip.com/").ConfigureAwait(false);

        return ParseSingleIPv4Address(urlContent);
    }

    public static async Task<string> GetUrlContentAsStringAsync(string url)
    {
        var urlContent = string.Empty;
        try
        {
            using (var httpClient = new HttpClient())
            using (var httpResonse = await httpClient.GetAsync(url).ConfigureAwait(false))
            {
                urlContent =
                  await httpResonse.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle exception
        }

        return urlContent;
    }

    public static IPAddress ParseSingleIPv4Address(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input string must not be null", input);
        }

        var addressBytesSplit = input.Trim().Split('.').ToList();
        if (addressBytesSplit.Count != 4)
        {
            throw new ArgumentException("Input string was not in valid IPV4 format \"a.b.c.d\"", input);
        }

        var addressBytes = new byte[4];
        foreach (var i in Enumerable.Range(0, addressBytesSplit.Count))
        {
            if (!int.TryParse(addressBytesSplit[i], out var parsedInt))
            {
                throw new FormatException($"Unable to parse integer from {addressBytesSplit[i]}");
            }

            if (0 > parsedInt || parsedInt > 255)
            {
                throw new ArgumentOutOfRangeException($"{parsedInt} not within required IP address range [0,255]");
            }

            addressBytes[i] = (byte)parsedInt;
        }

        return new IPAddress(addressBytes);
    }
}