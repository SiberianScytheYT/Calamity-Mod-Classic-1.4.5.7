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
    public class Ultima : ModItem
    {
        public const float FullChargeTime = 420f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ultima");
/*
            Tooltip.SetDefault("Casts a continuous stream of plasma bolts\n" +
                               "Over time the bolts are replaced with powerful lasers\n" +
                               "Bolts power up into solid beams as you continue shooting\n" +
                               "90% chance to not consume ammo");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 119;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 58;
            Item.useTime = Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = 10;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<UltimaBowProjectile>();
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Arrow;
            Item.channel = true;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).SafeNormalize(Vector2.UnitX * player.direction), ModContent.ProjectileType<UltimaBowProjectile>(), 0, 0f, player.whoAmI);
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(0, 100) >= 90;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PulseBow);
            recipe.AddIngredient(ItemID.LaserRifle);
            recipe.AddIngredient(ModContent.ItemType<TheStorm>());
            recipe.AddIngredient(ModContent.ItemType<AstralRepeater>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 15);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 15);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
