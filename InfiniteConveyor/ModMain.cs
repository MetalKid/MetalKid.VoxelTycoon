using HarmonyLib;
using System;
using VoxelTycoon.Modding;
using VoxelTycoon.Tracks.Conveyors;

namespace InfiniteConveyor
{
    /// <summary>
    /// This class mods the game so you can build any length conveyors.
    /// </summary>
    public class ModMain : Mod
    {
        protected override void OnGameStarted()
        {
            base.OnGameStarted();

            try
            {
                var harmony = new Harmony("com.github.metalcore.voxeltycoon.infiniteconveyor");
                harmony.PatchAll(typeof(ModMain).Assembly);
            }
            catch { }
        }

        [HarmonyPatch(typeof(ConveyorSharedData))]
        [HarmonyPatch(nameof(ConveyorSharedData.MaxLength), MethodType.Getter)]
        public class MaxLenghtForConveyor
        {
            [HarmonyPostfix]
            public static void MaxLengthPostfix(ref int __result, ConveyorSharedData __instance)
            {
                __result = 999_999_990;
            }
        }

        [HarmonyPatch(
            typeof(VoxelTycoon.Tools.TrackBuilder.ConveyorBuilderTool),
            "CheckLength",
            new Type[] { }
            )]
        public class PatchedConveyorBuilderTool
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }
    }
}
