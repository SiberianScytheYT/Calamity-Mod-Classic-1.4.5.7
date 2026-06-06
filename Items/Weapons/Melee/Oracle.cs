using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Yoyos;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Oracle : ModItem
    {
        public const int YoyoBaseDamage = 380;
		public const int AuraBaseDamage = 120;
		public const int AuraMaxDamage = 400;
		
		public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Oracle");
/*
            Tooltip.SetDefault("Gaze into the past, the present, the future... and the circumstances of your inevitable demise\n" +
			"Emits an aura of red lightning which charges up when hitting enemies\n" +
			"Fires auric orbs when supercharged\n" +
			"An exceptionally agile yoyo\n");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 42;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = YoyoBaseDamage;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<OracleYoyo>();
            Item.shootSpeed = 16f;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(platinum: 2, gold: 50);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.AddIngredient(ModContent.ItemType<TheObliterator>());
            r.AddIngredient(ModContent.ItemType<Lacerator>());
            r.AddIngredient(ModContent.ItemType<Verdant>());
            r.AddIngredient(ModContent.ItemType<Chaotrix>());
            r.AddIngredient(ModContent.ItemType<Quagmire>());
            r.AddIngredient(ModContent.ItemType<Shimmerspark>());
			r.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			r.Register();
        }
    }
}
