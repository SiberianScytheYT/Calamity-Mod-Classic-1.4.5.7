using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.Potions;

namespace CalRD.Items.Potions
{
    public class Baguette : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Baguette");
/*
            Tooltip.SetDefault("Minor improvements to all stats\n" +
			"Boosts the effects of Red Wine\n" +
			"[c/FCE391:je suis Monte]");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.rare = 1;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.buffType = ModContent.BuffType<BaguetteBuff>();
            Item.buffTime = CalamityUtils.SecondsToFrames(300f);
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override void OnConsumeItem(Player player)
        {
			//5 minutes for both
            player.AddBuff(ModContent.BuffType<BaguetteBuff>(), CalamityUtils.SecondsToFrames(300f));
            player.AddBuff(BuffID.WellFed, CalamityUtils.SecondsToFrames(300f));
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Hay, 10);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}
