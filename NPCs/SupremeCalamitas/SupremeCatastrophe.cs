using CalRD.Dusts;
using CalRD.Projectiles.Boss;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SupremeCalamitas
{
	[AutoloadBossHead]
    public class SupremeCatastrophe : ModNPC
    {
		private const int distanceX = 750;
		private int distanceY = 375;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Catastrophe");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.65f,
                PortraitScale = 0.65f
            };
            value.Position.Y -= 10f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
		}
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("The Witch's revived brother, Catastrophe. He is a shell of his former self however.")
            });
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.npcSlots = 5f;
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 100;
			NPC.DR_NERD(0.7f, 0.7f, 0.75f, 0.6f, true);
			CalamityGlobalNPC global = NPC.Calamity();
            global.multDRReductions.Add(BuffID.CursedInferno, 0.9f);
			NPC.LifeMaxNERB(1200000, 1500000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(distanceY);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            distanceY = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            CalamityGlobalNPC.SCalCatastrophe = NPC.whoAmI;
            bool expertMode = Main.expertMode;
            if (CalamityGlobalNPC.SCal < 0 || !Main.npc[CalamityGlobalNPC.SCal].active)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }
            NPC.TargetClosest(true);
            float num676 = 60f;
            float num677 = 1.5f;

			// Reduce acceleration if target is holding a true melee weapon
			Item targetSelectedItem = Main.player[NPC.target].inventory[Main.player[NPC.target].selectedItem];
			if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityLists.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
			{
				num677 *= 0.5f;
			}

			bool deadBrother = !NPC.AnyNPCs(ModContent.NPCType<SupremeCataclysm>());
			int scale = deadBrother ? 5 : 2;
            if (NPC.ai[3] < distanceX)
            {
                NPC.ai[3] += scale;
                distanceY -= scale;
            }
            else if (NPC.ai[3] < distanceX * 2)
            {
                NPC.ai[3] += scale;
                distanceY += scale;
            }
            else
                NPC.ai[3] = 0f;

            Vector2 vector83 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num678 = Main.player[NPC.target].Center.X - vector83.X - distanceX;
            float num679 = Main.player[NPC.target].Center.Y - vector83.Y + distanceY;
            NPC.rotation = MathHelper.PiOver2 * 3f;
            float num680 = (float)Math.Sqrt((double)(num678 * num678 + num679 * num679));
            num680 = num676 / num680;
            num678 *= num680;
            num679 *= num680;
            if (NPC.velocity.X < num678)
            {
                NPC.velocity.X = NPC.velocity.X + num677;
                if (NPC.velocity.X < 0f && num678 > 0f)
                {
                    NPC.velocity.X = NPC.velocity.X + num677;
                }
            }
            else if (NPC.velocity.X > num678)
            {
                NPC.velocity.X = NPC.velocity.X - num677;
                if (NPC.velocity.X > 0f && num678 < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X - num677;
                }
            }
            if (NPC.velocity.Y < num679)
            {
                NPC.velocity.Y = NPC.velocity.Y + num677;
                if (NPC.velocity.Y < 0f && num679 > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y + num677;
                }
            }
            else if (NPC.velocity.Y > num679)
            {
                NPC.velocity.Y = NPC.velocity.Y - num677;
                if (NPC.velocity.Y > 0f && num679 < 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - num677;
                }
            }
            if (NPC.localAI[0] < 120f)
            {
                NPC.localAI[0] += 1f;
            }
            if (NPC.localAI[0] >= 120f)
            {
                NPC.ai[1] += 1f;
				if (deadBrother)
				{
					NPC.ai[1] += 1f;
				}
				if (NPC.ai[1] >= 45f)
                {
                    NPC.ai[1] = 0f;
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneHellblastSound"), NPC.Center);
					int type = ModContent.ProjectileType<BrimstoneHellblast2>();
					int damage = NPC.GetProjectileDamage(type);
					if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(4f, 0f), type, damage, 0f, Main.myPlayer);
                    }
                }
                NPC.ai[2] += 1f;
                if (deadBrother)
                {
                    NPC.ai[2] += 2f;
                }
                if (NPC.ai[2] >= 300f)
                {
                    NPC.ai[2] = 0f;
                    float speed = 7f;
					int type = ModContent.ProjectileType<BrimstoneBarrage>();
					int damage = NPC.GetProjectileDamage(type);
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneShoot"), NPC.Center);
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(NPC.velocity.X, NPC.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * speed), (float)(Math.Cos(offsetAngle) * speed), type, damage, 0f, Main.myPlayer, 0f, 1f);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * speed), (float)(-Math.Cos(offsetAngle) * speed), type, damage, 0f, Main.myPlayer, 0f, 1f);
                        }
                    }
                    for (int dust = 0; dust <= 5; dust++)
                    {
                        Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f);
                    }
                }
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ModContent.ProjectileType<SonOfYharon>())
            {
                projectile.damage /= 2;
            }
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.ModifyHitInfo += NoDamage;
        }
        
        public void NoDamage(ref NPC.HitInfo hit)
        {
            if (hit.Damage >= NPC.lifeMax * 0.5f)
            {
                hit.Damage = 0;
            }
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 7;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (float)(num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
					vector41 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - screenPos;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/SupremeCalamitas/SupremeCatastropheGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 *= (float)(num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
					vector44 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

		public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
                NPC.width = 100;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
    }
}
