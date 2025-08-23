using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles.Voice;
using VoiceChat;

namespace MultiIntercom;

public static class EventHandlers
{
    public static void RegisterEvents()
    {
        PlayerEvents.SendingVoiceMessage += OnSendingVoiceMessage;
    }

    public static void UnregisterEvents()
    {
        PlayerEvents.SendingVoiceMessage -= OnSendingVoiceMessage;
    }

    private static void OnSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev)
    {
        if (!ev.Player.IsAlive || ev.Player.IsDummy || ev.Player.IsHost || ev.Player.IsSCP) return;
        if (Intercom.State != IntercomState.InUse) return;
        if (!Room.Get(RoomName.EzIntercom).First().Players.ToList().Contains(ev.Player)) return;

        ev.Message = ev.Message with { Channel = VoiceChatChannel.Intercom };
    }
}