using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Placeables.Ores;
using CalRD.Items.Weapons.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Astral
{
    public class AstralSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.damage = 40;
            NPC.width = 36;
            NPC.height = 31;
            NPC.aiStyle = 1;
            NPC.defense = 8;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.alpha = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.BlueSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AstralSlimeBanner>();
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            if (CalamityWorld.downedAstrageldon)
            {
                NPC.damage = 65;
                NPC.defense = 18;
                NPC.lifeMax = 310;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            //DO DUST
            Dust d = CalamityGlobalNPC.SpawnDustOnNPC(NPC, 44, frameHeight, ModContent.DustType<AstralOrange>(), new Rectangle(4, 4, 36, 24), Vector2.Zero, 0.15f, true);
            if (d != null)
            {
                d.customData = 0.04f;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            CalamityGlobalNPC.DoHitDust(NPC, hit.HitDirection, ModContent.DustType<AstralOrange>(), 1f, 4, 24);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            else if (spawnInfo.Player.InAstral(1))
            {
                return 0.21f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120, true);
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), Main.expertMode ? 1 : 2, 1, 3);
            int oreMin = Main.expertMode ? 11 : 8;
            int oreMax = Main.expertMode ? 16 : 12;
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AstralOre>(), CalamityWorld.downedStarGod, oreMin, oreMax);
			float slimeStaffDrop = (CalamityWorld.defiled ? DropHelper.DefiledDropRateFloat : 0.03f);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AbandonedSlimeStaff>(), CalamityWorld.downedAstrageldon, slimeStaffDrop, 1, 1);
        }
    }
}
