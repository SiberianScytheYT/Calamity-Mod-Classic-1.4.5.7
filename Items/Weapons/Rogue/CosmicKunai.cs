using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class CosmicKunai : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmic Kunai");
/*
            Tooltip.SetDefault("Fires a stream of short-range kunai\n" +
                "Stealth strikes spawn 5 Cosmic Scythes which home and explode");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 26;
            Item.damage = 150;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 2;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.height = 48;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<CosmicKunaiProj>();
            Item.shootSpeed = 28f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            if (player.Calamity().StealthStrikeAvailable() && player.ownedProjectileCounts[ModContent.ProjectileType<CosmicScythe>()] < 10)
            {
				Main.projectile[stealth].Calamity().stealthStrike = true;
                SoundEngine.PlaySound(SoundID.Item73, player.position);
                for (float i = 0; i < 5; i++)
                {
                    float angle = MathHelper.TwoPi / 5f * i;
                    Projectile.NewProjectile(source, player.Center, angle.ToRotationVector2() * 8f, ModContent.ProjectileType<CosmicScythe>(), (int)(damage * 0.8f), Item.knockBack, player.whoAmI, angle, 0f);
                }
            }
            return false;
        }
    }
}
