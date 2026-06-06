using CalRD.Projectiles.Ranged;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class CorrodedCaustibow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Corroded Caustibow");
/*
            Tooltip.SetDefault("Shoots slow, powerful shells that trail an irradiated aura");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 38;
            Item.crit += 20;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Shell>();
            Item.shootSpeed = 5f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<CorrodedShell>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<Shellshooter>());
            r.AddIngredient(ModContent.ItemType<Toxibow>());
            r.AddIngredient(ModContent.ItemType<CorrodedFossil>(), 10);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}
