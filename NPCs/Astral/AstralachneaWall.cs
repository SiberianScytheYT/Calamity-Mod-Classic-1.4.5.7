using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Magic;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Astral
{
    public class AstralachneaWall : ModNPC
    {
        private static Texture2D glowmask;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astralachnea");

            Main.npcFrameCount[NPC.type] = 4;

            if (!Main.dedServ)
                glowmask = ModContent.Request<Texture2D>("CalRD/NPCs/Astral/AstralachneaWallGlow").Value;

            base.SetStaticDefaults();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Rotation = -MathHelper.PiOver2 };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Once regular cave spiders, they have been reduced down to mere sentries for the infection.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.aiStyle = -1;
            NPC.damage = 55;
            NPC.defense = 20;
			NPC.DR_NERD(0.15f);
            NPC.lifeMax = 500;
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/AstralEnemyDeath");
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(0, 0, 20, 0);
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.timeLeft = NPC.activeTime * 2;
            AnimationType = NPCID.BlackRecluseWall;
            Banner = ModContent.NPCType<AstralachneaGround>();
            BannerItem = ModContent.ItemType<AstralachneaBanner>();
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BiomeManagers.Astral>().Type };
            if (CalamityWorld.downedAstrageldon)
            {
                NPC.damage = 90;
                NPC.defense = 30;
                NPC.lifeMax = 750;
            }
        }

        public override void AI()
        {
            CalamityGlobalNPC.DoSpiderWallAI(NPC, ModContent.NPCType<AstralachneaGround>(), (CalamityWorld.death ? 3.6f : 2.4f), (CalamityWorld.death ? 0.15f : 0.1f));
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter += 0.1f;
                NPC.frameCounter %= Main.npcFrameCount[NPC.type];
                NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
                return;
            }
            //DO DUST
            int frame = NPC.frame.Y / frameHeight;
            Rectangle rect = new Rectangle(12, 24, 18, 10);
            Rectangle rect2 = new Rectangle(12, 44, 18, 10);
            switch (frame)
            {
                case 1:
                    rect = new Rectangle(6, 26, 28, 8);
                    rect2 = new Rectangle(6, 44, 28, 8);
                    break;
                case 2:
                    rect = new Rectangle(12, 26, 18, 8);
                    rect2 = new Rectangle(12, 44, 18, 8);
                    break;
                case 3:
                    rect = new Rectangle(16, 24, 16, 10);
                    rect2 = new Rectangle(16, 44, 16, 10);
                    break;
            }
            Dust d = CalamityGlobalNPC.SpawnDustOnNPC(NPC, 80, frameHeight, ModContent.DustType<AstralOrange>(), rect, Vector2.Zero, 0.225f, true);
            Dust d2 = CalamityGlobalNPC.SpawnDustOnNPC(NPC, 80, frameHeight, ModContent.DustType<AstralOrange>(), rect2, Vector2.Zero, 0.225f, true);
            if (d != null)
            {
                d.customData = 0.04f;
            }
            if (d2 != null)
            {
                d2.customData = 0.04f;
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

            CalamityGlobalNPC.DoHitDust(NPC, hit.HitDirection, (Main.rand.Next(0, Math.Max(0, NPC.life)) == 0) ? 5 : ModContent.DustType<AstralEnemy>(), 1f, 4, 22);

            //if dead do gores
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity * 0.3f, Mod.Find<ModGore>("AstralachneaGore" + i).Type);
                    }
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = new Vector2(40f, 40f);
            spriteBatch.Draw(glowmask, NPC.Center - Main.screenPosition - new Vector2(0, 8f), NPC.frame, Color.White * 0.6f, NPC.rotation, origin, 1f, SpriteEffects.None, 0);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120, true);
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), 2, 3);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), Main.expertMode);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AstralachneaStaff>(), CalamityWorld.downedAstrageldon, 7, 1, 1);
        }
    }
}
