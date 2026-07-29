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
using CalRD.Items.Weapons.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.NPCs.BrimstoneElemental
{
	[AutoloadBossHead]
    public class BrimstoneElemental : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Elemental");
            Main.npcFrameCount[NPC.type] = 12;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 64f;
			NPC.GetNPCDamage();
			NPC.width = 100;
            NPC.height = 150;
            NPC.defense = 15;
            NPC.value = Item.buyPrice(0, 12, 0, 0);
            NPC.LifeMaxNERB(30000, 41000, 6500000);
			NPC.DR_NERD(0.15f);
            if (CalamityWorld.downedProvidence && !BossRushEvent.BossRushActive)
            {
                NPC.damage *= 3;
                NPC.defense *= 4;
                NPC.lifeMax *= 8;
                NPC.value *= 3f;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
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
            NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit23;
            NPC.DeathSound = SoundID.NPCDeath39;
            Music = MusicLoader.GetMusicSlot("CalRD/Sounds/Music/LeftAlone");
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.chaseable);
			writer.Write(NPC.localAI[0]);
			writer.Write(NPC.localAI[1]);
            for (int i = 0; i < 3; i++)
                writer.Write(NPC.Calamity().newAI[i]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.chaseable = reader.ReadBoolean();
			NPC.localAI[0] = reader.ReadSingle();
			NPC.localAI[1] = reader.ReadSingle();
            for (int i = 0; i < 3; i++)
                NPC.Calamity().newAI[i] = reader.ReadSingle();
		}

        public override void AI()
        {
			CalamityAI.BrimstoneElementalAI(NPC, Mod);
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 180, true);
        }

        public override void FindFrame(int frameHeight) //9 total frames
        {
            NPC.frameCounter += 1.0;
            if (NPC.ai[0] <= 2f)
            {
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y >= frameHeight * 4)
                {
                    NPC.frame.Y = 0;
                }
            }
            else if (NPC.ai[0] == 3f || NPC.ai[0] == 5f)
            {
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y < frameHeight * 4)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
                if (NPC.frame.Y >= frameHeight * 8)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
            }
            else
            {
                if (NPC.frameCounter > 12.0)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0.0;
                }
                if (NPC.frame.Y < frameHeight * 8)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
                if (NPC.frame.Y >= frameHeight * 12)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void OnKill()
        {
            DropHelper.DropBags(ModContent.ItemType<BrimstoneWaifuBag>(), NPC);

            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<BrimstoneElementalTrophy>(), 10);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeBrimstoneCrag>(), true, !CalamityWorld.downedBrimstoneElemental);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<KnowledgeBrimstoneElemental>(), true, !CalamityWorld.downedBrimstoneElemental);
            DropHelper.DropResidentEvilAmmo(NPC.GetSource_FromThis(), NPC, CalamityWorld.downedBrimstoneElemental, 4, 2, 1);

			CalamityGlobalTownNPC.SetNewShopVariable(new int[] { NPCID.Wizard }, CalamityWorld.downedBrimstoneElemental);

			if (!Main.expertMode)
            {
				//Materials
                DropHelper.DropItemSpray(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EssenceofChaos>(), 4, 8);
				DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 1f, 20, 30);

                // Weapons
                float w = DropHelper.DirectWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC.GetSource_FromThis(), NPC,
                    DropHelper.WeightStack<Brimlance>(w),
                    DropHelper.WeightStack<SeethingDischarge>(w),
                    DropHelper.WeightStack<DormantBrimseeker>(w)
                );

                // Equipment
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<RoseStone>(), 10);
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Abaddon>(), 2);

                // Vanity
                DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<BrimstoneWaifuMask>(), 7);
            }

			//if brimmy hasn't been killed, you can mine charred ore
            string key2 = "A protective spell has been lifted from the crags! You can now mine Charred Ore.";
            Color messageColor2 = Color.Crimson;
            if (!CalamityWorld.downedBrimstoneElemental)
                CalamityUtils.DisplayLocalizedText(key2, messageColor2);

            // mark brimmy as dead
            CalamityWorld.downedBrimstoneElemental = true;
            CalamityNetcode.SyncWorld();
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: balance -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 235, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
                NPC.width = 200;
                NPC.height = 150;
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
                for (int num623 = 0; num623 < 60; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }

                if (Main.netMode != NetmodeID.Server)
                {
                    float randomSpread = (float)(Main.rand.Next(-200, 200) / 100);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("BrimstoneGore1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("BrimstoneGore2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("BrimstoneGore3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity * randomSpread, Mod.Find<ModGore>("BrimstoneGore4").Type, 1f);
                }
            }
        }
    }
}
