using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class FlameScytheMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Subduction Slicer");
/*
            Tooltip.SetDefault("Throws a scythe that explodes on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.damage = 90;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.height = 48;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<FlameScytheProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 9);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
