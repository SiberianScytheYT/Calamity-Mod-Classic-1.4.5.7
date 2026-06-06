using CalRD.NPCs.GreatSandShark;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.SummonItems
{
    public class SandstormsCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sandstorm's Core");
/*
            Tooltip.SetDefault("Summons the Great Sand Shark");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = 7;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneDesert && !NPC.AnyNPCs(ModContent.NPCType<GreatSandShark>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<GreatSandShark>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<GreatSandShark>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 2);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
