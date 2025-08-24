using System.Collections.Generic;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using PlayerRoles.Voice;
using VoiceChat;

namespace MultiIntercom;

public static class EventHandlers
{
    public static void RegisterEvents()
    {
        PlayerEvents.UsingIntercom += OnUsingIntercom;
    }

    public static void UnregisterEvents()
    {
        PlayerEvents.UsingIntercom -= OnUsingIntercom;
    }

    private static void OnUsingIntercom(PlayerUsingIntercomEventArgs ev)
    {
        Timing.RunCoroutine(IntercomLogicLoop());
        
        
        if (!Room.Get(RoomName.EzIntercom).First().Players.ToList().Contains(ev.Player)) return;

        Intercom.TrySetOverride(ev.Player.ReferenceHub, true);
    }
    
    private static IEnumerator<float> IntercomLogicLoop()
    {
        while (Intercom.State == IntercomState.InUse)
        {
            foreach (Player player in Player.List)
            {
                if (Room.Get(RoomName.EzIntercom).First().Players.ToList().Contains(player))
                {
                    Intercom.TrySetOverride(player.ReferenceHub, true);
                }
                else
                {
                    Intercom.TrySetOverride(player.ReferenceHub, false);
                }
            }
            yield return Timing.WaitForSeconds(1f);
        }
        foreach (Player player in Player.List)
        {
            Intercom.TrySetOverride(player.ReferenceHub, false);
        }
    }
}