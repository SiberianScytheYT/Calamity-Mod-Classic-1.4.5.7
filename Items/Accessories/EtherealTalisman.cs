using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class EtherealTalisman : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ethereal Talisman");
/*
            Tooltip.SetDefault("15% increased magic damage, 5% increased magic critical strike chance, and 10% decreased mana usage\n" +
                "+150 max mana and reveals treasure locations if visibility is on\n" +
                "Increases pickup range for mana stars and you restore mana when damaged\n" +
                "You automatically use mana potions when needed if visibility is on");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eTalisman = true;
            if (!hideVisual)
            {
                player.findTreasure = true;
                player.manaFlower = true;
            }
            player.magicCuffs = true;
            player.manaMagnet = true;
            player.statManaMax2 += 150;
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.manaCost *= 0.9f;
            player.GetCritChance(DamageClass.Magic) += 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SigilofCalamitas>());
            recipe.AddIngredient(ItemID.ManaFlower);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
