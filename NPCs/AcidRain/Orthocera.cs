using CalRD.Dusts;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalRD.Buffs.StatDebuffs;
namespace CalRD.NPCs.AcidRain
{
    public class Orthocera : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orthocera");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 62;
            NPC.height = 34;
            NPC.aiStyle = AIType = -1;

            NPC.damage = 45;
            NPC.lifeMax = 280;
            NPC.defense = 15;
			NPC.DR_NERD(0.075f);

            if (CalamityWorld.downedPolterghast)
            {
                NPC.damage = 120;
                NPC.lifeMax = 7000;
                NPC.defense = 35;
				NPC.DR_NERD(0.15f);
            }

            NPC.knockBackResist = 0.6f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 4, 20);
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath13;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<OrthoceraBanner>();
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            NPC.ai[1] += 1f;
            float maxSpeed = CalamityWorld.downedPolterghast ? 12.8f : 10.5f;
            if (NPC.target >= 0 && NPC.target < 255)
            {
                Player player = Main.player[NPC.target];
                // Swim
                if (NPC.ai[1] % 250f < 180f)
                {
                    if (NPC.wet)
                    {
                        // Reset so we can jum again.
                        NPC.ai[3] = 0f;

                        // Swim towards the player if we're not too close.
                        // If we're close, simply resume our current movement.
                        if (NPC.Distance(player.Center) > 150f)
                        {
                            NPC.velocity = (NPC.velocity * 17f + NPC.DirectionTo(player.Center) * maxSpeed) / 18f;
                            if (NPC.ai[0] != 12f)
                            {
                                NPC.ai[0] = 12f;
                                NPC.netUpdate = true;
                            }
                        }
                        // Variable for X movement later.
                        // It seems to slow down for some dumb reason in the jump phase without this value
                        NPC.ai[2] = NPC.velocity.X;

                        // Make sure the value isn't 0 since we're relying on multiplication
                        if (NPC.ai[2] == 0f)
                            NPC.ai[2] = 0.1f;

                        if (Math.Abs(NPC.ai[2]) < 7f)
                            NPC.ai[2] = Math.Abs(NPC.ai[2]) * 7f;
                        if (Math.Abs(NPC.ai[2]) > 16f)
                            NPC.ai[2] = Math.Abs(NPC.ai[2]) * 16f;
                        NPC.ai[2] = Math.Abs(NPC.ai[2]) * (player.Center.X - NPC.Center.X > 0).ToDirectionInt();
                    }
                    else
                    {
                        if (NPC.ai[0] <= 0f)
                            NPC.velocity.Y += 0.2f;
                        else
                            NPC.ai[0] -= 1f;
                    }
                    NPC.direction = NPC.spriteDirection = (NPC.velocity.X > 0).ToDirectionInt();
                }
                // And jump/shoot
                else if (NPC.ai[1] % 220f > 180f)
                {
                    float yAcceleration = CalamityWorld.downedPolterghast ? 0.07f : 0.05f;
                    if (NPC.ai[1] % 220f < 200f)
                    {
                        NPC.velocity.Y -= yAcceleration;
                    }
                    else
                    {
                        NPC.velocity.Y += yAcceleration;
                    }
                    if (NPC.ai[1] % 220f == 219f)
                        NPC.ai[3] = 1f;
                    if (!NPC.wet)
                        NPC.velocity.X = NPC.ai[2];
                }
                // Don't jump mid-air
                if (NPC.ai[1] % 220f > 180f && NPC.ai[3] == 1f)
                {
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver2 + MathHelper.Pi;

                if (NPC.spriteDirection == -1)
                    NPC.rotation -= MathHelper.PiOver2;
                // If sitting on land, slow down and, if in the middle of a jump, release a stream of acid.
                if (!NPC.wet)
                {
                    NPC.velocity.X *= 0.92f;
                    // Spit out a stream of acid based on our rotation
                    if (NPC.ai[1] % 220f == 195f)
                    {
                        float rotation = NPC.rotation - MathHelper.Pi - MathHelper.PiOver2 - MathHelper.PiOver4;
                        if (NPC.spriteDirection == -1)
                            rotation += MathHelper.PiOver2;

                        int damage = CalamityWorld.downedPolterghast ? 40 : CalamityWorld.downedAquaticScourge ? 26 : 18;
						if (Main.expertMode)
							damage = (int)Math.Round(damage * 0.8);

                        if (CalamityWorld.downedPolterghast)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                float angle = MathHelper.Lerp(-0.3f, 0.3f, i / 2f);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (rotation + angle).ToRotationVector2() * 10f, ModContent.ProjectileType<OrthoceraStream>(), damage, 2f);
                            }
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<OrthoceraStream>(), damage, 2f);
                    }
                }
                // Prevent yeeting into the sky at the speed of light
                NPC.velocity = Vector2.Clamp(NPC.velocity, new Vector2(-maxSpeed), new Vector2(maxSpeed));
            }
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CorrodedFossil>(), 3 * (CalamityWorld.downedPolterghast ? 5 : 1), 1, 3);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<OrthoceraShell>(), 20);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("OrthoceraGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("OrthoceraGore2").Type, NPC.scale);
                    for (int k = 0; k < 10; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    }
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }
    }
}
