using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class AerospecBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aerospec Breastplate");
/*
            Tooltip.SetDefault("3% increased critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 3;
            player.GetCritChance(DamageClass.Ranged) += 3;
            player.GetCritChance(DamageClass.Magic) += 3;
            player.Calamity().throwingCrit += 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 11);
            recipe.AddIngredient(ItemID.Cloud, 10);
            recipe.AddIngredient(ItemID.RainCloud, 5);
            recipe.AddIngredient(ItemID.Feather, 2);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
