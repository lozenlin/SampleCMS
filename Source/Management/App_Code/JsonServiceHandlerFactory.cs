using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsonService
{
    public static class JsonServiceHandlerFactory
    {
        public static IJsonServiceHandler GetHandler(HttpContext context, string serviceName)
        {
            IJsonServiceHandler handler = null;

            switch (serviceName)
            {
                case "TempStoreRolePvg":
                    handler = new TemporarilyStoreRolePrivilege(context);
                    break;
                case "UpdateArticleIsAreaShowInFrontStage":
                    handler = new UpdateArticleIsAreaShowInFrontStage(context);
                    break;
                case "UpdateArticleSortFieldOfFrontStage":
                    handler = new UpdateArticleSortFieldOfFrontStage(context);
                    break;
            }

            return handler;
        }
    }
}