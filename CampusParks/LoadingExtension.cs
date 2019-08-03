using System;
using System.Net;
using System.Net.Security;
using System.Reflection;
using Harmony;
using ICities;
using UnityEngine;

namespace CampusParks
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private HarmonyInstance HarmonyInstance;

        public override void OnCreated(ILoading loading)
        {
            HarmonyInstance = HarmonyInstance.Create("github.com/bloodypenguin/Skylines-CampusParks");
   
            var transpiler = typeof(ParkBuildingAIPatch).GetMethod(nameof(ParkBuildingAIPatch.Transpiler), BindingFlags.Static | BindingFlags.Public);
            Transpile(nameof(ParkBuildingAI.BeginRelocating), transpiler);
            Transpile(nameof(ParkBuildingAI.BuildingLoaded), transpiler);
            Transpile(nameof(ParkBuildingAI.CheckBuildPosition), transpiler);
            Transpile(nameof(ParkBuildingAI.CreateBuilding), transpiler);
            Transpile(nameof(ParkBuildingAI.EndRelocating), transpiler);
            Transpile("FindRoadAccess", transpiler);
            Transpile("HandleCrime", transpiler);
            Transpile("HandleDead2", transpiler);
            Transpile(nameof(ParkBuildingAI.ParkAreaChanged), transpiler);
            Transpile(nameof(ParkBuildingAI.ReleaseBuilding), transpiler);
            
        }

        private void Transpile(string methodName , MethodInfo transpiler)
        {
            try
            {
                HarmonyInstance.Patch(
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
            HarmonyInstance?.UnpatchAll();
        }
    }
}