using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class ReaverScaleMail : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Scale Mail");
/*
            Tooltip.SetDefault("9% increased damage and 4% increased critical strike chance\n" +
                "+20 max life");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = 7;
            Item.defense = 19;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 20;
            player.GetDamage(DamageClass.Generic) += 0.09f;
            player.Calamity().AllCritBoost(4);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 15);
			recipe.AddIngredient(ItemID.JungleSpores, 12);
			recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 3);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
