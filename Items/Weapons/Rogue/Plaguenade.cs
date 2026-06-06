using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Plaguenade : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plaguenade");
/*
            Tooltip.SetDefault("Releases a swarm of angry plague bees\n" +
			"Stealth strikes spawn more bees and generate a larger explosion");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 20;
            Item.damage = 60;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1.5f;
            Item.maxStack = 999;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<PlaguenadeProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[proj].Calamity().stealthStrike = true;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(30);
            recipe.AddIngredient(ItemID.Beenade, 15);
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 3);
            recipe.AddIngredient(ItemID.Obsidian, 2);
            recipe.AddIngredient(ItemID.Stinger);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
