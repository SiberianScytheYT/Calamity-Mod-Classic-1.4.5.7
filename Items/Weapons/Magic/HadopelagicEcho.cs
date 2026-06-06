using CalRD.Projectiles.Magic;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Weapons.Magic
{
    public class HadopelagicEcho : ModItem
    {
		private int counter = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hadopelagic Echo");
/*
            Tooltip.SetDefault("Fires a string of bouncing sound waves\n" +
			"Sound waves fired later in the chain deal more damage\n" +
			"Sound waves echo additional sound waves on enemy hits\n" +
			"Sound waves deal more damage the more they pierce");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 769;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 8;
            Item.reuseDelay = 20;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<HadopelagicEchoSoundwave>();
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float damageMult = 1f;
			if (counter == 1)
				damageMult = 1.1f;
			if (counter == 2)
				damageMult = 1.2f;
			if (counter == 3)
				damageMult = 1.35f;
			if (counter == 4)
				damageMult = 1.5f;
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * damageMult), Item.knockBack, player.whoAmI, (float)counter, 0f);
			counter++;
			if (counter >= 5)
                counter = 0;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EidolicWail>());
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 20);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 20);
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
