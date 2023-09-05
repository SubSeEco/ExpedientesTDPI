using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Web
{
    /// <summary>
    /// TokenAccess
    /// </summary>
    public class TokenAccess
    {
        /// <summary>
        /// access_token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// token_type
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// expires_in
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// id_token
        /// </summary>
        public string id_token { get; set; }
    }
}