namespace ViewQuickApp.Server.Core.Dtos.Auth
{
    public class LoginServicesResponseDto
    {

        public string NewToken { get; set; }

        // this will be retured on front-end

        public UserInfoResult Userinfo { get; set; }

    }
}
