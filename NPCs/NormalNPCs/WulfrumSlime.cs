using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.NormalNPCs
{
	public class WulfrumSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Wulfrum Slime");
			Main.npcFrameCount[NPC.type] = 2;
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("you know, you shouldn't even be here")
			});
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = 1;
			NPC.damage = 8;
			NPC.width = 30;
			NPC.height = 22;
			NPC.defense = 2;
			NPC.lifeMax = 12;
			NPC.knockBackResist = 0f;
			AnimationType = NPCID.CorruptSlime;
			NPC.value = Item.buyPrice(0, 0, 0, 30);
			NPC.alpha = 60;
			NPC.lavaImmune = false;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<WulfrumSlimeBanner>();
			NPC.buffImmune[BuffID.Confused] = false;
		}

		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float pylonMult = NPC.AnyNPCs(ModContent.NPCType<WulfrumPylon>()) ? 2f : 1f;
			if (spawnInfo.playerSafe || spawnInfo.player.Calamity().ZoneSulphur)
			{
				return 0f;
			}
			return SpawnCondition.OverworldDaySlime.Chance * (Main.hardMode ? 0.08f : 0.33f) * pylonMult;
		}*/

		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int k = 0; k < 3; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 3, hit.HitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 15; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 3, hit.HitDirection, -1f, 0, default, 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ModContent.ItemType<WulfrumShard>());
			npcLoot.AddIf(() => Main.expertMode, ModContent.ItemType<WulfrumShard>(), 2);
		}
	}
}
