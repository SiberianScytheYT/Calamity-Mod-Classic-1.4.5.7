using CalRD.NPCs.Ravager;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.SummonItems
{
    public class AncientMedallion : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Death Whistle");
/*
            Tooltip.SetDefault("A very old temple whistle\n" +
                "Summons the Ravager");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = 8;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<RavagerBody>()) && player.ZoneOverworldHeight;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			SoundEngine.PlaySound(SoundID.ScaryScream, player.position);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(Entity.GetSource_FromThis(),(int)(player.position.X + Main.rand.Next(-100, 101)), (int)(player.position.Y - 350f), ModContent.NPCType<RavagerBody>(), 1);
				Main.npc[npc].timeLeft *= 20;
				CalamityUtils.BossAwakenMessage(npc);
            }
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<RavagerBody>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarTabletFragment, 5);
            recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
