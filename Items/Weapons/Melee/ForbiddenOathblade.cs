using CalRD.Projectiles.Melee;
using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class ForbiddenOathblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Oathblade");
/*
            Tooltip.SetDefault("Fires a demonic scythe and critical hits cause shadowflame explosions");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 74;
            Item.damage = 64;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<ForbiddenOathbladeProjectile>();
            Item.shootSpeed = 3f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BladecrestOathsword>());
            recipe.AddIngredient(ModContent.ItemType<OldLordOathsword>());
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 240);
            target.AddBuff(BuffID.OnFire, 240);
            if (hit.Crit)
            {
                target.AddBuff(BuffID.ShadowFlame, 720);
                target.AddBuff(BuffID.OnFire, 720);
                player.ApplyDamageToNPC(target, (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)) * 2, 0f, 0, false);
                float num50 = 1.7f;
                float num51 = 0.8f;
                float num52 = 2f;
                Vector2 value3 = (target.rotation - 1.57079637f).ToRotationVector2();
                Vector2 value4 = value3 * target.velocity.Length();
                SoundEngine.PlaySound(SoundID.Item14, target.position);
                int num3;
                for (int num53 = 0; num53 < 40; num53 = num3 + 1)
                {
                    int num54 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 173, 0f, 0f, 200, default, num50);
                    Dust dust = Main.dust[num54];
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)target.width / 2f;
                    dust.noGravity = true;
                    dust.velocity.Y -= 4.5f;
                    dust.velocity *= 3f;
                    dust.velocity += value4 * Main.rand.NextFloat();
                    num54 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 173, 0f, 0f, 100, default, num51);
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)target.width / 2f;
                    dust.velocity.Y -= 3f;
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    dust.color = Color.Crimson * 0.5f;
                    dust.velocity += value4 * Main.rand.NextFloat();
                    num3 = num53;
                }
                for (int num55 = 0; num55 < 20; num55 = num3 + 1)
                {
                    int num56 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 173, 0f, 0f, 0, default, num52);
                    Dust dust = Main.dust[num56];
                    dust.position = target.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)target.velocity.ToRotation(), default) * (float)target.width / 3f;
                    dust.noGravity = true;
                    dust.velocity.Y -= 1.5f;
                    dust.velocity *= 0.5f;
                    dust.velocity += value4 * (0.6f + 0.6f * Main.rand.NextFloat());
                    num3 = num55;
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Shadowflame>(), 240);
            target.AddBuff(BuffID.OnFire, 240);
            /*
            if (crit)
            {
                target.AddBuff(ModContent.BuffType<Shadowflame>(), 720);
                target.AddBuff(BuffID.OnFire, 720);
                SoundEngine.PlaySound(SoundID.Item14, target.position);
            }
            */
        }
    }
}
