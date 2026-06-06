using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class DragonPow : ModItem
    {
        public static float Speed = 13f;
        public static float ReturnSpeed = 20f;
        public static float SparkSpeed = 0.6f;
        public static float MinPetalSpeed = 24f;
        public static float MaxPetalSpeed = 30f;
        public static float MinWaterfallSpeed = 12f;
        public static float MaxWaterfallSpeed = 15.5f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragon Pow");
/*
            Tooltip.SetDefault("Fires a dragon head that releases draconic sparks\n"
                               +"Summons a barrage of petals and waterfalls on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 82;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 4500;
            Item.knockBack = 9f;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Custom/YharonRoarShort");
            Item.channel = true;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(2, 50, 0, 0);

            Item.shoot = ModContent.ProjectileType<DragonPowFlail>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.AddIngredient(ModContent.ItemType<Mourningstar>());
            r.AddIngredient(ItemID.DaoofPow);
            r.AddIngredient(ItemID.FlowerPow);
            r.AddIngredient(ItemID.Flairon);
            r.AddIngredient(ModContent.ItemType<BallOFugu>());
            r.AddIngredient(ModContent.ItemType<Tumbleweed>());
            r.AddIngredient(ModContent.ItemType<UrchinFlail>());
			r.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			r.AddIngredient(ModContent.ItemType<HellcasterFragment>(), 4);
			r.Register();
        }
    }
}
