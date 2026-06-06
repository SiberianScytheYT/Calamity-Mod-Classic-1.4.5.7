using CalRD.Dusts;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Accessories;
using CalRD.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.NPCs.AcidRain
{
    public class IrradiatedSlime : ModNPC
    {
        public bool Falling = true;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Irradiated Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 30;

            NPC.damage = 42;
            NPC.lifeMax = 220;
            NPC.defense = 5;

            NPC.knockBackResist = 0f;
            AnimationType = NPCID.CorruptSlime;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.alpha = 50;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<IrradiatedSlimeBanner>();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Falling);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Falling = reader.ReadBoolean();
        }

        public override void AI()
        {
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.6f, 0.8f, 0.6f);
			if (Falling)
			{
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
				NPC.aiStyle = AIType = -1;

				NPC.noTileCollide = NPC.noGravity = true;
                if (player.Top.Y < NPC.Bottom.Y)
                {
                    NPC.noTileCollide = NPC.noGravity = false;
                    Falling = false;
                    NPC.netUpdate = true;
                }
                else
                {
                    NPC.velocity = Vector2.UnitY * 6f;
                }
			}
			else
			{
				NPC.aiStyle = 1;
				AIType = NPCID.ToxicSludge;
			}
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
                    }
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("IrradiatedSlime").Type, 1f);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("IrradiatedSlime2").Type, 1f);
                }
            }
        }

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			CalamityGlobalNPC.DrawGlowmask(NPC, spriteBatch, ModContent.Request<Texture2D>(Texture + "Glow").Value);
		}

		public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<LeadCore>(), 30);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }
    }
}
