using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Melee;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Astral
{
    public class Mantis : ModNPC
    {
        private static Texture2D glowmask;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mantis");
            Main.npcFrameCount[NPC.type] = 14;
            if (!Main.dedServ)
                glowmask = ModContent.Request<Texture2D>("CalRD/NPCs/Astral/MantisGlow").Value;
        }

        public override void SetDefaults()
        {
            NPC.damage = 55;
            NPC.width = 60;
            NPC.height = 58;
            NPC.aiStyle = -1;
            NPC.defense = 6;
			NPC.DR_NERD(0.15f);
            NPC.lifeMax = 340;
            NPC.knockBackResist = 0.2f;
            NPC.value = Item.buyPrice(0, 0, 15, 0);
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/AstralEnemyDeath");
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<MantisBanner>();
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            if (CalamityWorld.downedAstrageldon)
            {
                NPC.damage = 85;
                NPC.defense = 16;
                NPC.knockBackResist = 0.1f;
                NPC.lifeMax = 510;
            }
        }

        public override void AI()
        {
            NPC.TargetClosest(false);

            Player target = Main.player[NPC.target];

            if (NPC.ai[0] == 0f)
            {
                float acceleration = CalamityWorld.death ? 0.07f : 0.045f;
                float maxSpeed = CalamityWorld.death ? 10.5f : 6.8f;
                if (NPC.Center.X > target.Center.X)
                {
                    NPC.velocity.X -= acceleration;
                    if (NPC.velocity.X > 0)
                        NPC.velocity.X -= acceleration;
                    if (NPC.velocity.X < -maxSpeed)
                        NPC.velocity.X = -maxSpeed;
                }
                else
                {
                    NPC.velocity.X += acceleration;
                    if (NPC.velocity.X < 0)
                        NPC.velocity.X += acceleration;
                    if (NPC.velocity.X > maxSpeed)
                        NPC.velocity.X = maxSpeed;
                }

                //if need to jump
                if (NPC.velocity.Y == 0f && (HoleBelow() || (NPC.collideX && NPC.position.X == NPC.oldPosition.X)))
                {
                    NPC.velocity.Y = CalamityWorld.death ? -7f : -5f;
                }

                //check if we can shoot at target.
                Vector2 vector = NPC.Center - target.Center;
                if (vector.Length() < 480f && Collision.CanHit(NPC.position, NPC.width, NPC.height, target.position, target.width, target.height))
                {
                    NPC.ai[1] += 1f;
                    if (NPC.ai[1] >= (CalamityWorld.death ? 60f : 120f))
                    {
                        //fire projectile
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = NPC.ai[2] = 0f;
                        NPC.frame.Y = 400;
                        NPC.frameCounter = 0;
                    }
                }
                else
                    NPC.ai[1] -= 0.5f;

                if (NPC.justHit)
                    NPC.ai[1] -= 60f;

                if (NPC.ai[1] < 0f)
                    NPC.ai[1] = 0f;
            }
            else
            {
                NPC.ai[2] += 1f;
                NPC.velocity.X *= 0.95f;
                if (NPC.ai[2] == 20f) //Don't do >= 20f or it'll cause a wave of scythes
                {
                    SoundEngine.PlaySound(SoundID.Item71, NPC.position);
                    Vector2 vector = Main.player[NPC.target].Center - NPC.Center;
                    vector.Normalize();
                    int damage = CalamityWorld.downedAstrageldon ? 55 : 45;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + (NPC.Center.X < target.Center.X ? -14f : 14f) * Vector2.UnitX, vector * 7f, ModContent.ProjectileType<MantisRing>(), damage, 0f);
                }
            }

            NPC.direction = NPC.Center.X > target.Center.X ? 0 : 1;
            NPC.spriteDirection = NPC.direction;
        }

        private bool HoleBelow()
        {
            //width of npc in tiles
            int tileWidth = 4;
            int tileX = (int)(NPC.Center.X / 16f) - tileWidth;
            if (NPC.velocity.X > 0) //if moving right
            {
                tileX += tileWidth;
            }
            int tileY = (int)((NPC.position.Y + NPC.height) / 16f);
            for (int y = tileY; y < tileY + 2; y++)
            {
                for (int x = tileX; x < tileX + tileWidth; x++)
                {
                    if (Main.tile[x, y].HasTile)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] == 0f)
            {
                if (NPC.velocity.Y != 0)
                {
                    NPC.frame.Y = frameHeight * 13;
                    NPC.frameCounter = 20;
                }
                else
                {
                    NPC.frameCounter += 0.8f + Math.Abs(NPC.velocity.X) * 0.5f;
                    if (NPC.frameCounter > 10.0)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;
                        if (NPC.frame.Y > frameHeight * 5)
                        {
                            NPC.frame.Y = 0;
                        }
                    }
                }
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 4)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                    if (NPC.frame.Y >= frameHeight * 13)
                    {
                        NPC.frame.Y = 0;
                        NPC.frameCounter = 0;
                        NPC.ai[0] = 0f;
                    }
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 15;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/AstralEnemyHit"), NPC.Center);
                        break;
                    case 1:
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/AstralEnemyHit2"), NPC.Center);
                        break;
                    case 2:
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/AstralEnemyHit3"), NPC.Center);
                        break;
                }
            }

            CalamityGlobalNPC.DoHitDust(NPC, hit.HitDirection, ModContent.DustType<AstralOrange>(), 1f, 4, 24);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //draw glowmask
            spriteBatch.Draw(glowmask, NPC.Center - Main.screenPosition - new Vector2(0, 8), NPC.frame, Color.White * 0.6f, NPC.rotation, new Vector2(70, 40), 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            else if (spawnInfo.Player.InAstral(1))
            {
                return 0.16f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120, true);
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), 2, 3);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), Main.expertMode);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AstralScythe>(), CalamityWorld.downedAstrageldon, 7, 1, 1);
        }
    }
}
