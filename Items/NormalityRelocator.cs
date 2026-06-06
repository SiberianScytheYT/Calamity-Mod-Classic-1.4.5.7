using CalRD.CalPlayer;
using CalRD.Items.Placeables.Plates;
using CalRD.Items.Placeables.Ores;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items
{
    public class NormalityRelocator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Normality Relocator");
/*
            Tooltip.SetDefault("I'll be there in the blink of an eye\n" +
                "This line is modified below\n" +
				"Teleportation is disabled while Chaos State is active\n" +
                "Boosts movement and fall speed by 10%\n" +
                "Works while in the inventory");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 7));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.NormalityRelocatorHotKey.TooltipHotkeyString();
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
                {
                    line2.Text = "Press " + hotkey + " to teleport to the position of the mouse";
                }
            }
        }


        public override void UpdateInventory(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.normalityRelocator = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RodofDiscord);
            recipe.AddIngredient(ItemID.FragmentStardust, 30);
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Cinderplate>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
