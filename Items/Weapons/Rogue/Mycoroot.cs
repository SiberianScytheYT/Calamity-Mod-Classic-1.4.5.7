using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Mycoroot : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mycoroot");
/*
            Tooltip.SetDefault("Fires a stream of short-range fungal roots\n" +
                "Stealth strikes spawn an explosion of fungi spores");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 32;
            Item.damage = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 32;
            Item.rare = 2;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.shoot = ModContent.ProjectileType<MycorootProj>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.NextFloat(-1.5f, 1.5f);
            float SpeedY = velocity.Y + Main.rand.NextFloat(-1.5f, 1.5f);
            int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            if (player.Calamity().StealthStrikeAvailable() && player.ownedProjectileCounts[ModContent.ProjectileType<ShroomerangSpore>()] < 20)
            {
				Main.projectile[stealth].Calamity().stealthStrike = true;
                for (float i = 0; i < Main.rand.Next(7,11); i++)
                {
					Vector2 velocity1 = CalamityUtils.RandomVelocity(50f, 10f, 50f, 0.01f);
                    int spore = Projectile.NewProjectile(source, player.Center, velocity1, ModContent.ProjectileType<ShroomerangSpore>(), (int)(damage * 0.5f), Item.knockBack, player.whoAmI);
					Main.projectile[spore].Calamity().lineColor = 1;
                }
            }
            return false;
        }
    }
}
