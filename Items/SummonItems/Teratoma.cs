using CalRD.Items.Materials;
using CalRD.NPCs.HiveMind;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class Teratoma : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Teratoma");
/*
            Tooltip.SetDefault("Summons the Hive Mind");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.rare = 3;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneCorrupt && !NPC.AnyNPCs(ModContent.NPCType<HiveMind>()) && !NPC.AnyNPCs(ModContent.NPCType<HiveMindP2>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(Entity.GetSource_FromThis(), (int)(player.position.X + Main.rand.Next(-100, 100)), (int)(player.position.Y - 150f), ModContent.NPCType<HiveMind>(), 1);
				Main.npc[npc].timeLeft *= 20;
				CalamityUtils.BossAwakenMessage(npc);
            }
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<HiveMind>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RottenChunk, 9);
            recipe.AddIngredient(ModContent.ItemType<TrueShadowScale>(), 5);
            recipe.AddIngredient(ItemID.DemoniteBar, 2);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
