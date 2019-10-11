using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace IntraVisionTest.Extentions
{
    public static class MyExtentions
    {
        public static MvcHtmlString ImageActionLink(this AjaxHelper ajaxHelper, byte[] imageData,string cssClass, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            TagBuilder image = new TagBuilder("img");
            var v = System.Convert.ToBase64String(imageData);
            image.Attributes.Add("src", "data:image;;base64," + System.Convert.ToBase64String(imageData));
            image.AddCssClass(cssClass);

            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, image.ToString()));
        }
        public static MvcHtmlString ImagePathActionLink(this AjaxHelper ajaxHelper, string imagePath, string cssClass, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            TagBuilder image = new TagBuilder("img");
            image.Attributes.Add("src", imagePath);
            image.AddCssClass(cssClass);

            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, image.ToString()));
        }
    }
}