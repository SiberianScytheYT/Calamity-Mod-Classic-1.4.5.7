using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Abyss
{
    public class GulperEelBodyAlt : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gulper Eel");
        }

        public override void SetDefaults()
        {
            NPC.damage = 90; //70
            NPC.width = 36; //36
            NPC.height = 36;
            NPC.defense = 80;
            NPC.lifeMax = 80000;
            NPC.aiStyle = -1; //new
            AIType = -1; //new
            NPC.knockBackResist = 0f;
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit9;
            NPC.DeathSound = SoundID.NPCDeath13;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
            NPC.chaseable = false;
            Banner = ModContent.NPCType<GulperEelHead>();
            BannerItem = ModContent.ItemType<GulperEelBanner>();
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void AI()
        {
            if (NPC.ai[3] > 0f)
            {
                NPC.realLife = (int)NPC.ai[3];
            }
            bool flag = false;
            if (NPC.ai[1] <= 0f)
            {
                flag = true;
            }
            else if (Main.npc[(int)NPC.ai[1]].life <= 0)
            {
                flag = true;
            }
            if (flag)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
            }
            if (!NPC.AnyNPCs(ModContent.NPCType<GulperEelHead>()))
            {
                NPC.active = false;
            }
            if (Main.npc[(int)NPC.ai[1]].alpha < 128)
            {
                NPC.alpha -= 42;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
            }
            Vector2 vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num191 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
            float num192 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
            num191 = (float)((int)(num191 / 16f) * 16);
            num192 = (float)((int)(num192 / 16f) * 16);
            vector18.X = (float)((int)(vector18.X / 16f) * 16);
            vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
            num191 -= vector18.X;
            num192 -= vector18.Y;
            float num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
            if (NPC.ai[1] > 0f && NPC.ai[1] < (float)Main.npc.Length)
            {
                try
                {
                    vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    num191 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector18.X;
                    num192 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector18.Y;
                } catch
                {
                }
                NPC.rotation = (float)System.Math.Atan2((double)num192, (double)num191) + 1.57f;
                num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
                int num194 = NPC.width;
                num193 = (num193 - (float)num194) / num193;
                num191 *= num193;
                num192 *= num193;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + num191;
                NPC.position.Y = NPC.position.Y + num192;
                if (num191 < 0f)
                {
                    NPC.spriteDirection = -1;
                }
                else if (num191 > 0f)
                {
                    NPC.spriteDirection = 1;
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 vector11 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
            Vector2 vector = center - Main.screenPosition;
            vector -= new Vector2((float)ModContent.Request<Texture2D>("CalRD/NPCs/Abyss/GulperEelBodyAltGlow").Value.Width, (float)(ModContent.Request<Texture2D>("CalRD/NPCs/Abyss/GulperEelBodyAltGlow").Value.Height / Main.npcFrameCount[NPC.type])) * 1f / 2f;
            vector += vector11 * 1f + new Vector2(0f, 0f + 4f + NPC.gfxOffY);
            Color color = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.LightYellow);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/NPCs/Abyss/GulperEelBodyAltGlow").Value, vector,
                new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, vector11, 1f, spriteEffects, 0f);
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
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GulperEel2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("GulperEel3").Type, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 120, true);
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 120, true);
        }
    }
}
