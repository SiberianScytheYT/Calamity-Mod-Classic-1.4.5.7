using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Spears;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Nadir : ModItem
    {
        public static int BaseDamage = 700;
        public static float ShootSpeed = 12f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nadir");
/*
            Tooltip.SetDefault("Fires void essences which flay nearby enemies with tentacles\n" + "Ignores immunity frames\n" +
                "'The abyss has stared back at you long enough. It now speaks, and it does not speak softly.'");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 144;
            Item.height = 144;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = BaseDamage;
            Item.knockBack = 8f;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(1, 80, 0, 0);

            Item.shoot = ModContent.ProjectileType<NadirSpear>();
            Item.shootSpeed = ShootSpeed;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SpatialLance>());
            recipe.AddIngredient(ModContent.ItemType<TwistingNether>(), 5);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 15);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
