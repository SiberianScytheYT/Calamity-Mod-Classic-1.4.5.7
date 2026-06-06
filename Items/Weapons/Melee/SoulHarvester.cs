using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class SoulHarvester : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soul Harvester");
/*
            Tooltip.SetDefault("Shoots a soul scythe\n" +
                "Enemies explode when on low health, spreading the plague");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.damage = 98;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.height = 64;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<SoulScythe>();
            Item.shootSpeed = 18f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 10);
            recipe.AddIngredient(ItemID.CursedFlame, 20);
            recipe.AddIngredient(ItemID.DeathSickle);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 75);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            target.AddBuff(BuffID.CursedInferno, 120);
            if (target.life <= (target.lifeMax * 0.15f))
            {
                SoundEngine.PlaySound(SoundID.Item14, target.position);

                // DEFECT -- HiveBombExplosion does not exist.
                // Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<HiveBombExplosion>(), (int)(item.damage * (player.allDamage + player.meleeDamage - 1f)), knockback, Main.myPlayer);

                for (int num621 = 0; num621 < 30; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 89, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 50; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 89, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 89, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            target.AddBuff(BuffID.CursedInferno, 120);
            if (target.statLife <= (target.statLifeMax * 0.15f))
            {
                SoundEngine.PlaySound(SoundID.Item14, target.position);

                // DEFECT -- HiveBombExplosion does not exist.
                // Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<HiveBombExplosion>(), (int)(item.damage * (player.allDamage + player.meleeDamage - 1f)), item.knockBack, Main.myPlayer);

                for (int num621 = 0; num621 < 30; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 89, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 50; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 89, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 89, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
    }
}
