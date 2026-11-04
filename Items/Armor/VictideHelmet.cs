using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VictideHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Victide Helmet");
/*
            Tooltip.SetDefault("9% increased minion damage\n" +
                "+1 max minion");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.rare = 2;
            Item.defense = 1; //8
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VictideBreastplate>() && legs.type == ModContent.ItemType<VictideLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.VictideHelmet");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.victideSet = true;
            modPlayer.urchin = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<VictideSummonSetBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<VictideSummonSetBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Urchin>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<Urchin>(), (int)(7f * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Summon).Additive - 1f)), 0f, Main.myPlayer, 0f, 0f);
                }
            }
            player.ignoreWater = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.GetDamage(DamageClass.Summon) += 0.1f;
                player.lifeRegen += 3;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.09f;
            player.maxMinions++;
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
