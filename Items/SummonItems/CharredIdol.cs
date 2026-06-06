using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.NPCs.BrimstoneElemental;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class CharredIdol : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Charred Idol");
/*
            Tooltip.SetDefault("Use in the Brimstone Crag at your own risk\n" +
               "Summons the Brimstone Elemental\n" +
			   "The boss enrages outside of her home in the crags");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.rare = 6;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            return modPlayer.ZoneCalamity && !NPC.AnyNPCs(ModContent.NPCType<BrimstoneElemental>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<BrimstoneElemental>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<BrimstoneElemental>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
