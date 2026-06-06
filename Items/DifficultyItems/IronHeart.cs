using CalRD.CalPlayer;
using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Items.DifficultyItems
{
	public class IronHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Iron Heart");
/*
            Tooltip.SetDefault("Healing with potions and all positive life regen is disabled.\n" +
				"Enemy damage scales with your max health.\n" +
                "Can be toggled on and off.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.expert = true;
            Item.rare = 9;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item119;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            // This world syncing code should only be run by one entity- the server, to prevent a race condition
            // with the packets.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            if (CalamityPlayer.areThereAnyDamnBosses || CalamityWorld.DoGSecondStageCountdown > 0 || BossRushEvent.BossRushActive)
			{
                string key = "You cannot change the rules now.";
                Color messageColor = Color.LightSkyBlue;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
				return true;
			}
			if (!CalamityWorld.ironHeart)
            {
                CalamityWorld.ironHeart = true;
                string key = "Iron Heart is active, healing is disabled.";
                Color messageColor = Color.LightSkyBlue;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }
            else
            {
                CalamityWorld.ironHeart = false;
                string key = "Iron Heart is not active, healing is restored.";
                Color messageColor = Color.LightSkyBlue;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }
            CalamityWorld.DoGSecondStageCountdown = 0;
            CalamityNetcode.SyncWorld();

            return true;
        }
    }
}
