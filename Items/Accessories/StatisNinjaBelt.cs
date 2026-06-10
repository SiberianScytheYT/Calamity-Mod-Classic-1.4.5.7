using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class StatisNinjaBelt : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statis' Ninja Belt");
/*
            Tooltip.SetDefault("8% increased jump speed and allows constant jumping\n" +
				"Increased fall damage resistance by 35 blocks\n" +
                "Can climb walls, dash, and dodge attacks");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.autoJump = true;
            player.jumpSpeedBoost += 0.4f;
            player.extraFall += 35;
            player.blackBelt = true;
			player.dash = 1;
            player.dashType = 1;
            player.spikedBoots = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FrogLeg);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 50);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>());
            recipe.AddIngredient(ItemID.MasterNinjaGear);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
