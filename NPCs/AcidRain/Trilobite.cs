using CalRD.Dusts;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Materials;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.AcidRain
{
    public class Trilobite : ModNPC
    {
        // When the abs(velocity) is less than this, lunge in the water
        public const float MinSpeedLungePrompt = 0.5f;
        public const float MinYDriftSpeed = 0.9f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Trilobite");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("An example of an ancient creature persisting in the sulphurous sea, they have remarkable strength to be able to fling themselves, yet weigh as much as they do.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 38;
            NPC.aiStyle = AIType = -1;

            NPC.damage = 45;
            NPC.lifeMax = 300;
            NPC.defense = 15;
			NPC.DR_NERD(0.25f);

            if (CalamityWorld.downedPolterghast)
            {
                NPC.damage = 80;
                NPC.lifeMax = 7500;
                NPC.defense = 30;
            }

            NPC.knockBackResist = 0.2f;
            NPC.value = Item.buyPrice(0, 0, 4, 0);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath27;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<TrilobiteBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            if (!NPC.wet)
            {
                if (NPC.ai[0] > 0)
                    NPC.ai[0]--;
                NPC.rotation += NPC.velocity.X * 0.1f;
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X *= 0.99f;
                    if (Math.Abs(NPC.velocity.X) < 0.01f)
                    {
                        NPC.velocity.X = 0f;
                    }
                    NPC.netUpdate = true;
                }
                NPC.velocity.Y += 0.3f;
                if (NPC.velocity.Y > 13f)
                {
                    NPC.velocity.Y = 13f;
                    NPC.netUpdate = true;
                }
            }
            else
            {
                Player player = Main.player[NPC.target];
                if (NPC.velocity.Length() < MinSpeedLungePrompt)
                {
                    NPC.TargetClosest(true);
                    float speed = 15f;
                    if (CalamityWorld.downedPolterghast)
                    {
                        speed = 18.5f;
                    }

                    NPC.velocity = NPC.DirectionTo(player.Center) * speed;
                    NPC.velocity.X *= 1.6f;
                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                    NPC.netUpdate = true;
                }
                else
                {
                    if (Math.Abs(NPC.velocity.X) < 20f)
                    {
                        NPC.velocity.X += NPC.direction * 0.02f;
                    }
                    NPC.rotation = NPC.velocity.X * 0.4f;
                    if (Math.Abs(NPC.velocity.Y) < MinYDriftSpeed)
                    {
                        NPC.velocity.X *= 0.96f;
                    }
                    else if (Math.Abs(NPC.velocity.X) < 3.5f)
                    {
                        float speedX = 18f;
                        float speedY = 9f;
                        if (CalamityWorld.downedPolterghast)
                        {
                            speedX = 22f;
                            speedY = 11f;
                        }
                        NPC.velocity = NPC.DirectionTo(player.Center) * new Vector2(speedX, speedY);
                        NPC.netUpdate = true;
                    }
                    NPC.velocity.Y *= 0.98f;
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => CalamityWorld.downedPolterghast, ModContent.ItemType<CorrodedFossil>(), 15, 1, 3);
            npcLoot.AddIf(() => !CalamityWorld.downedPolterghast, ModContent.ItemType<CorrodedFossil>(), 3, 1, 3);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.velocity.Length() > 0.5f)
            {
                CalamityGlobalNPC.DrawAfterimage(NPC, spriteBatch, drawColor, Color.Transparent, directioning: true);
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[0] <= 0f)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath11, NPC.Center);
                    int projDamage = CalamityWorld.downedPolterghast ? 35 : CalamityWorld.downedAquaticScourge ? 29 : 21;
					if (Main.expertMode)
						projDamage = (int)Math.Round(projDamage * 0.8);

                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + Utils.NextVector2Unit(Main.rand) * NPC.Size * 0.7f,
                        -NPC.velocity.RotatedByRandom(MathHelper.ToRadians(10f)), ModContent.ProjectileType<TrilobiteSpike>(),
                        projDamage, 3f);
                    NPC.ai[0] = Main.rand.Next(50, 65);
                    NPC.netUpdate = true;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 4)
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
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("TrilobiteGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("TrilobiteGore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("TrilobiteGore3").Type, NPC.scale);
                }
                for (int k = 0; k < 30; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }
    }
}
