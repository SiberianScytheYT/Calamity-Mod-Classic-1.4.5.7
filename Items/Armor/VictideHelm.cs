using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VictideHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Victide Helm");
/*
            Tooltip.SetDefault("5% increased melee damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.rare = 2;
            Item.defense = 4; //11
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VictideBreastplate>() && legs.type == ModContent.ItemType<VictideLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increased life regen and melee damage while submerged in liquid\n" +
                    "When using any weapon you have a 10% chance to throw a returning seashell projectile\n" +
                    "This seashell does true damage and does not benefit from any damage class\n" +
                    "Provides increased underwater mobility and slightly reduces breath loss in the abyss";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.victideSet = true;
            player.ignoreWater = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.GetDamage(DamageClass.Melee) += 0.1f;
                player.lifeRegen += 3;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.05f;
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
