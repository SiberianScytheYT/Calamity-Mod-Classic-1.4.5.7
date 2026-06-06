using CalRD.CalPlayer;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalRD.Items.DifficultyItems
{
    public class Revenge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Revengeance");
/*
            Tooltip.SetDefault("Enables/disables Revengeance Mode, can only be used in expert mode.\n" +
                "RAGE TOOLTIP LINE HERE\n" +
                "You gain rage whenever you take damage or hit an enemy with a true melee weapon.\n" +
                "ADRENALINE TOOLTIP LINE HERE\n" +
                "You gain adrenaline whenever a boss is alive. Getting hit drops adrenaline back to 0.\n" +
                "All enemies drop 50% more cash and spawn 15% more frequently\n" +
                "Certain enemies and projectiles deal between 5% and 25% more damage.\n" +
                "Makes certain enemies immune to life steal and nerfs the effectiveness of life steal.\n" +
                "Nerfs the effectiveness of the Titanium Armor set bonus.\n" +
                "Makes life regen scale with your current HP, the higher your HP the lower your life regen (this is not based on max HP).\n" +
                "Asphalt run speed is reduced by 33%, and the Nurse's healing cost is increased\n" +
                "Before you have killed your first boss you take 20% less damage from everything.\n" +
                "Changes ALL boss AIs and some enemy AIs in vanilla and the Calamity Mod.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = 11;
            Item.UseSound = SoundID.Item119;
            Item.consumable = false;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string rageKey = CalRD.RageHotKey.TooltipHotkeyString();
            string adrenKey = CalRD.AdrenalineHotKey.TooltipHotkeyString();
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                {
                    line2.Text = "Activates rage. When rage is maxed press " + rageKey + " to activate rage mode.";
                }
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip3")
                {
                    line2.Text = "Activates adrenaline. When adrenaline is maxed press " + adrenKey + " to activate adrenaline mode.";
                }
            }
        }

        public override bool CanUseItem(Player player) => Main.expertMode;

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            // This world syncing code should only be run by one entity- the server, to prevent a race condition
            // with the packets.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            if (CalamityPlayer.areThereAnyDamnBosses || CalamityWorld.DoGSecondStageCountdown > 0 || BossRushEvent.BossRushActive)
			{
                string key = "You cannot change the rules now.";
                Color messageColor = Color.Crimson;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
				return true;
			}
            if (!CalamityWorld.revenge)
            {
                CalamityWorld.revenge = true;
                string key = "Revengeance is active.";
                Color messageColor = Color.Crimson;
                CalamityUtils.DisplayLocalizedText(key, messageColor);

                CalamityNetcode.SyncWorld();
            }
            else
            {
                CalamityWorld.revenge = false;
                string key = "Revengeance is not active.";
                Color messageColor = Color.Crimson;
                CalamityUtils.DisplayLocalizedText(key, messageColor);

                if (CalamityWorld.death)
                {
                    CalamityWorld.death = false;
                    key = "Death is not active, not fun enough for you?";
                    messageColor = Color.Crimson;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
                if (CalamityWorld.defiled)
                {
                    CalamityWorld.defiled = false;
                    key = "Your soul is yours once more...";
                    messageColor = Color.DarkSeaGreen;
                    CalamityUtils.DisplayLocalizedText(key, messageColor);
                }
                CalamityWorld.DoGSecondStageCountdown = 0;
                CalamityNetcode.SyncWorld();
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
