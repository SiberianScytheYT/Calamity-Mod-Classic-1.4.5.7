using CalRD.Items.Materials;
using CalRD.NPCs.Yharon;
using CalRD.World;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class ChickenEgg : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragon Egg");
/*
            Tooltip.SetDefault("Summons the loyal guardian of the tyrant king\n" +
                               "It yearns for the jungle\n" +
                               "Not consumable");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneJungle && !NPC.AnyNPCs(ModContent.NPCType<Yharon>()) && CalamityWorld.downedBossAny;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Yharon>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Yharon>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 15);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
