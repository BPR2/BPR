namespace BPR_RazorLibrary.Models;

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
