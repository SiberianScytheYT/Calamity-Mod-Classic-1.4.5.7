using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
    public class StatigelHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statigel Helm");
/*
            Tooltip.SetDefault("10% increased melee damage and melee speed\n" +
                "7% increased melee critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = 4;
            Item.defense = 9; //27
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StatigelArmor>() && legs.type == ModContent.ItemType<StatigelGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When you take over 100 damage in one hit you become immune to damage for an extended period of time\n" +
                "Grants an extra jump and increased jump height\n" +
					"30% increased jump speed";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.statigelSet = true;
			modPlayer.statigelJump = true;
			Player.jumpHeight += 5;
			player.jumpSpeedBoost += 1.5f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetCritChance(DamageClass.Melee) += 7;
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
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
