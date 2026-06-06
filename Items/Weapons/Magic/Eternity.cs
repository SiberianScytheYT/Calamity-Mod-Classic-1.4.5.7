using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Eternity : ModItem
    {
        public const int BaseDamage = 4200;
        public const int ExplosionDamage = 42000;
        public const int MaxHomers = 40;
        public const int DustID = 16;
        public static readonly Color BlueColor = new Color(34, 34, 160);
        public static readonly Color PinkColor = new Color(169, 30, 184);
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eternity");
/*
            Tooltip.SetDefault("Hexes a possible nearby enemy, trapping them in a brilliant display of destruction\n" +
                               "This line is modified in ModifyTooltips");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 30;
            Item.width = 38;
            Item.height = 40;
            Item.useTime = Item.useAnimation = 120;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<EternityBook>();
            Item.channel = true;
            Item.shootSpeed = 0f;
            Item.rare = 10;
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var tt2 = tooltips.FirstOrDefault(x => x.Name == "Tooltip1" && x.Mod == "Terraria");
            tt2.Text = $"[" + DisoHex + "There's pictures of ponies in the book]";
        }
        public static string DisoHex => "c/" +
            ((int)(156 + Main.DiscoR * 99f / 255f)).ToString("X2") 
            + 108.ToString("X2") + 251.ToString("X2") + ":";
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeethingDischarge>());
            recipe.AddIngredient(ModContent.ItemType<SlitheringEels>());
            recipe.AddIngredient(ModContent.ItemType<GammaFusillade>());
            recipe.AddIngredient(ModContent.ItemType<PrimordialAncient>());
            recipe.AddIngredient(ModContent.ItemType<SubsumingVortex>());
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>(), 20);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 10);
            recipe.AddIngredient(ItemID.UnicornHorn, 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}