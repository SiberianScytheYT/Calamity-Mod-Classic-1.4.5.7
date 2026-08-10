using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
    public class SnowRuffianChestplate : ModItem
    {
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            EquipLoader.AddEquipTexture(Mod, Texture + "_Back", EquipType.Back, this);
            EquipLoader.AddEquipTexture(Mod, Texture + "_Neck", EquipType.Neck, this);
        }

        public override void EquipFrameEffects(Player player, EquipType type)
        { 
            if (player.body == Item.bodySlot)
            {
                player.back = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Back);
                player.neck = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Neck);
            }
        }

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Snow Ruffian Chestplate");
/*
            Tooltip.SetDefault("3% increased rogue critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 75, 0);
            Item.rare = 1;
            Item.defense = 2; //4
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingCrit += 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnySnowBlock", 30);
            recipe.AddRecipeGroup("AnyIceBlock", 15);
            recipe.AddIngredient(ItemID.BorealWood, 45);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
