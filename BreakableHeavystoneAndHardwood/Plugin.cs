using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using BepInEx.Configuration;
using HarmonyLib;
using Wish;

namespace BreakableHeavystoneAndHardwood;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private Harmony m_harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
    public static ManualLogSource logger;

	private static ConfigEntry<float> m_required_power;

    private void Awake()
    {
        // Plugin startup logic
        Plugin.logger = this.Logger;
        logger.LogInfo((object) $"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
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