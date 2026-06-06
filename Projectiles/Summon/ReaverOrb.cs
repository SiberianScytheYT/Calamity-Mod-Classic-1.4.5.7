using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Summon
{
    public class ReaverOrb : ModProjectile
    {
        public int dust = 3;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Orb");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 50;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 18000;
            Projectile.alpha = 50;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
        }

        public override void AI()
        {
            bool flag64 = Projectile.type == ModContent.ProjectileType<ReaverOrb>();
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            if (!modPlayer.reaverOrb)
            {
                Projectile.active = false;
                return;
            }
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.rOrb = false;
                }
                if (modPlayer.rOrb)
                {
                    Projectile.timeLeft = 2;
                }
            }
            dust--;
            if (dust >= 0)
            {
                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.MinionDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                int num501 = 50;
                for (int num502 = 0; num502 < num501; num502++)
                {
                    int num503 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 16f), Projectile.width, Projectile.height - 16, 157, 0f, 0f, 0, default, 1f);
                    Main.dust[num503].velocity *= 2f;
                    Main.dust[num503].scale *= 1.15f;
                }
            }
            if (player.MinionDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int damage2 = (int)((float)Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = damage2;
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 1f / 255f, (255 - Projectile.alpha) * 0f / 255f);
            Projectile.position.X = player.Center.X - (float)(Projectile.width / 2);
            Projectile.position.Y = player.Center.Y - (float)(Projectile.height / 2) + player.gfxOffY - 60f;
            if (player.gravDir == -1f)
            {
                Projectile.position.Y = Projectile.position.Y + 120f;
                Projectile.rotation = MathHelper.Pi;
            }
            else
            {
                Projectile.rotation = 0f;
            }
            Projectile.position.X = (float)(int)Projectile.position.X;
            Projectile.position.Y = (float)(int)Projectile.position.Y;
            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.ai[0] != 0f)
                {
                    Projectile.ai[0] -= 1f;
                    return;
                }
                bool foundTarget = false;
                float maxDist = 600f;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        if (Vector2.Distance(Projectile.Center, npc.Center) < maxDist && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            foundTarget = true;
							break;
                        }
                    }
                }
                if (foundTarget)
                {
                    int projAmt = Main.rand.Next(4, 9);
                    for (int u = 0; u < projAmt; u++)
                    {
						Vector2 source = new Vector2(Projectile.Center.X - 4f, Projectile.Center.Y);
						Vector2 velocity = CalamityUtils.RandomVelocity(100f, 90f, 120f);
                        int spore = Projectile.NewProjectile(Entity.GetSource_FromThis(), source, velocity, ProjectileID.SporeGas + Main.rand.Next(3), Projectile.damage, 1.5f, Projectile.owner, 0f, 0f);
                        Main.projectile[spore].minionSlots = 0f;
						Main.projectile[spore].Calamity().forceMinion = true;
						Main.projectile[spore].usesLocalNPCImmunity = true;
						Main.projectile[spore].localNPCHitCooldown = 30;
                    }
                    SoundEngine.PlaySound(SoundID.Item77, Projectile.position);
                    Projectile.ai[0] = 50f;
                }
            }
        }

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */
        {
            return false;
        }
    }
}
