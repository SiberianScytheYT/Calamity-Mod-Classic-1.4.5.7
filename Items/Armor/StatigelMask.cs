using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
    public class StatigelMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statigel Mask");
/*
            Tooltip.SetDefault("10% increased rogue damage and 34% chance to not consume rogue items\n" +
                "7% increased rogue critical strike chance and 12% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = 4;
            Item.defense = 6; //23
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StatigelArmor>() && legs.type == ModContent.ItemType<StatigelGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.StatigelMask");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.statigelSet = true;
			modPlayer.statigelJump = true;
            modPlayer.rogueStealthMax += 1f;
            modPlayer.wearingRogueArmor = true;
			Player.jumpHeight += 5;
			player.jumpSpeedBoost += 1.5f;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingAmmoCost *= 0.66f;
            player.Calamity().throwingDamage += 0.1f;
            player.Calamity().throwingCrit += 7;
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 5);
            recipe.AddIngredient(ItemID.HellstoneBar, 9);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }
    }
}
