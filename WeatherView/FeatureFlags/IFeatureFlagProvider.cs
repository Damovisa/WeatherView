namespace WeatherView.FeatureFlags
{
    public interface IFeatureFlagProvider
    {
        bool Toggle(string flagKey, int userId, string userName);
    }
}