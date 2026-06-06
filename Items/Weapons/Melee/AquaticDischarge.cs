using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class AquaticDischarge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Discharge");
/*
            Tooltip.SetDefault("Enemies release electric sparks on death");
*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTurn = false;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.width = 32;
            Item.height = 32;
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
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
            if (target.life <= 0)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<Spark>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, Main.myPlayer);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.statLife <= 0)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<Spark>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, Main.myPlayer);
            }
        }
    }
}
