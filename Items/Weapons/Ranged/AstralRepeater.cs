using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class AstralRepeater : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Bow");
/*
            Tooltip.SetDefault("Fires a burst of three arrows");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.crit += 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 78;
            Item.useTime = 5;
            Item.reuseDelay = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 7);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
