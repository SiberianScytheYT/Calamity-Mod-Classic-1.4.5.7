using CalRD.Items.Materials;
using CalRD.NPCs.Cryogen;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class CryoKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryo Key");
/*
            Tooltip.SetDefault("Summons Cryogen\n" +
				"The boss enrages outside of the snowy tundra");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.rare = 5;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneSnow && !NPC.AnyNPCs(ModContent.NPCType<Cryogen>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Cryogen>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Cryogen>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyIceBlock", 50);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
