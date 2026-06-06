using CalRD.Projectiles.Magic;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class WintersFury : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Winter's Fury");
/*
            Tooltip.SetDefault("The pages are freezing to the touch");
*/
        }
        public override void SetDefaults()
        {
            Item.damage = 59;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item9;
            Item.scale = 0.9f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Icicle>();
            Item.shootSpeed = 15f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.Next(4) != 0)
            {
                Vector2 speed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(Main.rand.Next(-15, 16)));
                speed.Normalize();
                speed *= 15f;
                speed.Y -= Math.Abs(speed.X) * 0.2f;
                int p = Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<FrostShardFriendly>(), damage, Item.knockBack, player.whoAmI);
                Main.projectile[p].Calamity().forceMagic = true;
            }
            if (Main.rand.NextBool(4))
            {
                SoundEngine.PlaySound(SoundID.Item1, position);
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.2f, velocity.Y * 1.2f, ModContent.ProjectileType<Snowball>(), damage, Item.knockBack * 2f, player.whoAmI);
            }
            velocity.X += Main.rand.Next(-40, 41) * 0.05f;
            velocity.Y += Main.rand.Next(-40, 41) * 0.05f;
            return true;
        }
    }
}
