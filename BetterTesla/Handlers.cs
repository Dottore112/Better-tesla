﻿using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using Handlers = Exiled.Events.Handlers;
using Exiled.API.Enums;
using System.Linq;
using Exiled.API.Extensions;
using System.Collections.Generic;

namespace BetterTesla
{
    public class Handlers
    {
        public int Tesla079;
        public static bool ActivatedTeslas;
        public static HashSet<Team> DisabledTeslaTeam = new HashSet<Team>();

        public void Dying(DyingEventArgs ev)
        {
            if(ev.Handler.Type == DamageType.Tesla && Plugin.Singleton.Config.InventoryErase)
                ev.Target.ClearInventory();
        }

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            ev.IsTriggerable = ActivatedTeslas;

            if (DisabledTeslaTeam.Contains(ev.Player.Team))
            {
                ev.IsTriggerable = false;
                ev.IsInIdleRange = false;
            }

            if (Plugin.Singleton.Config.TeslaDisableAtNoScp && Player.Get(Team.SCP).Count() == 0)
            {
                ev.IsTriggerable = false;
                ev.IsInIdleRange = false;
            }
            if (ev.Player.CurrentItem != null)
            {
                if (ev.Player.CurrentItem.Type == Plugin.Singleton.Config.BypassTeslaItem && Plugin.Singleton.Config.IfBypassTeslaItem)
                {
                    ev.IsTriggerable = false;
                    ev.IsInIdleRange = false;
                }
            }
        }
        public void PickItem(PickingUpItemEventArgs ev) 
        {
            if(ev.Pickup.Type == Plugin.Singleton.Config.BypassTeslaItem)
                ev.Player.Broadcast(Plugin.Singleton.Config.PickItemBC);
        }

        public void Interact079Tesla(InteractingTeslaEventArgs ev)
        {
            ev.AuxiliaryPowerCost = Plugin.Singleton.Config.TeslaCost079;
            if (Plugin.Singleton.Config.IfTesla079Limit)
            {
                if (Tesla079 >= Plugin.Singleton.Config.Tesla079Limit) {
                    ev.IsAllowed = false;
                }
                else if (Tesla079 < Plugin.Singleton.Config.Tesla079Limit)
                {
                    ev.IsAllowed = true;
                    Tesla079++;
                }
            }
        }
    }
}