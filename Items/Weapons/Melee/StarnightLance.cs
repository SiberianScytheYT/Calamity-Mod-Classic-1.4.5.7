using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class StarnightLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Starnight Lance");
/*
            Tooltip.SetDefault("Shoots a starnight beam");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 72;
            Item.damage = 95;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 23;
            Item.knockBack = 6;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<StarnightLanceProjectile>();
            Item.shootSpeed = 6f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
