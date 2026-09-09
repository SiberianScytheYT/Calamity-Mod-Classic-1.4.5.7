using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Melee;
using System.IO;
using CalRD.BiomeManagers;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRD.NPCs.Abyss
{
	public class MorayEel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moray Eel");
            Main.npcFrameCount[NPC.type] = 6;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("These eels prefer to remain hidden within underwater structures, only ocassionally roaming open water.")
            });
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.damage = 55;
            NPC.width = 136;
            NPC.height = 40;
            NPC.defense = 8;
            NPC.lifeMax = 360;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.buffImmune[ModContent.BuffType<CrushDepth>()] = true;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.75f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<MorayEelBanner>();
            NPC.chaseable = false;
            SpawnModBiomes = new int[] { ModContent.GetInstance<AbyssLayer1Biome>().Type };
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            CalamityAI.PassiveSwimmingAI(NPC, Mod, 0, Main.player[NPC.target].Calamity().GetAbyssAggro(300f, 250f), 0.15f, 0.1f, 6f, 4f, 0.1f);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return NPC.chaseable;
            }
            return null;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += NPC.chaseable ? 0.15f : 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 180, true);
            target.AddBuff(BuffID.Venom, 90, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneSulphur)
            {
                return 0f;
            }
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer1 && spawnInfo.Water)
            {
                return SpawnCondition.CaveJellyfish.Chance * 1.2f;
            }
            return SpawnCondition.OceanMonster.Chance * 0.1f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<DepthBlade>(), 10);
            npcLoot.AddIf(() => !Main.expertMode, ModContent.ItemType<DepthBlade>(), 15);
            npcLoot.Add(ItemID.Flipper, 20);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 25; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
