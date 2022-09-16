using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPR_RazorLib.Models
{
	/// <summary>
	/// Contains response states for communication betweeen app and API
	/// </summary>
	public enum WebResponse
	{
		Empty, //temporary value only
		ContentRetrievalSuccess, //found content successfully
		ContentRetrievalFailure, //did not find content
		AuthenticationSuccess, //login validation success
		AuthenticationFailure, //login validation fail
		ContentDoesNotExist, //no content with given data
		ContentDataCorrupted, //data is wrong or null
		ContentUpdateSuccess, //updating content success
		ContentUpdateFailure, //updating content failure
		ContentCreateSuccess, //create content success
		ContentCreateFailure, //create content fail
		ContentDeleteSuccess, //deletion of content success
		ContentDeleteFailure, //deletion of content failure
		ContentDuplicate //content already exists in database
	}
}
