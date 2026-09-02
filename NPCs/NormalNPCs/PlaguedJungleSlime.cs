using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Projectiles.Boss;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class PlaguedJungleSlime : ModNPC
    {
        public float spikeTimer = 60f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pestilent Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("A slime taken over by the plague nanomachines... Yet, in its slimy wisdom, it behaves the same.")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 1;
            NPC.damage = 55;
            NPC.width = 40;
            NPC.height = 30;
            NPC.defense = 12;
            NPC.lifeMax = 350;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.CorruptSlime;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.alpha = 60;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[BuffID.ShadowFlame] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<PestilentSlimeBanner>();
        }

        public override void AI()
        {
            if (spikeTimer > 0f)
            {
                spikeTimer -= 1f;
            }
            if (!NPC.wet && !Main.player[NPC.target].npcTypeNoAggro[NPC.type])
            {
                Vector2 vector3 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num14 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector3.X;
                float num15 = Main.player[NPC.target].position.Y - vector3.Y;
                float num16 = (float)Math.Sqrt((double)(num14 * num14 + num15 * num15));
                if (Main.expertMode && num16 < 120f && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && NPC.velocity.Y == 0f)
                {
                    NPC.ai[0] = -40f;
                    if (NPC.velocity.Y == 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.9f;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient && spikeTimer == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                        for (int n = 0; n < 5; n++)
                        {
                            Vector2 vector4 = new Vector2((float)(n - 2), -4f);
                            vector4.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
                            vector4.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
                            vector4.Normalize();
                            vector4 *= 4f + (float)Main.rand.Next(-50, 51) * 0.01f;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), vector3.X, vector3.Y, vector4.X, vector4.Y, ModContent.ProjectileType<PlagueStingerGoliathV2>(), 25, 0f, Main.myPlayer, 0f, 0f);
                            spikeTimer = 30f;
                        }
                    }
                }
                else if (num16 < 360f && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && NPC.velocity.Y == 0f)
                {
                    NPC.ai[0] = -40f;
                    if (NPC.velocity.Y == 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.9f;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient && spikeTimer == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item42, NPC.position);
                        num15 = Main.player[NPC.target].position.Y - vector3.Y - (float)Main.rand.Next(0, 200);
                        num16 = (float)Math.Sqrt((double)(num14 * num14 + num15 * num15));
                        num16 = 6.5f / num16;
                        num14 *= num16;
                        num15 *= num16;
                        spikeTimer = 50f;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), vector3.X, vector3.Y, num14, num15, ModContent.ProjectileType<PlagueStingerGoliathV2>(), 22, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !NPC.downedGolemBoss || spawnInfo.Player.Calamity().ZoneSunkenSea)
            {
                return 0f;
            }
            return SpawnCondition.HardmodeJungle.Chance * 0.09f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Plague, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Plague, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnKill()
        {
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ItemID.Stinger, Main.expertMode ? 0.5f : 0.25f);
			DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<PlagueCellCluster>(), 1, 2);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 300, true);
        }
    }
}
