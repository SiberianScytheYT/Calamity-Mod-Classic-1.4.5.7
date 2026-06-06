using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Carnage : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Carnage");
/*
            Tooltip.SetDefault("Enemies explode into homing blood on death");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 21;
            Item.knockBack = 5.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.height = 60;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item74, target.position);
                target.position.X += (float)(target.width / 2);
                target.position.Y += (float)(target.height / 2);
                target.position.X -= (float)(target.width / 2);
                target.position.Y -= (float)(target.height / 2);
                for (int num621 = 0; num621 < 15; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 25; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 5, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
                int num251 = Main.rand.Next(4, 6);
                for (int num252 = 0; num252 < num251; num252++)
                {
                    Vector2 value15 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    while (value15.X == 0f && value15.Y == 0f)
                    {
                        value15 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    }
                    value15.Normalize();
                    value15 *= (float)Main.rand.Next(70, 101) * 0.1f;
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, value15.X, value15.Y, ModContent.ProjectileType<Blood>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI, 0f, 0f);
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.statLife <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item74, target.position);
                target.position.X += (float)(target.width / 2);
                target.position.Y += (float)(target.height / 2);
                target.position.X -= (float)(target.width / 2);
                target.position.Y -= (float)(target.height / 2);
                for (int num621 = 0; num621 < 15; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 25; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 5, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
                int num251 = Main.rand.Next(4, 6);
                for (int num252 = 0; num252 < num251; num252++)
                {
                    Vector2 value15 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    while (value15.X == 0f && value15.Y == 0f)
                    {
                        value15 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    }
                    value15.Normalize();
                    value15 *= (float)Main.rand.Next(70, 101) * 0.1f;
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, value15.X, value15.Y, ModContent.ProjectileType<Blood>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI, 0f, 0f);
                }
            }
        }
    }
}
