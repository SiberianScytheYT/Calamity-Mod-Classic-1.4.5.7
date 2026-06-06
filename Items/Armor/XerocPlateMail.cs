using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class XerocPlateMail : ModItem
    {
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Armor/XerocPlateMail_Neck", EquipType.Neck, this);
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Armor/XerocPlateMail_Back", EquipType.Back, this);
            }
        }
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Empyrean Cloak");
/*
            Tooltip.SetDefault("Armor of the cosmos\n" +
				"+20 max life\n" +
                "6% increased movement speed\n" +
                "7% increased rogue damage and critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 32, 0, 0);
            Item.rare = 10;
            Item.defense = 27;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 20;
            player.moveSpeed += 0.06f;
            player.Calamity().throwingCrit += 7;
            player.Calamity().throwingDamage += 0.07f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeldiateBar>(), 22);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
