using LaunchDarkly.Client;

namespace WeatherView.FeatureFlags
{
    class LaunchDarklyFeatureFlagProvider : IFeatureFlagProvider
    {
        private readonly LdClient _ldClient;

        public LaunchDarklyFeatureFlagProvider(LdClient ldClient)
        {
            _ldClient = ldClient;
        }

        public bool Toggle(string flagKey, int userId, string userName)
        {
            var user = new User(userId.ToString())
            {
                Name = userName
            };
            return _ldClient.Toggle(flagKey, user);
        }
    }
}