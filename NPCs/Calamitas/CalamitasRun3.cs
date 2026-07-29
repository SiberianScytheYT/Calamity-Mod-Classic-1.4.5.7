using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using CalRD.Events;
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.TownNPCs;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs.Calamitas
{
	[AutoloadBossHead]
    public class CalamitasRun3 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Calamitas");
            Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 14f;
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 25;
            NPC.value = Item.buyPrice(0, 15, 0, 0);
			NPC.DR_NERD(0.15f);
			NPC.LifeMaxNERB(28125, 38812, 3900000);
            if (CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive)
            {
                NPC.damage *= 3;
                NPC.defense *= 3;
                NPC.lifeMax *= 3;
                NPC.value *= 2.5f;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[ModContent.BuffType<MarkedforDeath>()] = false;
			NPC.buffImmune[BuffID.Frostburn] = false;
			NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.Daybreak] = false;
            NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
			NPC.buffImmune[BuffID.BoneJavelin] = false;
			NPC.buffImmune[BuffID.SoulDrain] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
			NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = false;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/Calamitas");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(NPC.chaseable);
            for (int i = 0; i < 2; i++)
                writer.Write(NPC.Calamity().newAI[i]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			NPC.chaseable = reader.ReadBoolean();
            for (int i = 0; i < 2; i++)
                NPC.Calamity().newAI[i] = reader.ReadSingle();
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
			CalamityAI.CalamitasCloneAI(NPC, Mod, true);
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

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/Calamitas/CalamitasRun3Glow").Value;
			Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

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

		public override void OnKill()
        {
            DropHelper.DropBags(ModContent.ItemType<CalamitasBag>(), NPC);

            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.BrokenHeroSword, true);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CalamitasTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeCalamitasClone>(), !CalamityWorld.downedCalamitas);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedCalamitas, 4, 2, 1);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { ModContent.NPCType<THIEF>() }, CalamityWorld.downedCalamitas);

			if (!Main.expertMode)
            {
				//Materials
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofChaos>(), 4, 8);
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CalamityDust>(), 9, 14);
                DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<BlightedLens>(), 1, 2);
				DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 1f, 30, 40);

                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<TheEyeofCalamitas>(w),
                    DropHelper.WeightStack<Animosity>(w),
                    DropHelper.WeightStack<CalamitasInferno>(w),
                    DropHelper.WeightStack<BlightedEyeStaff>(w)
                );

                // Equipment
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ChaosStone>(), 10);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CalamitasMask>(), 7);
            }

            // Abyss awakens after killing Calamitas
            string key = "The ocean depths are trembling.";
            Color messageColor = Color.RoyalBlue;

            if (!CalamityWorld.downedCalamitas)
            {
                if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/WyrmScream"), Main.player[Main.myPlayer].position);

                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }

            // Mark Calamitas as dead
            CalamityWorld.downedCalamitas = true;
            CalamityNetcode.SyncWorld();
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Calamitas Clone";
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
	            if (Main.netMode != NetmodeID.Server)
	            {
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Calamitas").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Calamitas2").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Calamitas3").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Calamitas4").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Calamitas5").Type, 1f);
		            Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Calamitas6").Type, 1f);
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

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
			NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, true);
        }
    }
}
