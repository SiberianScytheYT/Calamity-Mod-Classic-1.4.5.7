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
    public class Death : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Death");
/*
            Tooltip.SetDefault("Makes bosses even more EXTREME.\n" +
                "Allows certain bosses to spawn naturally.\n" +
				"Certain biomes and events have additional weather effects.\n" +
				"Lethal lava effects are always enabled.\n" +
                "Increases enemy damage by 15%.\n" +
                "Greatly boosts enemy spawn rates during the blood moon.\n" +
                "Nerfs the effectiveness of life steal.\n" +
                "Makes the abyss more treacherous to navigate.\n" +
                "Nurse no longer heals while a boss is alive.\n" +
                "Increases damage done by several debuffs.\n" +
                "Effect can be toggled on and off.\n" +
                "Effect will only work if Revengeance Mode is active.");
*/
        }

        public override void SetDefaults()
        {
            Item.rare = 11;
            Item.width = 28;
            Item.height = 28;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item119;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player) => CalamityWorld.revenge;

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
            if (!CalamityWorld.death)
            {
                CalamityWorld.death = true;
                string key = "Death is active, enjoy the fun.";
                Color messageColor = Color.Crimson;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }
            else
            {
                CalamityWorld.death = false;
                string key = "Death is not active, not fun enough for you?";
                Color messageColor = Color.Crimson;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }
            CalamityWorld.DoGSecondStageCountdown = 0;
            CalamityNetcode.SyncWorld();

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
