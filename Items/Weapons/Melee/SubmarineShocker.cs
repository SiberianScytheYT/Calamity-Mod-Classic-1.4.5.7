using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SubmarineShocker : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Submarine Shocker");
/*
            Tooltip.SetDefault("Enemies release electric sparks on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTurn = false;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.width = 32;
            Item.height = 32;
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 226);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<Spark>(), (int)(Item.damage * player.MeleeDamage() * 0.7f), Item.knockBack, Main.myPlayer);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<Spark>(), (int)(Item.damage * player.MeleeDamage() * 0.7f), Item.knockBack, Main.myPlayer);
        }
    }
}
