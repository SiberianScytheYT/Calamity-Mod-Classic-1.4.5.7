using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class OldLordOathsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Old Lord Oathsword");
/*
            Tooltip.SetDefault("A relic of the ancient underworld\n" +
                "Critical hits cause lava explosions");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.width = 70;
            Item.height = 70;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                target.AddBuff(BuffID.OnFire, 600, false);
                player.ApplyDamageToNPC(target, (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), 0f, 0, false);
                float num50 = 1.7f;
                float num51 = 0.8f;
                float num52 = 2f;
                Vector2 value3 = (target.rotation - 1.57079637f).ToRotationVector2();
                Vector2 value4 = value3 * target.velocity.Length();
                SoundEngine.PlaySound(SoundID.Item14, target.position);
                int num3;
                for (int num53 = 0; num53 < 40; num53 = num3 + 1)
                {
                    int num54 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 127, 0f, 0f, 200, default, num50);
                    Dust dust = Main.dust[num54];
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)target.width / 2f;
                    dust.noGravity = true;
                    dust.velocity *= 3f;
                    dust.velocity += value4 * Main.rand.NextFloat();
                    num54 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 127, 0f, 0f, 100, default, num51);
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)target.width / 2f;
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    dust.color = Color.Crimson * 0.5f;
                    dust.velocity += value4 * Main.rand.NextFloat();
                    num3 = num53;
                }
                for (int num55 = 0; num55 < 20; num55 = num3 + 1)
                {
                    int num56 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 127, 0f, 0f, 0, default, num52);
                    Dust dust = Main.dust[num56];
                    dust.position = target.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)target.velocity.ToRotation(), default) * (float)target.width / 3f;
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += value4 * (0.6f + 0.6f * Main.rand.NextFloat());
                    num3 = num55;
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            /*
            if (crit)
            {
                target.AddBuff(BuffID.OnFire, 600, false);
                SoundEngine.PlaySound(SoundID.Item14, target.position);
            }
            */
        }
    }
}
