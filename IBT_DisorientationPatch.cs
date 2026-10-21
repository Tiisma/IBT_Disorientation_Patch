using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace IBTDisorientationCompat;

public class IBTDisorientationCompatModSystem : ModSystem
{
    private const string HarmonyId = "ibt.disorientation.compat";
    private const string LogPrefix = "[IBT Disorientation Compat]";

    private Harmony? harmony;

    public override void StartClientSide(ICoreClientAPI api)
    {
        var rendererType = AccessTools.TypeByName(
            "VS_ImmersiveBodyTemperature.GUI.PlayerStatRenderer"
        );

        if (rendererType == null)
        {
            api.Logger.Warning(
                $"{LogPrefix} PlayerStatRenderer was not found. " +
                "Immersive Body Temperature may not be installed."
            );
            return;
        }


        var original = AccessTools.Method(
            rendererType,
            "UpdateFeltTemperature"
        );

        if (original == null)
        {
            api.Logger.Warning(
                $"{LogPrefix} Could not find UpdateFeltTemperature."
            );
            return;
        }

        harmony = new Harmony(HarmonyId);
        harmony.Patch(
            original,
            prefix: new HarmonyMethod(
                typeof(IBTDisorientationCompatModSystem),
                nameof(SkipFeltTemperatureUpdate)
            )
        );

        api.Logger.Notification(
            $"{LogPrefix} Compatibility patches applied."
        );
    }

    private static bool SkipFeltTemperatureUpdate() => false;

    public override void Dispose()
    {
        harmony?.UnpatchAll(HarmonyId);
        base.Dispose();
    }
}