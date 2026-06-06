using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GoldplumeSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Goldplume Spear");
/*
            Tooltip.SetDefault("Shoots falling feathers");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 23;
            Item.knockBack = 5.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<GoldplumeSpearProjectile>();
            Item.shootSpeed = 5f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 10);
            recipe.AddIngredient(ItemID.SunplateBlock, 4);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
