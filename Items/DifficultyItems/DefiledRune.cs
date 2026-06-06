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
    public class DefiledRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Defiled Rune");
/*
            Tooltip.SetDefault("Wing flight is disabled and enemies can critically hit you\n" +
                "Increases most rare item drop chances and enemies drop 50% more cash\n" +
                "Can only be used in revengeance and death mode\n" +
                "Can be toggled on and off");
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
            Item.UseSound = SoundID.Item100;
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
                Color messageColor = Color.DarkSeaGreen;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
				return true;
			}
            if (!CalamityWorld.defiled)
            {
                CalamityWorld.defiled = true;
                string key = "Your soul is mine...";
                Color messageColor = Color.DarkSeaGreen;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }
            else
            {
                CalamityWorld.defiled = false;
                string key = "Your soul is yours once more...";
                Color messageColor = Color.DarkSeaGreen;
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
