using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;

namespace CalRD.Items.Weapons.Rogue
{
	public class LuminousStriker : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Luminous Striker");
/*
			Tooltip.SetDefault("Send the stars back to where they belong\n"
							  +"Throws a stardust javelin trailed by rising stardust shards\n"
                              +"Explodes into additional stardust shards upon hitting enemies\n"
							  +"Stealth Strikes cause the stardust shards to fly alongside the javelin instead of rising");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 86;
			Item.height = 102;
            Item.damage = 149;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<LuminousStrikerProj>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<SpearofPaleolith>());
			recipe.AddIngredient(ModContent.ItemType<ScourgeoftheSeas>());
			recipe.AddIngredient(ModContent.ItemType<Turbulance>());
            recipe.AddIngredient(ModContent.ItemType<MeldiateBar>(), 10);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
