using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class CrimulanBlightSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crimulan Blight Slime");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 1;
			AIType = NPCID.DungeonSlime;
			NPC.damage = 30;
            NPC.width = 60;
            NPC.height = 42;
            NPC.defense = 8;
            NPC.lifeMax = 130;
            NPC.knockBackResist = 0.3f;
            AnimationType = NPCID.RainbowSlime;
            NPC.value = Item.buyPrice(0, 0, 2, 0);
            NPC.alpha = 105;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Confused] = false;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CrimulanBlightSlimeBanner>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneAbyss)
            {
                return 0f;
            }
            return SpawnCondition.Crimson.Chance * 0.15f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 40; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.ManaSickness, 120, true);
            target.AddBuff(BuffID.Confused, 120, true);
        }

        public override void OnKill()
        {
            int item = Item.NewItem(NPC.GetSource_FromThis(), NPC.Center, NPC.Size, ModContent.ItemType<EbonianGel>(), Main.rand.Next(15, 21), false, 0, false, false);
            Main.item[item].notAmmo = true;
            NetMessage.SendData(MessageID.ItemTweaker, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);

            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ItemID.Gel, 10, 14);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Carnage>(), NPC.downedBoss3, 0.01f, 1, 1);
        }
    }
}
