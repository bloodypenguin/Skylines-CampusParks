using System;
using System.Reflection;
using CitiesHarmony.API;
using HarmonyLib;
using ICities;

namespace CampusParks
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private const string HarmonyId = "github.com/bloodypenguin/Skylines-CampusParks";
        private static Harmony _harmonyInstance;

        public override void OnCreated(ILoading loading)
        {
            if (!HarmonyHelper.IsHarmonyInstalled)
            {
                return;
            }

            _harmonyInstance = new Harmony(HarmonyId);
            _harmonyInstance.PatchAll();

            var transpiler = typeof(ParkBuildingAIPatch).GetMethod(nameof(ParkBuildingAIPatch.Transpiler), BindingFlags.Static | BindingFlags.Public);
            Transpile(nameof(ParkBuildingAI.BeginRelocating), transpiler);
            Transpile(nameof(ParkBuildingAI.BuildingLoaded), transpiler);
            Transpile(nameof(ParkBuildingAI.CheckBuildPosition), transpiler);
            Transpile(nameof(ParkBuildingAI.CreateBuilding), transpiler);
            Transpile(nameof(ParkBuildingAI.EndRelocating), transpiler);
            Transpile("FindRoadAccess", transpiler);
            Transpile("HandleCrime", transpiler);
            Transpile("HandleCommonConsumption", transpiler);
            Transpile("HandleDead2", transpiler);
            Transpile(nameof(ParkBuildingAI.ParkAreaChanged), transpiler);
            Transpile(nameof(ParkBuildingAI.ReleaseBuilding), transpiler);
        }

        private void Transpile(string methodName, MethodInfo transpiler)
        {
            try
            {
                _harmonyInstance.Patch(
                    typeof(ParkBuildingAI).GetMethod(methodName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
                    transpiler: new HarmonyMethod(transpiler));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Failed to transpile method " + methodName);
                UnityEngine.Debug.LogException(e);
            }
        }

        public override void OnReleased()
        {
            if (!HarmonyHelper.IsHarmonyInstalled)
            {
                return;
            }

            _harmonyInstance?.UnpatchAll();
        }
    }
}