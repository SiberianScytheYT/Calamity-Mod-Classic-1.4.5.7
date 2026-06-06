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
    public class ReaverHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Helmet");
/*
            Tooltip.SetDefault("5% increased minion damage, +2 max minions, and increased minion knockback\n" +
                "10% increased movement speed and can move freely through liquids");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 7;
            Item.defense = 3; //36
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ReaverScaleMail>() && legs.type == ModContent.ItemType<ReaverCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "16% increased minion damage\n" +
                "Summons a reaver orb that emits spore gas when enemies are near";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.reaverOrb = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<ReaverSummonSetBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<ReaverSummonSetBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<ReaverOrb>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<ReaverOrb>(), (int)(80f * player.MinionDamage()), 0f, Main.myPlayer, 50f, 0f);
                }
            }
            player.GetDamage(DamageClass.Summon) += 0.16f;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage(DamageClass.Summon) += 0.05f;
            player.GetKnockback(DamageClass.Summon).Base += 1f;
            player.maxMinions += 2;
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 8);
			recipe.AddIngredient(ItemID.JungleSpores, 6);
			recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>());
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
