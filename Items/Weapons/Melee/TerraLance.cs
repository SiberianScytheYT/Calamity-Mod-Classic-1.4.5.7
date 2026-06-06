using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TerraLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Lance");
/*
            Tooltip.SetDefault("Fires a lance beam");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.damage = 88;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 17;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 44;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<TerraLanceProjectile>();
            Item.shootSpeed = 11f;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChlorophytePartisan);
            recipe.AddIngredient(ItemID.DarkLance);
            recipe.AddIngredient(ItemID.Gungnir);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
