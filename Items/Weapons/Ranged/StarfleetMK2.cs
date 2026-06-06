using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class StarfleetMK2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Starmada");
/*
            Tooltip.SetDefault("Fires a barrage of stars and plasma blasts");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 222;
            Item.knockBack = 15f;
            Item.shootSpeed = 16f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 27;
            Item.useTime = 27;
            Item.reuseDelay = 0;
            Item.width = 122;
            Item.height = 50;
            Item.UseSound = SoundID.Item92;
            Item.shoot = ModContent.ProjectileType<StarfleetMK2Gun>();
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useTurn = false;
            Item.useAmmo = AmmoID.FallenStar;
            Item.autoReuse = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<StarfleetMK2Gun>(), 0, 0f, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Starfleet>());
            recipe.AddIngredient(ModContent.ItemType<StarSputter>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 15);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
