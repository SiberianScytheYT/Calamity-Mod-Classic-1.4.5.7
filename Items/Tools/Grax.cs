using CalRD.Buffs.StatBuffs;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Tools
{
    public class Grax : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Grax");
/*
            Tooltip.SetDefault("Hitting an enemy will greatly boost your defense and melee stats for a short time");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
			Item.scale = 1.5f;
            Item.damage = 500;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 4;
            Item.useTurn = true;
            Item.axe = 50;
            Item.hammer = 200;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.tileBoost += 5;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<InfernaCutter>());
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 5);
            recipe.AddRecipeGroup("LunarHamaxe");
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.AddBuff(ModContent.BuffType<GraxDefense>(), 600);
        }
    }
}
