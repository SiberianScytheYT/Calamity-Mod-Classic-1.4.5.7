using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class CrushsawCrasher : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crushsaw Crasher");
/*
            Tooltip.SetDefault("Throws bouncing axes\n" +
			"Stealth strikes throw five at once");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 38;
            Item.damage = 65;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<Crushax>();
            Item.shootSpeed = 11f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int spread = 3;
                for (int i = 0; i < 5; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-3,4), velocity.Y + Main.rand.Next(-3,4)).RotatedBy(MathHelper.ToRadians(spread));
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, perturbedspeed.X, perturbedspeed.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[proj].Calamity().stealthStrike = true;
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = 10;
                    spread -= Main.rand.Next(1,4);
                }
                return false;
            }
            return true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }
    }
}
