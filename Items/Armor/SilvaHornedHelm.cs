using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHornedHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Silva Horned Helm");
/*
            Tooltip.SetDefault("13% increased ranged damage and critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 90, 0, 0);
            Item.defense = 36; //110
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
            modPlayer.silvaRanged = true;
            player.setBonus = "All projectiles spawn healing leaf orbs on enemy hits\n" +
                "Max run speed and acceleration boosted by 5%\n" +
                "If you are reduced to 1 HP you will not die from any further damage for 10 seconds\n" +
                "If you get reduced to 1 HP again while this effect is active you will lose 100 max life\n" +
                "This effect only triggers once per life and if you are reduced to 400 max life the invincibility effect will stop\n" +
                "Your max life will return to normal if you die\n" +
                "Increases your rate of fire with all ranged weapons\n" +
				"After the silva invulnerability time your ranged weapons will do 10% more damage";
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.13f;
            player.GetCritChance(DamageClass.Ranged) += 13;
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
