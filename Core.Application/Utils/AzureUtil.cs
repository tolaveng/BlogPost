using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Core.Infrastructure.Settings;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Core.Application.Utils
{
    public static class AzureUtil
    {
        //public static string MakeConnectionString(AzureStorageSetting setting)
        //{
        //    //return $"DefaultEndpointsProtocol={setting.DefaultEndpointsProtocol};EndpointSuffix={setting.EndpointSuffix};" +
        //    //        $"AccountName={setting.AccountName};AccountKey={setting.AccountKey}";
        //    return setting.ConnectionString;
        //}

        //
        // https://docs.microsoft.com/en-us/azure/storage/blobs/sas-service-create?tabs=dotnet
        public static Uri GenerateSasUriForBlob(BlobClient blobClient,
            string storedPolicyName = null, bool NoExpiry = false)
        {
            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b" // b is blob, c is blob container
                };


                // Access permissions
                if (storedPolicyName == null)
                {
                    sasBuilder.SetPermissions(BlobSasPermissions.Read);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                // Access expiry
                if (NoExpiry)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddYears(99);
                } else
                {
                    // for 1 hour
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                }
                

                try
                {
                    Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                    return sasUri;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }


        // Generate Sas Uri and save to Meta data

        public static string GenerateSasUrl(BlobContainerClient containerClient, string blogName, bool NoExpiry = false)
        {
            try
            {
                if (!containerClient.CanGenerateSasUri) return "";

                var blobClient = containerClient.GetBlobClient(blogName);
                if (blobClient == null || !blobClient.Exists()) return "";

                var uri = GenerateSasUriForBlob(blobClient, null, true);
                if (uri == null) return "";

                // meta data has limit string
                var SasUri = uri.ToString().Split('?')[1];
                var props = blobClient.GetProperties().Value;
                var metaData = props.Metadata;
                metaData.Remove("SasUri");
                metaData.Add("SasUri", SasUri);

                blobClient.SetMetadata(metaData);

                return uri.ToString();

            } catch (Exception)
            {
                // ignored
            }
            return "";
        }


            // https://docs.microsoft.com/en-us/rest/api/eventhub/generate-sas-token
            public static string CreateSASToken(string resourceUri, string keyName, string key)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var aday = 60 * 60 * 24;
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + aday);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture,
                "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
            return sasToken;
        }
    }
}
