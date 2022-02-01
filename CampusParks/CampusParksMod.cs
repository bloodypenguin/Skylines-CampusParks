using CitiesHarmony.API;
using ICities;

namespace CampusParks
{
    public class ChangeLoadingImageMod : IUserMod
    {
        public string Name => "Campus Parks r1.0.2";

        public string Description => "Allows to place parks on pedestrian paths in campuses";

        public void OnEnabled()
        {
            HarmonyHelper.EnsureHarmonyInstalled();
        }
    }
}