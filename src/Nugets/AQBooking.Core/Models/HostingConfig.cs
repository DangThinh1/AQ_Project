using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Core.Models
{
    /// <summary>
    /// Represents startup hosting configuration parameters
    /// </summary>
    public class HostingConfig
    {
        /// <summary>
        /// Gets or sets custom forwarded HTTP header (e.g. CF-Connecting-IP, X-FORWARDED-PROTO, etc)
        /// </summary>
        public static string ForwardedHttpHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTTP_CLUSTER_HTTPS
        /// </summary>
        public static bool UseHttpClusterHttps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTTP_X_FORWARDED_PROTO
        /// </summary>
        public static bool UseHttpXForwardedProto { get; set; }
    }
}
