using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace FxSsh.SshServerModule
{


    /// <summary>
    /// Either provide Password or Key for login
    /// </summary>
    /// <value></value>
    public class User
    {
        public string Username { get; set; }
        public string UserRootDirectory { get; set; }
        public string HashedPassword { get; set; }
        public string RsaKey { get; set; }
        public bool OnlyWhitelistedIps { get; set; }
        public List<string> WhitelistedIps { get; set; }
        public DateTime LastSuccessfulLogin { get; set; }

        internal bool VerifyUserIpWhitelisted(EndPoint remoteEndpoint)
        {
            if (!OnlyWhitelistedIps || WhitelistedIps == null || WhitelistedIps.Count == 0) // No check needed
                return true;

            var endpoint = remoteEndpoint as IPEndPoint;

            // https://github.com/lduchosal/ipnetwork
            IPNetwork ipaddress = IPNetwork.Parse(endpoint.Address.ToString());

            foreach(var whitelisted in WhitelistedIps)
            {
                IPNetwork ipnetwork = IPNetwork.Parse(whitelisted); // whitelisted must contain  CIDR e.g. /16
                var success = ipnetwork.Contains(ipaddress);
                if (success)
                    return true;
            }

            return false;
        }

        internal bool VerifyUserKey(byte[] key, string keyAlgorithm)
        {
            throw new NotImplementedException();
        }

        internal bool VerifyUserPassword(string password)
        {
            var sha256 = new SHA256CryptoServiceProvider();
            var pwhashed = sha256.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password));
            var base64encoded = Convert.ToBase64String(pwhashed);

            //var testpw = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ="; // "1234"

            if (base64encoded == HashedPassword)
                return true;
            else
                return false;
        }
    }
}