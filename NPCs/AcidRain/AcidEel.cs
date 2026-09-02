using CalRD.BiomeManagers;
using CalRD.Dusts;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
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
    public class AcidEel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acid Eel");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 7;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionXOverride = 0
            };
            value.Position.X += 15;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Their dorsal fin allows great movement underwater, which allows them to rush towards prey.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 72;
            NPC.height = 18;

            NPC.damage = 20;
            NPC.lifeMax = 80;
            NPC.defense = 4;
            NPC.knockBackResist = 0.9f;

            if (CalamityWorld.downedPolterghast)
            {
				NPC.DR_NERD(0.05f);
                NPC.damage = 100;
                NPC.lifeMax = 6000;
                NPC.defense = 20;
				NPC.knockBackResist = 0.7f;
            }
            else if (CalamityWorld.downedAquaticScourge)
            {
                NPC.damage = 50;
                NPC.lifeMax = 240;
            }

            NPC.value = Item.buyPrice(0, 0, 3, 32);
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AcidEelBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };
        }

        public override void AI()
        {
            NPC.TargetClosest(false);

            if (Main.rand.NextBool(480))
                SoundEngine.PlaySound(SoundID.Zombie32, NPC.Center); // Slither sound

            if (NPC.ai[2] == 0f && !NPC.wet)
            {
                NPC.netUpdate = true;
                NPC.ai[2] = 1f;
            }
            if (NPC.ai[2] == 1f && NPC.wet)
            {
                NPC.netUpdate = true;
                NPC.ai[2] = 0f;
            }

            if (NPC.ai[2] == 0f)
            {
                NPC.ai[1] += 1f;
                if (NPC.ai[1] % 150f == 0f || NPC.direction == 0)
                {
                    NPC.direction = (Main.player[NPC.target].position.X > NPC.position.X).ToDirectionInt();
                }
                float acceleration = 0.3f;
                float yAcceleration = 0.08f;
                float maxSpeedX = 15f;
                float maxSpeedY = 4f;
                if (CalamityWorld.downedPolterghast)
                {
                    acceleration = 0.6f;
                    yAcceleration = 0.15f;
                    maxSpeedX = 20f;
                    maxSpeedY = 7f;
                }
                NPC.velocity.X += NPC.direction * acceleration;

                if (NPC.collideX)
                    NPC.direction *= -1;

                NPC.spriteDirection = NPC.direction;

                NPC.velocity.Y += (Main.player[NPC.target].position.Y > NPC.position.Y).ToDirectionInt() * yAcceleration;
                if (NPC.velocity.X > maxSpeedX)
                {
                    NPC.velocity.X = maxSpeedX;
                    NPC.netUpdate = true;
                }
                if (NPC.velocity.X < -maxSpeedX)
                {
                    NPC.velocity.X = -maxSpeedX;
                    NPC.netUpdate = true;
                }
                if (NPC.velocity.Y > maxSpeedY)
                {
                    NPC.velocity.Y = maxSpeedY;
                    NPC.netUpdate = true;
                }
                if (NPC.velocity.Y < -maxSpeedY)
                {
                    NPC.velocity.Y = -maxSpeedY;
                    NPC.netUpdate = true;
                }
                NPC.rotation = NPC.velocity.X * 0.02f;
            }
            else
            {
                NPC.rotation = NPC.rotation.AngleLerp(0f, 0.1f);
                NPC.velocity.X *= 0.95f;
                if (NPC.velocity.Y < 14f)
                    NPC.velocity.Y += 0.15f;
            }
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SulfuricScale>(), 2 * (CalamityWorld.downedAquaticScourge ? 6 : 1), 1, 3);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SlitheringEels>(), CalamityWorld.downedAquaticScourge, 0.05f);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            CalamityGlobalNPC.DrawGlowmask(NPC, spriteBatch, ModContent.Request<Texture2D>(Texture + "Glow").Value);
            if (NPC.velocity.Length() > 1.5f)
            {
                CalamityGlobalNPC.DrawAfterimage(NPC, spriteBatch, drawColor, Color.Transparent, directioning: true);
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
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AcidEelGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AcidEelGore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AcidEelGore3").Type, NPC.scale);
                    for (int k = 0; k < 20; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
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
