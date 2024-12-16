using HarmonyLib;
using VoxelTycoon.Cities;
using VoxelTycoon.Modding;

namespace UnlimitedStorage
{
    /// <summary>
    /// This class mods the game so industry buildings have virtually unlimited storage.
    /// </summary>
    public class ModMain : Mod
    {
        protected override void OnGameStarted()
        {
            base.OnGameStarted();

            try
            {
                var harmony = new Harmony("com.github.metalcore.voxeltycoon.unlimitedstorage");
                harmony.PatchAll(typeof(ModMain).Assembly);
            }
            catch { }
        }
    }

    [HarmonyPatch(typeof(CityDemand))]
    [HarmonyPatch(nameof(CityDemand.Capacity), MethodType.Getter)]
    public class CityDemandPatchForCapacity
    {
        [HarmonyPostfix]
        public static void CapacityPostfix(ref int __result, CityDemand __instance)
        {
            __result = 999_999_990;
        }
    }
}
