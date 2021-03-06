using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using AoLib.Utils;

namespace VhaBot.Plugins
{
    public class VhLag : PluginBase
    {
        public VhLag()
        {
            this.Name = "Client Lag Tweaks";
            this.InternalName = "VhLag";
            this.Author = "Jurosik (RK1) - Modded by Naturalistic";
            this.Version = 100;
            this.Description = "Various command shortcuts to improve client lag and display.";
            this.DefaultState = PluginState.Installed;
            this.Commands = new Command[] {
            new Command("lag", true, UserLevel.Guest, UserLevel.Member, UserLevel.Guest)
            };
        }

        public override void OnLoad(BotShell bot) { }

        public override void OnUnload(BotShell bot) { }

        public override void OnCommand(BotShell bot, CommandArgs e)
        {
            RichTextWindow window = new RichTextWindow(bot);
            window.AppendTitle("Clien Lag Tweak");
            window.AppendLineBreak();
            window.AppendHighlight("Environment and Visual (No Zoning Required)");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option ShowAllNames 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option ShowAllNames 0");
            window.AppendNormal (" Character names show over head");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option SimpleClouds 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option SimpleClouds 0");
            window.AppendNormal (" Display Simple Clouds ");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option RealisticClouds 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option RealisticClouds 0");
            window.AppendNormal (" Display Realistic Clouds ");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option RealisticMoons 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option RealisticMoons 0");
            window.AppendNormal (" Display Realistic Moons");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option StarRotation 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option StarRotation 0");
            window.AppendNormal (" Star rotation at night");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option Wildlife 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option Wildlife 0");
            window.AppendNormal (" Show wildlife");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option IsSpaceShipsShown 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option IsSpaceShipsShown 0");
            window.AppendNormal (" Show Sace Ships");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option Shadows 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option Shadows 0");
            window.AppendNormal (" Show Ground Shadows");
            window.AppendLineBreak(2);
            window.AppendHighlight("Environment and Visual (Zoning Required)");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option BuffsFX 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option BuffsFX 0");
            window.AppendNormal (" Show Buff Effects");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option EnvironmentFX 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option EnvironmentFX 0");
            window.AppendNormal (" Show Environment Effects");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option MuzzleFlashFX 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option MuzzleFlashFX 0");
            window.AppendNormal (" Show Muzzle Flash");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option NanoEffectFX 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option NanoEffectFX 0");
            window.AppendNormal (" Show Nano Effects (Calms, Damage Shields, etc.)");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option TracersFX 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option TracersFX 0");
            window.AppendNormal (" Show Tracers");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option OthersFX 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option OthersFX 0");
            window.AppendNormal (" Show Others Effects");
            window.AppendLineBreak(2);
            window.AppendHighlight("Audio Settings (No Zoning Required)");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option SoundOnOff 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option SoundOnOff 0");
            window.AppendNormal (" Sound Master ");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option SoundFXOn 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option SoundFXOn 0");
            window.AppendNormal (" Sound Effects");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option MusicOn 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option MusicOn 0");
            window.AppendNormal (" Music");
            window.AppendLineBreak();
            window.AppendCommand ("Enable", "/option VoiceSndFxOn 1");
            window.AppendNormal ("  ");
            window.AppendCommand ("Disable", "/option VoiceSndFxOn 0");
            window.AppendNormal (" Voices");
            window.AppendLineBreak(2);
            window.AppendHighlight("View Distance Settings (No Zoning Required)");
            window.AppendLineBreak();
            window.AppendCommand ("Min", "/option ViewDistance 0.05");
            window.AppendNormal ("  ");
            window.AppendCommand ("Low", "/option ViewDistance 0.30");
            window.AppendNormal ("  ");
            window.AppendCommand ("Medium", "/option ViewDistance 0.50");
            window.AppendNormal ("  ");
            window.AppendCommand ("High", "/option ViewDistance 0.75");
            window.AppendNormal ("  ");
            window.AppendCommand ("Max", "/option ViewDistance 1");
            window.AppendNormal (" View Distance");
            window.AppendLineBreak();
            window.AppendCommand ("Min", "/CharDist 5");
            window.AppendNormal ("  ");
            window.AppendCommand ("Low", "/CharDist 30");
            window.AppendNormal ("  ");
            window.AppendCommand ("Medium", "/CharDist 50");
            window.AppendNormal ("  ");
            window.AppendCommand ("High", "/CharDist 75");
            window.AppendNormal ("  ");
            window.AppendCommand ("Max", "/CharDist 100");
            window.AppendNormal (" Character View Distance");
            window.AppendLineBreak(2);
            bot.SendReply(e, " Lag Tweak �� ", window);
        }

        public override string OnHelp(BotShell bot, string command)
        {
            switch (command)
            {
                case "lag":
                    return "Client Lag Tweaks\n" +
                        "Usage: /tell " + bot.Character + " lag";

            }
            return null;
        }
    }
}
