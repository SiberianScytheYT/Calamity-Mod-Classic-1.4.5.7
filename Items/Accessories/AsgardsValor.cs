using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class AsgardsValor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asgard's Valor");
/*
            Tooltip.SetDefault("Grants immunity to fire blocks and knockback\n" +
				"Immune to most debuffs and reduces the damage caused by the Brimstone Flames debuff\n" +
                "10% damage reduction while submerged in liquid\n" +
                "+20 max life\n" +
                "Grants a holy dash which can be used to ram enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 44;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.defense = 8;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.dashMod = 2;
            player.noKnockback = true;
            player.fireWalk = true;
			modPlayer.abaddon = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.statLifeMax2 += 20;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            { player.endurance += 0.1f; }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AnkhShield);
            recipe.AddIngredient(ModContent.ItemType<OrnateShield>());
            recipe.AddIngredient(ModContent.ItemType<ShieldoftheOcean>());
            recipe.AddIngredient(ModContent.ItemType<Abaddon>());
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>());
            recipe.AddIngredient(ItemID.LifeFruit, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
