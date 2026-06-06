using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class FlakKraken : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flak Kraken");
/*
            Tooltip.SetDefault("Fires an energy reticle that becomes more powerful over time");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 54;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 152;
            Item.height = 58;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.reuseDelay = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.knockBack = 0f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserCannon");
            Item.shoot = ModContent.ProjectileType<FlakKrakenGun>();
            Item.shootSpeed = 30f; //30
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FlakKrakenGun>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FlakToxicannon>());
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
