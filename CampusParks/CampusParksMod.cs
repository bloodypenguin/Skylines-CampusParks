using CitiesHarmony.API;
using ICities;

namespace CampusParks
{
    public class ChangeLoadingImageMod : IUserMod
    {
        public string Name => "Campus Parks r2.0.0";

        public string Description => "Allows to place parks on pedestrian paths in campuses, airports and industry zones";

        public void OnEnabled()
        {
            HarmonyHelper.EnsureHarmonyInstalled();
        }
    }
}