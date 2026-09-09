using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace CalRD.NPCs.Astral
{
    public class Hive : ModNPC
    {
        private static Texture2D glowmask;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hive");
            if (!Main.dedServ)
                glowmask = ModContent.Request<Texture2D>("CalRD/NPCs/Astral/HiveGlow").Value;
            Main.npcFrameCount[NPC.type] = 6;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("An amalgamation of creatures, these hives will split off small chunks of themselves to attack enemies.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 60;
            NPC.aiStyle = -1;
            NPC.damage = 55;
            NPC.defense = 15;
			NPC.DR_NERD(0.15f);
            NPC.lifeMax = 470;
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/AstralEnemyDeath");
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 15, 0);
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<HiveBanner>();
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BiomeManagers.Astral>().Type };
            if (CalamityWorld.downedAstrageldon)
            {
                NPC.damage = 90;
                NPC.defense = 25;
                NPC.lifeMax = 700;
            }
        }

        public override void AI()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] > (CalamityWorld.death ? 120f : 180f))
            {
                if (Main.rand.NextBool(100) && NPC.CountNPCS(ModContent.NPCType<Hiveling>()) < 10)
                {
                    NPC.ai[0] = 0;

                    //spawn hiveling, it's ai[0] is the hive npc index.
                    int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Hiveling>(), 0, NPC.whoAmI);
                    Main.npc[n].velocity.X = Main.rand.NextFloat(-0.4f, 0.4f);
                    Main.npc[n].velocity.Y = Main.rand.NextFloat(-0.5f, -0.05f);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 10)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > frameHeight * 4)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //draw glowmask
            spriteBatch.Draw(glowmask, NPC.Center - Main.screenPosition, NPC.frame, Color.White * 0.6f, NPC.rotation, new Vector2(19, 30), 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
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

            CalamityGlobalNPC.DoHitDust(NPC, hit.HitDirection, (Main.rand.Next(0, Math.Max(0, NPC.life)) == 0) ? 5 : ModContent.DustType<AstralEnemy>(), 1f, 3, 20);

            //if dead do gores
            if (NPC.life <= 0)
            {
                int type = ModContent.NPCType<Hiveling>();
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].type == type)
                    {
                        Main.npc[i].ai[0] = -1f;
                    }
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalamityGlobalNPC.AnyEvents(spawnInfo.Player) || NPC.AnyNPCs(NPC.type))
            {
                return 0f;
            }
            else if (spawnInfo.Player.InAstral(2))
            {
                return 0.17f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120, true);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Stardust>(), 1, 2, 3);
            npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<Stardust>());
            npcLoot.AddIf(() => CalamityWorld.downedAstrageldon, ModContent.ItemType<HivePod>(), 7, 1, 1);
        }
    }
}
