using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Weapons.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SulfurHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphurous Helmet");
/*
            Tooltip.SetDefault("8% increased rogue damage\n" +
                "2% increased rogue critical strike chance\n" +
                "Grants underwater breathing");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 2;
            Item.defense = 5; 
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SulfurBreastplate>() && legs.type == ModContent.ItemType<SulfurLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.SulfurHelmet");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.sulfurSet = true;
            modPlayer.sulfurJump = true;
            modPlayer.rogueStealthMax += 1f;
            modPlayer.wearingRogueArmor = true;
            player.ignoreWater = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingDamage += 0.08f;
            player.Calamity().throwingCrit += 2;
            player.gills = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 10);
            recipe.AddIngredient(ModContent.ItemType<UrchinStinger>(), 15);
            recipe.AddIngredient(ModContent.ItemType<SulphurousSand>(), 10);
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 10);

            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
