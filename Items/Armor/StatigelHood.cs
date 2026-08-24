using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
    public class StatigelHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statigel Hood");
/*
            Tooltip.SetDefault("Increased minion knockback and +1 max minion");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = 4;
            Item.defense = 4; //20
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StatigelArmor>() && legs.type == ModContent.ItemType<StatigelGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "18% increased minion damage\n" +
                "Summons a mini slime god to fight for you, the type depends on what world evil you have\n" +
                "When you take over 100 damage in one hit you become immune to damage for an extended period of time\n" +
                "Grants an extra jump and increased jump height\n" +
				"30% increased jump speed";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.statigelSet = true;
            modPlayer.slimeGod = true;
			modPlayer.statigelJump = true;
			Player.jumpHeight += 5;
			player.jumpSpeedBoost += 1.5f;
            player.GetDamage(DamageClass.Summon) += 0.18f;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<StatigelSummonSetBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<StatigelSummonSetBuff>(), 3600, true);
                }
                if (WorldGen.crimson && player.ownedProjectileCounts[ModContent.ProjectileType<CrimsonSlimeGodMinion>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<CrimsonSlimeGodMinion>(), (int)(33f * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Summon).Additive - 1f)), 0f, Main.myPlayer, 0f, 0f);
                }
                else if (!WorldGen.crimson && player.ownedProjectileCounts[ModContent.ProjectileType<CorruptionSlimeGodMinion>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<CorruptionSlimeGodMinion>(), (int)(33f * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Summon).Additive - 1f)), 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetKnockback(DamageClass.Summon) += 1.5f;
            player.maxMinions++;
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
