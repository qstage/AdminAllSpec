using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace AdminAllSpec;

public class Plugin : BasePlugin
{
    public override string ModuleName => "AdminAllSpec";
    public override string ModuleVersion => "1.0.2";
    public override string ModuleAuthor => "xstage";

    public readonly FakeConVar<string> FlagCvar = new("css_adminallspec_flag", "Admin flag", "@css/generic");

    private readonly bool[] _hasFlag = new bool[64];
    private static readonly MemoryFunctionWithReturn<CPlayer_ObserverServices, CCSPlayerPawn, bool> IsValidObserverTarget = new(GameData.GetSignature("IsValidObserverTarget"));

    public override void Load(bool hotReload)
    {
        IsValidObserverTarget.Hook(Hook_IsValidObserverTarget, HookMode.Pre);

        RegisterEventHandler<EventPlayerConnectFull>(OnPlayerConnectFull);

        FlagCvar.ValueChanged += (_, _) => OnPlayersLoad();

        if (hotReload)
            OnPlayersLoad();
    }

    public override void Unload(bool hotReload)
    {
        IsValidObserverTarget.Unhook(Hook_IsValidObserverTarget, HookMode.Pre);
    }

    public void OnPlayersLoad()
    {
        foreach (var player in Utilities.GetPlayers())
            OnPlayerConnectFull(player);
    }

    private HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        var player = @event.Userid;

        OnPlayerConnectFull(player);

        return HookResult.Continue;
    }

    private void OnPlayerConnectFull(CCSPlayerController? player)
    {
        if (player is not { IsValid: true, IsBot: false }) return;

        _hasFlag[player.Slot] = AdminManager.PlayerHasPermissions(player, FlagCvar.Value);
    }

    private HookResult Hook_IsValidObserverTarget(DynamicHook hook)
    {
        var observerPawn = hook.GetParam<CPlayer_ObserverServices>(0).Pawn.Value.As<CCSPlayerPawn>();
        if (observerPawn is not { IsValid: true }) return HookResult.Continue;

        var observerPlayer = observerPawn.OriginalController.Value;
        if (observerPlayer is not { IsValid: true }) return HookResult.Continue;

        var targetPawn = hook.GetParam<CCSPlayerPawn>(1);
        if (targetPawn is not { IsValid: true, LifeState: (byte)LifeState_t.LIFE_ALIVE }) return HookResult.Continue;

        var targetPlayer = targetPawn.OriginalController.Value;

        if (targetPlayer is not { Connected: PlayerConnectedState.PlayerConnected, IsValid: true, TeamNum: > 1 }) return HookResult.Continue;

        if (_hasFlag[observerPlayer.Slot])
        {
            hook.SetReturn(true);

            return HookResult.Stop;
        }

        return HookResult.Continue;
    }
}