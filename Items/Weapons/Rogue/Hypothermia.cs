using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class Hypothermia : RogueWeapon
    {
		private int counter = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hypothermia");
/*
            Tooltip.SetDefault("Fires a constant barrage of black ice shards\n" +
                               "Stealth strikes additionally fire a short ranged ice chunk that shatters into ice shards");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 46;
            Item.height = 32;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item9;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;

            Item.damage = 369;
            Item.useAnimation = 21;
            Item.useTime = 3;
            Item.reuseDelay = 1;
            Item.crit = 16;
            Item.knockBack = 3f;
            Item.shoot = ModContent.ProjectileType<HypothermiaShard>();
            Item.shootSpeed = 8f;

            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable() && counter == 0) //setting up the stealth strikes
			{
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<HypothermiaChunk>(), damage, Item.knockBack, player.whoAmI);
                Main.projectile[stealth].Calamity().stealthStrike = true;
            }
			int projAmt = Main.rand.Next(1, 3);
			for (int index = 0; index < projAmt; ++index)
			{
				float SpeedX = velocity.X + Main.rand.NextFloat(-2f, 2f);
				float SpeedY = velocity.Y + Main.rand.NextFloat(-2f, 2f);
				Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, Main.rand.Next(4), 0f);
			}

			counter++;
			if (counter >= 7)
				counter = 0;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 6);
            recipe.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 24);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 6);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
