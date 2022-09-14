using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPR_RazorLib.Models
{
	public class WebContent
	{
		public WebResponse response;
		public object content;

		public WebContent(WebResponse response, object content)
		{
			this.response = response;
			this.content = content;
		}
	}
}
