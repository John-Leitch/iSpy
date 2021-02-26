using System.Net;

namespace iSpyApplication.Onvif
{
    public static class NetworkCredentialExtensions
    {
        public static bool IsEmpty(this NetworkCredential networkCredential) => string.IsNullOrEmpty(networkCredential.UserName) || networkCredential.Password == null;
    }
}
