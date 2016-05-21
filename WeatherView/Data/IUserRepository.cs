using WeatherView.Models;

namespace WeatherView.Data
{
    public interface IUserRepository
    {
        User GetUserProfile(int id);
    }
}