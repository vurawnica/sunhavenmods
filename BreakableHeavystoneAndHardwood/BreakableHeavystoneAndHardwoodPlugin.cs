using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using Wish;
using UnityEngine;

namespace BreakableHeavystoneAndHardwood;

[BepInPlugin(pluginGuid, pluginName, pluginVersion)]
public class BreakableHeavystoneAndHardwoodPlugin : BaseUnityPlugin
{
    private const string pluginGuid = "vurawnica.sunhaven.breakableheavystoneandhardwood";
    private const string pluginName = "BreakableHeavystoneAndHardwood";
    private const string pluginVersion = "0.0.2";
    private Harmony m_harmony = new Harmony(pluginGuid);
    public static ManualLogSource logger;

	private static ConfigEntry<float> m_required_power;

    private void Awake()
    {
        // Plugin startup logic
        BreakableHeavystoneAndHardwoodPlugin.logger = this.Logger;
        logger.LogInfo((object) $"Plugin {pluginName} is loaded!");
        m_required_power = this.Config.Bind<float>("General", "Required Tool Level for Heavystone/Hardwood", 0, "3 is the vanilla value indicating adamant, 0 is the default for this mod");
		this.m_harmony.PatchAll();
	}

    [HarmonyPatch(typeof(Rock), "Hit")]
    class HarmonyPatch_Rock_Hit
    {
        private static void Prefix(ref float ____requiredPower)
        {
            if(____requiredPower != 0)
            {
                ____requiredPower = m_required_power.Value;
            }
        }
    }

    [HarmonyPatch(typeof(Wood), "Hit")]
    class HarmonyPatch_Wood_Hit
    {
        private static void Prefix(ref float ____requiredPower)
        {
            if(____requiredPower != 0)
            {
                ____requiredPower = m_required_power.Value;
            }
        }
    }
}