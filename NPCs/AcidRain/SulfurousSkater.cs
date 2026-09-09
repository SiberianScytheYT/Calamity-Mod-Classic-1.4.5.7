using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Melee;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.AcidRain
{
    public class SulfurousSkater : ModNPC
    {
        public bool Flying = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphurous Skater");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("These creatures are capable of remaining on the surface of the water, barely noticeable, they're also capable of flight if they need to.")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Flying);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Flying = reader.ReadBoolean();
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 48;

            NPC.damage = 48;
            NPC.lifeMax = 280;
            NPC.defense = 3;

            if (CalamityWorld.downedPolterghast)
            {
                NPC.damage = 85;
                NPC.lifeMax = 7000;
                NPC.defense = 15;
            }

            NPC.knockBackResist = 0.8f;
            NPC.value = Item.buyPrice(0, 0, 5, 25);
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
            BannerItem = ModContent.ItemType<SulfurousSkaterBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<Sulphur>().Type, ModContent.GetInstance<AcidRainBiome>().Type };

            NPC.aiStyle = AIType = -1;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (!Flying)
            {
				NPC.knockBackResist = 0.8f;
                NPC.DR_NERD(0.35f);
                NPC.noGravity = false;
                float minimumDistance = float.PositiveInfinity;
                Projectile closestBubble = null;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<SulphuricAcidBubble>() && Main.projectile[i].active)
                    {
                        if (Math.Abs(NPC.Center.X - Main.projectile[i].Center.X) < minimumDistance &&
                            Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height) &&
                            Main.projectile[i].Center.Y > NPC.Bottom.Y)
                        {
                            minimumDistance = NPC.Distance(Main.projectile[i].Center);
                            closestBubble = Main.projectile[i];
                        }
                    }
                }
                if (minimumDistance >= 2400f)
                {
                    closestBubble = null;
                }

                Vector2 destination = player.Center;

                if (closestBubble != null)
                {
                    destination = closestBubble.Center;
                }
                // Stay on water instead of falling into it
                if (NPC.wet)
                {
                    if (NPC.velocity.Y >= 0f)
                    {
                        NPC.velocity.Y = -3f;
                    }
                }

                if (closestBubble != null && minimumDistance < 200f)
                {
                    NPC.velocity.Y += 0.2f;

                    if (closestBubble.Hitbox.Intersects(NPC.Hitbox))
                    {
                        Flying = true;
                        closestBubble.Kill();
                        NPC.netSpam = 0;
                        NPC.netUpdate = true;
                    }
                }
                if (NPC.velocity.Y == 0f || NPC.wet)
                {
                    NPC.TargetClosest(false);
                    NPC.velocity.X *= 0.85f;
                    NPC.ai[1]++;
                    float lungeForwardSpeed = 15f;
                    float jumpSpeed = 4f;
                    if (Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                    {
                        lungeForwardSpeed *= 1.2f;
                    }
                    if (NPC.ai[1] >= 17)
                    {
                        NPC.ai[1] = 0f;
                        NPC.velocity.Y -= jumpSpeed;
                        NPC.velocity.X = lungeForwardSpeed * (NPC.Center.X - destination.X < 0).ToDirectionInt();
                        NPC.spriteDirection = (NPC.Center.X - destination.X > 0).ToDirectionInt();
                        NPC.netSpam = 0;
                        NPC.netUpdate = true;
                    }
                }
                else
                {
                    NPC.knockBackResist = 0f;
                }
            }
            else
            {
				NPC.knockBackResist = 0.5f;
                NPC.DR_NERD(0f);
                float speed = CalamityWorld.downedPolterghast ? 17f : 14f;
                float inertia = CalamityWorld.downedPolterghast ? 20f : 24.5f;
                if (NPC.Distance(player.Center) < 200f)
                    inertia *= 0.667f;
                NPC.velocity = (NPC.velocity * inertia + NPC.DirectionTo(player.Center) * speed) / (inertia + 1f);
                NPC.spriteDirection = (NPC.velocity.X < 0).ToDirectionInt();
                if (NPC.Distance(player.Center) < player.Size.Length())
                {
                    Flying = false;
                    NPC.netSpam = 0;
                    NPC.netUpdate = true;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                SpriteDirection = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            if (!Flying && !NPC.IsABestiaryIconDummy)
                NPC.frame.Y = 0;
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 4)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                    if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                    {
                        NPC.frame.Y = frameHeight;
                    }
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return;
            CalamityGlobalNPC.DrawGlowmask(NPC, spriteBatch, ModContent.Request<Texture2D>(Texture + "Glow").Value, true, Vector2.UnitY * 4f);
            CalamityGlobalNPC.DrawAfterimage(NPC, spriteBatch, drawColor, Color.Transparent, directioning: true, invertedDirection: true);
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
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SulfurousSkaterGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SulfurousSkaterGore2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SulfurousSkaterGore3").Type, NPC.scale);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => CalamityWorld.downedPolterghast, ModContent.ItemType<CorrodedFossil>(), 15, 1, 3);
            npcLoot.AddIf(() => CalamityWorld.downedPolterghast, ModContent.ItemType<CorrodedFossil>(), 3, 1, 3);
            npcLoot.Add(ModContent.ItemType<SulphurousGrabber>(), 20);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }
    }
}
