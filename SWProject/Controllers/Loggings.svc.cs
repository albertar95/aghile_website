using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using SWProject.Models.DomainModel;

namespace SWProject.Controllers
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Loggings
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public static void TrackThis(HttpRequestBase REQUEST,HttpResponseBase RESPONSE)
        {
            try
            {
                SwpEntities db = new SwpEntities();
                Log log = new Log
                {
                    Id = Guid.NewGuid(),
                    Browser = REQUEST.Browser.Browser.ToString(),
                    RequestType = REQUEST.RequestType.ToString(),
                    ResponseStatus = RESPONSE.Status.ToString(),
                    TimeStamp = DateTime.Now,
                    Url = REQUEST.Url.ToString(),
                    UserAgent = REQUEST.UserAgent.ToString(),
                    VirtualPath = REQUEST.AppRelativeCurrentExecutionFilePath.ToString()
                };
                if (REQUEST.UrlReferrer == null)
                    log.UrlRefferer = "own";
                else
                    log.UrlRefferer = REQUEST.UrlReferrer.ToString();
                if ("swp".Contains(log.VirtualPath.ToLower()))
                    log.Type = false;
                else
                    log.Type = true;

                db.Logs.Add(log);
                db.SaveChanges();
                return;
            }
            catch (Exception)
            {
            }
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
