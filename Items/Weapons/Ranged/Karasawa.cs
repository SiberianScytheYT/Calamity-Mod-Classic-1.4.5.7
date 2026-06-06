using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Karasawa : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Karasawa");
/*
            Tooltip.SetDefault("...This is heavy...too heavy.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 44;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 1380;
            Item.knockBack = 12f;
            Item.useTime = 52;
            Item.useAnimation = 52;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/MechGaussRifle");
            Item.noMelee = true;

            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;

            Item.shoot = ModContent.ProjectileType<KarasawaShot>();
            Item.shootSpeed = 1f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool CanUseItem(Player player)
        {
            return CalamityGlobalItem.HasEnoughAmmo(player, Item, 5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            if (velocity1.Length() > 5f)
            {
                velocity1.Normalize();
                velocity1 *= 5f;
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity1.X, velocity1.Y, ModContent.ProjectileType<KarasawaShot>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);

            // Consume 5 ammo per shot
            CalamityGlobalItem.ConsumeAdditionalAmmo(player, Item, 5);

            return false;
        }

        // Disable vanilla ammo consumption
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<CrownJewel>());
            r.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            r.AddIngredient(ModContent.ItemType<BarofLife>(), 10);
            r.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 15);
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.Register();
        }
    }
}
