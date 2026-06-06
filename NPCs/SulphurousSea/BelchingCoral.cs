using CalRD.Dusts;
using CalRD.Projectiles.Enemy;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Magic;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SulphurousSea
{
	public class BelchingCoral : ModNPC
    {
        public const float CheckDistance = 480f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Belching Coral");
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.damage = 45;
            NPC.width = 54;
            NPC.height = 42;
            NPC.defense = 25;
            NPC.lifeMax = 1000;
            NPC.aiStyle = AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.value = Item.buyPrice(0, 0, 3, 50);
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath5;
            NPC.knockBackResist = 0f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<BelchingCoralBanner>();
        }
        public override void AI()
        {
            NPC.velocity.Y += 0.25f;
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (Math.Abs(player.Center.X - NPC.Center.X) < CheckDistance && player.Bottom.Y < NPC.Top.Y)
            {
                if (NPC.ai[0]++ % 35f == 34f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 velocity = new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-11f, -6f));
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Top + new Vector2(0f, 6f), velocity, ModContent.ProjectileType<BelchingCoralSpike>(), 27, 3f);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !spawnInfo.Player.Calamity().ZoneSulphur || !CalamityWorld.downedAquaticScourge)
            {
                return 0f;
            }
            return 0.085f;
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<CorrodedFossil>(), 15); // Rarer to encourage fighting Acid Rain
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<BelchingSaxophone>(), 10);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hit.HitDirection, -1f, 0, default, 1f);
                }

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BelchingCoralGore").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BelchingCoralGore2").Type, NPC.scale);
                }
            }
        }
    }
}
