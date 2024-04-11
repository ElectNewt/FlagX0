using System.Security.Claims;

namespace FlagX0.Web.Business.UserInfo
{

	public interface IFlagUserDetails
	{
		public string UserId { get; }
	}

	public class FlagUserDetails(IHttpContextAccessor httpContextAccessor)
		: IFlagUserDetails
	{

		public string UserId => httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
			?? throw new Exception("This workflow require authentication");
	}
}
