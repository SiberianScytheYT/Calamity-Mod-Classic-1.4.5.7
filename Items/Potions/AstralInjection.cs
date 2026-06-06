using CalRD.Buffs.Potions;
using CalRD.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Potions
{
    public class AstralInjection : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Injection");
/*
            Tooltip.SetDefault("Gives mana sickness and hurts you when used, but you regenerate mana extremely quickly even while moving or casting spells");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 999;
            Item.rare = 3;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<AstralInjectionBuff>();
            Item.buffTime = 180;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffID.ManaSickness, Player.manaSickTime / 2, true);
            player.statLife -= 5;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(-5, true);
            }
            if (player.statLife <= 0)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + "'s blood vessels burst from drug overdose."), 1000.0, 0, false);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ItemID.BottledWater, 4);
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 4);
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            recipe = CreateRecipe(8);
            recipe.AddIngredient(ItemID.BottledWater, 4);
            recipe.AddIngredient(ModContent.ItemType<BloodOrb>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
