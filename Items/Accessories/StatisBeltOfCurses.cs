using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class StatisBeltOfCurses : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statis' Void Sash");
/*
            Tooltip.SetDefault("24% increased jump speed and allows constant jumping\n" +
				"Increases fall damage resistance by 50 blocks\n" +
                "Can climb walls, dash, and dodge attacks\n" +
                "Dashes leave homing scythes in your wake");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 3));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().postMoonLordRarity = 14;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.autoJump = true;
            player.jumpSpeedBoost += 1.2f;
            player.extraFall += 50;
            player.blackBelt = true;
			modPlayer.dashMod = 7;
            player.spikedBoots = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StatisNinjaBelt>());
            recipe.AddIngredient(ModContent.ItemType<TwistingNether>(), 10);
			//This is not a mistake.  Only Nightmare Fuel is intentional for thematics.
            recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 10);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
