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
    public class Armageddon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Armageddon");
/*
            Tooltip.SetDefault("Makes any hit while a boss is alive instantly kill you\n" +
                "Effect can be toggled on and off\n" +
                "If a boss is defeated with this effect active it will drop 6 treasure bags, 5 in normal mode");
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
            Item.UseSound = SoundID.Item123;
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
                Color messageColor = Color.Fuchsia;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
				return true;
			}
            CalamityWorld.armageddon = !CalamityWorld.armageddon;
            CalamityWorld.DoGSecondStageCountdown = 0;

            string key2 = CalamityWorld.armageddon ? "Bosses will now kill you instantly." : "Bosses will no longer kill you instantly.";
            Color messageColor2 = Color.Fuchsia;
            CalamityUtils.DisplayLocalizedText(key2, messageColor2);

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
