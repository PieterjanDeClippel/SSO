using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Sso.Application.Data.Exceptions
{
    public class OtherAccountException : Exception
	{
		internal OtherAccountException(IList<UserLoginInfo> existing_logins)
			: base($"Please login with your {existing_logins[0].ProviderDisplayName} account instead")
		{
		}
	}
}
