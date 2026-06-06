using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class MarniteSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marnite Spear");
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 21;
            Item.knockBack = 5.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 50;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<MarniteSpearProjectile>();
            Item.shootSpeed = 5f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyGoldBar", 5);
            recipe.AddIngredient(ItemID.Granite, 9);
            recipe.AddIngredient(ItemID.Marble, 9);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
