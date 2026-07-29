using CalRD.Dusts;
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.Weapons.Melee;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.CeaselessVoid
{
    [AutoloadBossHead]
    public class CeaselessVoid : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ceaseless Void");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}
        
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
	        modifiers.SetMaxDamage(1);
        }

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 36f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.defense = 0;
            CalamityGlobalNPC global = NPC.Calamity();
            global.DR = 0.999999f;
            //global.unbreakableDR = true;
            NPC.lifeMax = 200;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/ScourgeofTheUniverse");
            if (CalamityWorld.DoGSecondStageCountdown <= 0)
            {
                NPC.value = Item.buyPrice(0, 35, 0, 0);
                Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Void");
            }
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.DeathSound = SoundID.NPCDeath14;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
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
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/CeaselessVoid/CeaselessVoidGlow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Cyan, 0.5f);

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 *= (float)(num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2((float)texture2D15.Width, (float)(texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
		}

        public override void AI()
        {
			CalamityAI.CeaselessVoidAI(NPC, Mod);
        }

        public override void OnKill()
        {
            // Only drop items if fought alone
            if (CalamityWorld.DoGSecondStageCountdown <= 0)
            {
                // Materials
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DarkPlasma>(), true, 2, 3);

                // Weapons
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<MirrorBlade>(), Main.expertMode ? 3 : 4);

                // Equipment
				DropHelper.DropItemRIV(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ArcanumoftheVoid>(), ModContent.ItemType<TheEvolution>(), 0.2f, DropHelper.RareVariantDropRateFloat);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CeaselessVoidTrophy>(), 10);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CeaselessVoidMask>(), 7);
				if (Main.rand.NextBool(20))
				{
					DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AncientGodSlayerHelm>());
					DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AncientGodSlayerChestplate>());
					DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AncientGodSlayerLeggings>());
				}

                // Other
                bool lastSentinelKilled = !CalamityWorld.downedSentinel1 && CalamityWorld.downedSentinel2 && CalamityWorld.downedSentinel3;
                DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeSentinels>(), true, lastSentinelKilled);
                DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedSentinel1, 5, 2, 1);
            }

            // If DoG's fight is active, set the timer for the remaining two sentinels
            else if (CalamityWorld.DoGSecondStageCountdown > 14460)
            {
                CalamityWorld.DoGSecondStageCountdown = 14460;
                if (Main.netMode == NetmodeID.Server)
                {
                    var netMessage = Mod.GetPacket();
                    netMessage.Write((byte)CalRDMessageType.DoGCountdownSync);
                    netMessage.Write(CalamityWorld.DoGSecondStageCountdown);
                    netMessage.Send();
                }
            }

			// Mark Ceaseless Void as dead
			if (CalamityWorld.DoGSecondStageCountdown <= 0)
			{
				CalamityWorld.downedSentinel1 = true;
				CalamityNetcode.SyncWorld();
			}
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 8;
                SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/OtherworldlyHit"), NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, hit.HitDirection, -1f, 0, default, 1f);
            }
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
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }

                if (Main.netMode != NetmodeID.Server)
                {
	                float randomSpread = (float)(Main.rand.Next(-200, 200) / 100);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CeaselessVoid").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CeaselessVoid2").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CeaselessVoid2").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CeaselessVoid3").Type, 1f);
	                Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("CeaselessVoid3").Type, 1f);
                }
            }
        }
    }
}
