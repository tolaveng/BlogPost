using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Settings
{
    public class AzureStorageSetting
    {
        public string DefaultEndpointsProtocol { get; set; } = string.Empty;
        public string EndpointSuffix { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountKey { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
