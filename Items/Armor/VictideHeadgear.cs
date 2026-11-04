using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VictideHeadgear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Victide Headgear");
/*
            Tooltip.SetDefault("5% increased rogue damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.rare = 2;
            Item.defense = 3; //10
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VictideBreastplate>() && legs.type == ModContent.ItemType<VictideLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.VictideHeadgear");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.victideSet = true;
            modPlayer.rogueStealthMax += 0.9f;
            modPlayer.wearingRogueArmor = true;
            player.ignoreWater = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.Calamity().throwingDamage += 0.1f;
                player.lifeRegen += 3;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingDamage += 0.05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
