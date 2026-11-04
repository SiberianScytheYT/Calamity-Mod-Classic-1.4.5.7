using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaMaskedCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Silva Masked Cap");
/*
            Tooltip.SetDefault("13% increased magic damage and critical strike chance\n" +
                "+100 max mana and 19% reduced mana usage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 90, 0, 0);
            Item.defense = 21; //110
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SilvaArmor>() && legs.type == ModContent.ItemType<SilvaLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.silvaSet = true;
            modPlayer.silvaMage = true;
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.SilvaMaskedCap");
        }

        public override void UpdateEquip(Player player)
        {
            player.manaCost *= 0.81f;
            player.GetDamage(DamageClass.Magic) += 0.13f;
            player.GetCritChance(DamageClass.Magic) += 13;
            player.statManaMax2 += 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 5);
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 5);
            recipe.AddRecipeGroup("AnyGoldBar", 5);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 6);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
