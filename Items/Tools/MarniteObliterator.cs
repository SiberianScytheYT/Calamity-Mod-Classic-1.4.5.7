using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Tools
{
    public class MarniteObliterator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marnite Obliterator");
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 36;
            Item.height = 18;
            Item.useTime = 7;
            Item.useAnimation = 25;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.pick = 57;
            Item.axe = 10;
            Item.tileBoost++;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MarniteObliteratorProj>();
            Item.shootSpeed = 40f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyGoldBar", 3);
            recipe.AddIngredient(ItemID.Granite, 5);
            recipe.AddIngredient(ItemID.Marble, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
