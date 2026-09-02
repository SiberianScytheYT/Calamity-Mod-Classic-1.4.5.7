using CalRD.BiomeManagers;
using CalRD.Dusts;
using CalRD.Projectiles.Enemy;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.StatDebuffs;
using Terraria.GameContent.Bestiary;

namespace CalRD.NPCs.AcidRain
{
    public class NuclearToad : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nuclear Toad");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers();
            value.Position.Y += 8;
            value.PortraitPositionYOverride = 28f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("When threatened, they burst in an attempt to save the rest of their species.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 62;
            NPC.height = 34;
            NPC.defense = 4;

            NPC.aiStyle = AIType = -1;

            NPC.damage = 15;
            NPC.lifeMax = 60;
            NPC.defense = 3;

            if (CalamityWorld.downedPolterghast)
            {
                NPC.damage = 80;
                NPC.lifeMax = 5000;
                NPC.defense = 15;
            }
            else if (CalamityWorld.downedAquaticScourge)
            {
                NPC.damage = 35;
                NPC.lifeMax = 200;
            }

            NPC.knockBackResist = 0.7f;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<NuclearToadBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];

            // Slow down over time on the X axis (to prevent endless sliding as a result of KB)
            NPC.velocity.X *= 0.96f;

            // Hover on the top of the water
            if (NPC.wet)
            {
                if (NPC.velocity.Y > 2f)
                {
                    NPC.velocity.Y *= 0.9f;
                }
                NPC.velocity.Y -= 0.16f;
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
            }
            else
            {
                if (NPC.velocity.Y < -2f)
                {
                    NPC.velocity.Y *= 0.9f;
                }
                NPC.velocity.Y += 0.16f;
                if (NPC.velocity.Y > 3f)
                {
                    NPC.velocity.Y = 3f;
                }
                NPC.ai[0] = 5f;
            }
            if (Main.rand.NextBool(480))
                SoundEngine.PlaySound(SoundID.Zombie13, NPC.Center); // Ribbit sound
            float explodeDistance = CalamityWorld.downedAquaticScourge ? 295f : 195f;
            if (CalamityWorld.downedPolterghast)
                explodeDistance = 470f;
            if (NPC.Distance(player.Center) < explodeDistance)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int damage = Main.expertMode ? CalamityWorld.downedAquaticScourge ? 21 : 8 : CalamityWorld.downedAquaticScourge ? 27 : 10;
                    float speed = Main.rand.NextFloat(6f, 9f);
                    if (CalamityWorld.downedPolterghast)
                    {
                        speed *= 1.8f;
                        damage = Main.expertMode ? 36 : 45;
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, angle.ToRotationVector2() * speed, ModContent.ProjectileType<NuclearToadGoo>(), damage, 1f);
                    }
                }
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, NPC.Center);
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                NPC.netUpdate = true;
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
            for (int k = 0; k < 8; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("NuclearToadGore1").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("NuclearToadGore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("NuclearToadGore3").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("NuclearToadGore4").Type, NPC.scale);
                }
                for (int i = 0; i < 25; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, Main.rand.NextFloat(-2f, 2f), -1f, 0, default, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            CalamityGlobalNPC.DrawGlowmask(NPC, spriteBatch, ModContent.Request<Texture2D>(Texture + "Glow").Value, true);
        }
    }
}
