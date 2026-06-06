using CalRD.Dusts;
using CalRD.Items.Accessories;
using CalRD.Items.Placeables.Banners;
using CalRD.Buffs.StatDebuffs;
using CalRD.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.AcidRain
{
    public abstract class CalamityBoss : ModNPC
    {
        public abstract List<int> Loot { get; }
    }

    public class GammaSlime : ModNPC
    {
        public float angularMultiplier1;
        public float angularMultiplier2;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gamma Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 44;

			NPC.damage = 110;
			NPC.lifeMax = 9200;
			NPC.DR_NERD(0.15f);
			NPC.defense = 25;

            NPC.aiStyle = AIType = -1;

			NPC.knockBackResist = 0f;
            AnimationType = NPCID.CorruptSlime;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 8, 30);
            NPC.alpha = 50;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<GammaSlimeBanner>();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(angularMultiplier1);
            writer.Write(angularMultiplier2);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            angularMultiplier1 = reader.ReadSingle();
            angularMultiplier2 = reader.ReadSingle();
        }

        public override void AI()
        {
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.6f, 0.8f, 0.6f);
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (NPC.velocity.Y == 0f && NPC.ai[3] <= 0f && !player.npcTypeNoAggro[NPC.type])
            {
                NPC.velocity.X *= 0.8f;
                if (NPC.ai[0]++ >= 30f)
                {
                    NPC.velocity.Y -= MathHelper.Clamp(Math.Abs(player.Center.Y - NPC.Center.Y) / 16f, 5f, 15f);
                    NPC.velocity.X = NPC.DirectionTo(player.Center).X * 16f;
                    NPC.ai[0] = 0f;
                    NPC.ai[1]++;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            float angle = MathHelper.TwoPi / 5f * i + (NPC.ai[1] % 2) * MathHelper.PiOver2;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, angle.ToRotationVector2() * 7f, ModContent.ProjectileType<GammaAcid>(),
                                Main.expertMode ? 36 : 45, 3f);
                        }
                    }
                    NPC.netUpdate = true;
                }
            }
            else
            {
                NPC.velocity.X *= 0.9935f;
            }
            if (NPC.ai[3] > 0f)
                NPC.ai[3]--;
            if (NPC.ai[3] > 240f)
            {
                NPC.velocity.X *= 0.95f;
                NPC.velocity.Y += 0.2f;
            }
            // Hold energy for laser
            if (NPC.ai[3] > 480f)
            {
                float scale = NPC.ai[3] < 530 ? 2.25f : 1.65f;
                Vector2 destination = NPC.Top + new Vector2(0f, 6f);
                Dust dust = Dust.NewDustPerfect(destination + new Vector2(0f, 12f).RotatedByRandom(MathHelper.TwoPi), (int)CalamityDusts.SulfurousSeaAcid);
                dust.velocity = Vector2.Normalize(destination - dust.position) * 3f;
                dust.scale = scale;
                dust.noGravity = true;
                if (NPC.ai[3] <= 540f)
                {
                    float length = MathHelper.Lerp(20f, 550f, (NPC.ai[3] - 480f) / 60f);
                    float outwardness = MathHelper.Lerp(1f, 0f, (NPC.ai[3] - 480f) / 60f);
                    for (float i = NPC.Top.Y + 4f; i >= NPC.Top.Y + 4f - length; i -= 8f)
                    {
                        float angle = i / 24f;
                        Vector2 spawnPosition = new Vector2(NPC.Center.X, i);
                        dust = Dust.NewDustPerfect(spawnPosition, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.scale = 1.5f;
                        dust.velocity = Vector2.UnitX * (float)Math.Cos(angle) * 4f * outwardness;
                        dust.noGravity = true;
                    }
                }
            }
            // Release laser
            if (NPC.ai[3] == 480f)
            {
                angularMultiplier1 = Main.rand.NextFloat(3f);
                angularMultiplier2 = Main.rand.NextFloat(4f);
                NPC.netUpdate = true;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.Zombie104, NPC.position); // Moon lord beam sound
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY, ModContent.ProjectileType<GammaBeam>(), Main.expertMode ? 96 : 120, 4f, Main.myPlayer, 0f, NPC.whoAmI);
                }
            }
            // Very complex particle effects while releasing the beam
            else if (NPC.ai[3] >= 300f)
            {
                float angle = (NPC.ai[3] / 30f) % MathHelper.TwoPi;
                float x = (float)Math.Sin(angle * angularMultiplier1) * (float)Math.Cos(angle);
                float y = (float)Math.Cos(angle * angularMultiplier1) * (float)Math.Sin(angle);
                Vector2 velocity = new Vector2(x * 4.5f, y * 2f);
                Dust dust = Dust.NewDustPerfect(NPC.Center + angle.ToRotationVector2() * 8f, (int)CalamityDusts.SulfurousSeaAcid);
                dust.velocity = velocity;
                dust.scale = (float)Math.Cos(angle) + 2f;
                dust.noGravity = true;

                dust = Dust.NewDustPerfect(NPC.Center + angle.ToRotationVector2() * 8f, (int)CalamityDusts.SulfurousSeaAcid);
                dust.velocity = -velocity;
                dust.scale = (float)Math.Cos(angle) + 2f;
                dust.noGravity = true;
            }
            if (Math.Abs(player.Center.X - NPC.Center.X) < 250f &&
                player.Center.X - NPC.Center.X < 0f &&
                NPC.ai[3] == 0f && 
                Main.rand.NextBool(110) && !player.npcTypeNoAggro[NPC.type])
            {
                NPC.ai[3] = 600f;
                NPC.netUpdate = true;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GammaSlimeGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GammaSlimeGore2").Type, NPC.scale);
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[3] >= 480f && NPC.ai[3] <= 540f)
            {
                float length = MathHelper.Lerp(20f, 550f, (NPC.ai[3] - 480f) / 60f);
                float opacity = MathHelper.Lerp(0.3f, 0.9f, (NPC.ai[3] - 480f) / 60f);
                Utils.DrawLine(spriteBatch, NPC.Top + new Vector2(0f, 4f), NPC.Top + new Vector2(0f, 4f) - Vector2.UnitY * length, Color.Lerp(Color.Lime, Color.Transparent, opacity));
            }
            CalamityGlobalNPC.DrawGlowmask(NPC, spriteBatch, ModContent.Request<Texture2D>(Texture + "Glow").Value);
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<LeadCore>(), 30);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }
    }
}
