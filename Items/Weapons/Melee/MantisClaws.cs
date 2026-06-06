using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class MantisClaws : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mantis Claws");
/*
            Tooltip.SetDefault("Explodes on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 8;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 20;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 33);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			//does no damage. Explosion is visual
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), 0, 0f, player.whoAmI, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
			//does no damage. Explosion is visual
            int boom = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), 0, 0f, player.whoAmI, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
        }
    }
}
