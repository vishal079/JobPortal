using System;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web
{
	public static class HtmlHelperExtensions
	{
		/// <summary>
		/// Render all messages that have been set during execution of the controller action.
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		public static HtmlString RenderMessages(this HtmlHelper htmlHelper)
		{
			var messages = String.Empty;

			foreach (var messageType in Enum.GetNames(typeof(MessageType)))
			{
				var message = htmlHelper.ViewContext.ViewData.ContainsKey(messageType)
								? htmlHelper.ViewContext.ViewData[messageType]
								:( htmlHelper.ViewContext.TempData.ContainsKey(messageType)
									? htmlHelper.ViewContext.TempData[messageType]
									: null);
				if (message != null)
				{
                    if(messageType ==  Enum.GetName(typeof(MessageType),MessageType.Success))
                    {
                        messages += "<div class='alert alert-success'><button type='button' class='close' data-dismiss='alert'>×</button>" + message + "</div>";
                    }
                    else if(messageType ==  Enum.GetName(typeof(MessageType),MessageType.Error))
                    {
                        messages += "<div class='alert alert-error'><button type='button' class='close' data-dismiss='alert'>×</button>" + message + "</div>";
                    }
                    else if (messageType == Enum.GetName(typeof(MessageType), MessageType.Warning))
                    {
                        messages += "<div class='alert'><button type='button' class='close' data-dismiss='alert'>×</button>" + message + "</div>";
                    }
                    
				}

                if(htmlHelper.ViewContext.ViewData.ContainsKey(messageType))
                {
                    htmlHelper.ViewContext.ViewData[messageType] = null;
                }
                else if (htmlHelper.ViewContext.TempData.ContainsKey(messageType))
                {
                    htmlHelper.ViewContext.TempData[messageType] = null;
                }

			}

			return MvcHtmlString.Create(messages);
		}
	}
}