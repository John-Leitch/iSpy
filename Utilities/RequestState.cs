using System.Net;

namespace iSpyApplication.Utilities
{
    public class RequestState
    {
        // This class stores the request state of the request.
        public WebRequest Request;
        public WebResponse Response;
        public ConnectionOptions ConnectionOptions;

        public RequestState()
        {
            Request = null;
            Response = null;
            ConnectionOptions = null;
        }
    }
}
