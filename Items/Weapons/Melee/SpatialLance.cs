using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SpatialLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Lance");
/*
            Tooltip.SetDefault("Rend the cosmos asunder!\n" +
                "Fires a lance beam that splits multiple times as it travels");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 88;
            Item.damage = 210;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.knockBack = 9.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 88;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<SpatialLanceProjectile>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TerraLance>());
            recipe.AddIngredient(ItemID.NorthPole);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
