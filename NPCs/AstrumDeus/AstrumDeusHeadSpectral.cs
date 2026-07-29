using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.LoreItems;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Placeables.Furniture.Trophies;
using CalRD.Items.TreasureBags;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
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

namespace CalRD.NPCs.AstrumDeus
{
    [AutoloadBossHead]
    public class AstrumDeusHeadSpectral : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astrum Deus");
			NPCID.Sets.TrailingMode[NPC.type] = 1;
		}

        public override void SetDefaults()
        {
			NPC.GetNPCDamage();
			NPC.npcSlots = 5f;
            NPC.width = 56;
            NPC.height = 56;
            NPC.defense = 25;
			NPC.LifeMaxNERB(187500, 225000, 6500000);
			double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.scale = 1.2f;
            if (Main.expertMode)
            {
                NPC.scale = 1.35f;
            }
            NPC.boss = true;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/AstrumDeusDeath");
            NPC.netAlways = true;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/AstrumDeus");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
            for (int i = 0; i < 4; i++)
            {
                writer.Write(NPC.Calamity().newAI[i]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
            for (int i = 0; i < 4; i++)
            {
                NPC.Calamity().newAI[i] = reader.ReadSingle();
            }
        }

        public override void AI()
        {
			CalamityAI.AstrumDeusAI(NPC, Mod, true);
		}

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2D16 = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumDeus/AstrumDeusHeadGlow2").Value;
			Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / 2);
			Color color36 = Color.White;
			float amount9 = 0.5f;
			int num153 = 5;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num155 = 1; num155 < num153; num155 += 2)
				{
					Color color38 = drawColor;
					color38 = Color.Lerp(color38, color36, amount9);
					color38 = NPC.GetAlpha(color38);
					color38 *= (num153 - num155) / 15f;
					Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector41 -= new Vector2(texture2D15.Width, texture2D15.Height) * NPC.scale / 2f;
					vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 vector43 = NPC.Center - Main.screenPosition;
			vector43 -= new Vector2(texture2D15.Width, texture2D15.Height) * NPC.scale / 2f;
			vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
			spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			texture2D15 = ModContent.Request<Texture2D>("CalRD/NPCs/AstrumDeus/AstrumDeusHeadGlow").Value;
			Color phaseColor = NPC.Calamity().newAI[3] >= 600f ? Color.Cyan : Color.Orange;
			Color color37 = Color.Lerp(Color.White, NPC.Calamity().newAI[0] != 0f ? phaseColor : Color.Cyan, 0.5f) * NPC.Opacity;
			Color color42 = Color.Lerp(Color.White, NPC.Calamity().newAI[0] != 0f ? phaseColor : Color.Orange, 0.5f) * NPC.Opacity;

			if (CalamityConfig.Instance.Afterimages)
			{
				for (int num163 = 1; num163 < num153; num163++)
				{
					Color color41 = color37;
					color41 = Color.Lerp(color41, color36, amount9);
					color41 *= (num153 - num163) / 15f;
					Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
					vector44 -= new Vector2(texture2D15.Width, texture2D15.Height) * NPC.scale / 2f;
					vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
					spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

					Color color43 = color42;
					color43 = Color.Lerp(color43, color36, amount9);
					color43 *= (num153 - num163) / 15f;
					spriteBatch.Draw(texture2D16, vector44, NPC.frame, color43, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
				}
			}

			spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			spriteBatch.Draw(texture2D16, vector43, NPC.frame, color42, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

			return false;
        }

		public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 50;
                NPC.height = 50;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 5; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.PurpleCosmolite, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 10; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType) => potionType = ModContent.ItemType<Stardust>();

        public override bool SpecialOnKill()
        {
			if (NPC.Calamity().newAI[0] == 0f)
				return false;

			int closestSegmentID = DropHelper.FindClosestWormSegment(NPC.GetSource_FromThis(), NPC,
                ModContent.NPCType<AstrumDeusHeadSpectral>(),
                ModContent.NPCType<AstrumDeusBodySpectral>(),
                ModContent.NPCType<AstrumDeusTailSpectral>());
            NPC.position = Main.npc[closestSegmentID].position;
            return false;
        }

        public override void OnKill()
        {
			// Unsplit Deus does not drop anything when killed/despawned.
            if (NPC.Calamity().newAI[0] == 0f)
				return;

            // Killing ANY split Deus makes all other Deus heads die immediately.
            for (int i = 0; i < Main.maxNPCs; ++i)
			{
                NPC otherWormHead = Main.npc[i];
                if (otherWormHead.active && otherWormHead.type == NPC.type)
                {
                    // Kill the other worm head after setting it to not drop loot.
                    otherWormHead.Calamity().newAI[0] = 0f;
                    otherWormHead.life = 0;
                    otherWormHead.checkDead();
                }
			} 

			DropHelper.DropBags(ModContent.ItemType<AstrumDeusBag>(), NPC);

            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.GreaterHealingPotion, 8, 14);
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AstrumDeusTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeAstrumDeus>(), !CalamityWorld.downedStarGod);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeAstralInfection>(), !CalamityWorld.downedStarGod);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedStarGod, 4, 2, 1);

            // Drop a large spray of all 4 lunar fragments
            int minFragments = Main.expertMode ? 20 : 12;
            int maxFragments = Main.expertMode ? 32 : 20;
            DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.FragmentSolar, minFragments, maxFragments);
            DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.FragmentVortex, minFragments, maxFragments);
            DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.FragmentNebula, minFragments, maxFragments);
            DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.FragmentStardust, minFragments, maxFragments);

            // All other drops are contained in the bag, so they only drop directly on Normal
            if (!Main.expertMode)
            {
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), 50, 80, 5);
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ItemID.FallenStar, 80, 150);

                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<TheMicrowave>(w),
                    DropHelper.WeightStack<StarSputter>(w),
                    DropHelper.WeightStack<Starfall>(w),
                    DropHelper.WeightStack<GodspawnHelixStaff>(w),
                    DropHelper.WeightStack<RegulusRiot>(w)
                );

                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Quasar>(), DropHelper.RareVariantDropRateInt);

                // Equipment
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<HideofAstrumDeus>(), DropHelper.RareVariantDropRateInt);
				DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<ChromaticOrb>(), 5);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AstrumDeusMask>(), 7);
            }

            // Notify players that Astral Ore can be mined if Deus has never been killed yet
            if (!CalamityWorld.downedStarGod)
            {
                string key = "The seal of the stars has been broken! You can now mine Astral Ore.";
                Color messageColor = Color.Gold;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }

            // Mark Astrum Deus as dead
            CalamityWorld.downedStarGod = true;
            CalamityNetcode.SyncWorld();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 240, true);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
        }
    }
}
