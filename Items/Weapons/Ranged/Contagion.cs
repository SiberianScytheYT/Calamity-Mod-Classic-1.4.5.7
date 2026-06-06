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
    public class Contagion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Contagion");
/*
            Tooltip.SetDefault("Fires contagion arrows that leave exploding orbs behind as they travel");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 4000;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ContagionBow>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Phangasm>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ContagionBow>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }
}
