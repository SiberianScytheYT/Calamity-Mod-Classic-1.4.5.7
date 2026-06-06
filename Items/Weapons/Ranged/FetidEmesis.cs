using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class FetidEmesis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fetid Emesis");
/*
            Tooltip.SetDefault("Has a chance to release rotten chunks instead of bullets.");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.height = 46;
            Item.useTime = Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.Calamity().postMoonLordRarity = 13;
			Item.rare = 10;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(8))
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y) * 0.45f,
                    ModContent.ProjectileType<EmesisGore>(), damage, Item.knockBack, player.whoAmI);
                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(position, 10, 10, 27);
                    dust.velocity = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)).RotatedByRandom(MathHelper.ToRadians(15f));
                    dust.noGravity = true;
                }
				if (player.Calamity().soundCooldown <= 0)
				{
					// WoF vomit sound.
					SoundEngine.PlaySound(SoundID.NPCDeath13.WithVolumeScale(0.5f), position);
					player.Calamity().soundCooldown = 120;
				}
                return false;
            }
            return true;
        }
    }
}
