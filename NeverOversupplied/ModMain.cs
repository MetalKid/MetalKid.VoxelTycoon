using System;
using System.Reflection;
using VoxelTycoon;
using VoxelTycoon.Cities;
using VoxelTycoon.Modding;

namespace NeverOversupplied
{
    /// <summary>
    /// This class mods the game so industry buildings never trigger Oversupplied status.
    /// </summary>
    public class ModMain : Mod
    {
        private static readonly ZeroTimeSpanCounterInt _zeroCounter = new ZeroTimeSpanCounterInt();
        private static readonly Type _cityDemandType = typeof(CityDemand);

        private readonly FieldInfo _oversupplyCounterField =
            _cityDemandType.GetField("_oversupplyCounter", BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly MethodInfo _invalidatePriceMethod =
            _cityDemandType.GetMethod("InvalidatePrice", BindingFlags.Instance | BindingFlags.NonPublic);

        protected override void OnGameStarted()
        {
            base.OnGameStarted();

            OverrideSupplyCounter();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            OverrideSupplyCounter();
        }

        private void OverrideSupplyCounter()
        {
            if (_oversupplyCounterField == null 
                || CityManager.Current == null 
                || CityManager.Current.Cities == null)
            {
                return;
            }

            foreach (var city in CityManager.Current.Cities.ToList())
            {
                if (city.Demands == null || city.Demands.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < city.Demands.Count; i++)
                {
                    var demand = city.Demands[i];
                    _oversupplyCounterField.SetValue(demand, _zeroCounter);
                    _invalidatePriceMethod.Invoke(demand, null);
                }
            }
        }
    }

    /// <summary>
    /// Overrides base class to essentially ignore all AddValue calls.
    /// </summary>
    public class ZeroTimeSpanCounterInt : TimeSpanCounterInt
    {
        protected override int AddValue(int a, int b)
        {
            return 0;
        }
    }
}
