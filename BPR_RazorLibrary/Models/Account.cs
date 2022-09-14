namespace BPR_RazorLib.Models
{
	public class Account
	{
		#region Properties

		private int? accountId;

		public int? AccountId
		{
			get { return accountId; }
			set { accountId = value; }
		}

		private string username;

		public string Username
		{
			get { return username; }
			set { username = value; }
		}

		private string password;

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string contact;

		public string Contact
		{
			get { return contact; }
			set { contact = value; }
		}

		private string email;

		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		private string location;

		public string Location
		{
			get { return location; }
			set { location = value; }
		}

		#endregion

		public Account()
		{

		}

		public Account(int? accountId, string username, string password, string name, string contact, string email, string location)
		{
			AccountId = accountId;
			Username = username;
			Password = password;
			Name = name;
			Contact = contact;
			Email = email;
			Location = location;
		}
	}
}
