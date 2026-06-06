using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.GreatSandShark
{
    [AutoloadBossHead]
    public class GreatSandShark : ModNPC
    {
        private bool resetAI = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Great Sand Shark");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 15f;
            NPC.damage = 100;
            NPC.width = 300;
            NPC.height = 120;
            NPC.defense = 60;
			NPC.DR_NERD(0.25f);
			NPC.LifeMaxNERB(8000, 11000);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[ModContent.BuffType<MarkedforDeath>()] = false;
			NPC.buffImmune[BuffID.Frostburn] = false;
			NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
			NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
			NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.behindTiles = true;
            NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.timeLeft = NPC.activeTime * 30;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<GreatSandSharkBanner>();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(resetAI);
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.localAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            resetAI = reader.ReadBoolean();
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
        }

        public override void AI()
        {
            bool expertMode = Main.expertMode;
            bool revenge = CalamityWorld.revenge;
            bool death = CalamityWorld.death;
            bool lowLife = (double)NPC.life <= (double)NPC.lifeMax * (expertMode ? 0.75 : 0.5);
            bool lowerLife = (double)NPC.life <= (double)NPC.lifeMax * (expertMode ? 0.35 : 0.2);
            bool youMustDie = !Main.player[NPC.target].ZoneDesert;
			if (!Terraria.GameContent.Events.Sandstorm.Happening)
			{
				CalamityUtils.StartSandstorm();
				CalamityNetcode.SyncWorld();
			}
            if (NPC.soundDelay <= 0)
            {
                NPC.soundDelay = 480;
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/GreatSandSharkRoar"), NPC.position);
            }
            if (NPC.localAI[3] >= 1f || Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 1000f)
            {
                if (!resetAI)
                {
                    NPC.localAI[0] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    resetAI = true;
                    NPC.netUpdate = true;
                }
                int num2 = expertMode ? 35 : 50;
                float num3 = expertMode ? 0.5f : 0.42f;
                float scaleFactor = expertMode ? 7.5f : 6.7f;
                int num4 = expertMode ? 28 : 30;
                float num5 = expertMode ? 15.5f : 14f;
                if (revenge || lowerLife)
                {
                    num3 *= 1.1f;
                    scaleFactor *= 1.1f;
                    num5 *= 1.1f;
                }
                if (death)
                {
                    num3 *= 1.1f;
                    scaleFactor *= 1.1f;
                    num5 *= 1.1f;
                    num4 = 25;
                }
                if (youMustDie)
                {
                    num3 *= 1.5f;
                    scaleFactor *= 1.5f;
                    num5 *= 1.5f;
                    num4 = 20;
                }
                Vector2 vector = NPC.Center;
                Player player = Main.player[NPC.target];
                if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
                {
                    NPC.TargetClosest(true);
                    player = Main.player[NPC.target];
                    NPC.netUpdate = true;
                }
                if (player.dead || Vector2.Distance(player.Center, vector) > 5600f)
                {
                    NPC.velocity.Y += 0.4f;
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    NPC.ai[0] = 0f;
                    NPC.ai[2] = 0f;
                }
                float num17 = (float)Math.Atan2((double)(player.Center.Y - vector.Y), (double)(player.Center.X - vector.X));
                if (NPC.spriteDirection == 1)
                {
                    num17 += 3.14159274f;
                }
                if (num17 < 0f)
                {
                    num17 += 6.28318548f;
                }
                if (num17 > 6.28318548f)
                {
                    num17 -= 6.28318548f;
                }
                float num18 = 0.04f;
                if (NPC.ai[0] == 1f)
                {
                    num18 = 0f;
                }
                if (NPC.rotation < num17)
                {
                    if ((double)(num17 - NPC.rotation) > 3.1415926535897931)
                    {
                        NPC.rotation -= num18;
                    }
                    else
                    {
                        NPC.rotation += num18;
                    }
                }
                if (NPC.rotation > num17)
                {
                    if ((double)(NPC.rotation - num17) > 3.1415926535897931)
                    {
                        NPC.rotation += num18;
                    }
                    else
                    {
                        NPC.rotation -= num18;
                    }
                }
                if (NPC.rotation > num17 - num18 && NPC.rotation < num17 + num18)
                {
                    NPC.rotation = num17;
                }
                if (NPC.rotation < 0f)
                {
                    NPC.rotation += 6.28318548f;
                }
                if (NPC.rotation > 6.28318548f)
                {
                    NPC.rotation -= 6.28318548f;
                }
                if (NPC.rotation > num17 - num18 && NPC.rotation < num17 + num18)
                {
                    NPC.rotation = num17;
                }
                if (NPC.ai[0] == 0f && !player.dead)
                {
                    if (NPC.ai[1] == 0f)
                    {
                        NPC.ai[1] = (float)(300 * Math.Sign((vector - player.Center).X));
                    }
                    Vector2 vector3 = Vector2.Normalize(player.Center + new Vector2(NPC.ai[1], -200f) - vector - NPC.velocity) * scaleFactor;
                    if (NPC.velocity.X < vector3.X)
                    {
                        NPC.velocity.X += num3;
                        if (NPC.velocity.X < 0f && vector3.X > 0f)
                        {
                            NPC.velocity.X += num3;
                        }
                    }
                    else if (NPC.velocity.X > vector3.X)
                    {
                        NPC.velocity.X -= num3;
                        if (NPC.velocity.X > 0f && vector3.X < 0f)
                        {
                            NPC.velocity.X -= num3;
                        }
                    }
                    if (NPC.velocity.Y < vector3.Y)
                    {
                        NPC.velocity.Y += num3;
                        if (NPC.velocity.Y < 0f && vector3.Y > 0f)
                        {
                            NPC.velocity.Y += num3;
                        }
                    }
                    else if (NPC.velocity.Y > vector3.Y)
                    {
                        NPC.velocity.Y -= num3;
                        if (NPC.velocity.Y > 0f && vector3.Y < 0f)
                        {
                            NPC.velocity.Y -= num3;
                        }
                    }
                    int num22 = Math.Sign(player.Center.X - vector.X);
                    if (num22 != 0)
                    {
                        if (NPC.ai[2] == 0f && num22 != NPC.direction)
                        {
                            NPC.rotation += 3.14159274f;
                        }
                        NPC.direction = num22;
                        if (NPC.spriteDirection != -NPC.direction)
                        {
                            NPC.rotation += 3.14159274f;
                        }
                        NPC.spriteDirection = -NPC.direction;
                    }
                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] >= (float)num2)
                    {
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.velocity = Vector2.Normalize(player.Center - vector) * num5;
                        NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X);
                        if (num22 != 0)
                        {
                            NPC.direction = num22;
                            if (NPC.spriteDirection == 1)
                            {
                                NPC.rotation += 3.14159274f;
                            }
                            NPC.spriteDirection = -NPC.direction;
                        }
                        NPC.netUpdate = true;
                        return;
                    }
                }
                else if (NPC.ai[0] == 1f)
                {
                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] >= (float)num4)
                    {
                        NPC.localAI[3] += 1f;
                        if (NPC.localAI[3] >= 2f)
                        {
                            NPC.localAI[3] = 0f;
                        }
                        NPC.ai[0] = 0f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.netUpdate = true;
                        return;
                    }
                }
            }
            else
            {
                resetAI = false;
                if (NPC.direction == 0)
                {
                    NPC.TargetClosest(true);
                }
                Point point15 = NPC.Center.ToTileCoordinates();
                Tile tileSafely = Framing.GetTileSafely(point15);
                bool flag121 = tileSafely.HasUnactuatedTile || tileSafely.LiquidAmount > 0;
                bool flag122 = false;
                NPC.TargetClosest(false);
                Vector2 vector260 = NPC.targetRect.Center.ToVector2();
                if (Main.player[NPC.target].velocity.Y > -0.1f && !Main.player[NPC.target].dead && NPC.Distance(vector260) > 150f)
                {
                    flag122 = true;
                }
                NPC.localAI[1] += 1f;
                if (lowLife)
                {
                    bool spawnFlag = NPC.localAI[1] == 150f;
                    if (NPC.CountNPCS(NPCID.SandShark) > 2)
                    {
                        spawnFlag = false;
                    }
                    if (spawnFlag && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y + 50, NPCID.SandShark, 0, 0f, 0f, 0f, 0f, 255);
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/GreatSandSharkRoar"), NPC.position);
                    }
                }
                if (NPC.localAI[1] >= 300f)
                {
                    NPC.localAI[1] = 0f;
                    if (NPC.localAI[2] > 0f)
                    {
                        NPC.localAI[2] = 0f;
                    }
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            NPC.ai[3] = 0f;
                            break;
                        case 1:
                            NPC.ai[3] = 1f;
                            break;
                        case 2:
                            NPC.ai[3] = 2f;
                            break;
                    }
                    int random = lowerLife ? 5 : 9;
                    if (lowLife && Main.rand.Next(random) == 0)
                    {
                        NPC.localAI[3] = 1f;
                    }
                    NPC.netUpdate = true;
                }
                if (NPC.localAI[0] == -1f && !flag121)
                {
                    NPC.localAI[0] = 20f;
                }
                if (NPC.localAI[0] > 0f)
                {
                    NPC.localAI[0] -= 1f;
                }
                if (flag121)
                {
                    float num1534 = NPC.ai[1];
                    bool flag123 = false;
                    point15 = (NPC.Center + new Vector2(0f, 24f)).ToTileCoordinates();
                    tileSafely = Framing.GetTileSafely(point15.X, point15.Y - 2);
                    if (tileSafely.HasUnactuatedTile)
                    {
                        flag123 = true;
                    }
                    NPC.ai[1] = (float)flag123.ToInt();
                    if (NPC.ai[2] < 30f)
                    {
                        NPC.ai[2] += 1f;
                    }
                    if (flag122)
                    {
                        NPC.TargetClosest(true);
                        NPC.velocity.X += (float)NPC.direction * 0.15f;
                        NPC.velocity.Y += (float)NPC.directionY * 0.15f;
                        float velocityX = 8f;
                        float velocityY = 6f;
                        switch ((int)NPC.ai[3])
                        {
                            case 0:
                                velocityX = 10f;
                                velocityY = 9f;
                                break;
                            case 1:
                                velocityX = 14f;
                                velocityY = 7f;
                                break;
                            case 2:
                                velocityX = 8f;
                                velocityY = 11f;
                                break;
                        }
                        if (revenge || lowerLife)
                        {
                            velocityX *= 1.1f;
                            velocityY *= 1.1f;
                        }
                        if (youMustDie)
                        {
                            velocityX *= 1.5f;
                            velocityY *= 1.5f;
                        }
						NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -velocityX, velocityX);
						NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -velocityY, velocityY);
                        Vector2 vec4 = NPC.Center + NPC.velocity.SafeNormalize(Vector2.Zero) * NPC.Size.Length() / 2f + NPC.velocity;
                        point15 = vec4.ToTileCoordinates();
                        tileSafely = Framing.GetTileSafely(point15);
                        bool flag124 = tileSafely.HasUnactuatedTile;
                        if (!flag124 && Math.Sign(NPC.velocity.X) == NPC.direction && (NPC.Distance(vector260) < 600f || youMustDie) &&
                            (NPC.ai[2] >= 30f || NPC.ai[2] < 0f))
                        {
                            if (NPC.localAI[0] == 0f)
                            {
                                SoundEngine.PlaySound(SoundID.NPCDeath15, NPC.position);
                                NPC.localAI[0] = -1f;
                                for (int num621 = 0; num621 < 25; num621++)
                                {
                                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 32, 0f, 0f, 100, default, 2f);
                                    Main.dust[num622].velocity.Y *= 6f;
                                    Main.dust[num622].velocity.X *= 3f;
                                    if (Main.rand.NextBool(2))
                                    {
                                        Main.dust[num622].scale = 0.5f;
                                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                                    }
                                }
                                for (int num623 = 0; num623 < 50; num623++)
                                {
                                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 85, 0f, 0f, 100, default, 3f);
                                    Main.dust[num624].noGravity = true;
                                    Main.dust[num624].velocity.Y *= 10f;
                                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 268, 0f, 0f, 100, default, 2f);
                                    Main.dust[num624].velocity.X *= 2f;
                                }
                                int spawnX = (int)(NPC.width / 2);
                                for (int sand = 0; sand < 5; sand++)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + (float)Main.rand.Next(-spawnX, spawnX), NPC.Center.Y,
                                        (float)Main.rand.Next(-3, 4), (float)Main.rand.Next(-12, -6), ModContent.ProjectileType<GreatSandBlast>(), 40, 0f, Main.myPlayer, 0f, 0f);
                            }
                            NPC.ai[2] = -30f;
                            Vector2 vector261 = NPC.DirectionTo(vector260 + new Vector2(0f, -80f));
                            NPC.velocity = vector261 * 18f;
                        }
                    }
                    else
                    {
                        float num1535 = 6f;
                        NPC.velocity.X += (float)NPC.direction * 0.1f;
                        if (NPC.velocity.X < -num1535 || NPC.velocity.X > num1535)
                        {
                            NPC.velocity.X *= 0.95f;
                        }
                        if (flag123)
                        {
                            NPC.ai[0] = -1f;
                        }
                        else
                        {
                            NPC.ai[0] = 1f;
                        }
                        float num1536 = 0.06f;
                        float num1537 = 0.01f;
                        if (NPC.ai[0] == -1f)
                        {
                            NPC.velocity.Y -= num1537;
                            if (NPC.velocity.Y < -num1536)
                            {
                                NPC.ai[0] = 1f;
                            }
                        }
                        else
                        {
                            NPC.velocity.Y += num1537;
                            if (NPC.velocity.Y > num1536)
                            {
                                NPC.ai[0] = -1f;
                            }
                        }
                        if (NPC.velocity.Y > 0.4f || NPC.velocity.Y < -0.4f)
                        {
                            NPC.velocity.Y *= 0.95f;
                        }
                    }
                }
                else
                {
                    if (NPC.velocity.Y == 0f)
                    {
                        if (flag122)
                        {
                            NPC.TargetClosest(true);
                        }
                        float num1538 = 1f;
                        NPC.velocity.X += (float)NPC.direction * 0.1f;
                        if (NPC.velocity.X < -num1538 || NPC.velocity.X > num1538)
                        {
                            NPC.velocity.X *= 0.95f;
                        }
                    }
                    if (NPC.localAI[2] == 0f)
                    {
                        NPC.localAI[2] = 1f;
                        float velocityX = 12f;
                        float velocityY = 12f;
                        switch ((int)NPC.ai[3])
                        {
                            case 0:
                                velocityX = 12f;
                                velocityY = 12f;
                                break;
                            case 1:
                                velocityX = 14f;
                                velocityY = 14f;
                                break;
                            case 2:
                                velocityX = 16f;
                                velocityY = 16f;
                                break;
                        }
                        if (revenge || lowerLife)
                        {
                            velocityX *= 1.1f;
                            velocityY *= 1.1f;
                        }
                        if (youMustDie)
                        {
                            velocityX *= 1.5f;
                            velocityY *= 1.5f;
                        }
                        NPC.velocity.Y = -velocityY;
                        NPC.velocity.X = velocityX * (float)NPC.direction;
                        NPC.netUpdate = true;
                    }
                    NPC.velocity.Y += 0.4f;
                    if (NPC.velocity.Y > 10f)
                    {
                        NPC.velocity.Y = 10f;
                    }
                    NPC.ai[0] = 1f;
                }
                NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
				NPC.rotation = MathHelper.Clamp(NPC.rotation, -0.1f, 0.1f);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.localAI[3] == 0f)
            {
                NPC.spriteDirection = -NPC.direction;
            }
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Color color24 = NPC.GetAlpha(drawColor);
            Color color25 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
            Texture2D texture2D3 = TextureAssets.Npc[NPC.type].Value;
            int num156 = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            int y3 = num156 * (int)NPC.frameCounter;
            Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            int num157 = 8;
            int num158 = 2;
            int num159 = 1;
            float num160 = 0f;
            int num161 = num159;
            while (((num158 > 0 && num161 < num157) || (num158 < 0 && num161 > num157)) && CalamityConfig.Instance.Afterimages)
            {
                Color color26 = NPC.GetAlpha(color25);
                {
                    goto IL_6899;
                }
                IL_6881:
                num161 += num158;
                continue;
                IL_6899:
                float num164 = (float)(num157 - num161);
                if (num158 < 0)
                {
                    num164 = (float)(num159 - num161);
                }
                color26 *= num164 / ((float)NPCID.Sets.TrailCacheLength[NPC.type] * 1.5f);
                Vector2 value4 = NPC.oldPos[num161];
                float num165 = NPC.rotation;
                Main.spriteBatch.Draw(texture2D3, value4 + NPC.Size / 2f - Main.screenPosition + new Vector2(0, NPC.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, num165 + NPC.rotation * num160 * (float)(num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, NPC.scale, spriteEffects, 0f);
                goto IL_6881;
            }
            var something = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture2D3, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, color24, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, something, 0);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int num621 = 0; num621 < 50; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 100; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.AncientBattleArmorMaterial, 1, 1);
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<GrandScale>(), 1, 1);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<GrandScale>(), Main.expertMode, 3, 1, 1);

            // Mark Great Sand Shark as dead
            CalamityWorld.downedGSS = true;
            CalamityNetcode.SyncWorld();
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * 0.8f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Rabies, 240, true);
            target.AddBuff(BuffID.Bleeding, 240, true);
        }
    }
}
