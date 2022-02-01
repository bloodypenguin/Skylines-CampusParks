using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace CampusParks
{
    public static class ParkBuildingAIPatch
    {
        private static bool IsPark(ref DistrictPark park)
        {
            return DistrictPark.IsParkType(park.m_parkType) || DistrictPark.IsCampusType(park.m_parkType);
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (var index = 0; index < codes.Count; index++)
            {
                var codeInstruction = codes[index];
                if (codeInstruction.opcode != OpCodes.Call || codeInstruction.operand == null ||
                    !codeInstruction.operand.ToString().Contains("get_IsPark"))
                {
                    continue;
                }
                codes[index] = new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(ParkBuildingAIPatch), nameof(IsPark)));
            }

            return codes.AsEnumerable();
        }
    }
}