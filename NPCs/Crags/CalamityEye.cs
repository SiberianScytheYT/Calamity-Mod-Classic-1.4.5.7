using CalRD.BiomeManagers;
using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.Crags
{
	public class CalamityEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Calamity Eye");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers();
            value.Position.Y -= 10f;
            value.PortraitPositionYOverride = -36f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
	        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
	        {
		        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
		        new FlavorTextBestiaryInfoElement("what a calamity")
	        });
        }

        public override void SetDefaults()
        {
            NPC.lavaImmune = true;
            NPC.aiStyle = 2;
            NPC.damage = 40;
            NPC.width = 30;
            NPC.height = 30;
            NPC.defense = 12;
            NPC.lifeMax = 140;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.DemonEye;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            if (CalamityWorld.downedProvidence)
            {
                NPC.damage = 80;
                NPC.defense = 20;
                NPC.lifeMax = 3000;
            }
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CalamityEyeBanner>();
			NPC.buffImmune[BuffID.Confused] = false;
			SpawnModBiomes = new int[] { ModContent.GetInstance<Crag>().Type };
        }

        public override void AI()
        {
			if (NPC.life < NPC.lifeMax * 0.5)
			{
				if (NPC.direction == -1 && NPC.velocity.X > -6f)
				{
					NPC.velocity.X -= 0.1f;
					if (NPC.velocity.X > 6f)
						NPC.velocity.X -= 0.1f;
					else if (NPC.velocity.X > 0f)
						NPC.velocity.X += 0.05f;
					if (NPC.velocity.X < -6f)
						NPC.velocity.X = -6f;
				}
				else if (NPC.direction == 1 && NPC.velocity.X < 6f)
				{
					NPC.velocity.X += 0.1f;
					if (NPC.velocity.X < -6f)
						NPC.velocity.X += 0.1f;
					else if (NPC.velocity.X < 0f)
						NPC.velocity.X -= 0.05f;
					if (NPC.velocity.X > 6f)
						NPC.velocity.X = 6f;
				}
				if (NPC.directionY == -1 && NPC.velocity.Y > -4f)
				{
					NPC.velocity.Y -= 0.1f;
					if (NPC.velocity.Y > 4f)
						NPC.velocity.Y -= 0.1f;
					else if (NPC.velocity.Y > 0f)
						NPC.velocity.Y += 0.05f;
					if (NPC.velocity.Y < -4f)
						NPC.velocity.Y = -4f;
				}
				else if (NPC.directionY == 1 && NPC.velocity.Y < 4f)
				{
					NPC.velocity.Y += 0.1f;
					if (NPC.velocity.Y < -4f)
						NPC.velocity.Y += 0.1f;
					else if (NPC.velocity.Y < 0f)
						NPC.velocity.Y -= 0.05f;
					if (NPC.velocity.Y > 4f)
						NPC.velocity.Y = 4f;
				}
			}
			if (Main.rand.NextBool(40))
			{
				int index = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + NPC.height * 0.25f), NPC.width, (int)(NPC.height * 0.5), DustID.Blood, NPC.velocity.X, 2f, 0, new Color(), 1f);
				Main.dust[index].velocity.X *= 0.5f;
				Main.dust[index].velocity.Y *= 0.1f;
			}
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.Calamity().ZoneCalamity ? 0.25f : 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Weak, 120, true);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120, true);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => CalamityWorld.downedProvidence, ModContent.ItemType<Bloodstone>(), 2);
            npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<EssenceofChaos>(), 3);
            npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<BlightedLens>(), 2);
            npcLoot.Add(ItemID.Lens, 2);
        }
    }
}
