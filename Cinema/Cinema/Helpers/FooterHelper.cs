using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinema.Helpers
{
    public static class FooterHelper
    {
        public static MvcHtmlString Footer(this HtmlHelper htmlHelper,
            params string[] items)
        {
            /*<footer><p>content</p></footer>*/
            var footer = new TagBuilder("footer");
            foreach (var item in items)
            {
                var p = new TagBuilder("p");
                p.SetInnerText(item);
                footer.InnerHtml += p.ToString();
            }
            return new MvcHtmlString(footer.ToString());
        }
    }
}