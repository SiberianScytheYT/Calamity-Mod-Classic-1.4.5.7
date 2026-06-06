using CalRD.NPCs.Crabulon;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.SummonItems
{
    public class DecapoditaSprout : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Decapodita Sprout");
/*
            Tooltip.SetDefault("Summons Crabulon");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.rare = 2;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneGlowshroom && !NPC.AnyNPCs(ModContent.NPCType<CrabulonIdle>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(Entity.GetSource_FromThis(), (int)(player.position.X + Main.rand.Next(-50, 51)), (int)(player.position.Y - 50f), ModContent.NPCType<CrabulonIdle>(), 1);
				Main.npc[npc].timeLeft *= 20;
				CalamityUtils.BossAwakenMessage(npc);
            }
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<CrabulonIdle>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GlowingMushroom, 25);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
