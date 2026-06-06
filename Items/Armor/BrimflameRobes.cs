using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class BrimflameRobes : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimflame Robes");
/*
            Tooltip.SetDefault("5% increased magic damage and critical strike chance\n" +
                "Grants obsidian rose effects");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.defense = 10; //41
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.lavaRose = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>(), 20);
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 4);
            recipe.AddIngredient(ItemID.ObsidianRose);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
