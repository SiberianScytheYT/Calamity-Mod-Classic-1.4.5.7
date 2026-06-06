using CalRD.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Materials;
namespace CalRD.Items.Fishing.FishingRods
{
	public class HeronRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heron Rod");
/*
            Tooltip.SetDefault("Increased fishing power in space.\n" + //John Steinbeck quote but fish instead of snake
				"A silent head and beak lanced down and plucked it out by the head,\n" +
				"and the beak swallowed the little fish while its tail waved frantically.");
*/
        }

        public override void SetDefaults()
        {
			Item.width = 24;
			Item.height = 28;
			Item.useAnimation = 8;
			Item.useTime = 8;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.fishingPole = 25;
			Item.shootSpeed = 14.5f;
			Item.shoot = ModContent.ProjectileType<HeronBobber>();
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 7);
            recipe.AddIngredient(ItemID.SunplateBlock, 5);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
