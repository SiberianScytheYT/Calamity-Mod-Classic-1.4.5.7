using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SeashellBoomerangMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seashell Boomerang");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.damage = 15;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.height = 34;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<SeashellBoomerangProjectile>();
            Item.shootSpeed = 11.5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
