using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class ThePack : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Pack");
/*
            Tooltip.SetDefault("Fires large homing rockets that explode into more homing mini rockets when in proximity to an enemy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 2500;
            Item.crit += 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 96;
            Item.height = 40;
            Item.useTime = 52;
            Item.useAnimation = 52;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<ThePackMissile>();
            Item.useAmmo = AmmoID.Rocket;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-40, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ThePackMissile>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Scorpion>());
            recipe.AddIngredient(ItemID.MarbleBlock, 50);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
