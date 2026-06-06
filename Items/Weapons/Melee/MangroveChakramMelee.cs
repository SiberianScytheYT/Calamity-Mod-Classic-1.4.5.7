using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class MangroveChakramMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mangrove Chakram");
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.damage = 84;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.height = 38;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<MangroveChakramProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
