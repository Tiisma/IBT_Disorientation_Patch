using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Client;

namespace IBTDisorientationCompat;

public class IBTDisorientationCompatModSystem : ModSystem
{
    private static readonly string[] MethodsToPatch =
    {
        "Compose",
        "UpdateFeltTemperature"
    };

    private Harmony? harmony;

    public override void StartClientSide(ICoreClientAPI api)
    {
        harmony = new Harmony("ibt.disorientation.compat");

        Type? rendererType = AccessTools.TypeByName(
            "VS_ImmersiveBodyTemperature.GUI.PlayerStatRenderer"
        );

        if (rendererType == null)
        {
            api.Logger.Warning(
                "[IBT Disorientation Compat] PlayerStatRenderer was not found. " +
                "Immersive Body Temperature may not be installed."
            );
            return;
        }

        MethodInfo prefix = AccessTools.Method(
            typeof(IBTDisorientationCompatModSystem),
            nameof(SkipIBTPlayerStatsGUI)
        );

        foreach (string methodName in MethodsToPatch)
        {
            MethodInfo? original = AccessTools.Method(rendererType, methodName);

            if (original == null)
            {
                api.Logger.Warning(
                    $"[IBT Disorientation Compat] Could not find {methodName}."
                );
                continue;
            }

            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }

        api.Logger.Notification(
            "[IBT Disorientation Compat] Compatibility patches applied."
        );
    }

    private static bool SkipIBTPlayerStatsGUI()
    {
        return false;
    }

    public override void Dispose()
    {
        harmony?.UnpatchAll("ibt.disorientation.compat");
        base.Dispose();
    }
}