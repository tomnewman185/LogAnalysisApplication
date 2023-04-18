using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    /// <summary>
    /// TorHelper - Class that contains logic relating to checking if an IP is a Tor end point or not
    /// </summary>
    public class TorHelper
    {
        /// <summary>
        /// DownloadAndCreateFileTorExitIPAddressesAsync() - pulls file containing list of Tor end points
        /// </summary>
        /// <param name="destinationFileName"></param>
        /// <returns></returns>
        public static async Task<bool> DownloadAndCreateFileTorExitIPAddressesAsync(string destinationFileName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = await client.GetStreamAsync(@"https://check.torproject.org/torbulkexitlist"))
                    {
                        using (var fs = new FileStream(destinationFileName, FileMode.OpenOrCreate))
                        {
                            await s.CopyToAsync(fs);

                            await fs.DisposeAsync();

                            await s.DisposeAsync();

                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// GetTorExitIPAddresses() - gets Tor end points
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<HashSet<string>> GetTorExitIPAddresses(string fileName)
        {
            // Check to see if there is a file that already exists
            bool fileAlreadyExists = File.Exists(fileName);

            // Attempt to download newer file containing Tor EndPoint IP addresses
            if (await TorHelper.DownloadAndCreateFileTorExitIPAddressesAsync(fileName) == true)

            {
                return await GetIPAddressesFromFile(fileName);
            }
            else
            {
                // Get here if new download failed.
                // If file already exists then use the previous file as error with trying to download a newer one
                if (fileAlreadyExists)
                {
                    return await GetIPAddressesFromFile(fileName);
                }
                else
                {
                    // Return an empty HashSet as not prior file and download failed.
                    return new HashSet<string>();
                }
            }

            static async Task<HashSet<string>> GetIPAddressesFromFile(string fileName)
            {
                HashSet<string> ipAddresses = new HashSet<string>();

                using (var rdr = new StreamReader(fileName))
                {
                    while (!rdr.EndOfStream)
                    {
                        ipAddresses.Add(await rdr.ReadLineAsync());
                    }
                }

                return ipAddresses;
            }
        }

        /// <summary>
        /// IsTorExitIPAddressAsync() - determines if a given IP is a Tor end point
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static async Task<bool> IsTorExitIPAddressAsync(string ipAddress)
        {
            try
            {
                var hostEntry = await Dns.GetHostEntryAsync($"{ReverseIPAddress(ipAddress)}.dnsel.torproject.org");

                return hostEntry.AddressList[0].ToString() == "127.0.0.2";
            }
            catch (Exception)
            {
                return false;
            }

            static string ReverseIPAddress(string ipAddress)

            {
                var parts = ipAddress.Split(new char[] { '.' });

                return string.Join('.', parts.Reverse());
            }
        }
    }
}
